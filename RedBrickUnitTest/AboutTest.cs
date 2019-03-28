using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RedBrick2;

namespace RedBrickUnitTest {
	[TestClass]
	public class AboutTest {
		private static string GetChangelog() {
			string fn_ = @"G:\Solid Works\Amstore_Macros\Redbrick\version.xml";
			string elementName = string.Empty;
			using (XmlReader r_ = XmlReader.Create(fn_)) {
				while (r_.Read()) {
					if (r_.NodeType == XmlNodeType.Element) {
						elementName = r_.Name;
					} else if (r_.NodeType == XmlNodeType.Text && r_.HasValue && elementName == @"message") {
						return r_.Value.Replace("\n", System.Environment.NewLine);
					}
				}
			}
			return string.Empty;
		}

		[TestMethod]
		public void TestAboutBox() {
			AboutBox a = new AboutBox();
			a.Visible = false;
			a.ShowDialog();
		}
	}
}
