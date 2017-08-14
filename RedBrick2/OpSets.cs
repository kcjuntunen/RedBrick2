using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
  public class OpSets : IList<OpSet> {
    private SldWorks SwApp;
    private ModelDoc2 ActiveDoc = null;
    public SwProperties PropertySet;
    private string partLookup = string.Empty;
    private List<OpSet> innerList = new List<OpSet>();

    public OpSets(SwProperties props, string partLookup) {
      SwApp = props.SwApp;
      ActiveDoc = props.ActiveDoc;
      PropertySet = props;
      this.partLookup = partLookup;
      Init();
    }

    private void Init() {
      ENGINEERINGDataSetTableAdapters.CutPartOpsTableAdapter cpo =
        new ENGINEERINGDataSetTableAdapters.CutPartOpsTableAdapter();
      int opNo = 1;
      string test = string.Empty;
      string partLookup = Path.GetFileNameWithoutExtension(this.partLookup).Split(' ')[0];
      foreach (ENGINEERINGDataSet.CutPartOpsRow row in cpo.GetDataBy1(partLookup)) {
        string opn = string.Format(@"OP{0}", opNo);
        OpProperty op = new OpProperty(opn, true, SwApp, ActiveDoc, @"POPOP");
        string odr = string.Format(@"OP{0}ORDER", opNo);
        IntProperty oporder = new IntProperty(odr, true, SwApp, ActiveDoc, @"CUT_PART_OPS", @"POPORDER");
        string stp = string.Format(@"OP{0}SETUP", opNo);
        DoubleProperty opsetup = new DoubleProperty(stp, true, SwApp, ActiveDoc, @"CUT_PART_OPS", @"POPSETUP");
        string run = string.Format(@"OP{0}RUN", opNo++);
        DoubleProperty oprun = new DoubleProperty(run, true, SwApp, ActiveDoc, @"CUT_PART_OPS", @"POPRUN");
        op.Data = row[@"POPOP"];
        Add(new OpSet(PropertySet, row, op, oporder, opsetup, oprun));
        //OpControl opc = new OpControl(row, PropertySet);
        //opc.Width = flowLayoutPanel1.Width - 25;
        //flowLayoutPanel1.Controls.Add(opc);
        //flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        //opc.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      }
    }

    public int IndexOf(OpSet item) {
      return innerList.IndexOf(item);
    }

    public void Insert(int index, OpSet item) {
      innerList.Insert(index, item);
    }

    public void RemoveAt(int index) {
      innerList.RemoveAt(index);
    }

    public OpSet this[int index] {
      get {
        return innerList[index];
      }
      set {
        innerList[index] = value;
      }
    }

    public void Add(OpSet item) {
      innerList.Add(item);
    }

    public void Clear() {
      innerList.Clear();
    }

    public bool Contains(OpSet item) {
      return innerList.Contains(item);
    }

    public void CopyTo(OpSet[] array, int arrayIndex) {
      innerList.CopyTo(array, arrayIndex);
    }

    public int Count {
      get { return innerList.Count; }
    }

    public bool IsReadOnly {
      get { return false; }
    }

    public bool Remove(OpSet item) {
      return innerList.Remove(item);
    }

    public IEnumerator<OpSet> GetEnumerator() {
      return innerList.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
      return innerList.GetEnumerator() as System.Collections.IEnumerator;
    }

    internal void Write() {
      foreach (OpSet item in innerList) {
        item.Write();
      }
    }
  }
}
