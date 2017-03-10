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

    new public int Data {
      get { return Data; }
      set {
        ENGINEERINGDataSetTableAdapters.CUT_MATERIALSTableAdapter cmta =
          new ENGINEERINGDataSetTableAdapters.CUT_MATERIALSTableAdapter();
        Data = value;
        Value = (string)cmta.GetDataByMatID(Data).Rows[0][@"DESCR"];
      }
    }

  }
}
