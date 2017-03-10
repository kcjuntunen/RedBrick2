using System;
using System.Collections.Generic;

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

    public OpSet Create(int opNo) {
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
      foreach (KeyValuePair<string, SwProperty> item in _innerDict) {
        item.Value.Write();
      }
    }

    public OpSet Get(int opNo) {
      OpProperty op = new OpProperty(string.Format(@"OP{0}", opNo), true, SwApp, ActiveDoc, @"POPOP");
      PartID = op.PartID;
      IntProperty oporder = new IntProperty(string.Format(@"OP{0}ORDER", opNo), true, SwApp, ActiveDoc, @"CUT_PART_OPS", @"POPORDER");
      IntProperty opsetup = new IntProperty(string.Format(@"OP{0}SETUP", opNo), true, SwApp, ActiveDoc, @"CUT_PART_OPS", @"POPSETUP");
      IntProperty oprun = new IntProperty(string.Format(@"OP{0}RUN", opNo), true, SwApp, ActiveDoc, @"CUT_PART_OPS", @"POPRUN");
      return this;
    }

    public void GetOps() {
      int res = 0;
      object propNames = null;
      object propTypes = null;
      object propValues = null;
      object resolved = null;
      res = GlobalPropertyManager.GetAll2(ref propNames, ref propTypes, ref propValues, ref resolved);
      foreach (var item in (propNames as string[])) {

      }
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

    public int Order {
      get { return Order; }
      set {
        Order = value;
        _innerDict[string.Format(@"OP{0}ORDER", Order)].Value = Order.ToString();
      }
    }

    public int Op {
      get { return Op; }
      set {
        Op = value;
        _innerDict[string.Format(@"OP{0}", Order)].Value = Op.ToString();
      }
    }

    public int SetupTime {
      get { return SetupTime; }
      set {
        SetupTime = value;
        _innerDict[string.Format(@"OP{0}SETUP", Order)].Value = SetupTime.ToString();
      }
    }

    public int RunTime { get; set; }
    public ModelDoc2 ActiveDoc { get; set; }
    public SldWorks SwApp { get; set; }
  }
}
