namespace RedBrick2 {
  partial class CreateCutlist {
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
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.gENCUSTOMERSBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.eNGINEERINGDataSet = new RedBrick2.ENGINEERINGDataSet();
			this.comboBox2 = new System.Windows.Forms.ComboBox();
			this.cUTCUTLISTSBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.label3 = new System.Windows.Forms.Label();
			this.comboBox3 = new System.Windows.Forms.ComboBox();
			this.revListBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.comboBox4 = new System.Windows.Forms.ComboBox();
			this.comboBox5 = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.gEN_CUSTOMERSTableAdapter = new RedBrick2.ENGINEERINGDataSetTableAdapters.GEN_CUSTOMERSTableAdapter();
			this.revListTableAdapter = new RedBrick2.ENGINEERINGDataSetTableAdapters.RevListTableAdapter();
			this.cUT_CUTLISTSTableAdapter = new RedBrick2.ENGINEERINGDataSetTableAdapters.CUT_CUTLISTSTableAdapter();
			this.cutlistPartsBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.cutlistPartsTableAdapter = new RedBrick2.ENGINEERINGDataSetTableAdapters.CutlistPartsTableAdapter();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gENCUSTOMERSBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.eNGINEERINGDataSet)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cUTCUTLISTSBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.revListBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cutlistPartsBindingSource)).BeginInit();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// dataGridView1
			// 
			this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.tableLayoutPanel1.SetColumnSpan(this.dataGridView1, 5);
			this.dataGridView1.Location = new System.Drawing.Point(3, 93);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.Size = new System.Drawing.Size(699, 232);
			this.dataGridView1.TabIndex = 0;
			this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
			this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
			this.dataGridView1.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_ColumnHeaderMouseClick);
			this.dataGridView1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dataGridView1_Scroll);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.ColumnCount = 5;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29F));
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.comboBox1, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.comboBox2, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.comboBox3, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.dateTimePicker1, 4, 1);
			this.tableLayoutPanel1.Controls.Add(this.label5, 3, 1);
			this.tableLayoutPanel1.Controls.Add(this.label4, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.comboBox4, 4, 0);
			this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.comboBox5, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.label6, 2, 0);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(13, 13);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(705, 328);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(99, 30);
			this.label1.TabIndex = 1;
			this.label1.Text = "Customer";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.Location = new System.Drawing.Point(3, 30);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(99, 30);
			this.label2.TabIndex = 2;
			this.label2.Text = "Item Number";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboBox1
			// 
			this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboBox1.DataSource = this.gENCUSTOMERSBindingSource;
			this.comboBox1.DisplayMember = "CUSTOMER";
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(108, 3);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(191, 21);
			this.comboBox1.TabIndex = 4;
			this.comboBox1.ValueMember = "CUSTID";
			this.comboBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.comboBox1_MouseClick);
			// 
			// gENCUSTOMERSBindingSource
			// 
			this.gENCUSTOMERSBindingSource.DataMember = "GEN_CUSTOMERS";
			this.gENCUSTOMERSBindingSource.DataSource = this.eNGINEERINGDataSet;
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
			this.comboBox2.DataSource = this.cUTCUTLISTSBindingSource;
			this.comboBox2.DisplayMember = "PARTNUM";
			this.comboBox2.FormattingEnabled = true;
			this.comboBox2.Location = new System.Drawing.Point(108, 33);
			this.comboBox2.Name = "comboBox2";
			this.comboBox2.Size = new System.Drawing.Size(191, 21);
			this.comboBox2.TabIndex = 5;
			this.comboBox2.ValueMember = "CLID";
			this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
			this.comboBox2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBox2_KeyDown);
			this.comboBox2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.comboBox1_MouseClick);
			// 
			// cUTCUTLISTSBindingSource
			// 
			this.cUTCUTLISTSBindingSource.DataMember = "CUT_CUTLISTS";
			this.cUTCUTLISTSBindingSource.DataSource = this.eNGINEERINGDataSet;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label3.Location = new System.Drawing.Point(3, 60);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(99, 30);
			this.label3.TabIndex = 3;
			this.label3.Text = "Item Description";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboBox3
			// 
			this.comboBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboBox3.DataSource = this.revListBindingSource;
			this.comboBox3.DisplayMember = "REV";
			this.comboBox3.FormattingEnabled = true;
			this.comboBox3.Location = new System.Drawing.Point(305, 33);
			this.comboBox3.Name = "comboBox3";
			this.comboBox3.Size = new System.Drawing.Size(43, 21);
			this.comboBox3.TabIndex = 6;
			this.comboBox3.ValueMember = "REV";
			this.comboBox3.SelectedValueChanged += new System.EventHandler(this.comboBox3_SelectedValueChanged);
			this.comboBox3.MouseClick += new System.Windows.Forms.MouseEventHandler(this.comboBox3_MouseClick);
			// 
			// revListBindingSource
			// 
			this.revListBindingSource.DataMember = "RevList";
			this.revListBindingSource.DataSource = this.eNGINEERINGDataSet;
			// 
			// dateTimePicker1
			// 
			this.dateTimePicker1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dateTimePicker1.Location = new System.Drawing.Point(502, 33);
			this.dateTimePicker1.Name = "dateTimePicker1";
			this.dateTimePicker1.Size = new System.Drawing.Size(200, 20);
			this.dateTimePicker1.TabIndex = 9;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label5.Location = new System.Drawing.Point(354, 30);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(142, 30);
			this.label5.TabIndex = 8;
			this.label5.Text = "Date";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label4.Location = new System.Drawing.Point(354, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(142, 30);
			this.label4.TabIndex = 7;
			this.label4.Text = "Drawing Reference";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboBox4
			// 
			this.comboBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboBox4.DataSource = this.cUTCUTLISTSBindingSource;
			this.comboBox4.DisplayMember = "DRAWING";
			this.comboBox4.FormattingEnabled = true;
			this.comboBox4.Location = new System.Drawing.Point(502, 3);
			this.comboBox4.Name = "comboBox4";
			this.comboBox4.Size = new System.Drawing.Size(200, 21);
			this.comboBox4.TabIndex = 10;
			this.comboBox4.ValueMember = "CLID";
			// 
			// comboBox5
			// 
			this.comboBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboBox5.DataSource = this.cUTCUTLISTSBindingSource;
			this.comboBox5.DisplayMember = "DESCR";
			this.comboBox5.FormattingEnabled = true;
			this.comboBox5.Location = new System.Drawing.Point(108, 63);
			this.comboBox5.Name = "comboBox5";
			this.comboBox5.Size = new System.Drawing.Size(191, 21);
			this.comboBox5.TabIndex = 11;
			this.comboBox5.ValueMember = "CLID";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label6.Location = new System.Drawing.Point(305, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(43, 30);
			this.label6.TabIndex = 12;
			this.label6.Text = "REV";
			this.label6.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// gEN_CUSTOMERSTableAdapter
			// 
			this.gEN_CUSTOMERSTableAdapter.ClearBeforeFill = true;
			// 
			// revListTableAdapter
			// 
			this.revListTableAdapter.ClearBeforeFill = true;
			// 
			// cUT_CUTLISTSTableAdapter
			// 
			this.cUT_CUTLISTSTableAdapter.ClearBeforeFill = true;
			// 
			// cutlistPartsBindingSource
			// 
			this.cutlistPartsBindingSource.DataMember = "CutlistParts";
			this.cutlistPartsBindingSource.DataSource = this.eNGINEERINGDataSet;
			// 
			// cutlistPartsTableAdapter
			// 
			this.cutlistPartsTableAdapter.ClearBeforeFill = true;
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
			this.statusStrip1.Location = new System.Drawing.Point(0, 344);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(730, 22);
			this.statusStrip1.TabIndex = 2;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(117, 17);
			this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
			// 
			// toolStripStatusLabel2
			// 
			this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
			this.toolStripStatusLabel2.Size = new System.Drawing.Size(117, 17);
			this.toolStripStatusLabel2.Text = "toolStripStatusLabel2";
			// 
			// CreateCutlist
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(730, 366);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "CreateCutlist";
			this.Text = "CreateCutlist";
			this.Load += new System.EventHandler(this.CreateCutlist_Load);
			this.Shown += new System.EventHandler(this.CreateCutlist_Shown);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.gENCUSTOMERSBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.eNGINEERINGDataSet)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cUTCUTLISTSBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.revListBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cutlistPartsBindingSource)).EndInit();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.DataGridView dataGridView1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ComboBox comboBox1;
    private System.Windows.Forms.ComboBox comboBox2;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.ComboBox comboBox3;
    private System.Windows.Forms.DateTimePicker dateTimePicker1;
    private System.Windows.Forms.ComboBox comboBox4;
    private System.Windows.Forms.ComboBox comboBox5;
    private ENGINEERINGDataSet eNGINEERINGDataSet;
    private System.Windows.Forms.BindingSource gENCUSTOMERSBindingSource;
    private ENGINEERINGDataSetTableAdapters.GEN_CUSTOMERSTableAdapter gEN_CUSTOMERSTableAdapter;
    private System.Windows.Forms.BindingSource revListBindingSource;
    private ENGINEERINGDataSetTableAdapters.RevListTableAdapter revListTableAdapter;
    private System.Windows.Forms.BindingSource cUTCUTLISTSBindingSource;
    private ENGINEERINGDataSetTableAdapters.CUT_CUTLISTSTableAdapter cUT_CUTLISTSTableAdapter;
    private System.Windows.Forms.BindingSource cutlistPartsBindingSource;
    private ENGINEERINGDataSetTableAdapters.CutlistPartsTableAdapter cutlistPartsTableAdapter;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.StatusStrip statusStrip1;
    private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
  }
}