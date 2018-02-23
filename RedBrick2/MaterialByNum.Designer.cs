namespace RedBrick2 {
	partial class MaterialByNum {
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
			this.rawPartNo_tb = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// rawPartNo_tb
			// 
			this.rawPartNo_tb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.rawPartNo_tb.Location = new System.Drawing.Point(13, 13);
			this.rawPartNo_tb.Name = "rawPartNo_tb";
			this.rawPartNo_tb.Size = new System.Drawing.Size(107, 22);
			this.rawPartNo_tb.TabIndex = 0;
			this.rawPartNo_tb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.rawPartNo_tb_KeyPress);
			// 
			// MaterialByPart
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(132, 46);
			this.ControlBox = false;
			this.Controls.Add(this.rawPartNo_tb);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MaterialByPart";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Material By Part #";
			this.TopMost = true;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox rawPartNo_tb;
	}
}