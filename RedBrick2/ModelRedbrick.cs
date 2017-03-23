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
    private bool initialated = false;
    private bool AssemblyEventsAssigned = false;
    private bool PartEventsAssigned = false;
    private bool DrawingEventsAssigned = false;
    private bool ModelSetup = false;
    private bool DrawingSetup = false;

    private SelectionMgr swSelMgr;
    private Component2 swSelComp;
    private AssemblyDoc ad;
    private PartDoc pd;
    private DrawingDoc dd;

    private DrawingRedbrick drawingRedbrick;

    private string partLookup;
    private Point scrollOffset;

    private const int WM_PAINT = 0x000F;
    private bool allowPaint;

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

    protected override void WndProc(ref Message m) {
      if ((m.Msg != WM_PAINT || (allowPaint && m.Msg == WM_PAINT))) {
        base.WndProc(ref m); 
      }
    }

    void groupBox1_MouseClick(object sender, MouseEventArgs e) {
      Redbrick.Clip(partLookup);
    }

    void textBox10_TextChanged(object sender, EventArgs e) {
      if (initialated) {
        try {
          double _val = (double.Parse(label19.Text) +
             double.Parse((sender as TextBox).Text));
          textBox13.Text = string.Format(Properties.Settings.Default.NumberFormat, _val);
        } catch (Exception) {
          textBox13.Text = @"#VALUE!";
        }
      }
    }

    void textBox9_TextChanged(object sender, EventArgs e) {
      if (initialated) {
        try {
          double _val = (double.Parse(label18.Text) +
            double.Parse((sender as TextBox).Text));
          textBox12.Text = string.Format(Properties.Settings.Default.NumberFormat, _val);
        } catch (Exception) {
          textBox12.Text = @"#VALUE!";
        }
      }
    }

    public void ReQuery(ModelDoc2 md) {
      ActiveDoc = md;
    }

    private void ReQuery() {
      GetCutlistData();
      flowLayoutPanel1.Controls.Clear();
      if (ActiveDoc != null) {
        textBox2.Text = PropertySet[@"LENGTH"].Value;
        textBox3.Text = PropertySet[@"WIDTH"].Value;
        textBox4.Text = PropertySet[@"THICKNESS"].Value;
        textBox5.Text = PropertySet[@"WALL THICKNESS"].Value;

        GetOps();

        //DisconnectEvents();
        //SelectTab();
        textBox_TextChanged(PropertySet[@"LENGTH"].Value, label18);
        textBox_TextChanged(PropertySet[@"WIDTH"].Value, label19);
        textBox_TextChanged(PropertySet[@"THICKNESS"].Value, label20);
        textBox_TextChanged(PropertySet[@"WALL THICKNESS"].Value, label21);

        flowLayoutPanel1.VerticalScroll.Value = scrollOffset.Y;
      } else {
        Enabled = false;
      }
    }

    private void GetCutlistData() {
      cutlistPartsTableAdapter.FillByPartNum(eNGINEERINGDataSet.CutlistParts, partLookup);
      cutlistPartsBindingSource.DataSource = cutlistPartsTableAdapter.GetDataByPartNum(partLookup);
      cUTPARTSBindingSource.DataSource = cUT_PARTSTableAdapter.GetDataByPartnum(partLookup);
    }

    private void GetOps() {
      ENGINEERINGDataSetTableAdapters.CutPartOpsTableAdapter cpo =
        new ENGINEERINGDataSetTableAdapters.CutPartOpsTableAdapter();
      ENGINEERINGDataSet.CutPartOpsDataTable cpodt =
        new ENGINEERINGDataSet.CutPartOpsDataTable();
      cpo.FillBy(cpodt, partLookup);
      foreach (ENGINEERINGDataSet.CutPartOpsRow row in cpodt) {
        OpControl opc = new OpControl(row);
        flowLayoutPanel1.Controls.Add(opc);
        flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        opc.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      }
    }

    private void ConnectDrawingEvents() {
      if (!DrawingEventsAssigned) {
        dd = (DrawingDoc)ActiveDoc;
        swSelMgr = ActiveDoc.SelectionManager;
        dd.NewSelectionNotify += dd_UserSelectionPostNotify;
        //dd.UserSelectionPostNotify += dd_UserSelectionPostNotify;
        DrawingEventsAssigned = true;
      }
    }

    int dd_UserSelectionPostNotify() {
      swSelMgr = ActiveDoc.SelectionManager;
      DrawingComponent dc = swSelMgr.GetSelectedObject6(1, -1);
      if (dc != null) {
        Component2 _c = dc.Component;
        ActiveDoc = _c.GetModelDoc2();
      } else {
        ActiveDoc = SwApp.ActiveDoc;
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
        lastModelDoc = SwApp.ActiveDoc;
      }
      DisconnectPartEvents();
      DisconnectDrawingEvents();
    }

    private void DisconnectDrawingEvents() {
      dd.UserSelectionPostNotify -= dd_UserSelectionPostNotify;
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
      Visible = false;
      return 0;
    }

    private int ad_UserSelectionPostNotify() {
      // What do we got?
      if (swSelMgr == null) {
        swSelMgr = ActiveDoc.SelectionManager;
      }
      swSelComp = swSelMgr.GetSelectedObjectsComponent4(1, -1);
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

    public void Commit() {
      PropertySet[@"Description"].Data = textBox1.Text;

      // Dimensions get special treatment since the mgr stores weird strings, and DB stores doubles.
      PropertySet[@"LENGTH"].Set(label18.Text, textBox2.Text);
      PropertySet[@"WIDTH"].Set(label19.Text, textBox3.Text);
      PropertySet[@"THICKNESS"].Set(label20.Text, textBox4.Text);
      PropertySet[@"WALL THICKNESS"].Set(label21.Text, textBox5.Text);

      PropertySet[@"COMMENT"].Data = textBox6.Text;
      
      PropertySet[@"CNC1"].Data = textBox7.Text;
      PropertySet[@"CNC2"].Data = textBox8.Text;
      PropertySet[@"OVERL"].Data = textBox9.Text;
      PropertySet[@"OVERW"].Data = textBox10.Text;
      PropertySet[@"BLANK QTY"].Data = textBox11.Text;
      PropertySet[@"UPDATE CNC"].Data = checkBox1.Checked;

      PropertySet[@"CUTLIST MATERIAL"].Data = comboBox1.SelectedValue;
      PropertySet[@"EDGE FRONT (L)"].Data = comboBox2.SelectedValue;
      PropertySet[@"EDGE BACK (L)"].Data = comboBox3.SelectedValue;
      PropertySet[@"EDGE LEFT (W)"].Data = comboBox4.SelectedValue;
      PropertySet[@"EDGE RIGHT (W)"].Data = comboBox5.SelectedValue;
      PropertySet.Write();
    }

    private void SetupDrawing() {
      if (!DrawingSetup) {
        drawingRedbrick = new DrawingRedbrick(ActiveDoc, SwApp);
        tabPage2.Controls.Add(drawingRedbrick);
        drawingRedbrick.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
      } else {
        drawingRedbrick.ReLoad(SwApp.ActiveDoc);
      }

      if (!DrawingEventsAssigned) {
        ConnectDrawingEvents();
      }

      tabControl1.SelectedTab = tabPage2;
    }

    private void SetupPart() {
      scrollOffset = new Point(0, flowLayoutPanel1.VerticalScroll.Value);

      if (PropertySet == null) {
        PropertySet = new SwProperties(SwApp);
      } else {
        PropertySet.Clear();
      }

      PropertySet.GetProperties(_activeDoc);
      string _configname = string.Empty;

      if (!(_activeDoc is DrawingDoc)) {
        _configname = ActiveDoc.ConfigurationManager.ActiveConfiguration.Name;
      }

      groupBox1.Text = string.Format(@"{0} - {1}",
        partLookup, _configname);

      Hash = Redbrick.GetHash(PartFileInfo.FullName);
      if (lastModelDoc != ActiveDoc) {
        ReQuery();
      }
      lastModelDoc = ActiveDoc;
      tabControl1.SelectedTab = tabPage1;
      DisconnectPartEvents();
      ConnectPartEvents();
    }

    public int PartID { get; set; }
    public int Hash { get; set; }
    public FileInfo PartFileInfo { get; set; }

    private ModelDoc2 _activeDoc;

    public ModelDoc2 ActiveDoc {
      get { return _activeDoc; }
      set {
        allowPaint = false;
        Visible = true;
        _activeDoc = value;

        PartFileInfo = new FileInfo(_activeDoc.GetPathName());
        partLookup = Path.GetFileNameWithoutExtension(PartFileInfo.Name).Split(' ')[0];

        swDocumentTypes_e dType = (swDocumentTypes_e)_activeDoc.GetType();
        swDocumentTypes_e odType = (swDocumentTypes_e)(SwApp.ActiveDoc as ModelDoc2).GetType();
        switch (dType) {
          case swDocumentTypes_e.swDocASSEMBLY:
            (tabPage1 as Control).Enabled = true;
            if (odType != swDocumentTypes_e.swDocDRAWING) {
              (tabPage2 as Control).Enabled = false;
            } else {
              (tabPage2 as Control).Enabled = true;
            }
            DisconnectAssemblyEvents();
            ConnectAssemblyEvents();
            SetupPart();
            break;
          case swDocumentTypes_e.swDocDRAWING:
            (tabPage2 as Control).Enabled = true;
            SetupDrawing();
            break;
          case swDocumentTypes_e.swDocLAYOUT:
            break;
          case swDocumentTypes_e.swDocNONE:
            break;
          case swDocumentTypes_e.swDocPART:
            (tabPage1 as Control).Enabled = true;
            if (odType != swDocumentTypes_e.swDocDRAWING) {
              (tabPage2 as Control).Enabled = false;
            } else {
              (tabPage2 as Control).Enabled = true;
            }
            SetupPart();
            break;
          case swDocumentTypes_e.swDocSDM:
            break;
          default:
            (tabPage1 as Control).Enabled = false;
            (tabPage2 as Control).Enabled = false;
            break;
        }
        allowPaint = true;
      } 
    }

    public SldWorks SwApp { get; set; }

    private void ModelRedbrick_Load(object sender, EventArgs e) {
      cUT_MATERIALSTableAdapter.Fill(eNGINEERINGDataSet.CUT_MATERIALS);
      cUT_EDGESTableAdapter.Fill(eNGINEERINGDataSet.CUT_EDGES);
      GetCutlistData();
      //SelectTab();
      initialated = true;
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
      if (initialated && ActiveDoc.GetType() != (int)swDocumentTypes_e.swDocDRAWING) {
        string dimension = dim.
          Replace(@"@" + PartFileInfo.Name, string.Empty).
          Replace(@"@" + ActiveDoc.ConfigurationManager.ActiveConfiguration.Name, string.Empty).
          Trim('"');
        double _val;
        if (double.TryParse(GetDim(dimension), out _val)) {
          l.Text = string.Format(Properties.Settings.Default.NumberFormat, _val);
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
