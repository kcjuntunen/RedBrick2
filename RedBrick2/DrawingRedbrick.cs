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
	public partial class DrawingRedbrick : UserControl {
		private string partLookup;
		private string projectDescr;
		private Revs revSet;

		public DrawingRedbrick(ModelDoc2 md, SldWorks sw) {
			ActiveDoc = md;
			SwApp = sw;
			InitializeComponent();
			ToggleFlameWar(Properties.Settings.Default.FlameWar);
		}

		public void ToggleFlameWar(bool on) {
			if (on) {
				textBox14.CharacterCasing = CharacterCasing.Upper;
				textBox15.CharacterCasing = CharacterCasing.Upper;
				textBox16.CharacterCasing = CharacterCasing.Upper;
				textBox17.CharacterCasing = CharacterCasing.Upper;
				textBox18.CharacterCasing = CharacterCasing.Upper;
			} else {
				textBox14.CharacterCasing = CharacterCasing.Normal;
				textBox15.CharacterCasing = CharacterCasing.Normal;
				textBox16.CharacterCasing = CharacterCasing.Normal;
				textBox17.CharacterCasing = CharacterCasing.Normal;
				textBox18.CharacterCasing = CharacterCasing.Normal;
			}
		}

		private void InitData() {
			treeView1.ImageList = Redbrick.TreeViewIcons;
			foreach (TreeNode tn in treeView1.Nodes) {
				if (tn != null) {
					tn.Remove();
				}
			}
			treeView1.Nodes.Clear();
			PartFileInfo = new FileInfo(ActiveDoc.GetPathName());
			string[] fi = Path.GetFileNameWithoutExtension(PartFileInfo.Name).Split(' ');
			partLookup = fi[0];
			string[] revcheck = Path.GetFileNameWithoutExtension(PartFileInfo.Name).
				Split(new string[] { @"REV", @" " }, StringSplitOptions.RemoveEmptyEntries);
			RevFromFile = revcheck.Length > 1 ? revcheck[1] : null;
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
		}

		//private int GetCorrectCustomer() {
		//  string pattern = @"([A-Z]{3,4})(\d{4})";
		//  System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(pattern);
		//  System.Text.RegularExpressions.Match matches = System.Text.RegularExpressions.Regex.Match(partLookup, pattern);
		//  if (r.IsMatch(partLookup)) {
		//    ENGINEERINGDataSetTableAdapters.SCH_PROJECTSTableAdapter spta =
		//      new ENGINEERINGDataSetTableAdapters.SCH_PROJECTSTableAdapter();
		//    ENGINEERINGDataSet.SCH_PROJECTSRow row = spta.GetDataByProject(matches.Groups[1].ToString())[0];
		//    projectDescr = row.DESCRIPTION;
		//    return row.CUSTID;
		//  }
		//  return 0;
		//}

		private void FigureOutCustomer() {
			SwProperty _p = new CustomerProperty(@"CUSTOMER", true, SwApp, ActiveDoc);
			PropertySet.Add(_p.Get());
			comboBox12.SelectedValue = (int)_p.Data;
			if (comboBox12.SelectedItem != null) {
				if ((ProjectCustomer != 0) && ((int)comboBox12.SelectedValue != ProjectCustomer)) {
					Redbrick.Warn(comboBox12);
				}
			} else {
				comboBox12.Text = _p.Value;
				Redbrick.Warn(comboBox12);
			}
		}

		private void FigureOutAuthor() {
			SwProperty _p = new AuthorProperty(@"DrawnBy", true, SwApp, ActiveDoc);
			PropertySet.Add(_p.Get());
			if (_p.Value != string.Empty) {
				comboBox13.SelectedValue = (int)_p.Data;
			} else {
				ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter gu =
					new ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter();
				int? uid = gu.GetUID(System.Environment.UserName);
				if (uid > 0) {
					comboBox13.SelectedValue = uid;
				}
			}
		}

		private void FigureOutRev() {
			ENGINEERINGDataSetTableAdapters.RevListTableAdapter rl =
				new ENGINEERINGDataSetTableAdapters.RevListTableAdapter();
			comboBox14.DisplayMember = @"REV";
			comboBox14.ValueMember = @"REV";
			comboBox14.DataSource = rl.GetData();
			SwProperty _p = new StringProperty(@"REVISION LEVEL", true, SwApp, ActiveDoc, string.Empty);
			PropertySet.Add(_p.Get());
			RevFromDrw = _p.Value;
			comboBox14.Text = RevFromDrw;
			if (RevFromFile != null && RevFromDrw != RevFromFile) {
				Redbrick.Warn(comboBox14);
			}
		}

		private void FigureOutDate() {
			DateProperty _p = new DateProperty(@"DATE", true, SwApp, ActiveDoc);
			PropertySet.Add(_p.Get());
			dateTimePicker1.Value = (DateTime)_p.Data;
		}

		private void FigureOutMatFinish() {
			ComboBox[] _cboxes = new ComboBox[] { comboBox7, comboBox8, comboBox9, comboBox10, comboBox11 };
			TextBox[] _tboxes = new TextBox[] { textBox14, textBox15, textBox16, textBox17, textBox18 };

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
			ENGINEERINGDataSetTableAdapters.CUT_CUTLISTSTableAdapter cc =
				new ENGINEERINGDataSetTableAdapters.CUT_CUTLISTSTableAdapter();
			ENGINEERINGDataSet.CUT_CUTLISTSDataTable dt =
				cc.GetDataByName(partLookup, RevFromDrw.ToString());
			if (dt.Rows.Count > 0) {
				comboBox15.SelectedValue = dt[0].STATEID;
			} else {
				comboBox15.SelectedValue = -1;
			}
		}

		public void Commit() {
			PropertySet[@"CUSTOMER"].Data = comboBox12.SelectedValue;
			PropertySet[@"REVISION LEVEL"].Data = comboBox14.Text;
			PropertySet[@"DrawnBy"].Data = comboBox13.SelectedValue;
			PropertySet[@"DATE"].Data = dateTimePicker1.Value;

			ComboBox[] _cboxes = new ComboBox[] { comboBox7, comboBox8, comboBox9, comboBox10, comboBox11 };
			TextBox[] _tboxes = new TextBox[] { textBox14, textBox15, textBox16, textBox17, textBox18 };

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

		public void ReLoad(ModelDoc2 md) {

			ActiveDoc = md;
			ReLoad();
		}

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
		public SwProperties PropertySet { get; set; }
		public int ProjectCustomer { get; set; }
		public string RevFromFile { get; set; }
		public string RevFromDrw { get; set; }
		public FileInfo PartFileInfo { get; set; }
		public ModelDoc2 ActiveDoc { get; set; }
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

		private void comboBox14_SelectedIndexChanged(object sender, EventArgs e) {
			if (RevFromFile == null || (sender as ComboBox).Text == RevFromFile) {
				Redbrick.Unwarn(sender as ComboBox);
			} else {
				Redbrick.Warn(sender as ComboBox);
			}
		}

		private void comboBox12_SelectedIndexChanged(object sender, EventArgs e) {
			if ((sender as ComboBox).SelectedItem != null) {
				if ((ProjectCustomer == 0) || (int)(sender as ComboBox).SelectedValue == ProjectCustomer) {
					Redbrick.Unwarn(sender as ComboBox);
				} else {
					Redbrick.Warn(sender as ComboBox);
				}
			} else {
				(sender as ComboBox).Text = PropertySet[@"CUSTOMER"].Value;
				Redbrick.Warn(sender as ComboBox);
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

		}

		private void button4_Click(object sender, EventArgs e) {
			SolidWorks.Interop.sldworks.View _v = Redbrick.GetFirstView(SwApp);
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
				TextBox[] tbs = { textBox18, textBox17, textBox16, textBox15, textBox14 };
				enterJump((sender as TextBox), tbs);
			} else if (e.KeyCode == Keys.Enter) {
				TextBox[] tbs = { textBox14, textBox15, textBox16, textBox17, textBox18 };
				enterJump((sender as TextBox), tbs);
			}
		}

		private void comboBox_KeyUp(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Enter && e.Shift) {
				ComboBox[] cbs = { comboBox11, comboBox10, comboBox9, comboBox8, comboBox7 };
				enterJump((sender as ComboBox), cbs);
			} else if (e.KeyCode == Keys.Enter) {
				ComboBox[] cbs = { comboBox7, comboBox8, comboBox9, comboBox10, comboBox11 };
				enterJump((sender as ComboBox), cbs);
			}
		}
	}
}
