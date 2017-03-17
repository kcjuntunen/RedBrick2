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
  [ComVisible(true)]
  [ProgId(SWTASKPANE_PROGID)]
  public partial class SWTaskpaneHost : UserControl {
    public const string SWTASKPANE_PROGID = "Redbrick2.SWTaskpane";
    public int cookie;

    public SWTaskpaneHost() {
      InitializeComponent();
    }

    public void Start() {
      if (SwApp == null) {
        SwApp = RequestSW();
      }
      PropertySet = new SwProperties(SwApp);
      SwApp.ActiveDocChangeNotify += SwApp_ActiveDocChangeNotify;
      SwApp.DestroyNotify += SwApp_DestroyNotify;
      SwApp.FileCloseNotify += SwApp_FileCloseNotify;
      SwApp.CommandCloseNotify += SwApp_CommandCloseNotify;
      ActiveDoc = SwApp.ActiveDoc;

      if (ModelDocs == null) {
        ModelDocs = new List<ModelDoc2>();
      }

      if (!ModelDocs.Contains(ActiveDoc)) {
        ModelDocs.Add(ActiveDoc);
      }
      ConnectSelection();
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
          PropertySet.GetProperties(comp);
          comp = null;
        } else {
          PropertySet.GetProperties(ActiveDoc);
        }
      } else {
        swSelMgr = null;
        PropertySet.GetProperties(ActiveDoc);
      }
      d = docT;
      od = overDocT;
    }

    internal void ConnectSelection() {
      System.GC.Collect(2, GCCollectionMode.Forced);
      PropertySet.CutlistID = 0;
      if (SwApp == null) {
        SwApp = RequestSW();
      }
      ActiveDoc = SwApp.ActiveDoc;
      if (ActiveDoc != null) {
        string filename = ActiveDoc.GetPathName();
        if (ActiveDoc != ModelDocs[ModelDocs.Count - 1]) {
          swDocumentTypes_e docT = swDocumentTypes_e.swDocNONE;
          swDocumentTypes_e overDocT = swDocumentTypes_e.swDocNONE;
          GetTypes(ref docT, ref overDocT);

          label1.Text = filename;

          label2.Text = PropertySet.GlobalTokenString;
          label3.Text = PropertySet.SpecificTokenString;
          PropertySet.Clear();
        }
      }
    }

    private int SwApp_CommandCloseNotify(int Command, int reason) {
      if ((swCommands_e)Command == swCommands_e.swCommands_Close || (swCommands_e)Command == swCommands_e.swCommands_Close_) {
        Enabled = false;
      }

      if ((swCommands_e)Command == swCommands_e.swCommands_Make_Lightweight ||
        (swCommands_e)Command == swCommands_e.swCommands_Lightweight_Toggle ||
        (swCommands_e)Command == swCommands_e.swCommands_Lightweight_All) {
        ReStart();
      }

      return 0;
    }

    private int SwApp_FileCloseNotify(string FileName, int reason) {
      Enabled = false;
      return 0;
    }

    private int SwApp_DestroyNotify() {
      Enabled = false;
      return 0;
    }

    private int SwApp_ActiveDocChangeNotify() {
      if (SwApp == null) {
        SwApp = RequestSW();
      }
      ConnectSelection();
      return 0;
    }

    protected SldWorks RequestSW() {
      if (OnRequestSW == null)
        throw new Exception("No SW!");

      return OnRequestSW();
    }

    public Func<SldWorks> OnRequestSW;


    protected SelectionMgr _swSelMgr;

    public SelectionMgr swSelMgr {
      get { return _swSelMgr; }
      set { _swSelMgr = value; }
    }

    protected SwProperties _propertySet;

    public SwProperties PropertySet {
      get { return _propertySet; }
      set { _propertySet = value; }
    }

    protected List<ModelDoc2> _modelDocs;

    public List<ModelDoc2> ModelDocs {
      get { return _modelDocs;}
      set { _modelDocs = value ;}
    }

    protected ModelDoc2 _activeDoc;

    public ModelDoc2 ActiveDoc {
      get { return _activeDoc; }
      set { _activeDoc = value; }
    }

    protected SldWorks _swApp;

    public SldWorks SwApp {
      get { return _swApp; }
      set { _swApp = value; }
    }

    internal void Write() {
      throw new NotImplementedException();
    }

    internal void ReStart() {
      ModelDocs.Clear();
      if (SwApp == null) {
        SwApp = RequestSW();
      }
      ConnectSelection();
    }

  }
}
