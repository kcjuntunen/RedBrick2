namespace RedBrick2 {
	partial class FormatFixtureBk {
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.recurse_cb = new System.Windows.Forms.CheckBox();
			this.pdf_rb = new System.Windows.Forms.RadioButton();
			this.slddrw_rb = new System.Windows.Forms.RadioButton();
			this.masterXLS_tb = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.xls_browse = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.saveAs_tb = new System.Windows.Forms.TextBox();
			this.saveAs_browse = new System.Windows.Forms.Button();
			this.cancel = new System.Windows.Forms.Button();
			this.go = new System.Windows.Forms.Button();
			this.action_lbl = new System.Windows.Forms.Label();
			this.target_lbl = new System.Windows.Forms.Label();
			this.openbtn = new System.Windows.Forms.Button();
			this.tableLayoutPanel1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
			this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.masterXLS_tb, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.xls_browse, 2, 2);
			this.tableLayoutPanel1.Controls.Add(this.label2, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.saveAs_tb, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.saveAs_browse, 2, 3);
			this.tableLayoutPanel1.Controls.Add(this.cancel, 2, 5);
			this.tableLayoutPanel1.Controls.Add(this.go, 1, 5);
			this.tableLayoutPanel1.Controls.Add(this.action_lbl, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.target_lbl, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.openbtn, 0, 5);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 6;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(605, 204);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// groupBox1
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.groupBox1, 2);
			this.groupBox1.Controls.Add(this.recurse_cb);
			this.groupBox1.Controls.Add(this.pdf_rb);
			this.groupBox1.Controls.Add(this.slddrw_rb);
			this.groupBox1.Location = new System.Drawing.Point(3, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(518, 57);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Drawing Source";
			// 
			// recurse_cb
			// 
			this.recurse_cb.AutoSize = true;
			this.recurse_cb.Location = new System.Drawing.Point(446, 21);
			this.recurse_cb.Name = "recurse_cb";
			this.recurse_cb.Size = new System.Drawing.Size(66, 17);
			this.recurse_cb.TabIndex = 2;
			this.recurse_cb.Text = "Recurse";
			this.recurse_cb.UseVisualStyleBackColor = true;
			// 
			// pdf_rb
			// 
			this.pdf_rb.AutoSize = true;
			this.pdf_rb.Location = new System.Drawing.Point(85, 21);
			this.pdf_rb.Name = "pdf_rb";
			this.pdf_rb.Size = new System.Drawing.Size(45, 17);
			this.pdf_rb.TabIndex = 1;
			this.pdf_rb.Text = "PDF";
			this.pdf_rb.UseVisualStyleBackColor = true;
			// 
			// slddrw_rb
			// 
			this.slddrw_rb.AutoSize = true;
			this.slddrw_rb.Checked = true;
			this.slddrw_rb.Location = new System.Drawing.Point(9, 21);
			this.slddrw_rb.Name = "slddrw_rb";
			this.slddrw_rb.Size = new System.Drawing.Size(70, 17);
			this.slddrw_rb.TabIndex = 0;
			this.slddrw_rb.TabStop = true;
			this.slddrw_rb.Text = "SLDDRW";
			this.slddrw_rb.UseVisualStyleBackColor = true;
			// 
			// masterXLS_tb
			// 
			this.masterXLS_tb.Location = new System.Drawing.Point(103, 96);
			this.masterXLS_tb.Name = "masterXLS_tb";
			this.masterXLS_tb.Size = new System.Drawing.Size(419, 22);
			this.masterXLS_tb.TabIndex = 2;
			this.masterXLS_tb.TextChanged += new System.EventHandler(this.masterXLS_tb_TextChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Right;
			this.label1.Location = new System.Drawing.Point(14, 93);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(83, 29);
			this.label1.TabIndex = 1;
			this.label1.Text = "Master XLS File";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// xls_browse
			// 
			this.xls_browse.Location = new System.Drawing.Point(528, 96);
			this.xls_browse.Name = "xls_browse";
			this.xls_browse.Size = new System.Drawing.Size(74, 23);
			this.xls_browse.TabIndex = 3;
			this.xls_browse.Text = "Browse...";
			this.xls_browse.UseVisualStyleBackColor = true;
			this.xls_browse.Click += new System.EventHandler(this.xls_browse_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Dock = System.Windows.Forms.DockStyle.Right;
			this.label2.Location = new System.Drawing.Point(52, 122);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(45, 29);
			this.label2.TabIndex = 4;
			this.label2.Text = "Save As";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// saveAs_tb
			// 
			this.saveAs_tb.Location = new System.Drawing.Point(103, 125);
			this.saveAs_tb.Name = "saveAs_tb";
			this.saveAs_tb.Size = new System.Drawing.Size(418, 22);
			this.saveAs_tb.TabIndex = 5;
			this.saveAs_tb.TextChanged += new System.EventHandler(this.saveAs_tb_TextChanged);
			// 
			// saveAs_browse
			// 
			this.saveAs_browse.Location = new System.Drawing.Point(528, 125);
			this.saveAs_browse.Name = "saveAs_browse";
			this.saveAs_browse.Size = new System.Drawing.Size(74, 23);
			this.saveAs_browse.TabIndex = 6;
			this.saveAs_browse.Text = "Browse...";
			this.saveAs_browse.UseVisualStyleBackColor = true;
			this.saveAs_browse.Click += new System.EventHandler(this.saveAs_browse_Click);
			// 
			// cancel
			// 
			this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancel.Location = new System.Drawing.Point(528, 178);
			this.cancel.Name = "cancel";
			this.cancel.Size = new System.Drawing.Size(74, 23);
			this.cancel.TabIndex = 7;
			this.cancel.Text = "Cancel";
			this.cancel.UseVisualStyleBackColor = true;
			this.cancel.Click += new System.EventHandler(this.cancel_Click);
			// 
			// go
			// 
			this.go.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.go.Location = new System.Drawing.Point(447, 178);
			this.go.Name = "go";
			this.go.Size = new System.Drawing.Size(75, 23);
			this.go.TabIndex = 8;
			this.go.Text = "Go";
			this.go.UseVisualStyleBackColor = true;
			this.go.Click += new System.EventHandler(this.go_Click);
			// 
			// action_lbl
			// 
			this.action_lbl.AutoSize = true;
			this.action_lbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.action_lbl.Location = new System.Drawing.Point(3, 63);
			this.action_lbl.Name = "action_lbl";
			this.action_lbl.Size = new System.Drawing.Size(94, 30);
			this.action_lbl.TabIndex = 9;
			this.action_lbl.Text = "Searching";
			this.action_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.action_lbl.Visible = false;
			// 
			// target_lbl
			// 
			this.target_lbl.AutoSize = true;
			this.target_lbl.Dock = System.Windows.Forms.DockStyle.Left;
			this.target_lbl.Location = new System.Drawing.Point(103, 63);
			this.target_lbl.Name = "target_lbl";
			this.target_lbl.Size = new System.Drawing.Size(0, 30);
			this.target_lbl.TabIndex = 10;
			this.target_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.target_lbl.Visible = false;
			// 
			// openbtn
			// 
			this.openbtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.openbtn.Enabled = false;
			this.openbtn.Location = new System.Drawing.Point(3, 178);
			this.openbtn.Name = "openbtn";
			this.openbtn.Size = new System.Drawing.Size(94, 23);
			this.openbtn.TabIndex = 11;
			this.openbtn.Text = "Open";
			this.openbtn.UseVisualStyleBackColor = true;
			this.openbtn.Click += new System.EventHandler(this.openbtn_Click);
			// 
			// FormatFixtureBk
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(605, 204);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MaximumSize = new System.Drawing.Size(617, 238);
			this.MinimumSize = new System.Drawing.Size(617, 238);
			this.Name = "FormatFixtureBk";
			this.ShowIcon = false;
			this.Text = "Format Fixture Book";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormatFixtureBk_FormClosing);
			this.Load += new System.EventHandler(this.FormatFixtureBk_Load);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox recurse_cb;
		private System.Windows.Forms.RadioButton pdf_rb;
		private System.Windows.Forms.RadioButton slddrw_rb;
		private System.Windows.Forms.TextBox masterXLS_tb;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button xls_browse;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox saveAs_tb;
		private System.Windows.Forms.Button saveAs_browse;
		private System.Windows.Forms.Button cancel;
		private System.Windows.Forms.Button go;
		private System.Windows.Forms.Label action_lbl;
		private System.Windows.Forms.Label target_lbl;
		private System.Windows.Forms.Button openbtn;
	}
}