using System;
using System.Windows.Forms;

namespace RedBrick2 {
	public partial class ManageCutlistTime : Form {
		public ManageCutlistTime() {
			InitializeComponent();

			partsListView.FullRowSelect = true;
			partsListView.HideSelection = false;
			partsListView.MultiSelect = false;
			partsListView.View = View.Details;
			partsListView.SmallImageList = Redbrick.TreeViewIcons;
			partsListView.ItemSelectionChanged += partsListView_ItemSelectionChanged;

			partOpsListView.FullRowSelect = true;
			partOpsListView.HideSelection = false;
			partOpsListView.MultiSelect = false;
			partOpsListView.View = View.Details;
			partOpsListView.SmallImageList = Redbrick.TreeViewIcons;

			cutlistTimeListView.FullRowSelect = true;
			cutlistTimeListView.HideSelection = false;
			cutlistTimeListView.MultiSelect = false;
			cutlistTimeListView.View = View.Details;
			cutlistTimeListView.SmallImageList = Redbrick.TreeViewIcons;
		}

		private void ManageCutlistTime_Load(object sender, EventArgs e) {
			// TODO: This line of code loads data into the 'eNGINEERINGDataSet.Cutlists' table. You can move, or remove it, as needed.
			cutlistsTableAdapter.Fill(eNGINEERINGDataSet.Cutlists);
		}

		private void cutlistComboBox_SelectedValueChanged(object sender, EventArgs e) {
			ComboBox cb_ = sender as ComboBox;
			if (cb_.SelectedItem != null) {
				int clid_ = Convert.ToInt32(cb_.SelectedValue);
				using (ENGINEERINGDataSetTableAdapters.CutlistPartsTableAdapter ta_ =
					new ENGINEERINGDataSetTableAdapters.CutlistPartsTableAdapter()) {
					using (ENGINEERINGDataSet.CutlistPartsDataTable dt_ =
						ta_.GetDataByCLID(clid_)) {
						if (dt_.Count > 0) {
							partsListView.Items.Clear();
							partOpsListView.Items.Clear();
							includeListBox.Items.Clear();
						}
						foreach (ENGINEERINGDataSet.CutlistPartsRow row in dt_) {
							string[] d_ = new string[] { row.PARTNUM, row.QTY.ToString() };
							if (!includeListBox.Items.Contains(row.TYPE.ToString())) {
								includeListBox.Items.Add(row.TYPE.ToString());
							}
							partsListView.Items.Add(new ListViewItem(d_));
						}
					}
				}
				using (ENGINEERINGDataSetTableAdapters.CUT_PART_OPSTableAdapter ta_ =
					new ENGINEERINGDataSetTableAdapters.CUT_PART_OPSTableAdapter()) {
					double setup_ = Convert.ToDouble(ta_.GetCutlistSetupTime(clid_));
					double run_ = Convert.ToDouble(ta_.GetCutlistRunTime(clid_));
					setupTextBox.Text = setup_.ToString(@"0.0000");
					runTextBox.Text = run_.ToString(@"0.0000");
				}
				//partsListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			}
		}

		private void partsListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {
			ListView lv_ = sender as ListView;
			if (lv_.SelectedItems.Count > 0 && lv_.SelectedItems[0] != null) {
				partOpsListView.Items.Clear();
				using (ENGINEERINGDataSetTableAdapters.FriendlyCutPartOpsTableAdapter ta_ =
					new ENGINEERINGDataSetTableAdapters.FriendlyCutPartOpsTableAdapter()) {
					using (ENGINEERINGDataSet.FriendlyCutPartOpsDataTable dt_ = ta_.GetDataByPartNum(lv_.SelectedItems[0].Text)) {
						foreach (ENGINEERINGDataSet.FriendlyCutPartOpsRow r_ in dt_) {
							string[] data_ = new string[] { r_.OPNAME,
								Redbrick.TitleCase(r_.OPDESCR),
								r_.POPSETUP.ToString(@"0.00"),
								r_.POPRUN.ToString(@"0.000000") };
							partOpsListView.Items.Add(new ListViewItem(data_));
						}
					}
				}
				//partOpsListView.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.HeaderSize);
				//partOpsListView.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent);
				//partOpsListView.AutoResizeColumn(2, ColumnHeaderAutoResizeStyle.HeaderSize);
				//partOpsListView.AutoResizeColumn(3, ColumnHeaderAutoResizeStyle.HeaderSize);
			}
		}

		private void includeListBox_SelectedIndexChanged(object sender, EventArgs e) {
		}
	}
}
