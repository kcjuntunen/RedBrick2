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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SWTaskpaneHost));
            this.tableAdapterManager1 = new RedBrick2.ENGINEERINGDataSetTableAdapters.TableAdapterManager();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cfg_btn = new System.Windows.Forms.Button();
            this.refresh_btn = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.grnchk_btn = new System.Windows.Forms.Button();
            this.qt_btn = new System.Windows.Forms.Button();
            this.archive_btn = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableAdapterManager1
            // 
            this.tableAdapterManager1.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager1.CLIENT_STUFFTableAdapter = null;
            this.tableAdapterManager1.Connection = null;
            this.tableAdapterManager1.CUT_CUTLIST_PARTSTableAdapter = null;
            this.tableAdapterManager1.CUT_CUTLISTSTableAdapter = null;
            this.tableAdapterManager1.CUT_DRAWING_MATERIALSTableAdapter = null;
            this.tableAdapterManager1.CUT_EDGES_XREFTableAdapter = null;
            this.tableAdapterManager1.CUT_EDGESTableAdapter = null;
            this.tableAdapterManager1.CUT_MATERIAL_SIZESTableAdapter = null;
            this.tableAdapterManager1.CUT_MATERIALSTableAdapter = null;
            this.tableAdapterManager1.CUT_OPS_METHODSTableAdapter = null;
            this.tableAdapterManager1.CUT_OPS_TYPESTableAdapter = null;
            this.tableAdapterManager1.CUT_OPSTableAdapter = null;
            this.tableAdapterManager1.CUT_PART_OPSTableAdapter = null;
            this.tableAdapterManager1.CUT_PART_TYPESTableAdapter = null;
            this.tableAdapterManager1.CUT_PARTSTableAdapter = null;
            this.tableAdapterManager1.CUT_STATESTableAdapter = null;
            this.tableAdapterManager1.CutlistsTableAdapter = null;
            this.tableAdapterManager1.CutPartOpsTableAdapter = null;
            this.tableAdapterManager1.ECR_DRAWINGSTableAdapter = null;
            this.tableAdapterManager1.ECR_LEGACYTableAdapter = null;
            this.tableAdapterManager1.FIX_MAINTableAdapter = null;
            this.tableAdapterManager1.GEN_CUSTOMERSTableAdapter = null;
            this.tableAdapterManager1.GEN_DEPTSTableAdapter = null;
            this.tableAdapterManager1.GEN_DRAWINGS_MTLTableAdapter = null;
            this.tableAdapterManager1.GEN_DRAWINGSTableAdapter = null;
            this.tableAdapterManager1.GEN_ODOMETERTableAdapter = null;
            this.tableAdapterManager1.GEN_USERSTableAdapter = null;
            this.tableAdapterManager1.jomastTableAdapter = null;
            this.tableAdapterManager1.LegacyECRObjLookupTableAdapter = null;
            this.tableAdapterManager1.SCH_PROJECTSTableAdapter = null;
            this.tableAdapterManager1.UpdateOrder = RedBrick2.ENGINEERINGDataSetTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel1.Controls.Add(this.cfg_btn);
            this.panel1.Controls.Add(this.refresh_btn);
            this.panel1.Controls.Add(this.button8);
            this.panel1.Controls.Add(this.grnchk_btn);
            this.panel1.Controls.Add(this.qt_btn);
            this.panel1.Controls.Add(this.archive_btn);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(276, 34);
            this.panel1.TabIndex = 0;
            this.panel1.TabStop = true;
            // 
            // cfg_btn
            // 
            this.cfg_btn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cfg_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cfg_btn.Image = global::RedBrick2.Properties.Resources.cfg;
            this.cfg_btn.Location = new System.Drawing.Point(229, 3);
            this.cfg_btn.Name = "cfg_btn";
            this.cfg_btn.Size = new System.Drawing.Size(39, 23);
            this.cfg_btn.TabIndex = 5;
            this.cfg_btn.TabStop = false;
            this.cfg_btn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.cfg_btn.UseVisualStyleBackColor = true;
            this.cfg_btn.Click += new System.EventHandler(this.cfg_btn_Click);
            // 
            // refresh_btn
            // 
            this.refresh_btn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.refresh_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refresh_btn.Image = global::RedBrick2.Properties.Resources.Refreshicon;
            this.refresh_btn.Location = new System.Drawing.Point(94, 3);
            this.refresh_btn.Name = "refresh_btn";
            this.refresh_btn.Size = new System.Drawing.Size(39, 23);
            this.refresh_btn.TabIndex = 2;
            this.refresh_btn.TabStop = false;
            this.refresh_btn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.refresh_btn.UseVisualStyleBackColor = true;
            this.refresh_btn.Click += new System.EventHandler(this.refresh_btn_Click);
            // 
            // button8
            // 
            this.button8.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button8.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.button8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button8.Image = global::RedBrick2.Properties.Resources.Tools;
            this.button8.Location = new System.Drawing.Point(49, 3);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(39, 23);
            this.button8.TabIndex = 1;
            this.button8.TabStop = false;
            this.button8.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // grnchk_btn
            // 
            this.grnchk_btn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grnchk_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.grnchk_btn.ImageKey = "(none)";
            this.grnchk_btn.Location = new System.Drawing.Point(4, 3);
            this.grnchk_btn.Name = "grnchk_btn";
            this.grnchk_btn.Size = new System.Drawing.Size(39, 23);
            this.grnchk_btn.TabIndex = 0;
            this.grnchk_btn.TabStop = false;
            this.grnchk_btn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.grnchk_btn.UseVisualStyleBackColor = true;
            this.grnchk_btn.Click += new System.EventHandler(this.grnchk_btn_Click);
            // 
            // qt_btn
            // 
            this.qt_btn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.qt_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.qt_btn.Image = ((System.Drawing.Image)(resources.GetObject("qt_btn.Image")));
            this.qt_btn.Location = new System.Drawing.Point(184, 3);
            this.qt_btn.Name = "qt_btn";
            this.qt_btn.Size = new System.Drawing.Size(39, 23);
            this.qt_btn.TabIndex = 4;
            this.qt_btn.TabStop = false;
            this.qt_btn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.qt_btn.UseVisualStyleBackColor = true;
            this.qt_btn.Click += new System.EventHandler(this.qt_btn_Click);
            // 
            // archive_btn
            // 
            this.archive_btn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.archive_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.archive_btn.Image = ((System.Drawing.Image)(resources.GetObject("archive_btn.Image")));
            this.archive_btn.Location = new System.Drawing.Point(139, 3);
            this.archive_btn.Name = "archive_btn";
            this.archive_btn.Size = new System.Drawing.Size(39, 23);
            this.archive_btn.TabIndex = 3;
            this.archive_btn.TabStop = false;
            this.archive_btn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.archive_btn.UseVisualStyleBackColor = true;
            this.archive_btn.Click += new System.EventHandler(this.archive_btn_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(282, 596);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // SWTaskpaneHost
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SWTaskpaneHost";
            this.Size = new System.Drawing.Size(282, 596);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    #endregion

    private ENGINEERINGDataSetTableAdapters.TableAdapterManager tableAdapterManager1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button cfg_btn;
		private System.Windows.Forms.Button refresh_btn;
		private System.Windows.Forms.Button button8;
		private System.Windows.Forms.Button grnchk_btn;
		private System.Windows.Forms.Button qt_btn;
		private System.Windows.Forms.Button archive_btn;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
	}
}
