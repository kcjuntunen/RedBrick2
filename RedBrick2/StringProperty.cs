using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
	/// <summary>
	/// A property where "Data" returns a string.
	/// </summary>
	public class StringProperty : SwProperty {
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">A property name.</param>
		/// <param name="global">Whether it's global.</param>
		/// <param name="sw">The connected application.</param>
		/// <param name="md">A ModelDoc2.</param>
		/// <param name="fieldName">The relevant field name.</param>
		public StringProperty(string name, bool global, SldWorks sw, ModelDoc2 md, string fieldName)
			: base(name, global, sw, md) {
			SWType = swCustomInfoType_e.swCustomInfoText;
			TableName = @"CUT_PARTS";
			FieldName = fieldName;
		}

		/// <summary>
		/// Read data from SolidWorks property.
		/// </summary>
		/// <returns>This.</returns>
		public override SwProperty Get() {
			return base.Get();
		}

		/// <summary>
		/// Internal value for "Data".
		/// </summary>
		protected new string _data;

		/// <summary>
		/// Data formatted for entry into the db.
		/// </summary>
		public override object Data {
			get { return _data; }
			set {
				Value = value.ToString();
				_data = Redbrick.FilterString(value);
			}
		}
	}
}
