using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
  public class SwProperty {
    private string v;
    private string resolvedV;
    private bool wasResolved;
    protected ENGINEERINGDataSetTableAdapters.CUT_PARTSTableAdapter cpta =
      new ENGINEERINGDataSetTableAdapters.CUT_PARTSTableAdapter();

    public SwProperty(string name, bool global, SldWorks sw, ModelDoc2 md) {
      Name = name;
      SWType = swCustomInfoType_e.swCustomInfoUnknown;
      Global = global;
      SwApp = sw;
      ActiveDoc = md;
      GetPropertyManager();
      GetFileInfo();
      SetDefaults();
    }

    private void SetDefaults() {
      ToDB = TableName != string.Empty && FieldName != string.Empty;
      Old = false;
    }

    private void GetFileInfo() {
      try {
        PartFileInfo = new FileInfo(ActiveDoc.GetPathName());
        PartID = (int)cpta.GetPartIDByPartnum(Path.GetFileNameWithoutExtension(PartFileInfo.Name));
        Hash = Redbrick.GetHash(string.Format(@"{0}\\{1}", PartFileInfo.Directory.FullName, PartFileInfo.Name));
      } catch (Exception) {
        throw;
      }
    }

    private void GetPropertyManager() {
      CustomPropertyManager _g = ActiveDoc.Extension.get_CustomPropertyManager(string.Empty);
      Configuration _c = (Configuration)ActiveDoc.ConfigurationManager.ActiveConfiguration;
      CustomPropertyManager _s = ActiveDoc.Extension.get_CustomPropertyManager(_c.Name);
      if (Global) {
        PropertyManager = _g;
      } else {
        PropertyManager = _s;
      }
    }

    public void InnerGet() {
      CustomPropertyManager _g = ActiveDoc.Extension.get_CustomPropertyManager(string.Empty);
      Configuration _c = (Configuration)ActiveDoc.ConfigurationManager.ActiveConfiguration;
      CustomPropertyManager _s = ActiveDoc.Extension.get_CustomPropertyManager(_c.Name);
      GetResult = (swCustomInfoGetResult_e)_g.Get5(Name, false, out v, out resolvedV, out wasResolved);
      if (v == string.Empty) {
        GetResult = (swCustomInfoGetResult_e)_s.Get5(Name, false, out v, out resolvedV, out wasResolved);
      }
    }

    public virtual SwProperty Get() {
      InnerGet();
      Data = Value;
      return this;
    }

    public virtual void Write() {
      WriteResult = 
        (swCustomInfoAddResult_e)PropertyManager.Add3(Name, 
        (int)SWType, 
        Value, 
        (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
    }

    public bool ToDB { get; set; }

    public bool Old { get; set; }

    public FileInfo PartFileInfo { get; set; }

    public int Hash { get; set; }

    public int PartID { get; set; }

    public bool WasResolved { get; set; }

    public swCustomInfoAddResult_e WriteResult { get; set; }

    public swCustomInfoGetResult_e GetResult { get; set; }

    public string Name { get; set; }

    public string Value {
      get { return v; }
      set { v = value; }
    }

    public string ResolvedValue {
      get { return resolvedV; }
      set { resolvedV = value; }
    }

    public bool Global { get; set; }

    public swCustomInfoType_e SWType { get; set; }

    public string TableName { get; protected set; }

    public string FieldName { get; protected set; }

    public object Data { get; private set; }

    public Configuration Config { get; set; }

    public CustomPropertyManager PropertyManager { get; set; }

    public ModelDoc2 ActiveDoc {
      get { return ActiveDoc; } 
      protected set { ActiveDoc = value; }
    }

    public  SldWorks SwApp {
      get { return SwApp;  } 
      protected set { SwApp = value; }
    }

  }
}
