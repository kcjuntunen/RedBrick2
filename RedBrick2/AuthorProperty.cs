using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
  public class AuthorProperty : StringProperty {
    ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter gu =
      new ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter();

    public AuthorProperty(string name, bool global, SldWorks sw, ModelDoc2 md)
      : base(name, global, sw, md, string.Empty) {
  
    }

    public override SwProperty Get() {
      InnerGet();

      ENGINEERINGDataSet.GEN_USERSDataTable dt = null;
      ENGINEERINGDataSet.GEN_USERSRow row = null;
      dt = gu.GetDataByInitial(Value);

      if (dt.Rows.Count > 0) {
        row = gu.GetDataByInitial(Value)[0];
      }

      if (row != null) {
        _data = (int)row[@"UID"];
        FullName = (string)row[@"Fullname"];
      }

      return this;
    }

    public string FullName {
      get { return Value; }
      private set { Value = value; }
    }

    public string Initials {
      get { return Value; }
      private set { Value = value; }
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
