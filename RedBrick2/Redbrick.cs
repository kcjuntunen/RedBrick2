using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swcommands;
using SolidWorks.Interop.swconst;
using SolidWorks.Interop.swpublished;

namespace RedBrick2 {
	/// <summary>
	/// The root of all redbrick stuff.
	/// </summary>
	public class Redbrick : ISwAddin {
		/// <summary>
		/// The connected application.
		/// </summary>
		public SldWorks swApp;
		private int cookie;
		private TaskpaneView taskpaneView;
		private SWTaskpaneHost taskpaneHost;
		private KeyValuePair<Version, Uri> publicVersion;
		private Version currentVersion;
		private bool askedToUpdate = false;
		private string UpdateMessage = string.Empty;
		/// <summary>
		/// Translation table of ugly field names to pretty ones.
		/// </summary>
		static public Dictionary<string, string> translation = new Dictionary<string, string>();
		/// <summary>
		/// A table of different actions to take given different field names.
		/// </summary>
		static public Dictionary<string, Format> action = new Dictionary<string, Format>();
		static private bool translationSetup = false;

		//private int count = 0;
		/// <summary>
		/// Hook up to Solidworks.
		/// </summary>
		/// <param name="ThisSW">The connected application.</param>
		/// <param name="Cookie">Yum.</param>
		/// <returns></returns>
		public bool ConnectToSW(object ThisSW, int Cookie) {
			swApp = (SldWorks)ThisSW;
			cookie = Cookie;

			bool res = swApp.SetAddinCallbackInfo(0, this, cookie);
			if (CheckNetwork()) {
				UISetup();
				return true;
			} else {
				swApp.SendMsgToUser2(Properties.Resources.NetworkNotAvailable,
					(int)swMessageBoxIcon_e.swMbWarning,
					(int)swMessageBoxBtn_e.swMbOk);
				return false;
			}
		}

		/// <summary>
		/// Try to find the company network.
		/// </summary>
		/// <returns>True or false.</returns>
		public static bool CheckNetwork() {
			bool res = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
			System.Net.NetworkInformation.Ping p = new System.Net.NetworkInformation.Ping();
			System.Net.NetworkInformation.PingReply pr = null;
			string host = Properties.Settings.Default.NetPath.ToString().Split('\\')[2];
			try {
				pr = p.Send(host, 1000);

				switch (pr.Status) {
					case System.Net.NetworkInformation.IPStatus.Success:
						res &= true;
						break;
					case System.Net.NetworkInformation.IPStatus.TimedOut:
						res &= false;
						break;
					case System.Net.NetworkInformation.IPStatus.DestinationHostUnreachable:
						res &= false;
						break;
					case System.Net.NetworkInformation.IPStatus.Unknown:
						res &= false;
						break;
					default:
						res &= false;
						break;
				}

			} catch (Exception) {
				res &= false;
			}

			return res;
		}

		/// <summary>
		/// Cleanly disconnect everything on shutdown.
		/// </summary>
		/// <returns></returns>
		public bool DisconnectFromSW() {
			CheckUpdate();
			this.UITearDown();
			return true;
		}

