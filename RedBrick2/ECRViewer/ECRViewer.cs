using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace RedBrick2.ECRViewer {
	/// <summary>
	/// A Form for viewing ECR data.
	/// </summary>
	public partial class ECRViewer : Form {
		private string Lookup;
		private const string dateFormat = @"MMM dd, yyyy";
		private List<int> items = new List<int>();
		private List<string> drawings = new List<string>();
		private string originalText = string.Empty;

		/// <summary>
		/// Instantiate an ECRViewer and look up related ECRs.
		/// </summary>
		/// <param name="lookup">A lookup <see cref="string"/>.</param>
		public ECRViewer(string lookup) {
			Lookup = lookup;
			if (lookup.Contains(@"REV")) {
				Lookup = lookup.Split(new string[] { @"REV" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
			}
			InitializeComponent();
			SetSettings();
			ConnectEvents();
			LookupItem(Lookup);
		}

		/// <summary>
		/// Instantiate an ECRViewer with an ECR preselected.
		/// </summary>
		/// <param name="ecr">An ECR #.</param>
		public ECRViewer(int ecr) {
			Lookup = ecr.ToString();
			InitializeComponent();
			SetSettings();
			ConnectEvents();
			LookupItem(Lookup);
		}

		private void SetSettings() {
#if DEBUG
			Redbrick.LastLegacyECR = 8995;
#endif
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

			if (Properties.Settings.Default.FlameWar) {
				ECRTextBox.CharacterCasing = CharacterCasing.Upper;
			}
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

		private void LookupItem(string item_) {
			if (int.TryParse(Lookup, out int test_)) {
				LookUpECR(test_);
				return;
			}
			LookUpPart(item_);
		} 

		private void LookUpPart(string _part) {
			bool cleared_ = false;
			using (ECRViewerDataSetTableAdapters.ECRObjLookupTableAdapter ta_ =
				new ECRViewerDataSetTableAdapters.ECRObjLookupTableAdapter()) {
				using (ECRViewerDataSet.ECRObjLookupDataTable dt_ = ta_.GetDataByItemNum(_part)) {
					if (dt_.Count > 0) {
						ECRlistView.Items.Clear();
						cleared_ = true;
						originalText = ECRTextBox.Text;
						foreach (ECRViewerDataSet.ECRObjLookupRow row in dt_.Rows) {
							if (ECRlistView.Items.Count > 0 &&
								ECRlistView.Items[ECRlistView.Items.Count - 1].Text == row.ECR_NUM.ToString()) {
								continue;
							}
							string[] row_str_ = new string[] { row.ECR_NUM.ToString(),
								row.DATE_CREATE.ToString(dateFormat),
								Redbrick.TitleCase(row.STATUS) };
							ListViewItem i_ = new ListViewItem(row_str_, 1);
							ECRlistView.Items.Add(i_);
						}
						Text = string.Format(@"ECR Viewer - {0}", _part);
						ECRlistView.Items[0].Selected = true;
					} else {
						ECRTextBox.Text = originalText;
					}
				}
			}

			using (ECRViewerDataSetTableAdapters.ECR_LEGACYTableAdapter ta_ =
				new ECRViewerDataSetTableAdapters.ECR_LEGACYTableAdapter()) {
				using (ECRViewerDataSet.ECR_LEGACYDataTable dt_ = ta_.GetDataByItemNum(_part)) {
					if (dt_.Count > 0) {
						if (!cleared_) {
							ECRlistView.Items.Clear();
						}
						foreach (ECRViewerDataSet.ECR_LEGACYRow row_ in dt_.Rows) {
							string holder_ = row_.IsHolderNull() ? "??" : Redbrick.TitleCase(row_.Holder);
							string[] row_str_ = new string[] { string.Format(@"{0} (Legacy)", row_.ECRNum),
								row_.DateRequested.ToString(dateFormat), holder_ };
							ListViewItem i_ = new ListViewItem(row_str_, 1);
							ECRlistView.Items.Add(i_);
						}
					}
				}
			}

			ECRlistView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
		}

		private void LookUpECR(int eco) {
			if (eco > Redbrick.LastLegacyECR) {
				using (ECRViewerDataSetTableAdapters.ECRObjLookupTableAdapter ta_ =
					new ECRViewerDataSetTableAdapters.ECRObjLookupTableAdapter()) {
					using (ECRViewerDataSet.ECRObjLookupDataTable dt_ = ta_.GetDataByECO(eco)) {
						if (dt_.Count > 0) {
							ECRlistView.Items.Clear();
							originalText = ECRTextBox.Text;
							foreach (ECRViewerDataSet.ECRObjLookupRow row in dt_.Rows) {
								string[] row_str_ = new string[] { row.ECR_NUM.ToString(),
								row.DATE_CREATE.ToString(dateFormat),
								Redbrick.TitleCase(row.STATUS) };
								ListViewItem i_ = new ListViewItem(row_str_, 1);
								ECRlistView.Items.Add(i_);
							}
							Text = @"ECR Viewer";
							ECRlistView.Items[0].Selected = true;
						} else {
							ECRTextBox.Text = originalText;
						}
					}
				}
			} else {
				using (ECRViewerDataSetTableAdapters.LegacyECRObjLookupTableAdapter ta_ =
					new ECRViewerDataSetTableAdapters.LegacyECRObjLookupTableAdapter()) {
					using (ECRViewerDataSet.LegacyECRObjLookupDataTable dt_ = ta_.GetDataByECO(eco.ToString())) {
						if (dt_.Count > 0) {
							ECRlistView.Items.Clear();
							originalText = ECRTextBox.Text;
							foreach (ECRViewerDataSet.LegacyECRObjLookupRow r_ in dt_.Rows) {
								string holder_ = !r_.IsHolderNull() ? Redbrick.TitleCase(r_.Holder) : @"??";
								string req_ = !r_.IsDateRequestedNull() ? r_.DateRequested.ToString(dateFormat) : @"??";
								string[] row_str_ = new string[] { string.Format(@"{0} (Legacy)", eco),
									req_, holder_ };
								ListViewItem i_ = new ListViewItem(row_str_, 1);
								ECRlistView.Items.Add(i_);
							}
							Text = @"ECR Viewer";
							ECRlistView.Items[0].Selected = true;
						} else {
							ECRTextBox.Text = originalText;
						}
					}
				}
			}
			ECRlistView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
		}

		private ECRViewerDataSet.SigneesDataTable signeesRows(int ecr_) {
			using (ECRViewerDataSetTableAdapters.SigneesTableAdapter ta_ =
				new ECRViewerDataSetTableAdapters.SigneesTableAdapter()) {
				return ta_.GetDataByECRNum(ecr_);
			}
		}

		private ECRViewerDataSet.ECRObjLookupRow ECRObjLookup(int ecr_) {
			using (ECRViewerDataSetTableAdapters.ECRObjLookupTableAdapter ta_ =
				new ECRViewerDataSetTableAdapters.ECRObjLookupTableAdapter()) {
				using (ECRViewerDataSet.ECRObjLookupDataTable dt_ = ta_.GetDataByECO(ecr_)) {
					if (dt_.Count > 0) {
						return dt_[0];
					}
				}
			}
			return null;
		}

		private ECRViewerDataSet.ECR_LEGACYRow LegacyECRObjLookup(int ecr_) {
			using (ECRViewerDataSetTableAdapters.ECR_LEGACYTableAdapter ta_ =
				new ECRViewerDataSetTableAdapters.ECR_LEGACYTableAdapter()) {
				using (ECRViewerDataSet.ECR_LEGACYDataTable dt_ = ta_.GetDataByECO(ecr_.ToString())) {
					if (dt_.Count > 0) {
						return dt_[0];
					}
				}
			}
			return null;
		}

		private ECRViewerDataSet.ECR_ITEMSDataTable eCR_ITEMSRows(int ecr_) {
			using (ECRViewerDataSetTableAdapters.ECR_ITEMSTableAdapter ta_ =
				new ECRViewerDataSetTableAdapters.ECR_ITEMSTableAdapter()) {
				return ta_.GetDataByECRNo(ecr_);
			}
		}

		private	ECRViewerDataSet.ECR_DRAWINGSDataTable eCR_DRAWINGSRows(int itemid) {
			using (ECRViewerDataSetTableAdapters.ECR_DRAWINGSTableAdapter ta_ =
				new ECRViewerDataSetTableAdapters.ECR_DRAWINGSTableAdapter()) {
				return ta_.GetDataByItemID(itemid);
			}
		}

		private	ECRViewerDataSet.FILE_MAINDataTable fILE_MAINRows(int ecr_) {
			using (ECRViewerDataSetTableAdapters.FILE_MAINTableAdapter ta_ =
				new ECRViewerDataSetTableAdapters.FILE_MAINTableAdapter()) {
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
				if (!(new System.IO.FileInfo(drawings[idx]).Exists)) {
					string msg_ = string.Format(@"'{0}' was in the database, but I couldn't find it. This could happen if a) the file was deleted, or b) you don't have the network drives correctly mapped. (Running SolidWorks could have this effect.", drawings[idx]);
					MessageBox.Show(this, msg_, @"Couldn't find the drawing.", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				System.Diagnostics.Process.Start(drawings[idx]);
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
					drawings.Add(string.Format(@"\\AMSTORE-SVR-02\shared\shared\general\Engineering Utility\ECR Drawings\{0}", it_[2]));
					if (affectedDrawingsListView.Items.Count > 0) {
						affectedDrawingsListView.Items[0].Selected = true;
					}
				} else {
					using (ECRViewerDataSetTableAdapters.GEN_DRAWINGSTableAdapter ta_ =
						new ECRViewerDataSetTableAdapters.GEN_DRAWINGSTableAdapter()) {
						using (ECRViewerDataSet.GEN_DRAWINGSDataTable dt_ =
							ta_.GetAnyDrawingByItemSearch(string.Format(@"{0}.PDF", lv_.SelectedItems[0].SubItems[0].Text))) {
							foreach (ECRViewerDataSet.GEN_DRAWINGSRow row_ in dt_.Rows) {
								string p_ = string.Format(@"{0}{1}", row_.FPath, row_.FName);
								int strt_ = p_.LastIndexOf("\\");
								string[] it_ = new string[] {
									string.Format(@"{0}...{1}", p_.Substring(0, 3), p_.Substring(strt_, p_.Length - strt_)),
									row_.DateCreated.ToString(dateFormat)
								};
								affectedDrawingsListView.Items.Add(new ListViewItem(it_, 6));
								drawings.Add(p_);
							}
						}
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

		private void GetDescription(int ecrno) {
			Font font = descriptionRichTextBox.Font;
			Font newfont = new Font(font, FontStyle.Italic | FontStyle.Underline);
			if (ecrno > Redbrick.LastLegacyECR) {
				ECRViewerDataSet.ECRObjLookupRow r_ = ECRObjLookup(ecrno);
				if (r_ != null) {
					descriptionRichTextBox.Clear();
					if (!r_.IsCHANGESNull()) {
						descriptionRichTextBox.SelectionFont = newfont;
						descriptionRichTextBox.SelectedText = string.Format(@"Requested Changes:{0}", Environment.NewLine);
						descriptionRichTextBox.SelectionFont = font;
						descriptionRichTextBox.AppendText(r_.CHANGES);
					}
					if (r_.IsREVISIONNull()) {
						return;
					}
					descriptionRichTextBox.AppendText(Environment.NewLine);
					descriptionRichTextBox.SelectionFont = newfont;
					descriptionRichTextBox.SelectedText = string.Format(@"{0}Completed changes and notes:{0}", Environment.NewLine);
					descriptionRichTextBox.SelectionFont = font;
					descriptionRichTextBox.AppendText(r_.REVISION);
				}
			} else {
				ECRViewerDataSet.ECR_LEGACYRow r_ = LegacyECRObjLookup(ecrno);
				if (r_ != null && !r_.IsChangeNull()) {
					descriptionRichTextBox.Clear();
					descriptionRichTextBox.SelectionFont = font;
					descriptionRichTextBox.AppendText(r_.Change);
				}
			}
		}

		private void GetECRItems(int ecrno) {
			if (ecrno > Redbrick.LastLegacyECR) {
				foreach (ECRViewerDataSet.ECR_ITEMSRow row in eCR_ITEMSRows(ecrno)) {
					string[] row_str_ = new string[] { row.ITEMNUMBER, row.ITEMREV, Redbrick.TitleCase(row.TypeName) };
					ListViewItem l_ = new ListViewItem(row_str_, 5);
					foreach (ECRViewerDataSet.ECR_DRAWINGSRow drw in eCR_DRAWINGSRows(row.ITEM_ID)) {
						l_.SubItems.Add(new ListViewItem.ListViewSubItem(l_, drw.ORIG_PATH));
						l_.SubItems.Add(new ListViewItem.ListViewSubItem(l_, drw.DRWREV));
						l_.SubItems.Add(new ListViewItem.ListViewSubItem(l_, drw.DRW_FILE));
					}
					if (row.ITEMNUMBER == Lookup) {
						l_.BackColor = System.Drawing.Color.Yellow;
					}
					items.Add(row.ITEM_ID);
					affectedItemsListView.Items.Add(l_);
				}
			} else {
				ECRViewerDataSet.ECR_LEGACYRow r_ = LegacyECRObjLookup(ecrno);
				if (r_ != null) {
					foreach (string prt_ in r_.AffectedParts.Split(new string[] { @",", @"/" }, StringSplitOptions.RemoveEmptyEntries)) {
						if (prt_.Trim() != string.Empty) {
							string[] row_str_ = new string[] { prt_.Trim(), @"???", @"Unknown" };
							ListViewItem l_ = new ListViewItem(row_str_, 5);
							affectedItemsListView.Items.Add(l_);
						}
					}
					string name_ = !r_.IsEngineerNull() ? Redbrick.TitleCase(r_.Engineer): "??";
					string status_ = !r_.IsHolderNull() ? Redbrick.TitleCase(r_.Holder) : "??";
					string started = !r_.IsDateStartedNull() ? r_.DateStarted.ToString(dateFormat) : "??";
					string req_ = !r_.IsDateRequestedNull() ? r_.DateRequested.ToString(dateFormat) : "??";
					string date_ = !r_.IsDateCompletedNull() ? r_.DateCompleted.ToString(dateFormat) : "??";
					string comment_ = string.Format("Requested: {0}.{4}Started: {1}{4}Finished on {2} by {3}",
						req_, started, date_, name_, Environment.NewLine);
					string[] signee_row_ = new string[] { name_, status_, date_, comment_ };
					signeesListView.Items.Add(new ListViewItem(signee_row_, 3));
				}
			}
		}

		private void ToggleControls(bool on) {
			//groupBox5.Enabled = on;
			groupBox6.Enabled = on;
			attachedFilesListView.Enabled = on;
			//splitContainer1.Enabled = on;
			//signeesListView.Enabled = on;
			//signeesTextBox.Enabled = on;
			affectedDrawingsListView.Columns[1].Text = on ? @"LVL" : @"Created";
			signeesListView.Columns[1].Text = on ? @"Status" : @"Holder";
			groupBox5.Text = on ? @"Signees" : @"Engineering";
		}

		private void GetSignees(int ecrno) {
			if (ecrno > Redbrick.LastLegacyECR) {
				foreach (ECRViewerDataSet.SigneesRow row in signeesRows(ecrno)) {
					string signoff_ = string.Empty;
					string comment_ = string.Empty;
					if (!row.IsSIGNOFFNull()) {
						signoff_ = row.SIGNOFF.ToString(dateFormat);
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
			}

			if (signeesListView.Items.Count > 0) {
				signeesListView.Items[0].Selected = true;
			}
		}

		private void ListView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {
			ListView lv_ = sender as ListView;
			if (lv_.SelectedItems.Count > 0 && lv_.SelectedItems[0] != null) {
				int ecrno = Convert.ToInt32(e.Item.SubItems[0].Text.Split(' ')[0]);
				ToggleControls(!e.Item.SubItems[0].Text.Contains(@"Legacy"));
				ECRTextBox.Text = ecrno.ToString();
				originalText = ecrno.ToString();

				GetDescription(ecrno);

				affectedItemsListView.Items.Clear();
				affectedDrawingsListView.Items.Clear();
				signeesListView.Items.Clear();
				attachedFilesListView.Items.Clear();
				items.Clear();
				drawings.Clear();
				signeesTextBox.Clear();

				GetECRItems(ecrno);

				if (affectedItemsListView.Items.Count > 0) {
					affectedItemsListView.Items[0].Selected = true;
				}

				GetSignees(ecrno);

				foreach (ECRViewerDataSet.FILE_MAINRow row in fILE_MAINRows(ecrno)) {
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
			WindowState = FormWindowState.Minimized;
			Show();
			WindowState = FormWindowState.Normal;
		}

		private void ECRViewer_FormClosing(object sender, FormClosingEventArgs e) {
			Properties.Settings.Default.ECRViewerLocation = Location;
			Properties.Settings.Default.ECRViewerSize = Size;
			Properties.Settings.Default.Save();
		}

		private void ECRTextBox_KeyDown(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Enter) {
				Lookup = ECRTextBox.Text;
				LookupItem(Lookup);
			}
			if (e.KeyCode == Keys.Up && int.TryParse(ECRTextBox.Text, out int _ecrup)) {
				Lookup = (++_ecrup).ToString();
				LookupItem(Lookup);
			}
			if (e.KeyCode == Keys.Down && int.TryParse(ECRTextBox.Text, out int _ecrdown)) {
				Lookup = (--_ecrdown).ToString();
				LookupItem(Lookup);
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
