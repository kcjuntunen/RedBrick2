using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using RedBrick2;
using SolidWorks.Interop.sldworks;

namespace RedBrickUnitTest {
	[TestClass]
	public class SwTableTypeTest {
		SldWorks app_ = null;
		[TestInitialize]
		public void setup() {
			app_ = System.Runtime.InteropServices.Marshal.GetActiveObject(@"SldWorks.Application") as SldWorks;
			//app_ = new SldWorks();
			//app_.Visible = true;
		}

		[TestMethod]
		public void TestPathGetting() {
			try {
				System.Collections.Specialized.StringCollection sc_ = new System.Collections.Specialized.StringCollection();
				foreach (var item in Redbrick.MasterHashes) {
					sc_.Add(item);
				}
				SwTableType swt_ = new SwTableType(app_.ActiveDoc as ModelDoc2, sc_, @"PART");
				Console.WriteLine(swt_.get_path2(swt_.Parts[1]).FullName);
				Assert.IsTrue((new FileInfo(swt_.get_path2(swt_.Parts[1]).FullName)).Exists);
			} catch (System.Runtime.InteropServices.COMException e) {
				Console.WriteLine(e.Message);
			}
		}
	}
}
