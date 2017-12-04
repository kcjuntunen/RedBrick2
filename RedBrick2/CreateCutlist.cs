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
	public partial class CreateCutlist : Form {

		private SortedDictionary<string, int> _dict = new SortedDictionary<string, int>();
		private SortedDictionary<string, SwProperties> _partlist = new SortedDictionary<string, SwProperties>();
		private SldWorks _swApp;

		private FileInfo PartFileInfo;
		private string topName = string.Empty;
		private string partLookup = string.Empty;
		private string _revFromFile = string.Empty;
		private string _revFromProperties = string.Empty;
		private string rev = @"100";
		private bool rev_changed_by_user = false;
		private bool rev_in_filename = false;
		private bool user_changed_item = false;
		private int included_parts = 0;
		private Configuration _config = null;
		private bool[] sort_directions = { false, false, false, false, false, false };
		private ToolTip rev_tooltip = new ToolTip();
		private ToolTip descr_tooltip = new ToolTip();
		private ToolTip cust_tooltip = new ToolTip();

		int total_parts = 0;
		UserProgressBar pb;

		public CreateCutlist(SldWorks s) {
			_swApp = s;
			InitializeComponent();
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
				SolidWorks.Interop.sldworks.View _v = Redbrick.GetFirstView(_swApp);
				m = _v.ReferencedDocument;
				_config = m.GetConfigurationByName(_v.ReferencedConfiguration);
				swConf = _config;
			}

			Text = string.Format(@"{0} - {1}", topName, _config.Name);

			swRootComp = (Component2)swConf.GetRootComponent();

			PartFileInfo = new FileInfo(m.GetPathName());
			string _pnwe = Path.GetFileNameWithoutExtension(PartFileInfo.Name);
			partLookup = _pnwe.Split(new string[] { @" " }, StringSplitOptions.RemoveEmptyEntries)[0];

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
			toolStripStatusLabel2.Text = string.Format("Included Parts: {0}", count_includes());
			toolStripStatusLabel1.Text = string.Format("Total Unique Parts: {0}", dataGridView1.Rows.Count - 1);
		}

		private void ToggleRevWarn(bool on) {
			if (on) {
				Redbrick.Warn(comboBox3);
				rev_tooltip.SetToolTip(comboBox3, Properties.Resources.RevisionNotMatching);
				rev_tooltip.SetToolTip(label6, Properties.Resources.RevisionNotMatching);
			} else {
				Redbrick.Unwarn(comboBox3 as ComboBox);
				rev_tooltip.RemoveAll();
			}
		}

		private void ToggleCustomerWarn(bool on) {
			if (on) {
				Redbrick.Warn(comboBox1);
				cust_tooltip.SetToolTip(comboBox1, Properties.Resources.CustomerNotMatching);
				cust_tooltip.SetToolTip(label1, Properties.Resources.CustomerNotMatching);
			} else {
				Redbrick.Unwarn(comboBox1);
				cust_tooltip.RemoveAll();
			}
		}

		private void ToggleDescrWarn(bool on) {
			if (on) {
				Redbrick.Warn(comboBox5);
				descr_tooltip.SetToolTip(comboBox5, Properties.Resources.NoDescriptionWarning);
				descr_tooltip.SetToolTip(label3, Properties.Resources.NoDescriptionWarning);
			} else {
				Redbrick.Unwarn(comboBox5);
				descr_tooltip.RemoveAll();
			}
		}

		private void CreateCutlist_Load(object sender, EventArgs e) {
			Location = Properties.Settings.Default.CreateCutlistLocation;
			Size = Properties.Settings.Default.CreateCutlistSize;
			// TODO: This line of code loads data into the 'eNGINEERINGDataSet.CUT_CUTLISTS' table. You can move, or remove it, as needed.
			//this.cUT_CUTLISTSTableAdapter.Fill(this.eNGINEERINGDataSet.CUT_CUTLISTS);
			// TODO: This line of code loads data into the 'eNGINEERINGDataSet.GEN_CUSTOMERS' table. You can move, or remove it, as needed.
			this.gEN_CUSTOMERSTableAdapter.Fill(this.eNGINEERINGDataSet.GEN_CUSTOMERS);
			if (Properties.Settings.Default.OnlyCurrentCustomers) {
				gENCUSTOMERSBindingSource.Filter = @"CUSTACTIVE = True";
			}
			this.revListTableAdapter.Fill(this.eNGINEERINGDataSet.RevList);

			ENGINEERINGDataSet.SCH_PROJECTSRow spr = (new ENGINEERINGDataSet.SCH_PROJECTSDataTable()).GetCorrectCustomer(partLookup);
			if (spr != null) {
				comboBox1.SelectedValue = spr.CUSTID;
			} else {
				CustomerProperty _cp = new CustomerProperty(@"CUSTOMER", true, _swApp, _swApp.ActiveDoc);
				_cp.Get();
				comboBox1.SelectedIndex = comboBox1.FindString(_cp.Value.Split('-')[0].Trim());
			}
			dateTimePicker1.Value = DateTime.Now;
			settle_rev(topName);
			get_names();
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

			comboBox5.Text = stpr.Value;
			if (comboBox5.Text == string.Empty) {
				ToggleDescrWarn(true);
			}
			comboBox2.Text = topName;
			if (rev_in_filename) {
				comboBox4.Text = string.Format(@"{0} REV {1}", topName, rev);
			} else {
				comboBox4.Text = topName;
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
				Redbrick.Unwarn(comboBox3);
				rev_tooltip.RemoveAll();
			}

			set_rev(rev);
		}

		private void set_rev(string r) {
			int idx = comboBox3.FindString(r);
			if (idx > -1) {
				comboBox3.SelectedIndex = idx;
			} else {
				comboBox3.Text = r;
			}
		}

		private int count_includes() {
			int x = 0;
			foreach (DataGridViewRow row in dataGridView1.Rows) {
				object test = (row.Cells[@"Include"] as DataGridViewCheckBoxCell).Value;
				if (test != null && (bool)test) {
					x++;
				}
			}
			return x;
		}

		private int count_includes(int add) {
			int x = add;
			foreach (DataGridViewRow row in dataGridView1.Rows) {
				object test = (row.Cells[@"Include"] as DataGridViewCheckBoxCell).Value;
				if (test != null && (bool)test) {
					x++;
				}
			}
			return x;
		}

		private void GetPart(ModelDoc2 m) {
			pb.Start(0, 1, @"Enumerating parts...");
			string name = Path.GetFileNameWithoutExtension(m.GetPathName());
			SwProperties s = new SwProperties(_swApp);
			s.Configuration = _config.Name;
			Configuration c_ = m.GetActiveConfiguration();
			s.Configuration = c_.Name;
			s.GetProperties(m);
			comboBox2.Text = partLookup;
			comboBox4.Text = partLookup;
			comboBox5.Text = s[@"Description"].Value;
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
				string name = swChildComp.Name2.Substring(0, swChildComp.Name2.LastIndexOf('-'));
				if (name.Contains("/")) {
					name = name.Substring(name.LastIndexOf('/') + 1);
				}
				pb.UpdateTitle(name);

				ModelDoc2 md = (swChildComp.GetModelDoc2() as ModelDoc2);
				if (md != null && md.GetType() == (int)swDocumentTypes_e.swDocPART) {
					SwProperties s = new SwProperties(_swApp, md);
					s.Configuration = _config.Name;
					Configuration c_ = md.GetActiveConfiguration();
					s.Configuration = c_.Name;
					s.GetProperties(md);
					if (!_dict.ContainsKey(name)) {
						_dict.Add(name, 1);
						_partlist.Add(name, s);
					} else {
						_dict[name] = _dict[name] + 1;
						_partlist[name][@"BLANK QTY"].Data = _dict[name];
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
			ENGINEERINGDataSetTableAdapters.CUT_MATERIALSTableAdapter cm =
				new ENGINEERINGDataSetTableAdapters.CUT_MATERIALSTableAdapter();
			ENGINEERINGDataSetTableAdapters.CUT_PART_TYPESTableAdapter cpt =
				new ENGINEERINGDataSetTableAdapters.CUT_PART_TYPESTableAdapter();

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

			//DataGridViewColumn mat = new DataGridViewColumn();
			//mat.Name = @"Material";
			//mat.CellTemplate = new DataGridViewTextBoxCell();
			//mat.ValueType = typeof(string);
			//mat.SortMode = DataGridViewColumnSortMode.Programmatic;

			DataGridViewComboBoxColumn mat = new DataGridViewComboBoxColumn();
			mat.Name = @"Material";
			mat.CellTemplate = new DataGridViewComboBoxCell();
			mat.HeaderText = @"Material";
			mat.DropDownWidth = 250;
			mat.DisplayMember = @"DESCR";
			mat.ValueMember = @"MATID";
			mat.AutoComplete = true;
			mat.DataSource = cm.GetData();
			mat.SortMode = DataGridViewColumnSortMode.Programmatic;
			mat.FlatStyle = FlatStyle.Popup;

			DataGridViewColumn part_qty = new DataGridViewColumn();
			part_qty.Name = @"Part Qty";
			part_qty.CellTemplate = new DataGridViewTextBoxCell();
			part_qty.ValueType = typeof(int);
			part_qty.SortMode = DataGridViewColumnSortMode.Programmatic;

			DataGridViewComboBoxColumn col = new DataGridViewComboBoxColumn();
			col.Name = @"Department";
			col.CellTemplate = new DataGridViewComboBoxCell();
			col.HeaderText = @"Department";
			col.DropDownWidth = 150;
			col.DisplayMember = @"TYPEDESC";
			col.ValueMember = @"TYPEID";
			col.AutoComplete = true;
			col.DataSource = cpt.GetData();
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			col.FlatStyle = FlatStyle.Popup;

			DataGridViewCheckBoxColumn inc = new DataGridViewCheckBoxColumn();
			inc.Name = @"Include";
			inc.CellTemplate = new DataGridViewCheckBoxCell();
			inc.HeaderText = @"Include";
			inc.SortMode = DataGridViewColumnSortMode.Programmatic;

			foreach (var item in new object[] { part_number, descr, mat, part_qty, col, inc }) {
				dataGridView1.Columns.Add((DataGridViewColumn)item);
			}
		}

		private void FillTable(SortedDictionary<string, int> pl, SortedDictionary<string, SwProperties> sp) {
			ENGINEERINGDataSetTableAdapters.CUT_PART_TYPESTableAdapter cpt =
				new ENGINEERINGDataSetTableAdapters.CUT_PART_TYPESTableAdapter();
			ENGINEERINGDataSetTableAdapters.CUT_MATERIALSTableAdapter cmt =
				new ENGINEERINGDataSetTableAdapters.CUT_MATERIALSTableAdapter();
			System.Text.RegularExpressions.Regex r =
				new System.Text.RegularExpressions.Regex(Redbrick.BOMFilter[0]);
			foreach (KeyValuePair<string, int> item in pl) {
				SwProperties val = sp[item.Key];
				string name = item.Key;
				DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
				row.Cells[0].Value = name;
				row.Cells[1].Value = val[@"Description"].Value;
				if ((int)val[@"CUTLIST MATERIAL"].Data > 0 && (int)val[@"CUTLIST MATERIAL"].Data <= (int)cmt.MaterialCount()) {
					row.Cells[2].Value = val[@"CUTLIST MATERIAL"].Data;
				}
				row.Cells[3].Value = item.Value;
				if ((int)val[@"DEPARTMENT"].Data > 0 && (int)val[@"DEPARTMENT"].Data <= (int)cpt.TypeCount()) {
					row.Cells[4].Value = val[@"DEPARTMENT"].Data;
				}
				row.Cells[5].Value = r.IsMatch(name);
				dataGridView1.Rows.Add(row);
			}
		}

		private void CreateCutlist_Shown(object sender, EventArgs e) {
			dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
		}

		private void dataGridView1_Scroll(object sender, ScrollEventArgs e) {
			DataGridView gv = sender as DataGridView;
			if (e.ScrollOrientation == ScrollOrientation.VerticalScroll) {
				gv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
				foreach (DataGridViewColumn column in gv.Columns) {
					if (column.GetPreferredWidth(DataGridViewAutoSizeColumnMode.DisplayedCells, true) > column.Width) {
						column.Width = column.GetPreferredWidth(DataGridViewAutoSizeColumnMode.DisplayedCells, true);
					}
				}
			}
		}

		

		private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) {
			if (user_changed_item) {
				ComboBox c = sender as ComboBox;
				DataRowView dr = c.SelectedItem as DataRowView;
				set_rev(dr[@"REV"].ToString());
				comboBox1.SelectedValue = (int)dr[@"CUSTID"];
				dateTimePicker1.Value = DateTime.Parse(dr[@"CDATE"].ToString());
				user_changed_item = false;
			}
		}

		private void comboBox1_MouseClick(object sender, MouseEventArgs e) {
			user_changed_item = true;
			if (sender is Control) {
				Redbrick.Unwarn((Control)sender);
			}
		}

		private void comboBox2_KeyDown(object sender, KeyEventArgs e) {
			user_changed_item = true;
			Redbrick.Unwarn((Control)sender);
		}

		private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
		}

		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) {
			string name = dataGridView1.Columns[e.ColumnIndex].Name;
			if (name == @"Include" && (e.RowIndex > -1 && e.RowIndex < dataGridView1.Rows.Count)) {
				int add = (bool)(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewCheckBoxCell).Value ? -1 : 1;
				toolStripStatusLabel2.Text = string.Format("Included Parts: {0}", count_includes(add));
			}
		}

		private void comboBox3_SelectedValueChanged(object sender, EventArgs e) {
			if (rev_changed_by_user) {
				ComboBox _cb = sender as ComboBox;
				if (_revFromFile == string.Empty || _cb.Text == _revFromFile) {
					Redbrick.Unwarn(_cb);
					rev_tooltip.RemoveAll();
				}
				rev = _cb.Text;
				if (rev_in_filename) {
					comboBox4.Text = string.Format(@"{0} REV {1}", topName, rev);
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
				ToggleDescrWarn(false);
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
	}
}