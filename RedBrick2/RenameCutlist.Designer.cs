namespace RedBrick2 {
	partial class RenameCutlist {
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
			this.label1 = new System.Windows.Forms.Label();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.label2 = new System.Windows.Forms.Label();
			this.from_cbx = new System.Windows.Forms.ComboBox();
			this.cUTCUTLISTSBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.eNGINEERINGDataSet = new RedBrick2.ENGINEERINGDataSet();
			this.to_tbx = new System.Windows.Forms.TextBox();
			this.rev_cbx = new System.Windows.Forms.ComboBox();
			this.revListBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.rename_button = new System.Windows.Forms.Button();
			this.close_btn = new System.Windows.Forms.Button();
			this.drw_tb = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.cust_cbx = new System.Windows.Forms.ComboBox();
			this.gENCUSTOMERSBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.label4 = new System.Windows.Forms.Label();
			this.revListTableAdapter = new RedBrick2.ENGINEERINGDataSetTableAdapters.RevListTableAdapter();
			this.cUT_CUTLISTSTableAdapter = new RedBrick2.ENGINEERINGDataSetTableAdapters.CUT_CUTLISTSTableAdapter();
			this.gEN_CUSTOMERSTableAdapter = new RedBrick2.ENGINEERINGDataSetTableAdapters.GEN_CUSTOMERSTableAdapter();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.cUTCUTLISTSBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.eNGINEERINGDataSet)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.revListBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gENCUSTOMERSBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(76, 27);
			this.label1.TabIndex = 0;
			this.label1.Text = "From:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.22222F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55.55556F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.22222F));
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.from_cbx, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.to_tbx, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.rev_cbx, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.rename_button, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.close_btn, 2, 4);
			this.tableLayoutPanel1.Controls.Add(this.drw_tb, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.cust_cbx, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 5;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(370, 138);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.Location = new System.Drawing.Point(3, 27);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(76, 27);
			this.label2.TabIndex = 1;
			this.label2.Text = "To:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// from_cbx
			// 
			this.from_cbx.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.from_cbx.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.from_cbx.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.tableLayoutPanel1.SetColumnSpan(this.from_cbx, 2);
			this.from_cbx.DataSource = this.cUTCUTLISTSBindingSource;
			this.from_cbx.DisplayMember = "PARTNUM";
			this.from_cbx.FormattingEnabled = true;
			this.from_cbx.Location = new System.Drawing.Point(85, 3);
			this.from_cbx.Name = "from_cbx";
			this.from_cbx.Size = new System.Drawing.Size(282, 21);
			this.from_cbx.TabIndex = 2;
			this.from_cbx.ValueMember = "CLID";
			this.from_cbx.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.from_cbx_DrawItem);
			this.from_cbx.SelectedIndexChanged += new System.EventHandler(this.from_cbx_SelectedIndexChanged);
			this.from_cbx.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbx_KeyDown);
			// 
			// cUTCUTLISTSBindingSource
			// 
			this.cUTCUTLISTSBindingSource.DataMember = "CUT_CUTLISTS";
			this.cUTCUTLISTSBindingSource.DataSource = this.eNGINEERINGDataSet;
			// 
			// eNGINEERINGDataSet
			// 
			this.eNGINEERINGDataSet.DataSetName = "ENGINEERINGDataSet";
			this.eNGINEERINGDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// to_tbx
			// 
			this.to_tbx.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.to_tbx.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.to_tbx.Location = new System.Drawing.Point(85, 30);
			this.to_tbx.Name = "to_tbx";
			this.to_tbx.Size = new System.Drawing.Size(199, 22);
			this.to_tbx.TabIndex = 3;
			this.to_tbx.TextChanged += new System.EventHandler(this.tbx_TextChanged);
			// 
			// rev_cbx
			// 
			this.rev_cbx.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.rev_cbx.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.rev_cbx.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.rev_cbx.DataSource = this.revListBindingSource;
			this.rev_cbx.DisplayMember = "REV";
			this.rev_cbx.FormattingEnabled = true;
			this.rev_cbx.Location = new System.Drawing.Point(290, 30);
			this.rev_cbx.Name = "rev_cbx";
			this.rev_cbx.Size = new System.Drawing.Size(77, 21);
			this.rev_cbx.TabIndex = 4;
			this.rev_cbx.ValueMember = "REV";
			this.rev_cbx.TextChanged += new System.EventHandler(this.cbx_TextChanged);
			this.rev_cbx.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbx_KeyDown);
			// 
			// revListBindingSource
			// 
			this.revListBindingSource.DataMember = "RevList";
			this.revListBindingSource.DataSource = this.eNGINEERINGDataSet;
			// 
			// rename_button
			// 
			this.rename_button.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.rename_button.Location = new System.Drawing.Point(3, 111);
			this.rename_button.Name = "rename_button";
			this.rename_button.Size = new System.Drawing.Size(76, 24);
			this.rename_button.TabIndex = 6;
			this.rename_button.Text = "Rename";
			this.rename_button.UseVisualStyleBackColor = true;
			this.rename_button.Click += new System.EventHandler(this.rename_button_Click);
			// 
			// close_btn
			// 
			this.close_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.close_btn.Location = new System.Drawing.Point(290, 111);
			this.close_btn.Name = "close_btn";
			this.close_btn.Size = new System.Drawing.Size(77, 24);
			this.close_btn.TabIndex = 5;
			this.close_btn.Text = "Cancel";
			this.close_btn.UseVisualStyleBackColor = true;
			this.close_btn.Click += new System.EventHandler(this.close_btn_Click);
			// 
			// drw_tb
			// 
			this.drw_tb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.drw_tb.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.tableLayoutPanel1.SetColumnSpan(this.drw_tb, 2);
			this.drw_tb.Location = new System.Drawing.Point(85, 57);
			this.drw_tb.Name = "drw_tb";
			this.drw_tb.Size = new System.Drawing.Size(282, 22);
			this.drw_tb.TabIndex = 7;
			this.drw_tb.TextChanged += new System.EventHandler(this.tbx_TextChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label3.Location = new System.Drawing.Point(3, 54);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(76, 27);
			this.label3.TabIndex = 8;
			this.label3.Text = "Drawing Ref:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// cust_cbx
			// 
			this.cust_cbx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cust_cbx.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.cust_cbx.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.tableLayoutPanel1.SetColumnSpan(this.cust_cbx, 2);
			this.cust_cbx.DataSource = this.gENCUSTOMERSBindingSource;
			this.cust_cbx.DisplayMember = "CUSTOMER";
			this.cust_cbx.FormattingEnabled = true;
			this.cust_cbx.Location = new System.Drawing.Point(85, 84);
			this.cust_cbx.Name = "cust_cbx";
			this.cust_cbx.Size = new System.Drawing.Size(282, 21);
			this.cust_cbx.TabIndex = 9;
			this.cust_cbx.ValueMember = "CUSTID";
			this.cust_cbx.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cust_cbx_DrawItem);
			this.cust_cbx.TextChanged += new System.EventHandler(this.cbx_TextChanged);
			this.cust_cbx.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbx_KeyDown);
			// 
			// gENCUSTOMERSBindingSource
			// 
			this.gENCUSTOMERSBindingSource.DataMember = "GEN_CUSTOMERS";
			this.gENCUSTOMERSBindingSource.DataSource = this.eNGINEERINGDataSet;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label4.Location = new System.Drawing.Point(3, 81);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(76, 27);
			this.label4.TabIndex = 10;
			this.label4.Text = "Customer:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// revListTableAdapter
			// 
			this.revListTableAdapter.ClearBeforeFill = true;
			// 
			// cUT_CUTLISTSTableAdapter
			// 
			this.cUT_CUTLISTSTableAdapter.ClearBeforeFill = true;
			// 
			// gEN_CUSTOMERSTableAdapter
			// 
			this.gEN_CUSTOMERSTableAdapter.ClearBeforeFill = true;
			// 
			// RenameCutlist
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(402, 170);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(410, 200);
			this.Name = "RenameCutlist";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Rename Cutlist";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RenameCutlist_FormClosing);
			this.Load += new System.EventHandler(this.RenameCutlist_Load);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.cUTCUTLISTSBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.eNGINEERINGDataSet)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.revListBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gENCUSTOMERSBindingSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox from_cbx;
		private System.Windows.Forms.TextBox to_tbx;
		private System.Windows.Forms.ComboBox rev_cbx;
		private ENGINEERINGDataSet eNGINEERINGDataSet;
		private System.Windows.Forms.BindingSource revListBindingSource;
		private ENGINEERINGDataSetTableAdapters.RevListTableAdapter revListTableAdapter;
		private System.Windows.Forms.Button close_btn;
		private System.Windows.Forms.Button rename_button;
		private System.Windows.Forms.BindingSource cUTCUTLISTSBindingSource;
		private ENGINEERINGDataSetTableAdapters.CUT_CUTLISTSTableAdapter cUT_CUTLISTSTableAdapter;
		private System.Windows.Forms.TextBox drw_tb;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cust_cbx;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.BindingSource gENCUSTOMERSBindingSource;
		private ENGINEERINGDataSetTableAdapters.GEN_CUSTOMERSTableAdapter gEN_CUSTOMERSTableAdapter;
	}
}