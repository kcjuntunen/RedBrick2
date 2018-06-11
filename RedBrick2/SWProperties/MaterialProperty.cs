using System;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
	/// <summary>
	/// A property where "Data" returns an int, and "Value" returns a description.
	/// </summary>
	public class MaterialProperty : IntProperty {
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">A property name.</param>
		/// <param name="global">Whether it's global.</param>
		/// <param name="sw">The connected application.</param>
		/// <param name="md">A ModelDoc2.</param>
		/// <param name="fieldName">The relevant field name.</param>
		public MaterialProperty(string name, bool global, SldWorks sw, ModelDoc2 md, string fieldName)
			: base(name, global, sw, md, @"CUT_CUTLIST_PARTS", fieldName) {
			SWType = swCustomInfoType_e.swCustomInfoText;
		}

		/// <summary>
		/// Directly set "_data" and "Value".
		/// </summary>
		/// <param name="data_">An int of MATID.</param>
		/// <param name="value_">A string of DESCR.</param>
		public override void Set(object data_, string value_) {
			Value = value_;
			_data = (int)data_;
		}

		/// <summary>
		/// Read data from SolidWorks property.
		/// </summary>
		/// <returns>This.</returns>
		public override SwProperty Get() {
			InnerGet();
			int intval = 0;
			int tp = 0;
			using (ENGINEERINGDataSet.CUT_PARTSDataTable cpdt = cpta.GetDataByPartID(PartID)) {
				if (cpdt.Rows.Count > 0) {
					tp = (int)cpdt.Rows[0][@"TYPE"];
				}
			}

			if (int.TryParse(Value, out intval)) {
				using (ENGINEERINGDataSetTableAdapters.CUT_MATERIALSTableAdapter cmta =
					new ENGINEERINGDataSetTableAdapters.CUT_MATERIALSTableAdapter()) {
					using (ENGINEERINGDataSet.CUT_MATERIALSDataTable cmdt = cmta.GetDataByMatID(intval)) {
						if (cmdt.Rows.Count > 0) {
							FriendlyValue = cmdt.Rows[0][@"DESCR"].ToString(); ;
							_data = intval;
						} else {

						}
					}
				}
			}
			return this;
		}

		/// <summary>
		/// A description. Do I use this?
		/// </summary>
		public string FriendlyValue { get; set; }

		/// <summary>
		/// Internal value for "Data".
		/// </summary>
		protected new int _data = 0;

		/// <summary>
		/// Data formatted for entry into db.
		/// </summary>
		public override object Data {
			get {
				if (_data == 0 && Value != null) {
					using (ENGINEERINGDataSetTableAdapters.CUT_MATERIALSTableAdapter cmta =
						new ENGINEERINGDataSetTableAdapters.CUT_MATERIALSTableAdapter()) {
						using (ENGINEERINGDataSet.CUT_MATERIALSDataTable cmdt = cmta.GetDataByDescr(Value)) {
							FriendlyValue = Value;
							if (cmdt.Rows.Count > 0) {
								_data = Convert.ToInt32(cmdt.Rows[0][@"MATID"]);
							}
						}
					}
				}
				return _data;
			}
			set {
				using (ENGINEERINGDataSetTableAdapters.CUT_MATERIALSTableAdapter cmta =
					new ENGINEERINGDataSetTableAdapters.CUT_MATERIALSTableAdapter()) {
					try {
						int res;
						if (int.TryParse(value.ToString(), out res) && res != 0) {
							_data = res;
							Value = cmta.GetDataByMatID(res)[0].DESCR;
						} else {
							Value = value.ToString();
						}
					} catch (Exception) {
						_data = Properties.Settings.Default.DefaultMaterial;
					}
				}
			}
		}
	}
}
