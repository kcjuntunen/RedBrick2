using Microsoft.VisualStudio.TestTools.UnitTesting;
using RedBrick2.ECRViewer;

namespace RedBrickUnitTest {
	[TestClass]
	public class ECRviewerTests {
		[TestMethod]
		public void TestECRViewer() {
			using (ECRViewer eCRViewer = new ECRViewer(@"KOAD1614-01-01")) {
				eCRViewer.Visible = false;
				eCRViewer.ShowDialog();
			}
		}

		[TestMethod]
		public void TestECRViewerCrash() {
			using (ECRViewer eCRViewer = new ECRViewer(@"FX-14.7L")) {
				eCRViewer.Visible = false;
				eCRViewer.ShowDialog();
			}
		}
	}
}
