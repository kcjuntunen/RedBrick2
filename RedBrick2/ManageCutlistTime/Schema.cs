using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBrick2.ManageCutlistTime {
	public static class Schema {
		public enum Cols {
			TYPE,				// 0
			IS_OP,			// 1
			SETUP_TIME,	// 2
			RUN_TIME,		// 3
			CTID,				// 4
			NOTE,				// 5
			OP,					// 6
			CLID				// 7
		}
	}
}
