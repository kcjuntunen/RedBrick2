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
      SwApp = RequestSW();
      PropertySet = new SwProperties(SwApp);
      SwApp.ActiveDocChangeNotify += SwApp_ActiveDocChangeNotify;
      SwApp.DestroyNotify += SwApp_DestroyNotify;
      SwApp.FileCloseNotify += SwApp_FileCloseNotify;
      SwApp.CommandCloseNotify += SwApp_CommandCloseNotify;
      ActiveDoc = SwApp.ActiveDoc;
      ConnectSelection();
    }

    internal void ConnectSelection() {
      throw new NotImplementedException();
    }

    private int SwApp_CommandCloseNotify(int Command, int reason) {
      throw new NotImplementedException();
    }

    private int SwApp_FileCloseNotify(string FileName, int reason) {
      throw new NotImplementedException();
    }

    private int SwApp_DestroyNotify() {
      throw new NotImplementedException();
    }

    private int SwApp_ActiveDocChangeNotify() {
      throw new NotImplementedException();
    }

    protected SldWorks RequestSW() {
      if (OnRequestSW == null)
        throw new Exception("No SW!");

      return OnRequestSW();
    }

    public Func<SldWorks> OnRequestSW;

    public SwProperties PropertySet {
      get { return PropertySet;  }
      set { PropertySet = value;  }
    }

    protected ModelDoc2 ActiveDoc {
      get { return ActiveDoc;  }
      set { ActiveDoc = value; }
    }

    protected SldWorks SwApp {
      get { return SwApp; }
      set { SwApp = value; }
    }

    internal void Write() {
      throw new NotImplementedException();
    }

    internal void ReStart() {
      throw new NotImplementedException();
    }
  }
}
