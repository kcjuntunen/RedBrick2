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
    private bool AssemblyEventsAssigned = false;
    private bool PartEventsAssigned = false;
    private SelectionMgr swSelMgr;
    private Component2 swSelComp;
    private AssemblyDoc ad;
    private PartDoc pd;

    private ModelDoc2 lastModelDoc = null;

    public ModelRedbrick(SldWorks sw, ModelDoc2 md) {
      SwApp = sw;
      InitializeComponent();
      ActiveDoc = md;
      tabPage1.Text = @"Model Properties";
      tabPage2.Text = @"DrawingProperties";
      groupBox1.MouseClick += groupBox1_MouseClick;
      label6.MouseDown += clip_click;
      label7.MouseDown += clip_click;
      label8.MouseDown += clip_click;
      label9.MouseDown += clip_click;
      label10.MouseDown += clip_click;

      textBox9.TextChanged += textBox9_TextChanged;
      textBox10.TextChanged += textBox10_TextChanged;
    }

    void groupBox1_MouseClick(object sender, MouseEventArgs e) {
      Redbrick.Clip((sender as GroupBox).Text.Split(new string [] {@" - "},StringSplitOptions.RemoveEmptyEntries)[0].Trim());
    }

    void textBox10_TextChanged(object sender, EventArgs e) {
      try {
        textBox13.Text = (double.Parse(label19.Text) +
          double.Parse((sender as TextBox).Text)).ToString();
      } catch (Exception) {
        textBox13.Text = @"#VALUE!";
      }
    }

    void textBox9_TextChanged(object sender, EventArgs e) {
      try {
        textBox12.Text = (double.Parse(label18.Text) +
          double.Parse((sender as TextBox).Text)).ToString();
      } catch (Exception) {
        textBox12.Text = @"#VALUE!";
      }
    }

    public void ReQuery(ModelDoc2 md) {
      ActiveDoc = md;
    }

    private void ReQuery() {
      if (ActiveDoc != null) {
        textBox2.Text = PropertySet.GetProperty(@"LENGTH").Value;
        textBox3.Text = PropertySet.GetProperty(@"WIDTH").Value;
        textBox4.Text = PropertySet.GetProperty(@"THICKNESS").Value;
        textBox5.Text = PropertySet.GetProperty(@"WALL THICKNESS").Value;

        GetCutlistData();
        //DisconnectEvents();
        SelectTab();

      } else {
        Enabled = false;
      }
    }

    private void GetCutlistData() {
      string partLookup = Path.GetFileNameWithoutExtension(PartFileInfo.Name).Split(' ')[0];

      cutlistPartsTableAdapter.FillByPartNum(eNGINEERINGDataSet.CutlistParts, partLookup);
      cutlistPartsBindingSource.DataSource = cutlistPartsTableAdapter.GetDataByPartNum(partLookup);
      cUTPARTSBindingSource.DataSource = cUT_PARTSTableAdapter.GetDataByPartnum(partLookup);
    }

    private void SelectTab() {
      swDocumentTypes_e overdoctype = swDocumentTypes_e.swDocNONE;
      swDocumentTypes_e doctype = swDocumentTypes_e.swDocNONE;
      GetTypes(ref doctype, ref overdoctype);
      switch (overdoctype) {
        case swDocumentTypes_e.swDocASSEMBLY:
          Enabled = true;
          ((Control)tabPage1).Enabled = true;
          ((Control)tabPage2).Enabled = false;
          tabControl1.SelectedTab = tabPage1;
          ConnectAssemblyEvents();
          switch (doctype) {
            case swDocumentTypes_e.swDocASSEMBLY:
              ConnectPartEvents();
              break;
            case swDocumentTypes_e.swDocPART:
              ConnectPartEvents();
              break;
            default:
              break;
          }
          break;
        case swDocumentTypes_e.swDocDRAWING:
          Enabled = true;
          ((Control)tabPage1).Enabled = false;
          ((Control)tabPage2).Enabled = true;
          tabControl1.SelectedTab = tabPage2;
          break;
        case swDocumentTypes_e.swDocLAYOUT:
          break;
        case swDocumentTypes_e.swDocNONE:
          break;
        case swDocumentTypes_e.swDocPART:
          Enabled = true;
          ((Control)tabPage1).Enabled = true;
          ((Control)tabPage2).Enabled = false;
          tabControl1.SelectedTab = tabPage1;
          break;
        case swDocumentTypes_e.swDocSDM:
          break;
        default:
          Enabled = false;
          break;
      }
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
        lastModelDoc = SwApp.ActiveDoc;
      }
      DisconnectPartEvents();
    }

    private void ConnectAssemblyEvents() {
      if ((ActiveDoc.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY) && !AssemblyEventsAssigned) {
        ad = (AssemblyDoc)ActiveDoc;
        swSelMgr = ActiveDoc.SelectionManager;
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
      return 0;
    }

    private int ad_UserSelectionPostNotify() {
      // What do we got?
      if (swSelMgr == null) {
        swSelMgr = ActiveDoc.SelectionManager;
        swSelComp = swSelMgr.GetSelectedObjectsComponent4(1, -1);
      }
      if (swSelComp != null) {
        ActiveDoc = swSelComp.GetModelDoc2();
      } else {
        // Nothing's selected?
        // Just look at the root item then.
        ActiveDoc = SwApp.ActiveDoc;
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
      return 0;
    }

    private int pd_FileSavePostNotify(int saveType, string FileName) {
      ActiveDoc = SwApp.ActiveDoc;
      return 0;
    }

    private int pd_ActiveConfigChangePostNotify() {
      ActiveDoc = SwApp.ActiveDoc;
      return 0;
    }

    /// <summary>
    /// Run this function after this.Document is populated. It fills two ref vars with swDocumentTypes_e.
    /// </summary>
    /// <param name="d">The document type.</param>
    /// <param name="od">The top-level document type.</param>
    private void GetTypes(ref swDocumentTypes_e d, ref swDocumentTypes_e od) {
      swDocumentTypes_e docT = (swDocumentTypes_e)ActiveDoc.GetType();
      ModelDoc2 overDoc = (ModelDoc2)SwApp.ActiveDoc;
      swDocumentTypes_e overDocT = (swDocumentTypes_e)overDoc.GetType();
      swSelMgr = ActiveDoc.SelectionManager;
      if ((docT != swDocumentTypes_e.swDocDRAWING && swSelMgr != null) && swSelMgr.GetSelectedObjectCount2(-1) > 0) {
        Component2 comp = (Component2)swSelMgr.GetSelectedObjectsComponent4(1, -1);
        if (comp != null) {
          ModelDoc2 cmd = (ModelDoc2)comp.GetModelDoc2();
          docT = (swDocumentTypes_e)cmd.GetType();
          ActiveDoc = cmd;
        } else {

        }
      } else {
        swSelMgr = null;
      }
      d = docT;
      od = overDocT;
    }

    public int PartID { get; set; }
    public int Hash { get; set; }
    public FileInfo PartFileInfo { get; set; }

    private ModelDoc2 _activeDoc;

    public ModelDoc2 ActiveDoc {
      get { return _activeDoc; }
      set {
        _activeDoc = value;
        if (PropertySet == null) {
          PropertySet = new SwProperties(SwApp);
        } else {
          PropertySet.Clear();
        }
        PropertySet.GetProperties(_activeDoc);
        PartFileInfo = new FileInfo(_activeDoc.GetPathName());

        groupBox1.Text = string.Format(@"{0} - {1}",
          Path.GetFileNameWithoutExtension(PartFileInfo.Name).Split(' ')[0],
          ActiveDoc.ConfigurationManager.ActiveConfiguration.Name);

        Hash = Redbrick.GetHash(PartFileInfo.FullName);
        ReQuery();
      } 
    }

    public SldWorks SwApp { get; set; }

    private void ModelRedbrick_Load(object sender, EventArgs e) {
      cUT_MATERIALSTableAdapter.Fill(eNGINEERINGDataSet.CUT_MATERIALS);
      cUT_EDGESTableAdapter.Fill(eNGINEERINGDataSet.CUT_EDGES);
      GetCutlistData();
      SelectTab();
    }

    private void comboBox_KeyDown(object sender, KeyEventArgs e) {
      (sender as ComboBox).DroppedDown = false;
    }

    private void comboBox_TextChanged(object sender, EventArgs e) {
      if ((sender as ComboBox).Text == string.Empty) {
        (sender as ComboBox).SelectedIndex = -1;
      }
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
      EquationMgr eqm = ActiveDoc.GetEquationMgr();
      for (int i = 0; i < eqm.GetCount(); i++) {
        if (eqm.get_Equation(i).Contains(equation)) {
          return eqm.get_Value(i).ToString();
        }
      }
      return @"#VALUE!";
    }

    private void textBox_TextChanged(string dim, Label l) {
      try {
        string dimension = dim.
          Replace(@"@" + PartFileInfo.Name, string.Empty).
          Replace(@"@" + ActiveDoc.ConfigurationManager.ActiveConfiguration.Name, string.Empty).
          Trim('"');

        l.Text = string.Format("{0:0,0.000}", GetDim(dimension));
      } catch (Exception) {
        l.Text = @"#VALUE!";
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
      Redbrick.Clip(comboBox1.Text);
    }

    private void label2_Click(object sender, EventArgs e) {
      Redbrick.Clip(comboBox2.Text);
    }

    private void label3_Click(object sender, EventArgs e) {
      Redbrick.Clip(comboBox3.Text);
    }

    private void label4_Click(object sender, EventArgs e) {
      Redbrick.Clip(comboBox4.Text);
    }

    private void label5_Click(object sender, EventArgs e) {
      Redbrick.Clip(comboBox5.Text);
    }

    private void label11_Click(object sender, EventArgs e) {
      Redbrick.Clip(comboBox6.Text);
    }

    private void label12_Click(object sender, EventArgs e) {
      Redbrick.Clip(textBox1.Text);
    }

    private void button1_Click(object sender, EventArgs e) {
      Machine_Priority_Control.MachinePriority mp =
        new Machine_Priority_Control.MachinePriority(Path.GetFileNameWithoutExtension(PartFileInfo.Name));
      mp.Show(this);
    }

  }
}
