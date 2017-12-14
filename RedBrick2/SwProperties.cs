using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using System.Web;

using SolidWorks.Interop.sldworks;

namespace RedBrick2 {
	/// <summary>
	/// A set of properties associated with a model.
	/// </summary>
	public class SwProperties : IDictionary<string, SwProperty> {
		private ENGINEERINGDataSet.CUT_PARTSDataTable cpdt = new ENGINEERINGDataSet.CUT_PARTSDataTable();
		private ENGINEERINGDataSet.CUT_CUTLIST_PARTSDataTable ccpdt = new ENGINEERINGDataSet.CUT_CUTLIST_PARTSDataTable();
		private ENGINEERINGDataSet.CUT_PART_OPSDataTable cpodt = new ENGINEERINGDataSet.CUT_PART_OPSDataTable();
		private Dictionary<string, SwProperty> _innerDict = new Dictionary<string, SwProperty>();
		private int globalCount = 0;
		private int totalCount = 0;
		private int nonGlobalCount = 0;
		/// <summary>
		/// The relevant configuration, should this property not be global.
		/// </summary>
		public string Configuration = string.Empty;

		/// <summary>
		/// Constructor for a new <see cref="RedBrick2.SwProperties"/> object.
		/// </summary>
		/// <param name="sw">A running <see cref="SolidWorks.Interop.sldworks.SldWorks"/> object.</param>
		public SwProperties(SldWorks sw) {
			SwApp = sw;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="sw">A running <see cref="SolidWorks.Interop.sldworks.SldWorks"/> object.</param>
		/// <param name="md">The relevant <see cref="SolidWorks.Interop.sldworks.ModelDoc2"/> object.</param>
		public SwProperties(SldWorks sw, ModelDoc2 md)
			: this(sw) {
			ActiveDoc = md;
		}

		/// <summary>
		/// Returns the named property, but returns null if it's not present.
		/// </summary>
		/// <param name="name">the name of the desired property.</param>
		/// <returns>a <see cref="RedBrick2.SwProperty"/>.</returns>
		public SwProperty GetProperty(string name) {
			if (Contains(name)) {
				return _innerDict[name];
			} else {
				return null;
			}
		}

		/// <summary>
		/// Returns the named property, but maybe creates it and returns the new one.
		/// </summary>
		/// <remarks>The add behavior isn't implemented.</remarks>
		/// <param name="name">the name of the desired property.</param>
		/// <param name="addIfNotExists">add or don't if the property isn't in the set.</param>
		/// <returns></returns>
		public SwProperty GetProperty(string name, bool addIfNotExists) {
			if (Contains(name)) {
				return _innerDict[name];
			} else if (addIfNotExists) {
				throw new NotImplementedException(@"Cannot create a default SWProperty.");
			} else {
				return null;
			}
		}

		/// <summary>
		/// Write the whole set to a model.
		/// </summary>
		/// <returns>A <see cref="SolidWorks.Interop.swconst.swCustomInfoAddResult_e"/></returns>
		public int Write() {
			int res = 1;
			foreach (KeyValuePair<string, SwProperty> item in _innerDict) {
				item.Value.Write();
				res &= (int)item.Value.WriteResult;
			}
			//if (opSets != null) {
			//  foreach (OpSet _opSet in opSets) {
			//    _opSet.Write();
			//  }
			//}
			return res;
		}

		/// <summary>
		/// Pull down all pertinent data from a model.
		/// </summary>
		/// <param name="comp">A <see cref="SolidWorks.Interop.sldworks.Component2"/> from which we'll
		/// get a <see cref="SolidWorks.Interop.sldworks.ModelDoc2"/>.</param>
		public void GetProperties(Component2 comp) {
			Configuration = comp.ReferencedConfiguration;
			GetProperties((ModelDoc2)comp.GetModelDoc2());
		}

		/// <summary>
		/// Pull down all pertinent data from a model.
		/// </summary>
		/// <param name="md">A <see cref="SolidWorks.Interop.sldworks.ModelDoc2"/>.</param>
		public void GetProperties(ModelDoc2 md) {
			ActiveDoc = md;
			DeptId deptid = new DeptId(@"DEPTID", true, SwApp, md, @"TYPE");
			deptid.ToDB = false;

			DepartmentProperty department = new DepartmentProperty(@"DEPARTMENT", true, SwApp, md, @"DEPTID");
			IntProperty blankQty = new IntProperty(@"BLANK QTY", true, SwApp, md, @"CUT_PARTS", @"BLANKQTY");

			StringProperty material = new StringProperty(@"MATERIAL", true, SwApp, md, string.Empty);
			material.ToDB = false;
			StringProperty weight = new StringProperty(@"WEIGHT", true, SwApp, md, string.Empty);
			weight.ToDB = false;

			StringProperty volume = new StringProperty(@"VOLUME", true, SwApp, md, string.Empty);
			volume.ToDB = false;

			StringProperty description = new StringProperty(@"Description", true, SwApp, md, @"DESCR");
			StringProperty comment = new StringProperty(@"COMMENT", true, SwApp, md, @"COMMENT");
			StringProperty cnc1 = new StringProperty(@"CNC1", true, SwApp, md, @"CNC1");
			StringProperty cnc2 = new StringProperty(@"CNC2", true, SwApp, md, @"CNC2");

			BooleanProperty includeInCutlist = new BooleanProperty(@"INCLUDE IN CUTLIST", false, SwApp, md, string.Empty);
			includeInCutlist.ToDB = false;

			BooleanProperty updateCNC = new BooleanProperty(@"UPDATE CNC", true, SwApp, md, "UPDATE_CNC");

			DimensionProperty length = new DimensionProperty(@"LENGTH", true, SwApp, md, @"FIN_L");
			DimensionProperty width = new DimensionProperty(@"WIDTH", true, SwApp, md, @"FIN_W");
			DimensionProperty thickness = new DimensionProperty(@"THICKNESS", true, SwApp, md, @"THICKNESS");
			DimensionProperty wallThickness = new DimensionProperty(@"WALL THICKNESS", true, SwApp, md, string.Empty);
			wallThickness.ToDB = false;

			DoubleProperty overL = new DoubleProperty(@"OVERL", true, SwApp, md, @"CUT_PARTS", @"OVER_L");
			DoubleProperty overW = new DoubleProperty(@"OVERW", true, SwApp, md, @"CUT_PARTS", @"OVER_W");

			OpProperty[] ops_ = new OpProperty[Properties.Settings.Default.OpCount];
			OpId[] opids_ = new OpId[Properties.Settings.Default.OpCount];
			for (int i = 0; i < Properties.Settings.Default.OpCount; i++) {
				string op_ = string.Format(@"OP{0}", i + 1);
				string opid_ = string.Format(@"OP{0}ID", i + 1);
				ops_[i] = new OpProperty(op_, true, SwApp, md, opid_);
				opids_[i] = new OpId(opid_, true, SwApp, md, @"POPOP");
			}

			//OpProperty op1 = new OpProperty(@"OP1", true, SwApp, md, @"OP1ID");
			//OpProperty op2 = new OpProperty(@"OP2", true, SwApp, md, @"OP2ID");
			//OpProperty op3 = new OpProperty(@"OP3", true, SwApp, md, @"OP3ID");
			//OpProperty op4 = new OpProperty(@"OP4", true, SwApp, md, @"OP4ID");
			//OpProperty op5 = new OpProperty(@"OP5", true, SwApp, md, @"OP5ID");

			//OpId op1id = new OpId(@"OP1ID", true, SwApp, md, @"POPOP");
			//OpId op2id = new OpId(@"OP2ID", true, SwApp, md, @"POPOP");
			//OpId op3id = new OpId(@"OP3ID", true, SwApp, md, @"POPOP");
			//OpId op4id = new OpId(@"OP4ID", true, SwApp, md, @"POPOP");
			//OpId op5id = new OpId(@"OP5ID", true, SwApp, md, @"POPOP");

			MaterialProperty cutlistMaterial = new MaterialProperty(@"CUTLIST MATERIAL", false, SwApp, md, @"MATID");
			EdgeProperty edgelf = new EdgeProperty(@"EDGE FRONT (L)", false, SwApp, md, @"EDGEID_LF");
			EdgeProperty edgelb = new EdgeProperty(@"EDGE BACK (L)", false, SwApp, md, @"EDGEID_LB");
			EdgeProperty edgewr = new EdgeProperty(@"EDGE RIGHT (W)", false, SwApp, md, @"EDGEID_WR");
			EdgeProperty edgewl = new EdgeProperty(@"EDGE LEFT (W)", false, SwApp, md, @"EDGEID_WL");

			MatId matid = new MatId("MATID", false, SwApp, md, @"MATID");
			matid.ToDB = false;

			EdgeId efid = new EdgeId(@"EFID", false, SwApp, md, @"EDGEID_LF");
			efid.ToDB = false;

			EdgeId ebid = new EdgeId(@"EBID", false, SwApp, md, @"EDGEID_LB");
			ebid.ToDB = false;

			EdgeId erid = new EdgeId(@"ERID", false, SwApp, md, @"EDGEID_WR");
			erid.ToDB = false;

			EdgeId elid = new EdgeId(@"ELID", false, SwApp, md, @"EDGEID_WL");
			elid.ToDB = false;


			foreach (SwProperty item in new SwProperty[] {
        deptid, department, blankQty,
        material, weight, volume, description, comment, cnc1, cnc2,
        includeInCutlist, updateCNC,
        length, width, thickness, wallThickness, overL, overW,
				//op1, op2, op3, op4, op5,
				//op1id, op2id, op3id, op4id, op5id,
        cutlistMaterial, edgelf, edgelb, edgewr, edgewl,
        matid, efid, ebid, erid, elid
      }) {
				item.Configuration = Configuration;
				item.Get();
				Add(item);
			}
			foreach (SwProperty p_ in ops_) {
				p_.Configuration = Configuration;
				p_.Get();
				Add(p_);
			}
			foreach (SwProperty p_ in opids_) {
				p_.Configuration = Configuration;
				p_.Get();
				Add(p_);
			}
			PartFileInfo = _innerDict[@"DEPARTMENT"].PartFileInfo;
		}

		#region inherited
		/// <summary>
		/// Add a new property to a set of properties.
		/// </summary>
		/// <remarks>This is here to fully implement IDictionary.</remarks>
		/// <param name="pair">A KeyValuePair.</param>
		public void Add(KeyValuePair<string, SwProperty> pair) {
			Add(pair.Value);
		}

		/// <summary>
		/// Add a new property to a set of properties.
		/// </summary>
		/// <remarks>This is here to fully implement IDictionary.</remarks>
		/// <param name="name">Name of property.</param>
		/// <param name="value">An SwProperty.</param>
		public void Add(string name, SwProperty value) {
			Add(new KeyValuePair<string, SwProperty>(name, value));
		}

		/// <summary>
		/// Add a new property to a set of properties.
		/// </summary>
		/// <param name="property">The SwProperty to be added.</param>
		public void Add(SwProperty property) {
			try {
				_innerDict.Add(property.Name, property);
				if (!property.DoNotWrite) {
					if (property.Global) {
						globalCount++;
					} else {
						nonGlobalCount++;
					}
					totalCount++;
				}
			} catch (Exception e) {
				AddException = e;
			}
		}

		/// <summary>
		/// Add a list of properties to a set of properties.
		/// </summary>
		/// <param name="prps">A List of properties to be added.</param>
		public void AddPropertyRange(List<SwProperty> prps) {
			foreach (SwProperty item in prps) {
				Add(item);
			}
		}

		/// <summary>
		/// Check if a key is in the set of properties.
		/// </summary>
		/// <param name="pair">The Key, Value pair you're looking for.</param>
		/// <returns>a <see cref="System.Boolean"/>, of course.</returns>
		public bool Contains(KeyValuePair<string, SwProperty> pair) {
			return _innerDict.ContainsKey(pair.Key);
		}

		/// <summary>
		/// Check if a key is in the set of properties.
		/// </summary>
		/// <param name="name">the name of the key you're looking for.</param>
		/// <returns>a <see cref="System.Boolean"/>, of course.</returns>
		public bool Contains(string name) {
			return _innerDict.ContainsKey(name);
		}

		/// <summary>
		/// Empty out the set.
		/// </summary>
		public void Clear() {
			_innerDict.Clear();
		}

		/// <summary>
		/// Returns the full count of items in the set.
		/// </summary>
		public int Count {
			get { return _innerDict.Count; }
		}

		/// <summary>
		/// Remove an item based on a pair.
		/// </summary>
		/// <param name="pair">The pair we want to pitch.</param>
		/// <returns>True or false.</returns>
		public bool Remove(KeyValuePair<string, SwProperty> pair) {
			return _innerDict.Remove(pair.Key);
		}

		/// <summary>
		/// Remove an item.
		/// </summary>
		/// <param name="item">The whole property you're looking to remove. 
		/// It really only needs to have the same name the property you want to remove.</param>
		/// <returns>sucess or failure.</returns>
		public bool Remove(SwProperty item) {
			return _innerDict.Remove(item.Name);
		}

		/// <summary>
		/// Remove an item.
		/// </summary>
		/// <param name="name">The name of the property you're looking to remove.</param>
		/// <returns>sucess or failure.</returns>
		public bool Remove(string name) {
			return _innerDict.Remove(name);
		}

		/// <summary>
		/// Query set of properties for a particular property by name.
		/// </summary>
		/// <param name="key">The name of the target property.</param>
		/// <returns>True or false.</returns>
		public bool ContainsKey(string key) {
			return _innerDict.ContainsKey(key);
		}

		/// <summary>
		/// A collection of all the names in the set.
		/// </summary>
		public ICollection<string> Keys {
			get { return _innerDict.Keys; }
		}

		/// <summary>
		/// Attempt to get a Property.
		/// </summary>
		/// <param name="key">The name you're looking for.</param>
		/// <param name="value">An object to be populated.</param>
		/// <returns>Success or failure.</returns>
		public bool TryGetValue(string key, out SwProperty value) {
			return _innerDict.TryGetValue(key, out value);
		}

		/// <summary>
		/// A collection of all the <see cref="RedBrick2.SwProperty"/>s in the set.
		/// </summary>
		public ICollection<SwProperty> Values {
			get { return _innerDict.Values; }
		}

		/// <summary>
		/// Get or set SwProperty by direct reference.
		/// </summary>
		/// <param name="key">The name of the target property.</param>
		/// <returns>A <see cref="RedBrick2.SwProperty"/>.</returns>
		public SwProperty this[string key] {
			get {
				return _innerDict[key];
			}
			set {
				_innerDict[key] = value;
			}
		}

		/// <summary>
		/// Not implemented yet.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		public void CopyTo(KeyValuePair<string, SwProperty>[] array, int arrayIndex) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// Returns stuff so we can do foreach loops.
		/// </summary>
		/// <returns>An IEnumerator.</returns>
		public IEnumerator<KeyValuePair<string, SwProperty>> GetEnumerator() {
			return _innerDict.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return _innerDict.GetEnumerator();
		}

		/// <summary>
		/// Never!
		/// </summary>
		public bool IsReadOnly {
			get { return false; }
			set { ; }
		}

		#endregion
		#region properties
		/// <summary>
		/// Generate a DataRow of part data from a property set.
		/// </summary>
		public ENGINEERINGDataSet.CUT_PARTSRow PartsData {
			get {
				if (cpdt.Rows.Count > 0) {
					return cpdt.Rows[0] as ENGINEERINGDataSet.CUT_PARTSRow;
				}

				System.IO.FileInfo fi_ = _innerDict[@"DEPARTMENT"].PartFileInfo;
				string lookup_ = Redbrick.FileInfoToLookup(fi_);
				ENGINEERINGDataSetTableAdapters.CUT_PARTSTableAdapter cpta =
					new ENGINEERINGDataSetTableAdapters.CUT_PARTSTableAdapter();
				cpdt = cpta.GetDataByPartnum(lookup_);
				if (cpdt.Rows.Count > 0) {
					return cpdt.Rows[0] as ENGINEERINGDataSet.CUT_PARTSRow;
				}

				ENGINEERINGDataSet.CUT_PARTSRow cpr = cpdt.NewCUT_PARTSRow();
				cpr.PARTID = (int)_innerDict[@"DEPARTMENT"].PartID;
				cpr.PARTNUM = lookup_;
				cpr.HASH = Redbrick.GetHash(fi_);
				foreach (var item in _innerDict) {
					if (item.Value.TableName == @"CUT_PARTS" &&
						item.Value.FieldName != string.Empty &&
						item.Value.ToDB) {
						cpr[item.Value.FieldName] = item.Value.Data;
					}
				}
				return cpr;
			}
		}

		/// <summary>
		/// Generate a DataRow of cutlist-part data from a property set.
		/// </summary>
		public ENGINEERINGDataSet.CUT_CUTLIST_PARTSRow CutlistPartsData {
			get {
				if (ccpdt.Rows.Count > 0) {
					return ccpdt.Rows[0] as ENGINEERINGDataSet.CUT_CUTLIST_PARTSRow;
				}
				System.IO.FileInfo fi_ = _innerDict[@"DEPARTMENT"].PartFileInfo;
				string lookup_ = Redbrick.FileInfoToLookup(fi_);
				if (CutlistID > 0 && PartID > 0) {
					ENGINEERINGDataSetTableAdapters.CUT_CUTLIST_PARTSTableAdapter ccpta =
						new ENGINEERINGDataSetTableAdapters.CUT_CUTLIST_PARTSTableAdapter();
					ccpdt = ccpta.GetDataByCutlistIDAndPartID(PartID, CutlistID);
					if (ccpdt.Rows.Count > 0) {
						return ccpdt.Rows[0] as ENGINEERINGDataSet.CUT_CUTLIST_PARTSRow;
					}
				}

				ENGINEERINGDataSet.CUT_CUTLIST_PARTSRow cpr = ccpdt.NewCUT_CUTLIST_PARTSRow();
				cpr.CLID = CutlistID;
				cpr.PARTID = PartID;
				foreach (var item in _innerDict) {
					if (item.Value.TableName == @"CUT_CUTLIST_PARTS" && item.Value.ToDB) {
						cpr[item.Value.FieldName] = item.Value.Data;
					}
				}
				cpr.QTY = CutlistQty;
				return cpr;
			}
		}

		/// <summary>
		/// Returns an array of DataRows of part-ops data from a property set.
		/// </summary>
		public ENGINEERINGDataSet.CUT_PART_OPSDataTable PartOpsRows {
			get {
				if (PartID > 0) {
					ENGINEERINGDataSetTableAdapters.CUT_PART_OPSTableAdapter cpota =
						new ENGINEERINGDataSetTableAdapters.CUT_PART_OPSTableAdapter();
					cpodt = cpota.GetDataByPartID(PartID);
					if (cpodt.Rows.Count > 0) {
						return cpodt;
					}
				}
				if (cpodt.Rows.Count < 1) {
					foreach (var item in _innerDict) {
						if (item.Value is OpProperty &&
							item.Value.ToDB) {
							OpProperty op_ = item.Value as OpProperty;
							ENGINEERINGDataSet.CUT_PART_OPSRow r_ = cpodt.NewRow() as ENGINEERINGDataSet.CUT_PART_OPSRow;
							r_.POPOP = (int)op_.Data;
							cpodt.AddCUT_PART_OPSRow(r_);
						}
					}
				}
				return cpodt;
			}
		}

		/// <summary>
		/// A URL-friendly string of global property data.
		/// </summary>
		public string GlobalTokenString {
			get {
				string result = string.Empty;
				int count = 0;
				foreach (KeyValuePair<string, SwProperty> item in _innerDict) {
					if (!item.Value.DoNotWrite && item.Value.Global) {
						result += string.Format(@"{0}={1}",
							HttpUtility.UrlEncode(item.Key), 
							HttpUtility.UrlEncode(item.Value.Data.ToString()));
						if (count++ < GlobalCount - 1) {
							result += @"&";
						}
					}
				}

				return result;
			}
		}

		/// <summary>
		/// A URL-friendly string of non-global property data.
		/// </summary>
		public string SpecificTokenString {
			get {
				string result = string.Empty;
				int count = 0;
				foreach (KeyValuePair<string, SwProperty> item in _innerDict) {
					if (!item.Value.DoNotWrite && !item.Value.Global) {
						result += string.Format(@"{0}={1}",
							HttpUtility.UrlEncode(item.Key), 
							HttpUtility.UrlEncode(item.Value.Data.ToString()));
						if (count++ < NonGlobalCount - 1) {
							result += @"&";
						}
					}
				}
				return result;
			}
		}

		/// <summary>
		/// A formatted string that should always work with db searches.
		/// </summary>
		public string PartLookup { get; set; }

		/// <summary>
		/// The hash of this part.
		/// </summary>
		public int Hash { get; set; }

		private System.IO.FileInfo _partFileInfo;
		/// <summary>
		/// A PartFileInfo object.
		/// </summary>
		public System.IO.FileInfo PartFileInfo {
			get {
				return _partFileInfo;
			}
			set {
				_partFileInfo = value;
				PartLookup = Redbrick.FileInfoToLookup(value);
				Hash = Redbrick.GetHash(PartFileInfo);
				foreach (SwProperty item in _innerDict.Values) {
					item.PartFileInfo = value;
					item.Hash = Hash;
				}
			}
		}

		/// <summary>
		/// The ID of the cutlist in question.
		/// </summary>
		public int CutlistID { get; set; }

		/// <summary>
		/// Quantity of this part in an assembly.
		/// </summary>
		public float CutlistQty { get; set; }

		private int _partID = 0;
		/// <summary>
		/// PartID from CUT_PARTS
		/// </summary>
		public int PartID {
			get {
				if (_partID == 0) {
					ENGINEERINGDataSetTableAdapters.CUT_PARTSTableAdapter cpta_ =
						new ENGINEERINGDataSetTableAdapters.CUT_PARTSTableAdapter();
					object test_obj_ = cpta_.GetPartIDByPartnum(PartLookup);
					int test_ = 0;
					if (test_obj_ != null && int.TryParse(test_obj_.ToString(), out test_)) {
						_partID = test_;
						foreach (SwProperty item in _innerDict.Values) {
							item.PartID = test_;
						}
					};
				}
				return _partID;
			}
			set {
				_partID = value;
				foreach (SwProperty item in _innerDict.Values) {
					item.PartID = _partID;
				}
			}
		}

		/// <summary>
		/// Return a list of ops with their data.
		/// </summary>
		public OpSets opSets { get; set; }

		/// <summary>
		/// Count of global properies in the set.
		/// </summary>
		public int GlobalCount {
			get { return globalCount; }
		}

		/// <summary>
		/// Count of non-global properties in the set.
		/// </summary>
		public int NonGlobalCount {
			get { return nonGlobalCount; }
		}

		/// <summary>
		/// Exception caused by the last added operation, if any.
		/// </summary>
		public Exception AddException { get; private set; }

		/// <summary>
		/// The current ModelDoc2.
		/// </summary>
		public ModelDoc2 ActiveDoc { get; private set; }

		/// <summary>
		/// Live SldWorks object.
		/// </summary>
		public SldWorks SwApp { get; private set; }
		#endregion
	}
}
