using System;
using System.Collections.Generic;
using System.Text;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
	/// <summary>
	/// A property where "Data" returns a DateTime.
	/// </summary>
	public class DateProperty : SwProperty {
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">A property name.</param>
		/// <param name="global">Whether it's global.</param>
		/// <param name="sw">The connected application.</param>
		/// <param name="md">A ModelDoc2.</param>
		public DateProperty(string name, bool global, SldWorks sw, ModelDoc2 md)
			: base(name, global, sw, md) {
			SWType = swCustomInfoType_e.swCustomInfoDate;
		}

		/// <summary>
		/// Read data from a SolidWorks property.
		/// </summary>
		/// <returns>This.</returns>
		public override SwProperty Get() {
			InnerGet();
			DateTime res = DateTime.Now;

			if (DateTime.TryParse(ResolvedValue, out res)) {
				_data = res;
			}
			return this;
		}

		/// <summary>
		/// Internal value for "Data".
		/// </summary>
		protected new DateTime _data = DateTime.Now;

		/// <summary>
		/// Data formatted for entry into the db.
		/// </summary>
		public override object Data {
			get {
				return _data;
			}
			set {
				if (DateTime.TryParse(value.ToString(), out _data)) {
					Value = value.ToString();
				}
			}
		}

	}
}
