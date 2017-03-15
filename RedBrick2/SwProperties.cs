using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using SolidWorks.Interop.sldworks;

namespace RedBrick2 {
  public class SwProperties {
    private ENGINEERINGDataSet.CUT_PARTSDataTable cpdt = new ENGINEERINGDataSet.CUT_PARTSDataTable();
    private ENGINEERINGDataSet.CUT_CUTLIST_PARTSDataTable ccpdt = new ENGINEERINGDataSet.CUT_CUTLIST_PARTSDataTable();
    private Dictionary<string, SwProperty> _innerDict = new Dictionary<string, SwProperty>();
    private int globalCount = 0;
    private int totalCount = 0;
    private int nonGlobalCount = 0;

    public SwProperties(SldWorks sw) {
      SwApp = sw;
    }

    public bool AddProperty(SwProperty property) {
      AddResult = false;
      try {
        _innerDict.Add(property.Name, property);
        if (property.Global) {
          globalCount++;
        } else {
          nonGlobalCount++;
        }
        totalCount++;
        AddResult = true;
      } catch (Exception e) {
        AddException = e;
      }
      return AddResult;
    }

    public bool AddPropertyRange(List<SwProperty> prps) {
      AddResult = false;
      foreach (SwProperty item in prps) {
        AddResult |= AddProperty(item);
      }
      return AddResult;
    }

    public SwProperty GetProperty(string name) {
      if (Contains(name)) {
        return _innerDict[name];
      } else {
        return null;
      }
    }

    public SwProperty GetProperty(string name, bool addIfNotExists) {
      if (Contains(name)) {
        return _innerDict[name];
      } else if (addIfNotExists) {
        throw new NotImplementedException(@"Cannot create a default SWProperty.");
      } else {
        return null;
      }
    }

    public int Write() {
      int res = 1;
      foreach (KeyValuePair<string, SwProperty> item in _innerDict) {
        item.Value.Write();
        res &= (int)item.Value.WriteResult;
      }
      return res;
    }

    public void GetProperties(Component2 comp) {
      GetProperties(comp.GetModelDoc2());
    }

    public void GetProperties(ModelDoc2 md) {
      //IntProperty crc32 = new IntProperty(@"CRC32", true, SwApp, md, @"CUT_PARTS", @"HASH");
      //crc32.Data = crc32.Hash;
      StringProperty department = new StringProperty(@"DEPARTMENT", true, SwApp, md, @"TYPE");
      IntProperty blankQty = new IntProperty(@"BLANK QTY", true, SwApp, md, @"CUT_PARTS", @"BLANKQTY");

      StringProperty material = new StringProperty(@"MATERIAL", true, SwApp, md, string.Empty);
      StringProperty weight = new StringProperty(@"WEIGHT", true, SwApp, md, string.Empty);
      StringProperty volume = new StringProperty(@"VOLUME", true, SwApp, md, string.Empty);
      StringProperty description = new StringProperty(@"Description", true, SwApp, md, @"DESCR");
      StringProperty comment = new StringProperty(@"COMMENT", true, SwApp, md, @"COMMENT");
      StringProperty cnc1 = new StringProperty(@"CNC1", true, SwApp, md, @"CNC1");
      StringProperty cnc2 = new StringProperty(@"CNC2", true, SwApp, md, @"CNC2");

      BooleanProperty includeInCutlist = new BooleanProperty(@"INCLUDE IN CUTLIST", false, SwApp, md, string.Empty);
      BooleanProperty updateCNC = new BooleanProperty(@"UPDATE CNC", true, SwApp, md, "UPDATE_CNC");

      DimensionProperty length = new DimensionProperty(@"LENGTH", true, SwApp, md, @"FIN_L");
      DimensionProperty width = new DimensionProperty(@"WIDTH", true, SwApp, md, @"FIN_W");
      DimensionProperty thickness = new DimensionProperty(@"THICKNESS", true, SwApp, md, @"THICKNESS");
      DimensionProperty wallThickness = new DimensionProperty(@"WALL THICKNESS", true, SwApp, md, string.Empty);
      DimensionProperty overL = new DimensionProperty(@"OVERL", true, SwApp, md, @"OVER_L");
      DimensionProperty overW = new DimensionProperty(@"OVERW", true, SwApp, md, @"OVER_W");

      MaterialProperty cutlistMaterial = new MaterialProperty(@"CUTLIST MATERIAL", false, SwApp, md, @"MATID");
      EdgeProperty edgelf = new EdgeProperty(@"EDGE FRONT (L)", false, SwApp, md, @"EDGEID_LF");
      EdgeProperty edgelb = new EdgeProperty(@"EDGE BACK (L)", false, SwApp, md, @"EDGEID_LB");
      EdgeProperty edgewr = new EdgeProperty(@"EDGE RIGHT (W)", false, SwApp, md, @"EDGEID_WR");
      EdgeProperty edgewl = new EdgeProperty(@"EDGE LEFT (W)", false, SwApp, md, @"EDGEID_WL");
      foreach (SwProperty item in new SwProperty [] {
        department, blankQty,
        material, weight, volume, description, comment, cnc1, cnc2,
        includeInCutlist, updateCNC,
        length, width, thickness, wallThickness, overL, overW,
        cutlistMaterial, edgelf, edgelb, edgewr, edgewl
      }) {
        item.Get();
        AddProperty(item);
      }
    }

