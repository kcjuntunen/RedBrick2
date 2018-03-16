using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
			partsListView.ItemSelectionChanged += PartsListView_ItemSelectionChanged;

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

		private void PartsListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {
			ListView lv_ = sender as ListView;
			if (lv_.SelectedItems.Count > 0 && lv_.SelectedItems[0] != null) {
				using (ENGINEERINGDataSetTableAdapters.CutPartOpsTableAdapter ta_ =
					new ENGINEERINGDataSetTableAdapters.CutPartOpsTableAdapter()) {
					using (ENGINEERINGDataSet.CutPartOpsDataTable dt_ = ta_.GetDataByPartID(9999)) {
						foreach (ENGINEERINGDataSet.CutPartOpsRow row in dt_) {
						}
					}
				}
			}
		}

		private void ManageCutlistTime_Load(object sender, EventArgs e) {
			// TODO: This line of code loads data into the 'eNGINEERINGDataSet.Cutlists' table. You can move, or remove it, as needed.
			cutlistsTableAdapter.Fill(eNGINEERINGDataSet.Cutlists);
		}

		private void cutlistComboBox_SelectedValueChanged(object sender, EventArgs e) {
			ComboBox cb_ = sender as ComboBox;
			if (cb_.SelectedItem != null) {
				using (ENGINEERINGDataSetTableAdapters.CutlistPartsTableAdapter ta_ =
					new ENGINEERINGDataSetTableAdapters.CutlistPartsTableAdapter()) {
					using (ENGINEERINGDataSet.CutlistPartsDataTable dt_ =
						ta_.GetDataByCLID(Convert.ToInt32(cb_.SelectedValue))) {
						if (dt_.Count > 0) {
							partsListView.Items.Clear();
						}
						foreach (ENGINEERINGDataSet.CutlistPartsRow row in dt_) {
							string[] d_ = new string[] { row.PARTNUM, row.QTY.ToString() };
							partsListView.Items.Add(new ListViewItem(d_));
						}
					}
				}
			}
		}
	}
}
