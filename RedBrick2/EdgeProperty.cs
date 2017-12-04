using System;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
	public class EdgeProperty : IntProperty {
		public EdgeProperty(string name, bool global, SldWorks sw, ModelDoc2 md, string fieldName)
			: base(name, global, sw, md, @"CUT_CUTLIST_PARTS", fieldName) {
				SWType = swCustomInfoType_e.swCustomInfoText;
		}

		public override void Set(object data_, string value_) {
			Value = value_;
			Data = (int)data_;
		}

		protected int _data = 0;

		public override object Data {
			get {
				if (_data == 0) {
					ENGINEERINGDataSetTableAdapters.CUT_EDGES_XREFTableAdapter ce =
						new ENGINEERINGDataSetTableAdapters.CUT_EDGES_XREFTableAdapter();
					int? mid = ce.GetEdgeID(Value);
					if (mid != null) {
						_data = (int)mid;
					}
				}
				return _data;
			}
			set {
				if (value is string) {
					ENGINEERINGDataSetTableAdapters.CUT_EDGES_XREFTableAdapter cex =
						new ENGINEERINGDataSetTableAdapters.CUT_EDGES_XREFTableAdapter();
					if (value != string.Empty) {
						Value = value.ToString();
						try {
							_data = (int)cex.GetEdgeID(value.ToString());
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
