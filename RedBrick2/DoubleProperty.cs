using System;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
	/// <summary>
	/// A property where "Data" returns a double.
	/// </summary>
	public class DoubleProperty : SwProperty {
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">A property name.</param>
		/// <param name="global">Global or config specific.</param>
		/// <param name="sw">The connected application.</param>
		/// <param name="md">A ModelDoc2.</param>
		/// <param name="tableName">The relevant table.</param>
		/// <param name="fieldName">The relevant field name.</param>
		public DoubleProperty(string name, bool global, SldWorks sw, ModelDoc2 md, string tableName, string fieldName)
			: base(name, global, sw, md) {
			SWType = swCustomInfoType_e.swCustomInfoDouble;
			TableName = tableName;
			FieldName = fieldName;
		}

		/// <summary>
		/// Read data from SolidWorks property.
		/// </summary>
		/// <returns>This.</returns>
		public override SwProperty Get() {
			InnerGet();
			double res = 0.0F;
			res = double.Parse(ResolvedValue);
			_data = res;
			return this;
		}

		/// <summary>
		/// Internal value for "Data".
		/// </summary>
		protected new double _data = 0.0F;

		/// <summary>
		/// Data formatted for entry into the db.
		/// </summary>
		public override object Data {
			get {
				double test_ = 0.0F;
				if (double.TryParse(ResolvedValue, out test_)) {
					_data = test_;
				}
				return _data;
			}
			set {
				_data = double.Parse(value.ToString());
				Value = value.ToString();
			}
		}
	}
}