		private void UISetup() {
			SetupTranslationAndActionTables();
			ENGINEERINGDataSetTableAdapters.LegacyECRObjLookupTableAdapter leol =
				new ENGINEERINGDataSetTableAdapters.LegacyECRObjLookupTableAdapter();
			int _maxecr = 0;
			if (int.TryParse(leol.GetLastLegacyECR().ToString(), out _maxecr)) {
				LastLegacyECR = _maxecr;
			}

			TreeViewIcons = new System.Windows.Forms.ImageList();
			TreeViewIcons.ImageSize = new System.Drawing.Size(20, 20);
			TreeViewIcons.Images.Add(System.Drawing.Image.FromFile(@"G:\Solid Works\Amstore_Macros\ICONS\rev.png"));
			TreeViewIcons.Images.Add(System.Drawing.Image.FromFile(@"G:\Solid Works\Amstore_Macros\ICONS\ecr.png"));
			TreeViewIcons.Images.Add(System.Drawing.Image.FromFile(@"G:\Solid Works\Amstore_Macros\ICONS\calendar_icon.png"));
			TreeViewIcons.Images.Add(System.Drawing.Image.FromFile(@"G:\Solid Works\Amstore_Macros\ICONS\freelance-icon.bmp"));
			TreeViewIcons.Images.Add(System.Drawing.Image.FromFile(@"G:\Solid Works\Amstore_Macros\ICONS\icon_discuss.bmp"));
			TreeViewIcons.Images.Add(System.Drawing.Image.FromFile(@"G:\Solid Works\Amstore_Macros\ICONS\21-128.png"));


			try {
				Version cv = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
				string ver = cv.ToString();
				taskpaneView = swApp.CreateTaskpaneView2(Properties.Settings.Default.NetPath + Properties.Settings.Default.Icon,
						string.Format(Properties.Resources.Title, ver));

				taskpaneHost = (SWTaskpaneHost)taskpaneView.AddControl(SWTaskpaneHost.SWTASKPANE_PROGID, string.Empty);
				taskpaneHost.OnRequestSW += new Func<SldWorks>(delegate { return swApp; });

				bool result = taskpaneView.AddStandardButton((int)swTaskPaneBitmapsOptions_e.swTaskPaneBitmapsOptions_Ok, "OK");
				result = taskpaneView.AddStandardButton((int)swTaskPaneBitmapsOptions_e.swTaskPaneBitmapsOptions_Options, "Configuration");
				//result = taskpaneView.AddStandardButton((int)swTaskPaneBitmapsOptions_e.swTaskPaneBitmapsOptions_Close, "Close");
				result = taskpaneView.AddCustomButton(Properties.Settings.Default.NetPath + Properties.Settings.Default.RefreshIcon, "Refresh");
				result = taskpaneView.AddCustomButton(Properties.Settings.Default.NetPath + Properties.Settings.Default.ArchiveIcon, "Archive PDF");
				result = taskpaneView.AddCustomButton(Properties.Settings.Default.NetPath + Properties.Settings.Default.HelpIcon, "Usage Help");

				taskpaneView.TaskPaneToolbarButtonClicked += taskpaneView_TaskPaneToolbarButtonClicked;
				taskpaneHost.cookie = cookie;
				taskpaneHost.Start();
			} catch (Exception e) {
				System.Windows.Forms.MessageBox.Show(e.Message,
					@"Redbrick Error",
					System.Windows.Forms.MessageBoxButtons.OK,
					System.Windows.Forms.MessageBoxIcon.Error);
			}
		}

		int taskpaneView_TaskPaneToolbarButtonClicked(int ButtonIndex) {
			switch (ButtonIndex) {
				case 0:
					taskpaneHost.Write();
					if (Properties.Settings.Default.MakeSounds)
						System.Media.SystemSounds.Beep.Play();
					break;
				case 1:
					RedbrickConfiguration rbc = new RedbrickConfiguration();
					rbc.ShowDialog();
					taskpaneHost.ConnectSelection();
					taskpaneHost.ToggleFlameWar(Properties.Settings.Default.FlameWar);
					break;
				case 2:
					taskpaneHost.ReStart();
					break;
				case 3:
					ENGINEERINGDataSet.GEN_ODOMETERDataTable gota =
						new ENGINEERINGDataSet.GEN_ODOMETERDataTable();
					gota.IncrementOdometer(Functions.ArchivePDF);
					ArchivePDF.csproj.ArchivePDFWrapper apw = new ArchivePDF.csproj.ArchivePDFWrapper(swApp, GeneratePathSet());
					apw.Archive();
					break;
				case 4:
					System.Diagnostics.Process.Start(Properties.Settings.Default.UsageLink);
					break;
				default:
					break;
			}
			return 1;
		}

		private void UITearDown() {
			taskpaneHost = null;
			taskpaneView.DeleteView();
			Marshal.ReleaseComObject(taskpaneView);
			taskpaneView = null;
		}

		/// <summary>
		/// Copy the installer.
		/// </summary>
		public void CopyInstaller() {
			System.IO.FileInfo nfi = new System.IO.FileInfo(Properties.Settings.Default.InstallerNetworkPath);
			nfi.CopyTo(Properties.Settings.Default.EngineeringDir + @"\InstallRedBrick.exe", true);
		}

