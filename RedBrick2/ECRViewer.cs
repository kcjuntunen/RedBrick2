using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RedBrick2 {
	public partial class ECRViewer : Form {
		private string Lookup;
		private List<int> items = new List<int>();
		private int selectedItem;
		public ECRViewer(string lookup) {
			Lookup = lookup;
			InitializeComponent();
			ECRlistView.FullRowSelect = true;
			ECRlistView.HideSelection = false;
			ECRlistView.MultiSelect = false;
			ECRlistView.View = View.Details;

			affectedItemsListView.FullRowSelect = true;
			affectedItemsListView.HideSelection = false;
			affectedItemsListView.MultiSelect = false;
			affectedItemsListView.View = View.Details;

			affectedDrawingsListView.FullRowSelect = true;
			affectedDrawingsListView.HideSelection = false;
			affectedDrawingsListView.MultiSelect = false;
			affectedDrawingsListView.View = View.Details;
			
			ECRlistView.ItemSelectionChanged += ListView1_ItemSelectionChanged;
			affectedItemsListView.ItemSelectionChanged += AffectedItemsListView_ItemSelectionChanged;
			Init();
		}

		private void AffectedItemsListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {
			ListView lv_ = sender as ListView;
			if (lv_.SelectedItems.Count > 0 && lv_.SelectedItems[0] != null) {
				int idx = lv_.Items.IndexOf(lv_.SelectedItems[0]);
				affectedDrawingsListView.Items.Clear();
				using (ENGINEERINGDataSet.ECR_DRAWINGSDataTable dt_ = eCR_DRAWINGSRows(items[idx])) {
					foreach (ENGINEERINGDataSet.ECR_DRAWINGSRow row in dt_.Rows) {
						string[] row_str_ = new string[] { row.ORIG_PATH, row.DRWREV };
						affectedDrawingsListView.Items.Add(new ListViewItem(row_str_));
					}
				}
			}
		}

		private void ListView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {
			ListView lv_ = sender as ListView;
			if (lv_.SelectedItems.Count > 0 && lv_.SelectedItems[0] != null) {
				int ecrno = Convert.ToInt32(e.Item.SubItems[0].Text);
				ENGINEERINGDataSet.ECRObjLookupRow r_ = ECRObjLookup(ecrno);
				descriptionTextBox.Text = r_.CHANGES;
				affectedItemsListView.Items.Clear();
				affectedDrawingsListView.Items.Clear();
				items.Clear();
				foreach (ENGINEERINGDataSet.ECR_ITEMSRow row in eCR_ITEMSRows(ecrno)) {
					string[] row_str_ = new string[] { row.ITEMNUMBER, row.ITEMREV, row.TYPE.ToString() };
					ListViewItem l_ = new ListViewItem(row_str_);
					items.Add(row.ITEM_ID);
					affectedItemsListView.Items.Add(l_);
				}
			}
		}

		private ENGINEERINGDataSet.ECRObjLookupRow ECRObjLookup(int ecr_) {
			using (ENGINEERINGDataSetTableAdapters.ECRObjLookupTableAdapter ta_ =
				new ENGINEERINGDataSetTableAdapters.ECRObjLookupTableAdapter()) {
				using (ENGINEERINGDataSet.ECRObjLookupDataTable dt_ = ta_.GetDataByECO(ecr_)) {
					if (dt_.Count > 0) {
						return dt_[0];
					}
				}
			}
			return null;
		}

		private ENGINEERINGDataSet.ECR_ITEMSDataTable eCR_ITEMSRows(int ecr_) {
			using (ENGINEERINGDataSetTableAdapters.ECR_ITEMSTableAdapter ta_ =
				new ENGINEERINGDataSetTableAdapters.ECR_ITEMSTableAdapter()) {
				return ta_.GetDataByECRNo(ecr_);
			}
		}

		private	ENGINEERINGDataSet.ECR_DRAWINGSDataTable eCR_DRAWINGSRows(int itemid) {
			using (ENGINEERINGDataSetTableAdapters.ECR_DRAWINGSTableAdapter ta_ =
				new ENGINEERINGDataSetTableAdapters.ECR_DRAWINGSTableAdapter()) {
				return ta_.GetDataByItemID(itemid);
			}
		}

		private void Init() {
			using (ENGINEERINGDataSetTableAdapters.ECRObjLookupTableAdapter ta_ =
				new ENGINEERINGDataSetTableAdapters.ECRObjLookupTableAdapter()) {
				using (ENGINEERINGDataSet.ECRObjLookupDataTable dt_ = ta_.GetDataByItemNum(Lookup)) {
					if (dt_.Count > 0) {
						foreach (ENGINEERINGDataSet.ECRObjLookupRow row in dt_.Rows) {
							string[] row_str_ = new string[] { row.ECR_NUM.ToString(),
								row.DATE_CREATE.ToString("dd MMM yyyy"),
								row.STATUS.ToString() };
							ListViewItem i_ = new ListViewItem(row_str_);
							ECRlistView.Items.Add(i_);
						}
					}
				}
			}
		}
	}
}
