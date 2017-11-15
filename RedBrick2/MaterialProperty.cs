using System;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
	public class MaterialProperty : IntProperty {
		public MaterialProperty(string name, bool global, SldWorks sw, ModelDoc2 md, string fieldName)
			: base(name, global, sw, md, @"CUT_CUTLIST_PARTS", fieldName) {
			SWType = swCustomInfoType_e.swCustomInfoText;
		}

		protected int _data = 0;

		public override object Data {
			get { return _data; }
			set {
				ENGINEERINGDataSetTableAdapters.CUT_MATERIALSTableAdapter cmta =
					new ENGINEERINGDataSetTableAdapters.CUT_MATERIALSTableAdapter();
				try {
					int res;
					if (!int.TryParse(value.ToString(), out res)) {
						res = 0;
					}
					_data = res;
					Value = cmta.GetDataByMatID(res)[0].DESCR;
				} catch (Exception) {
					_data = Properties.Settings.Default.DefaultMaterial;
				}
			}
		}

	}
}
