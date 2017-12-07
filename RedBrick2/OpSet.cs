using System;
using System.Collections.Generic;
using System.Windows.Forms;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
	/// <summary>
	/// A set of Routing properties including runtime, setuptime, etc.
	/// </summary>
	public class OpSet : ICollection<SwProperty> {
		/// <summary>
		/// Internal dictionary of routings.
		/// </summary>
		protected Dictionary<string, SwProperty> _innerDict = new Dictionary<string, SwProperty>();

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="sw">The connected application.</param>
		/// <param name="md">The relevant ModelDoc2.</param>
		public OpSet(SldWorks sw, ModelDoc2 md) {
			SwApp = sw;
			ActiveDoc = md;
			GlobalPropertyManager = ActiveDoc.Extension.get_CustomPropertyManager(string.Empty);
			Config = (Configuration)ActiveDoc.ConfigurationManager.ActiveConfiguration;
			SpecificPropertyManager = ActiveDoc.Extension.get_CustomPropertyManager(Config.Name);
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="prps">A set of properties.</param>
		/// <param name="row">The related row.</param>
		/// <param name="op">An OpProperty.</param>
		/// <param name="ordr">A sequence number.</param>
		/// <param name="stp">Setup time.</param>
		/// <param name="run">Run time.</param>
		public OpSet(SwProperties prps,
			ENGINEERINGDataSet.CutPartOpsRow row,
			OpProperty op,
			IntProperty ordr,
			DoubleProperty stp,
			DoubleProperty run) {
			SwApp = prps.SwApp;
			ActiveDoc = prps.ActiveDoc;
			Row = row;
			Add(op);
			Add(ordr);
			Add(stp);
			Add(run);
		}

		/// <summary>
		/// Generate a new Op.
		/// </summary>
		/// <param name="opNo">The sequence number.</param>
		/// <returns>This.</returns>
		public OpSet NewOp(int opNo) {
			Order = opNo;
			OpProperty op = new OpProperty(string.Format(@"OP{0}", opNo), true, SwApp, ActiveDoc, @"POPOP");
			PartID = op.PartID;
			IntProperty oporder = new IntProperty(string.Format(@"OP{0}ORDER", opNo), true, SwApp, ActiveDoc, @"CUT_PART_OPS", @"POPORDER");
			DoubleProperty opsetup = new DoubleProperty(string.Format(@"OP{0}SETUP", opNo), true, SwApp, ActiveDoc, @"CUT_PART_OPS", @"POPSETUP");
			DoubleProperty oprun = new DoubleProperty(string.Format(@"OP{0}RUN", opNo), true, SwApp, ActiveDoc, @"CUT_PART_OPS", @"POPRUN");
			Add(op);
			Add(oporder);
			Add(opsetup);
			Add(oprun);
			return this;
		}

		/// <summary>
		/// Write all properties in inner list to ModelDoc2.
		/// </summary>
		public void Write() {
			foreach (SwProperty item in _innerDict.Values) {
				item.Write();
			}
		}

		/// <summary>
		/// Remove an item from the dictionary.
		/// </summary>
		public void Delete() {
			foreach (SwProperty item in _innerDict.Values) {
				item.Delete();
			}
		}

		/// <summary>
		/// Collect routing properties.
		/// </summary>
		/// <param name="opNo">Sequence number.</param>
		/// <returns>This.</returns>
		public OpSet Get(int opNo) {
			OpProperty op = new OpProperty(string.Format(@"OP{0}", opNo), true, SwApp, ActiveDoc, @"POPOP");
			PartID = op.PartID;
			IntProperty oporder = new IntProperty(string.Format(@"OP{0}ORDER", opNo), true, SwApp, ActiveDoc, @"CUT_PART_OPS", @"POPORDER");
			DoubleProperty opsetup = new DoubleProperty(string.Format(@"OP{0}SETUP", opNo), true, SwApp, ActiveDoc, @"CUT_PART_OPS", @"POPSETUP");
			DoubleProperty oprun = new DoubleProperty(string.Format(@"OP{0}RUN", opNo), true, SwApp, ActiveDoc, @"CUT_PART_OPS", @"POPRUN");
			return this;
		}

		/// <summary>
		/// Add a routing property to the set.
		/// </summary>
		/// <param name="item"></param>
		public void Add(SwProperty item) {
			if (!_innerDict.ContainsKey(item.Name)) {
				_innerDict.Add(item.Name, item);
			}
		}

		/// <summary>
		/// Dump all set properties.
		/// </summary>
		public void Clear() {
			_innerDict.Clear();
		}

		/// <summary>
		/// Indicate whether the item is in the set.
		/// </summary>
		/// <param name="item">The SwProperty we're curious about.</param>
		/// <returns>True or false.</returns>
		public bool Contains(SwProperty item) {
			return _innerDict.ContainsKey(item.Name);
		}

		/// <summary>
		/// Copy into an array.
		/// </summary>
		/// <param name="array">Target array.</param>
		/// <param name="arrayIndex">Start here.</param>
		public void CopyTo(SwProperty[] array, int arrayIndex) {
			for (int i = arrayIndex; i < array.Length; i++) {
				array[i] = PropertyList[i];
			}
		}

		/// <summary>
		/// Number of items inthis list.
		/// </summary>
		public int Count {
			get { return _innerDict.Count; }
		}

		/// <summary>
		/// False. It's never read only.
		/// </summary>
		public bool IsReadOnly {
			get { return false; }
		}

		/// <summary>
		/// Remove an item.
		/// </summary>
		/// <param name="item">SwProperty to remove.</param>
		/// <returns>Bool.</returns>
		public bool Remove(SwProperty item) {
			return _innerDict.Remove(item.Name);
		}

		/// <summary>
		/// Get a generic IEnumerator.
		/// </summary>
		/// <returns>An IEnumerator.</returns>
		public IEnumerator<SwProperty> GetEnumerator() {
			return PropertyList.GetEnumerator();
		}

		/// <summary>
		/// Get an IEnumerator.
		/// </summary>
		/// <returns>IEnumerator.</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return _innerDict.GetEnumerator();
		}

		/// <summary>
		/// Return a generic List of SwProperties.
		/// </summary>
		public List<SwProperty> PropertyList {
			get {
				List<SwProperty> lst = new List<SwProperty>();
				foreach (KeyValuePair<string, SwProperty> item in _innerDict) {
					lst.Add(item.Value);
				}
				return lst;
			}
			private set { PropertyList = value; }
		}

		/// <summary>
		/// The relevant Configuration.
		/// </summary>
		public Configuration Config { get; set; }
		/// <summary>
		/// A global property manager.
		/// </summary>
		public CustomPropertyManager GlobalPropertyManager { get; set; }
		/// <summary>
		/// A property manager connected to the relevant Configuration.
		/// </summary>
		public CustomPropertyManager SpecificPropertyManager { get; set; }
		/// <summary>
		/// The ID of the part we're looking at.
		/// </summary>
		public int PartID { get; set; }
		/// <summary>
		/// Our row in the db.
		/// </summary>
		public ENGINEERINGDataSet.CutPartOpsRow Row { get; set; }
		/// <summary>
		/// The whole propertyset.
		/// </summary>
		public SwProperties PropertySet { get; set; }

		private OpControl opControl = null;
		/// <summary>
		/// A generated OpControl based on the info we have.
		/// </summary>
		public OpControl OperationControl {
			get {
				if (opControl == null) {
					opControl = new OpControl(Row, this);

				}
				return opControl;
			}
			set {
				opControl = value;
			}
		}

		private int _order = 0;
		/// <summary>
		/// The sequence number.
		/// </summary>
		public int Order {
			get { return _order; }
			set {
				_order = value;
				string propName = string.Format(@"OP{0}ORDER", _order);
				if (_innerDict.ContainsKey(propName)) {
					_innerDict[propName].Value = _order.ToString();
				}
			}
		}

		private int _op = 0;
		/// <summary>
		/// The property name of our OP.
		/// </summary>
		public int Op {
			get { return _op; }
			set {
				_op = value;
				string propName = string.Format(@"OP{0}", Order);
				if (_innerDict.ContainsKey(propName)) {
					_innerDict[propName].Value = _op.ToString();
				}
			}
		}

		private double _setupTime = 0.0F;
		/// <summary>
		/// Setup time.
		/// </summary>
		public double SetupTime {
			get { return _setupTime; }
			set {
				_setupTime = value;
				string propName = string.Format(@"OP{0}SETUP", Order);
				if (_innerDict.ContainsKey(propName)) {
					_innerDict[propName].Value = _setupTime.ToString();
				}
			}
		}

		private double _runTime = 0.0F;
		/// <summary>
		/// Run time.
		/// </summary>
		public double RunTime {
			get {
				return _runTime;
			}
			set {
				_runTime = value;
				string propName = string.Format(@"OP{0}RUN", Order);
				if (_innerDict.ContainsKey(propName)) {
					_innerDict[propName].Value = _runTime.ToString();
				}
			}
		}

		/// <summary>
		/// Relevant ModelDoc2.
		/// </summary>
		public ModelDoc2 ActiveDoc { get; set; }
		/// <summary>
		/// The connected application.
		/// </summary>
		public SldWorks SwApp { get; set; }
	}
}
