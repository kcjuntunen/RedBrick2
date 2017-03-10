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
      res = double.Parse(ResolvedValue);
      Data = res;
      return this;
    }

    new public double Data {
      get { return Data; }
      set {
        Data = value;
        Value = value.ToString();
      }
    }
  }
}
