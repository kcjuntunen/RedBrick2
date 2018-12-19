using System;
using System.Windows.Forms;

namespace RedBrick2 {
	public partial class CncTodo : Form {
		private bool Initialated = false;

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
			comboBox1.SelectedValue = Properties.Settings.Default.CNCTodoLastWC;
			try {
				QueryTab1();
			} catch (Exception ex) {
				Redbrick.ProcessError(ex);
			}
			WindowState = FormWindowState.Minimized;
			Show();
			WindowState = FormWindowState.Normal;
			Initialated = true;
		}

		private void QueryTab1() {
			listView1.Items.Clear();
			using (CNCTodo.CNCTodoDataSetTableAdapters.CUT_CNC_JOBS_VIEWTableAdapter ta_ =
				new CNCTodo.CNCTodoDataSetTableAdapters.CUT_CNC_JOBS_VIEWTableAdapter()) {
				ta_.FillGroupedByWC(cNCTodoDataSet.CUT_CNC_JOBS_VIEW, comboBox1.SelectedValue.ToString().Trim());
				foreach (CNCTodo.CNCTodoDataSet.CUT_CNC_JOBS_VIEWRow row in cNCTodoDataSet.CUT_CNC_JOBS_VIEW) {
					if (!showIgn_chb.Checked && row.IgnChk == -1) {
						continue;
					}
					string lastprn = row.LastPrn == "NOT PRINTED" ? "Not Printed" : Convert.ToDateTime(row.LastPrn).ToString(@"yyyy-MM-dd H:m tt");
					string[] data = new string[] {
						row.OpDue.ToString(@"yyyy-MM-dd"),	// 0
						row.JobNumber,											// 1
						row.JobQty.ToString(),							// 2
						Redbrick.TitleCase(row.JobStatus),	// 3
						row.PartNumber,											// 4
						row.PartRev,												// 5
						Redbrick.TitleCase(row.CutSt),			// 6
						lastprn,														// 7
						row.IgnChk == -1 ? "Yes" : "No",		// 8
						row.IsCutIDNull() ? string.Empty : row.CutID.ToString(),	// 9
						row.IsDESCRNull() ? string.Empty : row.DESCR,							// 10	
						row.JinfChk == -1 ? "Y" : "N"															// 11
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
			if (dataGridView1.SelectedCells.Count < 1) {
				return;
			}
			listView2.Items.Clear();
			int idx = dataGridView1.SelectedCells[0].RowIndex;
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
	}
}
