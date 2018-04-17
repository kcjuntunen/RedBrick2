using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RedBrick2 {
	public partial class ECRViewer : Form {
		private string Lookup;
		private string dateFormat = @"MMM dd, yyyy";
		private List<int> items = new List<int>();
		private List<string> drawings = new List<string>();
		private int selectedItem;
		private string originalText = string.Empty;
		public ECRViewer(string lookup) {
			Lookup = lookup;
			InitializeComponent();
			SetSettings();
			ConnectEvents();
			Init();
		}

		public ECRViewer(int ecr) {
			Lookup = ecr.ToString();
			InitializeComponent();
			SetSettings();
			ConnectEvents();
			Init();
		}

		private void SetSettings() {
			ECRlistView.FullRowSelect = true;
			ECRlistView.HideSelection = false;
			ECRlistView.MultiSelect = false;
			ECRlistView.View = View.Details;
			ECRlistView.SmallImageList = Redbrick.TreeViewIcons;

			affectedItemsListView.FullRowSelect = true;
			affectedItemsListView.HideSelection = false;
			affectedItemsListView.MultiSelect = false;
			affectedItemsListView.View = View.Details;
			affectedItemsListView.SmallImageList = Redbrick.TreeViewIcons;

			affectedDrawingsListView.FullRowSelect = true;
			affectedDrawingsListView.HideSelection = false;
			affectedDrawingsListView.MultiSelect = false;
			affectedDrawingsListView.View = View.Details;
			affectedDrawingsListView.SmallImageList = Redbrick.TreeViewIcons;

			signeesListView.FullRowSelect = true;
			signeesListView.HideSelection = false;
			signeesListView.MultiSelect = false;
			signeesListView.View = View.Details;
			signeesListView.SmallImageList = Redbrick.TreeViewIcons;
			
			attachedFilesListView.FullRowSelect = true;
			attachedFilesListView.HideSelection = false;
			attachedFilesListView.MultiSelect = false;
			attachedFilesListView.View = View.Details;
			attachedFilesListView.SmallImageList = Redbrick.TreeViewIcons;
		}

		private void ConnectEvents() {
			ECRlistView.ItemSelectionChanged += ListView1_ItemSelectionChanged;
			ECRlistView.ColumnClick += ColumnClick;
			affectedItemsListView.ItemSelectionChanged += AffectedItemsListView_ItemSelectionChanged;
			affectedItemsListView.ColumnClick += ColumnClick;
			affectedDrawingsListView.MouseDoubleClick += AffectedDrawingsListView_MouseDoubleClick;
			affectedDrawingsListView.ColumnClick += ColumnClick;
			signeesListView.SelectedIndexChanged += SigneesListView_SelectedIndexChanged;
			attachedFilesListView.MouseDoubleClick += AttachedFilesListView_MouseDoubleClick;
		}

		private void Init() {
			if (int.TryParse(Lookup, out int test_)) {
				LookUpECR(test_);
				return;
			}
			LookUpPart(Lookup);
		}

		private void LookUpPart(string _part) {
			using (ENGINEERINGDataSetTableAdapters.ECRObjLookupTableAdapter ta_ =
				new ENGINEERINGDataSetTableAdapters.ECRObjLookupTableAdapter()) {
				using (ENGINEERINGDataSet.ECRObjLookupDataTable dt_ = ta_.GetDataByItemNum(_part)) {
					if (dt_.Count > 0) {
						ECRlistView.Items.Clear();
						foreach (ENGINEERINGDataSet.ECRObjLookupRow row in dt_.Rows) {
							string[] row_str_ = new string[] { row.ECR_NUM.ToString(),
								row.DATE_CREATE.ToString(dateFormat),
								Redbrick.TitleCase(row.STATUS) };
							ListViewItem i_ = new ListViewItem(row_str_, 1);
							ECRlistView.Items.Add(i_);
						}
						ECRlistView.Items[0].Selected = true;
					}
				}
			}
			ECRlistView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
		}

		private void LookUpECR(int eco) {
			using (ENGINEERINGDataSetTableAdapters.ECRObjLookupTableAdapter ta_ =
				new ENGINEERINGDataSetTableAdapters.ECRObjLookupTableAdapter()) {
				using (ENGINEERINGDataSet.ECRObjLookupDataTable dt_ = ta_.GetDataByECO(eco)) {
					if (dt_.Count > 0) {
						ECRlistView.Items.Clear();
						originalText = ECRTextBox.Text;
						foreach (ENGINEERINGDataSet.ECRObjLookupRow row in dt_.Rows) {
							string[] row_str_ = new string[] { row.ECR_NUM.ToString(),
								row.DATE_CREATE.ToString(dateFormat),
								Redbrick.TitleCase(row.STATUS) };
							ListViewItem i_ = new ListViewItem(row_str_, 1);
							ECRlistView.Items.Add(i_);
						}
						ECRlistView.Items[0].Selected = true;
					} else {
						ECRTextBox.Text = originalText;
					}
				}
			}
			ECRlistView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
		}

		private ENGINEERINGDataSet.SigneesDataTable signeesRows(int ecr_) {
			using (ENGINEERINGDataSetTableAdapters.SigneesTableAdapter ta_ =
				new ENGINEERINGDataSetTableAdapters.SigneesTableAdapter()) {
				return ta_.GetDataByECRNum(ecr_);
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

		private	ENGINEERINGDataSet.FILE_MAINDataTable fILE_MAINRows(int ecr_) {
			using (ENGINEERINGDataSetTableAdapters.FILE_MAINTableAdapter ta_ =
				new ENGINEERINGDataSetTableAdapters.FILE_MAINTableAdapter()) {
				return ta_.GetDataByECRNum(Convert.ToInt16(ecr_));	
			}
		}
	
		private void ColumnClick(object sender, ColumnClickEventArgs e) {
			ListView lv_ = sender as ListView;
			if (lv_.Columns[e.Column].ListView.Sorting == SortOrder.Ascending) {
				lv_.Columns[0].ListView.Sorting = SortOrder.Descending;
			} else {
				lv_.Columns[0].ListView.Sorting = SortOrder.Ascending;
			}
		}

		private void AffectedDrawingsListView_MouseDoubleClick(object sender, MouseEventArgs e) {
			ListView lv_ = sender as ListView;
			if (lv_.SelectedItems.Count > 0 && lv_.SelectedItems[0] != null) {
				int idx = lv_.Items.IndexOf(lv_.SelectedItems[0]);
				string path = string.Format(@"\\AMSTORE-SVR-02\shared\shared\general\Engineering Utility\ECR Drawings\{0}",
					drawings[idx]);
				System.Diagnostics.Process.Start(path);
			}
		}

		private void AttachedFilesListView_MouseDoubleClick(object sender, MouseEventArgs e) {
			ListView lv_ = sender as ListView;
			if (lv_.SelectedItems.Count > 0 && lv_.SelectedItems[0] != null) {
				int idx = lv_.Items.IndexOf(lv_.SelectedItems[0]);
				string path = string.Format(@"\\AMSTORE-SVR-02\shared\shared\general\Engineering Utility\Submissions\{0}",
					lv_.SelectedItems[0].SubItems[2].Text);
				System.Diagnostics.Process.Start(path);
			}
		}

		private void AffectedItemsListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {
			ListView lv_ = sender as ListView;
			if (lv_.SelectedItems.Count > 0 && lv_.SelectedItems[0] != null) {
				int idx = lv_.Items.IndexOf(lv_.SelectedItems[0]);
				drawings.Clear();
				affectedDrawingsListView.Items.Clear();
				if (lv_.SelectedItems[0].SubItems.Count > 3) {
				string[] it_ = new string[] {
					lv_.SelectedItems[0].SubItems[3].Text,
					lv_.SelectedItems[0].SubItems[4].Text,
					lv_.SelectedItems[0].SubItems[5].Text };
				affectedDrawingsListView.Items.Add(new ListViewItem(it_, 6));
					string drw_itm = lv_.SelectedItems[0].SubItems[4].Text;
					drawings.Add(it_[2]);
					if (affectedDrawingsListView.Items.Count > 0) {
						affectedDrawingsListView.Items[0].Selected = true;
					}
				}
			}
			affectedDrawingsListView.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);
			affectedDrawingsListView.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.HeaderSize);
		}

		private void SigneesListView_SelectedIndexChanged(object sender, EventArgs e) {
			ListView lv_ = sender as ListView;
			if (lv_.SelectedItems.Count > 0 && lv_.SelectedItems[0] != null) {
				signeesTextBox.Text = lv_.SelectedItems[0].SubItems[3].Text;
			}
		}

		private void ListView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {
			ListView lv_ = sender as ListView;
			if (lv_.SelectedItems.Count > 0 && lv_.SelectedItems[0] != null) {
				int ecrno = Convert.ToInt32(e.Item.SubItems[0].Text);
				ECRTextBox.Text = ecrno.ToString();
				originalText = ecrno.ToString();
				ENGINEERINGDataSet.ECRObjLookupRow r_ = ECRObjLookup(ecrno);
				descriptionTextBox.Text = r_.CHANGES;
				affectedItemsListView.Items.Clear();
				affectedDrawingsListView.Items.Clear();
				signeesListView.Items.Clear();
				attachedFilesListView.Items.Clear();
				items.Clear();
				drawings.Clear();
				foreach (ENGINEERINGDataSet.ECR_ITEMSRow row in eCR_ITEMSRows(ecrno)) {
					string[] row_str_ = new string[] { row.ITEMNUMBER, row.ITEMREV, Redbrick.TitleCase(row.TypeName) };
					ListViewItem l_ = new ListViewItem(row_str_, 5);
					foreach (ENGINEERINGDataSet.ECR_DRAWINGSRow drw in eCR_DRAWINGSRows(row.ITEM_ID)) {
						l_.SubItems.Add(new ListViewItem.ListViewSubItem(l_, drw.ORIG_PATH));
						l_.SubItems.Add(new ListViewItem.ListViewSubItem(l_, drw.DRWREV));
						l_.SubItems.Add(new ListViewItem.ListViewSubItem(l_, drw.DRW_FILE));
					}
					items.Add(row.ITEM_ID);
					affectedItemsListView.Items.Add(l_);
				}
				if (affectedItemsListView.Items.Count > 0) {
					affectedItemsListView.Items[0].Selected = true;
				}
				foreach (ENGINEERINGDataSet.SigneesRow row in signeesRows(ecrno)) {
					string signoff_ = string.Empty;
					string comment_ = string.Empty;
					if (!row.IsSIGNOFFNull()) {
						signoff_  = row.SIGNOFF.ToString(dateFormat);
					}
					if (!row.IsCOMMENTNull()) {
						comment_ = row.COMMENT;
					}
					string[] row_str_ = new string[] { Redbrick.TitleCase(row.UserName),
						Redbrick.TitleCase(row.STATUS),
						signoff_,
						comment_ };
					ListViewItem l_ = new ListViewItem(row_str_, 3);
					signeesListView.Items.Add(l_);
				}
				if (signeesListView.Items.Count > 0) {
					signeesListView.Items[0].Selected = true;
				}
				foreach (ENGINEERINGDataSet.FILE_MAINRow row in fILE_MAINRows(ecrno)) {
					string[] row_str_ = new string[] { row.EXT.ToUpper().Replace(@".", string.Empty), row.COMMENT, row.FileName };
					attachedFilesListView.Items.Add(new ListViewItem(row_str_, 0));
					if (row.EXT == string.Empty) {
						attachedFilesListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
					} else {
						attachedFilesListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
					}
				}
			}
			affectedItemsListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			affectedItemsListView.AutoResizeColumn(2, ColumnHeaderAutoResizeStyle.HeaderSize);
			signeesListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			signeesListView.AutoResizeColumn(2, ColumnHeaderAutoResizeStyle.HeaderSize);
		}

		private void ECRViewer_Load(object sender, EventArgs e) {
			Location = Properties.Settings.Default.ECRViewerLocation;
			Size = Properties.Settings.Default.ECRViewerSize;
		}

		private void ECRViewer_FormClosing(object sender, FormClosingEventArgs e) {
			Properties.Settings.Default.ECRViewerLocation = Location;
			Properties.Settings.Default.ECRViewerSize = Size;
			Properties.Settings.Default.Save();
		}

		private void ECRTextBox_KeyDown(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Enter &&
				int.TryParse(ECRTextBox.Text, out int test_)) {
				LookUpECR(test_);
			}
		}

		private void affectedItemsListView_MouseDoubleClick(object sender, MouseEventArgs e) {
			ListView lv_ = (sender as ListView);
			ListViewItem item_ = lv_.GetItemAt(e.X, e.Y);
			if (item_ != null) {
				Lookup = item_.SubItems[0].Text;
				LookUpPart(Lookup);
			}
		}
	}
}
