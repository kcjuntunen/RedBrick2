namespace RedBrick2 {
  partial class OpControl {
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
      this.components = new System.ComponentModel.Container();
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.comboBox1 = new System.Windows.Forms.ComboBox();
      this.friendlyCutOpsBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.eNGINEERINGDataSet = new RedBrick2.ENGINEERINGDataSet();
      this.comboBox2 = new System.Windows.Forms.ComboBox();
      this.cUTPARTTYPESBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.textBox2 = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.cUTOPSTYPESBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.eNGINEERINGDataSet1 = new RedBrick2.ENGINEERINGDataSet();
      this.cUTOPSBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.cUT_OPSTableAdapter = new RedBrick2.ENGINEERINGDataSetTableAdapters.CUT_OPSTableAdapter();
      this.friendlyCutOpsTableAdapter = new RedBrick2.ENGINEERINGDataSetTableAdapters.FriendlyCutOpsTableAdapter();
      this.cUT_OPS_TYPESTableAdapter = new RedBrick2.ENGINEERINGDataSetTableAdapters.CUT_OPS_TYPESTableAdapter();
      this.cUT_PART_TYPESTableAdapter = new RedBrick2.ENGINEERINGDataSetTableAdapters.CUT_PART_TYPESTableAdapter();
      this.label5 = new System.Windows.Forms.Label();
      this.tableLayoutPanel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.friendlyCutOpsBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.eNGINEERINGDataSet)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.cUTPARTTYPESBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.cUTOPSTYPESBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.eNGINEERINGDataSet1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.cUTOPSBindingSource)).BeginInit();
      this.SuspendLayout();
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tableLayoutPanel1.ColumnCount = 3;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel1.Controls.Add(this.label2, 2, 0);
      this.tableLayoutPanel1.Controls.Add(this.comboBox2, 2, 1);
      this.tableLayoutPanel1.Controls.Add(this.label4, 2, 2);
      this.tableLayoutPanel1.Controls.Add(this.textBox2, 2, 3);
      this.tableLayoutPanel1.Controls.Add(this.textBox1, 1, 3);
      this.tableLayoutPanel1.Controls.Add(this.label3, 1, 2);
      this.tableLayoutPanel1.Controls.Add(this.comboBox1, 1, 1);
      this.tableLayoutPanel1.Controls.Add(this.label1, 1, 0);
      this.tableLayoutPanel1.Controls.Add(this.label5, 0, 0);
      this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 4;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.Size = new System.Drawing.Size(255, 90);
      this.tableLayoutPanel1.TabIndex = 0;
      // 
      // label1
      // 
      this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(33, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(106, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Operation";
      // 
      // label2
      // 
      this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(145, 0);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(107, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "Operation Type";
      // 
      // comboBox1
      // 
      this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBox1.DataSource = this.friendlyCutOpsBindingSource;
      this.comboBox1.DisplayMember = "FRIENDLYNAME";
      this.comboBox1.FormattingEnabled = true;
      this.comboBox1.Location = new System.Drawing.Point(33, 16);
      this.comboBox1.Name = "comboBox1";
      this.comboBox1.Size = new System.Drawing.Size(106, 21);
      this.comboBox1.TabIndex = 2;
      this.comboBox1.ValueMember = "OPID";
      this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
      // 
      // friendlyCutOpsBindingSource
      // 
      this.friendlyCutOpsBindingSource.DataMember = "FriendlyCutOps";
      this.friendlyCutOpsBindingSource.DataSource = this.eNGINEERINGDataSet;
      // 
      // eNGINEERINGDataSet
      // 
      this.eNGINEERINGDataSet.DataSetName = "ENGINEERINGDataSet";
      this.eNGINEERINGDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
      // 
      // comboBox2
      // 
      this.comboBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBox2.DataSource = this.cUTPARTTYPESBindingSource;
      this.comboBox2.DisplayMember = "TYPEDESC";
      this.comboBox2.FormattingEnabled = true;
      this.comboBox2.Location = new System.Drawing.Point(145, 16);
      this.comboBox2.Name = "comboBox2";
      this.comboBox2.Size = new System.Drawing.Size(107, 21);
      this.comboBox2.TabIndex = 3;
      this.comboBox2.ValueMember = "TYPEID";
      this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
      // 
      // cUTPARTTYPESBindingSource
      // 
      this.cUTPARTTYPESBindingSource.DataMember = "CUT_PART_TYPES";
      this.cUTPARTTYPESBindingSource.DataSource = this.eNGINEERINGDataSet;
      // 
      // textBox1
      // 
      this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBox1.Location = new System.Drawing.Point(33, 56);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new System.Drawing.Size(106, 20);
      this.textBox1.TabIndex = 4;
      this.textBox1.Validating += new System.ComponentModel.CancelEventHandler(this.textBox1_Validating);
      this.textBox1.Validated += new System.EventHandler(this.textBox1_Validated);
      // 
      // textBox2
      // 
      this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBox2.Location = new System.Drawing.Point(145, 56);
      this.textBox2.Name = "textBox2";
      this.textBox2.Size = new System.Drawing.Size(107, 20);
      this.textBox2.TabIndex = 5;
      this.textBox2.Validating += new System.ComponentModel.CancelEventHandler(this.textBox2_Validating);
      this.textBox2.Validated += new System.EventHandler(this.textBox2_Validated);
      // 
      // label3
      // 
      this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(33, 40);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(106, 13);
      this.label3.TabIndex = 6;
      this.label3.Text = "Setup Time (min)";
      // 
      // label4
      // 
      this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(145, 40);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(107, 13);
      this.label4.TabIndex = 7;
      this.label4.Text = "Run Time (min)";
      // 
      // cUTOPSTYPESBindingSource
      // 
      this.cUTOPSTYPESBindingSource.DataMember = "CUT_OPS_TYPES";
      this.cUTOPSTYPESBindingSource.DataSource = this.eNGINEERINGDataSet1;
      // 
      // eNGINEERINGDataSet1
      // 
      this.eNGINEERINGDataSet1.DataSetName = "ENGINEERINGDataSet";
      this.eNGINEERINGDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
      // 
      // cUTOPSBindingSource
      // 
      this.cUTOPSBindingSource.DataMember = "CUT_OPS";
      this.cUTOPSBindingSource.DataSource = this.eNGINEERINGDataSet;
      // 
      // cUT_OPSTableAdapter
      // 
      this.cUT_OPSTableAdapter.ClearBeforeFill = true;
      // 
      // friendlyCutOpsTableAdapter
      // 
      this.friendlyCutOpsTableAdapter.ClearBeforeFill = true;
      // 
      // cUT_OPS_TYPESTableAdapter
      // 
      this.cUT_OPS_TYPESTableAdapter.ClearBeforeFill = true;
      // 
      // cUT_PART_TYPESTableAdapter
      // 
      this.cUT_PART_TYPESTableAdapter.ClearBeforeFill = true;
      // 
      // label5
      // 
      this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.label5.AutoSize = true;
      this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label5.Location = new System.Drawing.Point(3, 0);
      this.label5.Name = "label5";
      this.tableLayoutPanel1.SetRowSpan(this.label5, 4);
      this.label5.Size = new System.Drawing.Size(24, 90);
      this.label5.TabIndex = 8;
      this.label5.Text = "0";
      this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // OpControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.Controls.Add(this.tableLayoutPanel1);
      this.Name = "OpControl";
      this.Size = new System.Drawing.Size(259, 96);
      this.Load += new System.EventHandler(this.OpControl_Load);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.friendlyCutOpsBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.eNGINEERINGDataSet)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.cUTPARTTYPESBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.cUTOPSTYPESBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.eNGINEERINGDataSet1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.cUTOPSBindingSource)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ComboBox comboBox1;
    private System.Windows.Forms.ComboBox comboBox2;
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.TextBox textBox2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.BindingSource cUTOPSBindingSource;
    private ENGINEERINGDataSet eNGINEERINGDataSet;
    private ENGINEERINGDataSetTableAdapters.CUT_OPSTableAdapter cUT_OPSTableAdapter;
    private System.Windows.Forms.BindingSource friendlyCutOpsBindingSource;
    private ENGINEERINGDataSetTableAdapters.FriendlyCutOpsTableAdapter friendlyCutOpsTableAdapter;
    private System.Windows.Forms.BindingSource cUTOPSTYPESBindingSource;
    private ENGINEERINGDataSet eNGINEERINGDataSet1;
    private ENGINEERINGDataSetTableAdapters.CUT_OPS_TYPESTableAdapter cUT_OPS_TYPESTableAdapter;
    private System.Windows.Forms.BindingSource cUTPARTTYPESBindingSource;
    private ENGINEERINGDataSetTableAdapters.CUT_PART_TYPESTableAdapter cUT_PART_TYPESTableAdapter;
    private System.Windows.Forms.Label label5;
  }
}
