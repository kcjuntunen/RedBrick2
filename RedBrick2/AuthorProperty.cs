using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
  class AuthorProperty : StringProperty {
    ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter gu =
      new ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter();

    public AuthorProperty(string name, bool global, SldWorks sw, ModelDoc2 md)
      : base(name, global, sw, md, string.Empty) {
  
    }

    public override SwProperty Get() {
      InnerGet();
      string searchTerm = ResolvedValue.Split('-')[0].Trim();

      ENGINEERINGDataSet.GEN_USERSRow row = null;
      try {
        row = gu.GetDataByInitial(Value)[0];
      } catch (Exception) {
        //
      }

      if (row != null) {
        _data = (int)row[@"UID"];
      }

      return this;
    }

    protected int _data;

    public override object Data {
      get { return _data; }
      set {
        _data = (int)value;
        ENGINEERINGDataSet.GEN_USERSRow row = null;
        try {
          row = gu.GetDataByUID(_data)[0];
        } catch (Exception) {
          //
        }
        if (row != null) {
          Value = (string)row[@"INITIAL"];
        }
      }
    }

  }
}
