using System;
using System.Collections.Generic;
using System.Text;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
	class OpId  : IntProperty {
		ENGINEERINGDataSetTableAdapters.CUT_OPSTableAdapter cota =
			new ENGINEERINGDataSetTableAdapters.CUT_OPSTableAdapter();

		//ENGINEERINGDataSetTableAdapters.CUT_PART_OPSTableAdapter cpota =
		//	new ENGINEERINGDataSetTableAdapters.CUT_PART_OPSTableAdapter();

		public OpId(string name, bool global, SldWorks sw, ModelDoc2 md, string fieldName)
			: base(name, global, sw, md, @"CUT_PART_OPS", fieldName) {
				DoNotWrite = true;
		}

		public override void Set(object data_, string value_) {
			Value = value_;
			_data = (int)data_;
		}

		public override SwProperty Get() {
			InnerGet();
			return this;
		}

		private int _type = 1;

		public int OpType {
			get { return _type; }
			set { _type = value; }
		}

		public override void Write() {
			int intVal = 0;
			if (int.TryParse(Value, out intVal)) {
				string op = (string)cota.GetOpNameByID(intVal);
				WriteResult =
					(swCustomInfoAddResult_e)PropertyManager.Add3(Name,
					(int)SWType,
					op,
					(int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
			} else {
				WriteResult =
					(swCustomInfoAddResult_e)PropertyManager.Add3(Name,
					(int)SWType,
					Value,
					(int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
			}
		}

		public string FriendlyValue { get; set; }

		protected new int _data = 0;

		public override object Data {
			get {
				if (_data == 0) {
					ENGINEERINGDataSetTableAdapters.FriendlyCutOpsTableAdapter fcota =
						new ENGINEERINGDataSetTableAdapters.FriendlyCutOpsTableAdapter();
					int opid_ = 0;
					if (int.TryParse(Value, out opid_)) {
						string name_ = (string)fcota.GetOpNameByOldID(opid_);
						int? testvl_ = fcota.GetID(_type, name_);
						if (testvl_ != null) {
							_data = (int)testvl_;
						}
					}
				}
				return _data;
			}
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
						Value = value.ToString();
					} catch (Exception) {

					}
				}
			}
		}

	}
}
