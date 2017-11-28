using System;
using System.Collections.Generic;
using System.Text;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
	class EdgeId : IntProperty {
		ENGINEERINGDataSetTableAdapters.CUT_MATERIALSTableAdapter cmta =
			new ENGINEERINGDataSetTableAdapters.CUT_MATERIALSTableAdapter();

		public EdgeId(string name, bool global, SldWorks sw, ModelDoc2 md, string fieldName)
			: base(name, global, sw, md, @"", fieldName) {

		}

		protected int _data = 0;

		public override object Data {
			get { return _data; }
			set {
				if (value is string) {
					ENGINEERINGDataSetTableAdapters.CUT_EDGES_XREFTableAdapter cex =
						new ENGINEERINGDataSetTableAdapters.CUT_EDGES_XREFTableAdapter();
					try {
						_data = (int)cex.GetEdgeID(value.ToString());
					} catch (Exception) {
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
