using RedBrick2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RedBrickUnitTest {
	[TestClass]
	public class RenameCutlistTest {
		[TestMethod]
		public void RenameCutlistNoArgumentTest() {
			using (RenameCutlist rc = new RenameCutlist()) {
				rc.ShowDialog();
			}
		}

		[TestMethod]
		public void RenameCutlistWithArgumentTest() {
			using (RenameCutlist rc = new RenameCutlist(13420)) {
				rc.ShowDialog();
			}
		}

		[TestMethod]
		public void RenameCutlistWithRowTest() {
			using (ENGINEERINGDataSet.CUT_CUTLISTSDataTable dt_ = new ENGINEERINGDataSet.CUT_CUTLISTSDataTable()) {
				ENGINEERINGDataSet.CUT_CUTLISTSRow r_ = dt_.NewCUT_CUTLISTSRow();
				r_.CLID = 66666666;
				r_.CUSTID = 17;
				r_.CDATE = new System.DateTime();
				r_.DESCR = @"Toordis.";
				r_.PARTNUM = @"Stupid Cutlist";
				r_.REV = @"105";
				r_.DRAWING = @"Monkies!";
				using (RenameCutlist rc = new RenameCutlist(r_)) {
					rc.ShowDialog();
				}
			}
		}

	}
}
