using System.IO;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
	/// <summary>
	/// A general, fundamental SolidWorks property handler.
	/// </summary>
	public class SwProperty {
		private string v;
		private string resolvedV;
		private bool wasResolved;
		private CustomPropertyManager globlProperty;
		private CustomPropertyManager localProperty;

		internal ENGINEERINGDataSetTableAdapters.CUT_PARTSTableAdapter cpta =
			new ENGINEERINGDataSetTableAdapters.CUT_PARTSTableAdapter();

		/// <summary>
		/// Constructor for a new <see cref="RedBrick2.SwProperty"/>.
		/// </summary>
		/// <param name="name">The name of the property. This will be the name used in the property summary page.</param>
		/// <param name="global">A general property, or configuration specific property.</param>
		/// <param name="sw">A running <see cref="SolidWorks.Interop.sldworks.SldWorks"/> object.</param>
		/// <param name="md">The <see cref="SolidWorks.Interop.sldworks.ModelDoc2"/> to which this property belongs.</param>
		public SwProperty(string name, bool global, SldWorks sw, ModelDoc2 md) {
			Name = name;
			SWType = swCustomInfoType_e.swCustomInfoUnknown;
			Global = global;
			SwApp = sw;
			ActiveDoc = md;
			if (ActiveDoc == null) {
				return;
			}
			GetPropertyManager();
			//GetFileInfo();
			SetDefaults();
		}

		/// <summary>
		/// Constructor for a new <see cref="RedBrick2.SwProperty"/>.
		/// </summary>
		/// <param name="name">The name of the property. This will be the name used in the property summary page.</param>
		/// <param name="global">A general property, or configuration specific property.</param>
		/// <param name="sw">A running <see cref="SolidWorks.Interop.sldworks.SldWorks"/> object.</param>
		/// <param name="comp">The <see cref="SolidWorks.Interop.sldworks.Component2"/> to which this property belongs.</param>
		public SwProperty(string name, bool global, SldWorks sw, Component2 comp) {
			Name = name;
			SWType = swCustomInfoType_e.swCustomInfoUnknown;
			Global = global;
			SwApp = sw;
			ActiveDoc = comp.GetModelDoc2();
			if (ActiveDoc == null) {
				return;
			}
			GetPropertyManager();
			//GetFileInfo();
			SetDefaults();
		}

		private void SetDefaults() {
			ToDB = TableName != string.Empty && FieldName != string.Empty;
			DoNotWrite = false;
		}

		private void GetFileInfo() {
			string path_ = ActiveDoc.GetPathName();
			PartID = 0;
			if (path_ == string.Empty) {
				return;
			}

			PartFileInfo = new FileInfo(ActiveDoc.GetPathName());
			Hash = Redbrick.GetHash(PartFileInfo);
			object _prtID = cpta.GetPartIDByPartnum(Redbrick.FileInfoToLookup(PartFileInfo));

			if (_prtID == null) {
				return;
			}

			PartID = (int)_prtID;
		}

		private void GetPropertyManager2() {
			ModelDocExtension ext = null;
			if (ActiveDoc != null) {
				while (ext == null) {
					ext = ActiveDoc.Extension;
				}
				if (ext != null) {
					globlProperty = ext.get_CustomPropertyManager(string.Empty);
					//config = (Configuration)ActiveDoc.GetConfigurationByName(Configuration);
					if (ActiveDoc is DrawingDoc) {
						PropertyManager = globlProperty;
					} else {
						localProperty = ext.get_CustomPropertyManager(Configuration);
					}
					if (Global || (ActiveDoc is DrawingDoc)) {
						PropertyManager = globlProperty;
					} else {
						PropertyManager = localProperty;
					}
				}
			}
		}

		private void GetPropertyManager() {
			if (ActiveDoc == null) {
				return;
			}

			ModelDocExtension ext = null;
			ext = ActiveDoc.Extension;

			if (ext == null) {
				return;
			}

			globlProperty = ext.get_CustomPropertyManager(string.Empty);

			if (ActiveDoc is DrawingDoc) {
				PropertyManager = globlProperty;
			} else {
				localProperty = ext.get_CustomPropertyManager(Configuration);
			}
			if (Global || (ActiveDoc is DrawingDoc)) {
				PropertyManager = globlProperty;
			} else {
				PropertyManager = localProperty;
			}

		}
		
		/// <summary>
		/// Do the actual reading from SolidWorks.
		/// </summary>
		protected void InnerGet2() {
			if (PropertyManager == null) {
				GetPropertyManager();
			}
			if (PropertyManager != null) {
				if (ActiveDoc is DrawingDoc) {
					string[] props_ = globlProperty.GetNames();
					if (arrayContains(props_, Name)) {
						GetResult = (swCustomInfoGetResult_e)globlProperty.Get5(Name, false, out v, out resolvedV, out wasResolved);
					}
				} else {
					string[] globprops_ = globlProperty.GetNames();
					if (arrayContains(globprops_, Name)) {
						GetResult = (swCustomInfoGetResult_e)globlProperty.Get5(Name, false, out v, out resolvedV, out wasResolved);
					}
					if (v == null || v == string.Empty) {
						if (localProperty != null) {
							string[] locprops_ = localProperty.GetNames();
							if (arrayContains(locprops_, Name)) {
								GetResult = (swCustomInfoGetResult_e)localProperty.Get5(Name, false, out v, out resolvedV, out wasResolved);
							}
						}
					}
					if (v == null || v == string.Empty) {
						if (arrayContains(globprops_, Name)) {
							GetResult = (swCustomInfoGetResult_e)globlProperty.Get5(Name, false, out v, out resolvedV, out wasResolved);
						}
					}
				}
			}
		}

		/// <summary>
		/// Do the actual reading from SolidWorks.
		/// </summary>
		protected void InnerGet() {
			if (PropertyManager == null) {
				GetPropertyManager();
			}

			if (PropertyManager == null) {
				return;
			}

			if (ActiveDoc is DrawingDoc) {
				string[] props_ = globlProperty.GetNames();
				if (arrayContains(props_, Name)) {
					GetResult = (swCustomInfoGetResult_e)globlProperty.Get5(Name, false, out v, out resolvedV, out wasResolved);
				}
			} else {
				string[] globprops_ = globlProperty.GetNames();
				if (arrayContains(globprops_, Name)) {
					GetResult = (swCustomInfoGetResult_e)globlProperty.Get5(Name, false, out v, out resolvedV, out wasResolved);
				}
				if (v == null || v == string.Empty) {
					if (localProperty != null) {
						string[] locprops_ = localProperty.GetNames();
						if (arrayContains(locprops_, Name)) {
							GetResult = (swCustomInfoGetResult_e)localProperty.Get5(Name, false, out v, out resolvedV, out wasResolved);
						}
					}
				}
				if (v == null || v == string.Empty) {
					if (arrayContains(globprops_, Name)) {
						GetResult = (swCustomInfoGetResult_e)globlProperty.Get5(Name, false, out v, out resolvedV, out wasResolved);
					}
				}
			}
		}

		static bool arrayContains(string[] arr, string st) {
			if (arr == null) {
				return false;
			}
			for (int i = 0; i < arr.Length; i++) {
				if (arr[i] == st) {
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Pulls property data from SolidWorks.
		/// </summary>
		/// <returns>Returns a <see cref="RedBrick2.SwProperty"/> object.</returns>
		public virtual SwProperty Get() {
			InnerGet();
			if (Value != null) {
				Data = Value;
			}
			return this;
		}

		/// <summary>
		/// Sets the Data, and Value fields.
		/// </summary>
		/// <param name="data">A <see cref="System.Object"/> and attempts to interpret it into the correct type.</param>
		/// <param name="val">A <see cref="System.String"/>.</param>
		public virtual void Set(object data, string val) {
			Data = data;
			Value = val;
		}

		/// <summary>
		/// Write the property data to SolidWorks.
		/// </summary>
		public virtual void Write() {
			if (!DoNotWrite) {
				if (Global && localProperty != null) {
					localProperty.Delete2(Name);
				}
				WriteResult =
					(swCustomInfoAddResult_e)PropertyManager.Add3(Name,
					(int)SWType,
					Value,
					(int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
			} else {
				DeleteResult = (swCustomInfoDeleteResult_e)PropertyManager.Delete(Name);
			}
		}

		/// <summary>
		/// Delete item from SW properties.
		/// </summary>
		public virtual void Delete() {
			DeleteResult = (swCustomInfoDeleteResult_e)PropertyManager.Delete2(Name);
		}

		/// <summary>
		/// Is this OK to write to db?
		/// </summary>
		public bool ToDB { get; set; }

		/// <summary>
		/// Write to SW if false, or delete from SW if true.
		/// </summary>
		public bool DoNotWrite { get; set; }

		/// <summary>
		/// Everything we know about the file.
		/// </summary>
		public FileInfo PartFileInfo { get; set; }

		/// <summary>
		/// CRC32
		/// </summary>
		public int Hash { get; set; }

		/// <summary>
		/// ID from CUT_PARTS
		/// </summary>
		public int PartID { get; set; }

		/// <summary>
		/// Resolved when read from SW?
		/// </summary>
		public bool WasResolved { get; set; }

		/// <summary>
		/// Result from deleting, if any.
		/// </summary>
		public swCustomInfoDeleteResult_e DeleteResult { get; set; }

		/// <summary>
		/// Result from writing, if any.
		/// </summary>
		public swCustomInfoAddResult_e WriteResult { get; set; }

		/// <summary>
		/// Result from getting, if any.
		/// </summary>
		public swCustomInfoGetResult_e GetResult { get; set; }

		/// <summary>
		/// Property name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Property value. This is what gets read from, or written to SW.
		/// </summary>
		public string Value {
			get { return v; }
			set { v = value; }
		}

		/// <summary>
		/// The property value, resolved by SW. SW propertese is translated to
		/// reality here.
		/// </summary>
		public string ResolvedValue {
			get { return resolvedV; }
			set { resolvedV = value; }
		}

		/// <summary>
		/// Whether this property is written to a specific configuration,
		/// or "".
		/// </summary>
		public bool Global { get; set; }

		/// <summary>
		/// The SolidWorks data type.
		/// </summary>
		public swCustomInfoType_e SWType { get; set; }

		/// <summary>
		/// The name of the table relevant to this property.
		/// </summary>
		public string TableName { get; protected set; }

		/// <summary>
		/// The name of the column relevant to this property.
		/// </summary>
		public string FieldName { get; protected set; }

		/// <summary>
		/// Internal value for "Data".
		/// </summary>
		protected object _data = "Data goes here.";

		/// <summary>
		/// Data formatted for entry into the db.
		/// </summary>
		public virtual object Data {
			get { return _data; }
			set { _data = value; }
		}

		private string _configuration = string.Empty;

		/// <summary>
		/// The configuration name that non-global parts are written to, or read from.
		/// </summary>
		public string Configuration {
			get {
				return _configuration;
			}
			set {
				_configuration = value;
				GetPropertyManager();
			}
		}

		/// <summary>
		/// The Configuration that non-global parts are written to, or read from.
		/// </summary>
		public Configuration Config { get; set; }

		/// <summary>
		/// The property manager we should use to write data to a ModelDoc2.
		/// </summary>
		public CustomPropertyManager PropertyManager { get; set; }

		private ModelDoc2 _activeDoc;
		/// <summary>
		/// The current ModelDoc2 in question.
		/// </summary>
		public ModelDoc2 ActiveDoc {
			get { return _activeDoc; }
			protected set { _activeDoc = value; }
		}

		/// <summary>
		/// Internal value for the connected application.
		/// </summary>
		protected SldWorks _swApp;
		/// <summary>
		/// The connected application.
		/// </summary>
		public SldWorks SwApp {
			get { return _swApp; }
			protected set { _swApp = value; }
		}

	}
}
