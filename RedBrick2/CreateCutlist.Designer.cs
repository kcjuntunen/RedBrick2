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
			this.cust_cbx = new System.Windows.Forms.ComboBox();
			this.gENCUSTOMERSBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.eNGINEERINGDataSet = new RedBrick2.ENGINEERINGDataSet();
			this.itm_cbx = new System.Windows.Forms.ComboBox();
			this.cUTCUTLISTSBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.label3 = new System.Windows.Forms.Label();
			this.rev_cbx = new System.Windows.Forms.ComboBox();
			this.revListBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.ref_cbx = new System.Windows.Forms.ComboBox();
			this.descr_cbx = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.cancel_btn = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.type_cbx = new System.Windows.Forms.ComboBox();
			this.cUTPARTTYPESBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.select_btn = new System.Windows.Forms.Button();
			this.unselect_btn = new System.Windows.Forms.Button();
			this.upload_btn = new System.Windows.Forms.Button();
			this.gEN_CUSTOMERSTableAdapter = new RedBrick2.ENGINEERINGDataSetTableAdapters.GEN_CUSTOMERSTableAdapter();
			this.revListTableAdapter = new RedBrick2.ENGINEERINGDataSetTableAdapters.RevListTableAdapter();
			this.cUT_CUTLISTSTableAdapter = new RedBrick2.ENGINEERINGDataSetTableAdapters.CUT_CUTLISTSTableAdapter();
			this.cutlistPartsBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.cutlistPartsTableAdapter = new RedBrick2.ENGINEERINGDataSetTableAdapters.CutlistPartsTableAdapter();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
			this.cUT_PART_TYPESTableAdapter = new RedBrick2.ENGINEERINGDataSetTableAdapters.CUT_PART_TYPESTableAdapter();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gENCUSTOMERSBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.eNGINEERINGDataSet)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cUTCUTLISTSBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.revListBindingSource)).BeginInit();
			this.panel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.cUTPARTTYPESBindingSource)).BeginInit();
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
			this.dataGridView1.Location = new System.Drawing.Point(3, 133);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.Size = new System.Drawing.Size(736, 179);
			this.dataGridView1.TabIndex = 0;
			this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
			this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
			this.dataGridView1.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_ColumnHeaderMouseClick);
			this.dataGridView1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dataGridView1_Scroll);
			this.dataGridView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseClick);
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
			this.tableLayoutPanel1.Controls.Add(this.cust_cbx, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.itm_cbx, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.rev_cbx, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.dateTimePicker1, 4, 1);
			this.tableLayoutPanel1.Controls.Add(this.label5, 3, 1);
			this.tableLayoutPanel1.Controls.Add(this.label4, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.ref_cbx, 4, 0);
			this.tableLayoutPanel1.Controls.Add(this.descr_cbx, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.label6, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.cancel_btn, 4, 5);
			this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.upload_btn, 0, 5);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(13, 13);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 6;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(742, 345);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(105, 30);
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
			this.label2.Size = new System.Drawing.Size(105, 30);
			this.label2.TabIndex = 2;
			this.label2.Text = "Item Number";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// cust_cbx
			// 
			this.cust_cbx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cust_cbx.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.cust_cbx.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.cust_cbx.DataSource = this.gENCUSTOMERSBindingSource;
			this.cust_cbx.DisplayMember = "CUSTOMER";
			this.cust_cbx.FormattingEnabled = true;
			this.cust_cbx.Location = new System.Drawing.Point(114, 3);
			this.cust_cbx.Name = "cust_cbx";
			this.cust_cbx.Size = new System.Drawing.Size(201, 21);
			this.cust_cbx.TabIndex = 4;
			this.cust_cbx.ValueMember = "CUSTID";
			this.cust_cbx.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBox_KeyDown);
			this.cust_cbx.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBox_KeyPress);
			this.cust_cbx.MouseClick += new System.Windows.Forms.MouseEventHandler(this.comboBox1_MouseClick);
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
			// itm_cbx
			// 
			this.itm_cbx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.itm_cbx.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.itm_cbx.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.itm_cbx.DataSource = this.cUTCUTLISTSBindingSource;
			this.itm_cbx.DisplayMember = "PARTNUM";
			this.itm_cbx.FormattingEnabled = true;
			this.itm_cbx.Location = new System.Drawing.Point(114, 33);
			this.itm_cbx.Name = "itm_cbx";
			this.itm_cbx.Size = new System.Drawing.Size(201, 21);
			this.itm_cbx.TabIndex = 5;
			this.itm_cbx.ValueMember = "CLID";
			this.itm_cbx.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
			this.itm_cbx.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBox2_KeyDown);
			this.itm_cbx.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBox_KeyPress);
			this.itm_cbx.MouseClick += new System.Windows.Forms.MouseEventHandler(this.comboBox1_MouseClick);
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
			this.label3.Size = new System.Drawing.Size(105, 30);
			this.label3.TabIndex = 3;
			this.label3.Text = "Item Description";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// rev_cbx
			// 
			this.rev_cbx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.rev_cbx.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.rev_cbx.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.rev_cbx.DataSource = this.revListBindingSource;
			this.rev_cbx.DisplayMember = "REV";
			this.rev_cbx.FormattingEnabled = true;
			this.rev_cbx.Location = new System.Drawing.Point(321, 33);
			this.rev_cbx.Name = "rev_cbx";
			this.rev_cbx.Size = new System.Drawing.Size(45, 21);
			this.rev_cbx.TabIndex = 6;
			this.rev_cbx.ValueMember = "REV";
			this.rev_cbx.SelectedValueChanged += new System.EventHandler(this.comboBox3_SelectedValueChanged);
			this.rev_cbx.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBox_KeyDown);
			this.rev_cbx.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBox_KeyPress);
			this.rev_cbx.MouseClick += new System.Windows.Forms.MouseEventHandler(this.comboBox3_MouseClick);
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
			this.dateTimePicker1.Location = new System.Drawing.Point(527, 33);
			this.dateTimePicker1.Name = "dateTimePicker1";
			this.dateTimePicker1.Size = new System.Drawing.Size(212, 22);
			this.dateTimePicker1.TabIndex = 9;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label5.Location = new System.Drawing.Point(372, 30);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(149, 30);
			this.label5.TabIndex = 8;
			this.label5.Text = "Date";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label4.Location = new System.Drawing.Point(372, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(149, 30);
			this.label4.TabIndex = 7;
			this.label4.Text = "Drawing Reference";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// ref_cbx
			// 
			this.ref_cbx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ref_cbx.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.ref_cbx.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.ref_cbx.DataSource = this.cUTCUTLISTSBindingSource;
			this.ref_cbx.DisplayMember = "DRAWING";
			this.ref_cbx.FormattingEnabled = true;
			this.ref_cbx.Location = new System.Drawing.Point(527, 3);
			this.ref_cbx.Name = "ref_cbx";
			this.ref_cbx.Size = new System.Drawing.Size(212, 21);
			this.ref_cbx.TabIndex = 10;
			this.ref_cbx.ValueMember = "CLID";
			this.ref_cbx.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBox_KeyDown);
			this.ref_cbx.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBox_KeyPress);
			// 
			// descr_cbx
			// 
			this.descr_cbx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.descr_cbx.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.descr_cbx.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.descr_cbx.DataSource = this.cUTCUTLISTSBindingSource;
			this.descr_cbx.DisplayMember = "DESCR";
			this.descr_cbx.FormattingEnabled = true;
			this.descr_cbx.Location = new System.Drawing.Point(114, 63);
			this.descr_cbx.Name = "descr_cbx";
			this.descr_cbx.Size = new System.Drawing.Size(201, 21);
			this.descr_cbx.TabIndex = 11;
			this.descr_cbx.ValueMember = "CLID";
			this.descr_cbx.TextChanged += new System.EventHandler(this.comboBox5_TextChanged);
			this.descr_cbx.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBox_KeyDown);
			this.descr_cbx.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBox_KeyPress);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label6.Location = new System.Drawing.Point(321, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(45, 30);
			this.label6.TabIndex = 12;
			this.label6.Text = "REV";
			this.label6.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// cancel_btn
			// 
			this.cancel_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancel_btn.Location = new System.Drawing.Point(664, 319);
			this.cancel_btn.Name = "cancel_btn";
			this.cancel_btn.Size = new System.Drawing.Size(75, 23);
			this.cancel_btn.TabIndex = 13;
			this.cancel_btn.Text = "Cancel";
			this.cancel_btn.UseVisualStyleBackColor = true;
			this.cancel_btn.Click += new System.EventHandler(this.button1_Click);
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tableLayoutPanel1.SetColumnSpan(this.panel1, 5);
			this.panel1.Controls.Add(this.tableLayoutPanel2);
			this.panel1.Location = new System.Drawing.Point(3, 93);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(736, 34);
			this.panel1.TabIndex = 15;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel2.ColumnCount = 4;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.Controls.Add(this.type_cbx, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.select_btn, 2, 0);
			this.tableLayoutPanel2.Controls.Add(this.unselect_btn, 3, 0);
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(728, 26);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// type_cbx
			// 
			this.type_cbx.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.type_cbx.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.type_cbx.DataSource = this.cUTPARTTYPESBindingSource;
			this.type_cbx.DisplayMember = "TYPEDESC";
			this.type_cbx.FormattingEnabled = true;
			this.type_cbx.Location = new System.Drawing.Point(374, 3);
			this.type_cbx.Name = "type_cbx";
			this.type_cbx.Size = new System.Drawing.Size(189, 21);
			this.type_cbx.TabIndex = 14;
			this.type_cbx.ValueMember = "TYPEID";
			this.type_cbx.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBox_KeyDown);
			// 
			// cUTPARTTYPESBindingSource
			// 
			this.cUTPARTTYPESBindingSource.DataMember = "CUT_PART_TYPES";
			this.cUTPARTTYPESBindingSource.DataSource = this.eNGINEERINGDataSet;
			// 
			// select_btn
			// 
			this.select_btn.Location = new System.Drawing.Point(569, 3);
			this.select_btn.Name = "select_btn";
			this.select_btn.Size = new System.Drawing.Size(75, 20);
			this.select_btn.TabIndex = 15;
			this.select_btn.Text = "Select";
			this.select_btn.UseVisualStyleBackColor = true;
			this.select_btn.Click += new System.EventHandler(this.select_btn_Click);
			// 
			// unselect_btn
			// 
			this.unselect_btn.Location = new System.Drawing.Point(650, 3);
			this.unselect_btn.Name = "unselect_btn";
			this.unselect_btn.Size = new System.Drawing.Size(75, 20);
			this.unselect_btn.TabIndex = 16;
			this.unselect_btn.Text = "Unselect";
			this.unselect_btn.UseVisualStyleBackColor = true;
			this.unselect_btn.Click += new System.EventHandler(this.unselect_btn_Click);
			// 
			// upload_btn
			// 
			this.upload_btn.Location = new System.Drawing.Point(3, 318);
			this.upload_btn.Name = "upload_btn";
			this.upload_btn.Size = new System.Drawing.Size(75, 23);
			this.upload_btn.TabIndex = 16;
			this.upload_btn.Text = "Upload";
			this.upload_btn.UseVisualStyleBackColor = true;
			this.upload_btn.Click += new System.EventHandler(this.upload_btn_Click);
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
			this.statusStrip1.Location = new System.Drawing.Point(0, 361);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(767, 22);
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
			// cUT_PART_TYPESTableAdapter
			// 
			this.cUT_PART_TYPESTableAdapter.ClearBeforeFill = true;
			// 
			// CreateCutlist
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(767, 383);
			this.ControlBox = false;
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "CreateCutlist";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Create Cutlist";
			this.TopMost = true;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CreateCutlist_FormClosing);
			this.Load += new System.EventHandler(this.CreateCutlist_Load);
			this.Shown += new System.EventHandler(this.CreateCutlist_Shown);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.gENCUSTOMERSBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.eNGINEERINGDataSet)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cUTCUTLISTSBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.revListBindingSource)).EndInit();
			this.panel1.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.cUTPARTTYPESBindingSource)).EndInit();
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
    private System.Windows.Forms.ComboBox cust_cbx;
    private System.Windows.Forms.ComboBox itm_cbx;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.ComboBox rev_cbx;
    private System.Windows.Forms.DateTimePicker dateTimePicker1;
    private System.Windows.Forms.ComboBox ref_cbx;
    private System.Windows.Forms.ComboBox descr_cbx;
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
		private System.Windows.Forms.Button cancel_btn;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.ComboBox type_cbx;
		private System.Windows.Forms.Button select_btn;
		private System.Windows.Forms.Button unselect_btn;
		private System.Windows.Forms.BindingSource cUTPARTTYPESBindingSource;
		private ENGINEERINGDataSetTableAdapters.CUT_PART_TYPESTableAdapter cUT_PART_TYPESTableAdapter;
		private System.Windows.Forms.Button upload_btn;
  }
}