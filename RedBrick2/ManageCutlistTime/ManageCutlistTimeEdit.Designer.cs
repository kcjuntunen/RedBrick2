﻿namespace RedBrick2 {
	partial class ManageCutlistTimeEdit {
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
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.cutlistComboBox = new System.Windows.Forms.ComboBox();
			this.cutlistsBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.manageCutlistTimeDataSet = new RedBrick2.ManageCutlistTime.ManageCutlistTimeDataSet();
			this.cutlistTimeListView = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.update_btn = new System.Windows.Forms.Button();
			this.update_all_btn = new System.Windows.Forms.Button();
			this.delete_btn = new System.Windows.Forms.Button();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.setup_tb = new System.Windows.Forms.TextBox();
			this.run_tb = new System.Windows.Forms.TextBox();
			this.note_tb = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.op_chb = new System.Windows.Forms.CheckBox();
			this.op_sel_cb = new System.Windows.Forms.ComboBox();
			this.friendlyCutOpsBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
			this.clr_btn = new System.Windows.Forms.Button();
			this.add_btn = new System.Windows.Forms.Button();
			this.save_btn = new System.Windows.Forms.Button();
			this.cutlistsTableAdapter = new RedBrick2.ManageCutlistTime.ManageCutlistTimeDataSetTableAdapters.CutlistsTableAdapter();
			this.friendlyCutOpsTableAdapter = new RedBrick2.ManageCutlistTime.ManageCutlistTimeDataSetTableAdapters.FriendlyCutOpsTableAdapter();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.cutlistsBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.manageCutlistTimeDataSet)).BeginInit();
			this.tableLayoutPanel3.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.friendlyCutOpsBindingSource)).BeginInit();
			this.tableLayoutPanel5.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.cutlistTimeListView, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 3);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(13, 13);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 6;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85.71429F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(254, 494);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.cutlistComboBox, 0, 1);
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 2;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.Size = new System.Drawing.Size(248, 54);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Cutlist";
			// 
			// cutlistComboBox
			// 
			this.cutlistComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel2.SetColumnSpan(this.cutlistComboBox, 2);
			this.cutlistComboBox.DataSource = this.cutlistsBindingSource;
			this.cutlistComboBox.DisplayMember = "CutlistDisplayName";
			this.cutlistComboBox.FormattingEnabled = true;
			this.cutlistComboBox.Location = new System.Drawing.Point(3, 16);
			this.cutlistComboBox.Name = "cutlistComboBox";
			this.cutlistComboBox.Size = new System.Drawing.Size(242, 21);
			this.cutlistComboBox.TabIndex = 2;
			this.cutlistComboBox.ValueMember = "CLID";
			this.cutlistComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cutlistComboBox_DrawItem);
			this.cutlistComboBox.SelectedIndexChanged += new System.EventHandler(this.cutlistComboBox_SelectedIndexChanged);
			// 
			// cutlistsBindingSource
			// 
			this.cutlistsBindingSource.DataMember = "Cutlists";
			this.cutlistsBindingSource.DataSource = this.manageCutlistTimeDataSet;
			// 
			// manageCutlistTimeDataSet
			// 
			this.manageCutlistTimeDataSet.DataSetName = "ManageCutlistTimeDataSet";
			this.manageCutlistTimeDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// cutlistTimeListView
			// 
			this.cutlistTimeListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cutlistTimeListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
			this.cutlistTimeListView.Location = new System.Drawing.Point(3, 63);
			this.cutlistTimeListView.Name = "cutlistTimeListView";
			this.cutlistTimeListView.Size = new System.Drawing.Size(248, 220);
			this.cutlistTimeListView.TabIndex = 1;
			this.cutlistTimeListView.UseCompatibleStateImageBehavior = false;
			this.cutlistTimeListView.SelectedIndexChanged += new System.EventHandler(this.cutlistTimeListView_SelectedIndexChanged);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Type";
			this.columnHeader1.Width = 100;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Op";
			this.columnHeader2.Width = 40;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Setup";
			this.columnHeader3.Width = 100;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Run";
			this.columnHeader4.Width = 100;
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel3.ColumnCount = 3;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel3.Controls.Add(this.update_btn, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.update_all_btn, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.delete_btn, 2, 0);
			this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 289);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 1;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(248, 31);
			this.tableLayoutPanel3.TabIndex = 2;
			// 
			// update_btn
			// 
			this.update_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.update_btn.Location = new System.Drawing.Point(3, 3);
			this.update_btn.Name = "update_btn";
			this.update_btn.Size = new System.Drawing.Size(76, 25);
			this.update_btn.TabIndex = 0;
			this.update_btn.Text = "Update";
			this.update_btn.UseVisualStyleBackColor = true;
			this.update_btn.Click += new System.EventHandler(this.update_btn_Click);
			// 
			// update_all_btn
			// 
			this.update_all_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.update_all_btn.Location = new System.Drawing.Point(85, 3);
			this.update_all_btn.Name = "update_all_btn";
			this.update_all_btn.Size = new System.Drawing.Size(76, 25);
			this.update_all_btn.TabIndex = 1;
			this.update_all_btn.Text = "Update All";
			this.update_all_btn.UseVisualStyleBackColor = true;
			this.update_all_btn.Click += new System.EventHandler(this.update_all_btn_Click);
			// 
			// delete_btn
			// 
			this.delete_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.delete_btn.Location = new System.Drawing.Point(167, 3);
			this.delete_btn.Name = "delete_btn";
			this.delete_btn.Size = new System.Drawing.Size(78, 25);
			this.delete_btn.TabIndex = 2;
			this.delete_btn.Text = "Delete";
			this.delete_btn.UseVisualStyleBackColor = true;
			this.delete_btn.Click += new System.EventHandler(this.delete_btn_Click);
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel4.ColumnCount = 4;
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
			this.tableLayoutPanel4.Controls.Add(this.label3, 0, 0);
			this.tableLayoutPanel4.Controls.Add(this.label4, 0, 1);
			this.tableLayoutPanel4.Controls.Add(this.label5, 0, 2);
			this.tableLayoutPanel4.Controls.Add(this.setup_tb, 1, 0);
			this.tableLayoutPanel4.Controls.Add(this.run_tb, 1, 1);
			this.tableLayoutPanel4.Controls.Add(this.note_tb, 1, 2);
			this.tableLayoutPanel4.Controls.Add(this.label6, 2, 0);
			this.tableLayoutPanel4.Controls.Add(this.label7, 2, 1);
			this.tableLayoutPanel4.Controls.Add(this.op_chb, 3, 0);
			this.tableLayoutPanel4.Controls.Add(this.op_sel_cb, 3, 1);
			this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel5, 1, 3);
			this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 326);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 4;
			this.tableLayoutPanel1.SetRowSpan(this.tableLayoutPanel4, 2);
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel4.Size = new System.Drawing.Size(248, 134);
			this.tableLayoutPanel4.TabIndex = 3;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label3.Location = new System.Drawing.Point(3, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(43, 33);
			this.label3.TabIndex = 0;
			this.label3.Text = "Setup";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label4.Location = new System.Drawing.Point(3, 33);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(43, 33);
			this.label4.TabIndex = 1;
			this.label4.Text = "Run";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label5.Location = new System.Drawing.Point(3, 66);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(43, 33);
			this.label5.TabIndex = 2;
			this.label5.Text = "Note";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// setup_tb
			// 
			this.setup_tb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.setup_tb.Location = new System.Drawing.Point(52, 3);
			this.setup_tb.Name = "setup_tb";
			this.setup_tb.Size = new System.Drawing.Size(56, 22);
			this.setup_tb.TabIndex = 3;
			// 
			// run_tb
			// 
			this.run_tb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.run_tb.Location = new System.Drawing.Point(52, 36);
			this.run_tb.Name = "run_tb";
			this.run_tb.Size = new System.Drawing.Size(56, 22);
			this.run_tb.TabIndex = 4;
			// 
			// note_tb
			// 
			this.note_tb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.note_tb.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.tableLayoutPanel4.SetColumnSpan(this.note_tb, 3);
			this.note_tb.Location = new System.Drawing.Point(52, 69);
			this.note_tb.Name = "note_tb";
			this.note_tb.Size = new System.Drawing.Size(193, 22);
			this.note_tb.TabIndex = 5;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label6.Location = new System.Drawing.Point(114, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(31, 33);
			this.label6.TabIndex = 6;
			this.label6.Text = "hrs";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label7.Location = new System.Drawing.Point(114, 33);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(31, 33);
			this.label7.TabIndex = 7;
			this.label7.Text = "hrs";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// op_chb
			// 
			this.op_chb.AutoSize = true;
			this.op_chb.Location = new System.Drawing.Point(151, 3);
			this.op_chb.Name = "op_chb";
			this.op_chb.Size = new System.Drawing.Size(93, 17);
			this.op_chb.TabIndex = 8;
			this.op_chb.Text = "Linked to Op";
			this.op_chb.UseVisualStyleBackColor = true;
			this.op_chb.CheckedChanged += new System.EventHandler(this.op_chb_CheckedChanged);
			// 
			// op_sel_cb
			// 
			this.op_sel_cb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.op_sel_cb.DataSource = this.friendlyCutOpsBindingSource;
			this.op_sel_cb.DisplayMember = "OPNAME";
			this.op_sel_cb.DropDownWidth = 150;
			this.op_sel_cb.FormattingEnabled = true;
			this.op_sel_cb.Location = new System.Drawing.Point(151, 36);
			this.op_sel_cb.Name = "op_sel_cb";
			this.op_sel_cb.Size = new System.Drawing.Size(94, 21);
			this.op_sel_cb.TabIndex = 9;
			this.op_sel_cb.ValueMember = "OPID";
			this.op_sel_cb.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.op_sel_cb_DrawItem);
			// 
			// friendlyCutOpsBindingSource
			// 
			this.friendlyCutOpsBindingSource.DataMember = "FriendlyCutOps";
			this.friendlyCutOpsBindingSource.DataSource = this.manageCutlistTimeDataSet;
			// 
			// tableLayoutPanel5
			// 
			this.tableLayoutPanel5.ColumnCount = 3;
			this.tableLayoutPanel4.SetColumnSpan(this.tableLayoutPanel5, 3);
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel5.Controls.Add(this.clr_btn, 0, 0);
			this.tableLayoutPanel5.Controls.Add(this.add_btn, 1, 0);
			this.tableLayoutPanel5.Controls.Add(this.save_btn, 2, 0);
			this.tableLayoutPanel5.Location = new System.Drawing.Point(52, 102);
			this.tableLayoutPanel5.Name = "tableLayoutPanel5";
			this.tableLayoutPanel5.RowCount = 1;
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel5.Size = new System.Drawing.Size(193, 29);
			this.tableLayoutPanel5.TabIndex = 10;
			// 
			// clr_btn
			// 
			this.clr_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.clr_btn.Location = new System.Drawing.Point(3, 3);
			this.clr_btn.Name = "clr_btn";
			this.clr_btn.Size = new System.Drawing.Size(42, 23);
			this.clr_btn.TabIndex = 0;
			this.clr_btn.Text = "Clear";
			this.clr_btn.UseVisualStyleBackColor = true;
			this.clr_btn.Click += new System.EventHandler(this.clr_btn_Click);
			// 
			// add_btn
			// 
			this.add_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.add_btn.Location = new System.Drawing.Point(51, 3);
			this.add_btn.Name = "add_btn";
			this.add_btn.Size = new System.Drawing.Size(90, 23);
			this.add_btn.TabIndex = 1;
			this.add_btn.Text = "Add as New";
			this.add_btn.UseVisualStyleBackColor = true;
			this.add_btn.Click += new System.EventHandler(this.add_btn_Click);
			// 
			// save_btn
			// 
			this.save_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.save_btn.Location = new System.Drawing.Point(147, 3);
			this.save_btn.Name = "save_btn";
			this.save_btn.Size = new System.Drawing.Size(43, 23);
			this.save_btn.TabIndex = 2;
			this.save_btn.Text = "Save";
			this.save_btn.UseVisualStyleBackColor = true;
			this.save_btn.Click += new System.EventHandler(this.save_btn_Click);
			// 
			// cutlistsTableAdapter
			// 
			this.cutlistsTableAdapter.ClearBeforeFill = true;
			// 
			// friendlyCutOpsTableAdapter
			// 
			this.friendlyCutOpsTableAdapter.ClearBeforeFill = true;
			// 
			// ManageCutlistTimeEdit
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(279, 519);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ManageCutlistTimeEdit";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Manage Cutlist Time";
			this.Load += new System.EventHandler(this.ManageCutlistTimeEdit_Load);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.cutlistsBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.manageCutlistTimeDataSet)).EndInit();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.friendlyCutOpsBindingSource)).EndInit();
			this.tableLayoutPanel5.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cutlistComboBox;
		private ManageCutlistTime.ManageCutlistTimeDataSet manageCutlistTimeDataSet;
		private System.Windows.Forms.BindingSource cutlistsBindingSource;
		private ManageCutlistTime.ManageCutlistTimeDataSetTableAdapters.CutlistsTableAdapter cutlistsTableAdapter;
		private System.Windows.Forms.ListView cutlistTimeListView;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.Button update_btn;
		private System.Windows.Forms.Button update_all_btn;
		private System.Windows.Forms.Button delete_btn;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox setup_tb;
		private System.Windows.Forms.TextBox run_tb;
		private System.Windows.Forms.TextBox note_tb;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.CheckBox op_chb;
		private System.Windows.Forms.ComboBox op_sel_cb;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
		private System.Windows.Forms.Button clr_btn;
		private System.Windows.Forms.Button add_btn;
		private System.Windows.Forms.Button save_btn;
		private System.Windows.Forms.BindingSource friendlyCutOpsBindingSource;
		private ManageCutlistTime.ManageCutlistTimeDataSetTableAdapters.FriendlyCutOpsTableAdapter friendlyCutOpsTableAdapter;
	}
}