﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
	/// <summary>
	/// A property where "Data" is an int of an ID from GEN_USERS.
	/// </summary>
	public class AuthorProperty : StringProperty {
		ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter gu =
			new ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter();

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">A property name.</param>
		/// <param name="global">Whether it's global.</param>
		/// <param name="sw">The connected application.</param>
		/// <param name="md">A ModelDoc2.</param>
		public AuthorProperty(string name, bool global, SldWorks sw, ModelDoc2 md)
			: base(name, global, sw, md, string.Empty) {

		}

		/// <summary>
		/// Read data from SolidWorks property.
		/// </summary>
		/// <returns>This.</returns>
		public override SwProperty Get() {
			InnerGet();
			if (Value.Length > 0) {
				ENGINEERINGDataSet.GEN_USERSDataTable dt =
					new ENGINEERINGDataSet.GEN_USERSDataTable();
				ENGINEERINGDataSet.GEN_USERSRow row = null;
				string initial_lookup_ = string.Format(@"{0}%", Value.Substring(0, 2));
				gu.FillByInitialAndDepartment(dt, initial_lookup_, Properties.Settings.Default.UserDept);
				if (dt.Rows.Count > 0) {
					row = dt[0];
					_data = row.UID;
					FullName = row.Fullname;
				} else {
					FullName = Value;
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
				_data = (int)value;
				ENGINEERINGDataSet.GEN_USERSDataTable dt = gu.GetDataByUID(_data);
				if (dt.Rows.Count > 0) {
					Value = dt[0].INITIAL.Substring(0, 2);
					FullName = dt[0].Fullname;
				} else {
					FullName = Value;
				}
			}
		}
	}
}
