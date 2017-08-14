namespace RedBrick2 {
  partial class SWTaskpaneHost {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.tableAdapterManager1 = new RedBrick2.ENGINEERINGDataSetTableAdapters.TableAdapterManager();
      this.SuspendLayout();
      // 
      // tableAdapterManager1
      // 
      this.tableAdapterManager1.BackupDataSetBeforeUpdate = false;
      this.tableAdapterManager1.Connection = null;
      this.tableAdapterManager1.CUT_CUTLIST_PARTSTableAdapter = null;
      this.tableAdapterManager1.CUT_CUTLISTSTableAdapter = null;
      this.tableAdapterManager1.CUT_DRAWING_MATERIALSTableAdapter = null;
      this.tableAdapterManager1.CUT_EDGES_XREFTableAdapter = null;
      this.tableAdapterManager1.CUT_EDGESTableAdapter = null;
      this.tableAdapterManager1.CUT_MATERIALSTableAdapter = null;
      this.tableAdapterManager1.CUT_OPS_METHODSTableAdapter = null;
      this.tableAdapterManager1.CUT_OPS_TYPESTableAdapter = null;
      this.tableAdapterManager1.CUT_OPSTableAdapter = null;
      this.tableAdapterManager1.CUT_PART_OPSTableAdapter = null;
      this.tableAdapterManager1.CUT_PART_TYPESTableAdapter = null;
      this.tableAdapterManager1.CUT_PARTSTableAdapter = null;
      this.tableAdapterManager1.CUT_STATESTableAdapter = null;
      //this.tableAdapterManager1.CutPartOpsTableAdapter = null;
      //this.tableAdapterManager1.FriendlyCutOpsTableAdapter = null;
      this.tableAdapterManager1.GEN_CUSTOMERSTableAdapter = null;
      this.tableAdapterManager1.GEN_ODOMETERTableAdapter = null;
      this.tableAdapterManager1.GEN_USERSTableAdapter = null;
      this.tableAdapterManager1.UpdateOrder = RedBrick2.ENGINEERINGDataSetTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
      // 
      // SWTaskpaneHost
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Name = "SWTaskpaneHost";
      this.Size = new System.Drawing.Size(267, 596);
      this.ResumeLayout(false);

    }

    #endregion

    private ENGINEERINGDataSetTableAdapters.TableAdapterManager tableAdapterManager1;

  }
}
