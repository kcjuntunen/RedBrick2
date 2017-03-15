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
      try {
        res = int.Parse(Value);
      } catch (Exception) {
        // Probably an empty string.
      }
      _data = res;
      return this;
    }

    protected int _data = 0;

    public override object Data {
      get {
        return _data == null ? int.Parse(Value) : _data;
      }
      set {
        try {
          _data = int.Parse(value.ToString());
          Value = value.ToString();
        } catch (Exception) {
          _data = 0;
          Value = @"0";
        }
      }
    }
  }
}
