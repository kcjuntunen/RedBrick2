using System;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
  public class EdgeProperty : IntProperty {
    public EdgeProperty(string name, bool global, SldWorks sw, ModelDoc2 md, string fieldName)
      : base(name, global, sw, md, @"CUT_CUTLIST_PARTS", fieldName) {
      SWType = swCustomInfoType_e.swCustomInfoText;
    }

    protected int _data = 0;

    public override object Data {
      get { return _data; }
      set {
        if (value is string) {
          ENGINEERINGDataSetTableAdapters.CUT_EDGES_XREFTableAdapter cex =
            new ENGINEERINGDataSetTableAdapters.CUT_EDGES_XREFTableAdapter();
          try {
            _data = (int)cex.GetEdgeID(value.ToString());
          } catch (Exception) {
            _data = 0;
          }
        } else {
          ENGINEERINGDataSetTableAdapters.CUT_EDGESTableAdapter ce =
            new ENGINEERINGDataSetTableAdapters.CUT_EDGESTableAdapter();
          try {
            int res;
            if (!int.TryParse(value.ToString(), out res)) {
              res = 0;
            }
            _data = res;
            Value = (string)ce.GetEdgeDescrByID(res);
          } catch (Exception) {
            _data = 0;
          }
        }
      }
    }

  }
}
