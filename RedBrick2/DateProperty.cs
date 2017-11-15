using System;
using System.Collections.Generic;
using System.Text;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
	public class DateProperty : SwProperty {
		public DateProperty(string name, bool global, SldWorks sw, ModelDoc2 md)
			: base(name, global, sw, md) {
			SWType = swCustomInfoType_e.swCustomInfoDate;
		}

		public override SwProperty Get() {
			InnerGet();
			DateTime res = DateTime.Now;

			if (DateTime.TryParse(ResolvedValue, out res)) {
				_data = res;
			}
			return this;
		}

		protected DateTime _data = DateTime.Now;

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
