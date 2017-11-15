using System;
using System.Collections.Generic;
using System.Text;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
	public class CustomerProperty : StringProperty {
		ENGINEERINGDataSetTableAdapters.GEN_CUSTOMERSTableAdapter gc =
			new ENGINEERINGDataSetTableAdapters.GEN_CUSTOMERSTableAdapter();

		public CustomerProperty(string name, bool global, SldWorks sw, ModelDoc2 md)
			: base(name, global, sw, md, string.Empty) {

		}

		public override SwProperty Get() {
			InnerGet();
			string searchTerm = ResolvedValue.Split('-')[0].Trim();

			ENGINEERINGDataSet.GEN_CUSTOMERSRow row = null;
			try {
				row = gc.GetDataBySearchTerm(searchTerm + '%')[0];
			} catch (Exception) {
				//
			}

			if (row != null) {
				_data = row.CUSTID;
			}

			return this;
		}

		protected int _data;

		public override object Data {
			get { return _data; }
			set {
				_data = (int)value;
				ENGINEERINGDataSet.GEN_CUSTOMERSRow row = null;
				try {
					row = gc.GetDataByCustID(_data)[0];
				} catch (Exception) {
					//
				}
				if (row != null) {
					string firstWord = row.CUSTOMER.Split(' ')[0];
					string shortCustName = string.Format(@"{0} - {1}", firstWord, row.CUSTNUM);
					Value = shortCustName;
				}
			}
		}

	}
}
