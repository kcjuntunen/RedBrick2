using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace RedBrick2.ManageCutlistTime {
	/// <summary>
	/// A form to manage cutlist time.
	/// </summary>
	public partial class ManageCutlistTime : Form {
		Dictionary<string, string[]> partdict_ = new Dictionary<string, string[]>();
		List<int> types_ = new List<int>();
		List<int> clids_ = new List<int>();
		List<string[]> cutlisttimes_ = new List<string[]>();
		Dictionary<string, int> type_table_ = new Dictionary<string, int>();
		string lookup_ = string.Empty;
		int clid_ = 1;
		int starting_clid_ = 0;
		int guessed_clid_ = 0;
		bool allow_refresh_ = false;
		bool initialized = false;
		bool recalc = false;

		/// <summary>
		/// Constructor. Preselect a cutlist.
		/// </summary>
		/// <param name="clid">A <see cref="int"/> of Cutlist ID to preselect.</param>
		public ManageCutlistTime(int clid) : base() {
			InitializeComponent();
			starting_clid_ = clid;
			cutlistsTableAdapter.Fill(manageCutlistTimeDataSet.Cutlists);
			setup_listviews();
			setup_types();
			initialized = true;
		}

		/// <summary>
		/// Constructor. Instantiate and try to guess at the cutlist.
		/// </summary>
		/// <param name="lookup">A <see cref="string"/> lookup value from <see cref="Redbrick.FileInfoToLookup(System.IO.FileInfo)"/>.</param>
		public ManageCutlistTime(string lookup) : base() {
			lookup_ = lookup.ToUpper();
			InitializeComponent();
			int clid = 0;
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
						string rev_ = r_.IsCutlistDisplayNameNull() ? @"N/A" : r_.CutlistDisplayName;
					}
				}
			}
			clids_.Add(cutlistComboBox.FindString(lookup));
			guessed_clid_ = clid;
			setup_listviews();
			setup_types();
			initialized = true;
		}

		/// <summary>
		/// Constructor. Preselect a cutlist.
		/// </summary>
		/// <param name="lookup">A <see cref="string"/> lookup value from <see cref="Redbrick.FileInfoToLookup(System.IO.FileInfo)"/>.</param>
		/// <param name="clid">A <see cref="int"/> of Cutlist ID to preselect.</param>
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
						ManageCutlistTimeDataSet.CutlistPartsRow r_ = cpdt_[0];
						clid = r_.CLID;
					}
				}
			}
			setup_listviews();
			setup_types();
			initialized = true;
		}

		/// <summary>
		/// Constructor. Instantiate without preselection.
		/// </summary>
		public ManageCutlistTime() {
			if (!initialized) {
				InitializeComponent();
				cutlistsTableAdapter.Fill(manageCutlistTimeDataSet.Cutlists);
				setup_listviews();
				setup_types();
			}
			initialized = true;
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
			cutlistTimeListView.MultiSelect = true;
			cutlistTimeListView.View = View.Details;
			cutlistTimeListView.SmallImageList = Redbrick.TreeViewIcons;
			cutlistComboBox.DrawMode = DrawMode.OwnerDrawFixed;
		}

		private void ManageCutlistTime_Load(object sender, EventArgs e) {
			GenerateTitle();
		}

		private void GenerateTitle() {
			if (starting_clid_ > 0) {
				ManageCutlistTimeDataSet.CutlistsRow r_ = manageCutlistTimeDataSet.Cutlists.FindByCLID(starting_clid_);
				if (r_ != null) {
					clids_.Add(r_.CLID);
					string rev_ = r_.IsREVNull() ? @"N/A" : r_.REV;
					string lookup_note = lookup_ != string.Empty ? string.Format(@" Part: {0}", lookup_) : string.Empty;
					Text = string.Format(@"Manage Cutlist Time - Cutlist: {0} REV {1}{2}", r_.PARTNUM, r_.REV, lookup_note);
				}
			} else if (guessed_clid_ > 0) {
				ManageCutlistTimeDataSet.CutlistsRow r_ = manageCutlistTimeDataSet.Cutlists.FindByCLID(guessed_clid_);
				if (r_ != null) {
					clids_.Add(r_.CLID);
					string rev_ = r_.IsREVNull() ? @"N/A" : r_.REV;
					string lookup_note = lookup_ != string.Empty ? string.Format(@" Part: {0}", lookup_) : string.Empty;
					Text = string.Format(@"Manage Cutlist Time - Guessed cutlist: {0} REV {1}{2}", r_.PARTNUM, r_.REV, lookup_note);
				}
			} else {
				if (lookup_ != string.Empty) {
					Text = string.Format(@"Manage Cutlist Time - Part: {0} (Undefined Cutlist)", lookup_);
				}
			}
		}

		private void ManageCutlistTime_Shown(object sender, EventArgs e) {
			if (starting_clid_ > 0) {
				cutlistComboBox.SelectedValue = starting_clid_;
			} else if (guessed_clid_ > 0) {
				cutlistComboBox.SelectedValue = guessed_clid_;
			} else {
				int idx = cutlistComboBox.FindString(lookup_);
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
				}
				includeListBox.Sorted = true;
				SelectAllTypes();
				query_cutlist_time();
				ShowIncludedParts();
			}
		}

		private void query_cutlist_time() {
			cutlistTimeListView.Items.Clear();
			cutlisttimes_.Clear();
			List<string[]> list_ = manageCutlistTimeDataSet.QueryCutlistTime(Convert.ToInt32(cutlistComboBox.SelectedValue));
			cutlisttimes_ = list_;
			foreach (string[] item_ in list_) {
				cutlistTimeListView.Items.Add(new ListViewItem(item_));
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
				if (lookup_ != string.Empty && item_.SubItems[(int)Schema.Cols.TYPE].Text.ToString().ToUpper().Equals(lookup_)) {
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
			recalc = false;
			foreach (var item in cutlistTimeListView.Items) {
				(item as ListViewItem).Selected = true;
			}
			recalc = true;
			Recalc();
		}

		private void noneButton_Click(object sender, EventArgs e) {
			recalc = false;
			cutlistTimeListView.SelectedItems.Clear();
			recalc = true;
			Recalc();
		}

		private void cutlistComboBox_DrawItem(object sender, DrawItemEventArgs e) {
			ComboBox cb_ = sender as ComboBox;
			int index = e.Index >= 0 ? e.Index : 0;
			System.Data.DataRowView drv_ = cb_.Items[index] as System.Data.DataRowView;
			if (clids_.Contains(index)) {
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

		private void manageButton_MouseClick(object sender, MouseEventArgs e) {
			using (ManageCutlistTimeEdit mcte_ = new ManageCutlistTimeEdit(clid_, cutlisttimes_)) {
				mcte_.ShowDialog(this);
			}
			query_cutlist_time();
		}

		private void cutlistTimeListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {
			if (recalc) {
				Recalc();
			}
		}

		private void Recalc() {
			List<string[]> list_ = manageCutlistTimeDataSet.QueryCutlistTime(Convert.ToInt32(cutlistComboBox.SelectedValue));
			ShowIncludedParts();
			double setup_ = 0.0f;
			double run_ = 0.0f;
			double.TryParse(setupTextBox.Text, out setup_);
			double.TryParse(runTextBox.Text, out run_);

			if (cutlistTimeListView.SelectedItems.Count > 0) {
				foreach (ListViewItem item in cutlistTimeListView.SelectedItems) {
					double.TryParse(item.SubItems[(int)Schema.Cols.SETUP_TIME].Text, out double setup_time_);
					double.TryParse(item.SubItems[(int)Schema.Cols.RUN_TIME].Text, out double run_time_);
					setup_ += setup_time_;
					run_ += run_time_;
				}
			}
			setupTextBox.Text = setup_.ToString(@"0.0000");
			runTextBox.Text = run_.ToString(@"0.0000");
		}
	}
}
