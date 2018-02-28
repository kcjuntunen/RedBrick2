using System;
using System.Windows.Forms;

using SolidWorks.Interop.sldworks;

namespace RedBrick2 {
	public partial class ToolChest : Form {
		private SldWorks swApp;
		public ToolChest() {
			InitializeComponent();
		}

		public ToolChest(SldWorks s) {
			swApp = s;
			InitializeComponent();
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
	}
}
