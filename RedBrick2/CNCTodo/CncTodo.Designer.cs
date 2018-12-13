namespace RedBrick2 {
	partial class CncTodo {
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
			this.listView1 = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.cUTCNCJOBSVIEW1BindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.cNCTodoDataSet = new RedBrick2.CNCTodo.CNCTodoDataSet();
			this.cUT_CNC_JOBS_VIEW1TableAdapter = new RedBrick2.CNCTodo.CNCTodoDataSetTableAdapters.CUT_CNC_JOBS_VIEW1TableAdapter();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.pARTDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dESCRIPTIONDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.cUTLISTDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.lDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.wDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.uPDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.cNC1DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.cNC2DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataTable1BindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.cUTCNCCUTLISTPARTSBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.cUT_CNC_CUTLIST_PARTSTableAdapter = new RedBrick2.CNCTodo.CNCTodoDataSetTableAdapters.CUT_CNC_CUTLIST_PARTSTableAdapter();
			((System.ComponentModel.ISupportInitialize)(this.cUTCNCJOBSVIEW1BindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cNCTodoDataSet)).BeginInit();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataTable1BindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cUTCNCCUTLISTPARTSBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// listView1
			// 
			this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9});
			this.listView1.Location = new System.Drawing.Point(6, 33);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(735, 335);
			this.listView1.TabIndex = 0;
			this.listView1.UseCompatibleStateImageBehavior = false;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Due";
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Job";
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Qty";
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Job Status";
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "Part";
			// 
			// columnHeader6
			// 
			this.columnHeader6.Text = "Rev";
			// 
			// columnHeader7
			// 
			this.columnHeader7.Text = "CL Status";
			// 
			// columnHeader8
			// 
			this.columnHeader8.Text = "Printed";
			// 
			// columnHeader9
			// 
			this.columnHeader9.Text = "X";
			// 
			// checkBox1
			// 
			this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new System.Drawing.Point(661, 371);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(80, 17);
			this.checkBox1.TabIndex = 1;
			this.checkBox1.Text = "checkBox1";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
			// 
			// comboBox1
			// 
			this.comboBox1.DataSource = this.cUTCNCJOBSVIEW1BindingSource;
			this.comboBox1.DisplayMember = "WorkCenter";
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(6, 6);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(260, 21);
			this.comboBox1.TabIndex = 2;
			this.comboBox1.ValueMember = "WorkCenter";
			this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
			// 
			// cUTCNCJOBSVIEW1BindingSource
			// 
			this.cUTCNCJOBSVIEW1BindingSource.DataMember = "CUT_CNC_JOBS_VIEW1";
			this.cUTCNCJOBSVIEW1BindingSource.DataSource = this.cNCTodoDataSet;
			// 
			// cNCTodoDataSet
			// 
			this.cNCTodoDataSet.DataSetName = "CNCTodoDataSet";
			this.cNCTodoDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// cUT_CNC_JOBS_VIEW1TableAdapter
			// 
			this.cUT_CNC_JOBS_VIEW1TableAdapter.ClearBeforeFill = true;
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Location = new System.Drawing.Point(13, 12);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(755, 420);
			this.tabControl1.TabIndex = 3;
			this.tabControl1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tabControl1_MouseDoubleClick);
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.listView1);
			this.tabPage1.Controls.Add(this.comboBox1);
			this.tabPage1.Controls.Add(this.checkBox1);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(747, 394);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "tabPage1";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.dataGridView1);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(747, 365);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "tabPage2";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// dataGridView1
			// 
			this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dataGridView1.AutoGenerateColumns = false;
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.pARTDataGridViewTextBoxColumn,
            this.dESCRIPTIONDataGridViewTextBoxColumn,
            this.cUTLISTDataGridViewTextBoxColumn,
            this.dataGridViewTextBoxColumn8,
            this.lDataGridViewTextBoxColumn,
            this.wDataGridViewTextBoxColumn,
            this.tDataGridViewTextBoxColumn,
            this.dataGridViewTextBoxColumn9,
            this.dataGridViewTextBoxColumn10,
            this.uPDataGridViewCheckBoxColumn,
            this.cNC1DataGridViewTextBoxColumn,
            this.cNC2DataGridViewTextBoxColumn,
            this.dataGridViewTextBoxColumn11});
			this.dataGridView1.DataSource = this.dataTable1BindingSource;
			this.dataGridView1.Location = new System.Drawing.Point(6, 6);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.Size = new System.Drawing.Size(735, 353);
			this.dataGridView1.TabIndex = 0;
			// 
			// pARTDataGridViewTextBoxColumn
			// 
			this.pARTDataGridViewTextBoxColumn.DataPropertyName = "PART";
			this.pARTDataGridViewTextBoxColumn.HeaderText = "PART";
			this.pARTDataGridViewTextBoxColumn.Name = "pARTDataGridViewTextBoxColumn";
			// 
			// dESCRIPTIONDataGridViewTextBoxColumn
			// 
			this.dESCRIPTIONDataGridViewTextBoxColumn.DataPropertyName = "DESCRIPTION";
			this.dESCRIPTIONDataGridViewTextBoxColumn.HeaderText = "DESCRIPTION";
			this.dESCRIPTIONDataGridViewTextBoxColumn.Name = "dESCRIPTIONDataGridViewTextBoxColumn";
			// 
			// cUTLISTDataGridViewTextBoxColumn
			// 
			this.cUTLISTDataGridViewTextBoxColumn.DataPropertyName = "CUTLIST";
			this.cUTLISTDataGridViewTextBoxColumn.HeaderText = "CUTLIST";
			this.cUTLISTDataGridViewTextBoxColumn.Name = "cUTLISTDataGridViewTextBoxColumn";
			// 
			// dataGridViewTextBoxColumn8
			// 
			this.dataGridViewTextBoxColumn8.DataPropertyName = "REV";
			this.dataGridViewTextBoxColumn8.HeaderText = "REV";
			this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
			// 
			// lDataGridViewTextBoxColumn
			// 
			this.lDataGridViewTextBoxColumn.DataPropertyName = "L";
			this.lDataGridViewTextBoxColumn.HeaderText = "L";
			this.lDataGridViewTextBoxColumn.Name = "lDataGridViewTextBoxColumn";
			// 
			// wDataGridViewTextBoxColumn
			// 
			this.wDataGridViewTextBoxColumn.DataPropertyName = "W";
			this.wDataGridViewTextBoxColumn.HeaderText = "W";
			this.wDataGridViewTextBoxColumn.Name = "wDataGridViewTextBoxColumn";
			// 
			// tDataGridViewTextBoxColumn
			// 
			this.tDataGridViewTextBoxColumn.DataPropertyName = "T";
			this.tDataGridViewTextBoxColumn.HeaderText = "T";
			this.tDataGridViewTextBoxColumn.Name = "tDataGridViewTextBoxColumn";
			// 
			// dataGridViewTextBoxColumn9
			// 
			this.dataGridViewTextBoxColumn9.DataPropertyName = "OVERL";
			this.dataGridViewTextBoxColumn9.HeaderText = "OVERL";
			this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
			// 
			// dataGridViewTextBoxColumn10
			// 
			this.dataGridViewTextBoxColumn10.DataPropertyName = "OVERW";
			this.dataGridViewTextBoxColumn10.HeaderText = "OVERW";
			this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
			// 
			// uPDataGridViewCheckBoxColumn
			// 
			this.uPDataGridViewCheckBoxColumn.DataPropertyName = "UP";
			this.uPDataGridViewCheckBoxColumn.HeaderText = "UP";
			this.uPDataGridViewCheckBoxColumn.Name = "uPDataGridViewCheckBoxColumn";
			// 
			// cNC1DataGridViewTextBoxColumn
			// 
			this.cNC1DataGridViewTextBoxColumn.DataPropertyName = "CNC1";
			this.cNC1DataGridViewTextBoxColumn.HeaderText = "CNC1";
			this.cNC1DataGridViewTextBoxColumn.Name = "cNC1DataGridViewTextBoxColumn";
			// 
			// cNC2DataGridViewTextBoxColumn
			// 
			this.cNC2DataGridViewTextBoxColumn.DataPropertyName = "CNC2";
			this.cNC2DataGridViewTextBoxColumn.HeaderText = "CNC2";
			this.cNC2DataGridViewTextBoxColumn.Name = "cNC2DataGridViewTextBoxColumn";
			// 
			// dataGridViewTextBoxColumn11
			// 
			this.dataGridViewTextBoxColumn11.DataPropertyName = "COMMENT";
			this.dataGridViewTextBoxColumn11.HeaderText = "COMMENT";
			this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
			// 
			// dataTable1BindingSource
			// 
			this.dataTable1BindingSource.DataMember = "CUT_CNC_MAIN";
			this.dataTable1BindingSource.DataSource = this.cNCTodoDataSet;
			// 
			// cUTCNCCUTLISTPARTSBindingSource
			// 
			this.cUTCNCCUTLISTPARTSBindingSource.DataMember = "CUT_CNC_CUTLIST_PARTS";
			this.cUTCNCCUTLISTPARTSBindingSource.DataSource = this.cNCTodoDataSet;
			// 
			// cUT_CNC_CUTLIST_PARTSTableAdapter
			// 
			this.cUT_CNC_CUTLIST_PARTSTableAdapter.ClearBeforeFill = true;
			// 
			// CncTodo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(780, 444);
			this.Controls.Add(this.tabControl1);
			this.Name = "CncTodo";
			this.Text = "CncTodo";
			this.Load += new System.EventHandler(this.CncTodo_Load);
			((System.ComponentModel.ISupportInitialize)(this.cUTCNCJOBSVIEW1BindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cNCTodoDataSet)).EndInit();
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataTable1BindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cUTCNCCUTLISTPARTSBindingSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.ColumnHeader columnHeader6;
		private System.Windows.Forms.ColumnHeader columnHeader7;
		private System.Windows.Forms.ColumnHeader columnHeader8;
		private System.Windows.Forms.ColumnHeader columnHeader9;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.ComboBox comboBox1;
		private CNCTodo.CNCTodoDataSet cNCTodoDataSet;
		private System.Windows.Forms.BindingSource cUTCNCJOBSVIEW1BindingSource;
		private CNCTodo.CNCTodoDataSetTableAdapters.CUT_CNC_JOBS_VIEW1TableAdapter cUT_CNC_JOBS_VIEW1TableAdapter;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.BindingSource cUTCNCCUTLISTPARTSBindingSource;
		private CNCTodo.CNCTodoDataSetTableAdapters.CUT_CNC_CUTLIST_PARTSTableAdapter cUT_CNC_CUTLIST_PARTSTableAdapter;
		private System.Windows.Forms.DataGridViewTextBoxColumn cLIDDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn pARTNUMDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn rEVDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn dESCRDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn sTATEIDDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn sTATEDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn fINLDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn fINWDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn tHICKNESSDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn oVERLDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn oVERWDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn cOMMENTDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
		private System.Windows.Forms.DataGridViewTextBoxColumn pARTIDDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn itemNumDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewCheckBoxColumn uPDATECNCDataGridViewCheckBoxColumn;
		private System.Windows.Forms.BindingSource dataTable1BindingSource;
		private CNCTodo.CNCTodoDataSetTableAdapters.CUT_CNC_MAINTableAdapter dataTable1TableAdapter;
		private System.Windows.Forms.DataGridViewTextBoxColumn pARTDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn dESCRIPTIONDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn cUTLISTDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
		private System.Windows.Forms.DataGridViewTextBoxColumn lDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn wDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn tDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
		private System.Windows.Forms.DataGridViewCheckBoxColumn uPDataGridViewCheckBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn cNC1DataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn cNC2DataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
	}
}