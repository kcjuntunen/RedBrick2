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

      if (_swApp.ActiveDoc is DrawingDoc) {
        SolidWorks.Interop.sldworks.View _v = Redbrick.GetFirstView(_swApp);
        m = _v.ReferencedDocument;
      }
      swConfMgr = (ConfigurationManager)m.ConfigurationManager;
      swConf = (Configuration)swConfMgr.ActiveConfiguration;
      swRootComp = (Component2)swConf.GetRootComponent();

      PartFileInfo = new FileInfo(m.GetPathName());
      string _pnwe = Path.GetFileNameWithoutExtension(PartFileInfo.Name);
      partLookup = _pnwe.Split(new string[] { @" " }, StringSplitOptions.RemoveEmptyEntries)[0];

      //TraverseModelFeatures(m, 1);
      _swApp.GetUserProgressBar(out pb);
      if (m is AssemblyDoc) {
        TraverseComponent(swRootComp, 1);
      }

      if (m is PartDoc) {
        GetPart(m);
      }
      //dataGridView1.DataSource = ToDataTable(_dict, _partlist);
      pb.End();
      AddColumns();
      FillTable(_dict, _partlist);
      toolStripStatusLabel2.Text = string.Format("Included Parts: {0}", count_includes());
      toolStripStatusLabel1.Text = string.Format("Total Unique Parts: {0}", dataGridView1.Rows.Count - 1);
    }

    private void CreateCutlist_Load(object sender, EventArgs e) {
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
        warn(comboBox3);
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
          SwProperties s = new SwProperties(_swApp);
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

      DataGridViewColumn blank_qty = new DataGridViewColumn();
      blank_qty.Name = @"Blank Qty";
      blank_qty.CellTemplate = new DataGridViewTextBoxCell();
      blank_qty.ValueType = typeof(int);
      blank_qty.SortMode = DataGridViewColumnSortMode.Programmatic;

      DataGridViewComboBoxColumn col = new DataGridViewComboBoxColumn();
      col.DataSource = cpt.GetData();
      col.Name = @"Department";
      col.CellTemplate = new DataGridViewComboBoxCell();
      col.HeaderText = @"Department";
      col.DropDownWidth = 200;
      col.DisplayMember = @"TYPEDESC";
      col.ValueMember = @"TYPEID";
      col.AutoComplete = true;
      col.DataSource = cpt.GetData();
      col.SortMode = DataGridViewColumnSortMode.Programmatic;

      DataGridViewCheckBoxColumn inc = new DataGridViewCheckBoxColumn();
      inc.Name = @"Include";
      inc.CellTemplate = new DataGridViewCheckBoxCell();
      inc.HeaderText = @"Include";
      inc.SortMode = DataGridViewColumnSortMode.Programmatic;

      foreach (var item in new object[] { part_number, descr, blank_qty, col, inc }) {
        dataGridView1.Columns.Add((DataGridViewColumn)item);
      }
    }

    private void FillTable(SortedDictionary<string, int> pl, SortedDictionary<string, SwProperties> sp) {
      ENGINEERINGDataSetTableAdapters.CUT_PART_TYPESTableAdapter cpt =
        new ENGINEERINGDataSetTableAdapters.CUT_PART_TYPESTableAdapter();
      System.Text.RegularExpressions.Regex r =
        new System.Text.RegularExpressions.Regex(Redbrick.BOMFilter[0]);
      foreach (KeyValuePair<string, int> item in pl) {
        SwProperties val = sp[item.Key];
        int qty = (int)val[@"BLANK QTY"].Data;
        string name = item.Key;
        val[@"BLANK QTY"].Data = qty;
        DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
        row.Cells[0].Value = name;
        row.Cells[1].Value = val[@"Description"].Value;
        row.Cells[2].Value = item.Value;
        if ((int)val[@"DEPARTMENT"].Data > 0 && (int)val[@"DEPARTMENT"].Data <= (int)cpt.TypeCount()) {
          row.Cells[3].Value = val[@"DEPARTMENT"].Data;
        }
        row.Cells[4].Value = r.IsMatch(name);
        dataGridView1.Rows.Add(row);
        //dataGridView1.Rows.Add(name, val[@"Description"].Value, item.Value, val[@"DEPARTMENT"].Data, r.IsMatch(name));
      }
    }

    private void warn(Control c) {
      c.ForeColor = Properties.Settings.Default.WarnForeground;
      c.BackColor = Properties.Settings.Default.WarnBackground;
    }

    private void unwarn(Control c) {
      c.ForeColor = Properties.Settings.Default.NormalForeground;
      c.BackColor = Properties.Settings.Default.NormalBackground;
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
        unwarn((Control)sender);
      }
    }

    private void comboBox2_KeyDown(object sender, KeyEventArgs e) {
      user_changed_item = true;
      unwarn((Control)sender);
    }

    private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
    }

    private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) {
      string name = dataGridView1.Columns[e.ColumnIndex].Name;
      if (name == @"Include") {
        int add = (bool)(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewCheckBoxCell).Value ? -1 : 1;
        toolStripStatusLabel2.Text = string.Format("Included Parts: {0}", count_includes(add));
      }
    }

    private void comboBox3_SelectedValueChanged(object sender, EventArgs e) {
      if (rev_changed_by_user) {
        ComboBox _cb = sender as ComboBox;
        if (_revFromFile == string.Empty || _cb.Text == _revFromFile) {
          unwarn(_cb);
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

    //public object DataSource {
    //  get {
    //    return dataGridView1.DataSource;
    //  }

    //  set {
    //    dataGridView1.DataSource = value;
    //  }
    //}
  }
}