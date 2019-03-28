using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RedBrick2.ENGINEERINGDataSetTableAdapters;

namespace Redbrick2DataTest {
  [TestClass]
  public class DataTest1 {
    [TestMethod]
    public void TestDrawingLookup() {
      GEN_DRAWINGSTableAdapter ta = new GEN_DRAWINGSTableAdapter();
      RedBrick2.ENGINEERINGDataSet.GEN_DRAWINGSDataTable gddt = new RedBrick2.ENGINEERINGDataSet.GEN_DRAWINGSDataTable();
      gddt = gddt.GetPDFData(@"KOHD1606");
      gddt = gddt.GetPDFData(@"KOHD1614-02");
      gddt = gddt.GetPDFData(@"Z63000 SPACER");
      Assert.AreEqual(@"K:\Z63-SW\", gddt.FPath);
    }
  }
}
