using System;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
  class BooleanProperty : SwProperty {
    public BooleanProperty(string name, bool global, SldWorks sw, ModelDoc2 md, string fieldName)
      : base(name, global, sw, md) {
        SWType = swCustomInfoType_e.swCustomInfoYesOrNo;
        TableName = @"CUT_PARTS";
        FieldName = fieldName;
    }

    public override SwProperty Get() {
      InnerGet();
      bool res = Value.ToUpper().Contains(@"Y");
      Data = res;
      return this;
    }

    protected bool _data = false;

    public override object Data {
      get { return _data; }
      set {
        _data = value.ToString().ToUpper().Contains(@"Y");
        Value = _data ? @"Yes" : @"NO";
      }
    }
  }
}
