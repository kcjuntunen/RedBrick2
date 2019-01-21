using System;
using System.Windows.Forms;

namespace RedBrick2.CutlistTableDisplay {
	public partial class CutlistTableDisplayForm : Form {
		private const int PartIDColumn = 23;
		private const int ClIDColumn = 24;
		private const int ClPartIDColumn = 25;
		private string Item = string.Empty;
		private string Rev = string.Empty;
		private int ClID = 0;
		private int PartID = 0;
		private int ClPartID = 0;
		private string PartLookup = string.Empty;
		private bool initialated = false;
		private bool populated = false;

		public CutlistTableDisplayForm() {
			InitializeComponent();
			dataGridView1.ReadOnly = true;
			PopulateComboBox();
		}

		public CutlistTableDisplayForm(int clid) {
			InitializeComponent();
			dataGridView1.ReadOnly = true;
			ClID = clid;
			GetClInfo();
			Query();
		}

		public CutlistTableDisplayForm(string item, string rev) {
			InitializeComponent();
			dataGridView1.ReadOnly = true;
			Item = item;
			Rev = rev;
			Query();
		}

		private void GetClInfo() {
			using (CTDDataSetTableAdapters.CUT_CUTLISTSTableAdapter ta =
				new CTDDataSetTableAdapters.CUT_CUTLISTSTableAdapter()) {
				ta.Fill(cTDDataSet.CUT_CUTLISTS, ClID);
				if (cTDDataSet.CUT_CUTLISTS.Count > 0) {
					CTDDataSet.CUT_CUTLISTSRow r_ = (CTDDataSet.CUT_CUTLISTSRow)cTDDataSet.CUT_CUTLISTS.Rows[0];
					if (!r_.IsREVNull()) {
						Item = r_.PARTNUM;
						Rev = r_.REV;
					}
				}
			}
		}

		private void Query() {
			this.cutlistCheckTableAdapter.FillByItem(this.cTDDataSet.CutlistCheckTable, Item, Rev);
		}

		private void PopulateComboBox() {
			Cursor = Cursors.WaitCursor;
			this.comboboxCutlistsTableAdapter.Fill(this.cTDDataSet.ComboboxCutlists);
			populated = true;
			Cursor = Cursors.Default;
		}

		private void CutlistTableDisplayForm_Load(object sender, EventArgs e) {
			Location = Properties.Settings.Default.CutlistDisplayLocation;
			Size = Properties.Settings.Default.CutlistDisplaySize;
			if (Item != string.Empty && Rev != string.Empty) {
				cutlist_cbx.Text = string.Format(@"{0} REV {1}", Item, Rev);
			}
			dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
			initialated = true;
		}

		private void dataGridView1_SelectionChanged(object sender, EventArgs e) {
			if (!initialated) {
				return;
			}

			DataGridView dgv = sender as DataGridView;
			if (dgv.SelectedCells.Count < 1) {
				selected_lbl.Text = "No selection";
				ClID = 0;
				PartID = 0;
				return;
			}

			int idx = dgv.SelectedCells[0].RowIndex;
			DataGridViewCellCollection cells = dgv.Rows[idx].Cells;
			PartLookup = cells[1].Value.ToString();
			selected_lbl.Text = string.Format(@"Selected: {0}", PartLookup);

			if (!int.TryParse(cells[PartIDColumn].Value.ToString(), out PartID)) {
				remove_btn.Enabled = false;
				throw new Exception("Couldn't get Part ID!");
			}

			if (!int.TryParse(cells[ClIDColumn].Value.ToString(), out ClID)) {
				remove_btn.Enabled = false;
				throw new Exception("Couldn't get Cutlist ID!");
			}

			if (!int.TryParse(cells[ClPartIDColumn].Value.ToString(), out ClPartID)) {
				remove_btn.Enabled = false;
				throw new Exception("Couldn't get Cutlist Part ID!");
			}
		}

		private void remove_btn_Click(object sender, EventArgs e) {
			if (PartID == 0 || ClID == 0 || ClPartID == 0 || PartLookup == string.Empty) {
				MessageBox.Show(@"Make a selection first.");
				return;
			}
			string msg = string.Format(@"Do you really want to remove '{0}' from {1} REV {2}?",
				PartLookup, Item, Rev);
			DialogResult dr = MessageBox.Show(this, msg, "RLY?",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
			if (dr != DialogResult.Yes && ClPartID > 0) {
				return;
			}
			using (CTDDataSetTableAdapters.QueriesTableAdapter ta_ =
					new CTDDataSetTableAdapters.QueriesTableAdapter()) {
				ta_.DeleteByClPartID(ClPartID);
				Query();
				dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
			}
		}

		private void cutlist_cbx_MouseClick(object sender, MouseEventArgs e) {
			if (!populated) {
				PopulateComboBox();
			}
		}

		private void cutlist_cbx_SelectedIndexChanged(object sender, EventArgs e) {
			ComboBox cb = sender as ComboBox;
			if (cb.SelectedItem == null) {
				return;
			}
			ClID = Convert.ToInt32(cb.SelectedValue);
			GetClInfo();
			this.cutlistCheckTableAdapter.FillByClID(this.cTDDataSet.CutlistCheckTable, ClID);
			dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
		}

		private void cutlist_cbx_KeyDown(object sender, KeyEventArgs e) {
			(sender as ComboBox).DroppedDown = false;
		}

		private void CutlistTableDisplayForm_FormClosing(object sender, FormClosingEventArgs e) {
			Properties.Settings.Default.CutlistDisplayLocation = Location;
			Properties.Settings.Default.CutlistDisplaySize = Size;
			Properties.Settings.Default.Save();
		}
	}
}
