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
			this.rawPartNo_tb.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rawPartNo_tb.Location = new System.Drawing.Point(0, 0);
			this.rawPartNo_tb.Name = "rawPartNo_tb";
			this.rawPartNo_tb.Size = new System.Drawing.Size(144, 22);
			this.rawPartNo_tb.TabIndex = 0;
			this.rawPartNo_tb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.rawPartNo_tb_KeyPress);
			// 
			// MaterialByNum
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(144, 34);
			this.ControlBox = false;
			this.Controls.Add(this.rawPartNo_tb);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(150, 58);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(150, 58);
			this.Name = "MaterialByNum";
			this.Opacity = 0.9D;
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