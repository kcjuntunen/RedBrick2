using System;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
  public class OpProperty : IntProperty {
    ENGINEERINGDataSetTableAdapters.CUT_OPSTableAdapter cota =
      new ENGINEERINGDataSetTableAdapters.CUT_OPSTableAdapter();

    ENGINEERINGDataSetTableAdapters.CUT_PART_OPSTableAdapter cpota =
      new ENGINEERINGDataSetTableAdapters.CUT_PART_OPSTableAdapter();

    public OpProperty(string name, bool global, SldWorks sw, ModelDoc2 md, string fieldName)
      : base(name, global, sw, md, @"CUT_PART_OPS", fieldName) {
      SWType = swCustomInfoType_e.swCustomInfoYesOrNo;
    }

    public override SwProperty Get() {
      InnerGet();
      ENGINEERINGDataSet.CUT_PARTSDataTable cpdt = new ENGINEERINGDataSet.CUT_PARTSDataTable();
      cpdt = cpta.GetDataByPartID(PartID);
      int tp = (int)cpdt.Rows[0][@"TYPE"];
      Data = (int)cota.GetDataByOpName(Value, tp).Rows[0][@"OPID"];
      return this;
    }

    private int _type = 1;

    public int OpType {
      get { return _type;}
      set { _type = value;} 
    }

    protected int _data = 0;

    public override object Data {
      get { return _data; }
      set {
        if (value is string) {
          try {
            ENGINEERINGDataSet.CUT_PARTSDataTable cpdt =
              new ENGINEERINGDataSet.CUT_PARTSDataTable();
            cpdt = cpta.GetDataByPartID(PartID);
            OpType = (int)cpdt.Rows[0][@"TYPE"];
          } catch (Exception) {
          
          }
          _data = (int)cota.GetOpIDByName(value.ToString(), OpType);
        } else {
          try {
            _data = int.Parse(value.ToString());
            Value = cota.GetOpNameByID(_data).ToString();
          } catch (Exception) {

          }
        }
      }
    }

  }
}
