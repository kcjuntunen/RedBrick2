namespace RedBrick2 {
	partial class AddToExistingCutlist {
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
			this.cutlist_cbx = new System.Windows.Forms.ComboBox();
			this.cutlistsBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.eNGINEERINGDataSet = new RedBrick2.ENGINEERINGDataSet();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.partq_nud = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.add_btn = new System.Windows.Forms.Button();
			this.cancel_btn = new System.Windows.Forms.Button();
			this.rev_cbx = new System.Windows.Forms.ComboBox();
			this.revListBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.revListTableAdapter = new RedBrick2.ENGINEERINGDataSetTableAdapters.RevListTableAdapter();
			this.cutlistsTableAdapter = new RedBrick2.ENGINEERINGDataSetTableAdapters.CutlistsTableAdapter();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.cutlistsBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.eNGINEERINGDataSet)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.partq_nud)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.revListBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.Controls.Add(this.cutlist_cbx, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.label2, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.partq_nud, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.label3, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.add_btn, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.cancel_btn, 2, 2);
			this.tableLayoutPanel1.Controls.Add(this.rev_cbx, 1, 2);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(338, 88);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// cutlist_cbx
			// 
			this.cutlist_cbx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cutlist_cbx.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.cutlist_cbx.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.tableLayoutPanel1.SetColumnSpan(this.cutlist_cbx, 2);
			this.cutlist_cbx.DataSource = this.cutlistsBindingSource;
			this.cutlist_cbx.DisplayMember = "CutlistDisplayName";
			this.cutlist_cbx.FormattingEnabled = true;
			this.cutlist_cbx.Location = new System.Drawing.Point(3, 16);
			this.cutlist_cbx.Name = "cutlist_cbx";
			this.cutlist_cbx.Size = new System.Drawing.Size(247, 21);
			this.cutlist_cbx.TabIndex = 3;
			this.cutlist_cbx.ValueMember = "CLID";
			this.cutlist_cbx.SelectedIndexChanged += new System.EventHandler(this.cutlist_cbx_SelectedIndexChanged);
			this.cutlist_cbx.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cutlist_cbx_KeyDown);
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
			this.label2.Location = new System.Drawing.Point(172, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(27, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "REV";
			this.label2.Visible = false;
			// 
			// partq_nud
			// 
			this.partq_nud.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.partq_nud.Location = new System.Drawing.Point(256, 16);
			this.partq_nud.Name = "partq_nud";
			this.partq_nud.Size = new System.Drawing.Size(79, 22);
			this.partq_nud.TabIndex = 0;
			this.partq_nud.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(256, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(49, 13);
			this.label3.TabIndex = 0;
			this.label3.Text = "Part QTY";
			// 
			// add_btn
			// 
			this.add_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.add_btn.Location = new System.Drawing.Point(3, 62);
			this.add_btn.Name = "add_btn";
			this.add_btn.Size = new System.Drawing.Size(75, 23);
			this.add_btn.TabIndex = 1;
			this.add_btn.Text = "Add";
			this.add_btn.UseVisualStyleBackColor = true;
			this.add_btn.Click += new System.EventHandler(this.add_btn_Click);
			// 
			// cancel_btn
			// 
			this.cancel_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancel_btn.Location = new System.Drawing.Point(260, 62);
			this.cancel_btn.Name = "cancel_btn";
			this.cancel_btn.Size = new System.Drawing.Size(75, 23);
			this.cancel_btn.TabIndex = 2;
			this.cancel_btn.Text = "Cancel";
			this.cancel_btn.UseVisualStyleBackColor = true;
			this.cancel_btn.Click += new System.EventHandler(this.cancel_btn_Click);
			// 
			// rev_cbx
			// 
			this.rev_cbx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.rev_cbx.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.cutlistsBindingSource, "REV", true));
			this.rev_cbx.DataSource = this.revListBindingSource;
			this.rev_cbx.DisplayMember = "REV";
			this.rev_cbx.FormattingEnabled = true;
			this.rev_cbx.Location = new System.Drawing.Point(172, 44);
			this.rev_cbx.Name = "rev_cbx";
			this.rev_cbx.Size = new System.Drawing.Size(78, 21);
			this.rev_cbx.TabIndex = 4;
			this.rev_cbx.ValueMember = "REV";
			this.rev_cbx.Visible = false;
			// 
			// revListBindingSource
			// 
			this.revListBindingSource.DataMember = "RevList";
			this.revListBindingSource.DataSource = this.eNGINEERINGDataSet;
			// 
			// revListTableAdapter
			// 
			this.revListTableAdapter.ClearBeforeFill = true;
			// 
			// cutlistsTableAdapter
			// 
			this.cutlistsTableAdapter.ClearBeforeFill = true;
			// 
			// AddToExistingCutlist
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(362, 112);
			this.ControlBox = false;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MinimumSize = new System.Drawing.Size(370, 120);
			this.Name = "AddToExistingCutlist";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Add To Existing Cutlist";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AddToExistingCutlist_FormClosing);
			this.Load += new System.EventHandler(this.AddToExistingCutlist_Load);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.cutlistsBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.eNGINEERINGDataSet)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.partq_nud)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.revListBindingSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.ComboBox cutlist_cbx;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown partq_nud;
		private System.Windows.Forms.ComboBox rev_cbx;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button add_btn;
		private System.Windows.Forms.Button cancel_btn;
		private ENGINEERINGDataSet eNGINEERINGDataSet;
		private System.Windows.Forms.BindingSource revListBindingSource;
		private ENGINEERINGDataSetTableAdapters.RevListTableAdapter revListTableAdapter;
		private System.Windows.Forms.BindingSource cutlistsBindingSource;
		private ENGINEERINGDataSetTableAdapters.CutlistsTableAdapter cutlistsTableAdapter;
	}
}