using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RedBrick2;

namespace RedBrickUnitTest {
	[TestClass]
	public class TestQuikTrac {
		[TestMethod]
		public void QuikTracTest() {
			QuickTracLookup quickTracLookup = new QuickTracLookup(@"084029");
			quickTracLookup.Visible = false;
			quickTracLookup.ShowDialog();
		}
	}
}
