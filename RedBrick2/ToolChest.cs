﻿using System;
using System.Windows.Forms;

using SolidWorks.Interop.sldworks;

namespace RedBrick2 {
	/// <summary>
	/// A landing pad for buttons that launch different tools.
	/// </summary>
	public partial class ToolChest : Form {
		private SldWorks swApp;
		private string lookup = string.Empty;
		/// <summary>
		/// Constructor.
		/// </summary>
		public ToolChest() {
			InitializeComponent();
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="lk">A part lookup string.</param>
		/// <param name="s">Accepts a <see cref="SldWorks"/> object because some
		/// of these tools require it.</param>
		public ToolChest(string lk, SldWorks s) {
			swApp = s;
			lookup = lk;
			InitializeComponent();
			Location = Properties.Settings.Default.ToolChestLocation;
			button7.Enabled = swApp.ActiveDoc is PartDoc;
			Deactivate += ToolChest_Deactivate;
		}

		private void ToolChest_Deactivate(object sender, EventArgs e) {
			Close();
		}

		private void button1_Click(object sender, EventArgs e) {
			using (FormatFixtureBk ffb = new FormatFixtureBk(swApp)) {
				ffb.ShowDialog(this);
			}
			Close();
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
			using (ECRViewer.ECRViewer ev_ = new ECRViewer.ECRViewer(lookup)) {
				ev_.ShowDialog(this);
			}
			Close();
		}

		private void button4_Click(object sender, EventArgs e) {
			using (QuickTracLookup qt_ = new QuickTracLookup(lookup)) {
				qt_.ShowDialog(this);
			}
			Close();
		}

		private void button5_Click(object sender, EventArgs e) {
			using (ManageCutlistTime mct_ = new ManageCutlistTime(lookup)) {
				mct_.ShowDialog(this);
			}
			Close();
		}

		private void button6_Click(object sender, EventArgs e) {
			using (RenameCutlist rc_ = new RenameCutlist(Properties.Settings.Default.LastCutlist)) {
				rc_.ShowDialog(this);
			}
			Close();
		}

		private void button7_Click(object sender, EventArgs e) {
			if (!(swApp.ActiveDoc is PartDoc)) {
				return;
			}
			(swApp.ActiveDoc as PartDoc).ExportFlatPatternView(@"C:\Optimize\Import\flat\temp.dxf",
				(int)SolidWorks.Interop.swconst.swExportFlatPatternViewOptions_e.swExportFlatPatternOption_None);
			Close();
		}
	}
}
