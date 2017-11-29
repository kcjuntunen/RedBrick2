using System;
using System.Collections.Generic;
using System.Text;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
	class MatId : IntProperty {
		ENGINEERINGDataSetTableAdapters.CUT_MATERIALSTableAdapter cmta =
			new ENGINEERINGDataSetTableAdapters.CUT_MATERIALSTableAdapter();

		public MatId(string name, bool global, SldWorks sw, ModelDoc2 md, string fieldName)
			: base(name, global, sw, md, @"", fieldName) {
				DoNotWrite = true;
		}

		public override void Set(object data_, string value_) {
			Value = value_;
			_data = (int)data_;
		}

		public override void Write() {
			base.Write();
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
				ENGINEERINGDataSet.CUT_MATERIALSDataTable cmdt = cmta.GetDataByMatID(intval);
				if (cmdt.Rows.Count > 0) {
					FriendlyValue = cmdt.Rows[0][@"DESCR"].ToString(); ;
					Data = intval;
				}
			} else {
				ENGINEERINGDataSet.CUT_MATERIALSDataTable cmdt = cmta.GetDataByDescr(Value);
				FriendlyValue = Value;
				if (cmdt.Rows.Count > 0) {
					Value = cmdt.Rows[0][@"MATID"].ToString();
				}
			}
			return this;
		}

		public string FriendlyValue { get; set; }

		protected int _data = 0;

		public override object Data {
			get { return _data; }
			set {
				ENGINEERINGDataSetTableAdapters.CUT_MATERIALSTableAdapter cmta =
					new ENGINEERINGDataSetTableAdapters.CUT_MATERIALSTableAdapter();
				try {
					int res;
					if (!int.TryParse(value.ToString(), out res)) {
						res = 0;
					}
					_data = res;
					Value = cmta.GetDataByMatID(res)[0].MATID.ToString();
				} catch (Exception) {
					_data = Properties.Settings.Default.DefaultMaterial;
				}
			}
		}

	}
}
