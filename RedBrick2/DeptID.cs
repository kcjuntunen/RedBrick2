using System;
using System.Collections.Generic;
using System.Text;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
	/// <summary>
	/// A property where "Data" is a int TYPEID, and "Value" is a string of that int.
	/// </summary>
	public class DeptId : IntProperty {
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">A property name.</param>
		/// <param name="global">Whether it's global.</param>
		/// <param name="sw">The connected application.</param>
		/// <param name="md">A ModelDoc2.</param>
		/// <param name="fieldName">The relevant field name.</param>
		public DeptId(string name, bool global, SldWorks sw, ModelDoc2 md, string fieldName)
			: base(name, global, sw, md, @"CUT_PARTS", fieldName) {
				DoNotWrite = true;
		}

		/// <summary>
		/// Directly set "_data" and "Value".
		/// </summary>
		/// <param name="data_">An int TYPEID.</param>
		/// <param name="value_">A string TYPEID.</param>
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
			using (ENGINEERINGDataSetTableAdapters.CUT_PART_TYPESTableAdapter cpt =
				new ENGINEERINGDataSetTableAdapters.CUT_PART_TYPESTableAdapter()) {
				if (Value != string.Empty) {
					int? _id = cpt.GetIDByDescr(Value);
					if (_id != null) {
						_data = (int)_id;
					} else {
						_data = 1;
					}
				} else {
					IntProperty i = new IntProperty(@"DEPTID", true, SwApp, ActiveDoc, @"CUT_PARTS", FieldName);
					i.Get();
					int tmp = 0;
					if (int.TryParse(i.Value, out tmp)) {
						_data = tmp;
						Value = (string)cpt.GetDescrByID(tmp);
					}
				}
			}
			return this;
		}

		/// <summary>
		/// Internal value for "Data".
		/// </summary>
		protected new int _data = 0;

		/// <summary>
		/// Data formatted for entry into db.
		/// </summary>
		public override object Data {
			get { return _data; }
			set
			{
				using (ENGINEERINGDataSetTableAdapters.CUT_PART_TYPESTableAdapter cpt =
					new ENGINEERINGDataSetTableAdapters.CUT_PART_TYPESTableAdapter()) {
					if (value is string) {
						_data = (int)cpt.GetIDByDescr(value.ToString());
						Value = value.ToString();
					} else {
						try {
							_data = int.Parse(value.ToString());
							Value = (string)cpt.GetDescrByID(_data);
						} catch (Exception) {
							_data = 1;
						}
					}
				}
			}
		}
	}
}
