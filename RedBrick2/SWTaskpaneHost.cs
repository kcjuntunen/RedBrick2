using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swcommands;
using SolidWorks.Interop.swconst;
using SolidWorks.Interop.swpublished;

using System.Runtime.InteropServices;

namespace RedBrick2 {
	/// <summary>
	/// The fundamental UserControl in the SWTaskpane.
	/// </summary>
	[ComVisible(true)]
	[ProgId(SWTASKPANE_PROGID)]
	public partial class SWTaskpaneHost : UserControl {
		/// <summary>
		/// Program ID.
		/// </summary>
		public const string SWTASKPANE_PROGID = "Redbrick2.SWTaskpane";

		/// <summary>
		/// Mmmm.... coookies.... Do I even use this?
		/// </summary>
		public int cookie;
		private ModelRedbrick mrb;
		private bool initialated = false;

		/// <summary>
		/// Constructor.
		/// </summary>
		public SWTaskpaneHost() {
			InitializeComponent();
		}

		/// <summary>
		/// Spin up the whole works.
		/// </summary>
		public void Start() {
			if (SwApp == null) {
				SwApp = RequestSW();
			}
			SwApp.ActiveDocChangeNotify += SwApp_ActiveDocChangeNotify;
			SwApp.DestroyNotify += SwApp_DestroyNotify;
			SwApp.FileCloseNotify += SwApp_FileCloseNotify;
			SwApp.CommandCloseNotify += SwApp_CommandCloseNotify;
			SwApp.FileOpenPostNotify += SwApp_FileOpenPostNotify;
			ActiveDoc = SwApp.ActiveDoc;

			if (ActiveDoc != null) {
				ConnectSelection();
			}
		}

		/// <summary>
		/// Uglify text in certain TextBoxes.
		/// </summary>
		/// <param name="on">Bool.</param>
		public void ToggleFlameWar(bool on) {
			if (mrb != null) {
				mrb.ToggleFlameWar(on);
			}
		}

		private int SwApp_FileOpenPostNotify(string FileName) {
			ConnectSelection();
			return 0;
		}

		private int SwApp_CommandCloseNotify(int Command, int reason) {
			if ((swCommands_e)Command == swCommands_e.swCommands_Close || (swCommands_e)Command == swCommands_e.swCommands_Close_) {
				mrb.Hide();
			}

			if ((swCommands_e)Command == swCommands_e.swCommands_Make_Lightweight ||
				(swCommands_e)Command == swCommands_e.swCommands_Lightweight_Toggle ||
				(swCommands_e)Command == swCommands_e.swCommands_Lightweight_All) {
				mrb.Enabled = false;
			}

			return 0;
		}

		private int SwApp_FileCloseNotify(string FileName, int reason) {
			mrb.Hide();
			return 0;
		}

		private int SwApp_DestroyNotify() {
			mrb.Hide();
			return 0;
		}

		private int SwApp_ActiveDocChangeNotify() {
			if (SwApp == null) {
				SwApp = RequestSW();
			}
			ConnectSelection();
			return 0;
		}

		/// <summary>
		/// Run this function after this.Document is populated. It fills two ref vars with swDocumentTypes_e.
		/// </summary>
		/// <param name="d">The document type.</param>
		/// <param name="od">The top-level document type.</param>
		private void GetTypes(ref swDocumentTypes_e d, ref swDocumentTypes_e od) {
			swDocumentTypes_e docT = (swDocumentTypes_e)ActiveDoc.GetType();
			ModelDoc2 overDoc = (ModelDoc2)_swApp.ActiveDoc;
			swDocumentTypes_e overDocT = (swDocumentTypes_e)overDoc.GetType();
			if ((docT != swDocumentTypes_e.swDocDRAWING && swSelMgr != null) && swSelMgr.GetSelectedObjectCount2(-1) > 0) {
				Component2 comp = (Component2)swSelMgr.GetSelectedObjectsComponent4(1, -1);
				if (comp != null) {
					ModelDoc2 cmd = (ModelDoc2)comp.GetModelDoc2();
					docT = (swDocumentTypes_e)cmd.GetType();
					ActiveDoc = cmd;
					//PropertySet.GetProperties(comp);
				} else {
					//PropertySet.GetProperties(ActiveDoc);
				}
			} else {
				swSelMgr = null;
				//PropertySet.GetProperties(ActiveDoc);
			}
			d = docT;
			od = overDocT;
		}

		private void BuildStuff() {
			if (!initialated) {
				mrb = new ModelRedbrick(SwApp, ActiveDoc);
				Controls.Add(mrb);
				mrb.Dock = DockStyle.Fill;
				initialated = true;
			}
		}

		internal void ConnectSelection() {
			System.GC.Collect(2, GCCollectionMode.Forced);
			BuildStuff();
			mrb.DumpActiveDoc();
			mrb.ReQuery(SwApp.ActiveDoc);
		}

		/// <summary>
		/// Get SwApp.
		/// </summary>
		/// <returns>OnRequestSW()</returns>
		protected SldWorks RequestSW() {
			if (OnRequestSW == null)
				throw new Exception("No SW!");

			return OnRequestSW();
		}

		/// <summary>
		/// Function type to return a SldWorks object.
		/// </summary>
		public Func<SldWorks> OnRequestSW;

		/// <summary>
		/// A selection manager.
		/// </summary>
		protected SelectionMgr _swSelMgr;

		/// <summary>
		/// A selection manager.
		/// </summary>
		public SelectionMgr swSelMgr {
			get { return _swSelMgr; }
			set { _swSelMgr = value; }
		}

		/// <summary>
		/// Internal value for a property set for this object.
		/// </summary>
		protected SwProperties _propertySet;

		/// <summary>
		/// A property set for this object.
		/// </summary>
		public SwProperties PropertySet {
			get { return _propertySet; }
			set { _propertySet = value; }
		}

		/// <summary>
		/// Internal value for an history of ModelDoc2s.
		/// </summary>
		protected List<ModelDoc2> _modelDocs;

		/// <summary>
		/// An history of ModelDoc2s.
		/// </summary>
		public List<ModelDoc2> ModelDocs {
			get { return _modelDocs; }
			set { _modelDocs = value; }
		}

		/// <summary>
		/// Internal value for a current ModelDoc2.
		/// </summary>
		protected ModelDoc2 _activeDoc;

		/// <summary>
		/// A current ModelDoc2.
		/// </summary>
		public ModelDoc2 ActiveDoc {
			get { return _activeDoc; }
			set { _activeDoc = value; }
		}

		/// <summary>
		/// Internal value for the connnected application.
		/// </summary>
		protected SldWorks _swApp;

		/// <summary>
		/// The connnected application.
		/// </summary>
		public SldWorks SwApp {
			get { return _swApp; }
			set { _swApp = value; }
		}

		internal void Write() {
			mrb.Commit();
		}

		internal void ReStart() {
			mrb.DumpActiveDoc();
			if (SwApp == null) {
				SwApp = RequestSW();
			}
			ConnectSelection();
		}

	}
}