    public bool Contains(string name) {
      return _innerDict.ContainsKey(name);
    }

    public void Clear() {
      _innerDict.Clear();
    }

    public ENGINEERINGDataSet.CUT_PARTSRow PartsData {
      get {
        ENGINEERINGDataSet.CUT_PARTSRow cpr = cpdt.NewCUT_PARTSRow();
        foreach (var item in _innerDict) {
          if (item.Value.TableName == @"CUT_PARTS") {
            cpr[item.Value.FieldName] = item.Value.Data;
          }
        }
        return cpr;
      }
    }

    public ENGINEERINGDataSet.CUT_CUTLIST_PARTSRow CutlistPartsData {
      get {
        ENGINEERINGDataSet.CUT_CUTLIST_PARTSRow cpr = ccpdt.NewCUT_CUTLIST_PARTSRow();
        cpr[@"CLID"] = CutlistID;
        cpr[@"PARTID"] = (int)_innerDict[@"CRC32"].PartID;
        foreach (var item in _innerDict) {
          if (item.Value.TableName == @"CUT_CUTLIST_PARTS") {
            cpr[item.Value.FieldName] = item.Value.Data;
          }
        }
        cpr[@"QTY"] = CutlistQty;
        return cpr;
      }
    }

    public ENGINEERINGDataSet.CUT_PART_OPSRow[] PartOpsRows {
      get {
        ENGINEERINGDataSet.CUT_PART_OPSDataTable cpodt =
          new ENGINEERINGDataSet.CUT_PART_OPSDataTable();
        List<ENGINEERINGDataSet.CUT_PART_OPSRow> rows =
          new List<ENGINEERINGDataSet.CUT_PART_OPSRow>();
        foreach (OpSet opset in OpSets) {
          ENGINEERINGDataSet.CUT_PART_OPSRow row = cpodt.NewCUT_PART_OPSRow();
          foreach (OpProperty op in opset) {
            row[op.FieldName] = op.Data;
          }
          rows.Add(row);
        }
        return rows.ToArray();
      }
    }

    public string GlobalTokenString {
      get {
        string result = string.Empty;
        int count = 0;
        foreach (KeyValuePair<string, SwProperty> item in _innerDict) {
          if (item.Value.Global) {
            result += string.Format(@"{0}={1}", item.Key, item.Value.Data.ToString());
            if (count++ < GlobalCount) {
              result += @"&";
            }
          }
        }
        return result;
      }
    }

    public string SpecificTokenString {
      get {
        string result = string.Empty;
        int count = 0;
        foreach (KeyValuePair<string, SwProperty> item in _innerDict) {
          if (!item.Value.Global) {
            result += string.Format(@"{0}={1}", item.Key, item.Value.Data.ToString());
            if (count++ < NonGlobalCount) {
              result += @"&";
            }
          }
        }
        return result;
      }
    }

    public int CutlistID { get; set; }

    public int CutlistQty {
      get {
        int res = 0;

        return res;
      }
    }

    public List<OpSet> OpSets { get; set; }

    public int GlobalCount {
      get { return globalCount; }
    }

    public int NonGlobalCount {
      get { return nonGlobalCount; }
    }

    public int TotalCount {
      get { return totalCount; }
    }

    public bool AddResult { get; private set; }
    public Exception AddException { get; private set; }
    public SldWorks SwApp { get; private set; }
  }
}
