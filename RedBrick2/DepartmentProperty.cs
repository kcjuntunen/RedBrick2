using System;
using System.Collections.Generic;
using System.Text;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
	class DepartmentProperty : IntProperty {
		public DepartmentProperty(string name, bool global, SldWorks sw, ModelDoc2 md, string fieldName)
			: base(name, global, sw, md, @"CUT_PARTS", @"TYPE") {
				SWType = swCustomInfoType_e.swCustomInfoText;
		}

		public override void Set(object data_, string value_) {
			Value = value_;
			_data = (int)data_;
		}

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

		protected new int _data = 0;

		public override object Data {
			get { return _data; }
			set {
				using (ENGINEERINGDataSetTableAdapters.CUT_PART_TYPESTableAdapter cpt =
					new ENGINEERINGDataSetTableAdapters.CUT_PART_TYPESTableAdapter()) {
					int res_ = 0;
					if (int.TryParse(value.ToString(), out res_)) {
						_data = res_;
						Value = (string)cpt.GetDescrByID(_data);
					} else {
						_data = (int)cpt.GetIDByDescr(value.ToString());
						Value = value.ToString();
					}
				}
			}
		}
	}
}
