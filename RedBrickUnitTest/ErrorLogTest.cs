using System;
using RedBrick2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RedBrickUnitTest {
	[TestClass]
	public class ErrorLogTest {
		[TestMethod]
		public void TestWindow() {
			using (RedBrick2.ErrorLog.ErrorLog e = new RedBrick2.ErrorLog.ErrorLog()) {
				e.Visible = false;
				e.Visible = true;
				e.Visible = false;
				e.ShowDialog();
			}

		}
	}
}
