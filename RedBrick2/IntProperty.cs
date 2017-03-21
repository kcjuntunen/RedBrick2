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
      int res;
      if (!int.TryParse(Value, out res)) {
        res = 0;
      }
      _data = res;
      return this;
    }

    public override void Set(object data, string val) {
      Value = val;
      if (data is int) {
        _data = (int)data;
      } else {
        int res = 0;
        try {
          res = int.Parse(data.ToString());
        } catch (Exception) {
          //
        }
        _data = res;
      }
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
