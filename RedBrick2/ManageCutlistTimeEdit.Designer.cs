namespace RedBrick2 {
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
			this.label2 = new System.Windows.Forms.Label();
			this.cutlistComboBox = new System.Windows.Forms.ComboBox();
			this.revTextBox = new System.Windows.Forms.TextBox();
			this.manageCutlistTimeDataSet = new RedBrick2.ManageCutlistTimeDataSet();
			this.cutlistsBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.cutlistsTableAdapter = new RedBrick2.ManageCutlistTimeDataSetTableAdapters.CutlistsTableAdapter();
			this.cutlistTimeListView = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.manageCutlistTimeDataSet)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cutlistsBindingSource)).BeginInit();
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
			this.tableLayoutPanel1.Location = new System.Drawing.Point(13, 13);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(254, 446);
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
			this.tableLayoutPanel2.Controls.Add(this.label2, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.cutlistComboBox, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.revTextBox, 1, 1);
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
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(189, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(25, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Rev";
			// 
			// cutlistComboBox
			// 
			this.cutlistComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cutlistComboBox.DataSource = this.cutlistsBindingSource;
			this.cutlistComboBox.DisplayMember = "PARTNUM";
			this.cutlistComboBox.FormattingEnabled = true;
			this.cutlistComboBox.Location = new System.Drawing.Point(3, 16);
			this.cutlistComboBox.Name = "cutlistComboBox";
			this.cutlistComboBox.Size = new System.Drawing.Size(180, 21);
			this.cutlistComboBox.TabIndex = 2;
			this.cutlistComboBox.ValueMember = "CLID";
			this.cutlistComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cutlistComboBox_DrawItem);
			this.cutlistComboBox.SelectedIndexChanged += new System.EventHandler(this.cutlistComboBox_SelectedIndexChanged);
			// 
			// revTextBox
			// 
			this.revTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.revTextBox.BackColor = System.Drawing.SystemColors.Control;
			this.revTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.revTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.cutlistsBindingSource, "REV", true));
			this.revTextBox.Location = new System.Drawing.Point(189, 16);
			this.revTextBox.Name = "revTextBox";
			this.revTextBox.Size = new System.Drawing.Size(56, 22);
			this.revTextBox.TabIndex = 3;
			// 
			// manageCutlistTimeDataSet
			// 
			this.manageCutlistTimeDataSet.DataSetName = "ManageCutlistTimeDataSet";
			this.manageCutlistTimeDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// cutlistsBindingSource
			// 
			this.cutlistsBindingSource.DataMember = "Cutlists";
			this.cutlistsBindingSource.DataSource = this.manageCutlistTimeDataSet;
			// 
			// cutlistsTableAdapter
			// 
			this.cutlistsTableAdapter.ClearBeforeFill = true;
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
			this.cutlistTimeListView.Size = new System.Drawing.Size(248, 360);
			this.cutlistTimeListView.TabIndex = 1;
			this.cutlistTimeListView.UseCompatibleStateImageBehavior = false;
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
			// ManageCutlistTimeEdit
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(279, 471);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "ManageCutlistTimeEdit";
			this.Text = "Manage Cutlist Time";
			this.Load += new System.EventHandler(this.ManageCutlistTimeEdit_Load);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.manageCutlistTimeDataSet)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cutlistsBindingSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cutlistComboBox;
		private System.Windows.Forms.TextBox revTextBox;
		private ManageCutlistTimeDataSet manageCutlistTimeDataSet;
		private System.Windows.Forms.BindingSource cutlistsBindingSource;
		private ManageCutlistTimeDataSetTableAdapters.CutlistsTableAdapter cutlistsTableAdapter;
		private System.Windows.Forms.ListView cutlistTimeListView;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
	}
}