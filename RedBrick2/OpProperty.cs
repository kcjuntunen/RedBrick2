using System;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
	/// <summary>
	/// A routing property. "Value" should be a routing name, and "Data" should be an OPID.
	/// </summary>
	public class OpProperty : IntProperty {
		ENGINEERINGDataSetTableAdapters.CUT_OPSTableAdapter cota =
			new ENGINEERINGDataSetTableAdapters.CUT_OPSTableAdapter();

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">A property name.</param>
		/// <param name="global">Whether it's global.</param>
		/// <param name="sw">The connected application.</param>
		/// <param name="md">A ModelDoc2.</param>
		/// <param name="fieldName">The field of CUT_PART_OPS.</param>
		public OpProperty(string name, bool global, SldWorks sw, ModelDoc2 md, string fieldName)
			: base(name, global, sw, md, @"CUT_PARTS", fieldName) {
			SWType = swCustomInfoType_e.swCustomInfoText;
		}

		/// <summary>
		/// Directly set Data and Value. This is useful if IDs are already known.
		/// </summary>
		/// <param name="data_">A POPOP value.</param>
		/// <param name="value_">An OPNAME to be written to the part property.</param>
		public override void Set(object data_, string value_) {
			Value = value_;
			_data = (int)data_;
		}

		/// <summary>
		/// Get data and populate db vars.
		/// </summary>
		/// <returns>This.</returns>
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

		/// <summary>
		/// The part type is necessary to find the right rows in CUT_PART_OPS.
		/// </summary>
		public int OpType {
			get { return _type; }
			set { _type = value; }
		}

		/// <summary>
		/// The op name.
		/// </summary>
		public string FriendlyValue { get; set; }

		/// <summary>
		/// Internal value of Data.
		/// </summary>
		protected new int _data = 0;

		/// <summary>
		/// An value appropriately formatted for the db.
		/// </summary>
		public override object Data {
			get {
				if (_data == 0) {
					ENGINEERINGDataSetTableAdapters.FriendlyCutOpsTableAdapter fcota =
						new ENGINEERINGDataSetTableAdapters.FriendlyCutOpsTableAdapter();
					_data = Convert.ToInt32(fcota.GetID(_type, Value));
				}
				return _data;
			}

			set {
				int res_ = 0;
				if (int.TryParse(value.ToString(), out res_)) {
					_data = res_;
					Value = Convert.ToString(cota.GetOpNameByID(_data));
				} else {
					_data = Convert.ToInt32(cota.GetOpIDByName(value.ToString(), OpType));
					Value = value.ToString();
				}
			}
		}

	}
}