		/// <summary>
		/// Pull down the last odometer reset date.
		/// </summary>
		/// <param name="t">Full path of the proper XML file.</param>
		/// <returns>A DateTime object.</returns>
		public static DateTime GetOdometerStart(string t) {
			DateTime dt = Properties.Settings.Default.OdometerStart;
			System.IO.FileInfo pi = new System.IO.FileInfo(t);
			string elementName = string.Empty;
			using (System.Xml.XmlReader r = System.Xml.XmlReader.Create(pi.FullName)) {
				while (r.Read()) {
					if (r.NodeType == System.Xml.XmlNodeType.Element) {
						elementName = r.Name;
					} else if (r.NodeType == System.Xml.XmlNodeType.Text && r.HasValue && elementName == @"OdometerStart") {
						DateTime.TryParse(r.Value, out dt);
					}
				}
			}
			return dt;
		}

		/// <summary>
		/// Possible functions for the odometer.
		/// </summary>
		public enum Functions {
			/// <summary>Write data to db and SolidWorks.</summary>
			GreenCheck,         //0
			/// <summary>Archive PDFs to their appropriate places.</summary>
			ArchivePDF,         //1
			/// <summary>Complicated routine to insert ECR data.</summary>
			InsertECR,          //2
			/// <summary>The Examine BOM button.</summary>
			ExamineBOM,         //3
			/// <summary>The material list button.</summary>
			MaterialList,       //4
			/// <summary>Update of a single cutlist part.</summary>
			UpdateCutlistPart,  //5
			/// <summary>Update a whole cutlist.</summary>
			UpdateCutlist,      //6
			/// <summary>The fancy, recursive drawing collector.</summary>
			DrawingCollector,   //7
			/// <summary>The Weeke program exporter.</summary>
			ExportPrograms,     //8
			/// <summary>Converts tooling from one machine to another.</summary>
			ConvertPrograms,    //9
			/// <summary>Machine Priority dialog executed from SolidWorks.</summary>
			MachinePrioritySW,  //10
			/// <summary>Machine Priority dialog executed from AutoCAD.</summary>
			MachinePriorityACAD //11
		}

		/// <summary>
		/// Formats for handling different types on a TreeView.
		/// </summary>
		public enum Format {
			/// <summary>An author.</summary>
			NAME,
			/// <summary>An ordinary string.</summary>
			STRING,
			/// <summary>A date.</summary>
			DATE,
			/// <summary>An item to ignore.</summary>
			SKIP
		}

		private void SetupTranslationAndActionTables() {
			if (!translationSetup) {
				translation.Add(@"LGCYID", @"Legacy ID");
				translation.Add(@"DateRequested", @"Date Requested");
				translation.Add(@"DateStarted", @"Date Started");
				translation.Add(@"DateCompleted", @"Date Completed");
				translation.Add(@"AffectedParts", @"Affected Parts");
				translation.Add(@"Change", @"Change");
				translation.Add(@"Engineer", @"Engineer");
				translation.Add(@"Holder", @"Holder");
				translation.Add(@"ECR_NUM", @"ECR Number");
				translation.Add(@"ReqBy", @"Requested By");
				translation.Add(@"CHANGES", @"Change");
				translation.Add(@"STATUS", @"Status");
				translation.Add(@"ERR_DESC", @"Error Description");
				translation.Add(@"REVISION", @"Revision");
				translation.Add(@"DATE_CREATE", @"Date Created");


				action.Add(@"LGCYID", Format.SKIP);
				action.Add(@"DateRequested", Format.DATE);
				action.Add(@"DateStarted", Format.DATE);
				action.Add(@"DateCompleted", Format.DATE);
				action.Add(@"AffectedParts", Format.STRING);
				action.Add(@"Change", Format.STRING);
				action.Add(@"Engineer", Format.NAME);
				action.Add(@"Holder", Format.STRING);
				action.Add(@"ECR_NUM", Format.SKIP);
				action.Add(@"ReqBy", Format.NAME);
				action.Add(@"CHANGES", Format.STRING);
				action.Add(@"STATUS", Format.STRING);
				action.Add(@"ERR_DESC", Format.STRING);
				action.Add(@"REVISION", Format.STRING);
				action.Add(@"DATE_CREATE", Format.DATE);
				translationSetup = true;
			}
		}

