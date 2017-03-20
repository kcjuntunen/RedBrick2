using System;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
  class DimensionProperty : SwProperty {
    public DimensionProperty(string name, bool global, SldWorks sw, ModelDoc2 md, string fieldName)
      : base(name, global, sw, md) {
      SWType = swCustomInfoType_e.swCustomInfoDouble;
      TableName = @"CUT_PARTS";
      FieldName = fieldName;
    }

    public override SwProperty Get() {
      InnerGet();
      double res = 0.0F;
      if (!double.TryParse(ResolvedValue, out res)) {
        _data = res;
      }

      return this;
    }

    protected double _data = 0.0F;

    public override object Data {
      get {
        return _data == null ? double.Parse(ResolvedValue) : _data;
      }
      set {
        _data = double.Parse(value.ToString());
        Value = value.ToString();
      }
    }
  }
}
