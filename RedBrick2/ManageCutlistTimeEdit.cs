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

		private void ManageCutlistTimeEdit_Load(object sender, EventArgs e) {
			// TODO: This line of code loads data into the 'manageCutlistTimeDataSet.FriendlyCutOps' table. You can move, or remove it, as needed.
			this.friendlyCutOpsTableAdapter.Fill(this.manageCutlistTimeDataSet.FriendlyCutOps);
			// TODO: This line of code loads data into the 'manageCutlistTimeDataSet.Cutlists' table. You can move, or remove it, as needed.
			this.cutlistsTableAdapter.Fill(this.manageCutlistTimeDataSet.Cutlists);
			ManageCutlistTimeDataSet.CutlistsRow r_ = manageCutlistTimeDataSet.Cutlists.FindByCLID(starting_clid);
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
				ListViewItem lvi_ = l_.SelectedItems[0] as ListViewItem;
				setup_tb.Text = lvi_.SubItems[2].Text;
				run_tb.Text = lvi_.SubItems[3].Text;
				op_chb.Checked = lvi_.SubItems[1].Text.Contains(@"Y");
				if (int.TryParse(lvi_.SubItems[6].Text, out int test_) && test_ > 0) {
					op_sel_cb.SelectedValue = test_;
				}
				note_tb.Text = lvi_.SubItems[5].Text;
			}
		}
	}
}
