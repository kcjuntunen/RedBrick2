using System;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
	/// <summary>
	/// A property that reads and writes a string to SW properties,
	/// and populates .Data with a double value.
	/// </summary>
	public class DimensionProperty : SwProperty {
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="name">Property name</param>
		/// <param name="global">Is the property global?</param>
		/// <param name="sw">An SwApp.</param>
		/// <param name="md">A ModelDoc2.</param>
		/// <param name="fieldName">Field name in DB.</param>
		public DimensionProperty(string name, bool global, SldWorks sw, ModelDoc2 md, string fieldName)
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
			InnerGet();
			double res = 0.0F;
			if (!double.TryParse(ResolvedValue, out res)) {
				_data = res;
			}

			return this;
		}

		/// <summary>
		/// Directly set "_data" and "Value".
		/// </summary>
		/// <param name="data">A double of dimension value.</param>
		/// <param name="val">A string of crazy SolidWorks dimensionese.</param>
		public override void Set(object data, string val) {
			double res;
			if (!double.TryParse((string)data, out res)) {
				res = 0.0f;
			}
			_data = res;
			Value = val;
		}

		/// <summary>
		/// The internal value for  "Data".
		/// </summary>
		protected new double _data = 0.0F;
		// TODO: This needs to take a SW type dimension string, and try to parse it.
		/// <summary>
		/// Data formatted for entry into db.
		/// </summary>
		public override object Data {
			get {
				if (_data < .001) {
					double test_ = 0.0F;
					if (double.TryParse(ResolvedValue.Replace("\"", string.Empty), out test_)) {
						_data = test_;
					}
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
