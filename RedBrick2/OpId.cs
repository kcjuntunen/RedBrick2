using System;
using System.Collections.Generic;
using System.Text;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
	class OpId  : IntProperty {
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

		public string FriendlyValue { get; set; }

		protected new int _data = 0;

		public override object Data {
			get {
				if (_data == 0) {
					using (ENGINEERINGDataSetTableAdapters.FriendlyCutOpsTableAdapter fcota =
						new ENGINEERINGDataSetTableAdapters.FriendlyCutOpsTableAdapter()) {
						int opid_ = 0;
						if (int.TryParse(Value, out opid_)) {
							string name_ = (string)fcota.GetOpNameByOldID(opid_);
							int? testvl_ = fcota.GetID(_type, name_);
							if (testvl_ != null) {
								_data = (int)testvl_;
							}
						}
					}
				}
				return _data;
			}
			set {
				if (value is string) {
					try {
						using (ENGINEERINGDataSetTableAdapters.CUT_PARTSTableAdapter cpta =
							new ENGINEERINGDataSetTableAdapters.CUT_PARTSTableAdapter()) {
							using (ENGINEERINGDataSet.CUT_PARTSDataTable cpdt =
								new ENGINEERINGDataSet.CUT_PARTSDataTable()) {
								cpta.FillByPartID(cpdt, PartID);
								OpType = (int)cpdt.Rows[0][@"TYPE"];
							}
						}
					} catch (Exception) {

					}
					using (ENGINEERINGDataSetTableAdapters.CUT_OPSTableAdapter cota =
						new ENGINEERINGDataSetTableAdapters.CUT_OPSTableAdapter()) {
						_data = (int)cota.GetOpIDByName(value.ToString(), OpType);
					}
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
