using System;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
	/// <summary>
	/// A property where "Data" returns a bool, and Value is figured out for Solidworks.
	/// </summary>
	public class BooleanProperty : SwProperty {
		/// <summary>
		/// Constructor for a new <see cref="RedBrick2.BooleanProperty"/>.
		/// </summary>
		/// <param name="name">The name of the property. This will be the name used in the property summary page.</param>
		/// <param name="global">A general property, or configuration specific property.</param>
		/// <param name="sw">A running <see cref="SolidWorks.Interop.sldworks.SldWorks"/> object.</param>
		/// <param name="md">The <see cref="SolidWorks.Interop.sldworks.ModelDoc2"/> to which this property belongs.</param>
		/// <param name="fieldName">The related field name in the database.</param>
		public BooleanProperty(string name, bool global, SldWorks sw, ModelDoc2 md, string fieldName)
			: base(name, global, sw, md) {
			SWType = swCustomInfoType_e.swCustomInfoYesOrNo;
			TableName = @"CUT_PARTS";
			FieldName = fieldName;
		}

		/// <summary>
		/// Sets the Data and Value members of this object.
		/// </summary>
		/// <param name="data">a <see cref="System.Object"/> to be interpreted into the <see cref="System.Boolean"/> type.</param>
		/// <param name="val">a <see cref="System.String"/> to go directly into SolidWorks properties.</param>
		public override void Set(object data, string val) {
			if (data is bool) {
				_data = (bool)data;
			} else {
				_data = data.ToString().ToUpper().Contains(@"Y");
			}
			Value = _data ? @"Yes" : @"NO";
		}

		/// <summary>
		/// Pulls property data from SolidWorks.
		/// </summary>
		/// <returns>Returns a <see cref="RedBrick2.SwProperty"/> object.</returns>
		public override SwProperty Get() {
			InnerGet();
			bool res = Value.ToUpper().Contains(@"Y");
			Data = res;
			return this;
		}

		/// <summary>
		/// Internal value for "Data".
		/// </summary>
		protected bool _data = false;

		/// <summary>
		/// Data formatted for db entry.
		/// </summary>
		public override object Data {
			get { return _data; }
			set {
				_data = (bool)value;
				Value = _data ? @"Yes" : @"N";
			}
		}
	}
}
