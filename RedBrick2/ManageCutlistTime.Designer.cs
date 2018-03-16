namespace RedBrick2 {
	partial class ManageCutlistTime {
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
			this.label2 = new System.Windows.Forms.Label();
			this.cutlistComboBox = new System.Windows.Forms.ComboBox();
			this.cutlistsBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.eNGINEERINGDataSet = new RedBrick2.ENGINEERINGDataSet();
			this.revTextBox = new System.Windows.Forms.TextBox();
			this.descrTextBox = new System.Windows.Forms.TextBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.partsListView = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.label4 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.partOpsListView = new System.Windows.Forms.ListView();
			this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.label3 = new System.Windows.Forms.Label();
			this.panel3 = new System.Windows.Forms.Panel();
			this.panel4 = new System.Windows.Forms.Panel();
			this.manageButton = new System.Windows.Forms.Button();
			this.allButton = new System.Windows.Forms.Button();
			this.noneButton = new System.Windows.Forms.Button();
			this.cutlistTimeListView = new System.Windows.Forms.ListView();
			this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.label5 = new System.Windows.Forms.Label();
			this.cutlistsTableAdapter = new RedBrick2.ENGINEERINGDataSetTableAdapters.CutlistsTableAdapter();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.cutlistsBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.eNGINEERINGDataSet)).BeginInit();
			this.panel2.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel4.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.panel4, 1, 0);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(13, 12);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 6;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.00003F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 49.99997F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(711, 461);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.66647F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33353F));
			this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.label2, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.cutlistComboBox, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.revTextBox, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.descrTextBox, 0, 2);
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 3;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(278, 109);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(179, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "Cutlist";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
			this.label2.Location = new System.Drawing.Point(188, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(87, 20);
			this.label2.TabIndex = 1;
			this.label2.Text = "Rev";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// cutlistComboBox
			// 
			this.cutlistComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.cutlistComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.cutlistComboBox.DataSource = this.cutlistsBindingSource;
			this.cutlistComboBox.DisplayMember = "CutlistDisplayName";
			this.cutlistComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cutlistComboBox.FormattingEnabled = true;
			this.cutlistComboBox.Location = new System.Drawing.Point(3, 23);
			this.cutlistComboBox.Name = "cutlistComboBox";
			this.cutlistComboBox.Size = new System.Drawing.Size(179, 21);
			this.cutlistComboBox.TabIndex = 2;
			this.cutlistComboBox.ValueMember = "CLID";
			this.cutlistComboBox.SelectedValueChanged += new System.EventHandler(this.cutlistComboBox_SelectedValueChanged);
			// 
			// cutlistsBindingSource
			// 
			this.cutlistsBindingSource.DataMember = "Cutlists";
			this.cutlistsBindingSource.DataSource = this.eNGINEERINGDataSet;
			// 
			// eNGINEERINGDataSet
			// 
			this.eNGINEERINGDataSet.DataSetName = "ENGINEERINGDataSet";
			this.eNGINEERINGDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// revTextBox
			// 
			this.revTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.cutlistsBindingSource, "REV", true));
			this.revTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.revTextBox.Location = new System.Drawing.Point(188, 23);
			this.revTextBox.Name = "revTextBox";
			this.revTextBox.Size = new System.Drawing.Size(87, 22);
			this.revTextBox.TabIndex = 3;
			// 
			// descrTextBox
			// 
			this.tableLayoutPanel2.SetColumnSpan(this.descrTextBox, 2);
			this.descrTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.cutlistsBindingSource, "DESCR", true));
			this.descrTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.descrTextBox.Location = new System.Drawing.Point(3, 53);
			this.descrTextBox.Name = "descrTextBox";
			this.descrTextBox.Size = new System.Drawing.Size(272, 22);
			this.descrTextBox.TabIndex = 4;
			// 
			// panel2
			// 
			this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel2.Controls.Add(this.partsListView);
			this.panel2.Controls.Add(this.label4);
			this.panel2.Location = new System.Drawing.Point(3, 218);
			this.panel2.Name = "panel2";
			this.tableLayoutPanel1.SetRowSpan(this.panel2, 2);
			this.panel2.Size = new System.Drawing.Size(278, 209);
			this.panel2.TabIndex = 2;
			// 
			// partsListView
			// 
			this.partsListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.partsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
			this.partsListView.Location = new System.Drawing.Point(3, 21);
			this.partsListView.Name = "partsListView";
			this.partsListView.Size = new System.Drawing.Size(272, 185);
			this.partsListView.TabIndex = 1;
			this.partsListView.UseCompatibleStateImageBehavior = false;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Part";
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Qty";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(3, 5);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(32, 13);
			this.label4.TabIndex = 0;
			this.label4.Text = "Parts";
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.partOpsListView);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Location = new System.Drawing.Point(287, 218);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(421, 109);
			this.panel1.TabIndex = 1;
			// 
			// partOpsListView
			// 
			this.partOpsListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.partOpsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader10});
			this.partOpsListView.Location = new System.Drawing.Point(4, 17);
			this.partOpsListView.Name = "partOpsListView";
			this.partOpsListView.Size = new System.Drawing.Size(414, 89);
			this.partOpsListView.TabIndex = 1;
			this.partOpsListView.UseCompatibleStateImageBehavior = false;
			// 
			// columnHeader7
			// 
			this.columnHeader7.Text = "Name";
			// 
			// columnHeader8
			// 
			this.columnHeader8.Text = "Operation";
			// 
			// columnHeader9
			// 
			this.columnHeader9.Text = "Setup";
			// 
			// columnHeader10
			// 
			this.columnHeader10.Text = "Run";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(3, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(88, 13);
			this.label3.TabIndex = 0;
			this.label3.Text = "Part Operations";
			// 
			// panel3
			// 
			this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel3.Location = new System.Drawing.Point(3, 118);
			this.panel3.Name = "panel3";
			this.tableLayoutPanel1.SetRowSpan(this.panel3, 2);
			this.panel3.Size = new System.Drawing.Size(278, 94);
			this.panel3.TabIndex = 3;
			// 
			// panel4
			// 
			this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel4.Controls.Add(this.manageButton);
			this.panel4.Controls.Add(this.allButton);
			this.panel4.Controls.Add(this.noneButton);
			this.panel4.Controls.Add(this.cutlistTimeListView);
			this.panel4.Controls.Add(this.label5);
			this.panel4.Location = new System.Drawing.Point(287, 3);
			this.panel4.Name = "panel4";
			this.tableLayoutPanel1.SetRowSpan(this.panel4, 3);
			this.panel4.Size = new System.Drawing.Size(421, 209);
			this.panel4.TabIndex = 4;
			// 
			// manageButton
			// 
			this.manageButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.manageButton.Location = new System.Drawing.Point(228, 9);
			this.manageButton.Name = "manageButton";
			this.manageButton.Size = new System.Drawing.Size(75, 23);
			this.manageButton.TabIndex = 4;
			this.manageButton.Text = "Manage";
			this.manageButton.UseVisualStyleBackColor = true;
			// 
			// allButton
			// 
			this.allButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.allButton.Location = new System.Drawing.Point(309, 9);
			this.allButton.Name = "allButton";
			this.allButton.Size = new System.Drawing.Size(45, 23);
			this.allButton.TabIndex = 3;
			this.allButton.Text = "All";
			this.allButton.UseVisualStyleBackColor = true;
			// 
			// noneButton
			// 
			this.noneButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.noneButton.Location = new System.Drawing.Point(360, 9);
			this.noneButton.Name = "noneButton";
			this.noneButton.Size = new System.Drawing.Size(57, 23);
			this.noneButton.TabIndex = 2;
			this.noneButton.Text = "None";
			this.noneButton.UseVisualStyleBackColor = true;
			// 
			// cutlistTimeListView
			// 
			this.cutlistTimeListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cutlistTimeListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
			this.cutlistTimeListView.Location = new System.Drawing.Point(4, 36);
			this.cutlistTimeListView.Name = "cutlistTimeListView";
			this.cutlistTimeListView.Size = new System.Drawing.Size(414, 170);
			this.cutlistTimeListView.TabIndex = 1;
			this.cutlistTimeListView.UseCompatibleStateImageBehavior = false;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Type";
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Op";
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "Setup";
			// 
			// columnHeader6
			// 
			this.columnHeader6.Text = "Run";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(1, 20);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(66, 13);
			this.label5.TabIndex = 0;
			this.label5.Text = "Cutlist Time";
			// 
			// cutlistsTableAdapter
			// 
			this.cutlistsTableAdapter.ClearBeforeFill = true;
			// 
			// ManageCutlistTime
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(736, 486);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "ManageCutlistTime";
			this.Text = "Manage Cutlist Time";
			this.Load += new System.EventHandler(this.ManageCutlistTime_Load);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.cutlistsBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.eNGINEERINGDataSet)).EndInit();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel4.ResumeLayout(false);
			this.panel4.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cutlistComboBox;
		private System.Windows.Forms.TextBox revTextBox;
		private System.Windows.Forms.TextBox descrTextBox;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.ListView partsListView;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ListView partOpsListView;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Button manageButton;
		private System.Windows.Forms.Button allButton;
		private System.Windows.Forms.Button noneButton;
		private System.Windows.Forms.ListView cutlistTimeListView;
		private System.Windows.Forms.Label label5;
		private ENGINEERINGDataSet eNGINEERINGDataSet;
		private System.Windows.Forms.BindingSource cutlistsBindingSource;
		private ENGINEERINGDataSetTableAdapters.CutlistsTableAdapter cutlistsTableAdapter;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader7;
		private System.Windows.Forms.ColumnHeader columnHeader8;
		private System.Windows.Forms.ColumnHeader columnHeader9;
		private System.Windows.Forms.ColumnHeader columnHeader10;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.ColumnHeader columnHeader6;
	}
}