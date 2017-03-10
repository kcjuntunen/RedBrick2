using System;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
  class EdgeProperty : SwProperty {
    public EdgeProperty(string name, bool global, SldWorks sw, ModelDoc2 md, string fieldName)
      : base(name, global, sw, md) {
      SWType = swCustomInfoType_e.swCustomInfoNumber;
      TableName = @"CUT_CUTLIST_PARTS";
      FieldName = fieldName;
    }

    new public int Data {
      get { return Data; }
      set {
        ENGINEERINGDataSetTableAdapters.CUT_EDGESTableAdapter ceta =
          new ENGINEERINGDataSetTableAdapters.CUT_EDGESTableAdapter();
        Data = value;
        Value = (string)ceta.GetDataByEdgeID(Data).Rows[0][@"DESCR"];
      }
    }
  }
}
