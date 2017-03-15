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

    protected int _data = 0;

    public override object Data {
      get { return _data; }
      set {
        ENGINEERINGDataSetTableAdapters.CUT_EDGESTableAdapter ceta =
          new ENGINEERINGDataSetTableAdapters.CUT_EDGESTableAdapter();
        try {
          _data = int.Parse(value.ToString());
          Value = (string)ceta.GetDataByEdgeID(_data).Rows[0][@"DESCR"];
        } catch (Exception) {
          _data = 0;
          Value = @"0";
        }
      }
    }
  }
}
