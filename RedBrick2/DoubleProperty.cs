using System;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
  class DoubleProperty : SwProperty {
    public DoubleProperty(string name, bool global, SldWorks sw, ModelDoc2 md, string tableName, string fieldName)
      : base(name, global, sw, md) {
      SWType = swCustomInfoType_e.swCustomInfoDouble;
      TableName = tableName;
      FieldName = fieldName;
    }

    public override SwProperty Get() {
      InnerGet();
      double res = 0.0F;
      res = double.Parse(ResolvedValue);
      Data = res;
      return this;
    }

    public double Data { get; set; }
  }
}
