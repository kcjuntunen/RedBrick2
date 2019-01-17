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

		public CutlistTableDisplayForm() {
			InitializeComponent();
		}


		public CutlistTableDisplayForm(string item, string rev) {
			InitializeComponent();
			Item = item;
			Rev = rev;
		}


		private void CutlistTableDisplayForm_Load(object sender, EventArgs e) {
			this.cutlistCheckTableAdapter.FillByItem(this.cTDDataSet.CutlistCheckTable, Item, Rev);
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
				throw new Exception("Couldn't get Part ID!");
				//return;
			}

			if (!int.TryParse(cells[ClIDColumn].Value.ToString(), out ClID)) {
				throw new Exception("Couldn't get Cutlist ID!");
				//return;
			}

			if (!int.TryParse(cells[ClPartIDColumn].Value.ToString(), out ClPartID)) {
				throw new Exception("Couldn't get Cutlist Part ID!");
				//return;
			}
		}

		private void remove_btn_Click(object sender, EventArgs e) {
			if (PartID == 0 || ClID == 0 || ClPartID == 0 || PartLookup == string.Empty) {
				MessageBox.Show(@"Make a selection first.");
				return;
			}
			string msg = string.Format(@"Do you really want to remove '{0}' from {1} REV {2}?",
				PartLookup, Item, Rev);
			MessageBox.Show(this, msg, "RLY?",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
		}
	}
}
