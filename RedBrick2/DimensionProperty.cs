using System;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
	public class DimensionProperty : SwProperty {
		public DimensionProperty(string name, bool global, SldWorks sw, ModelDoc2 md, string fieldName)
			: base(name, global, sw, md) {
			SWType = swCustomInfoType_e.swCustomInfoText;
			TableName = @"CUT_PARTS";
			FieldName = fieldName;
		}

		public override SwProperty Get() {
			InnerGet();
			double res = 0.0F;
			if (!double.TryParse(ResolvedValue, out res)) {
				_data = res;
			}

			return this;
		}

		public override void Set(object data, string val) {
			double res;
			if (!double.TryParse((string)data, out res)) {
				res = 0.0f;
			}
			_data = res;
			Value = val;
		}

		protected double _data = 0.0F;
		// TODO: This needs to take a SW type dimension string, and try to parse it.
		public override object Data {
			get {
				if (_data == null || _data < .001) {
					double test_ = 0.0F;
					if (double.TryParse(ResolvedValue, out test_)) {
						_data = test_;
					}
				}
				return _data;
			}
			set {
				_data = double.Parse(value.ToString());
				Value = value.ToString();
			}
		}
	}
}
