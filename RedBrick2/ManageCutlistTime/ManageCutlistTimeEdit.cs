using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace RedBrick2 {
	/// <summary>
	/// A form to manage cutlist time.
	/// </summary>
	public partial class ManageCutlistTimeEdit : Form {
		int starting_clid = 0;
		List<string[]> list = new List<string[]>();
		int idx = 0;
		bool allow_refresh = false;

		/// <summary>
		/// Instantiate <see cref="ManageCutlistTime"/> with nothing preselected.
		/// </summary>
		public ManageCutlistTimeEdit() {
			InitializeComponent();
			SetupListViews();
		}

		/// <summary>
		/// Instantiate <see cref="ManageCutlistTime"/> with stuff preselected.
		/// </summary>
		/// <param name="clid_">Preselect <see cref="int"/> Cutlist ID.</param>
		/// <param name="list_">Highlight any of the items that appear in <see cref="string"/>.</param>
		public ManageCutlistTimeEdit(int clid_, List<string[]> list_) {
			starting_clid = clid_;
			list = list_;
			InitializeComponent();
			SetupListViews();
		}

		private void SetupListViews() {
			cutlistTimeListView.FullRowSelect = true;
			cutlistTimeListView.HideSelection = false;
			cutlistTimeListView.MultiSelect = true;
			cutlistTimeListView.View = View.Details;
			cutlistTimeListView.SmallImageList = Redbrick.TreeViewIcons;
			cutlistComboBox.DrawMode = DrawMode.OwnerDrawFixed;
			op_sel_cb.DrawMode = DrawMode.OwnerDrawFixed;
		}

		private int ValidateAndReturnCTID(out double setup, out double run) {
			setup = 0.0f;
			run = 0.0f;

			int ctid = -1;
			bool setup_has_value = double.TryParse(setup_tb.Text, out setup);
			bool run_has_value = double.TryParse(run_tb.Text, out run);

			if (!(setup_has_value && run_has_value)) {
				return ctid;
			}

			if (Redbrick.FloatEquals((setup + run), 0)) {
				return ctid;
			}

			if (op_chb.Checked) {
				if (op_sel_cb.SelectedItem == null) {
					return ctid;
				}
			} else {
				if (note_tb.Text.Trim() == string.Empty) {
					return ctid;
				}
			}

			if (cutlistComboBox.SelectedItem == null) {
				return ctid;
			}

			if (cutlistTimeListView.SelectedItems.Count < 1) {
				MessageBox.Show(@"You must select an item.");
				return ctid;
			}

			if (cutlistTimeListView.SelectedItems[0] != null) {
				ListViewItem lvi = cutlistTimeListView.SelectedItems[0];
				ListViewItem.ListViewSubItem item = lvi.SubItems[(int)Schema.Cols.CTID];
				int.TryParse(item.Text, out ctid);
			}
			return ctid;
		}

		private void ManageCutlistTimeEdit_Load(object sender, EventArgs e) {
			// TODO: This line of code loads data into the 'manageCutlistTimeDataSet.FriendlyCutOps' table. You can move, or remove it, as needed.
			this.friendlyCutOpsTableAdapter.Fill(this.manageCutlistTimeDataSet.FriendlyCutOps);
			// TODO: This line of code loads data into the 'manageCutlistTimeDataSet.Cutlists' table. You can move, or remove it, as needed.
			this.cutlistsTableAdapter.Fill(this.manageCutlistTimeDataSet.Cutlists);
			ManageCutlistTime.ManageCutlistTimeDataSet.CutlistsRow r_ = manageCutlistTimeDataSet.Cutlists.FindByCLID(starting_clid);
			if (r_ != null) {
				idx = cutlistComboBox.FindStringExact(r_.PARTNUM);
				cutlistComboBox.SelectedIndex = idx;
			}

			foreach (string[] item_ in list) {
				cutlistTimeListView.Items.Add(new ListViewItem(item_));
			}
			allow_refresh = true;
		}

		private void query_cutlist_times(int clid) {
			list.Clear();
			List<string[]> list_ = manageCutlistTimeDataSet.QueryCutlistTime(clid);
			list = list_;
			foreach (string[] item_ in list_) {
				cutlistTimeListView.Items.Add(new ListViewItem(item_));
			}
		}

		private void cutlistComboBox_DrawItem(object sender, DrawItemEventArgs e) {
			ComboBox cb_ = sender as ComboBox;
			int index = e.Index >= 0 ? e.Index : 0;
			System.Data.DataRowView drv_ = cb_.Items[index] as System.Data.DataRowView;
			if (e.Index == idx) {
				Brush brush = ((e.State & DrawItemState.Selected) > 0) ? Brushes.Green : Brushes.Yellow;
				e.Graphics.FillRectangle(brush, e.Bounds);
				e.Graphics.DrawString(drv_[@"CutlistDisplayName"].ToString(), e.Font, SystemBrushes.ControlText,
					e.Bounds, StringFormat.GenericDefault);
			} else {
				e.DrawBackground();
				e.Graphics.DrawString(drv_[@"CutlistDisplayName"].ToString(), e.Font, SystemBrushes.ControlText,
					e.Bounds, StringFormat.GenericDefault);
				e.DrawFocusRectangle();
			}
		}

		private void op_sel_cb_DrawItem(object sender, DrawItemEventArgs e) {
			ComboBox cb_ = sender as ComboBox;
			int index = e.Index >= 0 ? e.Index : 0;
			System.Data.DataRowView drv_ = cb_.Items[index] as System.Data.DataRowView;
			e.Graphics.DrawString(drv_[@"FRIENDLYNAME"].ToString(), e.Font, SystemBrushes.ControlText,
				e.Bounds, StringFormat.GenericDefault);
		}

		private void cutlistComboBox_SelectedIndexChanged(object sender, EventArgs e) {
			if (allow_refresh) {
				ComboBox cb_ = sender as ComboBox;
				if (cb_.SelectedItem != null) {
					cutlistTimeListView.Items.Clear();
					query_cutlist_times(Convert.ToInt32(cb_.SelectedValue));
				}
			}
		}

		private void cutlistTimeListView_SelectedIndexChanged(object sender, EventArgs e) {
			ListView l_ = sender as ListView;
			if (l_.SelectedItems.Count > 0 && l_.SelectedItems[0] != null) {
				ListViewItem lvi_ = l_.SelectedItems[(int)Schema.Cols.TYPE] as ListViewItem;
				setup_tb.Text = lvi_.SubItems[(int)Schema.Cols.SETUP_TIME].Text;
				run_tb.Text = lvi_.SubItems[(int)Schema.Cols.RUN_TIME].Text;
				op_chb.Checked = lvi_.SubItems[(int)Schema.Cols.IS_OP].Text.Contains(@"Y");
				if (int.TryParse(lvi_.SubItems[(int)Schema.Cols.OP].Text, out int test_) && test_ > 0) {
					op_sel_cb.SelectedValue = test_;
				}
				note_tb.Text = lvi_.SubItems[(int)Schema.Cols.NOTE].Text;
			}
		}

		private void delete_btn_Click(object sender, EventArgs e) {
			if (cutlistTimeListView.SelectedItems.Count < 1) {
				return;
			}

			if (cutlistTimeListView.SelectedItems[0] == null) {
				return;
			}

			using (ManageCutlistTime.ManageCutlistTimeDataSetTableAdapters.CUT_CUTLISTS_TIMETableAdapter ta =
				new ManageCutlistTime.ManageCutlistTimeDataSetTableAdapters.CUT_CUTLISTS_TIMETableAdapter()) {
				ListViewItem lvi_ = cutlistTimeListView.SelectedItems[0];
				if (int.TryParse(lvi_.SubItems[4].Text, out int test_)) {
					ta.DeleteByCTID(test_);
				}
			}
		}

		private void update_btn_Click(object sender, EventArgs e) {
			if (cutlistTimeListView.SelectedItems.Count < 1) {
				return;
			}

			if (cutlistTimeListView.SelectedItems[0] == null) {
				return;
			}

			using (ManageCutlistTime.ManageCutlistTimeDataSet ta =
				new ManageCutlistTime.ManageCutlistTimeDataSet()) {
				ListViewItem lvi_ = cutlistTimeListView.SelectedItems[0];
				if (int.TryParse(lvi_.SubItems[4].Text, out int ctid_) &&
					int.TryParse(lvi_.SubItems[7].Text, out int clid_) &&
					int.TryParse(lvi_.SubItems[6].Text, out int ctop_)) {
					int opMethod = 0;
					int opSetup = 0;
					using (ManageCutlistTime.ManageCutlistTimeDataSetTableAdapters.FriendlyCutOpsTableAdapter co =
						new ManageCutlistTime.ManageCutlistTimeDataSetTableAdapters.FriendlyCutOpsTableAdapter()) {
						opSetup = Convert.ToInt32(co.LookupOpSetup(ctop_));
						opMethod = Convert.ToInt32(co.LookupOpMethod(ctop_));
						ta.UpdCLTimeByID(ctid_, clid_, ctop_, opMethod, opSetup);
					}
				}
			}
		}

		private void update_all_btn_Click(object sender, EventArgs e) {
			MessageBox.Show(@"Not implemented.");
		}

		private void clr_btn_Click(object sender, EventArgs e) {
			setup_tb.Text = string.Empty;
			run_tb.Text = string.Empty;
			op_chb.Checked = false;
			op_sel_cb.SelectedIndex = -1;
			note_tb.Text = string.Empty;
		}

		private void save_btn_Click(object sender, EventArgs e) {
			int ctid = ValidateAndReturnCTID(out double setup_, out double run_);

			if (ctid == -1) {
				return;
			}

			using (ManageCutlistTime.ManageCutlistTimeDataSetTableAdapters.CUT_CUTLISTS_TIMETableAdapter ta_ =
				new ManageCutlistTime.ManageCutlistTimeDataSetTableAdapters.CUT_CUTLISTS_TIMETableAdapter()) {
				ta_.UpdateRecord(op_chb.Checked, Convert.ToInt32(op_sel_cb.SelectedValue), setup_, run_, note_tb.Text.Trim(), ctid);
				ListViewItem lvi = cutlistTimeListView.SelectedItems[0];
				lvi.SubItems[5].Text = note_tb.Text.Trim();
				list[cutlistTimeListView.SelectedIndices[0]][5] = note_tb.Text.Trim();
			}

		}

		private void add_btn_Click(object sender, EventArgs e) {
			int ctid = ValidateAndReturnCTID(out double setup_, out double run_);

			if (ctid == -1) {
				return;
			}

			using (ManageCutlistTime.ManageCutlistTimeDataSetTableAdapters.CUT_CUTLISTS_TIMETableAdapter ta_ =
				new ManageCutlistTime.ManageCutlistTimeDataSetTableAdapters.CUT_CUTLISTS_TIMETableAdapter()) {
				ta_.InsertRecord(Convert.ToInt32(cutlistComboBox.SelectedValue), op_chb.Checked, Convert.ToInt32(op_sel_cb.SelectedValue), setup_, run_, note_tb.Text.Trim());
			}
		}
	}
}
