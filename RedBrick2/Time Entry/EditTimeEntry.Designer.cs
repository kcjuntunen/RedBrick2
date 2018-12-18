namespace RedBrick2.Time_Entry {
	partial class EditTimeEntry {
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
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.proj_cbx = new System.Windows.Forms.ComboBox();
			this.sCHPROJECTSBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.timeEntryDataSet = new RedBrick2.Time_Entry.TimeEntryDataSet();
			this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
			this.proc_cbx = new System.Windows.Forms.ComboBox();
			this.sCHPROCESSBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.hrs_tb = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.descr_tb = new System.Windows.Forms.TextBox();
			this.cust_tb = new System.Windows.Forms.TextBox();
			this.save_btn = new System.Windows.Forms.Button();
			this.discard_btn = new System.Windows.Forms.Button();
			this.sCH_PROJECTSTableAdapter = new RedBrick2.Time_Entry.TimeEntryDataSetTableAdapters.SCH_PROJECTSTableAdapter();
			this.sCH_PROCESSTableAdapter = new RedBrick2.Time_Entry.TimeEntryDataSetTableAdapters.SCH_PROCESSTableAdapter();
			this.sCHPROJECTSBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.sCHPROJECTSBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.timeEntryDataSet)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.sCHPROCESSBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.sCHPROJECTSBindingSource1)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.ColumnCount = 5;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.label5, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.proj_cbx, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.dateTimePicker1, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.proc_cbx, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.hrs_tb, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.label6, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.descr_tb, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.cust_tb, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.save_btn, 4, 4);
			this.tableLayoutPanel1.Controls.Add(this.discard_btn, 3, 4);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(13, 12);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 5;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(517, 168);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(97, 33);
			this.label1.TabIndex = 0;
			this.label1.Text = "Project";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.Location = new System.Drawing.Point(3, 33);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(97, 33);
			this.label2.TabIndex = 1;
			this.label2.Text = "Description";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label3.Location = new System.Drawing.Point(3, 66);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(97, 33);
			this.label3.TabIndex = 2;
			this.label3.Text = "Date";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label4.Location = new System.Drawing.Point(3, 99);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(97, 33);
			this.label4.TabIndex = 3;
			this.label4.Text = "Process";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label5.Location = new System.Drawing.Point(3, 132);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(97, 36);
			this.label5.TabIndex = 4;
			this.label5.Text = "Hours";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// proj_cbx
			// 
			this.proj_cbx.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.proj_cbx.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.proj_cbx.DataSource = this.sCHPROJECTSBindingSource;
			this.proj_cbx.DisplayMember = "DisplayName";
			this.proj_cbx.DropDownWidth = 250;
			this.proj_cbx.FormattingEnabled = true;
			this.proj_cbx.Location = new System.Drawing.Point(106, 3);
			this.proj_cbx.Name = "proj_cbx";
			this.proj_cbx.Size = new System.Drawing.Size(97, 21);
			this.proj_cbx.TabIndex = 5;
			this.proj_cbx.ValueMember = "PID";
			this.proj_cbx.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.proj_cbx_DrawItem);
			this.proj_cbx.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbx_KeyDown);
			// 
			// sCHPROJECTSBindingSource
			// 
			this.sCHPROJECTSBindingSource.DataMember = "SCH_PROJECTS";
			this.sCHPROJECTSBindingSource.DataSource = this.timeEntryDataSet;
			// 
			// timeEntryDataSet
			// 
			this.timeEntryDataSet.DataSetName = "TimeEntryDataSet";
			this.timeEntryDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// dateTimePicker1
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.dateTimePicker1, 2);
			this.dateTimePicker1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dateTimePicker1.Location = new System.Drawing.Point(106, 69);
			this.dateTimePicker1.Name = "dateTimePicker1";
			this.dateTimePicker1.Size = new System.Drawing.Size(200, 20);
			this.dateTimePicker1.TabIndex = 6;
			// 
			// proc_cbx
			// 
			this.proc_cbx.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.proc_cbx.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.proc_cbx.DataSource = this.sCHPROCESSBindingSource;
			this.proc_cbx.DisplayMember = "PROCESS";
			this.proc_cbx.Dock = System.Windows.Forms.DockStyle.Fill;
			this.proc_cbx.DropDownWidth = 150;
			this.proc_cbx.FormattingEnabled = true;
			this.proc_cbx.Location = new System.Drawing.Point(106, 102);
			this.proc_cbx.Name = "proc_cbx";
			this.proc_cbx.Size = new System.Drawing.Size(97, 21);
			this.proc_cbx.TabIndex = 7;
			this.proc_cbx.ValueMember = "PROCID";
			this.proc_cbx.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.proc_cbx_DrawItem);
			// 
			// sCHPROCESSBindingSource
			// 
			this.sCHPROCESSBindingSource.DataMember = "SCH_PROCESS";
			this.sCHPROCESSBindingSource.DataSource = this.timeEntryDataSet;
			// 
			// hrs_tb
			// 
			this.hrs_tb.Dock = System.Windows.Forms.DockStyle.Fill;
			this.hrs_tb.Location = new System.Drawing.Point(106, 135);
			this.hrs_tb.Name = "hrs_tb";
			this.hrs_tb.Size = new System.Drawing.Size(97, 20);
			this.hrs_tb.TabIndex = 8;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label6.Location = new System.Drawing.Point(209, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(97, 33);
			this.label6.TabIndex = 9;
			this.label6.Text = "Customer";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// descr_tb
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.descr_tb, 4);
			this.descr_tb.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.sCHPROJECTSBindingSource, "DESCRIPTION", true));
			this.descr_tb.Dock = System.Windows.Forms.DockStyle.Fill;
			this.descr_tb.Location = new System.Drawing.Point(106, 36);
			this.descr_tb.Name = "descr_tb";
			this.descr_tb.Size = new System.Drawing.Size(408, 20);
			this.descr_tb.TabIndex = 10;
			// 
			// cust_tb
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.cust_tb, 2);
			this.cust_tb.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.sCHPROJECTSBindingSource, "CUSTOMER", true));
			this.cust_tb.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cust_tb.Location = new System.Drawing.Point(312, 3);
			this.cust_tb.Name = "cust_tb";
			this.cust_tb.Size = new System.Drawing.Size(202, 20);
			this.cust_tb.TabIndex = 11;
			// 
			// save_btn
			// 
			this.save_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.save_btn.Location = new System.Drawing.Point(439, 142);
			this.save_btn.Name = "save_btn";
			this.save_btn.Size = new System.Drawing.Size(75, 23);
			this.save_btn.TabIndex = 12;
			this.save_btn.Text = "Save";
			this.save_btn.UseVisualStyleBackColor = true;
			this.save_btn.Click += new System.EventHandler(this.save_btn_Click);
			// 
			// discard_btn
			// 
			this.discard_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.discard_btn.Location = new System.Drawing.Point(334, 142);
			this.discard_btn.Name = "discard_btn";
			this.discard_btn.Size = new System.Drawing.Size(75, 23);
			this.discard_btn.TabIndex = 13;
			this.discard_btn.Text = "Discard";
			this.discard_btn.UseVisualStyleBackColor = true;
			this.discard_btn.Click += new System.EventHandler(this.discard_btn_Click);
			// 
			// sCH_PROJECTSTableAdapter
			// 
			this.sCH_PROJECTSTableAdapter.ClearBeforeFill = true;
			// 
			// sCH_PROCESSTableAdapter
			// 
			this.sCH_PROCESSTableAdapter.ClearBeforeFill = true;
			// 
			// sCHPROJECTSBindingSource1
			// 
			this.sCHPROJECTSBindingSource1.DataMember = "SCH_PROJECTS";
			this.sCHPROJECTSBindingSource1.DataSource = this.timeEntryDataSet;
			// 
			// EditTimeEntry
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(542, 192);
			this.ControlBox = false;
			this.Controls.Add(this.tableLayoutPanel1);
			this.MinimumSize = new System.Drawing.Size(550, 200);
			this.Name = "EditTimeEntry";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Edit Time Entry";
			this.Load += new System.EventHandler(this.EditTimeEntry_Load);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.sCHPROJECTSBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.timeEntryDataSet)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.sCHPROCESSBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.sCHPROJECTSBindingSource1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox proj_cbx;
		private System.Windows.Forms.DateTimePicker dateTimePicker1;
		private System.Windows.Forms.ComboBox proc_cbx;
		private System.Windows.Forms.TextBox hrs_tb;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox descr_tb;
		private System.Windows.Forms.TextBox cust_tb;
		private System.Windows.Forms.Button save_btn;
		private System.Windows.Forms.Button discard_btn;
		private TimeEntryDataSet timeEntryDataSet;
		private System.Windows.Forms.BindingSource sCHPROJECTSBindingSource;
		private TimeEntryDataSetTableAdapters.SCH_PROJECTSTableAdapter sCH_PROJECTSTableAdapter;
		private System.Windows.Forms.BindingSource sCHPROCESSBindingSource;
		private TimeEntryDataSetTableAdapters.SCH_PROCESSTableAdapter sCH_PROCESSTableAdapter;
		private System.Windows.Forms.BindingSource sCHPROJECTSBindingSource1;
	}
}