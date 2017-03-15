using System;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
  class OpProperty : SwProperty {
    ENGINEERINGDataSetTableAdapters.CUT_OPSTableAdapter cota =
      new ENGINEERINGDataSetTableAdapters.CUT_OPSTableAdapter();

    ENGINEERINGDataSetTableAdapters.CUT_PART_OPSTableAdapter cpota =
      new ENGINEERINGDataSetTableAdapters.CUT_PART_OPSTableAdapter();

    public OpProperty(string name, bool global, SldWorks sw, ModelDoc2 md, string fieldName)
      : base(name, global, sw, md) {
      SWType = swCustomInfoType_e.swCustomInfoYesOrNo;
      TableName = @"CUT_PART_OPS";
      FieldName = fieldName;
    }

    public override SwProperty Get() {
      InnerGet();
      ENGINEERINGDataSet.CUT_PARTSDataTable cpdt = new ENGINEERINGDataSet.CUT_PARTSDataTable();
      cpdt = cpta.GetDataByPartID(PartID);
      int tp = (int)cpdt.Rows[0][@"TYPE"];
      Data = (int)cota.GetDataByOpName(Value, tp).Rows[0][@"OPID"];
      return this;
    }

    protected int _data = 0;

    public override object Data {
      get { return _data; }
      set {
        ENGINEERINGDataSet.CUT_PARTSDataTable cpdt =
          new ENGINEERINGDataSet.CUT_PARTSDataTable();
        cpdt = cpta.GetDataByPartID(PartID);
        int tp = (int)cpdt.Rows[0][@"TYPE"];
        _data = int.Parse(value.ToString());
        Value = (string)cota.GetDataByOpID(int.Parse(value.ToString()), tp).Rows[0][@"OPNAME"];
      }
    }

  }
}
