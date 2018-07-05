using System;
using System.Collections.Generic;
using System.Text;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
	/// <summary>
	/// A property that manages customer data.
	/// </summary>
	public class CustomerProperty : StringProperty {
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">A property name.</param>
		/// <param name="global">Whether it's global.</param>
		/// <param name="sw">The connected application.</param>
		/// <param name="md">A ModelDoc2.</param>
		public CustomerProperty(string name, bool global, SldWorks sw, ModelDoc2 md)
			: base(name, global, sw, md, string.Empty) {

		}

		/// <summary>
		/// Read data from SolidWorks property.
		/// </summary>
		/// <returns>This.</returns>
		public override SwProperty Get() {
			InnerGet();
			if (ResolvedValue == null) {
				Value = string.Empty;
				ResolvedValue = string.Empty;
				return this;
			}
			string searchTerm = ResolvedValue.Split('-')[0].Trim();

			ENGINEERINGDataSet.GEN_CUSTOMERSRow row = null;
			using (ENGINEERINGDataSetTableAdapters.GEN_CUSTOMERSTableAdapter gc =
				new ENGINEERINGDataSetTableAdapters.GEN_CUSTOMERSTableAdapter()) {
				using (ENGINEERINGDataSet.GEN_CUSTOMERSDataTable gcdt = gc.GetDataBySearchTerm(searchTerm + '%')) {
					if (gcdt.Count > 0) {
						row = gcdt[0];
					}
				}
			}

			if (row != null) {
				_data = row.CUSTID;
			}

			return this;
		}

		/// <summary>
		/// Internal value for "Data".
		/// </summary>
		protected new int _data;

		/// <summary>
		/// Data formated for entry into a db.
		/// </summary>
		public override object Data
		{
			get { return _data; }
			set
			{
				if (value != null) {
					_data = (int)value;
					using (ENGINEERINGDataSetTableAdapters.GEN_CUSTOMERSTableAdapter gc =
						new ENGINEERINGDataSetTableAdapters.GEN_CUSTOMERSTableAdapter()) {
						ENGINEERINGDataSet.GEN_CUSTOMERSDataTable dt_ = gc.GetDataByCustID(_data);
						if (dt_.Count > 0) {
							ENGINEERINGDataSet.GEN_CUSTOMERSRow row = dt_[0] as ENGINEERINGDataSet.GEN_CUSTOMERSRow;
							if (row != null) {
								string firstWord = row.CUSTOMER.Split(' ')[0];
								string custnum = !row.IsCUSTNUMNull() ? row.CUSTNUM.ToString() : string.Empty;
								string shortCustName = string.Format(@"{0} - {1}", firstWord, custnum);
								Value = shortCustName;
							}
						}
					}
				}
			}
		}
	}
}
