using System;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
	/// <summary>
	/// A property where "Data" returns an int and "Value" returns a description.
	/// </summary>
	public class EdgeProperty : IntProperty {
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">A property name.</param>
		/// <param name="global">Whether it's global.</param>
		/// <param name="sw">The connected application.</param>
		/// <param name="md">A ModelDoc2.</param>
		/// <param name="fieldName">The relevant field name.</param>
		public EdgeProperty(string name, bool global, SldWorks sw, ModelDoc2 md, string fieldName)
			: base(name, global, sw, md, @"CUT_CUTLIST_PARTS", fieldName) {
				SWType = swCustomInfoType_e.swCustomInfoText;
		}

		/// <summary>
		/// Directly set "_data" and "Value".
		/// </summary>
		/// <param name="data_">An edge ID int.</param>
		/// <param name="value_">A edge DESCR string.</param>
		public override void Set(object data_, string value_) {
			Value = value_;
			Data = (int)data_;
		}

		/// <summary>
		/// Internal value for "Data".
		/// </summary>
		protected new int _data = 0;

		/// <summary>
		/// Data formatted for entry into the db.
		/// </summary>
		public override object Data {
			get {
				if (Value == null) {
					Get();
				}
				if (_data == 0) {
					ENGINEERINGDataSet.CUT_EDGESDataTable dt_ =
						new ENGINEERINGDataSet.CUT_EDGESDataTable();
					_data = dt_.GetEdgeIDByDescr(Value);
				}
				return _data;
			}
			set {
				if (value is string) {
					ENGINEERINGDataSet.CUT_EDGESDataTable dt_ =
						new ENGINEERINGDataSet.CUT_EDGESDataTable();
					if ((string)value != string.Empty) {
						Value = value.ToString();
						try {
							_data = dt_.GetEdgeIDByDescr(value.ToString());
						} catch (Exception) {
							_data = 0;
						}
					} else {
						_data = 0;
					}
				} else {
					ENGINEERINGDataSetTableAdapters.CUT_EDGESTableAdapter ce =
						new ENGINEERINGDataSetTableAdapters.CUT_EDGESTableAdapter();
					if (value != null) {
						int res;
						if (!int.TryParse(value.ToString(), out res)) {
							res = 0;
						}
						_data = res;
						Value = (string)ce.GetEdgeDescrByID(res);
					} else {
						_data = 0;
					}
				}
			}
		}

	}
}
