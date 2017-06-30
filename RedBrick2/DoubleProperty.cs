using System;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
  public class DoubleProperty : SwProperty {
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
      _data = res;
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
