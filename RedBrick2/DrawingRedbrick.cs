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
    private Revs revSet;

    public DrawingRedbrick(ModelDoc2 md, SldWorks sw) {
      ActiveDoc = md;
      SwApp = sw;
      InitializeComponent();
    }

    private void InitData() {
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
      RevFromFile = revcheck.Length > 1 ? revcheck[1] : Properties.Settings.Default.DefaultRev;
      groupBox5.Text = partLookup;
    }

    private void FigureOutCustomer() {
      SwProperty _p = new CustomerProperty(@"CUSTOMER", true, SwApp, ActiveDoc);
      PropertySet.Add(_p.Get());
      comboBox12.SelectedValue = (int)_p.Data;
    }

    private void FigureOutAuthor() {
      SwProperty _p = new AuthorProperty(@"DrawnBy", true, SwApp, ActiveDoc);
      PropertySet.Add(_p.Get());
      comboBox13.SelectedValue = (int)_p.Data;
    }

    private void FigureOutRev() {
      ENGINEERINGDataSetTableAdapters.RevListTableAdapter rl =
        new ENGINEERINGDataSetTableAdapters.RevListTableAdapter();
      comboBox14.DataSource = rl.GetData();
      comboBox14.DisplayMember = @"REV";
      comboBox14.ValueMember = @"REV";
      SwProperty _p = new StringProperty(@"REVISION LEVEL", true, SwApp, ActiveDoc, string.Empty);
      PropertySet.Add(_p.Get());
      RevFromDrw = _p.Value;
      if (RevFromDrw != RevFromFile) {
        comboBox14.Text = RevFromFile;
      } else {
        comboBox14.Text = RevFromDrw;
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
      ENGINEERINGDataSet.CUT_CUTLISTSRow _row = null;
      try {
        _row = cc.GetDataByName(partLookup, RevFromDrw.ToString())[0];
      } catch (Exception) {
        //
      }
      if (_row != null) {
        comboBox15.SelectedValue = _row[@"STATEID"];
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
      BuildTree();
    }

    private void BuildTree() {
      RevSet = new Revs(SwApp);
      foreach (Rev r in RevSet) {
        treeView1.Nodes.Add(r.Node);
      }
    }

    private Revs RevSet {
      get { return revSet; }
      set { revSet = value; }
    }
    public SwProperties PropertySet { get; set; }
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
  }
}
