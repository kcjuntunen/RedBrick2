﻿using System;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
	/// <summary>
	/// A property where "Data" returns an int.
	/// </summary>
	public class IntProperty : SwProperty {
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">A property name.</param>
		/// <param name="global">Whether it's global.</param>
		/// <param name="sw">The connected application.</param>
		/// <param name="md">A ModelDoc2.</param>
		/// <param name="tableName">The relevant table name.</param>
		/// <param name="fieldName">The relevant field name.</param>
		public IntProperty(string name, bool global, SldWorks sw, ModelDoc2 md, string tableName, string fieldName)
			: base(name, global, sw, md) {
			SWType = swCustomInfoType_e.swCustomInfoNumber;
			TableName = tableName;
			FieldName = fieldName;
		}

		/// <summary>
		/// Read data from SolidWorks property.
		/// </summary>
		/// <returns>This.</returns>
		public override SwProperty Get() {
			InnerGet();
			int res;
			if (!int.TryParse(Value, out res)) {
				res = 0;
			}
			_data = res;
			return this;
		}

		/// <summary>
		/// Internal value for "Data".
		/// </summary>
		protected new int _data = 0;

		/// <summary>
		/// Data formatted for entry into the db.
		/// </summary>
		public override object Data {
			get {
				int test_ = -1;
				if (int.TryParse(Value, out test_)) {
					_data = test_;
				}
				return _data;
			}
			set {
				Value = value.ToString();
				if (value is string) {
					int test_ = 0;
					if (int.TryParse(value as string, out test_)) {
						_data = test_;
					}
				}

				if (value is int) {
					_data = (int)value;
				}

				if (value is decimal) {
					_data = Convert.ToInt32(value);
				}
			}
		}
	}
}
