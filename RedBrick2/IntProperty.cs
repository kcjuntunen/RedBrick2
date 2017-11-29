using System;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
	public class IntProperty : SwProperty {
		public IntProperty(string name, bool global, SldWorks sw, ModelDoc2 md, string tableName, string fieldName)
			: base(name, global, sw, md) {
			SWType = swCustomInfoType_e.swCustomInfoNumber;
			TableName = tableName;
			FieldName = fieldName;
		}

		public override SwProperty Get() {
			InnerGet();
			int res;
			if (!int.TryParse(Value, out res)) {
				res = 0;
			}
			_data = res;
			return this;
		}

		protected int _data = 0;

		public override object Data {
			get {
				return _data == null ? int.Parse(Value) : _data;
			}
			set {
				try {
					if (value is string) {
						_data = int.Parse(value.ToString());
						Value = value.ToString();
					}

					if (value is int) {
						_data = (int)value;
						Value = value.ToString();
					}
				} catch (Exception) {
					_data = 0;
					Value = @"0";
				}
			}
		}
	}
}
