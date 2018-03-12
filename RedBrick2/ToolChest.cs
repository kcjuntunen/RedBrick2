using System;
using System.Windows.Forms;

using SolidWorks.Interop.sldworks;

namespace RedBrick2 {
	/// <summary>
	/// A landing pad for buttons that launch different tools.
	/// </summary>
	public partial class ToolChest : Form {
		private SldWorks swApp;
		/// <summary>
		/// Constructor.
		/// </summary>
		public ToolChest() {
			InitializeComponent();
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="s">Accepts a <see cref="SldWorks"/> object because some
		/// of these tools require it.</param>
		public ToolChest(SldWorks s) {
			swApp = s;
			InitializeComponent();
			Location = Properties.Settings.Default.ToolChestLocation;
		}

		private void button1_Click(object sender, EventArgs e) {
			using (FormatFixtureBk ffb = new FormatFixtureBk(swApp)) {
				ffb.ShowDialog(this);
			}
		}

		private void button2_Click(object sender, EventArgs e) {
			DrawingCompiler.SolidWorksMacro s = new DrawingCompiler.SolidWorksMacro();
			s.Main(swApp);
		}

		private void ToolChest_FormClosing(object sender, FormClosingEventArgs e) {
			Properties.Settings.Default.ToolChestLocation = Location;
			Properties.Settings.Default.Save();
		}

		private void button3_Click(object sender, EventArgs e) {
			System.IO.FileInfo fi_ = new System.IO.FileInfo((swApp.ActiveDoc as ModelDoc2).GetPathName());
			using (ECRViewer ev_ = new ECRViewer(Redbrick.FileInfoToLookup(fi_))) {
				ev_.ShowDialog(this);
			}
		}
	}
}
