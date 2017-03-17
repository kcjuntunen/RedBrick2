using System;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
  class MaterialProperty : IntProperty {
    public MaterialProperty(string name, bool global, SldWorks sw, ModelDoc2 md, string fieldName)
      : base(name, global, sw, md, @"CUT_CUTLIST_PARTS", fieldName) {
      SWType = swCustomInfoType_e.swCustomInfoNumber;
    }

    public override SwProperty Get() {
      SwProperty p = base.Get();
      if (_data == 0) {
        Data = Value;
      }

      return base.Get();
    }

    protected int _data = 0;

    public override object Data {
      get { return _data; }
      set {
        ENGINEERINGDataSetTableAdapters.CUT_MATERIALSTableAdapter cmta =
          new ENGINEERINGDataSetTableAdapters.CUT_MATERIALSTableAdapter();
        try {
          _data = (int)cmta.GetMaterialID(value.ToString());
          Value = value.ToString();
        } catch (Exception) {
          _data = Properties.Settings.Default.DefaultMaterial;
        }
      }
    }

  }
}
