using System;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
  class MaterialProperty : SwProperty {
    public MaterialProperty(string name, bool global, SldWorks sw, ModelDoc2 md, string fieldName)
      : base(name, global, sw, md) {
      SWType = swCustomInfoType_e.swCustomInfoNumber;
      TableName = @"CUT_CUTLIST_PARTS";
      FieldName = fieldName;
    }

    protected int _data = 0;

    public override object Data {
      get { return _data; }
      set {
        ENGINEERINGDataSetTableAdapters.CUT_MATERIALSTableAdapter cmta =
          new ENGINEERINGDataSetTableAdapters.CUT_MATERIALSTableAdapter();
        try {
          _data = int.Parse(value.ToString());
          Value = (string)cmta.GetDataByMatID(_data).Rows[0][@"DESCR"];
        } catch (Exception) {
          _data = Properties.Settings.Default.DefaultMaterial;
          Value = Properties.Settings.Default.DefaultMaterial.ToString();
        }
      }
    }

  }
}
