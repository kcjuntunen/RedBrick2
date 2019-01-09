using System;
using SolidWorks.Interop.sldworks;

namespace RedBrick2 {
	class EdgeId : IntProperty {
		public EdgeId(string name, bool global, SldWorks sw, ModelDoc2 md, string fieldName)
			: base(name, global, sw, md, @"", fieldName) {
				DoNotWrite = false;
		}

		public override void Set(object data_, string value_) {
			Value = value_;
			Data = (int)data_;
		}

		protected new int _data = 0;

		public override object Data {
			get {
				if (Value == null) {
					Get();
				}
				if (_data == 0) {
					if (int.TryParse(Value, out int test_)) {
						_data = test_;
					}
				}
				return _data;
			}
			set {
				if (value is string) {
					using (ENGINEERINGDataSetTableAdapters.CUT_EDGES_XREFTableAdapter cex =
						new ENGINEERINGDataSetTableAdapters.CUT_EDGES_XREFTableAdapter()) {
						try {
							_data = (int)cex.GetEdgeID(value.ToString());
						} catch (Exception e) {
							_data = 0;
							Redbrick.ProcessError(e);
						}
					}
				} else {
					using (ENGINEERINGDataSetTableAdapters.CUT_EDGESTableAdapter ce =
						new ENGINEERINGDataSetTableAdapters.CUT_EDGESTableAdapter()) {
						if (value != null) {
							int res;
							if (!int.TryParse(value.ToString(), out res)) {
								res = 0;
							}
							Value = value.ToString();
							//Value = (string)ce.GetEdgeDescrByID(res);
						} else {
							_data = 0;
						}
					}
				}
			}
		}
	}
}
