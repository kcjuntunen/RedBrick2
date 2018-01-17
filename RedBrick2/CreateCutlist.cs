using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Linq;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
	/// <summary>
	/// This is the form for collecting and inserting all property data into the cutlist.
	/// </summary>
	public partial class CreateCutlist : Form {
		private ENGINEERINGDataSetTableAdapters.CUT_PART_TYPESTableAdapter cpt =
			new ENGINEERINGDataSetTableAdapters.CUT_PART_TYPESTableAdapter();
		private ENGINEERINGDataSetTableAdapters.CUT_MATERIALSTableAdapter cmt =
			new ENGINEERINGDataSetTableAdapters.CUT_MATERIALSTableAdapter();
		private ENGINEERINGDataSetTableAdapters.FriendlyCutOpsTableAdapter fco =
			new ENGINEERINGDataSetTableAdapters.FriendlyCutOpsTableAdapter();
		private ENGINEERINGDataSetTableAdapters.CUT_EDGESTableAdapter ce =
			new ENGINEERINGDataSetTableAdapters.CUT_EDGESTableAdapter();
		private ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter guta =
			new ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter();

		private ENGINEERINGDataSetTableAdapters.CUT_CUTLIST_PARTSTableAdapter ta_ccp =
			new ENGINEERINGDataSetTableAdapters.CUT_CUTLIST_PARTSTableAdapter();
		private ENGINEERINGDataSetTableAdapters.CUT_PARTSTableAdapter ta_cp =
			new ENGINEERINGDataSetTableAdapters.CUT_PARTSTableAdapter();
		private ENGINEERINGDataSetTableAdapters.CUT_PART_OPSTableAdapter ta_cpo =
			new ENGINEERINGDataSetTableAdapters.CUT_PART_OPSTableAdapter();
		private ENGINEERINGDataSetTableAdapters.CUT_CUTLISTSTableAdapter ta_cc =
		new ENGINEERINGDataSetTableAdapters.CUT_CUTLISTSTableAdapter();

		private ENGINEERINGDataSet.CUT_CUTLIST_PARTSDataTable dt_ccp =
			new ENGINEERINGDataSet.CUT_CUTLIST_PARTSDataTable();
		private ENGINEERINGDataSet.CUT_PARTSDataTable dt_cp =
			new ENGINEERINGDataSet.CUT_PARTSDataTable();
		private ENGINEERINGDataSet.CUT_PART_OPSDataTable dt_cpo =
			new ENGINEERINGDataSet.CUT_PART_OPSDataTable();
		private ENGINEERINGDataSet.CUT_CUTLISTSDataTable dt_cc =
			new ENGINEERINGDataSet.CUT_CUTLISTSDataTable();

		private SortedDictionary<string, int> _dict = new SortedDictionary<string, int>();
		private SortedDictionary<string, SwProperties> _partlist = new SortedDictionary<string, SwProperties>();
		private SldWorks _swApp;

		private FileInfo PartFileInfo;
		private string topName = string.Empty;
		private string partLookup = string.Empty;
		private string selectedPart = string.Empty;
		private string sourceComp = string.Empty;
		private FileInfo foundPDF;
		private string _revFromFile = string.Empty;
		private string _revFromProperties = string.Empty;
		private int clid = 0;
		private string rev = @"100";
		private bool rev_changed_by_user = false;
		private bool rev_in_filename = false;
		private bool user_changed_item = false;
		private Configuration _config = null;
		private int? uid = null;
		private bool[] sort_directions = { false, false, false, false, false, false,
																			 false, false, false, false, false, false,
																			 false, false, false, false, false, false,
																			 false, false, false, false, false, false };
		private bool ok = false;
		private string itm = string.Empty;
		private string descr = string.Empty;
		private string refr = string.Empty;
		private ToolTip rev_tooltip = new ToolTip();
		private ToolTip descr_tooltip = new ToolTip();
		private ToolTip cust_tooltip = new ToolTip();
		private ToolTip item_tooltip = new ToolTip();
		private ToolTip drw_tooltip = new ToolTip();

		private UserProgressBar pb;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="s">The connected application.</param>
		public CreateCutlist(SldWorks s) {
			_swApp = s;
			InitializeComponent();
			dataGridView1.DataError += dataGridView1_DataError;
			dataGridView1.UserDeletedRow += DataGridView1_UserDeletedRow;
			ConfigurationManager swConfMgr;
			Configuration swConf;
			Component2 swRootComp;
			ModelDoc2 m = _swApp.ActiveDoc;

			topName = Path.GetFileNameWithoutExtension(m.GetPathName());
			rev_in_filename = topName.Contains(@"REV");

			_config = m.GetActiveConfiguration();
			swConfMgr = (ConfigurationManager)m.ConfigurationManager;
			swConf = (Configuration)swConfMgr.ActiveConfiguration;
			if (_swApp.ActiveDoc is DrawingDoc) {
				SolidWorks.Interop.sldworks.View _v = GetSelectedView(m);
				if (_v == null) {
					_v = Redbrick.GetFirstView(_swApp);
				}
				m = _v.ReferencedDocument;
				sourceComp = _v.Name;
				_config = m.GetConfigurationByName(_v.ReferencedConfiguration);
				swConf = _config;
				Text = string.Format(@"{0} - {1} (from {2})", topName, _config.Name, sourceComp);
			} else {
				Text = string.Format(@"{0} - {1}", topName, _config.Name);
			}

			swRootComp = (Component2)swConf.GetRootComponent();

			PartFileInfo = new FileInfo(m.GetPathName());
			partLookup = Redbrick.FileInfoToLookup(PartFileInfo);

			//TraverseModelFeatures(m, 1);
			_swApp.GetUserProgressBar(out pb);
			Cursor.Current = Cursors.WaitCursor;
			if (m is AssemblyDoc) {
				TraverseComponent(swRootComp, 1);
			}

			if (m is PartDoc) {
				GetPart(m);
			}
			//dataGridView1.DataSource = ToDataTable(_dict, _partlist);
			pb.End();
			Cursor.Current = Cursors.Default;
			AddColumns();
			FillTable(_dict, _partlist);
			count_includes();
		}

		private void DataGridView1_UserDeletedRow(object sender, DataGridViewRowEventArgs e) {
			count_includes();
		}

		void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e) {
			e.ThrowException = false;
			System.Diagnostics.Debug.Print(e.Exception.Message);
		}

		private SolidWorks.Interop.sldworks.View GetSelectedView(ModelDoc2 m) {
			SelectionMgr sm_ = m.SelectionManager;
			if (sm_ != null) {
				object v_ = sm_.GetSelectedObject6(1, -1);
				if (v_ is SolidWorks.Interop.sldworks.View) {
					return v_ as SolidWorks.Interop.sldworks.View;
				}
			}
			return null;
		}

		private void ToggleRevWarn(bool on) {
			if (on) {
				Redbrick.Err(rev_cbx);
				rev_tooltip.SetToolTip(rev_cbx, Properties.Resources.RevisionNotMatching);
				rev_tooltip.SetToolTip(label6, Properties.Resources.RevisionNotMatching);
			} else {
				Redbrick.UnErr(rev_cbx as ComboBox);
				rev_tooltip.RemoveAll();
			}
		}

		private void ToggleCustomerWarn(bool on) {
			if (on) {
				Redbrick.Err(cust_cbx);
				cust_tooltip.SetToolTip(cust_cbx, Properties.Resources.CustomerNotMatching);
				cust_tooltip.SetToolTip(label1, Properties.Resources.CustomerNotMatching);
			} else {
				Redbrick.UnErr(cust_cbx);
				cust_tooltip.RemoveAll();
			}
		}

		private void ToggleDescrWarn(bool on) {
			if (on) {
				Redbrick.Err(descr_cbx);
				descr_tooltip.SetToolTip(descr_cbx, Properties.Resources.NoDescriptionWarning);
				descr_tooltip.SetToolTip(label3, Properties.Resources.NoDescriptionWarning);
			} else {
				Redbrick.UnErr(descr_cbx);
				descr_tooltip.RemoveAll();
			}
		}

		private void CreateCutlist_Load(object sender, EventArgs e) {
			// TODO: This line of code loads data into the 'eNGINEERINGDataSet.CUT_PART_TYPES' table. You can move, or remove it, as needed.
			this.cUT_PART_TYPESTableAdapter.Fill(this.eNGINEERINGDataSet.CUT_PART_TYPES);
			Location = Properties.Settings.Default.CreateCutlistLocation;
			Size = Properties.Settings.Default.CreateCutlistSize;
			// TODO: This line of code loads data into the 'eNGINEERINGDataSet.CUT_CUTLISTS' table. You can move, or remove it, as needed.
			//this.cUT_CUTLISTSTableAdapter.Fill(this.eNGINEERINGDataSet.CUT_CUTLISTS);
			// TODO: This line of code loads data into the 'eNGINEERINGDataSet.GEN_CUSTOMERS' table. You can move, or remove it, as needed.
			this.gEN_CUSTOMERSTableAdapter.Fill(this.eNGINEERINGDataSet.GEN_CUSTOMERS);
			uid = guta.GetUID(System.Environment.UserName);

			if (Properties.Settings.Default.OnlyCurrentCustomers) {
				gENCUSTOMERSBindingSource.Filter = @"CUSTACTIVE = True";
			}
			this.revListTableAdapter.Fill(this.eNGINEERINGDataSet.RevList);

			ENGINEERINGDataSet.SCH_PROJECTSRow spr = (new ENGINEERINGDataSet.SCH_PROJECTSDataTable()).GetCorrectCustomer(partLookup);
			if (spr != null) {
				cust_cbx.SelectedValue = spr.CUSTID;
			} else {
				CustomerProperty _cp = new CustomerProperty(@"CUSTOMER", true, _swApp, _swApp.ActiveDoc);
				_cp.Get();
				cust_cbx.SelectedIndex = cust_cbx.FindString(_cp.Value.Split('-')[0].Trim());
			}
			dateTimePicker1.Value = DateTime.Now;
			settle_rev(topName);
			get_names();
		}

		private void check_ok() {
			ok = (itm_cbx.Text != string.Empty &&
						descr_cbx.Text != string.Empty &&
						ref_cbx.Text != string.Empty &&
						cust_cbx.SelectedItem != null &&
						uid != null);
			upload_btn.Enabled = ok;
		}

		private void get_names() {
			StringProperty stpr = null;
			if (_swApp.ActiveDoc is DrawingDoc) {
				SolidWorks.Interop.sldworks.View _v = Redbrick.GetFirstView(_swApp);
				stpr = new StringProperty(@"Description", true, _swApp, _v.ReferencedDocument, string.Empty);
			} else {
				stpr = new StringProperty(@"Description", true, _swApp, _swApp.ActiveDoc, string.Empty);
			}

			stpr.Get();

			descr_cbx.Text = stpr.Value;
			if (descr_cbx.Text == string.Empty) {
				ToggleDescrWarn(true);
			}
			itm_cbx.Text = topName;
			if (rev_in_filename) {
				ref_cbx.Text = string.Format(@"{0} REV {1}", topName, rev);
			} else {
				ref_cbx.Text = topName;
			}

		}

		private void settle_rev(string pnwe) {
			string[] strings = pnwe.Split(new string[] { @"REV" }, StringSplitOptions.RemoveEmptyEntries);
			topName = strings[0];
			if (rev_in_filename) {
				_revFromFile = strings[1].Trim();
			}

			SwProperty _s = new SwProperty(@"REVISION LEVEL", true, _swApp, _swApp.ActiveDoc);
			_s.Get();
			_revFromProperties = _s.Value;
			rev = _revFromProperties;

			if (_revFromFile != string.Empty && _revFromFile != _revFromProperties) {
				Redbrick.Err(rev_cbx);
				rev_tooltip.SetToolTip(rev_cbx, Properties.Resources.RevisionNotMatching);
			}

			set_rev(rev);
		}

		private void set_rev(string r) {
			int idx = rev_cbx.FindString(r);
			if (idx > -1) {
				rev_cbx.SelectedIndex = idx;
			} else {
				rev_cbx.Text = r;
			}
		}

		private void count_includes() {
			int x = 0;
			foreach (DataGridViewRow row in dataGridView1.Rows) {
				DataGridViewCheckBoxCell cell_ = row.Cells[@"Include"] as DataGridViewCheckBoxCell;
				if (Convert.ToBoolean(cell_.EditedFormattedValue)) {
					x++;
				}
			}
			toolStripStatusLabel1.Text = string.Format(@"Total Unique Parts: {0}", dataGridView1.Rows.Count - 1);
			toolStripStatusLabel2.Text = string.Format(@"Included Parts: {0}", x);
		}

		private void GetPart(ModelDoc2 m) {
			pb.Start(0, 1, @"Enumerating parts...");
			string name = Path.GetFileNameWithoutExtension(m.GetPathName());
			SwProperties s = new SwProperties(_swApp);
			s.Configuration = _config.Name;
			s.GetProperties(m);
			s.PartFileInfo = new FileInfo(m.GetPathName());
			itm_cbx.Text = partLookup;
			ref_cbx.Text = partLookup;
			descr_cbx.Text = s[@"Description"].Value;
			_dict.Add(name, 1);
			_partlist.Add(name, s);
			pb.UpdateTitle(m.GetTitle());
			pb.UpdateProgress(1);
		}

		private void TraverseComponent(Component2 swComp, long nLevel) {
			int pos = 0;
			object[] vChildComp;
			Component2 swChildComp;
			string sPadStr = " ";
			long i = 0;
			ModelDoc2 m = swComp.GetModelDoc2();
			//Text = string.Format(@"{0} - {1}", topName, m.GetActiveConfiguration().Name);

			for (i = 0; i <= nLevel - 1; i++) {
				sPadStr = sPadStr + " ";
			}

			vChildComp = (object[])swComp.GetChildren();
			if (nLevel == 1) {
				pb.Start(0, vChildComp.Length, @"Enumerating parts...");
			}
			for (i = 0; i < vChildComp.Length; i++) {
				swChildComp = (Component2)vChildComp[i];
				//Debug.Print("comp" + sPadStr + "+" + swChildComp.Name2 + " <" + swChildComp.ReferencedConfiguration + ">");
				FileInfo fi_;
				string name = swChildComp.Name2.Substring(0, swChildComp.Name2.LastIndexOf('-'));
				if (name.Contains("/")) {
					name = name.Substring(name.LastIndexOf('/') + 1);
				}

				ModelDoc2 md = (swChildComp.GetModelDoc2() as ModelDoc2);
				if (md != null && md.GetType() == (int)swDocumentTypes_e.swDocPART) {
					fi_ = new FileInfo(md.GetPathName());
					name = Redbrick.FileInfoToLookup(fi_);
					pb.UpdateTitle(name);
					SwProperties s = new SwProperties(_swApp, md);
					s.Configuration = swChildComp.ReferencedConfiguration;
					ConfigurationManager cm_ = md.ConfigurationManager;
					s.GetProperties(md);
					if (!_dict.ContainsKey(name)) {
						_dict.Add(name, 1);
						s.PartFileInfo = new FileInfo(md.GetPathName());
						_partlist.Add(name, s);
					} else {
						_dict[name] = _dict[name] + 1;
						//_partlist[name][@"BLANK QTY"].Data = _dict[name];
					}
					if (nLevel == 1) {
						pb.UpdateProgress(++pos);
					}
					//if (!x.ContainsKey(name)) {
					//  x.Add(name, swChildComp);
					//  types.Add(name, (swDocumentTypes_e)(swChildComp.GetModelDoc2() as ModelDoc2).GetType());
					//  qty.Add(name, 1);
					//} else {
					//  qty[name] = qty[name] + 1;
					//}
				}

				//TraverseComponentFeatures(swChildComp, nLevel);
				TraverseComponent(swChildComp, nLevel + 1);
			}
		}

		private void TraverseModelFeatures(ModelDoc2 swModel, long nLevel) {
			Feature swFeat;

			swFeat = (Feature)swModel.FirstFeature();
			TraverseFeatureFeatures(swFeat, nLevel);
		}

		private void TraverseFeatureFeatures(Feature swFeat, long nLevel) {
			Feature swSubFeat;
			Feature swSubSubFeat;
			Feature swSubSubSubFeat;
			string sPadStr = " ";
			long i = 0;

			for (i = 0; i <= nLevel; i++) {
				sPadStr = sPadStr + " ";
			}
			while ((swFeat != null)) {
				//Debug.Print("0" + sPadStr + swFeat.Name + " [" + swFeat.GetTypeName2() + "]");
				swSubFeat = (Feature)swFeat.GetFirstSubFeature();

				while ((swSubFeat != null)) {
					//Debug.Print("1" + sPadStr + "  " + swSubFeat.Name + " [" + swSubFeat.GetTypeName() + "]");
					swSubSubFeat = (Feature)swSubFeat.GetFirstSubFeature();

					while ((swSubSubFeat != null)) {
						//Debug.Print("2" + sPadStr + "    " + swSubSubFeat.Name + " [" + swSubSubFeat.GetTypeName() + "]");
						swSubSubSubFeat = (Feature)swSubSubFeat.GetFirstSubFeature();

						while ((swSubSubSubFeat != null)) {
							//Debug.Print("3" + sPadStr + "      " + swSubSubSubFeat.Name + " [" + swSubSubSubFeat.GetTypeName() + "]");
							swSubSubSubFeat = (Feature)swSubSubSubFeat.GetNextSubFeature();

						}

						swSubSubFeat = (Feature)swSubSubFeat.GetNextSubFeature();

					}

					swSubFeat = (Feature)swSubFeat.GetNextSubFeature();

				}

				swFeat = (Feature)swFeat.GetNextFeature();

			}

		}

		private ENGINEERINGDataSet.CUT_PARTSDataTable Translate(DataTable dt) {
			ENGINEERINGDataSet.CUT_PARTSDataTable outdt = new ENGINEERINGDataSet.CUT_PARTSDataTable();
			//ENGINEERINGDataSet.CUT_CUTLIST_PARTSDataTable outdt = new ENGINEERINGDataSet.CUT_CUTLIST_PARTSDataTable();
			for (int i = 0; i < dt.Rows.Count; i++) {
				DataRow row = dt.Rows[i];
				if ((bool)row[@"Include"]) {
					SwProperties p = _partlist[row["Part Number"].ToString()];
					ENGINEERINGDataSet.CUT_PARTSRow r = (ENGINEERINGDataSet.CUT_PARTSRow)outdt.NewRow();
					r.BLANKQTY = (int)row["Blank Qty"];
					r.CNC1 = (string)p[@"CNC1"].Data;
					r.CNC2 = (string)p[@"CNC2"].Data;
					r.COMMENT = (string)p["COMMENT"].Data;
					r.DESCR = row[@"Description"].ToString();
					r.FIN_L = (float)p[@"LENGTH"].Data;
					r.FIN_W = (float)p[@"WIDTH"].Data;
					r.THICKNESS = (float)p[@"THICKNESS"].Data;
					r.HASH = p[@"DEPARTMART"].Hash;
				}
			}
			return outdt;
		}

		private void AddColumns() {
			DataGridViewColumn part_number = new DataGridViewColumn();
			part_number.Name = @"Part Number";
			part_number.CellTemplate = new DataGridViewTextBoxCell();
			part_number.ValueType = typeof(string);
			part_number.SortMode = DataGridViewColumnSortMode.Programmatic;

			DataGridViewColumn descr = new DataGridViewColumn();
			descr.Name = @"Description";
			descr.CellTemplate = new DataGridViewTextBoxCell();
			descr.ValueType = typeof(string);
			descr.SortMode = DataGridViewColumnSortMode.Programmatic;

			DataGridViewColumn part_qty = new DataGridViewColumn();
			part_qty.Name = @"Part Qty";
			part_qty.CellTemplate = new DataGridViewTextBoxCell();
			part_qty.ValueType = typeof(int);
			part_qty.Width = 50;
			part_qty.SortMode = DataGridViewColumnSortMode.Programmatic;

			DataGridViewComboBoxColumn mat = new DataGridViewComboBoxColumn();
			mat.Name = @"Material";
			mat.CellTemplate = new DataGridViewComboBoxCell();
			mat.HeaderText = @"Material";
			mat.DropDownWidth = 250;
			mat.DisplayMember = @"DESCR";
			mat.ValueMember = @"MATID";
			mat.AutoComplete = true;
			mat.DataSource = cmt.GetData();
			mat.SortMode = DataGridViewColumnSortMode.Programmatic;
			mat.FlatStyle = FlatStyle.Popup;

			DataGridViewColumn length = new DataGridViewColumn();
			length.Name = @"L";
			length.CellTemplate = new DataGridViewTextBoxCell();
			length.ValueType = typeof(double);
			length.SortMode = DataGridViewColumnSortMode.Programmatic;

			DataGridViewColumn width = new DataGridViewColumn();
			width.Name = @"W";
			width.CellTemplate = new DataGridViewTextBoxCell();
			width.ValueType = typeof(double);
			width.SortMode = DataGridViewColumnSortMode.Programmatic;

			DataGridViewColumn thickness = new DataGridViewColumn();
			thickness.Name = @"T";
			thickness.CellTemplate = new DataGridViewTextBoxCell();
			thickness.ValueType = typeof(double);
			thickness.SortMode = DataGridViewColumnSortMode.Programmatic;

			DataGridViewColumn blnk_qty = new DataGridViewColumn();
			blnk_qty.Name = @"Blank Qty";
			blnk_qty.CellTemplate = new DataGridViewTextBoxCell();
			blnk_qty.ValueType = typeof(int);
			blnk_qty.Width = 50;
			blnk_qty.SortMode = DataGridViewColumnSortMode.Programmatic;

			DataGridViewColumn overl = new DataGridViewColumn();
			overl.Name = @"Over L";
			overl.CellTemplate = new DataGridViewTextBoxCell();
			overl.ValueType = typeof(double);
			overl.Width = 50;
			overl.SortMode = DataGridViewColumnSortMode.Programmatic;

			DataGridViewColumn overw = new DataGridViewColumn();
			overw.Name = @"Over W";
			overw.CellTemplate = new DataGridViewTextBoxCell();
			overw.ValueType = typeof(double);
			overw.Width = 50;
			overw.SortMode = DataGridViewColumnSortMode.Programmatic;

			DataGridViewColumn cnc1 = new DataGridViewColumn();
			cnc1.Name = @"CNC 1";
			cnc1.CellTemplate = new DataGridViewTextBoxCell();
			cnc1.ValueType = typeof(string);
			cnc1.SortMode = DataGridViewColumnSortMode.Programmatic;

			DataGridViewColumn cnc2 = new DataGridViewColumn();
			cnc2.Name = @"CNC 2";
			cnc2.CellTemplate = new DataGridViewTextBoxCell();
			cnc2.ValueType = typeof(string);
			cnc2.SortMode = DataGridViewColumnSortMode.Programmatic;

			DataGridViewComboBoxColumn op1 = new DataGridViewComboBoxColumn();
			op1.Name = @"Op 1";
			op1.CellTemplate = new DataGridViewComboBoxCell();
			op1.HeaderText = @"Op 1";
			op1.DropDownWidth = 200;
			op1.DisplayMember = @"OPNAME";
			op1.ValueMember = @"OPID";
			op1.AutoComplete = true;
			op1.DataSource = fco.GetData();
			op1.ValueType = typeof(int);
			op1.SortMode = DataGridViewColumnSortMode.Programmatic;
			op1.FlatStyle = FlatStyle.Popup;

			DataGridViewComboBoxColumn op2 = new DataGridViewComboBoxColumn();
			op2.Name = @"Op 2";
			op2.CellTemplate = new DataGridViewComboBoxCell();
			op2.HeaderText = @"Op 2";
			op2.DropDownWidth = 200;
			op2.DisplayMember = @"OPNAME";
			op2.ValueMember = @"OPID";
			op2.AutoComplete = true;
			op2.DataSource = fco.GetData();
			op2.ValueType = typeof(int);
			op2.SortMode = DataGridViewColumnSortMode.Programmatic;
			op2.FlatStyle = FlatStyle.Popup;

			DataGridViewComboBoxColumn op3 = new DataGridViewComboBoxColumn();
			op3.Name = @"Op 3";
			op3.CellTemplate = new DataGridViewComboBoxCell();
			op3.HeaderText = @"Op 3";
			op3.DropDownWidth = 200;
			op3.DisplayMember = @"OPNAME";
			op3.ValueMember = @"OPID";
			op3.AutoComplete = true;
			op3.DataSource = fco.GetData();
			op3.ValueType = typeof(int);
			op3.SortMode = DataGridViewColumnSortMode.Programmatic;
			op3.FlatStyle = FlatStyle.Popup;

			DataGridViewComboBoxColumn op4 = new DataGridViewComboBoxColumn();
			op4.Name = @"Op 4";
			op4.CellTemplate = new DataGridViewComboBoxCell();
			op4.HeaderText = @"Op 4";
			op4.DropDownWidth = 200;
			op4.DisplayMember = @"OPNAME";
			op4.ValueMember = @"OPID";
			op4.AutoComplete = true;
			op4.DataSource = fco.GetData();
			op4.ValueType = typeof(int);
			op4.SortMode = DataGridViewColumnSortMode.Programmatic;
			op4.FlatStyle = FlatStyle.Popup;

			DataGridViewComboBoxColumn op5 = new DataGridViewComboBoxColumn();
			op5.Name = @"Op 5";
			op5.CellTemplate = new DataGridViewComboBoxCell();
			op5.HeaderText = @"Op 5";
			op5.DropDownWidth = 200;
			op5.DisplayMember = @"OPNAME";
			op5.ValueMember = @"OPID";
			op5.AutoComplete = true;
			op5.DataSource = fco.GetData();
			op5.ValueType = typeof(int);
			op5.SortMode = DataGridViewColumnSortMode.Programmatic;
			op5.FlatStyle = FlatStyle.Popup;

			DataGridViewComboBoxColumn dpt_col = new DataGridViewComboBoxColumn();
			dpt_col.Name = @"Department";
			dpt_col.CellTemplate = new DataGridViewComboBoxCell();
			dpt_col.HeaderText = @"Department";
			dpt_col.DropDownWidth = 150;
			dpt_col.DisplayMember = @"TYPEDESC";
			dpt_col.ValueMember = @"TYPEID";
			dpt_col.AutoComplete = true;
			dpt_col.DataSource = cpt.GetData();
			dpt_col.SortMode = DataGridViewColumnSortMode.Programmatic;
			dpt_col.FlatStyle = FlatStyle.Popup;

			DataGridViewComboBoxColumn ef = new DataGridViewComboBoxColumn();
			ef.Name = @"ef";
			ef.CellTemplate = new DataGridViewComboBoxCell();
			ef.HeaderText = @"Edge Front (L)";
			ef.DropDownWidth = 250;
			ef.DisplayMember = @"DESCR";
			ef.ValueMember = @"EDGEID";
			ef.AutoComplete = true;
			ef.DataSource = ce.GetData();
			ef.SortMode = DataGridViewColumnSortMode.Programmatic;
			ef.FlatStyle = FlatStyle.Popup;

			DataGridViewComboBoxColumn eb = new DataGridViewComboBoxColumn();
			eb.Name = @"eb";
			eb.CellTemplate = new DataGridViewComboBoxCell();
			eb.HeaderText = @"Edge Back (L)";
			eb.DropDownWidth = 250;
			eb.DisplayMember = @"DESCR";
			eb.ValueMember = @"EDGEID";
			eb.AutoComplete = true;
			eb.DataSource = ce.GetData();
			eb.SortMode = DataGridViewColumnSortMode.Programmatic;
			eb.FlatStyle = FlatStyle.Popup;

			DataGridViewComboBoxColumn el = new DataGridViewComboBoxColumn();
			el.Name = @"el";
			el.CellTemplate = new DataGridViewComboBoxCell();
			el.HeaderText = @"Edge Left (W)";
			el.DropDownWidth = 250;
			el.DisplayMember = @"DESCR";
			el.ValueMember = @"EDGEID";
			el.AutoComplete = true;
			el.DataSource = ce.GetData();
			el.SortMode = DataGridViewColumnSortMode.Programmatic;
			el.FlatStyle = FlatStyle.Popup;

			DataGridViewComboBoxColumn er = new DataGridViewComboBoxColumn();
			er.Name = @"er";
			er.CellTemplate = new DataGridViewComboBoxCell();
			er.HeaderText = @"Edge Right (W)";
			er.DropDownWidth = 250;
			er.DisplayMember = @"DESCR";
			er.ValueMember = @"EDGEID";
			er.AutoComplete = true;
			er.DataSource = ce.GetData();
			er.SortMode = DataGridViewColumnSortMode.Programmatic;
			er.FlatStyle = FlatStyle.Popup;

			DataGridViewCheckBoxColumn inc = new DataGridViewCheckBoxColumn();
			inc.Name = @"Include";
			inc.CellTemplate = new DataGridViewCheckBoxCell();
			inc.HeaderText = @"Include";
			inc.SortMode = DataGridViewColumnSortMode.Programmatic;

			foreach (var item in new object[] { inc, dpt_col,
			  part_number, descr, part_qty, mat,
				length, width, thickness, blnk_qty, overl, overw, cnc1, cnc2,
				op1, op2, op3, op4, op5,
				ef, eb, el, er}) {
				dataGridView1.Columns.Add((DataGridViewColumn)item);
			}
		}

		private void SetType(int _type, SwProperties _props, DataGridViewRow _row) {
			_row.Cells[@"Department"].Value = _type;
			for (int i = 1; i < Properties.Settings.Default.OpCount + 1; i++) {
				string op_ = string.Format(@"OP{0}", i);
				string opid_ = string.Format(@"OP{0}ID", i);
				(_props[op_] as OpProperty).OpType = _type;
				(_props[opid_] as OpId).OpType = _type;
			}
		}

		private void ToggleCellWarn(DataGridViewCell _cell, bool on) {
			if (on) {
				_cell.Style.BackColor = Properties.Settings.Default.WarnForeground;
			} else {
				_cell.Style.BackColor = Properties.Settings.Default.NormalBackground;
				_cell.Style.ForeColor = Properties.Settings.Default.NormalForeground;
			}
		}

		private void ToggleCellErr(DataGridViewCell _cell, bool on) {
			if (on) {
				_cell.Style.BackColor = Properties.Settings.Default.WarnBackground;
				_cell.Style.ForeColor = Properties.Settings.Default.WarnForeground;
			} else {
				_cell.Style.BackColor = Properties.Settings.Default.NormalBackground;
				_cell.Style.ForeColor = Properties.Settings.Default.NormalForeground;
			}
		}

		private void SetMaterialCellTooltip(DataGridViewComboBoxCell _c, string _id_field) {
			DataTable dv_ = _c.DataSource as DataTable;
			string filter_ = string.Format(@"{0} = {1}", _id_field, Convert.ToString(_c.Value));
			DataRow[] dr_ = dv_.Select(filter_);
			_c.ToolTipText = Convert.ToString(dr_[0][@"COLOR"]);
		}

		private void FillTable(SortedDictionary<string, int> pl, SortedDictionary<string, SwProperties> sp) {
			System.Text.RegularExpressions.Regex r =
				new System.Text.RegularExpressions.Regex(Redbrick.BOMFilter[0]);
			foreach (KeyValuePair<string, int> item in pl) {
				SwProperties val = sp[item.Key];
				string name = item.Key;
				int idx = dataGridView1.Rows.Add();
				DataGridViewRow row = dataGridView1.Rows[idx];

				if ((int)val[@"DEPARTMENT"].Data > 0 && (int)val[@"DEPARTMENT"].Data <= (int)cpt.TypeCount()) {
					SetType((int)val[@"DEPARTMENT"].Data, val, row);
				} else if ((int)val[@"DEPTID"].Data > 0) {
					SetType((int)val[@"DEPTID"].Data, val, row);
				}

				row.Cells[@"Part Number"].Value = name;
				row.Cells[@"Part Number"].ReadOnly = true;

				row.Cells[@"Description"].Value = val[@"Description"].Value;

				if ((int)val[@"CUTLIST MATERIAL"].Data > 0) {
					row.Cells[@"Material"].Value = val[@"CUTLIST MATERIAL"].Data;
				} else if ((int)val[@"MATID"].Data > 0) {
					row.Cells[@"Material"].Value = val[@"MATID"].Data;
					val[@"CUTLIST MATERIAL"].Data = val[@"MATID"].Data;
				}

				row.Cells[@"L"].Value = Redbrick.enforce_number_format((double)val[@"LENGTH"].Data);
				row.Cells[@"W"].Value = Redbrick.enforce_number_format((double)val[@"WIDTH"].Data);
				row.Cells[@"T"].Value = Redbrick.enforce_number_format((double)val[@"THICKNESS"].Data);
				row.Cells[@"Blank Qty"].Value = (int)val[@"BLANK QTY"].Data;

				row.Cells[@"Over L"].Value = Redbrick.enforce_number_format((double)val[@"OVERL"].Data);
				row.Cells[@"Over W"].Value = Redbrick.enforce_number_format((double)val[@"OVERW"].Data);

				row.Cells[@"CNC 1"].Value = val[@"CNC1"].Data;
				row.Cells[@"CNC 2"].Value = val[@"CNC2"].Data;

				for (int i = 1; i < Properties.Settings.Default.OpCount + 1; i++) {
					string col_ = string.Format(@"Op {0}", i);
					string op_ = string.Format(@"OP{0}", i);
					string opid_ = string.Format(@"OP{0}ID", i);
					if ((int)val[op_].Data > 0) {
						row.Cells[col_].Value = val[op_].Data;
					} else if ((int)val[opid_].Data > 0) {
						row.Cells[col_].Value = val[opid_].Data;
						val[op_].Data = val[opid_].Data;
					}
				}

				if ((int)val[@"EDGE FRONT (L)"].Data > 0) {
					row.Cells[@"ef"].Value = val[@"EDGE FRONT (L)"].Data;
				} else if ((int)val[@"EFID"].Data > 0) {
					row.Cells[@"ef"].Value = val[@"EFID"].Data;
					val[@"EDGE FRONT (L)"].Data = val[@"EFID"].Data;
				}

				if ((int)val[@"EDGE BACK (L)"].Data > 0) {
					row.Cells[@"eb"].Value = val[@"EDGE BACK (L)"].Data;
				} else if ((int)val[@"EBID"].Data > 0) {
					row.Cells[@"eb"].Value = val[@"EBID"].Data;
					val[@"EDGE BACK (L)"].Data = val[@"EBID"].Data;
				}

				if ((int)val[@"EDGE LEFT (W)"].Data > 0) {
					row.Cells[@"el"].Value = val[@"EDGE LEFT (W)"].Data;
				} else if ((int)val[@"ELID"].Data > 0) {
					row.Cells[@"el"].Value = val[@"ELID"].Data;
					val[@"EDGE LEFT (W)"].Data = val[@"ELID"].Data;
				}

				if ((int)val[@"EDGE RIGHT (W)"].Data > 0) {
					row.Cells[@"er"].Value = val[@"EDGE RIGHT (W)"].Data;
				} else if ((int)val[@"ERID"].Data > 0) {
					row.Cells[@"er"].Value = val[@"ERID"].Data;
					val[@"EDGE RIGHT (W)"].Data = val[@"ERID"].Data;
				}

				row.Cells[@"Part Qty"].Value = item.Value;
				val.CutlistQty = item.Value;

				row.Cells[@"Include"].Value = r.IsMatch(name)
					&& row.Cells[@"Department"].Value != null
					&& Convert.ToInt32(row.Cells[@"Department"].Value) != 5; // 5 = "OTHER"
			}
		}

		private void FillCutlistTable() {
			ta_cc.FillByName(eNGINEERINGDataSet.CUT_CUTLISTS, itm, rev_cbx.Text);
			if (eNGINEERINGDataSet.CUT_CUTLISTS.Rows.Count < 1) {
				eNGINEERINGDataSet.CUT_CUTLISTS.AddCUT_CUTLISTSRow(
					itm, rev_cbx.Text, refr, (int)cust_cbx.SelectedValue, dateTimePicker1.Value,
					descr, 0.0f, 0.0f, 0.0f, (int)uid, (int)uid, Properties.Settings.Default.DefaultState,
					new byte[8]);
			} else {
				clid = (eNGINEERINGDataSet.CUT_CUTLISTS.Rows[0] as ENGINEERINGDataSet.CUT_CUTLISTSRow).CLID;
			}
		}

		private void AddPart(SwProperties _pp) {
			string partnum_ = _pp.PartLookup;
			ta_cp.FillByPartnum(dt_cp, partnum_);
			ta_ccp.FillByCutlistIDAndPartID(dt_ccp, _pp.PartID, clid);
			if (dt_cp.Rows.Count < 1) {
				eNGINEERINGDataSet.CUT_PARTS.AddCUT_PARTSRow(partnum_, (string)_pp[@"Description"].Data,
					(float)(double)_pp[@"LENGTH"].Data, (float)(double)_pp[@"WIDTH"].Data,
					(float)(double)_pp[@"THICKNESS"].Data, (string)_pp[@"CNC1"].Data,
					(string)_pp[@"CNC2"].Data, (int)_pp[@"BLANK QTY"].Data,
					(float)(double)_pp[@"OVERL"].Data, (float)(double)_pp[@"OVERW"].Data,
					//(int)_pp[@"OP1"].Data, (int)_pp[@"OP2"].Data, (int)_pp[@"OP3"].Data,
					//(int)_pp[@"OP4"].Data, (int)_pp[@"OP5"].Data,
					(string)_pp[@"COMMENT"].Data,
					(bool)_pp[@"UPDATE CNC"].Data, (int)_pp[@"DEPARTMENT"].Data,
					_pp.Hash);
			}

			if (dt_ccp.Rows.Count < 1) {
				eNGINEERINGDataSet.CUT_CUTLIST_PARTS.AddCUT_CUTLIST_PARTSRow(clid, _pp.PartID,
					(int)_pp[@"CUTLIST MATERIAL"].Data,
					(int)_pp[@"EDGE FRONT (L)"].Data, (int)_pp[@"EDGE BACK (L)"].Data,
					(int)_pp[@"EDGE RIGHT (W)"].Data, (int)_pp[@"EDGE LEFT (W)"].Data,
					_pp.CutlistQty);
			}
		}

		private void FillTables() {
			FillCutlistTable();
			for (int i = 0; i < dataGridView1.Rows.Count; i++) {
				DataGridViewRow dgvr_ = dataGridView1.Rows[i];
				bool inc_ = Convert.ToBoolean(dgvr_.Cells[@"Include"].Value);
				if (inc_) {
					string partnum_ = Convert.ToString(dgvr_.Cells[@"Part Number"].Value);
					AddPart(_partlist[partnum_]);
				}
			}
		}

		private void CreateCutlist_Shown(object sender, EventArgs e) {
			dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
		}

		private void dataGridView1_Scroll(object sender, ScrollEventArgs e) {
			DataGridView gv = sender as DataGridView;
			if (e.ScrollOrientation == ScrollOrientation.VerticalScroll) {
				gv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
				List<string> ignore_ = new List<string>(new string[] { @"Part Qty", @"Blank Qty", @"Over L", @"Over W" });
				foreach (DataGridViewColumn column in gv.Columns) {
					if (!ignore_.Contains(column.Name)) {
						if (column.GetPreferredWidth(DataGridViewAutoSizeColumnMode.DisplayedCells, true) > column.Width) {
							column.Width = column.GetPreferredWidth(DataGridViewAutoSizeColumnMode.DisplayedCells, true);
						}
					}
				}
			}
		}

		

		private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) {
			ComboBox c = sender as ComboBox;
			if (user_changed_item) {
				DataRowView dr = c.SelectedItem as DataRowView;
				set_rev(dr[@"REV"].ToString());
				cust_cbx.SelectedValue = (int)dr[@"CUSTID"];
				dateTimePicker1.Value = DateTime.Parse(dr[@"CDATE"].ToString());
				user_changed_item = false;
			}
		}

		private void comboBox1_MouseClick(object sender, MouseEventArgs e) {
			user_changed_item = true;
			if (sender is Control) {
				Redbrick.UnErr((Control)sender);
			}
		}

		private void comboBox2_KeyDown(object sender, KeyEventArgs e) {
			comboBox_KeyDown(sender, e);
			user_changed_item = true;
			Redbrick.UnErr((Control)sender);
		}

		private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
			DataGridView grid_ = sender as DataGridView;
			DataGridViewCell part_cell_ = grid_.Rows[e.RowIndex].Cells[@"Part Number"];
			System.Text.RegularExpressions.Regex r =
				new System.Text.RegularExpressions.Regex(Redbrick.BOMFilter[0]);
			if (part_cell_.Value != null && r.IsMatch(part_cell_.Value.ToString())) {
				DataGridViewCell dpt_cell_ = grid_.Rows[e.RowIndex].Cells[@"Department"];
				DataGridViewCell ppb_cell_ = grid_.Rows[e.RowIndex].Cells[@"Blank Qty"];
				DataGridViewCell descr_cell_ = grid_.Rows[e.RowIndex].Cells[@"Description"];
				DataGridViewComboBoxCell mat_ = grid_.Rows[e.RowIndex].Cells[@"Material"] as DataGridViewComboBoxCell;
				DataGridViewComboBoxCell ef_ = grid_.Rows[e.RowIndex].Cells[@"ef"] as DataGridViewComboBoxCell;
				DataGridViewComboBoxCell eb_ = grid_.Rows[e.RowIndex].Cells[@"eb"] as DataGridViewComboBoxCell;
				DataGridViewComboBoxCell er_ = grid_.Rows[e.RowIndex].Cells[@"er"] as DataGridViewComboBoxCell;
				DataGridViewComboBoxCell el_ = grid_.Rows[e.RowIndex].Cells[@"el"] as DataGridViewComboBoxCell;
				if (dpt_cell_.Value != null) {
					ToggleCellWarn(dpt_cell_, false);
				} else {
					ToggleCellWarn(dpt_cell_, true);
				}

				if (ppb_cell_.Value != null && (int)ppb_cell_.Value > 0) {
					ToggleCellErr(ppb_cell_, false);
				} else if (ppb_cell_.Value == null || (int)ppb_cell_.Value < 1) {
					ToggleCellErr(ppb_cell_, true);
				}

				if (descr_cell_.Value != null &&
					descr_cell_.Value.ToString() != string.Empty) {
					ToggleCellWarn(descr_cell_, false);
				} else if (descr_cell_.Value == null ||
					(descr_cell_.Value.ToString() == string.Empty ||
					descr_cell_.Value.ToString().Contains("$"))) {
					ToggleCellWarn(descr_cell_, true);
				}

				if (mat_.Value != null && (int)mat_.Value > 0) {
					SetMaterialCellTooltip(mat_, @"MATID");
				}

				if (ef_.Value != null && (int)ef_.Value > 0) {
					SetMaterialCellTooltip(ef_, @"EDGEID");
				}

				if (eb_.Value != null && (int)eb_.Value > 0) {
					SetMaterialCellTooltip(eb_, @"EDGEID");
				}

				if (er_.Value != null && (int)er_.Value > 0) {
					SetMaterialCellTooltip(er_, @"EDGEID");
				}

				if (el_.Value != null && (int)el_.Value > 0) {
					SetMaterialCellTooltip(el_, @"EDGEID");
				}
			}
		}

		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) {
			string name = dataGridView1.Columns[e.ColumnIndex].Name;
			if (name == @"Include" && (e.RowIndex > -1 && e.RowIndex < dataGridView1.Rows.Count)) {
				DataGridViewCheckBoxCell cbx_ = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewCheckBoxCell;
				int add = (bool)cbx_.Value ? -1 : 1;
				count_includes();
			}
		}

		private void comboBox3_SelectedValueChanged(object sender, EventArgs e) {
			if (rev_changed_by_user) {
				ComboBox _cb = sender as ComboBox;
				if (_revFromFile == string.Empty || _cb.Text == _revFromFile) {
					Redbrick.UnErr(_cb);
					rev_tooltip.RemoveAll();
				}
				rev = _cb.Text;
				if (rev_in_filename) {
					ref_cbx.Text = string.Format(@"{0} REV {1}", topName, rev);
				}
				rev_changed_by_user = false;
			}
		}

		private void comboBox3_MouseClick(object sender, MouseEventArgs e) {
			rev_changed_by_user = true;
		}

		private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e) {
			DataGridView d = (sender as DataGridView);
			if (sort_directions[e.ColumnIndex]) {
				d.Sort(d.Columns[e.ColumnIndex], ListSortDirection.Descending);
			} else {
				d.Sort(d.Columns[e.ColumnIndex], ListSortDirection.Ascending);
			}
			sort_directions[e.ColumnIndex] = !sort_directions[e.ColumnIndex];
		}

		private void comboBox5_TextChanged(object sender, EventArgs e) {
			ComboBox c = (sender as ComboBox);
			if (c.Text == string.Empty) {
				ToggleDescrWarn(true);
			} else {
				if (c.Text.Length > eNGINEERINGDataSet.CUT_CUTLISTS.DESCRColumn.MaxLength) {
					string msg_ = string.Format(Properties.Resources.LengthWarning,
						eNGINEERINGDataSet.CUT_CUTLISTS.DESCRColumn.MaxLength);
					Redbrick.Err(c);
					descr_tooltip.SetToolTip(c, msg_);
				} else {
					ToggleDescrWarn(false);
				}
			}
		}

		private void comboBox_KeyPress(object sender, KeyPressEventArgs e) {
			if (Properties.Settings.Default.FlameWar && char.IsLetter(e.KeyChar)) {
				e.KeyChar = char.ToUpper(e.KeyChar);
			}
		}

		private void button1_Click(object sender, EventArgs e) {
			Close();
		}

		private void CreateCutlist_FormClosing(object sender, FormClosingEventArgs e) {
			Properties.Settings.Default.CreateCutlistLocation = Location;
			Properties.Settings.Default.CreateCutlistSize = Size;
			Properties.Settings.Default.Save();
		}

		private void select_btn_Click(object sender, EventArgs e) {
			System.Text.RegularExpressions.Regex r =
				new System.Text.RegularExpressions.Regex(Redbrick.BOMFilter[0]);
			foreach (DataGridViewRow item in dataGridView1.Rows) {
				string part_ = (string)item.Cells[@"Part Number"].Value;
				if (part_ != null && r.IsMatch(part_)) {
					DataGridViewComboBoxCell dept = item.Cells[@"Department"] as DataGridViewComboBoxCell;
					if (dept.Value != null) {
						if ((int)dept.Value == (int)type_cbx.SelectedValue) {
							DataGridViewCheckBoxCell inc = item.Cells[@"Include"] as DataGridViewCheckBoxCell;
							inc.Value = true;
							count_includes();
						}
					}
				}
			}
		}

		private void unselect_btn_Click(object sender, EventArgs e) {
			foreach (DataGridViewRow item in dataGridView1.Rows) {
				DataGridViewComboBoxCell dept = item.Cells[@"Department"] as DataGridViewComboBoxCell;
				if (dept.Value != null) {
					if ((int)dept.Value == (int)type_cbx.SelectedValue) {
						DataGridViewCheckBoxCell inc = item.Cells[@"Include"] as DataGridViewCheckBoxCell;
						inc.Value = false;
						count_includes();
					}
				}
			}
		}

		private void comboBox_KeyDown(object sender, KeyEventArgs e) {
			(sender as ComboBox).DroppedDown = false;
		}

		private void dataGridView1_MouseClick(object sender, MouseEventArgs e) {
			DataGridView grid_ = (sender as DataGridView);
			if (e.Button == MouseButtons.Right && _swApp != null) {
				ContextMenu m = new ContextMenu();
				int current_row = grid_.HitTest(e.X, e.Y).RowIndex;
				if (current_row >= 0) {
					DataGridViewCell cell_ = (grid_[@"Part Number", current_row] as DataGridViewCell);
					if (cell_.Value != null) {
						selectedPart = cell_.Value.ToString();
						foundPDF = find_pdf(selectedPart);
						MenuItem [] items = { new MenuItem(string.Format(@"Open Model ({0})...", selectedPart), OnClickOpenModel),
																	new MenuItem(@"-"),
																	new MenuItem(@"Open Drawing...", OnClickOpenDrawing),
																	new MenuItem(@"Open PDF...", OnClickOpenPDF),
																	new MenuItem(@"Create Drawing..."),
																	new MenuItem(@"-"),
																	new MenuItem(@"Machine Priority...", OnClickMachinePriority) };
						items[2].Enabled = DrawingExists(selectedPart);
						items[3].Enabled = foundPDF != null ? foundPDF.Exists : false;
						items[4].Enabled = false;
						m.MenuItems.AddRange(items);
					}
				}
				m.Show(grid_, new Point(e.X, e.Y));
			}
		}

		private FileInfo find_doc(string doc) {
			foreach (KeyValuePair<string, SwProperties> item in _partlist) {
				string fn = item.Value[@"DEPARTMENT"].PartFileInfo.Name;
				if (fn.Trim().ToUpper().Contains(doc.Trim().ToUpper())) {
					return item.Value[@"DEPARTMENT"].PartFileInfo;
				}
			}
			return null;
		}

		private string GetPath() {
			return System.IO.Path.GetDirectoryName(
				(_swApp.ActiveDoc as ModelDoc2).GetPathName());
		}

		private bool DrawingExists(string part) {
			FileInfo fi = find_doc(part);
			string ext = fi.Extension;
			FileInfo dwgfi = new FileInfo(fi.FullName.Replace(ext, @".SLDDRW"));
			return dwgfi.Exists;
		}

		private FileInfo find_pdf(string doc) {
			string searchterm_ = string.Format(@"{0}.PDF", doc);
			ENGINEERINGDataSetTableAdapters.GEN_DRAWINGSTableAdapter gdta =
				new ENGINEERINGDataSetTableAdapters.GEN_DRAWINGSTableAdapter();
			ENGINEERINGDataSet.GEN_DRAWINGSDataTable dt = gdta.GetDataByFName(searchterm_);
			if (dt.Rows.Count > 0) {
				ENGINEERINGDataSet.GEN_DRAWINGSRow r = (dt.Rows[0] as ENGINEERINGDataSet.GEN_DRAWINGSRow);
				return new FileInfo(string.Format(@"{0}{1}", r.FPath, r.FName));
			} else {
				ENGINEERINGDataSetTableAdapters.GEN_DRAWINGS_MTLTableAdapter gdmta =
					new ENGINEERINGDataSetTableAdapters.GEN_DRAWINGS_MTLTableAdapter();
				ENGINEERINGDataSet.GEN_DRAWINGS_MTLDataTable mdt = gdmta.GetDataByFName(searchterm_);
				if (mdt.Rows.Count > 0) {
					ENGINEERINGDataSet.GEN_DRAWINGS_MTLRow mr = (mdt.Rows[0] as ENGINEERINGDataSet.GEN_DRAWINGS_MTLRow);
					return new FileInfo(string.Format(@"{0}{1}", mr.FPath, mr.FName));
				}
			}
			return null;
		}

		private void OnClickOpenPDF(object sender, EventArgs e) {
			System.Diagnostics.Process.Start(foundPDF.FullName);
		}

		private void OnClickMachinePriority(object sender, EventArgs e) {
			Machine_Priority_Control.MachinePriority mp = new Machine_Priority_Control.MachinePriority(selectedPart);
			mp.Show(this);
		}

		private void OnClickOpenModel(object sender, EventArgs e) {
			try {
				int err = 0;
				DirectoryInfo di = new DirectoryInfo(GetPath());
				FileInfo fi = find_doc(selectedPart);
				string t = fi.FullName.ToUpper();
				if (File.Exists(t)) {
					_swApp.ActivateDoc3(t, true,
						(int)swRebuildOnActivation_e.swDontRebuildActiveDoc, ref err);
					Close();
				} else {
					MessageBox.Show(this, string.Format(@"Couldn't find '{0}'", t),
						@"Not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			} catch (NullReferenceException nex) {
				MessageBox.Show(this, string.Format("You must select a row with something in it.\n{0}", nex.Message),
					@"Not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
			} catch (Exception ex) {
				MessageBox.Show(this, ex.Message,
					@"Not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void OnClickOpenDrawing(object sender, EventArgs e) {
			try {
				int err = 0;
				DirectoryInfo di = new DirectoryInfo(GetPath());
				FileInfo fi = find_doc(selectedPart);
				string t = fi.FullName.ToUpper();
				string ext = Path.GetExtension(t).ToUpper();
				string fullpath = t.Replace(ext, @".SLDDRW");
				if (File.Exists(fullpath)) {
					_swApp.OpenDocSilent(fullpath,
						(int)swDocumentTypes_e.swDocDRAWING,
						(int)swOpenDocOptions_e.swOpenDocOptions_Silent);
					_swApp.ActivateDoc3(fullpath, true,
						(int)swRebuildOnActivation_e.swDontRebuildActiveDoc, ref err);
					Close();
				} else {
					MessageBox.Show(this, string.Format(@"Couldn't find '{0}'", fullpath),
						@"Not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			} catch (NullReferenceException nex) {
				MessageBox.Show(this, string.Format("You must select a row with something in it.\n{0}", nex.Message),
					@"Not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
			} catch (Exception ex) {
				MessageBox.Show(this, ex.Message,
					@"Not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void upload_btn_Click(object sender, EventArgs e) {
			if (cust_cbx.SelectedItem != null && uid != null) {
				Cursor = Cursors.WaitCursor;
				List<SwProperties> parts_ = new List<SwProperties>();

				for (int i = 0; i < dataGridView1.Rows.Count; i++) {
					DataGridViewRow dgvr_ = dataGridView1.Rows[i];
					bool inc_ = Convert.ToBoolean(dgvr_.Cells[@"Include"].FormattedValue);
					if (inc_) {
						string partnum_ = Convert.ToString(dgvr_.Cells[@"Part Number"].Value);
						_partlist[partnum_][@"DEPARTMENT"].Data = Convert.ToInt32(dgvr_.Cells[@"Department"].Value);
						_partlist[partnum_][@"Description"].Data = Convert.ToString(dgvr_.Cells[@"Description"].Value);
						_partlist[partnum_].CutlistQty = Convert.ToInt32(dgvr_.Cells[@"Part Qty"].Value);
						_partlist[partnum_][@"BLANK QTY"].Data = Convert.ToInt32(dgvr_.Cells[@"Blank Qty"].Value);
						_partlist[partnum_][@"CUTLIST MATERIAL"].Data = Convert.ToInt32(dgvr_.Cells[@"Material"].Value);
						_partlist[partnum_][@"EDGE FRONT (L)"].Data = Convert.ToInt32(dgvr_.Cells[@"ef"].Value);
						_partlist[partnum_][@"EDGE BACK (L)"].Data = Convert.ToInt32(dgvr_.Cells[@"eb"].Value);
						_partlist[partnum_][@"EDGE LEFT (W)"].Data = Convert.ToInt32(dgvr_.Cells[@"el"].Value);
						_partlist[partnum_][@"EDGE RIGHT (W)"].Data = Convert.ToInt32(dgvr_.Cells[@"er"].Value);
						_partlist[partnum_].Write();
						_partlist[partnum_].Save();
						parts_.Add(_partlist[partnum_]);
					}
				}

				int custid_ = Convert.ToInt32(cust_cbx.SelectedValue);
				dt_cc.UpdateCutlist(itm_cbx.Text, ref_cbx.Text, rev_cbx.Text, descr_cbx.Text, custid_,
					dateTimePicker1.Value, Properties.Settings.Default.DefaultState, Convert.ToInt32(uid), parts_);
				(sender as Control).Enabled = false;
				cancel_btn.Text = @"Close";
				Cursor = Cursors.Arrow;
			}
		}

		private void hide_btn_Click(object sender, EventArgs e) {
			List<DataGridViewRow> l_ = new List<DataGridViewRow>();
			for (int i = 0; i < dataGridView1.Rows.Count; i++) {
				DataGridViewRow dgvr_ = dataGridView1.Rows[i];
				bool inc_ = Convert.ToBoolean(dgvr_.Cells[@"Include"].FormattedValue);
				if (!inc_ && !dgvr_.IsNewRow) {
					l_.Add(dgvr_);
				}
			}

			foreach (DataGridViewRow item in l_) {
				dataGridView1.Rows.RemoveAt(item.Index);
			}
			count_includes();
		}

		private void ref_cbx_TextChanged(object sender, EventArgs e) {
			ComboBox c = sender as ComboBox;
			if (c.Text.Length > eNGINEERINGDataSet.CUT_CUTLISTS.DRAWINGColumn.MaxLength) {
				string msg_ = string.Format(Properties.Resources.LengthWarning,
					eNGINEERINGDataSet.CUT_CUTLISTS.DRAWINGColumn.MaxLength);
				Redbrick.Err(c);
				drw_tooltip.SetToolTip(c, msg_);
			} else {
				Redbrick.UnErr(c);
				drw_tooltip.RemoveAll();
			}
		}

		private void itm_cbx_TextUpdate(object sender, EventArgs e) {
			ComboBox c = sender as ComboBox;
			if (c.Text.Length > eNGINEERINGDataSet.CUT_CUTLISTS.PARTNUMColumn.MaxLength) {
				string msg_ = string.Format(Properties.Resources.LengthWarning,
					eNGINEERINGDataSet.CUT_CUTLISTS.PARTNUMColumn.MaxLength);
				Redbrick.Err(c);
				item_tooltip.SetToolTip(c, msg_);
			} else if (c.Text.Length < 1) {
				Redbrick.Err(c);
				item_tooltip.SetToolTip(c, Properties.Resources.EmptyWarning);
			} else {
				Redbrick.UnErr(c);
				item_tooltip.RemoveAll();
			}
		}
	}
}