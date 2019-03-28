using System;
using RedBrick2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RedBrickUnitTest {
	[TestClass]
	public class EstimationTest {
		[TestMethod]
		public void TestLaserTime() {
			double res = Estimation.GetLaserPartRuntime(@"KOJA1802-05");
			System.Diagnostics.Debug.WriteLine(res);
			Assert.IsTrue(Math.Abs(3.206061 - res) < 0.0003);
		}
	}
}
