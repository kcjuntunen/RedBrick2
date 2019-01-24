using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace RedBrick2 {
	public partial class CncTodo : Form {
		private bool Initialated = false;
		private string selectedMetalAlertPath = string.Empty;
		private string selectedMetalAlertPDFPath = string.Empty;
		private string selectedMetalAlertSWPath = string.Empty;
		private string metalPath = @"S:\shared\general\Metals\METAL MANUFACTURING\";
		private	List<int> stringIdxs = new List<int>() { 2, 3 };
		private ToolTip ttSW = new ToolTip();

		public CncTodo() {
			InitializeComponent();
			InitListView();
		}
		
		private void InitListView() {
			listView1.FullRowSelect = true;
			listView1.HideSelection = false;
			listView1.MultiSelect = false;
			listView1.View = View.Details;
			listView1.SmallImageList = Redbrick.TreeViewIcons;

			listView2.FullRowSelect = true;
			listView2.HideSelection = false;
			listView2.MultiSelect = false;
			listView2.View = View.Details;
			listView2.SmallImageList = Redbrick.TreeViewIcons;

			aff_prts_lv.FullRowSelect = true;
			aff_prts_lv.HideSelection = false;
			aff_prts_lv.MultiSelect = false;
			aff_prts_lv.View = View.Details;
			aff_prts_lv.SmallImageList = Redbrick.TreeViewIcons;
		}

		private void CncTodo_Load(object sender, EventArgs e) {
			this.cUT_CNC_MAINTableAdapter.Fill(this.cNCTodoDataSet.CUT_CNC_MAIN);
			this.cUT_CNC_JOBS_VIEW1TableAdapter.Fill(this.cNCTodoDataSet.CUT_CNC_JOBS_VIEW1);
			this.metalAlertTableAdapter.FillBy(this.cNCTodoDataSet.MetalAlert);
			comboBox1.SelectedValue = Properties.Settings.Default.CNCTodoLastWC;
			Location = Properties.Settings.Default.CNCTodoLocation; 
			Size = Properties.Settings.Default.CNCTodoSize;
			try {
				QueryTab1();
			} catch (Exception ex) {
				Redbrick.ProcessError(ex);
			}
			WindowState = FormWindowState.Minimized;
			Show();
			WindowState = FormWindowState.Normal;
			Filter();
			Initialated = true;
		}

		private void QueryTab1() {
			listView1.Items.Clear();
			using (CNCTodo.CNCTodoDataSetTableAdapters.CUT_CNC_CLISSTableAdapter ta_ =
				new CNCTodo.CNCTodoDataSetTableAdapters.CUT_CNC_CLISSTableAdapter()) {
				ta_.FillByWC(cNCTodoDataSet.CUT_CNC_CLISS, comboBox1.SelectedValue.ToString().Trim());
				foreach (CNCTodo.CNCTodoDataSet.CUT_CNC_CLISSRow row in cNCTodoDataSet.CUT_CNC_CLISS) {
					if (!showIgn_chb.Checked && row.IgnChk != 0) {
						continue;
					}
					string lastprn = row.LastPrn == "NOT PRINTED" ? "Not Printed" : Convert.ToDateTime(row.LastPrn).ToString(@"yyyy-MM-dd");
					string[] data = new string[] {
					// If the comment numbers on the right side of the following don't line up,
					// then we have an argument for spaces over tabs.
						row.OpDue.ToString(@"yyyy-MM-dd"),	// 0
						row.JobNumber,											// 1
						row.JobQty.ToString(),							// 2
						Redbrick.TitleCase(row.JobStatus),	// 3
						row.PartNumber,											// 4
						row.PartRev,												// 5
						Redbrick.TitleCase(row.IssChk),			// 6
						lastprn,														// 7
						row.IgnChk == 0 ? "No" : "Yes",			// 8
						row.IsCutIDNull() ? string.Empty : row.CutID.ToString(),	// 9
						row.CutDesc,												// 10
						row.Linked == 0 ? "N" : "Y",				// 11
						row.OpNumber.ToString()							// 12
					};
					ListViewItem l = new ListViewItem(data);
					l.Checked = row.IgnChk == -1;
					listView1.Items.Add(l);
				}
			}
			listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
		}

		private void QueryCutlist() {
			aff_prts_lv.Items.Clear();
			if (listView1.SelectedItems.Count < 1) {
				return;
			}

			ListViewItem lvi = listView1.SelectedItems[0];
			string clid = lvi.SubItems[9].Text.Trim();

			if (clid == string.Empty) {
				cutl_tb.Text = "No Cutlist";
				rev_tb.Text = @"N\A";
				stat_tb.Text = "No Cutlist";
				descr_tb.Text = string.Empty;
				linked_chb.Checked = false;
				aff_prts_lv.Items.Clear();
				return;
			}

			cutl_tb.Text = lvi.SubItems[4].Text;
			rev_tb.Text = lvi.SubItems[5].Text;
			descr_tb.Text = lvi.SubItems[10].Text;
			stat_tb.Text = lvi.SubItems[6].Text;
			linked_chb.Checked = lvi.SubItems[11].Text.Contains("Y");

			if (int.TryParse(clid, out int res)) {
				using (CNCTodo.CNCTodoDataSetTableAdapters.CUT_PARTSTableAdapter ta =
					new CNCTodo.CNCTodoDataSetTableAdapters.CUT_PARTSTableAdapter()) {
					ta.FillByClID(cNCTodoDataSet.CUT_PARTS, res);
					foreach (CNCTodo.CNCTodoDataSet.CUT_PARTSRow row in cNCTodoDataSet.CUT_PARTS.Rows) {
						string issue = string.Empty;
						if (row.IsCNC1Null()) {
							issue = "Blank CNC1 field";
						}
						if (row.IsCNC2Null()) {
							issue = "Blank CNC2 field";
						}
						if (row.UPDATE_CNC) {
							issue = "Checked for update";
						}
						string[] data = new string[] {
							row.PART,
							issue
						};
						aff_prts_lv.Items.Add(new ListViewItem(data));
					}
				}
				aff_prts_lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			}
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e) {
			try {
				QueryTab1();
			} catch (Exception ex) {
				Redbrick.ProcessError(ex);
			}
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
			if (!Initialated) {
				return;
			}

			if ((sender as ComboBox).SelectedItem == null) {
				return;
			}

			Properties.Settings.Default.CNCTodoLastWC = (sender as ComboBox).SelectedValue.ToString().Trim();
			try {
				QueryTab1();
			} catch (Exception ex) {
				Redbrick.ProcessError(ex);
			}
		}

		private void dataGridView1_SelectionChanged(object sender, EventArgs e) {
			DataGridView dgv = sender as DataGridView;
			if (dgv.SelectedCells.Count < 1) {
				return;
			}
			listView2.Items.Clear();
			int idx = dgv.SelectedCells[0].RowIndex;
			DataGridViewCell c = (sender as DataGridView)[0, idx];
			opTableAdapter.FillBy(cNCTodoDataSet.Op, c.Value.ToString());
			foreach (CNCTodo.CNCTodoDataSet.OpRow row in cNCTodoDataSet.Op) {
				string[] data = new string[] {
					row.POPORDER.ToString(),
					string.Format ("{0} - {1}", row.OPNAME, Redbrick.TitleCase(row.OPDESCR).Replace("Cnc", "CNC"))
				};
				listView2.Items.Add(new ListViewItem(data));
			}
			listView2.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
		}

		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e) {
			dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
		}

		private void listView1_SelectedIndexChanged(object sender, EventArgs e) {
			try {
				QueryCutlist();
			} catch (Exception ex) {
				Redbrick.ProcessError(ex);
			}
		}

		private void CncTodo_FormClosing(object sender, FormClosingEventArgs e) {
			Properties.Settings.Default.CNCTodoLocation = Location;
			Properties.Settings.Default.CNCTodoSize = Size;
			Properties.Settings.Default.Save();
		}

		private void Filter() {
			StringBuilder sb = new StringBuilder();
			if (show_unch_chb.Checked) {
				sb.Append("ALERTCHK = 0");
			}
			metalAlertBindingSource.Filter = sb.ToString();
			dataGridView2.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
			if (dataGridView2.RowCount > dataGridView2.DisplayedRowCount(false)) {
				dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.RowCount - 1;
			}
		}

		private void show_unch_chb_CheckedChanged(object sender, EventArgs e) {
			Filter();
		}

		private void viewPDF_btn_Click(object sender, EventArgs e) {
			if (selectedMetalAlertPath == string.Empty) {
				return;
			}
			if ((new FileInfo(selectedMetalAlertPath)).Exists) {
				System.Diagnostics.Process.Start(selectedMetalAlertPath);
			} else {
				MessageBox.Show(@"Couldn't find " + selectedMetalAlertPath);
			}
		}

		private void dataGridView2_SelectionChanged(object sender, EventArgs e) {
			DataGridView dgv = sender as DataGridView;
			if (dgv.SelectedCells.Count < 1) {
				pathLabel.Text = "-";
				selectedMetalAlertPath = string.Empty;
				selectedMetalAlertPDFPath = string.Empty;
				selectedMetalAlertSWPath = string.Empty;
				ttSW.RemoveAll();
				return;
			}
			int idx = dgv.SelectedCells[0].RowIndex;
			string _path = dgv.Rows[idx].Cells[5].Value.ToString();
			string _file = dgv.Rows[idx].Cells[6].Value.ToString();
			selectedMetalAlertPath = string.Format(@"{0}{1}", _path, _file);
			pathLabel.Text = selectedMetalAlertPath;
			selectedMetalAlertPDFPath = selectedMetalAlertPath.Replace(@"K:\", metalPath);
			selectedMetalAlertSWPath = selectedMetalAlertPDFPath.ToUpper().Replace(@"PDF", @"SLDDRW");
			ttSW.SetToolTip(sw_btn, selectedMetalAlertSWPath);
		}

		private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) {
			if (stringIdxs.Contains(e.ColumnIndex)) {
				string v = e.Value.ToString().Trim();
				e.Value = v;
				e.FormattingApplied = true;
			}
		}

		private void sw_btn_Click(object sender, EventArgs e) {
			if (selectedMetalAlertSWPath == string.Empty) {
				return;
			}

			if ((new FileInfo(selectedMetalAlertSWPath)).Exists) {
				System.Diagnostics.Process.Start(selectedMetalAlertSWPath);
			} else {
				MessageBox.Show(@"Couldn't find " + selectedMetalAlertSWPath);
			}

		}

		private void chk_btn_Click(object sender, EventArgs e) {
			if (dataGridView2.SelectedCells.Count < 1) {
				return;
			}

			using (CNCTodo.CNCTodoDataSetTableAdapters.MetalAlertTableAdapter ta_ =
				new CNCTodo.CNCTodoDataSetTableAdapters.MetalAlertTableAdapter()) {
				foreach (DataGridViewCell cell in dataGridView2.SelectedCells) {
					int rowidx = cell.RowIndex;
					string alertID = dataGridView2[7, rowidx].Value.ToString();
					string checked_ = dataGridView2[4, rowidx].Value.ToString();
					if (!int.TryParse(alertID, out int id_)) {
						continue;
					}

					if (!bool.TryParse(checked_, out bool chk_)) {
						continue;
					}

					ta_.UpdateAlertChecked(!chk_, id_);
					dataGridView2[4, rowidx].Value = !chk_;
				}
			}
		}

		private void IgnoreOrNot(string job, string op) {
			if (job == string.Empty) {
				return;
			}

			if (op == string.Empty || !int.TryParse(op, out int op_)) {
				return;
			}

			using (CNCTodo.CNCTodoDataSetTableAdapters.QueriesTableAdapter ta_ =
				new CNCTodo.CNCTodoDataSetTableAdapters.QueriesTableAdapter()) {
				int? igID = (int?)ta_.GetIgnID(job, op_);
				if (igID != null) {
					ta_.Unignore(Convert.ToInt32(igID));
				} else {
					ta_.Ignore(Redbrick.UID, job, op_);
				}
			}
			QueryTab1();
		}

		private void ign_btn_Click(object sender, EventArgs e) {
			if (listView1.SelectedItems.Count < 1) {
				return;
			}

			ListViewItem lvi = listView1.SelectedItems[0];
			string job = lvi.SubItems[1].Text;
			string op = lvi.SubItems[12].Text;
			IgnoreOrNot(job, op);
		}
	}
}
