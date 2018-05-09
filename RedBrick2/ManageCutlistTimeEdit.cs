using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace RedBrick2 {
	public partial class ManageCutlistTimeEdit : Form {
		int starting_clid = 0;
		List<string[]> list = new List<string[]>();
		int idx = 0;
		bool allow_refresh = false;

		public ManageCutlistTimeEdit() {
			InitializeComponent();
			SetupListViews();
		}

		public ManageCutlistTimeEdit(int clid_, List<string[]> list_) {
			starting_clid = clid_;
			list = list_;
			InitializeComponent();
			SetupListViews();
		}

		private void SetupListViews() {
			cutlistTimeListView.FullRowSelect = true;
			cutlistTimeListView.HideSelection = false;
			cutlistTimeListView.MultiSelect = false;
			cutlistTimeListView.View = View.Details;
			cutlistTimeListView.SmallImageList = Redbrick.TreeViewIcons;
			cutlistComboBox.DrawMode = DrawMode.OwnerDrawFixed;
		}

		private void ManageCutlistTimeEdit_Load(object sender, EventArgs e) {
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

		private void cutlistComboBox_SelectedIndexChanged(object sender, EventArgs e) {
			if (allow_refresh) {
				ComboBox cb_ = sender as ComboBox;
				if (cb_.SelectedItem != null) {
					cutlistTimeListView.Items.Clear();
					query_cutlist_times(Convert.ToInt32(cb_.SelectedValue));
				}
			}
		}
	}
}
