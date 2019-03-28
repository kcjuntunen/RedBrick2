using System;
using RedBrick2.Org;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RedBrickUnitTest {
	[TestClass]
	public class OrgTests {
		[TestMethod]
		public void TestMethod1() {
			OrgGenerator o = new OrgGenerator();
			string f = o.Generate();
			Assert.AreNotEqual(f, string.Empty);
		}
	}
}
