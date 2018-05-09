using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace RedBrick2 {
	public partial class ManageCutlistTime : Form {
		Dictionary<string, string[]> partdict_ = new Dictionary<string, string[]>();
		List<int> types_ = new List<int>();
		List<int> clids_ = new List<int>();
		Dictionary<string, int> type_table_ = new Dictionary<string, int>();
		string lookup_ = string.Empty;
		int clid_ = 1;
		int starting_clid_ = 1;
		bool allow_refresh_ = false;
		bool initialized = false;

		public ManageCutlistTime(int clid) : base() {
			InitializeComponent();
			starting_clid_ = clid;
			cutlistsTableAdapter.Fill(manageCutlistTimeDataSet.Cutlists);
			ManageCutlistTimeDataSet.CutlistsRow r_ = manageCutlistTimeDataSet.Cutlists.FindByCLID(clid);
			if (r_ != null) {
				clids_.Add(cutlistComboBox.FindStringExact(r_.PARTNUM));
				string rev_ = r_.IsREVNull() ? @"N/A" : r_.REV;
				Text = string.Format(@"Manage Cutlist Time - Found cutlist: {0} REV {1}", r_.PARTNUM, r_.REV);
			}
			setup_listviews();
			setup_types();
			initialized = true;
		}

		public ManageCutlistTime(string lookup) : base() {
			lookup_ = lookup.ToUpper();
			InitializeComponent();
			int clid = 1;
			cutlistsTableAdapter.Fill(manageCutlistTimeDataSet.Cutlists);
			using (ManageCutlistTimeDataSetTableAdapters.CutlistPartsTableAdapter cpta_ =
				new ManageCutlistTimeDataSetTableAdapters.CutlistPartsTableAdapter()) {
				using (ManageCutlistTimeDataSet.CutlistPartsDataTable cpdt_ =
					cpta_.GetDataByPartNum(lookup)) {
					if (cpdt_.Rows.Count > 0) {
						foreach (ManageCutlistTimeDataSet.CutlistPartsRow row_ in cpdt_) {
							clids_.Add(cutlistComboBox.FindString(row_.CUTLIST));
						}
						ManageCutlistTimeDataSet.CutlistPartsRow r_ = cpdt_[0];
						clid = r_.CLID;
						string rev_ = r_.IsREVNull() ? @"N/A" : r_.REV;
						Text = string.Format(@"Manage Cutlist Time - Cutlist: {0} REV {1}, Part: {2}", r_.PARTNUM, r_.REV, lookup);
					} else {
						clid = 1;
					}
				}
			}
			clids_.Add(cutlistComboBox.FindStringExact(lookup));
			starting_clid_ = clid;
			setup_listviews();
			setup_types();
			initialized = true;
		}

		public ManageCutlistTime(string lookup, int clid) : base() {
			lookup_ = lookup.ToUpper();
			starting_clid_ = clid;
			InitializeComponent();
			cutlistsTableAdapter.Fill(manageCutlistTimeDataSet.Cutlists);
			using (ManageCutlistTimeDataSetTableAdapters.CutlistPartsTableAdapter cpta_ =
				new ManageCutlistTimeDataSetTableAdapters.CutlistPartsTableAdapter()) {
				using (ManageCutlistTimeDataSet.CutlistPartsDataTable cpdt_ =
					cpta_.GetDataByPartNum(lookup)) {
					if (cpdt_.Rows.Count > 0) {
						foreach (ManageCutlistTimeDataSet.CutlistPartsRow row_ in cpdt_) {
							clids_.Add(cutlistComboBox.FindString(row_.CUTLIST));
						}
						clid = cpdt_[0].CLID;
					} else {
						clid = 1;
					}
				}
			}
			setup_listviews();
			setup_types();
			initialized = true;
		}

		public ManageCutlistTime() {
			if (!initialized) {
				InitializeComponent();
				cutlistsTableAdapter.Fill(manageCutlistTimeDataSet.Cutlists);
				setup_listviews();
				setup_types();
			}
		}

		private void setup_types() {
			using (ManageCutlistTimeDataSetTableAdapters.CUT_PART_TYPESTableAdapter cpta_ =
				new ManageCutlistTimeDataSetTableAdapters.CUT_PART_TYPESTableAdapter()) {
				cpta_.Fill(manageCutlistTimeDataSet.CUT_PART_TYPES);
				foreach (ManageCutlistTimeDataSet.CUT_PART_TYPESRow row_ in manageCutlistTimeDataSet.CUT_PART_TYPES) {
					type_table_.Add(row_.TYPEDESC, row_.TYPEID);
				}
			}
		}

		private void setup_listviews() {
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
			cutlistComboBox.DrawMode = DrawMode.OwnerDrawFixed;
		}

		private void ManageCutlistTime_Load(object sender, EventArgs e) {

		}

		private void ManageCutlistTime_Shown(object sender, EventArgs e) {
			if (starting_clid_ > 1) {
			  cutlistComboBox.SelectedValue = starting_clid_;
			} else {
				int idx = cutlistComboBox.FindStringExact(lookup_);
				cutlistComboBox.SelectedIndex = idx > 1 ? idx : 1; 
			}
		}

		private void cutlistComboBox_SelectedValueChanged(object sender, EventArgs e) {
			allow_refresh_ = false;
			if (initialized) {
				ComboBox cb_ = sender as ComboBox;
				if (cb_.SelectedItem != null) {
					clid_ = Convert.ToInt32(cb_.SelectedValue);
					using (ManageCutlistTimeDataSetTableAdapters.CutlistPartsTableAdapter ta_ =
						new ManageCutlistTimeDataSetTableAdapters.CutlistPartsTableAdapter()) {
						using (ManageCutlistTimeDataSet.CutlistPartsDataTable dt_ =
							ta_.GetDataByCLID(clid_)) {
							if (dt_.Count > 0) {
								partsListView.Items.Clear();
								partOpsListView.Items.Clear();
								includeListBox.Items.Clear();
								cutlistTimeListView.Items.Clear();
								partdict_.Clear();
							}
							foreach (ManageCutlistTimeDataSet.CutlistPartsRow row in dt_) {
								ManageCutlistTimeDataSet.CUT_PART_TYPESRow type_ =
									manageCutlistTimeDataSet.CUT_PART_TYPES.FindByTYPEID(row.TYPE);
								bool got_a_type_ = type_ != null && !type_.IsTYPEDESCNull();
								string typd_ = got_a_type_ ? type_.TYPEDESC : @"N/A";
								string[] d_ = new string[] { row.PARTNUM, row.QTY.ToString(), row.TYPE.ToString(), typd_ };
								if (got_a_type_ && !includeListBox.Items.Contains(typd_)) {
									includeListBox.Items.Add(typd_);
								}
								partdict_.Add(d_[0], d_);
							}
						}
					}
					//partsListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
				}
				includeListBox.Sorted = true;
				SelectAllTypes();
				query_cutlist_time(clid_);
				ShowIncludedParts();
			}
		}

		private void query_cutlist_time(int clid) {
			using (ManageCutlistTimeDataSetTableAdapters.CUT_CUTLISTS_TIMETableAdapter ta_ =
				new ManageCutlistTimeDataSetTableAdapters.CUT_CUTLISTS_TIMETableAdapter()) {
				foreach (ManageCutlistTimeDataSet.CUT_CUTLISTS_TIMERow row_ in ta_.GetDataByCLID(clid)) {
					string type_ = !row_.IsOPNAMENull() ? row_.OPNAME : row_.CTNOTE;
					string op_ = row_.CTISOP ? "Y" : "N";
					string setupTime_ = row_.CTSETUP.ToString(@"0.00");
					string runTime_ = row_.CTRUN.ToString(@"0.000000");
					string[] d_ = new string[] { type_, op_, setupTime_, runTime_, row_.CTID.ToString() };
					cutlistTimeListView.Items.Add(new ListViewItem(d_));
				}
			}
		}

		private void partsListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {
			ListView lv_ = sender as ListView;
			if (lv_.SelectedItems.Count > 0 && lv_.SelectedItems[0] != null) {
				partOpsListView.Items.Clear();
				using (ManageCutlistTimeDataSetTableAdapters.FriendlyCutPartOpsTableAdapter ta_ =
					new ManageCutlistTimeDataSetTableAdapters.FriendlyCutPartOpsTableAdapter()) {
					using (ManageCutlistTimeDataSet.FriendlyCutPartOpsDataTable dt_ = ta_.GetDataByPartNum(lv_.SelectedItems[0].Text)) {
						foreach (ManageCutlistTimeDataSet.FriendlyCutPartOpsRow r_ in dt_) {
							string[] data_ = new string[] { r_.OPNAME,
								Redbrick.TitleCase(r_.OPDESCR),
								r_.POPSETUP.ToString(@"0.00"),
								r_.POPRUN.ToString(@"0.000000") };
							partOpsListView.Items.Add(new ListViewItem(data_));
						}
					}
				}
			}
		}

		private void ShowIncludedParts() {
			allow_refresh_ = false;
			partsListView.Items.Clear();
			types_.Clear();
			for (int i = 0; i < includeListBox.Items.Count; i++) {
				if (includeListBox.GetSelected(i)) {
					types_.Add(type_table_[includeListBox.Items[i].ToString()]);
					foreach (KeyValuePair<string, string[]> pair_ in partdict_) {
						if (includeListBox.Items[i].ToString() == pair_.Value[3]) {
							partsListView.Items.Add(new ListViewItem(pair_.Value));
						}
					}
				}
			}
			if (types_.Count > 0) {
				using (ManageCutlistTimeDataSetTableAdapters.CUT_CUTLISTS_TIMETableAdapter dt_ = 
					new ManageCutlistTimeDataSetTableAdapters.CUT_CUTLISTS_TIMETableAdapter()) {
					double setup_ = Convert.ToDouble(dt_.GetCutlistSetupTime(clid_, types_.ToArray()));
					double run_ = Convert.ToDouble(dt_.GetCutlistRunTime(clid_, types_.ToArray()));
					setupTextBox.Text = setup_.ToString(@"0.0000");
					runTextBox.Text = run_.ToString(@"0.0000");
				}
			} else {
				setupTextBox.Text = @"0.0000";
				runTextBox.Text = @"0.0000";
			}
			if (partsListView.Items.Count > 0) {
				partsListView.Items[0].Selected = true;
			}
			foreach (ListViewItem item_ in partsListView.Items) {
				if (lookup_ != string.Empty && item_.SubItems[0].Text.ToString().ToUpper().Equals(lookup_)) {
					item_.BackColor = Color.Yellow;
				}
			}
			allow_refresh_ = true;
		}

		private void includeListBox_SelectedIndexChanged(object sender, EventArgs e) {
			if (allow_refresh_)
				ShowIncludedParts();
		}

		private void button3_Click(object sender, EventArgs e) {
			ShowIncludedParts();
		}

		private void button1_Click(object sender, EventArgs e) {
			SelectAllTypes();
		}

		private void button2_Click(object sender, EventArgs e) {
			for (int i = 0; i < includeListBox.Items.Count; i++) {
				includeListBox.SetSelected(i, false);
			}
		}

		private void SelectAllTypes() {
			for (int i = 0; i < includeListBox.Items.Count; i++) {
				includeListBox.SetSelected(i, true);
			}
		}

		private void allButton_Click(object sender, EventArgs e) {
			foreach (var item in cutlistTimeListView.Items) {
				(item as ListViewItem).Selected = true;
			}
		}

		private void noneButton_Click(object sender, EventArgs e) {
			foreach (var item in cutlistTimeListView.Items) {
				(item as ListViewItem).Selected = false;
			}
		}

		private void cutlistComboBox_DrawItem(object sender, DrawItemEventArgs e) {
			ComboBox cb_ = sender as ComboBox;
			int index = e.Index >= 0 ? e.Index : 0;
			System.Data.DataRowView drv_ = cb_.Items[index] as System.Data.DataRowView;
			if (clids_.Contains(index)) {
				Brush brush = ((e.State & DrawItemState.Selected) > 0) ? Brushes.Green : Brushes.Yellow;
				e.Graphics.FillRectangle(brush, e.Bounds);
				e.Graphics.DrawString(drv_[@"PARTNUM"].ToString(), e.Font, SystemBrushes.ControlText,
					e.Bounds, StringFormat.GenericDefault);
			} else {
				e.DrawBackground();
				e.Graphics.DrawString(drv_[@"PARTNUM"].ToString(), e.Font, SystemBrushes.ControlText,
					e.Bounds, StringFormat.GenericDefault);
				e.DrawFocusRectangle();
			}
		}
	}
}
