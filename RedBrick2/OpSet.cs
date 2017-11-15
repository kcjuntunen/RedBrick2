﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
	public class OpSet : ICollection<SwProperty> {
		protected Dictionary<string, SwProperty> _innerDict = new Dictionary<string, SwProperty>();

		public OpSet(SldWorks sw, ModelDoc2 md) {
			SwApp = sw;
			ActiveDoc = md;
			GlobalPropertyManager = ActiveDoc.Extension.get_CustomPropertyManager(string.Empty);
			Config = (Configuration)ActiveDoc.ConfigurationManager.ActiveConfiguration;
			SpecificPropertyManager = ActiveDoc.Extension.get_CustomPropertyManager(Config.Name);
		}

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

		public void Write() {
			foreach (SwProperty item in _innerDict.Values) {
				item.Write();
			}
		}

		public void Delete() {
			foreach (SwProperty item in _innerDict.Values) {
				item.Delete();
			}
		}

		public OpSet Get(int opNo) {
			OpProperty op = new OpProperty(string.Format(@"OP{0}", opNo), true, SwApp, ActiveDoc, @"POPOP");
			PartID = op.PartID;
			IntProperty oporder = new IntProperty(string.Format(@"OP{0}ORDER", opNo), true, SwApp, ActiveDoc, @"CUT_PART_OPS", @"POPORDER");
			DoubleProperty opsetup = new DoubleProperty(string.Format(@"OP{0}SETUP", opNo), true, SwApp, ActiveDoc, @"CUT_PART_OPS", @"POPSETUP");
			DoubleProperty oprun = new DoubleProperty(string.Format(@"OP{0}RUN", opNo), true, SwApp, ActiveDoc, @"CUT_PART_OPS", @"POPRUN");
			return this;
		}

		public void Add(SwProperty item) {
			if (!_innerDict.ContainsKey(item.Name)) {
				_innerDict.Add(item.Name, item);
			}
		}

		public void Clear() {
			_innerDict.Clear();
		}

		public bool Contains(SwProperty item) {
			return _innerDict.ContainsKey(item.Name);
		}

		public void CopyTo(SwProperty[] array, int arrayIndex) {
			for (int i = arrayIndex; i < array.Length; i++) {
				array[i] = PropertyList[i];
			}
		}

		public int Count {
			get { return _innerDict.Count; }
		}

		public bool IsReadOnly {
			get { return false; }
		}

		public bool Remove(SwProperty item) {
			return _innerDict.Remove(item.Name);
		}

		public IEnumerator<SwProperty> GetEnumerator() {
			return PropertyList.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return _innerDict.GetEnumerator();
		}

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

		public Configuration Config { get; set; }
		public CustomPropertyManager GlobalPropertyManager { get; set; }
		public CustomPropertyManager SpecificPropertyManager { get; set; }
		public int PartID { get; set; }
		public ENGINEERINGDataSet.CutPartOpsRow Row { get; set; }
		public SwProperties PropertySet { get; set; }

		private OpControl opControl = null;
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

		public ModelDoc2 ActiveDoc { get; set; }
		public SldWorks SwApp { get; set; }
	}
}
