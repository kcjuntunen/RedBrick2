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
	/// <summary>
	/// The heart to the Redbrick. Everything starts here.
	/// </summary>
	public partial class ModelRedbrick : UserControl {
		private SwProperties PropertySet;
		private ENGINEERINGDataSetTableAdapters.CUT_PARTSTableAdapter cpta =
			new ENGINEERINGDataSetTableAdapters.CUT_PARTSTableAdapter();
		private ENGINEERINGDataSetTableAdapters.CUT_CUTLIST_PARTSTableAdapter ccpta =
			new ENGINEERINGDataSetTableAdapters.CUT_CUTLIST_PARTSTableAdapter();
		private ENGINEERINGDataSetTableAdapters.CUT_PART_OPSTableAdapter cpota =
			new ENGINEERINGDataSetTableAdapters.CUT_PART_OPSTableAdapter();

		private ENGINEERINGDataSet.CUT_PARTSRow Row = null;
		private ENGINEERINGDataSet.CUT_CUTLIST_PARTSRow CutlistPartsRow = null;

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

		private Single db_length = 0.0F;
		private Single db_width = 0.0F;
		private Single db_thickness = 0.0F;


		private Point scrollOffset;

		private const int WM_PAINT = 0x000F;
		private bool allowPaint;

		private ModelDoc2 lastModelDoc = null;
		private DirtTracker dirtTracker;

		private bool initialated = false;
		private bool AssemblyEventsAssigned = false;
		private bool PartEventsAssigned = false;
		private bool DrawingEventsAssigned = false;
		private bool DrawingSetup = false;
		private bool checked_at_start = false;
		private bool ov_userediting = false;
		private bool bl_userediting = false;
		private bool cl_userediting = false;
		private bool cl_stat_userediting = false;
		private bool data_from_db = false;
		private bool do_savepostnotify = true;
		private string req_info_ = string.Empty;
		private string mat_price_ = string.Empty;
		private string edgef_price_ = string.Empty;
		private string edgeb_price_ = string.Empty;
		private string edgel_price_ = string.Empty;
		private string edger_price_ = string.Empty;
		private ComboBox[] cbxes;
		private ToolTip groupbox_tooltip = new ToolTip();
		private ToolTip cutlistMat_tooltip = new ToolTip();
		private ToolTip cutlist_tooltip = new ToolTip();
		private ToolTip edging_tooltip = new ToolTip();
		private ToolTip descr_tooltup = new ToolTip();
		private ToolTip swap_tooltup = new ToolTip();
		private ToolTip ppb_tooltip = new ToolTip();
		private ToolTip partq_tooltip = new ToolTip();
		private ToolTip over_tooltip = new ToolTip();
		private ToolTip type_tooltip = new ToolTip();
		private ToolTip req_tooltip = new ToolTip();
		private ToolTip l_tooltip = new ToolTip();
		private ToolTip w_tooltip = new ToolTip();
		private ToolTip t_tooltip = new ToolTip();
		private ToolTip cnc1_tooltip = new ToolTip();
		private ToolTip cnc2_tooltip = new ToolTip();
		private ToolTip cutlist_mat_tip = new ToolTip();
		private ToolTip edgef_mat_tip = new ToolTip();
		private ToolTip edgeb_mat_tip = new ToolTip();
		private ToolTip edgel_mat_tip = new ToolTip();
		private ToolTip edger_mat_tip = new ToolTip();
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="sw">The connected application.</param>
		/// <param name="md">An initial ModelDoc2.</param>
		public ModelRedbrick(SldWorks sw, ModelDoc2 md) {
			SwApp = sw;
			InitializeComponent();
			new ToolTip().SetToolTip(pull_btn, @"Pull material properties from part.");
			dirtTracker = new DirtTracker(this);
			cbxes = new ComboBox[] { op1_cbx, op2_cbx, op3_cbx, op4_cbx, op5_cbx };
			ToggleFlameWar(Properties.Settings.Default.FlameWar);

			ActiveDoc = md;
			tabPage1.Text = @"Model Properties";
			tabPage2.Text = @"DrawingProperties";

			AutoCompleteStringCollection skdim_ = new AutoCompleteStringCollection();
			for (int i = 0; i < Properties.Settings.Default.SketchDimensions.Count; i++) {
				skdim_.Add(Properties.Settings.Default.SketchDimensions[i]);
			}

			lengthtb.AutoCompleteCustomSource = skdim_;
			widthtb.AutoCompleteCustomSource = skdim_;
			thicknesstb.AutoCompleteCustomSource = skdim_;
			wallthicknesstb.AutoCompleteCustomSource = skdim_;

			groupBox1.MouseClick += groupBox1_MouseClick;
			groupBox4.MouseClick += groupBox4_MouseClick;
			label6.MouseDown += clip_click;
			label7.MouseDown += clip_click;
			label8.MouseDown += clip_click;
			label9.MouseDown += clip_click;
			label10.MouseDown += clip_click;

			overLtb.TextChanged += overL_TextChanged;
			overWtb.TextChanged += overW_TextChanged;
			dirtTracker.Besmirched += dirtTracker_Besmirched;

			Control[] btns_ = new Control[] { button3, button4, button5, button6, button7 };
			foreach (Control item_ in btns_) {
				new ToolTip().SetToolTip(item_, Properties.Resources.TimeHint);
			}
			new ToolTip().SetToolTip(prioritybtn, Properties.Resources.PriorityHint);
			new ToolTip().SetToolTip(update_btn, Properties.Resources.UpdateHint);
			new ToolTip().SetToolTip(add_prt_btn, Properties.Resources.AddPartHint);
			new ToolTip().SetToolTip(remove_btn, Properties.Resources.RemoveHint);
		}

		void dirtTracker_Besmirched(object sender, EventArgs e) {
			if (!groupBox1.Text.StartsWith(Properties.Settings.Default.NotSavedMark)) {
				groupBox1.Text = Properties.Settings.Default.NotSavedMark + groupBox1.Text;
			}
		}

		/// <summary>
		/// Decide whether to turn the "Machine Priority" button off or on.
		/// </summary>
		public void TogglePriorityButton() {
			bool no_cnc1 = (cnc1tb.Text == @"NA") || (cnc1tb.Text == string.Empty);
			bool no_cnc2 = (cnc2tb.Text == @"NA") || (cnc2tb.Text == string.Empty);
			bool enabled = data_from_db && !(no_cnc1 && no_cnc2);
			prioritybtn.Enabled = enabled;
		}

		/// <summary>
		/// Make text gross and unpleasant.
		/// </summary>
		/// <param name="on">True or false.</param>
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

		/// <summary>
		/// Trying to pause drawing reduce flickering. Doing so causes the Redbrick
		/// to hang though. So "allowPaint" is always true.
		/// </summary>
		/// <param name="m">A Message value.</param>
		protected override void WndProc(ref Message m) {
			if ((m.Msg != WM_PAINT || (allowPaint && m.Msg == WM_PAINT))) {
				base.WndProc(ref m);
			}
		}

		void groupBox1_MouseClick(object sender, MouseEventArgs e) {
			Redbrick.Clip(partLookup);
		}

		void groupBox4_MouseClick(object sender, MouseEventArgs e) {
			if (data_from_db) {
				Properties.Settings.Default.EstimateSource = !Properties.Settings.Default.EstimateSource;
				Properties.Settings.Default.Save();
				GetEstimationFromDB();
			} else {
				GetEstimationFromPart();
			}
		}

		void overL_TextChanged(object sender, EventArgs e) {
			if (initialated) {
				if (ov_userediting) {
					Single _edge_thickness = 0.0F;
					if (edgel.SelectedItem != null) {
						_edge_thickness += Convert.ToSingle((edgel.SelectedItem as DataRowView)[@"THICKNESS"]);
					}

					if (edger.SelectedItem != null) {
						_edge_thickness += Convert.ToSingle((edger.SelectedItem as DataRowView)[@"THICKNESS"]);
					}
					float test_ = 0.0F;
					if (float.TryParse((sender as TextBox).Text, out test_)) {
						calculate_blanksize_from_oversize(test_, blnkszLtb, length, _edge_thickness);
					}
					ov_userediting = false;
				}
			}
			//overLtb.Text = Redbrick.enforce_number_format(overLtb.Text);
		}

		void overW_TextChanged(object sender, EventArgs e) {
			if (initialated) {
				if (ov_userediting) {
					Single _edge_thickness = 0.0F;
					if (edgef.SelectedItem != null) {
						_edge_thickness += Convert.ToSingle((edgef.SelectedItem as DataRowView)[@"THICKNESS"]);
					}

					if (edgeb.SelectedItem != null) {
						_edge_thickness += Convert.ToSingle((edgeb.SelectedItem as DataRowView)[@"THICKNESS"]);
					}
					float test_ = 0.0F;
					if (float.TryParse((sender as TextBox).Text, out test_)) {
						calculate_blanksize_from_oversize(test_, blnkszWtb, width, _edge_thickness);
					}
					ov_userediting = false;
				}
			}
			//overWtb.Text = Redbrick.enforce_number_format(overWtb.Text);
		}

		/// <summary>
		/// Dispose of whatever we have.
		/// </summary>
		public void DumpActiveDoc() {
			this.Component = null;
			configuration = string.Empty;
			lastModelDoc = null;
			_activeDoc = null;
		}

		public void ReStart() {
			cUT_MATERIALSTableAdapter.Fill(eNGINEERINGDataSet.CUT_MATERIALS);
			cUT_EDGESTableAdapter.Fill(eNGINEERINGDataSet.CUT_EDGES);
			ReQuery(SwApp.ActiveDoc);
		}

		/// <summary>
		/// Query over again.
		/// </summary>
		/// <param name="md">ModelDoc2 to query.</param>
		public void ReQuery(ModelDoc2 md) {
			ActiveDoc = md;
		}

		private void ReQuery() {
			Properties.Settings.Default.NumberFormat = get_format_txt(_activeDoc);
			Properties.Settings.Default.Save();
			dirtTracker.IsDirty = false;
			groupBox1.Text = groupBox1.Text.Replace(Properties.Settings.Default.NotSavedMark, string.Empty);
			GetCutlistData();
			flowLayoutPanel1.Controls.Clear();
			if (ActiveDoc != null && PropertySet.Count > 0) {
				Enabled = true;
				lengthtb.Text = PropertySet[@"LENGTH"].Value.Replace("\"", string.Empty);
				widthtb.Text = PropertySet[@"WIDTH"].Value.Replace("\"", string.Empty);
				thicknesstb.Text = PropertySet[@"THICKNESS"].Value.Replace("\"", string.Empty);
				wallthicknesstb.Text = PropertySet[@"WALL THICKNESS"].Value.Replace("\"", string.Empty);

				GetRouting();

				if (Row != null) {
					db_length = (Single)Row[@"FIN_L"];
					db_width = (Single)Row[@"FIN_W"];
					db_thickness = (Single)Row[@"THICKNESS"];

					length = DimTryProp(@"LENGTH");
					width = DimTryProp(@"WIDTH");
					thickness = DimTryProp(@"THICKNESS");

					length_label.Text = Redbrick.enforce_number_format(length);
					width_label.Text = Redbrick.enforce_number_format(width);
					thickness_label.Text = Redbrick.enforce_number_format(thickness);
					CheckDims();
				} else {
					length_label.Text = Redbrick.enforce_number_format(PropertySet[@"LENGTH"].ResolvedValue);
					width_label.Text = Redbrick.enforce_number_format(PropertySet[@"WIDTH"].ResolvedValue);
					thickness_label.Text = Redbrick.enforce_number_format(PropertySet[@"THICKNESS"].ResolvedValue);
					UnErrDims();
				}

				wall_thickness_label.Text = Redbrick.enforce_number_format(PropertySet[@"WALL THICKNESS"].ResolvedValue);
				CheckThickness();
				CheckEdgingOps();

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

		private void CheckDims() {
			if (!Redbrick.FloatEquals(db_length, length)) {
				l_tooltip.SetToolTip(length_label,
					string.Format(Properties.Resources.DimensionNotMatch,
					Redbrick.enforce_number_format(db_length)));
				Redbrick.Warn(length_label);
			} else {
				Redbrick.UnErr(length_label);
				l_tooltip.RemoveAll();
			}

			if (!Redbrick.FloatEquals(db_width, width)) {
				w_tooltip.SetToolTip(width_label,
					string.Format(Properties.Resources.DimensionNotMatch,
					Redbrick.enforce_number_format(db_width)));
				Redbrick.Warn(width_label);
			} else {
				Redbrick.UnErr(width_label);
				w_tooltip.RemoveAll();
			}

			if (!Redbrick.FloatEquals(db_thickness, thickness)) {
				t_tooltip.SetToolTip(thickness_label,
					string.Format(Properties.Resources.DimensionNotMatch,
					Redbrick.enforce_number_format(db_thickness)));
				Redbrick.Warn(thickness_label);
			} else {
				Redbrick.UnErr(thickness_label);
				t_tooltip.RemoveAll();
			}
		}

		private void UnErrDims() {
			Redbrick.UnErr(length_label);
			Redbrick.UnErr(width_label);
			Redbrick.UnErr(thickness_label);
			l_tooltip.RemoveAll();
			w_tooltip.RemoveAll();
			t_tooltip.RemoveAll();
		}

		private void GetCutlistData() {
			if (partLookup != null) {
				cpta.FillByPartnum(eNGINEERINGDataSet.CUT_PARTS, partLookup);
				if (eNGINEERINGDataSet.CUT_PARTS.Count > 0) {
					Row = eNGINEERINGDataSet.CUT_PARTS.Rows[0] as ENGINEERINGDataSet.CUT_PARTSRow;
					PropertySet.PartID = Row.PARTID;
					ToggleNotInDBWarn(true);
				} else {
					Row = null;
					CutlistPartsRow = null;
				}

				cutlistPartsTableAdapter.FillByPartNum(eNGINEERINGDataSet.CutlistParts, partLookup);
				cutlistPartsBindingSource.DataSource = cutlistPartsTableAdapter.GetDataByPartNum(partLookup);
				cUTPARTSBindingSource.DataSource = cUT_PARTSTableAdapter.GetDataByPartnum(partLookup);
				overLtb.Text = Redbrick.enforce_number_format(overLtb.Text);
				overWtb.Text = Redbrick.enforce_number_format(overWtb.Text);
				blnkszLtb.Text = Redbrick.enforce_number_format(blnkszLtb.Text);
				blnkszWtb.Text = Redbrick.enforce_number_format(blnkszWtb.Text);
				checked_at_start = updateCNCcb.Checked;
			}
			if (Row != null) {
				cpota.FillByPartID(eNGINEERINGDataSet.CUT_PART_OPS, Row.PARTID);
				if (cutlistctl.SelectedItem != null) {
					ccpta.FillByCutlistIDAndPartID(eNGINEERINGDataSet.CUT_CUTLIST_PARTS, Row.PARTID, Convert.ToInt32(cutlistctl.SelectedValue));
					PropertySet.CutlistID = Convert.ToInt32(cutlistctl.SelectedValue);
					if (eNGINEERINGDataSet.CUT_CUTLIST_PARTS.Count > 0) {
						CutlistPartsRow = eNGINEERINGDataSet.CUT_CUTLIST_PARTS[0];
					}
				}
			} else {
				ToggleNotInDBWarn(false);
				GetDataFromPart();
			}
			remove_btn.Enabled = PropertySet.CutlistAndPartIDsOK;
			SelectLastCutlist();
		}

		private void SelectLastCutlist() {
			if (ComboBoxContainsValue(Properties.Settings.Default.LastCutlist, cutlistctl)) {
				cutlistctl.SelectedValue = Properties.Settings.Default.LastCutlist;
				EnableCutlistSpec(true);
				ToggleCutlistErr(false);
				comboBox6_SelectedIndexChanged(cutlistctl, new EventArgs());
			} else if (cutlistctl.Items.Count < 1) {
				cutlistctl.SelectedValue = -1;
				EnableCutlistSpec(true);
				ToggleCutlistErr(false);
				GetMaterialFromPart();
				GetEdgesFromPart();
			} else {
				cutlistctl.SelectedValue = -1;
				ToggleCutlistErr(true);
				EnableCutlistSpec(false);
				GetMaterialFromPart();
				GetEdgesFromPart();
			}
		}

		private void ToggleTypeWarn(bool on) {
			if (on) {
				Redbrick.Err(type_cbx);
				Control [] ttrg_ = { type_cbx, groupBox4,
														 op1_cbx, op2_cbx, op3_cbx, op4_cbx, op5_cbx,
														 label32, label33, label34, label35, label36 };
				foreach (Control item in ttrg_) {
					type_tooltip.SetToolTip(item, Properties.Resources.NoTypeWarning);
				}
			} else {
				Redbrick.UnErr(type_cbx);
				type_tooltip.RemoveAll();
			}
		}

		private void ToggleDescrWarn(bool on) {
			if (on) {
				Redbrick.Warn(descriptiontb);
				descr_tooltup.SetToolTip(descriptiontb, Properties.Resources.NoDescriptionWarning);
				descr_tooltup.SetToolTip(label12, Properties.Resources.NoDescriptionWarning);
			} else {
				Redbrick.UnErr(descriptiontb);
				descr_tooltup.RemoveAll();
			}
		}

		private void ToggleThicknessWarn(bool on) {
			if (on) {
				Redbrick.Err(cutlistMat);
				cutlistMat_tooltip.SetToolTip(cutlistMat, Properties.Resources.ThicknessWarning);
				cutlistMat_tooltip.SetToolTip(label1, Properties.Resources.ThicknessWarning);
				cutlistMat_tooltip.SetToolTip(thickness_label, Properties.Resources.ThicknessWarning);
				cutlistMat_tooltip.SetToolTip(wall_thickness_label, Properties.Resources.ThicknessWarning);
			} else {
				Redbrick.UnErr(cutlistMat);
				Redbrick.UnErr(thickness_label);
				Redbrick.UnErr(wall_thickness_label);
				cutlistMat_tooltip.RemoveAll();
			}
		}

		private void ToggleThicknessWarn(bool on, string message) {
			if (on) {
				Redbrick.Err(cutlistMat);
				Redbrick.Err(thickness_label);
				Redbrick.Err(wall_thickness_label);
				cutlistMat_tooltip.SetToolTip(cutlistMat, message);
				cutlistMat_tooltip.SetToolTip(label1, message);
				cutlistMat_tooltip.SetToolTip(thickness_label, message);
				cutlistMat_tooltip.SetToolTip(wall_thickness_label, message);
			} else {
				Redbrick.UnErr(cutlistMat);
				Redbrick.UnErr(thickness_label);
				Redbrick.UnErr(wall_thickness_label);
				cutlistMat_tooltip.RemoveAll();
			}
		}

		private void ToggleCutlistErr(bool on) {
			if (on) {
				Redbrick.Warn(cutlistctl);
				cutlist_tooltip.SetToolTip(cutlistctl, Properties.Resources.CutlistNotSelectedWarning);
				cutlist_tooltip.SetToolTip(label11, Properties.Resources.CutlistNotSelectedWarning);
			} else {
				Redbrick.UnErr(cutlistctl);
				cutlist_tooltip.RemoveAll();
			}
		}

		private void ToggleEdgingOpWarn(bool on) {
			if (on) {
				Redbrick.Warn(edgef);
				Redbrick.Warn(edgeb);
				Redbrick.Warn(edgel);
				Redbrick.Warn(edger);
				edging_tooltip.SetToolTip(edgef, Properties.Resources.InconsistentEdgingOp);
				edging_tooltip.SetToolTip(edgeb, Properties.Resources.InconsistentEdgingOp);
				edging_tooltip.SetToolTip(edgel, Properties.Resources.InconsistentEdgingOp);
				edging_tooltip.SetToolTip(edger, Properties.Resources.InconsistentEdgingOp);
			} else {
				Redbrick.UnErr(edgef);
				Redbrick.UnErr(edgeb);
				Redbrick.UnErr(edgel);
				Redbrick.UnErr(edger);
				edging_tooltip.RemoveAll();
			}
		}

		private void ToggleEdgingOpWarn(bool on, Control _c) {
			if (on) {
				Redbrick.Warn(_c);
				edging_tooltip.SetToolTip(_c, Properties.Resources.InconsistentEdgingOp);
			} else {
				Redbrick.UnErr(edgef);
				Redbrick.UnErr(edgeb);
				Redbrick.UnErr(edgel);
				Redbrick.UnErr(edger);
				edging_tooltip.RemoveAll();
			}
		}

		private void EnableCutlistSpec(bool on) {
			cutlistMat.Enabled = on;
			edgef.Enabled = on;
			edgeb.Enabled = on;
			edgel.Enabled = on;
			edger.Enabled = on;
			partq.Enabled = on;
		}

		private void ToggleCutlistQtyErr(bool on) {
			if (on) {
				Redbrick.Err(partq);
				partq_tooltip.SetToolTip(partq, Properties.Resources.NotNaturalNumberWarning);
				partq_tooltip.SetToolTip(label30, Properties.Resources.NotNaturalNumberWarning);
			} else {
				Redbrick.UnErr(partq);
				partq_tooltip.RemoveAll();
			}
		}

		private void TogglePPBErr(bool on) {
			if (on) {
				Redbrick.Err(ppb_nud);
				ppb_tooltip.SetToolTip(ppb_nud, Properties.Resources.NotNaturalNumberWarning);
				ppb_tooltip.SetToolTip(label27, Properties.Resources.NotNaturalNumberWarning);
			} else {
				Redbrick.UnErr(ppb_nud);
				ppb_tooltip.RemoveAll();
			}
		}

		private void ToggleEdgeWarn(bool on) {
			ComboBox[] edgs = { edgef, edgeb, edgel, edger};
			for (int i = 0; i < edgs.Length; i++) {
				if (on) {
					Redbrick.Warn(edgs[i]);
					edging_tooltip.SetToolTip(edgs[i], Properties.Resources.DimensionSwapWarning);
				} else {
					Redbrick.UnErr(edgs[i]);
				}
			}
			if (!on) {
				edging_tooltip.RemoveAll();
			}
		}

		private void ToggleOversizeWarn(bool on) {
			if (on) {
				Redbrick.Warn(overLtb);
				Redbrick.Warn(overWtb);
				Redbrick.Warn(blnkszLtb);
				Redbrick.Warn(blnkszWtb);
			} else {
				Redbrick.UnErr(overLtb);
				Redbrick.UnErr(overWtb);
				Redbrick.UnErr(blnkszLtb);
				Redbrick.UnErr(blnkszWtb);
				over_tooltip.RemoveAll();
			}
		}

		private void ToggleOversizeWarn(bool on, string message) {
			if (on) {
				Redbrick.Warn(overLtb);
				Redbrick.Warn(overWtb);
				Redbrick.Warn(blnkszLtb);
				Redbrick.Warn(blnkszWtb);
				over_tooltip.SetToolTip(overLtb, message);
				over_tooltip.SetToolTip(overWtb, message);
				over_tooltip.SetToolTip(blnkszLtb, message);
				over_tooltip.SetToolTip(blnkszWtb, message);
			} else {
				Redbrick.UnErr(overLtb);
				Redbrick.UnErr(overWtb);
				Redbrick.UnErr(blnkszLtb);
				Redbrick.UnErr(blnkszWtb);
				over_tooltip.RemoveAll();
			}
		}

		/// <summary>
		/// Search a combobox for a value.
		/// </summary>
		/// <param name="value">The int we're looking for.</param>
		/// <param name="comboBox">The ComboBox we want to ransack.</param>
		/// <returns></returns>
		public static bool ComboBoxContainsValue(int value, ComboBox comboBox) {
			foreach (var item in comboBox.Items) {
				DataRowView drv = (item as DataRowView);
				if ((int)drv[comboBox.ValueMember] == value) {
					return true;
				}
			}
			return false;
		}

		static private string get_format_txt(ModelDoc2 _md) {
			int prec_ = _md.Extension.GetUserPreferenceInteger(
				(int)swUserPreferenceIntegerValue_e.swDetailingLinearDimPrecision,
				(int)swUserPreferenceOption_e.swDetailingLinearDimension);
			string format_txt_ = @"{0:0.";
			for (int i = 0; i < prec_; i++) {
				format_txt_ += @"0";
			}
			format_txt_ += @"}";
			return format_txt_;
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
				string hash_ = Row[@"HASH"] == DBNull.Value ? "\nOld part; no location information." : string.Empty;
				if (Row[@"HASH"] != DBNull.Value) {
					hash_ = PropertySet.Hash == Row.HASH ?
						"\nLocation is correct." : "\nThis part has been moved.";
				}
				string msg_ = Properties.Resources.InfoFromDB + hash_;
				groupbox_tooltip.SetToolTip(groupBox1, msg_);
			} else {
				string msg_ = Properties.Resources.InfoNotFromDB;
				//if (Properties.Settings.Default.ExtraInfo) {
				//	msg_ += GetLocations();
				//}
				groupBox1.ForeColor = Properties.Settings.Default.WarnBackground;
				groupbox_tooltip.SetToolTip(groupBox1, msg_);
				foreach (Control control in groupBox1.Controls) {
					control.ForeColor = Properties.Settings.Default.NormalForeground;
				}
			}
		}

		private string GetLocations() {
			List<string> l_ = eNGINEERINGDataSet.CLIENT_STUFF.GetLocations(partLookup);
			string loc_ = string.Empty;
			for (int i = 0; i < l_.Count; i++) {
				loc_ += l_[i] + "\n";
			}
			System.Diagnostics.Debug.Print(loc_);
			return "\n" + loc_;
		}

		private void GetDataFromPart() {
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
			ppb_nud.Value = Convert.ToDecimal(IntTryProp("BLANK QTY"));
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
			FilterOps(string.Format(@"TYPEID = {0}", type));
		}

		private void GetMaterialFromPart() {
			int mr = (int)IntTryProp("MATID");
			if (mr < 1 && PropertySet.Contains("CUTLIST MATERIAL")) {
				mr = (int)PropertySet[@"CUTLIST MATERIAL"].Data;
			}
			cutlistMat.SelectedValue = mr;
		}

		private void GetEdgesFromPart() {
			ENGINEERINGDataSet.CUT_EDGESDataTable ed =
				new ENGINEERINGDataSet.CUT_EDGESDataTable();
			int er = IntTryProp("EFID");
			if (er < 1 && PropertySet.Contains(@"EDGE FRONT (L)")) {
				er = (int)PropertySet[@"EDGE FRONT (L)"].Data;
			}
			edgef.SelectedValue = er;
			er = IntTryProp("EBID");
			if (er < 1 && PropertySet.Contains(@"EDGE BACK (L)")) {
				er = (int)PropertySet[@"EDGE BACK (L)"].Data;
			}
			edgeb.SelectedValue = er;
			er = IntTryProp("ERID");
			if (er < 1 && PropertySet.Contains(@"EDGE RIGHT (W)")) {
				er = (int)PropertySet[@"EDGE RIGHT (W)"].Data;
			}
			edger.SelectedValue = er;

			er = IntTryProp("ELID");
			if (er < 1 && PropertySet.Contains(@"EDGE LEFT (W)")) {
				er = (int)PropertySet[@"EDGE LEFT (W)"].Data;
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
			if (Row != null) {
				type_cbx.SelectedValue = Row.TYPE;
				FilterOps(string.Format(@"TYPEID = {0}", Row.TYPE));
			}

			cpota.FillByPartID(eNGINEERINGDataSet.CUT_PART_OPS, Row.PARTID);

			for (int i = 0; i < cbxes.Length; i++) {
				ComboBox current = cbxes[i];
				string op_ = string.Format(@"OP{0}", i + 1);
				string opid_ = string.Format(@"OP{0}ID", i + 1);
				if (i < eNGINEERINGDataSet.CUT_PART_OPS.Rows.Count) {
					ENGINEERINGDataSet.CUT_PART_OPSRow r =
						(eNGINEERINGDataSet.CUT_PART_OPS.Rows[i] as ENGINEERINGDataSet.CUT_PART_OPSRow);
					current.SelectedValue = r.POPOP;
					if (PropertySet.ContainsKey(op_)) {
						OpProperty prop_ = PropertySet[op_] as OpProperty;
						prop_.OpType = Convert.ToInt32(type_cbx.SelectedValue);
						prop_.Data = r.POPOP;
					}
					if (PropertySet.ContainsKey(opid_)) {
						OpId propid_ = PropertySet[opid_] as OpId;
						propid_.OpType = Convert.ToInt32(type_cbx.SelectedValue);
						propid_.Set(r.POPOP, r.POPOP.ToString());
					}
				} else {
					current.SelectedValue = -1;
				}
			}
			GetEstimationFromDB();
		}

		private void GetEstimationFromDB() {
			double setupTime = 0.0f;
			double runTime = 0.0f;
			string scope_ = string.Empty;
			if (Properties.Settings.Default.EstimateSource || PropertySet.CutlistID < 1) {
				scope_ = @"/part";
				if (PropertySet.PartID != 0) {
					setupTime = Convert.ToDouble(cpota.GetPartSetupTime(PropertySet.PartID));
					runTime = Convert.ToDouble(cpota.GetPartRunTime(PropertySet.PartID));
				}
			} else {
				scope_ = @"/cutlist";
				if (PropertySet.CutlistID != 0) {
					setupTime = Convert.ToDouble(cpota.GetCutlistSetupTime(PropertySet.CutlistID));
					runTime = Convert.ToDouble(cpota.GetCutlistRunTime(PropertySet.CutlistID));
				}
			}

			double total_run_ = ((runTime + (setupTime / Properties.Settings.Default.SPQ))) * 60;
			string run_fmt_ = @"{0:0.0} min";
			if (total_run_ > 60) {
				total_run_ = total_run_ / 60;
				run_fmt_ = "{0:0.0} hr";
			}

			string fmt_ = string.Format("Routing ({0}/SPQ: {1}{2})", run_fmt_,
			Properties.Settings.Default.SPQ, scope_);

			groupBox4.Text = string.Format(fmt_, total_run_);
		}

		private void GetEstimationFromPart() {
			double setupTime = 0.0f;
			double runTime = 0.0f;
			for (int i = 0; i < cbxes.Length; i++) {
				ComboBox current = cbxes[i];
				DataRowView r = current.SelectedItem as DataRowView;
				if (r != null) {
					setupTime += Convert.ToDouble(r[@"OPSETUP"]);
					runTime += Convert.ToDouble(r[@"OPRUN"]);
				}
			}

			double total_run_ = ((runTime + (setupTime / Properties.Settings.Default.SPQ))) * 60;
			groupBox4.Text = string.Format("Routing ({0:0.0} min/SPQ: {1}/part)", total_run_,
				Properties.Settings.Default.SPQ);
		}

		private void GetRoutingFromPart() {
			int type = 1;
			if (type_cbx.SelectedItem != null) {
				type = (int)type_cbx.SelectedValue;
			}
			int er = 0;

			for (int i = 0; i < cbxes.Length; i++) {
				string opid_name = string.Format("OP{0}ID", i + 1);
				string op_name = string.Format("OP{0}", i + 1);
				if (PropertySet.ContainsKey(opid_name)) {
					(PropertySet[opid_name] as OpId).OpType = type;
					er = (int)PropertySet[opid_name].Data;
				}
				if (er < 1) {
					if (PropertySet.ContainsKey(op_name)) {
						(PropertySet[op_name] as OpProperty).OpType = type;
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
				filter = "TYPEID = 1";
			}
			BindingSource[] bs_ = { friendlyCutOpsBindingSource, friendlyCutOpsBindingSource1,
															friendlyCutOpsBindingSource2, friendlyCutOpsBindingSource3,
															friendlyCutOpsBindingSource4 };
			foreach (BindingSource b_ in bs_) {
				b_.Filter = filter;
			}
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
				//dd.UserSelectionPostNotify += dd_UserSelectionPostNotify;
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
			if (DrawingEventsAssigned) {
				dd.UserSelectionPostNotify -= dd_UserSelectionPostNotify;
				DrawingEventsAssigned = false;
			}
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

				ad.ActiveConfigChangePostNotify += ad_ActiveConfigChangePostNotify;
				ad.ViewNewNotify2 += ad_ViewNewNotify2;
				//DisconnectDrawingEvents();
				AssemblyEventsAssigned = true;
			} else {
				// We're already set up, I guess.
			}
		}

		int ad_ViewNewNotify2(object viewBeingAdded) {
			ReQuery();
			return 0;
		}

		int ad_ActiveConfigChangePostNotify() {
			configurationManager = _activeDoc.ConfigurationManager;
			configuration = configurationManager.ActiveConfiguration.Name;
			ReQuery();
			return 0;
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
				if (swSelComp.GetModelDoc2() is ModelDoc2) {
					configurationManager = (swSelComp.GetModelDoc2() as ModelDoc2).ConfigurationManager;
					configuration = swSelComp.ReferencedConfiguration;
					this.Enabled = true;
					ReQuery(swSelComp.GetModelDoc2());
				}
			} else {
				// Nothing's selected?
				// Just look at the root item then.
				try {
					configurationManager = SwApp.ActiveDoc.ConfigurationManager;
					configuration = configurationManager.ActiveConfiguration.Name;
					this.Enabled = true;
					groupBox1.Text = string.Format(@"{0} - {1}",
						partLookup, PropertySet.Configuration);
					ReQuery(SwApp.ActiveDoc);
				} catch (Exception ex) {
					System.Diagnostics.Debug.WriteLine(ex.Message + " from " + ex.Source + "\n" + ex.StackTrace);
				} finally {
				}
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
				ad.ActiveConfigChangePostNotify -= ad_ActiveConfigChangePostNotify;
				//swSelMgr = null;
			}
			AssemblyEventsAssigned = false;
		}

		private int pd_DestroyNotify2(int DestroyType) {
			Visible = false;
			return 0;
		}

		private int pd_FileSavePostNotify(int saveType, string FileName) {
			if (do_savepostnotify) {
				ModelDoc2 md_ = _activeDoc;
				DumpActiveDoc();
				ActiveDoc = SwApp.ActiveDoc;
			}
			do_savepostnotify = true;
			return 0;
		}

		private int pd_ActiveConfigChangePostNotify() {
			ModelDoc2 md = ActiveDoc;
			DumpActiveDoc();
			_activeDoc = null;
			ReQuery(md);
			cutlistctl.SelectedIndex = -1;
			Properties.Settings.Default.LastCutlist = -1;
			ToggleCutlistErr(true);
			return 0;
		}

		private string EnQuote(string stuff) {
			if (!double.TryParse(stuff, out double test_)) {
				return string.Format("\"{0}\"", stuff.Replace("\"", string.Empty));
			}
			return stuff;
		}

		private void UpdateGeneralProperties() {
			DataRowView _drv = type_cbx.SelectedItem as DataRowView;
			PropertySet[@"DEPARTMENT"].Set(type_cbx.SelectedValue, _drv[@"TYPEDESC"].ToString());
			PropertySet[@"Description"].Data = descriptiontb.Text;

			// Dimensions get special treatment since the mgr stores weird strings, and DB stores doubles.
			PropertySet[@"LENGTH"].Set(length_label.Text, EnQuote(lengthtb.Text));
			PropertySet[@"WIDTH"].Set(width_label.Text, EnQuote(widthtb.Text));
			PropertySet[@"THICKNESS"].Set(thickness_label.Text, EnQuote(thicknesstb.Text));
			PropertySet[@"WALL THICKNESS"].Set(wall_thickness_label.Text, EnQuote(wallthicknesstb.Text));

			PropertySet[@"COMMENT"].Data = commenttb.Text;

			int descr_limit_ = eNGINEERINGDataSet.CUT_PARTS.DESCRColumn.MaxLength;
			if (descriptiontb.Text.Length > descr_limit_) {
				Row.DESCR = descriptiontb.Text.Substring(0, descr_limit_);
			} else {
				Row.DESCR = descriptiontb.Text;
			}
			float test_ = 0.0F;
			if (float.TryParse(length_label.Text, out test_)) {
				Row.FIN_L = test_;
				test_ = 0.0F;
			}

			if (float.TryParse(width_label.Text, out test_)) {
				Row.FIN_W = test_;
				test_ = 0.0F;
			}

			if (float.TryParse(thickness_label.Text, out test_)) {
				Row.THICKNESS = test_;
				test_ = 0.0F;
			}

			Row.COMMENT = commenttb.Text;
			Row.TYPE = Convert.ToInt32(type_cbx.SelectedValue);
			Row.HASH = Hash;
		}

		private void UpdateMachineProperties() {
			PropertySet[@"CNC1"].Data = cnc1tb.Text;
			PropertySet[@"CNC2"].Data = cnc2tb.Text;
			PropertySet[@"OVERL"].Data = overLtb.Text;
			PropertySet[@"OVERW"].Data = overWtb.Text;
			PropertySet[@"BLANK QTY"].Data = ppb_nud.Value;
			PropertySet[@"UPDATE CNC"].Data = updateCNCcb.Checked;

			Row.CNC1 = cnc1tb.Text;
			Row.CNC2 = cnc2tb.Text;

			float test_float_ = 0.0F;
			if (float.TryParse(overLtb.Text, out test_float_)) {
				Row.OVER_L = test_float_;
				test_float_ = 0.0F;
			}

			if (float.TryParse(overWtb.Text, out test_float_)) {
				Row.OVER_W = test_float_;
				test_float_ = 0.0F;
			}

			Row.BLANKQTY = Convert.ToInt32(ppb_nud.Value);
			Row.UPDATE_CNC = updateCNCcb.Checked;
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

			if (CutlistPartsRow != null) {
				CutlistPartsRow.MATID = Convert.ToInt32(cutlistMat.SelectedValue);
				CutlistPartsRow.EDGEID_LF = Convert.ToInt32(edgef.SelectedValue);
				CutlistPartsRow.EDGEID_LB = Convert.ToInt32(edgeb.SelectedValue);
				CutlistPartsRow.EDGEID_WL = Convert.ToInt32(edgel.SelectedValue);
				CutlistPartsRow.EDGEID_WR = Convert.ToInt32(edger.SelectedValue);
			}

			PropertySet.CutlistQty = (float)Convert.ToDouble(partq.Value);
		}

		private void UpdateRoutingProperties() {
			for (int i = 0; i < cbxes.Length; i++) {
				ComboBox cbx = cbxes[i];
				string op = string.Format(@"OP{0}", i + 1);
				string opid = string.Format(@"OP{0}ID", i + 1);
				if (cbx.SelectedItem != null) {
					DataRowView drv = (cbx.SelectedItem as DataRowView);
					PropertySet[op].Set((int)cbx.SelectedValue, drv[@"OPNAME"].ToString());
					PropertySet[opid].Set((int)cbx.SelectedValue, cbx.SelectedValue.ToString());
					if (eNGINEERINGDataSet.CUT_PART_OPS.Rows.Count > i && 
						eNGINEERINGDataSet.CUT_PART_OPS.Rows[i].RowState != DataRowState.Deleted) {
						ENGINEERINGDataSet.CUT_PART_OPSRow r_ =
							eNGINEERINGDataSet.CUT_PART_OPS.Rows[i] as ENGINEERINGDataSet.CUT_PART_OPSRow;
						r_.POPORDER = i + 1;
						r_.POPOP = Convert.ToInt32(cbx.SelectedValue);
						r_.POPSETUP = Convert.ToDouble(drv[@"OPSETUP"]);
						r_.POPRUN = Convert.ToDouble(drv[@"OPRUN"]);
					} else {
						ENGINEERINGDataSet.CUT_PART_OPSRow r_ =
							eNGINEERINGDataSet.CUT_PART_OPS.NewRow() as ENGINEERINGDataSet.CUT_PART_OPSRow;
						r_.POPPART = Row.PARTID;
						r_.POPORDER = i + 1;
						r_.POPOP = Convert.ToInt32(cbx.SelectedValue);
						r_.POPSETUP = Convert.ToDouble(drv[@"OPSETUP"]);
						r_.POPRUN = Convert.ToDouble(drv[@"OPRUN"]);
						eNGINEERINGDataSet.CUT_PART_OPS.AddCUT_PART_OPSRow(r_);
					}
				} else {
					PropertySet[op].Set(0, string.Empty);
					PropertySet[opid].Set(0, @"0");
					if (eNGINEERINGDataSet.CUT_PART_OPS.Rows.Count > i) {
						eNGINEERINGDataSet.CUT_PART_OPS.Rows[i].Delete();
					}
				}
			}
			//Row.OP1ID = Convert.ToInt32(op1_cbx.SelectedValue);
			//Row.OP2ID = Convert.ToInt32(op2_cbx.SelectedValue);
			//Row.OP3ID = Convert.ToInt32(op3_cbx.SelectedValue);
			//Row.OP4ID = Convert.ToInt32(op4_cbx.SelectedValue);
			//Row.OP5ID = Convert.ToInt32(op5_cbx.SelectedValue);
		}

		private void save_part() {
			string warning_ = data_from_db ? 
				Properties.Resources.InDBReadOnlyWarning : Properties.Resources.NotInDBReadOnlyWarning;
			if (Properties.Settings.Default.ReadOnlyWarn && _activeDoc.IsOpenedReadOnly()) {
				MessageBox.Show(this, warning_, @"Read only",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
			} else {
				PropertySet.Save();
			}
		}

		/// <summary>
		/// Populate and write properties. Create proper rows to be Updated into the db.
		/// </summary>
		public void Commit() {
			if (Row == null) {
				ENGINEERINGDataSet.CUT_PARTSDataTable dt =
					new ENGINEERINGDataSet.CUT_PARTSDataTable();
				Row = dt.NewRow() as ENGINEERINGDataSet.CUT_PARTSRow;
				Row.PARTNUM = partLookup;
			}
			if (tabControl1.SelectedTab == tabPage1) {
				UpdateGeneralProperties();
				UpdateMachineProperties();
				UpdateCutlistProperties();
				UpdateRoutingProperties();

				PropertySet.Write();
				if (data_from_db) {
					if (Row != null && Row.PARTID > 0) {
						eNGINEERINGDataSet.CUT_PARTS.UpdatePart(PropertySet);
						//cpta.Update(Row);
						if (CutlistPartsRow != null && PropertySet.CutlistAndPartIDsOK) {
							eNGINEERINGDataSet.CUT_CUTLIST_PARTS.UpdateCutlistPart(PropertySet);
							//ccpta.Update(CutlistPartsRow);
						}
						eNGINEERINGDataSet.CUT_PART_OPS.UpdateOps(PropertySet);
						//cpota.Update(eNGINEERINGDataSet.CUT_PART_OPS);
					}
					GetEstimationFromDB();
					if (Properties.Settings.Default.AutoOpenPriority && checked_at_start && !updateCNCcb.Checked) {
						popup_priority_(PropertySet.PartLookup);
						checked_at_start = false;
					}
				} else {
					GetEstimationFromPart();
				}
				do_savepostnotify = false;
				save_part();
				_activeDoc.Rebuild((int)swRebuildOptions_e.swRebuildAll);
				checked_at_start = updateCNCcb.Checked;
				groupBox1.Text = groupBox1.Text.Replace(Properties.Settings.Default.NotSavedMark, string.Empty);
				recalculate_blanksizeL();
				recalculate_blanksizeW();
				int gc_ = GC.GetGeneration(this);
				GC.Collect(gc_, GCCollectionMode.Optimized);
			} else if (tabControl1.SelectedTab == tabPage2) {
				drawingRedbrick.Commit();
			}
			eNGINEERINGDataSet.GEN_ODOMETER.IncrementOdometer(Redbrick.Functions.GreenCheck);
		}

		private void popup_priority_(string _lookup) {
			Machine_Priority_Control.MachinePriority m_ =
				new Machine_Priority_Control.MachinePriority(_lookup);
			m_.ShowDialog(this);
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

			if (!(_activeDoc is DrawingDoc)) {
				configuration = _activeDoc.ConfigurationManager.ActiveConfiguration.Name;
			}

			if (Component != null) {
				configuration = Component.ReferencedConfiguration;
				PropertySet.Configuration = configuration;
				PropertySet.GetProperties(Component);
			} else if (partLookup != null) {
				configuration = _activeDoc.ConfigurationManager.ActiveConfiguration.Name;
				PropertySet.Configuration = configuration;
				PropertySet.GetProperties(_activeDoc);
			} else {
				//
			}

			lastModelDoc = _activeDoc;
			//DisconnectPartEvents();
			ConnectPartEvents();
			ReQuery();
		}

		private void CheckThickness() {
			if (cutlistMat.SelectedItem != null) {
				DataRowView r_ = cutlistMat.SelectedItem as DataRowView;
				double matthk_ = Convert.ToDouble(r_[@"THICKNESS"]);
				double epsilon_ = Properties.Settings.Default.Epsilon;
				double thk_ = 0.0f;
				double wthk_ = 0.0f;
				bool thk_parsed_ = double.TryParse(thickness_label.Text, out thk_);
				bool wthk_parsed_ = double.TryParse(wall_thickness_label.Text, out wthk_);
				bool err_ = matthk_ > epsilon_ && (thk_parsed_ || wthk_parsed_);
				if (Properties.Settings.Default.Warn && err_) {
					string msg_ = string.Format("Material thickness ({0}) doesn't match dimensions.",
						Redbrick.enforce_number_format(matthk_));
					bool equal_ = (Redbrick.FloatEquals(matthk_, thk_) || Redbrick.FloatEquals(matthk_, wthk_));
					ToggleThicknessWarn(!equal_, msg_);
				} else {
					ToggleThicknessWarn(false);
				}
			}
		}

		private void CheckOversize() {
			bool ok_ = Properties.Settings.Default.OpWarn;
			if (Properties.Settings.Default.Warn) {
				string message_ = string.Empty;
				int ps_op_ = 0;
				int not_cnc_op_ = 0;
				double oversize_ = 0.0F;
				int bq_ = Convert.ToInt32(ppb_nud.Value);
				bool warn_ = false;
				List<string> ops_ = new List<string> { };
				List<bool> cnc_ops_ = new List<bool> { };
				double test_ = 0.0F;
				if (double.TryParse(overLtb.Text, out test_)) {
					oversize_ += test_;
				}
				if (double.TryParse(overWtb.Text, out test_)) {
					oversize_ += test_;
				}
				for (int i = 0; i < cbxes.Length; i++) {
					if (cbxes[i].SelectedItem != null) {
						DataRowView drv_ = cbxes[i].SelectedItem as DataRowView;
						ops_.Add(drv_[@"OPNAME"].ToString());
						cnc_ops_.Add(Convert.ToBoolean(drv_[@"OPPROG"]));
					} else {
						ops_.Add(string.Empty);
						cnc_ops_.Add(false);
					}
				}

				for (int i = 0; i < cbxes.Length; i++) {
					if (ops_[i] == @"PS")
						ps_op_ = i;

					if ((i < cbxes.Length - 1) &&
						(ops_[i] == @"PS" && (!cnc_ops_[i + 1])// || (ops_[i + 1] == string.Empty)
						)) {
						not_cnc_op_ = i + 1;
						if (bq_ == 1 && oversize_ > 0) {
							string _not_cnc = ops_[not_cnc_op_].Trim() != string.Empty ? ops_[not_cnc_op_] : @"[NO OP]";
							string msg_ = string.Format(@"No CNC op between {0} and {1}; check oversize values.",
								ops_[ps_op_], _not_cnc);
							warn_ = true;
							ToggleOversizeWarn(true, msg_);
							break;
						}
					}
				}
				if (!warn_) {
					ToggleOversizeWarn(false);
				}
			}
		}

		private void CheckEdgingOps() {
			bool ee_ = edging_exists();
			bool eoe_ = edging_op_exists();
			bool ok_ = (ee_ && eoe_) || (!ee_ && !eoe_);
			if (Properties.Settings.Default.Warn && ok_) {
				ToggleEdgingOpWarn(false);
			} else {
				ToggleEdgingOpWarn(true);
			}
		}

		/// <summary>
		/// The PartID from CUT_PARTS.
		/// </summary>
		public int PartID { get; set; }

		/// <summary>
		/// Hash of the path of the part in question.
		/// </summary>
		public int Hash { get; set; }

		/// <summary>
		/// A useful FileInfo object of the part in question.
		/// </summary>
		public FileInfo PartFileInfo { get; set; }

		private Component2 _component;

		/// <summary>
		/// Component2 in question, if any.
		/// </summary>
		public Component2 Component {
			get { return _component; }
			set {
				_component = value;
			}
		}

		private ModelDoc2 _activeDoc;

		/// <summary>
		/// ModelDoc2 in question. Lots of stuff can go wrong here.
		/// </summary>
		public ModelDoc2 ActiveDoc {
			get { return _activeDoc; }
			set {
				//allowPaint = false;
				int gc_ = GC.GetGeneration(this);
				GC.Collect(gc_, GCCollectionMode.Optimized);
				if (value != null && value != ActiveDoc) {
					DisconnectEvents();
					Show();
					ToggleEdgeWarn(false);
					lastModelDoc = _activeDoc;
					_activeDoc = value;
					req_info_ = string.Empty;
					mat_price_ = string.Empty;
					edgef_price_ = string.Empty;
					edgeb_price_ = string.Empty;
					edgel_price_ = string.Empty;
					edger_price_ = string.Empty;
					string _fn = _activeDoc.GetPathName();
					if (_fn != string.Empty) {
						PartFileInfo = new FileInfo(_fn);
						partLookup = Redbrick.FileInfoToLookup(PartFileInfo);
					} else {
						partLookup = null;
						PartFileInfo = new FileInfo(Path.GetTempFileName());
						Hash = Redbrick.GetHash(PartFileInfo);
					}

					swDocumentTypes_e dType = (swDocumentTypes_e)_activeDoc.GetType();
					swDocumentTypes_e odType = (swDocumentTypes_e)(SwApp.ActiveDoc as ModelDoc2).GetType();
					PropertySet = new SwProperties(SwApp, _activeDoc);
					switch (odType) {
						case swDocumentTypes_e.swDocASSEMBLY:                     //Window looking at assembly.
							(tabPage1 as Control).Enabled = true;
							//DisconnectAssemblyEvents();
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
							(tabPage1 as Control).Enabled = false;
							//switch (dType) {
							//	case swDocumentTypes_e.swDocASSEMBLY:                     //Selected assembly in drawing.
							//		(tabPage1 as Control).Enabled = true;
							//		SetupPart();
							//		tabControl1.SelectedTab = tabPage1;
							//		break;
							//	case swDocumentTypes_e.swDocDRAWING:
							//		tabControl1.SelectedTab = tabPage2;
							//		break;
							//	case swDocumentTypes_e.swDocPART:                         //Selected part in drawing.
							//		(tabPage1 as Control).Enabled = true;
							//		SetupPart();
							//		tabControl1.SelectedTab = tabPage1;
							//		break;
							//	default:
							//		break;
							//}
							SetupDrawing();
							tabControl1.SelectedTab = tabPage2;
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

		/// <summary>
		/// The SolidWorks application we're connected to.
		/// </summary>
		public SldWorks SwApp { get; set; }

		private void ModelRedbrick_Load(object sender, EventArgs e) {
			cUT_MATERIALSTableAdapter.Fill(eNGINEERINGDataSet.CUT_MATERIALS);
			cUT_EDGESTableAdapter.Fill(eNGINEERINGDataSet.CUT_EDGES);
			cUT_PART_TYPESTableAdapter.Fill(eNGINEERINGDataSet.CUT_PART_TYPES);
			friendlyCutOpsTableAdapter.Fill(eNGINEERINGDataSet.FriendlyCutOps);
			cUT_STATESTableAdapter.Fill(eNGINEERINGDataSet.CUT_STATES);
			swap_tooltup.SetToolTip(swapLnW, Properties.Resources.DimensionSwapWarning);
			swap_tooltup.SetToolTip(swapWnT, Properties.Resources.DimensionSwapWarning);
			//GetCutlistData();
			//SelectTab();
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
			}

			if (double.TryParse(prp, out double test_)) {
				return prp;
			} else {
				return DimensionByEquation(prp);
			}
		}

		private string DimensionByEquation(string equation) {
			string res = string.Empty;
			if (ActiveDoc == null) {
				return Properties.Settings.Default.ValErr;
			}
			EquationMgr eqm = ActiveDoc.GetEquationMgr();
			for (int i = 0; i < eqm.GetCount(); i++) {
				if (eqm.get_Equation(i).Contains(equation)) {
					return eqm.get_Value(i).ToString();
				}
			}
			return Properties.Settings.Default.ValErr;
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
					if (Properties.Settings.Default.Warn) {
						CheckThickness();
					}
				} else {
					l.Text = Properties.Settings.Default.ValErr;
				}
			}
		}

		private void clip_click(object sender, EventArgs e) {
			Redbrick.Clip((sender as Control).Text);
		}

		private void textBox2_TextChanged(object sender, EventArgs e) {
			TextBox _tb = (sender as TextBox);
			textBox_TextChanged(_tb.Text, length_label);
		}

		private void textBox3_TextChanged(object sender, EventArgs e) {
			TextBox _tb = (sender as TextBox);
			textBox_TextChanged(_tb.Text, width_label);
		}

		private void textBox4_TextChanged(object sender, EventArgs e) {
			TextBox _tb = (sender as TextBox);
			textBox_TextChanged(_tb.Text, thickness_label);
		}

		private void textBox5_TextChanged(object sender, EventArgs e) {
			TextBox _tb = (sender as TextBox);
			textBox_TextChanged(_tb.Text, wall_thickness_label);
		}

		private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) {
			Redbrick.UnErr(sender as Control);
			if ((sender as ComboBox).SelectedIndex == -1) {
				label7.Visible = false;
			} else {
				label7.Visible = true;
				if (edging_op_exists()) {
					ToggleEdgingOpWarn(false);
				} else {
					ToggleEdgingOpWarn(true);
				}
				edger_price_ = string.Empty;
				CheckEdgingOps();
			}
			float edge_thickness_ = get_edge_thickness_total(edgef, edgeb);
			if (float.TryParse(overLtb.Text, out float test_)) {
				calculate_blanksize_from_oversize(test_, blnkszLtb, length, edge_thickness_);
			}
			edgef_price_ = string.Empty;
			CheckEdgingOps();
		}

		private void comboBox3_SelectedIndexChanged(object sender, EventArgs e) {
			Redbrick.UnErr(sender as Control);
			if ((sender as ComboBox).SelectedIndex == -1) {
				label8.Visible = false;
			} else {
				label8.Visible = true;
				if (edging_op_exists()) {
					ToggleEdgingOpWarn(false);
				} else {
					ToggleEdgingOpWarn(true);
				}
				edger_price_ = string.Empty;
				CheckEdgingOps();
			}
			float edge_thickness_ = get_edge_thickness_total(edgef, edgeb);
			if (float.TryParse(overLtb.Text, out float test_)) {
				calculate_blanksize_from_oversize(test_, blnkszLtb, length, edge_thickness_);
			}
			edgeb_price_ = string.Empty;
			CheckEdgingOps();
		}

		private void comboBox4_SelectedIndexChanged(object sender, EventArgs e) {
			Redbrick.UnErr(sender as Control);
			if ((sender as ComboBox).SelectedIndex == -1) {
				label9.Visible = false;
			} else {
				label9.Visible = true;
				if (edging_op_exists()) {
					ToggleEdgingOpWarn(false);
				} else {
					ToggleEdgingOpWarn(true);
				}
				edger_price_ = string.Empty;
				CheckEdgingOps();
			}
			float edge_thickness_ = get_edge_thickness_total(edgel, edger);
			if (float.TryParse(overLtb.Text, out float test_)) {
				calculate_blanksize_from_oversize(test_, blnkszWtb, width, edge_thickness_);
			}
			edgel_price_ = string.Empty;
			CheckEdgingOps();
		}

		private void comboBox5_SelectedIndexChanged(object sender, EventArgs e) {
			Redbrick.UnErr(sender as Control);
			if ((sender as ComboBox).SelectedIndex == -1) {
				label10.Visible = false;
			} else {
				label10.Visible = true;
				if (edging_op_exists()) {
					ToggleEdgingOpWarn(false);
				} else {
					ToggleEdgingOpWarn(true);
				}
				edger_price_ = string.Empty;
				CheckEdgingOps();
			}
			float edge_thickness_ = get_edge_thickness_total(edgel, edger);
			if (float.TryParse(overLtb.Text, out float test_)) {
				calculate_blanksize_from_oversize(test_, blnkszWtb, width, edge_thickness_);
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

		private void label22_Click(object sender, EventArgs e) {
			Redbrick.Clip(cnc1tb.Text);
		}

		private void label23_Click(object sender, EventArgs e) {
			Redbrick.Clip(cnc2tb.Text);
		}

		private void label28_Click(object sender, EventArgs e) {
			Redbrick.Clip(string.Format(@"{0} X {1} X {2}", blnkszLtb.Text, blnkszWtb.Text, thickness_label.Text));
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

		private void blankL_TextChanged(object sender, EventArgs e) {
			TextBox _me = (sender as TextBox);
			Single _test = 0.0F;
			Single _length = 0.0F;

			if (bl_userediting && Single.TryParse(_me.Text, out _test) && Single.TryParse(length_label.Text, out _length)) {
				Single _edge_thickness = 0.0F;

				if (edgel.SelectedItem != null) {
					_edge_thickness += Convert.ToSingle((edgel.SelectedItem as DataRowView)[@"THICKNESS"]);
				}

				if (edger.SelectedItem != null) {
					_edge_thickness += Convert.ToSingle((edger.SelectedItem as DataRowView)[@"THICKNESS"]);
				}
				bl_userediting = false;
				calculate_oversize_from_blanksize(_test, overLtb, length, _edge_thickness);
			}
		}

		private void blankW_TextChanged(object sender, EventArgs e) {
			TextBox _me = (sender as TextBox);
			Single _test = 0.0F;
			Single _width = 0.0F;
			if (bl_userediting && Single.TryParse(_me.Text, out _test) && Single.TryParse(width_label.Text, out _width)) {
				Single _edge_thickness = 0.0F;

				if (edgef.SelectedItem != null) {
					_edge_thickness += Convert.ToSingle((edgef.SelectedItem as DataRowView)[@"THICKNESS"]);
				}

				if (edgeb.SelectedItem != null) {
					_edge_thickness += Convert.ToSingle((edgeb.SelectedItem as DataRowView)[@"THICKNESS"]);
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

		static private Single get_edge_thickness_total(ComboBox c1, ComboBox c2) {
			Single _edge_thickness = 0.0F;
			if (c1.SelectedItem != null && c1.SelectedIndex > 0) {
				DataRowView drv_ = (c1.SelectedItem as DataRowView);
				if (drv_.Row.RowState != DataRowState.Detached) {
					_edge_thickness += Convert.ToSingle(drv_[@"THICKNESS"]);
				}
			}

			if (c2.SelectedItem != null && c2.SelectedIndex > 0) {
				DataRowView drv_ = (c2.SelectedItem as DataRowView);
				if (drv_.Row.RowState != DataRowState.Detached) {
					_edge_thickness += Convert.ToSingle(drv_[@"THICKNESS"]);
				}
			}
			return _edge_thickness;
		}

		private void recalculate_blanksizeL() {
			if (float.TryParse(overLtb.Text, out float test_)) {
				float edge_thickness_ = get_edge_thickness_total(edgef, edgeb);
				calculate_blanksize_from_oversize(test_, blnkszLtb, length, edge_thickness_);
			}
		}

		private void recalculate_blanksizeW() {
			if (float.TryParse(overLtb.Text, out float test_)) {
				float edge_thickness_ = get_edge_thickness_total(edgel, edger);
				calculate_blanksize_from_oversize(test_, blnkszWtb, width, edge_thickness_);
			}
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

		private void comboBox_SelectedIndexChanged(object sender, EventArgs e) {
			mat_price_ = string.Empty;
			CheckThickness();
		}

		private void comboBox6_SelectedIndexChanged(object sender, EventArgs e) {
			// It may seem ridiculous to query for data that's supposed to be in a DataRowView 
			// of (sender as ComboBox).SelectedItem. Well, it turns out that the values shift around
			// unpredicably--at least where the column count is high. I haven't seen values shift in
			// the first few columns.
			if (dirtTracker != null) {
				dirtTracker.Besmirched -= dirtTracker_Besmirched;
			}
			if (cutlistctl.SelectedIndex > -1) {
				if (eNGINEERINGDataSet.CUT_CUTLIST_PARTS.Count > 0) {
					CutlistPartsRow = eNGINEERINGDataSet.CUT_CUTLIST_PARTS[0];
				}

				ccpta.FillByCutlistIDAndPartID(eNGINEERINGDataSet.CUT_CUTLIST_PARTS, PropertySet.PartID,
					Convert.ToInt32(cutlistctl.SelectedValue));
				if (eNGINEERINGDataSet.CUT_CUTLIST_PARTS.Rows.Count > 0) {
					CutlistPartsRow = (eNGINEERINGDataSet.CUT_CUTLIST_PARTS.Rows[0] as ENGINEERINGDataSet.CUT_CUTLIST_PARTSRow);
					Set_Specific(CutlistPartsRow);
				}
			}

			if (cl_userediting && (sender as ComboBox).SelectedValue != null) {
				Properties.Settings.Default.LastCutlist = (int)(sender as ComboBox).SelectedValue;
				Properties.Settings.Default.Save();
				cl_userediting = false;
			}

			if (dirtTracker != null) {
				dirtTracker.Besmirched += dirtTracker_Besmirched;
			}
		}

		private void Set_Specific(ENGINEERINGDataSet.CUT_CUTLIST_PARTSRow _row) {
			EnableCutlistSpec(true);
			ToggleCutlistErr(false);
			cutlistMat.SelectedValue = _row.MATID;
			edgef.SelectedValue = _row.EDGEID_LF;
			edgeb.SelectedValue = _row.EDGEID_LB;
			edger.SelectedValue = _row.EDGEID_WR;
			edgel.SelectedValue = _row.EDGEID_WL;
			partq.Value = Convert.ToInt32(_row.QTY);
			PropertySet.CutlistID = _row.CLID;
			PropertySet.CutlistQty = _row.QTY;
			GetEstimationFromDB();
		}

		private void FocusHere(object sender, MouseEventArgs e) {
			Redbrick.FocusHere(sender, e);
		}

		private bool edging_exists() {
			ComboBox[] ebxs_ = new ComboBox[] { edgef, edgeb, edgel, edger };
			foreach (ComboBox b_ in ebxs_) {
				if (b_.SelectedItem != null) {
					return true;
				}
			}
			return false;
		}

		private bool edging_op_exists() {
			foreach (ComboBox cbx_ in cbxes) {
				foreach (string op_ in Properties.Settings.Default.EdgingOps) {
					if (cbx_.Text.ToUpper().Contains(op_)) {
						return true;
					}
				}
			}
			return false;
		}

		private void comboBox6_MouseClick(object sender, MouseEventArgs e) {
			cl_userediting = true;
			Redbrick.FocusHere(sender, e);
		}

		private void textBox7_TextChanged(object sender, EventArgs e) {
			TextBox t_ = sender as TextBox;
			if (t_.Text.Length > eNGINEERINGDataSet.CUT_PARTS.CNC1Column.MaxLength) {
				string msg_ = string.Format(Properties.Resources.LengthWarning,
					eNGINEERINGDataSet.CUT_PARTS.CNC1Column.MaxLength);
				Redbrick.Err(t_);
				cnc1_tooltip.SetToolTip(t_, msg_);
			} else {
				Redbrick.UnErr(t_);
				cnc1_tooltip.RemoveAll();
			}
			TogglePriorityButton();
		}

		private void textBox8_TextChanged(object sender, EventArgs e) {
			TextBox t_ = sender as TextBox;
			if (t_.Text.Length > eNGINEERINGDataSet.CUT_PARTS.CNC2Column.MaxLength) {
				string msg_ = string.Format(Properties.Resources.LengthWarning,
					eNGINEERINGDataSet.CUT_PARTS.CNC1Column.MaxLength);
				Redbrick.Err(t_);
				cnc2_tooltip.SetToolTip(t_, msg_);
			} else {
				Redbrick.UnErr(t_);
				cnc1_tooltip.RemoveAll();
			}
			TogglePriorityButton();
		}

		private void comboBox12_SelectedIndexChanged(object sender, EventArgs e) {
			ComboBox c_ = sender as ComboBox;
			if (c_.SelectedItem != null) {
				ToggleTypeWarn(false);
				FilterOps(string.Format(@"TYPEID = {0}", (sender as ComboBox).SelectedValue));
			} else {
				if (Properties.Settings.Default.Warn) {
					ToggleTypeWarn(true);
				}
				FilterOps(@"TYPEID = 1");
			}

			for (int i = 0; i < cbxes.Length; i++) {
				cbxes[i].SelectedValue = -1;
			}
		}

		private void button3_Click(object sender, EventArgs e) {
			if (Row != null && eNGINEERINGDataSet.CUT_PART_OPS.Rows.Count > 0) {
				ENGINEERINGDataSetTableAdapters.CutPartOpsTableAdapter cpoa_ =
					new ENGINEERINGDataSetTableAdapters.CutPartOpsTableAdapter();
				ENGINEERINGDataSet.CutPartOpsRow r =
					(cpoa_.GetDataByIDnOrder(Row.PARTID, 1)[0] as ENGINEERINGDataSet.CutPartOpsRow);
				EditOp eo = new EditOp(r);
				eo.ShowDialog(this);
			}
			GetRouting();
		}

		private void button4_Click(object sender, EventArgs e) {
			if (Row != null && eNGINEERINGDataSet.CUT_PART_OPS.Rows.Count > 1) {
				ENGINEERINGDataSetTableAdapters.CutPartOpsTableAdapter cpoa_ =
					new ENGINEERINGDataSetTableAdapters.CutPartOpsTableAdapter();
				ENGINEERINGDataSet.CutPartOpsRow r =
					(cpoa_.GetDataByIDnOrder(Row.PARTID, 2)[0] as ENGINEERINGDataSet.CutPartOpsRow);
				EditOp eo = new EditOp(r);
				eo.ShowDialog(this);
			}
			GetRouting();
		}

		private void button5_Click(object sender, EventArgs e) {
			if (Row != null && eNGINEERINGDataSet.CUT_PART_OPS.Rows.Count > 2) {
				ENGINEERINGDataSetTableAdapters.CutPartOpsTableAdapter cpoa_ =
					new ENGINEERINGDataSetTableAdapters.CutPartOpsTableAdapter();
				ENGINEERINGDataSet.CutPartOpsRow r =
					(cpoa_.GetDataByIDnOrder(Row.PARTID, 3)[0] as ENGINEERINGDataSet.CutPartOpsRow);
				EditOp eo = new EditOp(r);
				eo.ShowDialog(this);
			}
			GetRouting();
		}

		private void button6_Click(object sender, EventArgs e) {
			if (Row != null && eNGINEERINGDataSet.CUT_PART_OPS.Rows.Count > 3) {
				ENGINEERINGDataSetTableAdapters.CutPartOpsTableAdapter cpoa_ =
					new ENGINEERINGDataSetTableAdapters.CutPartOpsTableAdapter();
				ENGINEERINGDataSet.CutPartOpsRow r =
					(cpoa_.GetDataByIDnOrder(Row.PARTID, 4)[0] as ENGINEERINGDataSet.CutPartOpsRow);
				EditOp eo = new EditOp(r);
				eo.ShowDialog(this);
			}
			GetRouting();
		}

		private void button7_Click(object sender, EventArgs e) {
			if (Row != null && eNGINEERINGDataSet.CUT_PART_OPS.Rows.Count > 4) {
				ENGINEERINGDataSetTableAdapters.CutPartOpsTableAdapter cpoa_ =
					new ENGINEERINGDataSetTableAdapters.CutPartOpsTableAdapter();
				ENGINEERINGDataSet.CutPartOpsRow r =
					(cpoa_.GetDataByIDnOrder(Row.PARTID, 5)[0] as ENGINEERINGDataSet.CutPartOpsRow);
				EditOp eo = new EditOp(r);
				eo.ShowDialog(this);
			}
			GetRouting();
		}

		private void textBox1_TextChanged(object sender, EventArgs e) {
			bool err_ = (sender as TextBox).Text == string.Empty;
			bool length_err_ = (sender as TextBox).Text.Length > eNGINEERINGDataSet.CUT_PARTS.DESCRColumn.MaxLength;
			if (Properties.Settings.Default.Warn && err_) {
				ToggleDescrWarn(true);
			} else if (length_err_) {
				string msg_ = string.Format(Properties.Resources.LengthWarning,
					eNGINEERINGDataSet.CUT_PARTS.DESCRColumn.MaxLength);
				Redbrick.Err(sender as Control);
				descr_tooltup.SetToolTip(sender as Control, msg_);
			} else {
				ToggleDescrWarn(false);
			}
		}

		private void textBox11_TextChanged(object sender, EventArgs e) {
			bool err_ = (sender as TextBox).Text == string.Empty || (sender as TextBox).Text == @" ";
			if (Properties.Settings.Default.Warn && err_) {
				TogglePPBErr(true);
			} else {
				TogglePPBErr(false);
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

		private void swapLnW_Click(object sender, EventArgs e) {
			ov_userediting = true;
			Redbrick.SwapTextBoxContents(lengthtb, widthtb);
			if (Properties.Settings.Default.Warn) {
				ToggleEdgeWarn(true);
			}
			ov_userediting = false;
		}

		private void swapWnT_Click(object sender, EventArgs e) {
			ov_userediting = true;
			Redbrick.SwapTextBoxContents(widthtb, thicknesstb);
			if (Properties.Settings.Default.Warn) {
				ToggleEdgeWarn(true);
			}
			ov_userediting = false;
		}

		private void cutlistctl_KeyPress(object sender, KeyPressEventArgs e) {
			cl_userediting = true;
		}

		private void op_cbx_SelectedIndexChanged(object sender, EventArgs e) {
			CheckOversize();
			CheckEdgingOps();
		}

		private void dimension_textBox_Validated(object sender, EventArgs e) {
			TextBox _me = (sender as TextBox);
			string _text = Redbrick.enforce_number_format(_me.Text);
			_me.Text = _text;
			CheckOversize();
		}

		private void ppb_nud_ValueChanged(object sender, EventArgs e) {
			NumericUpDown nud_ = sender as NumericUpDown;
			if (Properties.Settings.Default.Warn && nud_.Value < 1) {
				TogglePPBErr(true);
			} else {
				TogglePPBErr(false);
				if (nud_.Value == 1) {
					CheckOversize();
				} else {
					ToggleOversizeWarn(false);
				}
			}
		}

		private void partq_ValueChanged(object sender, EventArgs e) {
			if (PropertySet != null && sender != null) {
				NumericUpDown nud_ = sender as NumericUpDown;
				PropertySet.CutlistQty = (float)Convert.ToDouble(nud_.Value);
				bool err_ = nud_.Value < 1 && cutlistctl.SelectedItem != null;
				if (Properties.Settings.Default.Warn && err_) {
					ToggleCutlistQtyErr(true);
				} else {
					ToggleCutlistQtyErr(false);
				}
			}
		}

		private void edgef_TextChanged(object sender, EventArgs e) {
			ComboBox cbx_ = sender as ComboBox;
			if (cbx_.Text == string.Empty) {
				cbx_.SelectedIndex = -1;
				label7.Visible = false;
				if (PropertySet != null) {
					PropertySet[@"EDGE FRONT (L)"].Data = 0;
					PropertySet[@"EFID"].Data = 0;
				}
				recalculate_blanksizeL();
				CheckEdgingOps();
			}
		}

		private void edgeb_TextChanged(object sender, EventArgs e) {
			ComboBox cbx_ = sender as ComboBox;
			if (cbx_.Text == string.Empty) {
				cbx_.SelectedIndex = -1;
				label8.Visible = false;
				if (PropertySet != null) {
					PropertySet[@"EDGE BACK (L)"].Data = 0;
					PropertySet[@"EBID"].Data = 0;
				}
				recalculate_blanksizeL();
				CheckEdgingOps();
			}
		}

		private void edgel_TextChanged(object sender, EventArgs e) {
			ComboBox cbx_ = sender as ComboBox;
			if (cbx_.Text == string.Empty) {
				cbx_.SelectedIndex = -1;
				label7.Visible = false;
				if (PropertySet != null) {
					PropertySet[@"EDGE LEFT (W)"].Data = 0;
					PropertySet[@"ELID"].Data = 0;
				}
				recalculate_blanksizeW();
				CheckEdgingOps();
			}
		}

		private void edger_TextChanged(object sender, EventArgs e) {
			ComboBox cbx_ = sender as ComboBox;
			if (cbx_.Text == string.Empty) {
				cbx_.SelectedIndex = -1;
				label10.Visible = false;
				if (PropertySet != null) {
					PropertySet[@"EDGE RIGHT (W)"].Data = 0;
					PropertySet[@"ERID"].Data = 0;
				}
				recalculate_blanksizeW();
				CheckEdgingOps();
			}
		}

		private void stat_cbx_SelectedIndexChanged(object sender, EventArgs e) {
			if (cl_stat_userediting && cutlistctl.SelectedItem != null) {
				DataRowView rv_ = cutlistctl.SelectedItem as DataRowView;
				int clid_ = Convert.ToInt32(rv_[@"CLID"]);
				ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter gu_ =
					new ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter();
				ENGINEERINGDataSetTableAdapters.CUT_CUTLISTSTableAdapter cc_ =
					new ENGINEERINGDataSetTableAdapters.CUT_CUTLISTSTableAdapter();
				ComboBox cbx_ = sender as ComboBox;
				int uid = Convert.ToInt32(gu_.GetUID(System.Environment.UserName));
				if (clid_ != 0 && cbx_.SelectedItem != null) {
					cc_.UpdateState(uid, Convert.ToInt32(cbx_.SelectedValue), clid_);
				}
				cl_stat_userediting = false;
			}
		}

		private void stat_cbx_Enter(object sender, EventArgs e) {
			cl_stat_userediting = true;
		}

		private void stat_cbx_Leave(object sender, EventArgs e) {
			cl_stat_userediting = false;
		}

		private void stat_cbx_MouseClick(object sender, MouseEventArgs e) {
			cl_stat_userediting = true;
		}

		private void update_btn_MouseClick(object sender, MouseEventArgs e) {
			CreateCutlist cc_ = new CreateCutlist(SwApp);
			cc_.ShowDialog(this);
		}

		private void remove_btn_MouseClick(object sender, MouseEventArgs e) {
			if (cutlistctl.SelectedItem != null) {
				DataRowView rv_ = cutlistctl.SelectedItem as DataRowView;
				string cutlist_name_ = Convert.ToString(rv_[@"CutlistDisplayName"]);
				string q_ = string.Format(@"Do you really want to remove '{0}' from '{1}'?", PropertySet.PartLookup, cutlist_name_);
				DialogResult dr_ = MessageBox.Show(this, q_, @"RLY?",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question,
					MessageBoxDefaultButton.Button2);
				if (dr_ == DialogResult.Yes) {
					eNGINEERINGDataSet.CUT_CUTLIST_PARTS.RemovePartFromCutlist(PropertySet);
					eNGINEERINGDataSet.GEN_ODOMETER.IncrementOdometer(Redbrick.Functions.RemovePart);
				}
			}
		}

		private void add_prt_btn_MouseClick(object sender, MouseEventArgs e) {
			AddToExistingCutlist atc_ = new AddToExistingCutlist(PropertySet);
			atc_.ShowDialog(this);
			eNGINEERINGDataSet.GEN_ODOMETER.IncrementOdometer(Redbrick.Functions.AddPart);
		}

		private void groupBox2_MouseHover(object sender, EventArgs e) {
			if (Properties.Settings.Default.ExtraInfo && req_info_ == string.Empty) {
				ENGINEERINGDataSetTableAdapters.RequestInfoTableAdapter ri_ =
					new ENGINEERINGDataSetTableAdapters.RequestInfoTableAdapter();
				ri_.FillByFixtureID(eNGINEERINGDataSet.RequestInfo, PropertySet.PartLookup);
				if (eNGINEERINGDataSet.RequestInfo.Count > 0) {
					ENGINEERINGDataSet.RequestInfoRow r_ =
						(ENGINEERINGDataSet.RequestInfoRow)eNGINEERINGDataSet.RequestInfo[0];
					StringBuilder sb_ = new StringBuilder();
					if (r_[@"ITEMNUM"] != DBNull.Value) {
						sb_.AppendFormat(@"Project '{0}' is in '{1}' status.", Convert.ToString(r_.ITEMNUM), r_.RSNAME);
					} else {
						sb_.AppendFormat(@"Project '{0}' is in '{1}' status.", Convert.ToString(r_.FIXID), r_.RSNAME);
					}
					sb_.AppendLine();
					sb_.AppendLine();
					foreach (string line in Redbrick.WrapText(r_.DESCRIPTION, 40)) {
						sb_.AppendLine(line);
					}
					sb_.AppendLine();
					sb_.AppendFormat(@"Created by {0} on {1:M/d/yyyy}", Redbrick.TitleCase(r_.Creator), r_.CDATE);
					sb_.AppendLine();
					sb_.AppendFormat(@"Lead: {0}", Redbrick.TitleCase(r_.Lead));
					sb_.AppendLine();
					req_info_ = sb_.ToString();
				}
			}
			req_tooltip.Show(req_info_, sender as GroupBox, 30000);
		}

		private void pull_btn_Click(object sender, EventArgs e) {
			GetMaterialFromPart();
			GetEdgesFromPart();
		}

		private string GetMaterialPrices(int _matid) {
			if (mat_price_ != string.Empty) {
				return mat_price_;
			}

			string msg_ = string.Empty;
			ENGINEERINGDataSet.CUT_MATERIALSDataTable dt_ =
				new ENGINEERINGDataSet.CUT_MATERIALSDataTable();
			foreach (var item in dt_.GetMaterialPricing(_matid)) {
				msg_ += string.Format(@"{0} {1}{2}", Properties.Settings.Default.Bullet, item, System.Environment.NewLine);
			}
			mat_price_ = msg_;
			return mat_price_;
		}

		private void GetEdgePrices(int _edgeid, ref string _info) {
			if (_info != string.Empty) {
				return;
			}

			string msg_ = string.Empty;
			ENGINEERINGDataSet.CUT_MATERIALSDataTable dt_ =
				new ENGINEERINGDataSet.CUT_MATERIALSDataTable();
			foreach (var item in dt_.GetEdgePricing(_edgeid)) {
				msg_ += string.Format(@"{0} {1}{2}", Properties.Settings.Default.Bullet, item, System.Environment.NewLine);
			}
			_info = msg_;
		}

		private void label6_MouseHover(object sender, EventArgs e) {
			if (Properties.Settings.Default.ExtraInfo && cutlistMat.SelectedItem != null) {
				DataRowView drv_ = cutlistMat.SelectedItem as DataRowView;
				int matid_ = Convert.ToInt32(drv_[@"MATID"]);
				string msg_ = GetMaterialPrices(matid_);
				cutlistMat_tooltip.Show(msg_, sender as Label, 30000);
			}
		}

		private void label7_MouseHover(object sender, EventArgs e) {
			if (Properties.Settings.Default.ExtraInfo && edgef.SelectedItem != null) {
				DataRowView drv_ = edgef.SelectedItem as DataRowView;
				int edgeid_ = Convert.ToInt32(drv_[@"EDGEID"]);
				GetEdgePrices(edgeid_, ref edgef_price_);
				edgef_mat_tip.Show(edgef_price_, sender as Label, 30000);
			} else {
				edgef_price_ = string.Empty;
			}
		}

		private void label8_MouseHover(object sender, EventArgs e) {
			if (Properties.Settings.Default.ExtraInfo && edgeb.SelectedItem != null) {
				DataRowView drv_ = edgeb.SelectedItem as DataRowView;
				int edgeid_ = Convert.ToInt32(drv_[@"EDGEID"]);
				GetEdgePrices(edgeid_, ref edgeb_price_);
				edgeb_mat_tip.Show(edgeb_price_, sender as Label, 30000);
			} else {
				edgeb_price_ = string.Empty;
			}
		}

		private void label9_MouseHover(object sender, EventArgs e) {
			if (Properties.Settings.Default.ExtraInfo && edgel.SelectedItem != null) {
				DataRowView drv_ = edgel.SelectedItem as DataRowView;
				int edgeid_ = Convert.ToInt32(drv_[@"EDGEID"]);
				GetEdgePrices(edgeid_, ref edgel_price_);
				edgel_mat_tip.Show(edgel_price_, sender as Label, 30000);
			} else {
				edgel_price_ = string.Empty;
			}
		}

		private void label10_MouseHover(object sender, EventArgs e) {
			if (Properties.Settings.Default.ExtraInfo && edger.SelectedItem != null) {
				DataRowView drv_ = edger.SelectedItem as DataRowView;
				int edgeid_ = Convert.ToInt32(drv_[@"EDGEID"]);
				GetEdgePrices(edgeid_, ref edger_price_);
				edger_mat_tip.Show(edger_price_, sender as Label, 30000);
			} else {
				edger_price_ = string.Empty;
			}
		}

		private void cutlistctl_TextChanged(object sender, EventArgs e) {
			ComboBox c_ = sender as ComboBox;
			if (cl_userediting && c_.Text.Trim() == string.Empty) {
				c_.SelectedIndex = -1;
				PropertySet.CutlistID = 0;
				CutlistPartsRow = null;
				GetMaterialFromPart();
				GetEdgesFromPart();
				ToggleCutlistErr(true);
				EnableCutlistSpec(false);
				Properties.Settings.Default.LastCutlist = 0;
				Properties.Settings.Default.Save();
				cl_userediting = false;
			}
		}

		private void cutlistctl_KeyDown(object sender, KeyEventArgs e) {
			(sender as ComboBox).DroppedDown = false;
			cl_userediting = true;
		}

		private void edgeL_KeyDown(object sender, KeyEventArgs e) {
			ComboBox c_ = sender as ComboBox;
			c_.DroppedDown = false;
			//if (e.Control && (e.KeyCode == Keys.V || e.KeyCode == Keys.Insert || e.KeyCode == Keys.X)) {
			//	if (c_.Items.Contains(c_.Text)) {
			//		c_.SelectedIndex = c_.Items.IndexOf(c_.Text);
			//	}
			//	recalculate_blanksizeL();
			//}
		}

		private void edgeW_KeyDown(object sender, KeyEventArgs e) {
			ComboBox c_ = sender as ComboBox;
			c_.DroppedDown = false;
			//if (e.Control && (e.KeyCode == Keys.V || e.KeyCode == Keys.Insert || e.KeyCode == Keys.X)) {
			//	if (c_.Items.Contains(c_.Text)) {
			//		c_.SelectedIndex = c_.Items.IndexOf(c_.Text);
			//	}
			//	recalculate_blanksizeW();
			//}
		}
	}
}
