using System;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
  class StringProperty : SwProperty {
    public StringProperty(string name, bool global, SldWorks sw, ModelDoc2 md, string fieldName)
      : base(name, global, sw, md) {
      SWType = swCustomInfoType_e.swCustomInfoText;
      TableName = @"CUT_PARTS";
      FieldName = fieldName;
    }

    public override SwProperty Get() {
      return base.Get();
    }

    new public string Data {
      get { return Data; }
      set {
        Data = value;
        Value = value;
      }
    }
  }
}
