using System;
using System.Threading;
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
			ShowHideButtons();
			ShowAccessControlledButtons();
			Deactivate += ToolChest_Deactivate;
		}

		void ShowHideButtons() {
			button7.Enabled = swApp.ActiveDoc is PartDoc;
			button2.Enabled = swApp.ActiveDoc != null && !(swApp.ActiveDoc is PartDoc);
			if (swApp.ActiveDoc is DrawingDoc) {
				SolidWorks.Interop.sldworks.View v_ = Redbrick.GetFirstView(swApp);
				button2.Enabled = !(v_ == null) && v_.ReferencedDocument != null;
			}
			button9.Visible = true;
			button10.Visible = true;
		}

		void ShowAccessControlledButtons() {
			bool show = Redbrick.IsDeveloper() || Redbrick.IsSuperAdmin();
			button8.Visible = show;
		}

		private void ToolChest_Deactivate(object sender, EventArgs e) {
			Close();
		}
		private void button1_Click(object sender, EventArgs e) {
			try {
				using (FormatFixtureBk ffb = new FormatFixtureBk(swApp)) {
					ffb.ShowInTaskbar = true;
					ffb.ShowDialog();
				}
			} catch (Exception ex) {
				Redbrick.ProcessError(ex);
			}
			Close();
		}

		private void button2_Click(object sender, EventArgs e) {
			if (swApp.ActiveDoc == null) {
				Close();
				return;
			}
			try {
				using (DrawingCollector.DrawingCollector dc_ = new DrawingCollector.DrawingCollector(swApp)) {
					dc_.ShowDialog(this);
				}
			} catch (Exception ex) {
				Redbrick.ProcessError(ex);
			}
			Close();
		}

		private void ToolChest_FormClosing(object sender, FormClosingEventArgs e) {
			Properties.Settings.Default.ToolChestLocation = Location;
			Properties.Settings.Default.Save();
		}

		static private void ecrViewer(object obj) {
			string lookup = obj != null ? obj as string : string.Empty;
			if (lookup == string.Empty) {
				int maxecr = 0;
				using (RedbrickDataSetTableAdapters.QueriesTableAdapter q_ = new RedbrickDataSetTableAdapters.QueriesTableAdapter()) {
					maxecr = Convert.ToInt32(q_.MaxEcrNum());
					try {
						using (ECRViewer.ECRViewer ev_ = new ECRViewer.ECRViewer(maxecr)) {
							ev_.ShowInTaskbar = true;
							ev_.ShowDialog();
						}
					} catch (Exception ex) {
						Redbrick.ProcessError(ex);
					}
				}
			} else {
				try {
					using (ECRViewer.ECRViewer ev_ = new ECRViewer.ECRViewer(lookup)) {
						ev_.ShowInTaskbar = true;
						ev_.ShowDialog();
					}
				} catch (Exception ex) {
					Redbrick.ProcessError(ex);
				}
			}
		}

		private void button3_Click(object sender, EventArgs e) {
			ParameterizedThreadStart pts = new ParameterizedThreadStart(ecrViewer);
			Thread t = new Thread(pts);
			t.SetApartmentState(ApartmentState.STA);
			t.Start(lookup);
			Close();
		}

		static void QTThread(object obj) {
			string lookup = obj != null ? obj as string : string.Empty;
			if (lookup == string.Empty) {
				try {
					using (QuickTracLookup qt_ = new QuickTracLookup()) {
						qt_.ShowInTaskbar = true;
						qt_.ShowDialog();
					}
				} catch (Exception ex) {
					Redbrick.ProcessError(ex);
				}
			} else {
				try {
					using (QuickTracLookup qt_ = new QuickTracLookup(lookup)) {
						qt_.ShowInTaskbar = true;
						qt_.ShowDialog();
					}
				} catch (Exception ex) {
					Redbrick.ProcessError(ex);
				}
			}
		}

		private void button4_Click(object sender, EventArgs e) {
			ParameterizedThreadStart pts = new ParameterizedThreadStart(QTThread);
			Thread t = new Thread(pts);
			t.SetApartmentState(ApartmentState.STA);
			t.Start(lookup);
			Close();
		}

		static private void mct(object obj) {
			string lookup = obj != null ? obj as string : string.Empty;
			if (lookup == string.Empty) {
				using (ManageCutlistTime.ManageCutlistTime mct_ = new ManageCutlistTime.ManageCutlistTime()) {
					mct_.ShowInTaskbar = true;
					mct_.ShowDialog();
				}
			} else {
				using (ManageCutlistTime.ManageCutlistTime mct_ = new ManageCutlistTime.ManageCutlistTime(lookup)) {
					mct_.ShowInTaskbar = true;
					mct_.ShowDialog();
				}
			}
		}

		private void button5_Click(object sender, EventArgs e) {
			ParameterizedThreadStart pts = new ParameterizedThreadStart(mct);
			Thread t = new Thread(pts);
			t.SetApartmentState(ApartmentState.STA);
			t.Start(lookup);
			Close();
		}

		static private void rc() {
			using (RenameCutlist rc_ = new RenameCutlist(Properties.Settings.Default.LastCutlist)) {
				rc_.ShowInTaskbar = true;
				rc_.ShowDialog();
			}
		}

		private void button6_Click(object sender, EventArgs e) {
			Thread t = new Thread(new ThreadStart(rc));
			t.SetApartmentState(ApartmentState.STA);
			t.Start();
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

		static void LaunchErrorLog() {
			using (ErrorLog.ErrorLog er_ = new ErrorLog.ErrorLog()) {
				er_.ShowDialog();
			}
		}

		private void button8_Click(object sender, EventArgs e) {
			Thread t = new Thread(new ThreadStart(LaunchErrorLog));
			t.SetApartmentState(ApartmentState.STA);
			t.Start();
			Close();
		}

		static void LaunchCNCMAIN() {
			using (CncTodo c = new CncTodo()) {
				c.ShowDialog();
			}
		}

		static void LaunchCutlistOverView() {
			using (CutlistTableDisplay.CutlistTableDisplayForm ctd =
				new CutlistTableDisplay.CutlistTableDisplayForm()) {
				ctd.ShowInTaskbar = true;
				try {
					ctd.ShowDialog();
				} catch(Exception e) {
					Redbrick.ProcessError(e);
				}
			}
		}


		private void button9_MouseClick(object sender, MouseEventArgs e) {
			Thread t = new Thread(new ThreadStart(LaunchCNCMAIN));
			t.SetApartmentState(ApartmentState.STA);
			t.Start();
			Close();
		}

		static void LaunchTimeEntry() {
			using (Time_Entry.TimeEntry te = new Time_Entry.TimeEntry()) {
				te.ShowDialog();
			}
		}

		private void button10_Click(object sender, EventArgs e) {
			Thread t = new Thread(new ThreadStart(LaunchTimeEntry));
			t.SetApartmentState(ApartmentState.STA);
			t.Start();
			Close();
		}

		private void ctl_ovr_btn_Click(object sender, EventArgs e) {
			Thread t = new Thread(new ThreadStart(LaunchCutlistOverView));
			t.SetApartmentState(ApartmentState.STA);
			t.Start();
			Close();
		}
	}
}
