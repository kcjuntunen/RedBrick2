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
	/// A form for manipulating data on a SolidWorks Drawing.
	/// </summary>
	public partial class DrawingRedbrick : UserControl {
		private string partLookup;
		private string projectDescr;
		private Revs revSet;
		private ToolTip cust_tooltip = new ToolTip();
		private ToolTip rev_tooltip = new ToolTip();
		private ToolTip status_tooltip = new ToolTip();
		private int clid = 0;
		private bool user_editing = false;
		private bool jobs = false;
		private string jobs_msg = string.Empty;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="md">A ModelDoc2.</param>
		/// <param name="sw">The connected application.</param>
		public DrawingRedbrick(ModelDoc2 md, SldWorks sw) {
			ActiveDoc = md;
			SwApp = sw;
			InitializeComponent();
			groupBox5.MouseClick += groupBox5_MouseClick;
			label32.MouseDown += label32_MouseDown;
			label34.MouseDown += label34_MouseDown;
			label36.MouseDown += label36_MouseDown;
			label38.MouseDown += label38_MouseDown;
			label40.MouseDown += label40_MouseDown;
			label41.MouseDown += label41_MouseDown;
			label42.MouseDown += label42_MouseDown;
			ToggleFlameWar(Properties.Settings.Default.FlameWar);
		}

		void label42_MouseDown(object sender, MouseEventArgs e) {
			Redbrick.Clip(Redbrick.TitleCase(Convert.ToString(auth_cpx.Text)));
		}

		void label40_MouseDown(object sender, MouseEventArgs e) {
			Redbrick.Clip(fin5_tb.Text);
		}

		void label38_MouseDown(object sender, MouseEventArgs e) {
			Redbrick.Clip(fin4_tb.Text);
		}

		void label36_MouseDown(object sender, MouseEventArgs e) {
			Redbrick.Clip(fin3_tb.Text);
		}

		void label34_MouseDown(object sender, MouseEventArgs e) {
			Redbrick.Clip(fin2_tb.Text);
		}

		void label32_MouseDown(object sender, MouseEventArgs e) {
			Redbrick.Clip(fin1_tb.Text);
		}

		private void label41_MouseDown(object sender, MouseEventArgs e) {
			if (cust_cbx.SelectedItem != null) {
				DataRowView rv_ = cust_cbx.SelectedItem as DataRowView;
				Redbrick.Clip(Convert.ToString(rv_[@"CUSTOMER"]));
			}
		}

		void groupBox5_MouseClick(object sender, MouseEventArgs e) {
			Redbrick.Clip(partLookup);
		}

		private string GetJobsDue() {
			if (partLookup != null && partLookup != string.Empty &&
				RevFromDrw != null && RevFromDrw != string.Empty) {
				ENGINEERINGDataSetTableAdapters.jomastTableAdapter jo_ =
					new ENGINEERINGDataSetTableAdapters.jomastTableAdapter();
				jo_.FillByItemAndRev(eNGINEERINGDataSet.jomast, partLookup, RevFromDrw);
				int lim_ = eNGINEERINGDataSet.jomast.Count > 3 ? 3 : eNGINEERINGDataSet.jomast.Count;
				if (lim_ > 0) {
					string msg_ = string.Format("Open/Released jobs for {0} REV {1}\n", partLookup, RevFromDrw);
					int len_ = msg_.Length;
					for (int i = 0; i < len_; i++) {
						msg_ += "-";
					}
					msg_ += "\n";
					for (int i = 0; i < lim_; i++) {
						ENGINEERINGDataSet.jomastRow r_ = eNGINEERINGDataSet.jomast[i];
						msg_ += string.Format("Job #: {0}; Due: {1:M/d/yyyy}; Qty: {2:0}; Status: {3}\n",
							r_.fjobno, r_.fddue_date, r_.fquantity, r_.fstatus);
					}
					//foreach (ENGINEERINGDataSet.jomastRow r_ in eNGINEERINGDataSet.jomast) {
					//	msg_ += string.Format("Job #: {0}; Due: {1:M/d/yyyy}; Qty: {2:0}; Status: {3}\n",
					//		r_.fjobno, r_.fddue_date, r_.fquantity, r_.fstatus);
					//}
					ENGINEERINGDataSetTableAdapters.jomastTotalsTableAdapter jot_ =
						new ENGINEERINGDataSetTableAdapters.jomastTotalsTableAdapter();
					jot_.FillCountByItemAndRev(eNGINEERINGDataSet.jomastTotals, partLookup, RevFromDrw);
					if (eNGINEERINGDataSet.jomastTotals.Count > 0) {
						ENGINEERINGDataSet.jomastTotalsRow r_ = eNGINEERINGDataSet.jomastTotals[0] as ENGINEERINGDataSet.jomastTotalsRow;
						if (r_.jobqty > lim_) {
							msg_ += "...\n";
						}
						if (r_.jobqty > 1) {
							msg_ += string.Format("There are {0} jobs, requiring a total quantity of {1:0} parts,\n"
								+ "with an average of {2:0.0} parts per job.",
								r_.jobqty, r_.partqty, r_.partavgqty);
						} else if (r_.jobqty == 1) {
							msg_ += string.Format(@"There's {0} job, requiring a total quantity of {1:0} parts.",
								Convert.ToInt32(r_.jobqty), Convert.ToDouble(r_.partqty));
						}
					}
					return msg_;
				}
			}
			return @"No jobs.";
		}

		/// <summary>
		/// Conditionally constrain character casing.
		/// </summary>
		/// <param name="on">"True" forces hideous text in certain TextBoxes. "False" allows the full
		/// complement of characters.</param>
		public void ToggleFlameWar(bool on) {
			if (on) {
				fin1_tb.CharacterCasing = CharacterCasing.Upper;
				fin2_tb.CharacterCasing = CharacterCasing.Upper;
				fin3_tb.CharacterCasing = CharacterCasing.Upper;
				fin4_tb.CharacterCasing = CharacterCasing.Upper;
				fin5_tb.CharacterCasing = CharacterCasing.Upper;
			} else {
				fin1_tb.CharacterCasing = CharacterCasing.Normal;
				fin2_tb.CharacterCasing = CharacterCasing.Normal;
				fin3_tb.CharacterCasing = CharacterCasing.Normal;
				fin4_tb.CharacterCasing = CharacterCasing.Normal;
				fin5_tb.CharacterCasing = CharacterCasing.Normal;
			}
		}

		private void InitData() {
			jobs = false;
			treeView1.ImageList = Redbrick.TreeViewIcons;
			foreach (TreeNode tn in treeView1.Nodes) {
				if (tn != null) {
					tn.Remove();
				}
			}
			treeView1.Nodes.Clear();
			// TODO: fix this for new drawings.
			string path_ = ActiveDoc.GetPathName();
			if (path_ != string.Empty) {
				PartFileInfo = new FileInfo(path_);
				partLookup = Redbrick.FileInfoToLookup(PartFileInfo);
				if (PartFileInfo.Name.ToUpper().Contains(@"REV")) {
					string[] revcheck = Path.GetFileNameWithoutExtension(PartFileInfo.Name).
						Split(new string[] { @"REV" }, StringSplitOptions.RemoveEmptyEntries);
					RevFromFile = revcheck.Length > 1 ? revcheck[revcheck.Length - 1] : null;
				} else {
					RevFromFile = null;
				}
				ENGINEERINGDataSet.SCH_PROJECTSRow r =
					(new ENGINEERINGDataSet.SCH_PROJECTSDataTable()).GetCorrectCustomer(partLookup);
				if (r != null) {
					ProjectCustomer = r.CUSTID;
					projectDescr = r.DESCRIPTION;
				} else {
					ProjectCustomer = 0;
					projectDescr = string.Empty;
				}
				//ProjectCustomer = GetCorrectCustomer();
				groupBox5.Text = projectDescr != string.Empty ? string.Format(@"{0} - {1}", partLookup, projectDescr) : partLookup;
			} else {
				groupBox5.Text = @"New Drawing";
				DrawingDoc dd_ = ActiveDoc as DrawingDoc;
				dd_.FileSavePostNotify += dd__FileSavePostNotify;
			}
		}

		int dd__FileSavePostNotify(int saveType, string FileName) {
			if (FileName.ToUpper().EndsWith(@"SLDDRW")) {
				ReLoad(SwApp.ActiveDoc);
			}
			return 0;
		}

		private void FigureOutCustomer() {
			SwProperty _p = new CustomerProperty(@"CUSTOMER", true, SwApp, ActiveDoc);
			PropertySet.Add(_p.Get());
			cust_cbx.SelectedValue = (int)_p.Data;
			if (cust_cbx.SelectedItem != null) {
				if ((ProjectCustomer != 0) && ((int)cust_cbx.SelectedValue != ProjectCustomer)) {
					ToggleCustomerWarn(true);
				}
			} else {
				cust_cbx.Text = _p.Value;
				ToggleCustomerWarn(true);
			}
		}

		private void FigureOutAuthor() {
			SwProperty ap_ = new AuthorProperty(@"DrawnBy", true, SwApp, ActiveDoc);
			SwProperty afip_ = new AuthorUIDProperty(@"Author", true, SwApp, ActiveDoc);
			PropertySet.Add(ap_.Get());
			PropertySet.Add(afip_.Get());
			if (afip_.Value != string.Empty) {
				auth_cpx.SelectedValue = (int)afip_.Data;
			} else if (ap_.Value != string.Empty) {
				auth_cpx.SelectedValue = (int)ap_.Data;
			} else {
				ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter gu =
					new ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter();
				int? uid = gu.GetUID(System.Environment.UserName);
				if (uid > 0) {
					auth_cpx.SelectedValue = uid;
				}
			}
		}

		private void FigureOutRev() {
			ENGINEERINGDataSetTableAdapters.RevListTableAdapter rl =
				new ENGINEERINGDataSetTableAdapters.RevListTableAdapter();
			rev_cbx.DisplayMember = @"REV";
			rev_cbx.ValueMember = @"REV";
			rev_cbx.DataSource = rl.GetData();
			SwProperty _p = new StringProperty(@"REVISION LEVEL", true, SwApp, ActiveDoc, string.Empty);
			PropertySet.Add(_p.Get());
			RevFromDrw = _p.Value;
			rev_cbx.Text = RevFromDrw;
			if (RevFromFile != null && RevFromDrw != RevFromFile) {
				ToggleRevWarn(true);
			}
		}

		private void FigureOutDate() {
			DateProperty _p = new DateProperty(@"DATE", true, SwApp, ActiveDoc);
			PropertySet.Add(_p.Get());
			dateTimePicker1.Value = (DateTime)_p.Data;
		}

		private void FigureOutMatFinish() {
			ComboBox[] _cboxes = new ComboBox[] { mat1_cbx, mat2_cbx, mat3_cbx, mat4_cbx, mat5_cbx };
			TextBox[] _tboxes = new TextBox[] { fin1_tb, fin2_tb, fin3_tb, fin4_tb, fin5_tb };

			for (int i = 0; i < 5; i++) {
				string _mat = string.Format(@"M{0}", i + 1);
				string _fin = string.Format(@"FINISH {0}", i + 1);
				StringProperty _m = new StringProperty(_mat, true, SwApp, ActiveDoc, string.Empty);
				StringProperty _f = new StringProperty(_fin, true, SwApp, ActiveDoc, string.Empty);
				PropertySet.Add(_m.Get());
				PropertySet.Add(_f.Get());
				_cboxes[i].Text = _m.ResolvedValue;
				_tboxes[i].Text = _f.ResolvedValue;
			}
		}

		private void FigureOutStatus() {
			if (partLookup != null && partLookup.Length > 0) {
				ENGINEERINGDataSetTableAdapters.CUT_CUTLISTSTableAdapter cc =
					new ENGINEERINGDataSetTableAdapters.CUT_CUTLISTSTableAdapter();
				ENGINEERINGDataSet.CUT_CUTLISTSDataTable dt =
					cc.GetDataByName(partLookup, RevFromDrw.ToString());
				if (dt.Rows.Count > 0) {
					status_cbx.SelectedValue = dt[0].STATEID;
					clid = dt[0].CLID;
				} else {
					status_cbx.SelectedValue = -1;
				}
			}
		}

		/// <summary>
		/// Write data to properties.
		/// </summary>
		public void Commit() {
			PropertySet[@"CUSTOMER"].Data = cust_cbx.SelectedValue;
			PropertySet[@"REVISION LEVEL"].Data = rev_cbx.Text;
			PropertySet[@"DrawnBy"].Data = auth_cpx.SelectedValue;
			PropertySet[@"Author"].Data = auth_cpx.SelectedValue;
			PropertySet[@"DATE"].Data = dateTimePicker1.Value;

			ComboBox[] _cboxes = new ComboBox[] { mat1_cbx, mat2_cbx, mat3_cbx, mat4_cbx, mat5_cbx };
			TextBox[] _tboxes = new TextBox[] { fin1_tb, fin2_tb, fin3_tb, fin4_tb, fin5_tb };

			for (int i = 0; i < 5; i++) {
				string _mat = string.Format(@"M{0}", i + 1);
				string _fin = string.Format(@"FINISH {0}", i + 1);
				PropertySet[_mat].Data = _cboxes[i].Text;
				PropertySet[_fin].Data = _tboxes[i].Text;
			}
			PropertySet.Write();
			RevSet.Write();
			(ActiveDoc as DrawingDoc).ForceRebuild();
		}

		private void DrawingRedbrick_Load(object sender, EventArgs e) {
			gEN_CUSTOMERSTableAdapter.Fill(eNGINEERINGDataSet.GEN_CUSTOMERS);
			gEN_USERSTableAdapter.Fill(eNGINEERINGDataSet.GEN_USERS);
			cUT_STATESTableAdapter.Fill(eNGINEERINGDataSet.CUT_STATES);
			cUT_DRAWING_MATERIALSTableAdapter.Fill(eNGINEERINGDataSet.CUT_DRAWING_MATERIALS);
			ReLoad();
		}

		/// <summary>
		/// Reload data.
		/// </summary>
		/// <param name="md">ModelDoc2 from which to reload data.</param>
		public void ReLoad(ModelDoc2 md) {
			ActiveDoc = md;
			ReLoad();
		}

		/// <summary>
		/// Reload everything with current ModelDoc2.
		/// </summary>
		public void ReLoad() {
			InitData();

			if (Properties.Settings.Default.OnlyActiveAuthors) {
				gENUSERSBindingSource.Filter = @"ACTIVE = True AND DEPT = 6";
			} else {
				gENUSERSBindingSource.Filter = @"DEPT = 6";
			}

			if (Properties.Settings.Default.OnlyCurrentCustomers) {
				gENCUSTOMERSBindingSource.Filter = @"CUSTACTIVE = True";
			}

			if (PropertySet == null) {
				PropertySet = new SwProperties(SwApp, ActiveDoc);
			} else {
				PropertySet.Clear();
			}

			FigureOutCustomer();
			FigureOutAuthor();
			FigureOutRev();
			FigureOutDate();
			FigureOutStatus();
			FigureOutMatFinish();
			RevSet = new Revs(SwApp);
			BuildTree();
		}

		private void BuildTree() {
			treeView1.Nodes.Clear();
			foreach (Rev r in RevSet) {
				treeView1.Nodes.Add(r.Node);
			}
		}

		private Revs RevSet {
			get { return revSet; }
			set { revSet = value; }
		}

		/// <summary>
		/// The property set we've gotten from the ActiveDoc.
		/// </summary>
		public SwProperties PropertySet { get; set; }

		/// <summary>
		/// The customer ID for this project.
		/// </summary>
		public int ProjectCustomer { get; set; }

		/// <summary>
		/// The rev we've collected from the file, if any.
		/// </summary>
		public string RevFromFile { get; set; }

		/// <summary>
		/// The rev we've collected from the drawing, if any.
		/// </summary>
		public string RevFromDrw { get; set; }

		/// <summary>
		/// A FileInfo object of this drawing's file.
		/// </summary>
		public FileInfo PartFileInfo { get; set; }

		/// <summary>
		/// The current ModelDoc2.
		/// </summary>
		public ModelDoc2 ActiveDoc { get; set; }

		/// <summary>
		/// The connected application.
		/// </summary>
		public SldWorks SwApp { get; set; }

		private void comboBox_KeyDown(object sender, KeyEventArgs e) {
			(sender as ComboBox).DroppedDown = false;
		}

		private void treeView1_DrawNode(object sender, DrawTreeNodeEventArgs e) {
			Color nodeColor = Color.Black;
			if ((e.State & TreeNodeStates.Selected) != 0)
				nodeColor = SystemColors.HighlightText;
			foreach (KeyValuePair<string, Redbrick.Format> item in Redbrick.action) {
				switch (item.Value) {
					case Redbrick.Format.NAME:
						nodeColor = Color.Gray;
						break;
					case Redbrick.Format.STRING:
						break;
					case Redbrick.Format.DATE:
						nodeColor = Color.Green;
						break;
					case Redbrick.Format.SKIP:
						break;
					default:
						break;
				}
			}
			TextRenderer.DrawText(e.Graphics,
														e.Node.Text,
														e.Node.NodeFont,
														e.Bounds,
														nodeColor,
														Color.Empty,
														TextFormatFlags.VerticalCenter);
		}

		private void ToggleRevWarn(bool on) {
			if (on) {
				Redbrick.Err(rev_cbx);
				rev_tooltip.SetToolTip(rev_cbx, Properties.Resources.RevisionNotMatching);
				rev_tooltip.SetToolTip(label44, Properties.Resources.RevisionNotMatching);
			} else {
				Redbrick.UnErr(rev_cbx as ComboBox);
				rev_tooltip.RemoveAll();
			}
		}

		private void ToggleCustomerWarn(bool on) {
			if (on) {
				Redbrick.Err(cust_cbx);
				cust_tooltip.SetToolTip(cust_cbx, Properties.Resources.CustomerNotMatching);
				cust_tooltip.SetToolTip(label41, Properties.Resources.CustomerNotMatching);
			} else {
				Redbrick.UnErr(cust_cbx);
				cust_tooltip.RemoveAll();
			}
		}

		private void comboBox14_SelectedIndexChanged(object sender, EventArgs e) {
			if (RevFromFile == null || (sender as ComboBox).Text == RevFromFile) {
				ToggleRevWarn(false);
			} else {
				ToggleRevWarn(true);
			}
		}

		private void comboBox12_SelectedIndexChanged(object sender, EventArgs e) {
			if ((sender as ComboBox).SelectedItem != null) {
				if ((ProjectCustomer == 0) || (int)(sender as ComboBox).SelectedValue == ProjectCustomer) {
					ToggleCustomerWarn(false);
				} else {
					ToggleCustomerWarn(true);
				}
			} else {
				(sender as ComboBox).Text = PropertySet[@"CUSTOMER"].Value;
				ToggleCustomerWarn(true);
			}
		}

		private void button1_Click(object sender, EventArgs e) {
			EditRev eo = new EditRev(RevSet);
			eo.Added += er_Added;
			eo.ShowDialog(this);
			BuildTree();
		}

		private int SelectedNode() {
			TreeNode node = treeView1.SelectedNode;
			if (node != null) {
				while (node.Parent != null) {
					node = node.Parent;
				}
				return node.Index;
			} else {
				return 0;
			}
		}

		private void button6_Click(object sender, EventArgs e) {
			TreeNode node = treeView1.SelectedNode;
			if (node != null) {
				while (node.Parent != null) {
					node = node.Parent;
				}
				EditRev er = new EditRev(SelectedNode(), RevSet);
				er.Added += er_Added;
				er.ShowDialog(this);
			} else {
				EditRev er = new EditRev(0, RevSet);
				er.Added += er_Added;
				er.ShowDialog(this);
			}
			BuildTree();
		}

		void er_Added(object sender, EventArgs e) {
			DrawingDoc thisdd = (DrawingDoc)SwApp.ActiveDoc;
			Commit();
			int lastrev = revSet.Count - 1;
			treeView1.Nodes.Add(revSet[lastrev].Node);
			revSet.Write();
			thisdd.ForceRebuild();
		}

		private void button7_Click(object sender, EventArgs e) {
			RevSet.Delete(SelectedNode());
			BuildTree();
		}

		private void comboBox_Resize(object sender, EventArgs e) {
			ComboBox _me = (sender as ComboBox);
			_me.SelectionLength = 0;
		}

		private void button2_Click(object sender, EventArgs e) {
			CreateCutlist c = new CreateCutlist(SwApp);
			c.ShowDialog(this);
		}

		private void enterJump(object sender, object[] boxes) {
			for (int i = 0; i < boxes.Length; i++) {
				if (sender == boxes[i] && i < boxes.Length) {
					if (sender is TextBox) {
						TextBox cur = sender as TextBox;
						TextBox nxt = boxes[i + 1] as TextBox;
						if (cur.SelectionStart < cur.TextLength) {
							int curPos = cur.SelectionStart;
							cur.SelectionLength = cur.TextLength - curPos;
							string selectedText = cur.SelectedText;
							cur.Text = cur.Text.Remove(curPos);
							nxt.Text = string.Format(@"{0}{1}", selectedText, nxt.Text).Trim();
						}
						nxt.Focus();
						nxt.SelectionLength = 0;
					} else if (sender is ComboBox) {
						(boxes[i + 1] as ComboBox).Focus();
					}
				}
			}
		}

		private void textBox_KeyUp(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Enter && e.Shift) {
				TextBox[] tbs = { fin5_tb, fin4_tb, fin3_tb, fin2_tb, fin1_tb };
				enterJump((sender as TextBox), tbs);
			} else if (e.KeyCode == Keys.Enter) {
				TextBox[] tbs = { fin1_tb, fin2_tb, fin3_tb, fin4_tb, fin5_tb };
				enterJump((sender as TextBox), tbs);
			}
		}

		private void comboBox_KeyUp(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Enter && e.Shift) {
				ComboBox[] cbs = { mat5_cbx, mat4_cbx, mat3_cbx, mat2_cbx, mat1_cbx };
				enterJump((sender as ComboBox), cbs);
			} else if (e.KeyCode == Keys.Enter) {
				ComboBox[] cbs = { mat1_cbx, mat2_cbx, mat3_cbx, mat4_cbx, mat5_cbx };
				enterJump((sender as ComboBox), cbs);
			}
		}

		private void status_cbx_SelectedIndexChanged(object sender, EventArgs e) {
			if (user_editing) {
				ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter gu_ =
					new ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter();
				ENGINEERINGDataSetTableAdapters.CUT_CUTLISTSTableAdapter cc_ =
					new ENGINEERINGDataSetTableAdapters.CUT_CUTLISTSTableAdapter();
				ComboBox cbx_ = sender as ComboBox;
				int uid = Convert.ToInt32(gu_.GetUID(System.Environment.UserName));
				if (clid != 0 && cbx_.SelectedItem != null) {
					cc_.UpdateState(uid, Convert.ToInt32(cbx_.SelectedValue), clid);
				}
			}
		}

		private void status_cbx_Enter(object sender, EventArgs e) {
			user_editing = true;
		}

		private void status_cbx_Leave(object sender, EventArgs e) {
			user_editing = false;
		}

		private void button3_Click(object sender, EventArgs e) {
			string q_ = string.Format(@"Are you sure you want to delete {0} REV {1}", partLookup, rev_cbx.Text);
			DialogResult dr_ = MessageBox.Show(this, q_, @"RLY?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (dr_ == DialogResult.Yes) {
				System.Diagnostics.Debug.Print(@"Deleting...");
			}
		}

		private void status_cbx_MouseClick(object sender, MouseEventArgs e) {
			user_editing = true;
			Redbrick.FocusHere(sender, e);
		}

		private void label45_MouseHover(object sender, EventArgs e) {
			if (!jobs) {
				jobs_msg = GetJobsDue();
			}
			status_tooltip.Show(jobs_msg, sender as Label, 30000);
		}
	}
}
