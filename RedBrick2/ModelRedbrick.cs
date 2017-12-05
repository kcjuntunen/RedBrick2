using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using System.IO;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
	public partial class ModelRedbrick : UserControl {
		private SwProperties PropertySet;
		private ENGINEERINGDataSetTableAdapters.CUT_PARTSTableAdapter cpta =
			new ENGINEERINGDataSetTableAdapters.CUT_PARTSTableAdapter();
		private ENGINEERINGDataSet.CUT_PARTSRow Row = null;

		private SelectionMgr swSelMgr;
		private Component2 swSelComp;
		private ConfigurationManager configurationManager;
		private string configuration;
		private AssemblyDoc ad;
		private PartDoc pd;
		private DrawingDoc dd;

		private DrawingRedbrick drawingRedbrick;

		private string partLookup;
		private Single length = 0.0F;
		private Single width = 0.0F;
		private Single thickness = 0.0F;

		private Point scrollOffset;

		private const int WM_PAINT = 0x000F;
		private bool allowPaint;

		private ModelDoc2 lastModelDoc = null;

		private bool initialated = false;
		private bool cutlistMatChanged = false;
		private bool AssemblyEventsAssigned = false;
		private bool PartEventsAssigned = false;
		private bool DrawingEventsAssigned = false;
		private bool ModelSetup = false;
		private bool DrawingSetup = false;
		private bool ov_userediting = false;
		private bool bl_userediting = false;
		private bool cl_userediting = false;
		private bool data_from_db = false;
		private ComboBox[] cbxes;
		private ToolTip groupbox_tooltip = new ToolTip();
		private ToolTip cutlist_tooltip = new ToolTip();
		private ToolTip descr_tooltup = new ToolTip();
		private ToolTip ppb_tooltip = new ToolTip();
		private ToolTip over_tooltip = new ToolTip();

		public ModelRedbrick(SldWorks sw, ModelDoc2 md) {
			SwApp = sw;
			InitializeComponent();
			ToggleFlameWar(Properties.Settings.Default.FlameWar);

			ActiveDoc = md;
			tabPage1.Text = @"Model Properties";
			tabPage2.Text = @"DrawingProperties";
			groupBox1.MouseClick += groupBox1_MouseClick;
			label6.MouseDown += clip_click;
			label7.MouseDown += clip_click;
			label8.MouseDown += clip_click;
			label9.MouseDown += clip_click;
			label10.MouseDown += clip_click;

			overLtb.TextChanged += textBox9_TextChanged;
			overWtb.TextChanged += textBox10_TextChanged;
		}

		public void TogglePriorityButton() {
			bool no_cnc1 = (cnc1tb.Text == @"NA") || (cnc1tb.Text == string.Empty);
			bool no_cnc2 = (cnc2tb.Text == @"NA") || (cnc2tb.Text == string.Empty);
			bool enabled = !(no_cnc1 && no_cnc2);
			prioritybtn.Enabled = enabled;
		}

		public void ToggleFlameWar(bool on) {
			if (drawingRedbrick != null) {
				drawingRedbrick.ToggleFlameWar(on);
			}
			if (on) {
				descriptiontb.CharacterCasing = CharacterCasing.Upper;
				commenttb.CharacterCasing = CharacterCasing.Upper;
				cnc1tb.CharacterCasing = CharacterCasing.Upper;
				cnc2tb.CharacterCasing = CharacterCasing.Upper;
			} else {
				descriptiontb.CharacterCasing = CharacterCasing.Normal;
				commenttb.CharacterCasing = CharacterCasing.Normal;
				cnc1tb.CharacterCasing = CharacterCasing.Normal;
				cnc2tb.CharacterCasing = CharacterCasing.Normal;
			}
		}

		protected override void WndProc(ref Message m) {
			if ((m.Msg != WM_PAINT || (allowPaint && m.Msg == WM_PAINT))) {
				base.WndProc(ref m);
			}
		}

		void groupBox1_MouseClick(object sender, MouseEventArgs e) {
			Redbrick.Clip(partLookup);
		}

		void textBox9_TextChanged(object sender, EventArgs e) {
			if (initialated) {
				try {
					Single _edge_thickness = 0.0F;
					if (edgel.SelectedItem != null) {
						_edge_thickness += (Single)(edgel.SelectedItem as DataRowView)[@"THICKNESS"];
					}

					if (edger.SelectedItem != null) {
						_edge_thickness += (Single)(edger.SelectedItem as DataRowView)[@"THICKNESS"];
					}

					//double _val = ((double.Parse(label18.Text) +
					//  double.Parse((sender as TextBox).Text))) - _edge_thickness;

					if (ov_userediting) {
						calculate_blanksize_from_oversize(float.Parse((sender as TextBox).Text), blnkszLtb, length, _edge_thickness);
						//textBox12.Text = enforce_number_format(_val);
						ov_userediting = false;
					}
				} catch (Exception) {
					blnkszLtb.Text = @"#VALUE!";
				}
			}
		}

		void textBox10_TextChanged(object sender, EventArgs e) {
			if (initialated) {
				try {
					Single _edge_thickness = 0.0F;
					if (edgef.SelectedItem != null) {
						_edge_thickness += (Single)(edgef.SelectedItem as DataRowView)[@"THICKNESS"];
					}

					if (edgeb.SelectedItem != null) {
						_edge_thickness += (Single)(edgeb.SelectedItem as DataRowView)[@"THICKNESS"];
					}
					//double _val = ((double.Parse(label19.Text) +
					//   double.Parse((sender as TextBox).Text))) - _edge_thickness;
					if (ov_userediting) {
						calculate_blanksize_from_oversize(float.Parse((sender as TextBox).Text), blnkszWtb, width, _edge_thickness);
						//textBox13.Text = enforce_number_format(_val);
						ov_userediting = false;
					}
				} catch (Exception) {
					blnkszWtb.Text = @"#VALUE!";
				}
			}
		}

		public void DumpActiveDoc() {
			lastModelDoc = null;
			_activeDoc = null;
		}

		public void ReQuery(ModelDoc2 md) {
			ActiveDoc = md;
		}

		private void ReQuery() {
			GetCutlistData();
			flowLayoutPanel1.Controls.Clear();
			if (ActiveDoc != null) {
				lengthtb.Text = PropertySet[@"LENGTH"].Value.Replace("\"", string.Empty);
				widthtb.Text = PropertySet[@"WIDTH"].Value.Replace("\"", string.Empty);
				thicknesstb.Text = PropertySet[@"THICKNESS"].Value.Replace("\"", string.Empty);
				wallthicknesstb.Text = PropertySet[@"WALL THICKNESS"].Value.Replace("\"", string.Empty);

				GetRouting();

				if (Row != null) {
					length = (Single)Row[@"FIN_L"];
					width = (Single)Row[@"FIN_W"];
					thickness = (Single)Row[@"THICKNESS"];

					label18.Text = Redbrick.enforce_number_format(length);
					label19.Text = Redbrick.enforce_number_format(width);
					label20.Text = Redbrick.enforce_number_format(thickness);
				} else {
					label18.Text = Redbrick.enforce_number_format(PropertySet[@"LENGTH"].ResolvedValue);
					label19.Text = Redbrick.enforce_number_format(PropertySet[@"WIDTH"].ResolvedValue);
					label20.Text = Redbrick.enforce_number_format(PropertySet[@"THICKNESS"].ResolvedValue);
				}

				label21.Text = Redbrick.enforce_number_format(PropertySet[@"WALL THICKNESS"].ResolvedValue);

				//textBox_TextChanged(PropertySet[@"WALL THICKNESS"].Value, label21);

				flowLayoutPanel1.VerticalScroll.Value = scrollOffset.Y;
				float _val = 0.0F;
				if (float.TryParse(overLtb.Text, out _val)) {
					calculate_blanksize_from_oversize(_val, blnkszLtb, length, get_edge_thickness_total(edgel, edger));
				}

				if (float.TryParse(overWtb.Text, out _val)) {
					calculate_blanksize_from_oversize(_val, blnkszWtb, width, get_edge_thickness_total(edgef, edgeb));
				}

				groupBox1.Text = string.Format(@"{0} - {1}",
					partLookup, configuration);
			} else {
				Enabled = false;
			}
		}

		private void GetCutlistData() {
			ToggleNotInDBWarn(true);
			if (partLookup != null) {
				ENGINEERINGDataSet.CUT_PARTSDataTable dt =
					new ENGINEERINGDataSet.CUT_PARTSDataTable();
				cpta.FillByPartnum(dt, partLookup);
				if (dt.Count > 0) {
					Row = cpta.GetDataByPartnum(partLookup)[0];
				} else {
					Row = null;
				}
				cutlistPartsTableAdapter.FillByPartNum(eNGINEERINGDataSet.CutlistParts, partLookup);
				cutlistPartsBindingSource.DataSource = cutlistPartsTableAdapter.GetDataByPartNum(partLookup);
				cUTPARTSBindingSource.DataSource = cUT_PARTSTableAdapter.GetDataByPartnum(partLookup);

				SelectLastCutlist();
			}
			if (Row == null) {
				GetDataFromPart();
			}
		}

		private void SelectLastCutlist() {
			if (ComboBoxContainsValue(Properties.Settings.Default.LastCutlist, cutlistctl)) {
				cutlistctl.SelectedValue = Properties.Settings.Default.LastCutlist;
				ToggleCutlistWarn(false);
			} else {
				cutlistctl.SelectedValue = -1;
				ToggleCutlistWarn(true);
			}
		}

		private void ToggleDescrWarn(bool on) {
			if (on) {
				Redbrick.Warn(descriptiontb);
				descr_tooltup.SetToolTip(descriptiontb, Properties.Resources.NoDescriptionWarning);
				descr_tooltup.SetToolTip(label12, Properties.Resources.NoDescriptionWarning);
			} else {
				Redbrick.Unwarn(descriptiontb);
				descr_tooltup.RemoveAll();
			}
		}

		private void ToggleCutlistWarn(bool on) {
			if (on) {
				Redbrick.Warn(cutlistctl);
				cutlist_tooltip.SetToolTip(cutlistctl, Properties.Resources.CutlistNotSelectedWarning);
				cutlist_tooltip.SetToolTip(label11, Properties.Resources.CutlistNotSelectedWarning);
			} else {
				Redbrick.Unwarn(cutlistctl);
				cutlist_tooltip.RemoveAll();
			}
		}

		private void TogglePPBWarn(bool on) {
			if (on) {
				Redbrick.Warn(ppbtb);
				ppb_tooltip.SetToolTip(ppbtb, Properties.Resources.NotNaturalNumberWarning);
				ppb_tooltip.SetToolTip(label27, Properties.Resources.NotNaturalNumberWarning);
			} else {
				Redbrick.Unwarn(ppbtb);
				ppb_tooltip.RemoveAll();
			}
		}

		public static bool ComboBoxContainsValue(int value, ComboBox comboBox) {
			foreach (var item in comboBox.Items) {
				DataRowView drv = (item as DataRowView);
				if ((int)drv[comboBox.ValueMember] == value) {
					return true;
				}
			}
			return false;
		}

		private int IntTryProp(string propname) {
			int _out = 0;
			if (PropertySet.ContainsKey(propname)) {
				if (int.TryParse(PropertySet[propname].Value, out _out)) {
					return _out;
				}
			}
			return 0;
		}

		private string StrTryProp(string propname) {
			if (PropertySet.ContainsKey(propname)) {
				return PropertySet[propname].Value;
			}
			return string.Empty;
		}

		private bool BoolTryProp(string propname) {
			if (PropertySet.ContainsKey(propname)) {
				return PropertySet[propname].Value.ToUpper().Contains("YES");
			}
			return false;
		}

		private float DimTryProp(string propname) {
			float _out = 0;
			if (PropertySet.ContainsKey(propname)) {
				if (float.TryParse(PropertySet[propname].ResolvedValue, out _out)) {
					return _out;
				}
			}
			return _out;
		}

		private void ToggleNotInDBWarn(bool isIn) {
			data_from_db = isIn;
			if (isIn) {
				groupBox1.ForeColor = Properties.Settings.Default.NormalForeground;
				groupbox_tooltip.SetToolTip(groupBox1, Properties.Resources.InfoFromDB);
			} else {
				groupBox1.ForeColor = Properties.Settings.Default.WarnBackground;
				groupbox_tooltip.SetToolTip(groupBox1, Properties.Resources.InfoNotFromDB);
				foreach (Control control in groupBox1.Controls) {
					control.ForeColor = Properties.Settings.Default.NormalForeground;
				}
			}
		}

		private void GetDataFromPart() {
			ToggleNotInDBWarn(false);
			GetDepartmentFromPart();
			GetMaterialFromPart();
			GetEdgesFromPart();

			descriptiontb.Text = StrTryProp("Description");
			commenttb.Text = StrTryProp("COMMENT");
			cnc1tb.Text = StrTryProp("CNC1");
			cnc2tb.Text = StrTryProp("CNC2");
			ov_userediting = true;
			overLtb.Text = Redbrick.enforce_number_format(StrTryProp("OVERL"));
			ov_userediting = true;
			overWtb.Text = Redbrick.enforce_number_format(StrTryProp("OVERW"));
			ppbtb.Text = StrTryProp("BLANK QTY");
			updateCNCcb.Checked = BoolTryProp("UPDATE CNC");
			length = DimTryProp(@"LENGTH");
			width = DimTryProp(@"WIDTH");
			thickness = DimTryProp(@"THICKNESS");
		}

		private void GetDepartmentFromPart() {
			int type = IntTryProp(@"DEPTID");
			if (type < 1) {
				ENGINEERINGDataSetTableAdapters.CUT_PART_TYPESTableAdapter pta =
					new ENGINEERINGDataSetTableAdapters.CUT_PART_TYPESTableAdapter();
				string dept = StrTryProp(@"DEPARTMENT");
				if (dept != string.Empty) {
					type = (int)pta.GetIDByDescr(StrTryProp(@"DEPARTMENT"));
				}
			}
			type_cbx.SelectedValue = type;
		}

		private void GetMaterialFromPart() {
			int mr = (int)IntTryProp("MATID");
			if (mr < 1) {
				ENGINEERINGDataSet.CUT_MATERIALSDataTable md =
					new ENGINEERINGDataSet.CUT_MATERIALSDataTable();
				mr = md.GetMaterialIDByDescr(StrTryProp("CUTLIST MATERIAL"));
			}
			cutlistMat.SelectedValue = mr;
		}

		private void GetEdgesFromPart() {
			ENGINEERINGDataSet.CUT_EDGESDataTable ed =
				new ENGINEERINGDataSet.CUT_EDGESDataTable();
			int er = IntTryProp("EFID");
			if (er < 1) {
				er = ed.GetEdgeIDByDescr(StrTryProp("EDGE FRONT (L)"));
			}
			edgef.SelectedValue = er;
			er = IntTryProp("EBID");
			if (er < 1) {
				er = ed.GetEdgeIDByDescr(StrTryProp("EDGE BACK (L)"));
			}
			edgeb.SelectedValue = er;
			er = IntTryProp("ERID");
			if (er < 1) {
				er = ed.GetEdgeIDByDescr(StrTryProp("EDGE RIGHT (W)"));
			}
			edger.SelectedValue = er;

			er = IntTryProp("ELID");
			if (er < 1) {
				er = ed.GetEdgeIDByDescr(StrTryProp("EDGE LEFT (W)"));
			}
			edgel.SelectedValue = er;
		}

		private void GetRouting() {
			if (data_from_db) {
				GetRoutingFromDB();
			} else {
				GetRoutingFromPart();
			}
		}

		private void GetRoutingFromDB() {
			double setupTime = 0.0f;
			double runTime = 0.0f;
			if (eNGINEERINGDataSet.CutlistParts.Rows.Count > 0) {
				ENGINEERINGDataSet.CutlistPartsRow r =
					(ENGINEERINGDataSet.CutlistPartsRow)eNGINEERINGDataSet.CutlistParts.Rows[0];
				type_cbx.SelectedValue = r.TYPE;
				FilterOps(string.Format(@"TYPEID = {0}", r.TYPE));
			}
			ENGINEERINGDataSetTableAdapters.CutPartOpsTableAdapter cpota =
				new ENGINEERINGDataSetTableAdapters.CutPartOpsTableAdapter();
			cpota.FillBy(eNGINEERINGDataSet.CutPartOps, partLookup);

			for (int i = 0; i < cbxes.Length; i++) {
				ComboBox current = cbxes[i];
				if (i < eNGINEERINGDataSet.CutPartOps.Rows.Count) {
					ENGINEERINGDataSet.CutPartOpsRow r =
						(eNGINEERINGDataSet.CutPartOps.Rows[i] as ENGINEERINGDataSet.CutPartOpsRow);
					setupTime += r.POPSETUP;
					runTime += r.POPRUN;
					current.SelectedValue = r.POPOP;
				} else {
					current.SelectedValue = -1;
				}
			}

			double setup = setupTime / Properties.Settings.Default.SPQ;
			groupBox4.Text = string.Format("Routing (Setup: {0:0} min/Run: {1:0} min)",
				setup * 60,
				runTime * 60);
		}

		private void GetEstimationFromDB() {
			double setupTime = 0.0f;
			double runTime = 0.0f;
			for (int i = 0; i < cbxes.Length; i++) {
				ComboBox current = cbxes[i];
				DataRowView r = current.SelectedItem as DataRowView;
				if (r != null) {
					setupTime += (double)r[@"POPSETUP"];
					runTime += (double)r[@"POPRUN"];
				}
			}

			double setup = setupTime / Properties.Settings.Default.SPQ;
			groupBox4.Text = string.Format("Routing (Setup: {0:0} min/Run: {1:0} min)",
				setup * 60,
				runTime * 60);
		}

		private void GetEstimationFromPart() {
			double setupTime = 0.0f;
			double runTime = 0.0f;
			for (int i = 0; i < cbxes.Length; i++) {
				ComboBox current = cbxes[i];
				DataRowView r = current.SelectedItem as DataRowView;
				if (r != null) {
					setupTime += (double)r[@"OPSETUP"];
					runTime += (double)r[@"OPRUN"];
				}
			}

			double setup = setupTime / Properties.Settings.Default.SPQ;
			groupBox4.Text = string.Format("Routing (Setup: {0:0} min/Run: {1:0} min)",
				setup * 60,
				runTime * 60);
		}

		private void GetRoutingFromPart() {
			int type = (int)type_cbx.SelectedValue;
			int er = 0;

			for (int i = 0; i < cbxes.Length; i++) {
				string opid_name = string.Format("OP{0}ID", i + 1);
				string op_name = string.Format("OP{0}", i + 1);
				if (PropertySet.ContainsKey(opid_name)) {
					er = (int)PropertySet[opid_name].Data;
				}
				if (er < 1) {
					if (PropertySet.ContainsKey(op_name)) {
						er = (int)PropertySet[op_name].Data;
					}
				}

				if (er < 1) {
					er = -1;
				}

				cbxes[i].SelectedValue = er;
				GetEstimationFromPart();
			}
		}

		private void FilterOps(string filter) {
			if (filter == string.Empty) {
				filter = "1";
			}
			friendlyCutOpsBindingSource.Filter = filter;
			friendlyCutOpsBindingSource1.Filter = filter;
			friendlyCutOpsBindingSource2.Filter = filter;
			friendlyCutOpsBindingSource3.Filter = filter;
			friendlyCutOpsBindingSource4.Filter = filter;
		}

		private void GetOps() {
			flowLayoutPanel1.Controls.Clear();
			OpSets ops = new OpSets(PropertySet, partLookup);
			PropertySet.opSets = ops;
			foreach (OpSet op in ops) {
				OpControl opc = op.OperationControl;
				opc.Width = flowLayoutPanel1.Width - 25;
				flowLayoutPanel1.Controls.Add(opc);
				flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				opc.Anchor = AnchorStyles.Left | AnchorStyles.Right;
			}

			double setup = ops.TotalSetupTime / Properties.Settings.Default.SPQ;

			groupBox4.Text = string.Format("Routing (Setup: {0:0} min/Run: {1:0} min)",
				setup * 60,
				ops.TotalRunTime * 60);
		}

		private void ConnectDrawingEvents() {
			if (!DrawingEventsAssigned) {
				dd = (DrawingDoc)ActiveDoc;
				swSelMgr = ActiveDoc.SelectionManager;
				dd.UserSelectionPostNotify += dd_UserSelectionPostNotify;
				dd.DestroyNotify2 += dd_DestroyNotify2;
				DrawingEventsAssigned = true;
			}
		}

		int dd_DestroyNotify2(int DestroyType) {
			Hide();
			return 0;
		}

		int dd_UserSelectionPostNotify() {
			if (swSelMgr == null) {
				swSelMgr = ActiveDoc.SelectionManager;
			}
			object pt = swSelMgr.GetSelectionPoint2(swSelMgr.GetSelectedObjectCount2(-1), -1);
			object selectedObject = swSelMgr.GetSelectedObject6(1, -1);
			if (selectedObject == null) {
				selectedObject = swSelMgr.GetSelectedObjectsComponent4(1, -1);
			}
			if (selectedObject is SolidWorks.Interop.sldworks.View) {
				SolidWorks.Interop.sldworks.View v = (selectedObject as SolidWorks.Interop.sldworks.View);
				ReQuery(v.ReferencedDocument);
			} else if (selectedObject is DrawingComponent) {
				DrawingComponent dc = selectedObject as DrawingComponent;
				if (dc != null) {
					Component = dc.Component;
					ReQuery(Component.GetModelDoc2());
				} else {
					Component = null;
					ReQuery(SwApp.ActiveDoc);
				}
			}
			return 0;
		}

		private void ConnectPartEvents() {
			if (ActiveDoc.GetType() == (int)swDocumentTypes_e.swDocPART && !PartEventsAssigned) {
				pd = (PartDoc)ActiveDoc;
				// When the config changes, the app knows.
				pd.ActiveConfigChangePostNotify += pd_ActiveConfigChangePostNotify;
				pd.FileSavePostNotify += pd_FileSavePostNotify;
				//pd.ChangeCustomPropertyNotify += pd_ChangeCustomPropertyNotify;
				pd.DestroyNotify2 += pd_DestroyNotify2;
				//DisconnectDrawingEvents();
				PartEventsAssigned = true;
			}
		}

		private void DisconnectPartEvents() {
			// unhook 'em all
			if (PartEventsAssigned) {
				pd.ActiveConfigChangePostNotify -= pd_ActiveConfigChangePostNotify;
				pd.DestroyNotify2 -= pd_DestroyNotify2;
				//pd.ChangeCustomPropertyNotify -= pd_ChangeCustomPropertyNotify;
				pd.FileSavePostNotify -= pd_FileSavePostNotify;
			}
			PartEventsAssigned = false;
		}

		private void DisconnectEvents() {
			if (SwApp.ActiveDoc != lastModelDoc) {
				DisconnectAssemblyEvents();
				//lastModelDoc = SwApp.ActiveDoc;
			}
			DisconnectPartEvents();
			DisconnectDrawingEvents();
		}

		private void DisconnectDrawingEvents() {
			dd.UserSelectionPostNotify -= dd_UserSelectionPostNotify;
			DrawingEventsAssigned = false;
		}

		private void ConnectAssemblyEvents(ModelDoc2 md) {
			if ((md.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY) && !AssemblyEventsAssigned) {
				ad = (AssemblyDoc)md;
				swSelMgr = md.SelectionManager;
				//ad.UserSelectionPreNotify += ad_UserSelectionPreNotify;

				// user clicks part/subassembly
				ad.UserSelectionPostNotify += ad_UserSelectionPostNotify;

				// doc closing, I think.
				ad.DestroyNotify2 += ad_DestroyNotify2;

				// Not sure, and not implemented yet.
				//ad.ActiveDisplayStateChangePostNotify += ad_ActiveDisplayStateChangePostNotify;

				// switching docs
				ad.ActiveViewChangeNotify += ad_ActiveViewChangeNotify;
				//DisconnectDrawingEvents();
				AssemblyEventsAssigned = true;
			} else {
				// We're already set up, I guess.
			}
		}

		int ad_ActiveViewChangeNotify() {
			ActiveDoc = SwApp.ActiveDoc;
			return 0;
		}

		private int ad_DestroyNotify2(int DestroyType) {
			Visible = false;
			return 0;
		}

		private int ad_UserSelectionPostNotify() {
			// What do we got?
			if (swSelMgr == null) {
				swSelMgr = ActiveDoc.SelectionManager;
			}
			swSelComp = swSelMgr.GetSelectedObjectsComponent4(1, -1);
			if (swSelComp == null) {
				object selection_ = swSelMgr.GetSelectedObject6(1, -1);
				if (selection_ is Component2) {
					swSelComp = (Component2)selection_;
					this.Enabled = true;
				}
			}
			if (swSelComp != null) {
				Component = swSelComp;
				//try {
					configurationManager = (swSelComp.GetModelDoc2() as ModelDoc2).ConfigurationManager;
					configuration = swSelComp.ReferencedConfiguration;
					this.Enabled = true;
					ReQuery(swSelComp.GetModelDoc2());
				//} catch (NullReferenceException e) {
				//	Frame f = SwApp.Frame();
				//	f.SetStatusBarText(e.Message);
				//	this.Enabled = false;
				//}
			} else {
				// Nothing's selected?
				// Just look at the root item then.
				configurationManager = SwApp.ActiveDoc.ConfigurationManager;
				configuration = configurationManager.ActiveConfiguration.Name;
				this.Enabled = true;
				groupBox1.Text = string.Format(@"{0} - {1}",
					partLookup, PropertySet.Configuration);
				ReQuery(SwApp.ActiveDoc);
			}
			return 0;
		}

		private void DisconnectAssemblyEvents() {
			// unhook 'em all
			if (AssemblyEventsAssigned) {
				//ad.UserSelectionPreNotify -= ad_UserSelectionPreNotify;
				ad.UserSelectionPostNotify -= ad_UserSelectionPostNotify;
				ad.DestroyNotify2 -= ad_DestroyNotify2;
				//ad.ActiveDisplayStateChangePostNotify -= ad_ActiveDisplayStateChangePostNotify;
				ad.ActiveViewChangeNotify -= ad_ActiveViewChangeNotify;
				//swSelMgr = null;
			}
			AssemblyEventsAssigned = false;
		}

		private int pd_DestroyNotify2(int DestroyType) {
			Visible = false;
			return 0;
		}

		private int pd_FileSavePostNotify(int saveType, string FileName) {
			DumpActiveDoc();
			ActiveDoc = SwApp.ActiveDoc;
			return 0;
		}

		private int pd_ActiveConfigChangePostNotify() {
			ModelDoc2 md = ActiveDoc;
			DumpActiveDoc();
			_activeDoc = null;
			ReQuery(md);
			cutlistctl.SelectedIndex = -1;
			Properties.Settings.Default.LastCutlist = -1;
			ToggleCutlistWarn(true);
			return 0;
		}

		private string EnQuote(string stuff) {
			return string.Format("\"{0}\"", stuff.Replace("\"", string.Empty));
		}

		private void UpdateGeneralProperties() {
			DataRowView _drv = type_cbx.SelectedItem as DataRowView;
			PropertySet[@"DEPARTMENT"].Set(type_cbx.SelectedValue, _drv[@"TYPEDESC"].ToString());
			PropertySet[@"Description"].Data = descriptiontb.Text;

			// Dimensions get special treatment since the mgr stores weird strings, and DB stores doubles.
			PropertySet[@"LENGTH"].Set(label18.Text, EnQuote(lengthtb.Text));
			PropertySet[@"WIDTH"].Set(label19.Text, EnQuote(widthtb.Text));
			PropertySet[@"THICKNESS"].Set(label20.Text, EnQuote(thicknesstb.Text));
			PropertySet[@"WALL THICKNESS"].Set(label21.Text, EnQuote(wallthicknesstb.Text));

			PropertySet[@"COMMENT"].Data = commenttb.Text;
		}

		private void UpdateMachineProperties() {
			PropertySet[@"CNC1"].Data = cnc1tb.Text;
			PropertySet[@"CNC2"].Data = cnc2tb.Text;
			PropertySet[@"OVERL"].Data = overLtb.Text;
			PropertySet[@"OVERW"].Data = overWtb.Text;
			PropertySet[@"BLANK QTY"].Data = ppbtb.Text;
			PropertySet[@"UPDATE CNC"].Data = updateCNCcb.Checked;
		}

		private void UpdateCutlistProperties() {
			if (cutlistMat.SelectedItem != null) {
				DataRowView _drv = cutlistMat.SelectedItem as DataRowView;
				PropertySet[@"CUTLIST MATERIAL"].Set((int)cutlistMat.SelectedValue, _drv[@"DESCR"].ToString());
				PropertySet[@"MATID"].Set((int)cutlistMat.SelectedValue, cutlistMat.SelectedValue.ToString());
			}

			if (edgef.SelectedItem != null) {
				DataRowView _drv = edgef.SelectedItem as DataRowView;
				PropertySet[@"EDGE FRONT (L)"].Set((int)edgef.SelectedValue, _drv[@"DESCR"].ToString());
				PropertySet[@"EFID"].Set((int)edgef.SelectedValue, edgef.SelectedValue.ToString());
			}

			if (edgeb.SelectedItem != null) {
				DataRowView _drv = edgeb.SelectedItem as DataRowView;
				PropertySet[@"EDGE BACK (L)"].Set((int)edgeb.SelectedValue, _drv[@"DESCR"].ToString());
				PropertySet[@"EBID"].Set((int)edgeb.SelectedValue, edgeb.SelectedValue.ToString());
			}

			if (edgel.SelectedItem != null) {
				DataRowView _drv = edgel.SelectedItem as DataRowView;
				PropertySet[@"EDGE LEFT (W)"].Set((int)edgel.SelectedValue, _drv[@"DESCR"].ToString());
				PropertySet[@"ELID"].Set((int)edgel.SelectedValue, edgel.SelectedValue.ToString());
			}

			if (edger.SelectedItem != null) {
				DataRowView _drv = edger.SelectedItem as DataRowView;
				PropertySet[@"EDGE RIGHT (W)"].Set((int)edger.SelectedValue, _drv[@"DESCR"].ToString());
				PropertySet[@"ERID"].Set((int)edger.SelectedValue, edger.SelectedValue.ToString());
			}
		}

		private void UpdateRoutingProperties() {
			for (int i = 0; i < cbxes.Length; i++) {
				ComboBox cbx = cbxes[i];
				string op = string.Format(@"OP{0}", i + 1);
				string opid = string.Format(@"OP{0}ID", i + 1);
				if (cbx.SelectedItem != null) {
					DataRowView drv = (cbx.SelectedItem as DataRowView);
					PropertySet[op].Set((int)cbx.SelectedValue, drv[@"OPNAME"].ToString());
					PropertySet[opid].Set((int)cbx.SelectedValue, cbx.SelectedItem.ToString());
				} else {
					PropertySet[op].Set(0, string.Empty);
					PropertySet[opid].Set(0, @"0");
				}
			}
		}

		public void Commit() {
			if (tabControl1.SelectedTab == tabPage1) {
				UpdateGeneralProperties();
				UpdateMachineProperties();
				UpdateCutlistProperties();
				UpdateRoutingProperties();

				PropertySet.Write();
				if (data_from_db) {
					GetEstimationFromDB();
				} else {
					GetEstimationFromPart();
				}
			} else if (tabControl1.SelectedTab == tabPage2) {
				drawingRedbrick.Commit();
			}
		}

		private void SetupDrawing() {
			if (!DrawingSetup) {
				drawingRedbrick = new DrawingRedbrick(ActiveDoc, SwApp);
				tabPage2.Controls.Add(drawingRedbrick);
				drawingRedbrick.Dock = DockStyle.Fill;
				DrawingSetup = true;
			} else {
				drawingRedbrick.ReLoad(SwApp.ActiveDoc);
			}

			if (!DrawingEventsAssigned) {
				ConnectDrawingEvents();
			}
			this.Enabled = true;
			//tabControl1.SelectedTab = tabPage2;
		}

		private void SetupPart() {
			scrollOffset = new Point(0, flowLayoutPanel1.VerticalScroll.Value);
			PropertySet = new SwProperties(SwApp, ActiveDoc);

			if (!(_activeDoc is DrawingDoc)) {
				configuration = _activeDoc.ConfigurationManager.ActiveConfiguration.Name;
			}

			if (Component != null) {
				configuration = Component.ReferencedConfiguration;
				PropertySet.Configuration = configuration;
				PropertySet.GetProperties(Component);
			} else {
				configuration = _activeDoc.ConfigurationManager.ActiveConfiguration.Name;
				PropertySet.Configuration = configuration;
				PropertySet.GetProperties(_activeDoc);
			}

			lastModelDoc = _activeDoc;
			DisconnectPartEvents();
			ConnectPartEvents();
			ReQuery();
		}

		public int PartID { get; set; }
		public int Hash { get; set; }
		public FileInfo PartFileInfo { get; set; }

		private Component2 _component;

		public Component2 Component {
			get { return _component; }
			set {
				_component = value;
			}
		}

		private ModelDoc2 _activeDoc;

		public ModelDoc2 ActiveDoc {
			get { return _activeDoc; }
			set {
				//allowPaint = false;
				if (value != null && value != ActiveDoc) {
					Show();
					lastModelDoc = _activeDoc;
					_activeDoc = value;

					string _fn = _activeDoc.GetPathName();
					if (_fn != string.Empty) {
						PartFileInfo = new FileInfo(_fn);
						partLookup = Path.GetFileNameWithoutExtension(PartFileInfo.Name).Split(' ')[0];
					} else {
						partLookup = null;
						PartFileInfo = new FileInfo(Path.GetTempFileName());
						Hash = Redbrick.GetHash(PartFileInfo.FullName);
					}

					swDocumentTypes_e dType = (swDocumentTypes_e)_activeDoc.GetType();
					swDocumentTypes_e odType = (swDocumentTypes_e)(SwApp.ActiveDoc as ModelDoc2).GetType();
					PropertySet = new SwProperties(SwApp, _activeDoc);
					switch (odType) {
						case swDocumentTypes_e.swDocASSEMBLY:                     //Window looking at assembly.
							(tabPage1 as Control).Enabled = true;
							DisconnectAssemblyEvents();
							ConnectAssemblyEvents(SwApp.ActiveDoc as ModelDoc2);
							switch (dType) {
								case swDocumentTypes_e.swDocASSEMBLY:                     //Selected sub-assembly in window.
									(tabPage2 as Control).Enabled = false;
									SetupPart();
									break;
								case swDocumentTypes_e.swDocDRAWING:
									break;
								case swDocumentTypes_e.swDocPART:                         //Selected on part in window.
									(tabPage2 as Control).Enabled = false;
									SetupPart();
									break;
								default:
									break;
							}
							tabControl1.SelectedTab = tabPage1;
							break;
						case swDocumentTypes_e.swDocDRAWING:                      //Window looking at drawing.
							(tabPage2 as Control).Enabled = true;
							switch (dType) {
								case swDocumentTypes_e.swDocASSEMBLY:                     //Selected assembly in drawing.
									(tabPage1 as Control).Enabled = true;
									SetupPart();
									tabControl1.SelectedTab = tabPage1;
									break;
								case swDocumentTypes_e.swDocDRAWING:
									tabControl1.SelectedTab = tabPage2;
									break;
								case swDocumentTypes_e.swDocPART:                         //Selected part in drawing.
									(tabPage1 as Control).Enabled = true;
									SetupPart();
									tabControl1.SelectedTab = tabPage1;
									break;
								default:
									break;
							}
							SetupDrawing();
							break;
						case swDocumentTypes_e.swDocPART:                         //Window looking at part.
							Component = null;
							(tabPage1 as Control).Enabled = true;
							if (odType != swDocumentTypes_e.swDocDRAWING) {
								(tabPage2 as Control).Enabled = false;
							} else {
								(tabPage2 as Control).Enabled = true;
							}
							SetupPart();
							tabControl1.SelectedTab = tabPage1;
							break;
						default:
							//Hide();
							break;
					}
				} else {
					if (value == null) {
						//Hide();
					}
				}
				allowPaint = true;
			}
		}

		public SldWorks SwApp { get; set; }

		private void ModelRedbrick_Load(object sender, EventArgs e) {
			cUT_MATERIALSTableAdapter.Fill(eNGINEERINGDataSet.CUT_MATERIALS);
			cUT_EDGESTableAdapter.Fill(eNGINEERINGDataSet.CUT_EDGES);
			cUT_PART_TYPESTableAdapter.Fill(eNGINEERINGDataSet.CUT_PART_TYPES);
			friendlyCutOpsTableAdapter.Fill(eNGINEERINGDataSet.FriendlyCutOps);

			//GetCutlistData();
			//SelectTab();
			cbxes = new ComboBox[] { op1_cbx, op2_cbx, op3_cbx, op4_cbx, op5_cbx };
			initialated = true;
		}

		private void comboBox_KeyDown(object sender, KeyEventArgs e) {
			(sender as ComboBox).DroppedDown = false;
		}

		private void comboBox_TextChanged(object sender, EventArgs e) {

		}

		private string GetDim(string prp) {
			Dimension d = ActiveDoc.Parameter(prp);
			if (d != null) {
				return d.Value.ToString();
			} else {
				return DimensionByEquation(prp);
			}
		}

		private string DimensionByEquation(string equation) {
			string res = string.Empty;
			if (ActiveDoc == null) {
				return @"#VALUE!";
			}
			EquationMgr eqm = ActiveDoc.GetEquationMgr();
			for (int i = 0; i < eqm.GetCount(); i++) {
				if (eqm.get_Equation(i).Contains(equation)) {
					return eqm.get_Value(i).ToString();
				}
			}
			return @"#VALUE!";
		}

		private void textBox_TextChanged(string dim, Label l) {
			if (ov_userediting && initialated && ActiveDoc.GetType() != (int)swDocumentTypes_e.swDocDRAWING) {
				string dimension = dim.
					Replace(@"@" + PartFileInfo.Name, string.Empty).
					Replace(@"@" + ActiveDoc.ConfigurationManager.ActiveConfiguration.Name, string.Empty).
					Trim('"');
				double _val;
				if (double.TryParse(GetDim(dimension), out _val)) {
					l.Text = Redbrick.enforce_number_format(_val);
				} else {
					l.Text = @"#VALUE!";
				}
			}
		}

		private void clip_click(object sender, EventArgs e) {
			Redbrick.Clip((sender as Control).Text);
		}

		private void textBox2_TextChanged(object sender, EventArgs e) {
			TextBox _tb = (sender as TextBox);
			textBox_TextChanged(_tb.Text, label18);
		}

		private void textBox3_TextChanged(object sender, EventArgs e) {
			TextBox _tb = (sender as TextBox);
			textBox_TextChanged(_tb.Text, label19);
		}

		private void textBox4_TextChanged(object sender, EventArgs e) {
			TextBox _tb = (sender as TextBox);
			textBox_TextChanged(_tb.Text, label20);
		}

		private void textBox5_TextChanged(object sender, EventArgs e) {
			TextBox _tb = (sender as TextBox);
			textBox_TextChanged(_tb.Text, label21);
		}

		private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) {
			if ((sender as ComboBox).SelectedIndex == -1) {
				label7.Visible = false;
			} else {
				label7.Visible = true;
			}
		}

		private void comboBox3_SelectedIndexChanged(object sender, EventArgs e) {
			if ((sender as ComboBox).SelectedIndex == -1) {
				label8.Visible = false;
			} else {
				label8.Visible = true;
			}
		}

		private void comboBox4_SelectedIndexChanged(object sender, EventArgs e) {
			if ((sender as ComboBox).SelectedIndex == -1) {
				label9.Visible = false;
			} else {
				label9.Visible = true;
			}
		}

		private void comboBox5_SelectedIndexChanged(object sender, EventArgs e) {
			if ((sender as ComboBox).SelectedIndex == -1) {
				label10.Visible = false;
			} else {
				label10.Visible = true;
			}
		}

		private void label1_Click(object sender, EventArgs e) {
			Redbrick.Clip(cutlistMat.Text);
		}

		private void label2_Click(object sender, EventArgs e) {
			Redbrick.Clip(edgef.Text);
		}

		private void label3_Click(object sender, EventArgs e) {
			Redbrick.Clip(edgeb.Text);
		}

		private void label4_Click(object sender, EventArgs e) {
			Redbrick.Clip(edgel.Text);
		}

		private void label5_Click(object sender, EventArgs e) {
			Redbrick.Clip(edger.Text);
		}

		private void label11_Click(object sender, EventArgs e) {
			Redbrick.Clip(cutlistctl.Text);
		}

		private void label12_Click(object sender, EventArgs e) {
			Redbrick.Clip(descriptiontb.Text);
		}

		private void button1_Click(object sender, EventArgs e) {
			string _filename = Path.GetFileNameWithoutExtension(PartFileInfo.Name);
			string _lookup = _filename.Split(' ')[0];
			Machine_Priority_Control.MachinePriority mp =
				new Machine_Priority_Control.MachinePriority(_lookup);
			mp.Show(this);
		}

		private void textBox_Enter(object sender, EventArgs e) {
			ov_userediting = true;
		}

		private void textBox_Leave(object sender, EventArgs e) {
			ov_userediting = false;
		}

		private void button2_Click(object sender, EventArgs e) {
			
		}

		private void textBox12_TextChanged(object sender, EventArgs e) {
			TextBox _me = (sender as TextBox);
			Single _test = 0.0F;
			Single _length = 0.0F;

			if (bl_userediting && Single.TryParse(_me.Text, out _test) && Single.TryParse(label18.Text, out _length)) {
				Single _edge_thickness = 0.0F;

				if (edgel.SelectedItem != null) {
					_edge_thickness += (Single)(edgel.SelectedItem as DataRowView)[@"THICKNESS"];
				}

				if (edger.SelectedItem != null) {
					_edge_thickness += (Single)(edger.SelectedItem as DataRowView)[@"THICKNESS"];
				}
				bl_userediting = false;
				calculate_oversize_from_blanksize(_test, overLtb, length, _edge_thickness);
			}
		}

		private void textBox13_TextChanged(object sender, EventArgs e) {
			TextBox _me = (sender as TextBox);
			Single _test = 0.0F;
			Single _width = 0.0F;
			if (bl_userediting && Single.TryParse(_me.Text, out _test) && Single.TryParse(label19.Text, out _width)) {
				Single _edge_thickness = 0.0F;

				if (edgef.SelectedItem != null) {
					_edge_thickness += (Single)(edgef.SelectedItem as DataRowView)[@"THICKNESS"];
				}

				if (edgeb.SelectedItem != null) {
					_edge_thickness += (Single)(edgeb.SelectedItem as DataRowView)[@"THICKNESS"];
				}
				bl_userediting = false;
				calculate_oversize_from_blanksize(_test, overWtb, width, _edge_thickness);
			}
		}

		private void bl_textBox_KeyDown(object sender, KeyEventArgs e) {
			//TextBox _me = (sender as TextBox);
			bl_userediting = true;
		}

		private void bl_textBox_KeyUp(object sender, KeyEventArgs e) {
			//TextBox _me = (sender as TextBox);
			bl_userediting = false;
		}

		private void ov_textBox_KeyDown(object sender, KeyEventArgs e) {
			ov_userediting = true;
		}

		private void dimension_textBox_Leave(object sender, EventArgs e) {
			TextBox _me = (sender as TextBox);
			string _text = Redbrick.enforce_number_format(_me.Text);
			_me.Text = Redbrick.enforce_number_format(_text);
		}

		static private Single get_edge_thickness_total(ComboBox c1, ComboBox c2) {
			Single _edge_thickness = 0.0F;
			if (c1.SelectedItem != null) {
				_edge_thickness += (Single)(c1.SelectedItem as DataRowView)[@"THICKNESS"];
			}

			if (c2.SelectedItem != null) {
				_edge_thickness += (Single)(c2.SelectedItem as DataRowView)[@"THICKNESS"];
			}
			return _edge_thickness;
		}

		static private void calculate_blanksize_from_oversize(Single ov_box_val, TextBox bl_box, Single length, Single total_edging) {
			Decimal _val = Math.Round(Convert.ToDecimal((length + ov_box_val) - total_edging), 3);
			bl_box.Text = Redbrick.enforce_number_format(_val);
		}

		static private void calculate_oversize_from_blanksize(Single bl_box_val, TextBox ov_box, Single length, Single total_edging) {
			Decimal _val = Math.Round(Convert.ToDecimal((bl_box_val - length) + total_edging), 3);
			ov_box.Text = Redbrick.enforce_number_format(_val);
		}

		private void comboBox_Resize(object sender, EventArgs e) {
			ComboBox _me = (sender as ComboBox);
			_me.SelectionLength = 0;
		}

		private void label6_Click(object sender, EventArgs e) {
			CreateCutlist c = new CreateCutlist(SwApp);
			c.ShowDialog(this);
		}

		private void comboBox_SelectedIndexChanged(object sender, EventArgs e) {
			cutlistMatChanged = true;
		}

		private void comboBox6_SelectedIndexChanged(object sender, EventArgs e) {
			if (cl_userediting) {
				ToggleCutlistWarn(false);
				Properties.Settings.Default.LastCutlist = (int)(sender as ComboBox).SelectedValue;
				Properties.Settings.Default.Save();
				cl_userediting = false;
			}
		}

		private void comboBox6_MouseClick(object sender, MouseEventArgs e) {
			cl_userediting = true;
			FocusHere(sender, e);
		}

		private void textBox7_TextChanged(object sender, EventArgs e) {
			TogglePriorityButton();
		}

		private void textBox8_TextChanged(object sender, EventArgs e) {
			TogglePriorityButton();
		}

		private void comboBox12_SelectedIndexChanged(object sender, EventArgs e) {
			FilterOps(string.Format(@"TYPEID = {0}", (sender as ComboBox).SelectedValue));
		}

		private void button3_Click(object sender, EventArgs e) {
			if (eNGINEERINGDataSet.CutPartOps.Rows.Count > 0) {
				ENGINEERINGDataSet.CutPartOpsRow r = 
					(eNGINEERINGDataSet.CutPartOps.Rows[0] as ENGINEERINGDataSet.CutPartOpsRow);
				EditOp eo = new EditOp(r);
				eo.ShowDialog(this);
			}
			GetRouting();
		}

		private void button4_Click(object sender, EventArgs e) {
			if (eNGINEERINGDataSet.CutPartOps.Rows.Count > 1) {
				ENGINEERINGDataSet.CutPartOpsRow r =
					(eNGINEERINGDataSet.CutPartOps.Rows[1] as ENGINEERINGDataSet.CutPartOpsRow);
				EditOp eo = new EditOp(r);
				eo.ShowDialog(this);
			}
			GetRouting();
		}

		private void button5_Click(object sender, EventArgs e) {
			if (eNGINEERINGDataSet.CutPartOps.Rows.Count > 2) {
				ENGINEERINGDataSet.CutPartOpsRow r =
					(eNGINEERINGDataSet.CutPartOps.Rows[2] as ENGINEERINGDataSet.CutPartOpsRow);
				EditOp eo = new EditOp(r);
				eo.ShowDialog(this);
			}
			GetRouting();
		}

		private void button6_Click(object sender, EventArgs e) {
			if (eNGINEERINGDataSet.CutPartOps.Rows.Count > 3) {
				ENGINEERINGDataSet.CutPartOpsRow r =
					(eNGINEERINGDataSet.CutPartOps.Rows[3] as ENGINEERINGDataSet.CutPartOpsRow);
				EditOp eo = new EditOp(r);
				eo.ShowDialog(this);
			}
			GetRouting();
		}

		private void button7_Click(object sender, EventArgs e) {
			if (eNGINEERINGDataSet.CutPartOps.Rows.Count > 4) {
				ENGINEERINGDataSet.CutPartOpsRow r =
					(eNGINEERINGDataSet.CutPartOps.Rows[4] as ENGINEERINGDataSet.CutPartOpsRow);
				EditOp eo = new EditOp(r);
				eo.ShowDialog(this);
			}
			GetRouting();
		}

		private void textBox1_TextChanged(object sender, EventArgs e) {
			if ((sender as TextBox).Text == string.Empty) {
				ToggleDescrWarn(true);
			} else {
				ToggleDescrWarn(false);
			}
		}

		private void textBox11_TextChanged(object sender, EventArgs e) {
			if ((sender as TextBox).Text == string.Empty || (sender as TextBox).Text == @" ") {
				TogglePPBWarn(true);
			} else {
				TogglePPBWarn(false);
			}
		}

		private void comboBox_validating(object sender, CancelEventArgs e) {
			ComboBox cbx = sender as ComboBox;
			if (cbx.Text != string.Empty) {
				cbx.SelectedIndex = cbx.FindStringExact(cbx.Text);
			} else {
				cbx.SelectedIndex = -1;
			}
		}

		private void FocusHere(object sender, MouseEventArgs e) {
			if (sender is ComboBox) {
				if ((sender as ComboBox).DroppedDown) {
					//
				} else {
					(sender as ComboBox).Focus();
				}
			} else if (sender is TextBox) {
				(sender as TextBox).Focus();
			} else if (sender is NumericUpDown) {
				(sender as NumericUpDown).Focus();
			} else if (sender is CheckBox) {
				(sender as CheckBox).Focus();
			}
		}
	}
}