		/// <summary>
		/// Get data from the version XML file that goes with the installer.
		/// </summary>
		/// <param name="t">Full path of target XML file.</param>
		private void GetPublicData(string t) {
			System.IO.FileInfo pi = new System.IO.FileInfo(t);
			Version v = new Version();
			Uri u = new Uri(pi.FullName);
			string m = string.Empty;
			string elementName = string.Empty;

			if (pi.Exists) {
				using (System.Xml.XmlReader r = System.Xml.XmlReader.Create(pi.FullName)) {
					while (r.Read()) {
						if (r.NodeType == System.Xml.XmlNodeType.Element) {
							elementName = r.Name;
						} else {
							if (r.NodeType == System.Xml.XmlNodeType.Text && r.HasValue) {
								switch (elementName) {
									case "version":
										v = new Version(r.Value);
										break;
									case "url":
										u = new Uri(r.Value);
										break;
									case "message":
										m = r.Value;
										break;
									default:
										break;
								}
							}
						}
					}
				}
			}
			publicVersion = new KeyValuePair<Version, Uri>(v, u);
			UpdateMessage = m;
		}

		/// <summary>
		/// Determine whether we're using a version older than the one on the network.
		/// </summary>
		/// <returns>A bool.</returns>
		public bool Old() {
			System.IO.FileInfo pi = new System.IO.FileInfo(Properties.Settings.Default.InstallerNetworkPath);
			if (true) {
				GetPublicData(pi.DirectoryName + @"\version.xml");
				currentVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

				if (currentVersion.CompareTo(publicVersion.Key) < 0)
					return true;
			}

			return false;
		}

