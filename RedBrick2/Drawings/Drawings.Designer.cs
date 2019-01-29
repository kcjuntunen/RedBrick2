namespace RedBrick2.Drawings {
	partial class Drawings {
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.std_drw_listBox = new System.Windows.Forms.ListBox();
			this.gENDRAWINGSBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.drawingDataSet = new RedBrick2.Drawings.DrawingDataSet();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.std_mdl_listBox = new System.Windows.Forms.ListBox();
			this.gENDRAWINGSEDRWBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.mtl_drw_listBox = new System.Windows.Forms.ListBox();
			this.gENDRAWINGSMTLBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.gEN_DRAWINGSTableAdapter = new RedBrick2.Drawings.DrawingDataSetTableAdapters.GEN_DRAWINGSTableAdapter();
			this.gEN_DRAWINGS_EDRWTableAdapter = new RedBrick2.Drawings.DrawingDataSetTableAdapters.GEN_DRAWINGS_EDRWTableAdapter();
			this.gEN_DRAWINGS_MTLTableAdapter = new RedBrick2.Drawings.DrawingDataSetTableAdapters.GEN_DRAWINGS_MTLTableAdapter();
			this.open_std_drw = new System.Windows.Forms.Button();
			this.open_std_mdl = new System.Windows.Forms.Button();
			this.open_mtl_drw = new System.Windows.Forms.Button();
			this.open_sw_std_drw = new System.Windows.Forms.Button();
			this.open_sw_mtl_drw = new System.Windows.Forms.Button();
			this.tableLayoutPanel1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gENDRAWINGSBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.drawingDataSet)).BeginInit();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gENDRAWINGSEDRWBindingSource)).BeginInit();
			this.groupBox3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gENDRAWINGSMTLBindingSource)).BeginInit();
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
			this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.groupBox2, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.groupBox3, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.textBox1, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(808, 459);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.std_drw_listBox);
			this.groupBox1.Controls.Add(this.open_sw_std_drw);
			this.groupBox1.Controls.Add(this.open_std_drw);
			this.groupBox1.Location = new System.Drawing.Point(3, 29);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(398, 210);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Standard Drawings";
			// 
			// std_drw_listBox
			// 
			this.std_drw_listBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.std_drw_listBox.DataSource = this.gENDRAWINGSBindingSource;
			this.std_drw_listBox.DisplayMember = "FName";
			this.std_drw_listBox.FormattingEnabled = true;
			this.std_drw_listBox.Location = new System.Drawing.Point(7, 17);
			this.std_drw_listBox.Name = "std_drw_listBox";
			this.std_drw_listBox.Size = new System.Drawing.Size(385, 160);
			this.std_drw_listBox.TabIndex = 3;
			this.std_drw_listBox.ValueMember = "FileID";
			// 
			// gENDRAWINGSBindingSource
			// 
			this.gENDRAWINGSBindingSource.DataMember = "GEN_DRAWINGS";
			this.gENDRAWINGSBindingSource.DataSource = this.drawingDataSet;
			// 
			// drawingDataSet
			// 
			this.drawingDataSet.DataSetName = "DrawingDataSet";
			this.drawingDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.std_mdl_listBox);
			this.groupBox2.Controls.Add(this.open_std_mdl);
			this.groupBox2.Location = new System.Drawing.Point(407, 29);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(398, 210);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Standard Models";
			// 
			// std_mdl_listBox
			// 
			this.std_mdl_listBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.std_mdl_listBox.DataSource = this.gENDRAWINGSEDRWBindingSource;
			this.std_mdl_listBox.DisplayMember = "FName";
			this.std_mdl_listBox.FormattingEnabled = true;
			this.std_mdl_listBox.Location = new System.Drawing.Point(6, 17);
			this.std_mdl_listBox.Name = "std_mdl_listBox";
			this.std_mdl_listBox.Size = new System.Drawing.Size(386, 160);
			this.std_mdl_listBox.TabIndex = 0;
			this.std_mdl_listBox.ValueMember = "FileID";
			// 
			// gENDRAWINGSEDRWBindingSource
			// 
			this.gENDRAWINGSEDRWBindingSource.DataMember = "GEN_DRAWINGS_EDRW";
			this.gENDRAWINGSEDRWBindingSource.DataSource = this.drawingDataSet;
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Controls.Add(this.open_sw_mtl_drw);
			this.groupBox3.Controls.Add(this.mtl_drw_listBox);
			this.groupBox3.Controls.Add(this.open_mtl_drw);
			this.groupBox3.Location = new System.Drawing.Point(3, 245);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(398, 211);
			this.groupBox3.TabIndex = 2;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Metal Drawings";
			// 
			// mtl_drw_listBox
			// 
			this.mtl_drw_listBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.mtl_drw_listBox.DataSource = this.gENDRAWINGSMTLBindingSource;
			this.mtl_drw_listBox.DisplayMember = "FName";
			this.mtl_drw_listBox.FormattingEnabled = true;
			this.mtl_drw_listBox.Location = new System.Drawing.Point(7, 20);
			this.mtl_drw_listBox.Name = "mtl_drw_listBox";
			this.mtl_drw_listBox.Size = new System.Drawing.Size(385, 147);
			this.mtl_drw_listBox.TabIndex = 0;
			this.mtl_drw_listBox.ValueMember = "FileID";
			// 
			// gENDRAWINGSMTLBindingSource
			// 
			this.gENDRAWINGSMTLBindingSource.DataMember = "GEN_DRAWINGS_MTL";
			this.gENDRAWINGSMTLBindingSource.DataSource = this.drawingDataSet;
			// 
			// textBox1
			// 
			this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBox1.Location = new System.Drawing.Point(407, 3);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(398, 20);
			this.textBox1.TabIndex = 0;
			this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(372, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(29, 26);
			this.label1.TabIndex = 3;
			this.label1.Text = "Filter";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// gEN_DRAWINGSTableAdapter
			// 
			this.gEN_DRAWINGSTableAdapter.ClearBeforeFill = true;
			// 
			// gEN_DRAWINGS_EDRWTableAdapter
			// 
			this.gEN_DRAWINGS_EDRWTableAdapter.ClearBeforeFill = true;
			// 
			// gEN_DRAWINGS_MTLTableAdapter
			// 
			this.gEN_DRAWINGS_MTLTableAdapter.ClearBeforeFill = true;
			// 
			// open_std_drw
			// 
			this.open_std_drw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.open_std_drw.Location = new System.Drawing.Point(7, 181);
			this.open_std_drw.Name = "open_std_drw";
			this.open_std_drw.Size = new System.Drawing.Size(155, 23);
			this.open_std_drw.TabIndex = 0;
			this.open_std_drw.Text = "Open Standard Drawing";
			this.open_std_drw.UseVisualStyleBackColor = true;
			this.open_std_drw.Click += new System.EventHandler(this.open_std_drw_Click);
			// 
			// open_std_mdl
			// 
			this.open_std_mdl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.open_std_mdl.Location = new System.Drawing.Point(6, 181);
			this.open_std_mdl.Name = "open_std_mdl";
			this.open_std_mdl.Size = new System.Drawing.Size(155, 23);
			this.open_std_mdl.TabIndex = 1;
			this.open_std_mdl.Text = "Open Standard Model";
			this.open_std_mdl.UseVisualStyleBackColor = true;
			this.open_std_mdl.Click += new System.EventHandler(this.open_std_mdl_Click);
			// 
			// open_mtl_drw
			// 
			this.open_mtl_drw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.open_mtl_drw.Location = new System.Drawing.Point(7, 173);
			this.open_mtl_drw.Name = "open_mtl_drw";
			this.open_mtl_drw.Size = new System.Drawing.Size(155, 23);
			this.open_mtl_drw.TabIndex = 2;
			this.open_mtl_drw.Text = "Open Metal Drawing";
			this.open_mtl_drw.UseVisualStyleBackColor = true;
			this.open_mtl_drw.Click += new System.EventHandler(this.open_mtl_drw_Click);
			// 
			// open_sw_std_drw
			// 
			this.open_sw_std_drw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.open_sw_std_drw.Location = new System.Drawing.Point(168, 181);
			this.open_sw_std_drw.Name = "open_sw_std_drw";
			this.open_sw_std_drw.Size = new System.Drawing.Size(155, 23);
			this.open_sw_std_drw.TabIndex = 3;
			this.open_sw_std_drw.Text = "Open Standard SW Drawing";
			this.open_sw_std_drw.UseVisualStyleBackColor = true;
			this.open_sw_std_drw.Click += new System.EventHandler(this.open_sw_std_drw_Click);
			// 
			// open_sw_mtl_drw
			// 
			this.open_sw_mtl_drw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.open_sw_mtl_drw.Location = new System.Drawing.Point(168, 173);
			this.open_sw_mtl_drw.Name = "open_sw_mtl_drw";
			this.open_sw_mtl_drw.Size = new System.Drawing.Size(155, 23);
			this.open_sw_mtl_drw.TabIndex = 4;
			this.open_sw_mtl_drw.Text = "Open Metal SW Drawing";
			this.open_sw_mtl_drw.UseVisualStyleBackColor = true;
			this.open_sw_mtl_drw.Click += new System.EventHandler(this.open_sw_mtl_drw_Click);
			// 
			// Drawings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(832, 483);
			this.Controls.Add(this.tableLayoutPanel1);
			this.MinimizeBox = false;
			this.Name = "Drawings";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Drawings";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Drawings_FormClosing);
			this.Load += new System.EventHandler(this.Drawings_Load);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.gENDRAWINGSBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.drawingDataSet)).EndInit();
			this.groupBox2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.gENDRAWINGSEDRWBindingSource)).EndInit();
			this.groupBox3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.gENDRAWINGSMTLBindingSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ListBox std_drw_listBox;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.BindingSource gENDRAWINGSBindingSource;
		private DrawingDataSet drawingDataSet;
		private DrawingDataSetTableAdapters.GEN_DRAWINGSTableAdapter gEN_DRAWINGSTableAdapter;
		private System.Windows.Forms.ListBox std_mdl_listBox;
		private System.Windows.Forms.BindingSource gENDRAWINGSEDRWBindingSource;
		private DrawingDataSetTableAdapters.GEN_DRAWINGS_EDRWTableAdapter gEN_DRAWINGS_EDRWTableAdapter;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.ListBox mtl_drw_listBox;
		private System.Windows.Forms.BindingSource gENDRAWINGSMTLBindingSource;
		private DrawingDataSetTableAdapters.GEN_DRAWINGS_MTLTableAdapter gEN_DRAWINGS_MTLTableAdapter;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button open_sw_std_drw;
		private System.Windows.Forms.Button open_mtl_drw;
		private System.Windows.Forms.Button open_std_mdl;
		private System.Windows.Forms.Button open_std_drw;
		private System.Windows.Forms.Button open_sw_mtl_drw;
	}
}