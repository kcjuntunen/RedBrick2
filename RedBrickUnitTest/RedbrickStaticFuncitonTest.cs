using RedBrick2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;

namespace RedBrickUnitTest {
	[TestClass]
	public class RedbrickStaticFuncitonTest {
		[TestMethod]
		public void TestLookup() {

			Dictionary<string, FileInfo> files = new Dictionary<string, FileInfo>();
			files.Add(@"GC-48.70SP", new FileInfo(@"G:\KOHLS\GC-48.70SP REV102.SLDASM"));
			files.Add(@"G470.35", new FileInfo(@"G:\KOHLS\ARCHIVED\G470.35 REV101.SLDASM"));
			files.Add(@"A-542", new FileInfo(@"G:\KOHLS\A-542.SLDDRW"));

			foreach (KeyValuePair<string, FileInfo> item in files) {
				string expected = item.Key;
				string actual = Redbrick.FileInfoToLookup(item.Value);
				Assert.AreEqual(expected, actual);
			}
		}
	}
}