		/// <summary>
		/// Ask user if upgrade is wanted.
		/// </summary>
		public void Update() {
			string chge = string.Format(Properties.Resources.Update, currentVersion.ToString(), publicVersion.Key.ToString());
			swMessageBoxResult_e res = (swMessageBoxResult_e)swApp.SendMsgToUser2(
				string.Format("{0}\n\nCHANGE: {1}", chge, UpdateMessage),
				(int)swMessageBoxIcon_e.swMbQuestion,
				(int)swMessageBoxBtn_e.swMbYesNo);

			switch (res) {
				case swMessageBoxResult_e.swMbHitAbort:
					break;
				case swMessageBoxResult_e.swMbHitCancel:
					break;
				case swMessageBoxResult_e.swMbHitIgnore:
					break;
				case swMessageBoxResult_e.swMbHitNo:
					break;
				case swMessageBoxResult_e.swMbHitOk:
					break;
				case swMessageBoxResult_e.swMbHitRetry:
					break;
				case swMessageBoxResult_e.swMbHitYes:
					//swApp.DestroyNotify += swApp_DestroyNotify;
					swApp_DestroyNotify();
					//swApp.SendMsgToUser2(Properties.Resources.Restart,
					//  (int)swMessageBoxIcon_e.swMbWarning,
					//  (int)swMessageBoxBtn_e.swMbOk);
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// Fire install process.
		/// </summary>
		/// <returns>0</returns>
		private int swApp_DestroyNotify() {
			System.Diagnostics.Process.Start(publicVersion.Value.ToString());
			return 0;
		}

		/// <summary>
		/// Update if old.
		/// </summary>
		public void CheckUpdate() {
			if (Old() && !askedToUpdate) {
				Update();
				askedToUpdate = true;
			}
		}

		/// <summary>
		/// Execute update.
		/// </summary>
		private void DoUpdate() {
			System.Diagnostics.Process p = new System.Diagnostics.Process();
			p.StartInfo.FileName = Properties.Settings.Default.EngineeringDir + @"\InstallRedBrick.exe";
			p.Start();
		}

		/// <summary>
		/// Correct names that have been annoyingly formatted to all caps.
		/// </summary>
		/// <param name="allCapsInput">An irritating string.</param>
		/// <returns>A more pleasantly formatted string.</returns>
		static public string TitleCase(string allCapsInput) {
			var t = new System.Globalization.CultureInfo("en-US", false).TextInfo;
			return t.ToTitleCase(allCapsInput.ToLower());
		}
		/// <summary>
		/// Hash a string.
		/// </summary>
		/// <param name="_fileinfo">FileInfo of a part.</param>
		/// <returns>A 32-bit int.</returns>
		static public int GetHash(System.IO.FileInfo _fileinfo) {

			DamienG.Security.Cryptography.Crc32 crc = new DamienG.Security.Cryptography.Crc32();

			byte[] b = new byte[_fileinfo.FullName.Length];
			string hash = string.Empty;

			for (int i = 0; i < _fileinfo.Length; i++)
				b[i] = (byte)_fileinfo.FullName[i];

			foreach (byte byt in crc.ComputeHash(b))
				hash += byt.ToString("x2").ToLower();

			try {
				return int.Parse(hash, System.Globalization.NumberStyles.HexNumber);
			} catch (Exception) {
				return 0;
			}
		}

		/// <summary>
		/// Hash a string.
		/// </summary>
		/// <param name="fullPath">Full path of a part.</param>
		/// <returns>A 32-bit int.</returns>
		static public int GetHash(string fullPath) {

			DamienG.Security.Cryptography.Crc32 crc = new DamienG.Security.Cryptography.Crc32();

			byte[] b = new byte[fullPath.Length];
			string hash = string.Empty;

			for (int i = 0; i < fullPath.Length; i++)
				b[i] = (byte)fullPath[i];

			foreach (byte byt in crc.ComputeHash(b))
				hash += byt.ToString("x2").ToLower();

			try {
				return int.Parse(hash, System.Globalization.NumberStyles.HexNumber);
			} catch (Exception) {
				return 0;
			}
		}

		/// <summary>
		/// Copy to_clip to clipboard.
		/// </summary>
		/// <param name="to_clip">A string.</param>
		static public void Clip(string to_clip) {
			if ((to_clip != null && to_clip != string.Empty) && to_clip != System.Windows.Forms.Clipboard.GetText()) {
				System.Windows.Forms.Clipboard.SetText(to_clip.Replace(Properties.Settings.Default.NotSavedMark, string.Empty));
				if (Properties.Settings.Default.MakeSounds) {
					try {
						System.Media.SoundPlayer sp = new System.Media.SoundPlayer(Properties.Settings.Default.ClipboardSound);
						sp.PlaySync();
					} catch (Exception ex) {
						System.Windows.Forms.MessageBox.Show(ex.Message,
							@"Redbrick Error",
							 System.Windows.Forms.MessageBoxButtons.OK,
							 System.Windows.Forms.MessageBoxIcon.Error);
					}
				}
			} else {
				if (Properties.Settings.Default.MakeSounds) {
					System.Media.SystemSounds.Asterisk.Play();
				}
			}
		}

		/// <summary>
		/// De-blue comboboxes.
		/// </summary>
		/// <param name="controls">A ControlCollection object.</param>
		static public void unselect(System.Windows.Forms.Control.ControlCollection controls) {
			foreach (System.Windows.Forms.Control c in controls) {
				if (c is System.Windows.Forms.ComboBox) {
					(c as System.Windows.Forms.ComboBox).SelectionLength = 0;
				}
			}
		}

		/// <summary>
		/// Accept string of a number, try to parse and conform to style.
		/// </summary>
		/// <param name="input">A string of a number.</param>
		/// <returns>An appropriately styled number.</returns>
		static public string enforce_number_format(string input) {
			double _val = 0.0F;
			if (double.TryParse(input, out _val)) {
				return string.Format(Properties.Settings.Default.NumberFormat, _val);
			}
			return @"#VALUE!";
		}

		/// <summary>
		/// Accept double of a number, try to parse and conform to style.
		/// </summary>
		/// <param name="input">A double of a number.</param>
		/// <returns>An appropriately styled number.</returns>
		static public string enforce_number_format(double input) {
			return string.Format(Properties.Settings.Default.NumberFormat, input);
		}

		/// <summary>
		/// Accept Single of a number, try to parse and conform to style.
		/// </summary>
		/// <param name="input">A Single of a number.</param>
		/// <returns>An appropriately styled number.</returns>
		static public string enforce_number_format(Single input) {
			return string.Format(Properties.Settings.Default.NumberFormat, input);
		}

		/// <summary>
		/// Accept decimal of a number, try to parse and conform to style.
		/// </summary>
		/// <param name="input">A decimal of a number.</param>
		/// <returns>An appropriately styled number.</returns>
		static public string enforce_number_format(decimal input) {
			return string.Format(Properties.Settings.Default.NumberFormat, input);
		}

		/// <summary>
		/// The Archiver needs a bunch of settings. It's done in an old, stupid way. Maybe I'll change it someday.
		/// </summary>
		/// <returns>A <see cref="ArchivePDF.csproj.PathSet"/> object.</returns>
		static public ArchivePDF.csproj.PathSet GeneratePathSet() {
			ArchivePDF.csproj.PathSet ps = new ArchivePDF.csproj.PathSet();
			ps.GaugePath = Properties.Settings.Default.GaugePath;
			ps.GaugeRegex = Properties.Settings.Default.GaugeRegex;
			ps.ShtFmtPath = Properties.Settings.Default.ShtFmtPath;
			ps.JPGPath = Properties.Settings.Default.JPGPath;
			ps.KPath = Properties.Settings.Default.KPath;
			ps.GPath = Properties.Settings.Default.GPath;
			ps.MetalPath = Properties.Settings.Default.MetalPath;
			ps.SaveFirst = Properties.Settings.Default.SaveFirst;
			ps.SilenceGaugeErrors = Properties.Settings.Default.SilenceGaugeErrors;
			ps.ExportPDF = Properties.Settings.Default.ExportPDF;
			ps.ExportEDrw = Properties.Settings.Default.ExportEDrw;
			ps.ExportImg = Properties.Settings.Default.ExportImg;
			ps.WriteToDb = true;
			ps.Initialated = true;
			return ps;
		}

		/// <summary>
		/// Generate and insert a fresh BOM from the first view in the current drawing.
		/// </summary>
		/// <param name="swApp">A running <see cref="SolidWorks.Interop.sldworks.SldWorks"/> object.</param>
		public static void InsertBOM(SldWorks swApp) {
			ModelDoc2 md = (ModelDoc2)swApp.ActiveDoc;
			DrawingDoc dd = (DrawingDoc)swApp.ActiveDoc;
			ModelDocExtension ex = (ModelDocExtension)md.Extension;
			int bom_type = (int)swBomType_e.swBomType_PartsOnly;
			int bom_numbering = (int)swNumberingType_e.swNumberingType_Flat;
			int bom_anchor = (int)swBOMConfigurationAnchorType_e.swBOMConfigurationAnchor_TopLeft;
			SolidWorks.Interop.sldworks.View v = GetFirstView(swApp);

			if (dd.ActivateView(v.Name)) {
				v.InsertBomTable4(
					false,
					(double)Properties.Settings.Default.BOMLocation.X * 0.01F,
					(double)Properties.Settings.Default.BOMLocation.Y * 0.01F,
					bom_anchor,
					bom_type,
					v.ReferencedConfiguration,
					Properties.Settings.Default.BOMTemplatePath,
					false,
					bom_numbering,
					false);
			}
		}

		/// <summary>
		/// Get the first view from a drawing.
		/// </summary>
		/// <param name="sw">Active SldWorks object.</param>
		/// <returns>A View object.</returns>
		public static SolidWorks.Interop.sldworks.View GetFirstView(SldWorks sw) {
			ModelDoc2 swModel = (ModelDoc2)sw.ActiveDoc;
			SolidWorks.Interop.sldworks.View v;
			DrawingDoc d = (DrawingDoc)swModel;
			string[] shtNames = (String[])d.GetSheetNames();
			string message = string.Empty;

			//This should find the first page with something on it.
			int x = 0;
			do {
				try {
					d.ActivateSheet(shtNames[x]);
				} catch (IndexOutOfRangeException e) {
					throw new IndexOutOfRangeException("Went beyond the number of sheets.", e);
				} catch (Exception e) {
					throw e;
				}
				v = (SolidWorks.Interop.sldworks.View)d.GetFirstView();
				v = (SolidWorks.Interop.sldworks.View)v.GetNextView();
				x++;
			} while ((v == null) && (x < d.GetSheetCount()));

			message = (string)v.GetName2() + ":\n";

			if (v == null) {
				throw new Exception("I couldn't find a model anywhere in this document.");
			}
			return v;
		}

		/// <summary>
		/// Attempt to resolve SolidWorks' strange dimension varables.
		/// </summary>
		/// <param name="md">The ModelDoc2 of the variable string in question.</param>
		/// <param name="prp">The variable string.</param>
		/// <returns>A string of a number hopefully. '#VALUE!' like Excel if we can't figure it out.</returns>
		public static string GetDim(ModelDoc2 md, string prp) {
			Dimension d = md.Parameter(prp);
			if (d != null) {
				return d.Value.ToString();
			} else {
				return DimensionByEquation(md, prp);
			}
		}

		/// <summary>
		/// Attempt to evaluate an EquationMgr type of equation.
		/// </summary>
		/// <param name="md">The ModelDoc2 of the variable string in question.</param>
		/// <param name="equation">The equation string.</param>
		/// <returns>A string of a number hopefully. '#VALUE!' like Excel if we can't figure it out.</returns>
		public static string DimensionByEquation(ModelDoc2 md, string equation) {
			string res = string.Empty;
			EquationMgr eqm = md.GetEquationMgr();
			for (int i = 0; i < eqm.GetCount(); i++) {
				if (eqm.get_Equation(i).Contains(equation)) {
					return eqm.get_Value(i).ToString();
				}
			}
			return @"#VALUE!";
		}

		/// <summary>
		/// Apply error colors to a Control.
		/// </summary>
		/// <param name="c">A control.</param>
		public static void Err(System.Windows.Forms.Control c) {
			c.ForeColor = Properties.Settings.Default.WarnForeground;
			c.BackColor = Properties.Settings.Default.WarnBackground;
		}

		/// <summary>
		/// Return a control to normal colors.
		/// </summary>
		/// <param name="c">A control.</param>
		public static void UnErr(System.Windows.Forms.Control c) {
			c.ForeColor = Properties.Settings.Default.NormalForeground;
			c.BackColor = Properties.Settings.Default.NormalBackground;
		}

		/// <summary>
		/// Apply warning colors to a Control.
		/// </summary>
		/// <param name="c">A control.</param>
		public static void Warn(System.Windows.Forms.Control c) {
			c.BackColor = Properties.Settings.Default.WarnForeground;
		}

		/// <summary>
		/// Swap the text of two TextBoxes.
		/// </summary>
		/// <param name="_left">TextBox A</param>
		/// <param name="_right">TextBox B</param>
		public static void SwapTextBoxContents(System.Windows.Forms.TextBox _left, System.Windows.Forms.TextBox _right) {
			string temp_ = _left.Text;
			_left.Text = _right.Text;
			_right.Text = temp_;
		}

		/// <summary>
		/// Get a lookup value for searching DB.
		/// </summary>
		/// <param name="_fi">A <see cref="System.IO.FileInfo"/> object.</param>
		/// <returns>A string for DB searches.</returns>
		static public string FileInfoToLookup(System.IO.FileInfo _fi) {
			return System.IO.Path.GetFileNameWithoutExtension(_fi.FullName).Split(' ')[0].Trim();
		}

		/// <summary>
		/// A central place for TreeViewIcons.
		/// </summary>
		static public System.Windows.Forms.ImageList TreeViewIcons { get; set; }

		/// <summary>
		/// The last legacy ECR is stored here so it doesn't have to be queried a lot.
		/// </summary>
		static public int LastLegacyECR { get; set; }

		/// <summary>
		/// Convert a specialized StringCollection to an array of strings.
		/// </summary>
		static public string[] BOMFilter {
			get {
				string[] regex_patterns = new string[Properties.Settings.Default.BOMFilter.Count];
				Properties.Settings.Default.BOMFilter.CopyTo(regex_patterns, 0);
				return regex_patterns;
			}
		}

		/// <summary>
		/// Convert a specialized StringCollection to an array of strings.
		/// </summary>
		static public string[] MasterHashes {
			get {
				string[] hs = new string[Properties.Settings.Default.MasterTableHashes.Count];
				Properties.Settings.Default.MasterTableHashes.CopyTo(hs, 0);
				return hs;
			}
		}

		[ComRegisterFunction()]
		private static void ComRegister(Type t) {
			Properties.Settings.Default.Upgrade();
			string keyPath = String.Format(@"SOFTWARE\SolidWorks\AddIns\{0:b}", t.GUID);

			using (Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(keyPath)) {
				rk.SetValue(null, 1); // Load at startup
				rk.SetValue("Title", "Redbrick2");
				rk.SetValue("Description", "Change properties the Amstore way.");
			}
		}

		[ComUnregisterFunction()]
		private static void ComUnregister(Type t) {
			string keyPath = String.Format(@"SOFTWARE\SolidWorks\AddIns\{0:b}", t.GUID);
			Microsoft.Win32.Registry.LocalMachine.DeleteSubKeyTree(keyPath);
		}
	}
}
