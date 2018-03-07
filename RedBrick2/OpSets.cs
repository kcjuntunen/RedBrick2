using System.Collections.Generic;
using System.IO;

using SolidWorks.Interop.sldworks;

namespace RedBrick2 {
	/// <summary>
	/// A set of op property sets.
	/// </summary>
	public class OpSets : IList<OpSet> {
		private SldWorks SwApp;
		private ModelDoc2 ActiveDoc = null;
		/// <summary>
		/// The entire PropertySet of a ModelDoc2.
		/// </summary>
		public SwProperties PropertySet;
		private string partLookup = string.Empty;
		private ENGINEERINGDataSet.CutPartOpsDataTable cpodt;
		private List<OpSet> innerList = new List<OpSet>();

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="props">A list of related routing properties.</param>
		/// <param name="partLookup">The related part.</param>
		public OpSets(SwProperties props, string partLookup) {
			SwApp = props.SwApp;
			ActiveDoc = props.ActiveDoc;
			PropertySet = props;
			this.partLookup = partLookup;
			Init();
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="props">A set of properties.</param>
		/// <param name="partLookup">The related part name.</param>
		/// <param name="cdt">Table of relate routings.</param>
		public OpSets(SwProperties props, string partLookup, ENGINEERINGDataSet.CutPartOpsDataTable cdt) {
			cpodt = cdt;
			SwApp = props.SwApp;
			ActiveDoc = props.ActiveDoc;
			PropertySet = props;
			this.partLookup = partLookup;
			Init(cpodt);
		}

		private void Init(ENGINEERINGDataSet.CutPartOpsDataTable dt) {
			int opNo = 1;
			string test = string.Empty;
			string partLookup = Path.GetFileNameWithoutExtension(this.partLookup).Split(' ')[0];
			foreach (ENGINEERINGDataSet.CutPartOpsRow row in dt.Rows) {
				string opn = string.Format(@"OP{0}", opNo);
				OpProperty op = new OpProperty(opn, true, SwApp, ActiveDoc, @"POPOP");
				string odr = string.Format(@"OP{0}ORDER", opNo);
				IntProperty oporder = new IntProperty(odr, true, SwApp, ActiveDoc, @"CUT_PART_OPS", @"POPORDER");
				string stp = string.Format(@"OP{0}SETUP", opNo);
				DoubleProperty opsetup = new DoubleProperty(stp, true, SwApp, ActiveDoc, @"CUT_PART_OPS", @"POPSETUP");
				string run = string.Format(@"OP{0}RUN", opNo++);
				DoubleProperty oprun = new DoubleProperty(run, true, SwApp, ActiveDoc, @"CUT_PART_OPS", @"POPRUN");
				op.Data = row[@"POPOP"];
				TotalSetupTime += row.POPSETUP;
				TotalRunTime += row.POPRUN;
				Add(new OpSet(PropertySet, row, op, oporder, opsetup, oprun));
				//OpControl opc = new OpControl(row, PropertySet);
				//opc.Width = flowLayoutPanel1.Width - 25;
				//flowLayoutPanel1.Controls.Add(opc);
				//flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				//opc.Anchor = AnchorStyles.Left | AnchorStyles.Right;
			}
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
				TotalSetupTime += row.POPSETUP;
				TotalRunTime += row.POPRUN;
				Add(new OpSet(PropertySet, row, op, oporder, opsetup, oprun));
				//OpControl opc = new OpControl(row, PropertySet);
				//opc.Width = flowLayoutPanel1.Width - 25;
				//flowLayoutPanel1.Controls.Add(opc);
				//flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				//opc.Anchor = AnchorStyles.Left | AnchorStyles.Right;
			}
		}

		/// <summary>
		/// Cumulative setup time.
		/// </summary>
		public double TotalSetupTime;

		/// <summary>
		/// Cumulative run time.
		/// </summary>
		public double TotalRunTime;

		/// <summary>
		/// Get index of an OpSet item.
		/// </summary>
		/// <param name="item">An Opset item.</param>
		/// <returns>An index.</returns>
		public int IndexOf(OpSet item) {
			return innerList.IndexOf(item);
		}

		/// <summary>
		/// Insert an OpSet item.
		/// </summary>
		/// <param name="index">Where to insert.</param>
		/// <param name="item">What to insert.</param>
		public void Insert(int index, OpSet item) {
			innerList.Insert(index, item);
		}

		/// <summary>
		/// Remove an item.
		/// </summary>
		/// <param name="index">Where to remove.</param>
		public void RemoveAt(int index) {
			innerList.RemoveAt(index);
		}

		/// <summary>
		/// Indexing.
		/// </summary>
		/// <param name="index">Which?</param>
		/// <returns>OpSet.</returns>
		public OpSet this[int index] {
			get {
				return innerList[index];
			}
			set {
				innerList[index] = value;
			}
		}

		/// <summary>
		/// Add an Opset item.
		/// </summary>
		/// <param name="item">An OpSet.</param>
		public void Add(OpSet item) {
			innerList.Add(item);
		}

		/// <summary>
		/// Dump the whole list of sets.
		/// </summary>
		public void Clear() {
			innerList.Clear();
		}

		/// <summary>
		/// Check for the existence of "item" in the set.
		/// </summary>
		/// <param name="item">An OpSet item.</param>
		/// <returns>True or False.</returns>
		public bool Contains(OpSet item) {
			return innerList.Contains(item);
		}

		/// <summary>
		/// Copy the sets to an array.
		/// </summary>
		/// <param name="array">An OpSet array.</param>
		/// <param name="arrayIndex">Where to start.</param>
		public void CopyTo(OpSet[] array, int arrayIndex) {
			innerList.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Number of OpSet items.
		/// </summary>
		public int Count {
			get { return innerList.Count; }
		}

		/// <summary>
		/// Never.
		/// </summary>
		public bool IsReadOnly {
			get { return false; }
		}

		/// <summary>
		/// Remove an OpSet item.
		/// </summary>
		/// <param name="item">An OpSet item.</param>
		/// <returns>True or false.</returns>
		public bool Remove(OpSet item) {
			return innerList.Remove(item);
		}

		/// <summary>
		/// Get a generic IEnumerator.
		/// </summary>
		/// <returns>A generic IEnumerator.</returns>
		public IEnumerator<OpSet> GetEnumerator() {
			return innerList.GetEnumerator();
		}

		/// <summary>
		/// Get an IEnumerator.
		/// </summary>
		/// <returns>An IEnumerator.</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return innerList.GetEnumerator() as System.Collections.IEnumerator;
		}

		/// <summary>
		/// Write the list to SolidWorks.
		/// </summary>
		internal void Write() {
			foreach (OpSet item in innerList) {
				item.Write();
			}
		}
	}
}
