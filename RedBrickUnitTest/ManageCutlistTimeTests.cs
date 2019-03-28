using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RedBrickUnitTest {
	[TestClass]
	public class ManageCutlistTimeTests {
		[TestMethod]
		public void TestCutlistTime() {
			RedBrick2.ManageCutlistTime.ManageCutlistTime manage = new RedBrick2.ManageCutlistTime.ManageCutlistTime() {
				Visible = true
			};
			manage.Visible = false;
			manage.ShowDialog();
		}

		[TestMethod]
		public void TestGuessedCutlistTime() {
			RedBrick2.ManageCutlistTime.ManageCutlistTime manage = new RedBrick2.ManageCutlistTime.ManageCutlistTime(@"KASC1400-03-05") {
				Visible = true
			};
			manage.Visible = false;
			manage.ShowDialog();
		}

		[TestMethod]
		public void TestCountEdges() {
			RedBrick2.ManageCutlistTime.ManageCutlistTimeDataSetTableAdapters.CutlistPartsTableAdapter cpa_ =
				new RedBrick2.ManageCutlistTime.ManageCutlistTimeDataSetTableAdapters.CutlistPartsTableAdapter();
			Assert.AreEqual(3, cpa_.CountEdges(13588, 17));
		}
	}
}
