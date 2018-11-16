#pragma warning disable 1591
namespace RedBrick2.ManageCutlistTime {
	/// <summary>
	/// I was tired of using raw numbers. These names kinda mean something to me.
	/// </summary>
	public static class Schema {
		/// <summary>
		/// Names from the CUTLIST TIMES db.
		/// </summary>
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
