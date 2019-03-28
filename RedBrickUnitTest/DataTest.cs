using System;
using RedBrick2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RedBrickUnitTest {
	[TestClass]
	public class DataTest {
		[TestMethod]
		public void TestGetCutlistRuntime() {
			ENGINEERINGDataSet.CUT_PART_OPSDataTable dt_ = new ENGINEERINGDataSet.CUT_PART_OPSDataTable();
			int[] processtypes = { 1, 2, 3, 4, 5, 6 };
			double r = dt_.GetCutlistRunTime(55, processtypes);
			double s = dt_.GetCutlistSetupTime(55, processtypes);
			Assert.IsTrue(Math.Abs(s - 3.2327) < .0001);
			Assert.IsTrue(Math.Abs(r - 0.2030) < .0001);
		}
	}
}
