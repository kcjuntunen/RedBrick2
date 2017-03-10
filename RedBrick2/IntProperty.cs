using System;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
  class IntProperty : SwProperty {
    public IntProperty(string name, bool global, SldWorks sw, ModelDoc2 md, string tableName, string fieldName)
      : base(name, global, sw, md) {
      SWType = swCustomInfoType_e.swCustomInfoYesOrNo;
      TableName = tableName;
      FieldName = fieldName;
    }

    public override SwProperty Get() {
      InnerGet();
      int res = 0;
      res = int.Parse(Value);
      Data = res;
      return this;
    }

    new public int Data {
      get { return Data; }
      set {
        Data = value;
        Value = value.ToString();
      }
    }
  }
}
