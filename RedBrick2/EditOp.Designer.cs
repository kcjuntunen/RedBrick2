namespace RedBrick2 {
  partial class EditOp {
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

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.cUTPARTTYPESBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.eNGINEERINGDataSet = new RedBrick2.ENGINEERINGDataSet();
			this.label2 = new System.Windows.Forms.Label();
			this.comboBox2 = new System.Windows.Forms.ComboBox();
			this.friendlyCutOpsBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.cUTOPSTYPESBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.cUT_OPS_TYPESTableAdapter = new RedBrick2.ENGINEERINGDataSetTableAdapters.CUT_OPS_TYPESTableAdapter();
			this.cUT_PART_TYPESTableAdapter = new RedBrick2.ENGINEERINGDataSetTableAdapters.CUT_PART_TYPESTableAdapter();
			this.cUTOPSBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.cUT_OPSTableAdapter = new RedBrick2.ENGINEERINGDataSetTableAdapters.CUT_OPSTableAdapter();
			this.friendlyCutOpsTableAdapter = new RedBrick2.ENGINEERINGDataSetTableAdapters.FriendlyCutOpsTableAdapter();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.cUTPARTTYPESBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.eNGINEERINGDataSet)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.friendlyCutOpsBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cUTOPSTYPESBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cUTOPSBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.comboBox1, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.comboBox2, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.label3, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.label4, 0, 6);
			this.tableLayoutPanel1.Controls.Add(this.textBox1, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this.textBox2, 0, 7);
			this.tableLayoutPanel1.Controls.Add(this.button1, 0, 8);
			this.tableLayoutPanel1.Controls.Add(this.button2, 1, 8);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 9;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(192, 233);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.label1, 2);
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(86, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Operation Type";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// comboBox1
			// 
			this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboBox1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.comboBox1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.tableLayoutPanel1.SetColumnSpan(this.comboBox1, 2);
			this.comboBox1.DataSource = this.cUTPARTTYPESBindingSource;
			this.comboBox1.DisplayMember = "TYPEDESC";
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(3, 16);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(186, 21);
			this.comboBox1.TabIndex = 1;
			this.comboBox1.ValueMember = "TYPEID";
			this.comboBox1.Resize += new System.EventHandler(this.comboBox_Resize);
			// 
			// cUTPARTTYPESBindingSource
			// 
			this.cUTPARTTYPESBindingSource.DataMember = "CUT_PART_TYPES";
			this.cUTPARTTYPESBindingSource.DataSource = this.eNGINEERINGDataSet;
			// 
			// eNGINEERINGDataSet
			// 
			this.eNGINEERINGDataSet.DataSetName = "ENGINEERINGDataSet";
			this.eNGINEERINGDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label2.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.label2, 2);
			this.label2.Location = new System.Drawing.Point(3, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(60, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Operation";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// comboBox2
			// 
			this.comboBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboBox2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.comboBox2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.tableLayoutPanel1.SetColumnSpan(this.comboBox2, 2);
			this.comboBox2.DataSource = this.friendlyCutOpsBindingSource;
			this.comboBox2.DisplayMember = "FRIENDLYNAME";
			this.comboBox2.FormattingEnabled = true;
			this.comboBox2.Location = new System.Drawing.Point(3, 56);
			this.comboBox2.Name = "comboBox2";
			this.comboBox2.Size = new System.Drawing.Size(186, 21);
			this.comboBox2.TabIndex = 3;
			this.comboBox2.ValueMember = "OPID";
			this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
			this.comboBox2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.user_editing);
			this.comboBox2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.user_editing);
			this.comboBox2.Resize += new System.EventHandler(this.comboBox_Resize);
			// 
			// friendlyCutOpsBindingSource
			// 
			this.friendlyCutOpsBindingSource.DataMember = "FriendlyCutOps";
			this.friendlyCutOpsBindingSource.DataSource = this.eNGINEERINGDataSet;
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label3.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.label3, 2);
			this.label3.Location = new System.Drawing.Point(3, 80);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(91, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "Setup Time (min)";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label4.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.label4, 2);
			this.label4.Location = new System.Drawing.Point(3, 121);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(82, 13);
			this.label4.TabIndex = 6;
			this.label4.Text = "Run Time (min)";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textBox1
			// 
			this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.SetColumnSpan(this.textBox1, 2);
			this.textBox1.Location = new System.Drawing.Point(3, 96);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(186, 22);
			this.textBox1.TabIndex = 7;
			// 
			// textBox2
			// 
			this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.SetColumnSpan(this.textBox2, 2);
			this.textBox2.Location = new System.Drawing.Point(3, 137);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(186, 22);
			this.textBox2.TabIndex = 8;
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Location = new System.Drawing.Point(3, 207);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(90, 23);
			this.button1.TabIndex = 9;
			this.button1.Text = "OK";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.Location = new System.Drawing.Point(99, 207);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(90, 23);
			this.button2.TabIndex = 10;
			this.button2.Text = "Cancel";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// cUTOPSTYPESBindingSource
			// 
			this.cUTOPSTYPESBindingSource.DataMember = "CUT_OPS_TYPES";
			this.cUTOPSTYPESBindingSource.DataSource = this.eNGINEERINGDataSet;
			// 
			// cUT_OPS_TYPESTableAdapter
			// 
			this.cUT_OPS_TYPESTableAdapter.ClearBeforeFill = true;
			// 
			// cUT_PART_TYPESTableAdapter
			// 
			this.cUT_PART_TYPESTableAdapter.ClearBeforeFill = true;
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
			// EditOp
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(192, 252);
			this.ControlBox = false;
			this.Controls.Add(this.tableLayoutPanel1);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MinimumSize = new System.Drawing.Size(200, 260);
			this.Name = "EditOp";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Edit Operation";
			this.TopMost = true;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditOp_FormClosing);
			this.Load += new System.EventHandler(this.EditOp_Load);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.cUTPARTTYPESBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.eNGINEERINGDataSet)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.friendlyCutOpsBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cUTOPSTYPESBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cUTOPSBindingSource)).EndInit();
			this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox comboBox1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ComboBox comboBox2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.TextBox textBox2;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button button2;
    private ENGINEERINGDataSet eNGINEERINGDataSet;
    private System.Windows.Forms.BindingSource cUTOPSTYPESBindingSource;
    private ENGINEERINGDataSetTableAdapters.CUT_OPS_TYPESTableAdapter cUT_OPS_TYPESTableAdapter;
    private System.Windows.Forms.BindingSource cUTPARTTYPESBindingSource;
    private ENGINEERINGDataSetTableAdapters.CUT_PART_TYPESTableAdapter cUT_PART_TYPESTableAdapter;
    private System.Windows.Forms.BindingSource cUTOPSBindingSource;
    private ENGINEERINGDataSetTableAdapters.CUT_OPSTableAdapter cUT_OPSTableAdapter;
    private System.Windows.Forms.BindingSource friendlyCutOpsBindingSource;
    private ENGINEERINGDataSetTableAdapters.FriendlyCutOpsTableAdapter friendlyCutOpsTableAdapter;
  }
}