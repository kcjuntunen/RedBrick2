using System;
using System.Collections.Generic;
using System.Text;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
  public class DepartmentProperty : IntProperty {
    public DepartmentProperty(string name, bool global, SldWorks sw, ModelDoc2 md, string fieldName)
      : base(name, global, sw, md, @"CUT_PARTS", fieldName) {

    }

    public override SwProperty Get() {
      InnerGet();
      ENGINEERINGDataSetTableAdapters.CUT_PART_TYPESTableAdapter cpt =
        new ENGINEERINGDataSetTableAdapters.CUT_PART_TYPESTableAdapter();
      int? _id = cpt.GetIDByDescr(Value);
      if (_id != null) {
        _data = (int)_id;
      } else {
        _data = 1;
      }
      return this;
    }

    protected int _data = 0;

    public override object Data {
      get { return _data; }
      set {
        ENGINEERINGDataSetTableAdapters.CUT_PART_TYPESTableAdapter cpt =
          new ENGINEERINGDataSetTableAdapters.CUT_PART_TYPESTableAdapter();
        if (value is string) {
            _data =  (int)cpt.GetIDByDescr(value.ToString());
            Value = value.ToString();
          } else {
            try {
              _data = int.Parse(value.ToString());
              Value = (string)cpt.GetDescrByID(_data);
            } catch (Exception) {
              _data = Properties.Settings.Default.DefaultMaterial;
            }
          }
      }
    }

  }
}
