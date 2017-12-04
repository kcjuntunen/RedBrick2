using System;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
	public class OpProperty : IntProperty {
		ENGINEERINGDataSetTableAdapters.CUT_OPSTableAdapter cota =
			new ENGINEERINGDataSetTableAdapters.CUT_OPSTableAdapter();

		//ENGINEERINGDataSetTableAdapters.CUT_PART_OPSTableAdapter cpota =
		//	new ENGINEERINGDataSetTableAdapters.CUT_PART_OPSTableAdapter();

		public OpProperty(string name, bool global, SldWorks sw, ModelDoc2 md, string fieldName)
			: base(name, global, sw, md, @"CUT_PART_OPS", fieldName) {
			SWType = swCustomInfoType_e.swCustomInfoText;
		}

		public override void Set(object data_, string value_) {
			Value = value_;
			_data = (int)data_;
		}

		public override SwProperty Get() {
			InnerGet();
			int intval = 0;
			int tp = 0;
			ENGINEERINGDataSet.CUT_PARTSDataTable cpdt = new ENGINEERINGDataSet.CUT_PARTSDataTable();
			cpdt = cpta.GetDataByPartID(PartID);

			if (cpdt.Rows.Count > 0) {
				tp = (int)cpdt.Rows[0][@"TYPE"];
			}
			
			if (int.TryParse(Value, out intval)) {
					ENGINEERINGDataSet.CUT_OPSDataTable codt = cota.GetDataByOpID(intval, tp);
					if (codt.Rows.Count > 0) {
						FriendlyValue = codt.Rows[0][@"OPNAME"].ToString();
						Data = intval;
					}
			} else {
				//ENGINEERINGDataSet.CUT_OPSDataTable codt = cota.GetDataByOpName(Value, tp);
				FriendlyValue = Value;
				//if (codt.Rows.Count > 0) {
				//	Value = codt.Rows[0][@"OPID"].ToString();
				//}
			}
			return this;
		}

		private int _type = 1;

		public int OpType {
			get { return _type; }
			set { _type = value; }
		}

		public string FriendlyValue { get; set; }

		protected int _data = 0;

		public override object Data {
			get {
				if (_data == 0) {
					ENGINEERINGDataSet.CUT_OPSDataTable codt = cota.GetDataByOpName(Value, _type);
					if (codt.Rows.Count > 0) {
						_data = (int)(codt.Rows[0] as ENGINEERINGDataSet.CUT_OPSRow)[@"OPID"];
					}
				}
				return _data;
			}

			set {
				int res_ = 0;
				if (int.TryParse(value.ToString(), out res_)) {
					_data = res_;
					Value = cota.GetOpNameByID(_data).ToString();
				} else {
					_data = (int)cota.GetOpIDByName(value.ToString(), OpType);
					Value = cota.GetOpNameByID(_data).ToString();
				}
			}
		}

	}
}
