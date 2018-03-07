using SolidWorks.Interop.sldworks;

namespace RedBrick2 {
	class AuthorUIDProperty : StringProperty {
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">A property name.</param>
		/// <param name="global">Whether it's global.</param>
		/// <param name="sw">The connected application.</param>
		/// <param name="md">A ModelDoc2.</param>
		public AuthorUIDProperty(string name, bool global, SldWorks sw, ModelDoc2 md)
			: base(name, global, sw, md, string.Empty) {
				ToDB = false;
		}

		/// <summary>
		/// Read data from SolidWorks property.
		/// </summary>
		/// <returns>This.</returns>
		public override SwProperty Get() {
			InnerGet();
			if (Value.Length > 0) {
				using (ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter gu =
					new ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter()) {
					ENGINEERINGDataSet.GEN_USERSDataTable dt = gu.GetDataByUsername(Value);
					if (dt.Rows.Count > 0) {
						ENGINEERINGDataSet.GEN_USERSRow row = dt.Rows[0] as ENGINEERINGDataSet.GEN_USERSRow;
						row = dt[0];
						_data = row.UID;
						FullName = row.Fullname;
					} else {
						FullName = Value;
					}
				}
			}
			return this;
		}

		/// <summary>
		/// An author's full name.
		/// </summary>
		public string FullName { get; set; }

		/// <summary>
		/// An author's initials.
		/// </summary>
		public string Initials {
			get { return Value; }
			private set { Value = value; }
		}

		/// <summary>
		/// Internal value for "Data".
		/// </summary>
		protected new int _data;

		/// <summary>
		/// Data formatted for entry into db.
		/// </summary>
		public override object Data {
			get { return _data; }
			set {
				if (value is int) {
					_data = (int)value;
					using (ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter gu =
						new ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter()) {
						ENGINEERINGDataSet.GEN_USERSDataTable dt = gu.GetDataByUID(_data);
						if (dt.Rows.Count > 0) {
							Value = dt[0].USERNAME;
							FullName = dt[0].Fullname;
						} else {
							FullName = Value;
						}
					}
				}
			}
		}
	}
}
