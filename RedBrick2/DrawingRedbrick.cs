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
    Dictionary<string, string> translation = new Dictionary<string,string>();
    Dictionary<string, Format> action = new Dictionary<string,Format>();

    enum Format {
      NAME,
      STRING,
      DATE,
      SKIP
    }

    public DrawingRedbrick(ModelDoc2 md, SldWorks sw) {
      ActiveDoc = md;
      SwApp = sw;
      InitializeComponent();
      SetupTranslationAndActionTables();
    }

    private void SetupTranslationAndActionTables() {
      translation.Add(@"LGCYID", @"Legacy ID");
      translation.Add(@"DateRequested", @"Date Requested");
      translation.Add(@"DateStarted", @"Date Started");
      translation.Add(@"DateCompleted", @"Date Completed");
      translation.Add(@"AffectedParts", @"Affected Parts");
      translation.Add(@"Change", @"Change");
      translation.Add(@"Engineer", @"Engineer");
      translation.Add(@"Holder", @"Holder");
      translation.Add(@"ECR_NUM", @"ECR Number");
      translation.Add(@"ReqBy", @"Requested By");
      translation.Add(@"CHANGES", @"Change");
      translation.Add(@"STATUS", @"Status");
      translation.Add(@"ERR_DESC", @"Error Description");
      translation.Add(@"REVISION", @"Revision");
      translation.Add(@"DATE_CREATE", @"Date Created");


      action.Add(@"LGCYID", Format.SKIP);
      action.Add(@"DateRequested", Format.DATE);
      action.Add(@"DateStarted", Format.DATE);
      action.Add(@"DateCompleted", Format.DATE);
      action.Add(@"AffectedParts", Format.STRING);
      action.Add(@"Change", Format.STRING);
      action.Add(@"Engineer", Format.NAME);
      action.Add(@"Holder", Format.STRING);
      action.Add(@"ECR_NUM", Format.SKIP);
      action.Add(@"ReqBy", Format.NAME);
      action.Add(@"CHANGES", Format.STRING);
      action.Add(@"STATUS", Format.STRING);
      action.Add(@"ERR_DESC", Format.STRING);
      action.Add(@"REVISION", Format.STRING);
      action.Add(@"DATE_CREATE", Format.DATE);
    }

    private void InitData() {
      PartFileInfo = new FileInfo(ActiveDoc.GetPathName());
      string[] fi = Path.GetFileNameWithoutExtension(PartFileInfo.Name).Split(' ');
      partLookup = fi[0];
      string[] revcheck = fi[0].Split(new string[] { @"REV" }, StringSplitOptions.RemoveEmptyEntries);
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
      SwProperty _p = new StringProperty(@"REVISION LEVEL", true, SwApp, ActiveDoc, string.Empty);
      PropertySet.Add(_p.Get());
      RevFromDrw = _p.Value;
      if (RevFromDrw != RevFromFile) {
        System.Windows.Forms.MessageBox.Show(@"AAAAAAA!");
      }
      comboBox14.Text = RevFromDrw;
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
      treeView1.Nodes.Clear();
      RevSet = new Revs(SwApp);
      foreach (Rev r in RevSet) {
        TreeNode topNode = new TreeNode(r.Level);
        TreeNode ecoNode = new TreeNode(string.Format(@"ECR #: {0}", r.ECO));
        TreeNode lNode = new TreeNode(string.Format(@"By: {0}", r.AuthorFullName));
        TreeNode dNode = new TreeNode(string.Format(@"Date: {0}", r.Date.ToShortDateString()));
        foreach (KeyValuePair<string, string> kvp in r.ecoData) {
          switch (action[kvp.Key]) {
            case Format.NAME:
              ecoNode.Nodes.Add(
                new TreeNode(string.Format(@"{0}: {1}", translation[kvp.Key], Redbrick.TitleCase(kvp.Value))));
              break;
            case Format.STRING:
              if (kvp.Value.Contains("\n")) {
                TreeNode subNode = new TreeNode(translation[kvp.Key]);
                foreach (string subs in kvp.Value.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries)) {
                  TreeNode subsubNode = new TreeNode(subs);
                  subNode.Nodes.Add(subsubNode);
                }
                ecoNode.Nodes.Add(subNode);
              } else {
                ecoNode.Nodes.Add(
                  new TreeNode(string.Format(@"{0}: {1}", translation[kvp.Key], kvp.Value)));
              }
              break;
            case Format.DATE:
              DateTime _dt = new DateTime();
              if (DateTime.TryParse(kvp.Value, out _dt)) {
                ecoNode.Nodes.Add(
                  new TreeNode(string.Format(@"{0}: {1}", translation[kvp.Key], _dt.ToShortDateString())));
              }
              break;
            case Format.SKIP:
              continue;
            default:
              break;
            //TreeNode subNode = new TreeNode(string.Format(@"{0}: {1}", Redbrick.TitleCase(kvp.Key), kvp.Value));
            //ecoNode.Nodes.Add(subNode);
          }
        }
        topNode.Nodes.AddRange(new TreeNode[] { ecoNode, lNode, dNode });
        treeView1.Nodes.Add(topNode);
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

  }
}
