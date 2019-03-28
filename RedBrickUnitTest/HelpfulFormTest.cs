using RedBrick2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RedBrick2.Time_Entry;
using RedBrick2.CutlistTableDisplay;

namespace RedBrickUnitTest {
	[TestClass]
	public class HelpfulFormTest {
		[TestMethod]
		public void TestCNCTodo() {
			using (CncTodo ct_ = new CncTodo()) {
				ct_.Visible = false;
				ct_.Visible = true;
				ct_.Visible = false;
				ct_.ShowDialog();
			}
		}

		[TestMethod]
		public void TestTimeEntry() {
			using (TimeEntry te = new TimeEntry()) {
				te.Visible = false;
				te.Visible = true;
				te.Visible = false;
				te.ShowDialog();
			}
		}

		[TestMethod]
		public void TestDrawings() {
			using (RedBrick2.Drawings.Drawings d = new RedBrick2.Drawings.Drawings()) {
				d.Visible = false;
				d.Visible = true;
				d.Visible = false;
				d.ShowDialog();
			}
		}
		[TestMethod]
		public void TestCutlistDisplay() {
			using (CutlistTableDisplayForm te = new CutlistTableDisplayForm("TEST", "100")) {
				te.Visible = false;
				te.Visible = true;
				te.Visible = false;
				te.ShowDialog();
			}
		}

		[TestMethod]
		public void TestCutlistDisplay2() {
			using (CutlistTableDisplayForm te = new CutlistTableDisplayForm(55)) {
				te.Visible = false;
				te.Visible = true;
				te.Visible = false;
				te.ShowDialog();
			}
		}
	}
}
