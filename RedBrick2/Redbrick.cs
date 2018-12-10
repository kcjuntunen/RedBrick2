using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Linq;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SolidWorks.Interop.swpublished;

namespace RedBrick2 {
	/// <summary>
	/// The root of all redbrick stuff.
	/// </summary>
	[ComVisible(true)]
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

		static private int _uacc = -1;
		static public int UACC
		{
			get
			{
				if (_uacc > -1) {
					return _uacc;
				}
				using (ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter ta =
					new ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter()) {
					_uacc = Convert.ToInt32(ta.GetAccessLevel(System.Environment.UserName));
				}
				return _uacc;
			}
			private set { _uacc = value; }
		}

		static private int _uid = -1;
		static public int UID
		{
			get
			{
				if (_uid > -1) {
					return _uid;
				}
				using (ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter ta =
					new ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter()) {
					_uid = Convert.ToInt32(ta.GetUID(System.Environment.UserName));
				}
				return _uid;
			}
			private set { _uid = value; }
		}

		public static bool IsDeveloper() {
			return ((UACC & 32) == 32);
		}

		public static bool IsSuperAdmin() {
			return ((UACC & 64) == 64);
		}

		static private string _fullName = string.Empty;
		static public string FullName
		{
			get
			{
				if (_fullName != string.Empty) {
					return _fullName;
				}
				using(ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter ta =
					new ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter()) {
					_fullName = Convert.ToString(ta.GetCurrentUserFullname(System.Environment.UserName));
				}
				return _fullName;
			}
			private set { _fullName = value; }
		}

		static private string _initial = string.Empty;
		static public string Initial
		{
			get
			{
				if (_initial != string.Empty) {
					return _initial;
				}
				using (ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter ta =
					new ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter()) {
					_initial = ta.GetUserInitial(System.Environment.UserName);
				}
				return _initial;
			}
			private set { _initial = value; }
		}

		static public System.Windows.Forms.AutoCompleteStringCollection ECRNos { get; set; } = new System.Windows.Forms.AutoCompleteStringCollection();
		static public System.Windows.Forms.AutoCompleteStringCollection ECRDescriptions { get; set; } = new System.Windows.Forms.AutoCompleteStringCollection();
		static public string LastECRNo { get; set; } = string.Empty;
		/// <summary>
		/// A place to store the last ECR description that was typed.
		/// </summary>
		static public string LastECRDescription { get; set; } = string.Empty;
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
		[ComVisible(true)]
		public bool ConnectToSW(object ThisSW, int Cookie) {
			swApp = (SldWorks)ThisSW;
			cookie = Cookie;

			bool res = swApp.SetAddinCallbackInfo2(0, this, cookie);
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
		[ComVisible(true)]
		public bool DisconnectFromSW() {
			this.UITearDown();
			return true;
		}

		private void UISetup() {
			CheckUpdate();
			SetupTranslationAndActionTables();
			using (ENGINEERINGDataSetTableAdapters.LegacyECRObjLookupTableAdapter leol =
				new ENGINEERINGDataSetTableAdapters.LegacyECRObjLookupTableAdapter()) {
				int _maxecr = 0;
				if (int.TryParse(leol.GetLastLegacyECR().ToString(), out _maxecr)) {
					LastLegacyECR = _maxecr;
				}
			}

			//AppDomain appDomain = AppDomain.CurrentDomain;
			//appDomain.UnhandledException += AppDomain_UnhandledException;

			TreeViewIcons = new System.Windows.Forms.ImageList();
			TreeViewIcons.ImageSize = new System.Drawing.Size(20, 20);
			TreeViewIcons.Images.Add(System.Drawing.Image.FromFile(@"G:\Solid Works\Amstore_Macros\ICONS\rev.png"));
			TreeViewIcons.Images.Add(System.Drawing.Image.FromFile(@"G:\Solid Works\Amstore_Macros\ICONS\ecr.png"));
			TreeViewIcons.Images.Add(System.Drawing.Image.FromFile(@"G:\Solid Works\Amstore_Macros\ICONS\calendar_icon.png"));
			TreeViewIcons.Images.Add(System.Drawing.Image.FromFile(@"G:\Solid Works\Amstore_Macros\ICONS\freelance-icon.bmp"));
			TreeViewIcons.Images.Add(System.Drawing.Image.FromFile(@"G:\Solid Works\Amstore_Macros\ICONS\icon_discuss.bmp"));
			TreeViewIcons.Images.Add(System.Drawing.Image.FromFile(@"G:\Solid Works\Amstore_Macros\ICONS\21-128.png"));
			TreeViewIcons.Images.Add(System.Drawing.Image.FromFile(@"G:\Solid Works\Amstore_Macros\ICONS\Archive-icon.bmp"));


			try {
				Version cv = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
				string ver = cv.ToString();
				taskpaneView = swApp.CreateTaskpaneView2(Properties.Settings.Default.NetPath + Properties.Settings.Default.Icon,
						string.Format(Properties.Resources.Title, ver));
				taskpaneHost = (SWTaskpaneHost)taskpaneView.AddControl(SWTaskpaneHost.SWTASKPANE_PROGID, string.Empty);
				taskpaneHost.OnRequestSW += new Func<SldWorks>(delegate { return swApp; });

				//bool result = taskpaneView.AddStandardButton((int)swTaskPaneBitmapsOptions_e.swTaskPaneBitmapsOptions_Ok, "OK");
				//result = taskpaneView.AddStandardButton((int)swTaskPaneBitmapsOptions_e.swTaskPaneBitmapsOptions_Options, "Configuration");
				//result = taskpaneView.AddCustomButton(Properties.Settings.Default.NetPath + Properties.Settings.Default.RefreshIcon, "Refresh");
				//result = taskpaneView.AddCustomButton(Properties.Settings.Default.NetPath + Properties.Settings.Default.ArchiveIcon, "Archive PDF");
				//result = taskpaneView.AddCustomButton(Properties.Settings.Default.NetPath + Properties.Settings.Default.GlassesIcon, "QuikTrac Lookup");
				//result = taskpaneView.AddCustomButton(Properties.Settings.Default.NetPath + Properties.Settings.Default.ToolsIcon, "Tools");
				
				//taskpaneView.TaskPaneToolbarButtonClicked += taskpaneView_TaskPaneToolbarButtonClicked;
				taskpaneHost.cookie = cookie;
				taskpaneHost.Start();
			} catch (Exception e_) {
				ProcessError(e_);
			}
		}

		private void add_menu_items() {
			System.Reflection.Assembly thisAssembly = default(System.Reflection.Assembly);
			int mId_ = 0;
			string[] images = new string[3];

			thisAssembly = System.Reflection.Assembly.GetAssembly(this.GetType());
			mId_ = swApp.AddMenu((int)swDocumentTypes_e.swDocDRAWING, @"Redbrick Tools", 0);
			mId_ = swApp.AddMenuItem5((int)swDocumentTypes_e.swDocDRAWING, cookie, @"Reformat Fixture Book@Redbrick Tools", 0,
				@"RenumberFB", @"RenumberFBEnableMethod",
				@"Renumber drawings in a fixture book according to a formatted Excel document.",
				new string[3]);

			thisAssembly = null;
		}

		private void RenumberFB() {
			using (FormatFixtureBk ffb = new FormatFixtureBk(swApp)) {
				ffb.ShowDialog(taskpaneHost);
			}
		}

		private void AppDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
			Exception e_ = (Exception)e.ExceptionObject;
			string output_ = string.Format("{0}\n{1}\n{2}", e_.Message, e_.StackTrace, e_.Source);
			System.Windows.Forms.MessageBox.Show(output_, @"Redbrick Error - " + e_.HResult.ToString(),
				System.Windows.Forms.MessageBoxButtons.OK, 
				System.Windows.Forms.MessageBoxIcon.Error);
		}

		private void QuikTracLookup() {
			if (taskpaneHost.ModelRedbrickIsNotNull && taskpaneHost.ActiveDocIsNotNull) {
				using (ENGINEERINGDataSetTableAdapters.CLIENT_STUFFTableAdapter ta_ =
					new ENGINEERINGDataSetTableAdapters.CLIENT_STUFFTableAdapter()) {
					System.IO.FileInfo fi_ = new System.IO.FileInfo(taskpaneHost.ActiveDocPathName);
					string lu_ = FileInfoToLookup(fi_);
					using (QuickTracLookup qt_ = new QuickTracLookup(lu_)) {
						qt_.ShowDialog(taskpaneHost);
					}
				}
			}
		}

		private void ArchivePDF() {
			if (taskpaneHost.ModelRedbrickActiveDoc.GetType() == (int)swDocumentTypes_e.swDocDRAWING) {
				using (ENGINEERINGDataSet.GEN_ODOMETERDataTable gota =
					new ENGINEERINGDataSet.GEN_ODOMETERDataTable()) {
					gota.IncrementOdometer(Functions.ArchivePDF);
					ArchivePDF.csproj.ArchivePDFWrapper apw = new ArchivePDF.csproj.ArchivePDFWrapper(swApp, GeneratePathSet());
					apw.Archive();
				}
				taskpaneHost.GetFileDates();
			}
		}

		private bool changed = false;
		private void ConfigureRedbrick() {
			using (RedbrickConfiguration rbc = new RedbrickConfiguration()) {
				rbc.ChangedStuff += Rbc_ChangedStuff;
				rbc.ShowDialog(taskpaneHost);
				rbc.ChangedStuff -= Rbc_ChangedStuff;
			}

			if (changed) {
				taskpaneHost.ConnectSelection(true);
				taskpaneHost.ToggleFlameWar(Properties.Settings.Default.FlameWar);
			}
			changed = false;
		}

		private void Default_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
			changed = true;
		}

		private void Rbc_ChangedStuff(object sender, EventArgs e) {
			changed = true;
		}

		private void GreenCheck() {
			taskpaneHost.Write();
			if (Properties.Settings.Default.MakeSounds)
				System.Media.SystemSounds.Beep.Play();
		}

		private void OpenToolChest() {
			string lk_ = Redbrick.FileInfoToLookup(taskpaneHost.PartFileInfo);
			using (ToolChest t_ = new ToolChest(lk_, swApp)) {
				t_.ShowDialog(taskpaneHost);
			}
		}

		int taskpaneView_TaskPaneToolbarButtonClicked(int ButtonIndex) {
			switch (ButtonIndex) {
				case 0:
					GreenCheck();
					break;
				case 1:
					ConfigureRedbrick();
					break;
				case 2:
					taskpaneHost.ReStart();
					break;
				//case 3:
				//	ArchivePDF();
				//	break;
				//case 4:
				//	QuikTracLookup();
				//	break;
				case 3:
					OpenToolChest();
					break;
				default:
					break;
			}
			return 1;
		}

		private void UITearDown() {
			taskpaneHost.DisconnectStuff();
			taskpaneHost.Dispose();
			taskpaneView.DeleteView();
			Marshal.ReleaseComObject(taskpaneView);
			Marshal.ReleaseComObject(swApp);
			GC.Collect();
			GC.WaitForPendingFinalizers();
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
			MachinePriorityACAD,//11
			/// <summary>Part added to cutlist via 'Add part...' button.</summary>
			AddPart,            //12
			/// <summary>Part removed from cutlist via 'Remove...' button.</summary>
			RemovePart          //13
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
				translation.Add(@"DateRequested", @"Requested");
				translation.Add(@"DateStarted", @"Started");
				translation.Add(@"DateCompleted", @"Completed");
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
					swApp.DestroyNotify += swApp_DestroyNotify;
					swApp.ExitApp();
					//swApp_DestroyNotify();
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
			if (allCapsInput != null) {
				var t = new System.Globalization.CultureInfo("en-US", false).TextInfo;
				return t.ToTitleCase(allCapsInput.ToLower()).Trim();
			}
			return Properties.Settings.Default.ValErr;
		}

		/// <summary>
		/// An attempt to overcome weird tabbing. Focus where you clicked.
		/// </summary>
		/// <param name="sender">A control object.</param>
		/// <param name="e">Mouse Event Args</param>
		static public void FocusHere(object sender, System.Windows.Forms.MouseEventArgs e) {
			if (sender is System.Windows.Forms.ComboBox) {
				if ((sender as System.Windows.Forms.ComboBox).DroppedDown) {
					//
				} else {
					(sender as System.Windows.Forms.ComboBox).Focus();
				}
			} else if (sender is System.Windows.Forms.TextBox) {
				(sender as System.Windows.Forms.TextBox).Focus();
			} else if (sender is System.Windows.Forms.NumericUpDown) {
				(sender as System.Windows.Forms.NumericUpDown).Focus();
			} else if (sender is System.Windows.Forms.CheckBox) {
				(sender as System.Windows.Forms.CheckBox).Focus();
			}
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

			for (int i = 0; i < _fileinfo.FullName.Length; i++)
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
				System.Windows.Forms.Clipboard.SetText(to_clip.Trim());
				if (Properties.Settings.Default.MakeSounds) {
					try {
						System.Media.SoundPlayer sp = new System.Media.SoundPlayer(Properties.Settings.Default.ClipboardSound);
						sp.PlaySync();
					} catch (Exception ex) {
						ProcessError(ex);
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
			if (input == null) {
				return Properties.Settings.Default.ValErr;
			}
			double _val = 0.0F;
			input = input.Replace("\"", string.Empty);
			if (double.TryParse(input, out _val)) {
				return string.Format(Properties.Settings.Default.NumberFormat, _val);
			}
			return Properties.Settings.Default.ValErr;
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
			ps.ExportEDrw = true;
			ps.ExportSTEP = Properties.Settings.Default.ExportSTEP;
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
					ProcessError(e);
				} catch (Exception e) {
					ProcessError(e);
				}
				v = (SolidWorks.Interop.sldworks.View)d.GetFirstView();
				v = (SolidWorks.Interop.sldworks.View)v.GetNextView();
				x++;
			} while ((v == null) && (x < d.GetSheetCount()));

			if (v != null) {
				message = (string)v.GetName2() + ":\n";
			}

			return v;
		}

		/// <summary>
		/// Dump an error into the db.
		/// </summary>
		/// <param name="e">An <see cref="Exception"/> object.</param>
		public static void ProcessError(Exception e) {
			int uid_ = 0;
			using (RedbrickDataSetTableAdapters.QueriesTableAdapter q_ =
				new RedbrickDataSetTableAdapters.QueriesTableAdapter()) {
				uid_ = Convert.ToInt32(q_.UserQuery(System.Environment.UserName));
				string[] stack = e.StackTrace.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
				string offender = stack[stack.Length - 1].Trim();
				int len = offender.Length > 255 ? 255 : offender.Length;
				string msg_ = string.Format(@"Error `{0}' occurred {1}.", e.Message, offender);
#if !DEBUG
				int aff = q_.InsertError(
					DateTime.Now,
					uid_,
					e.HResult,
					e.Message,
					offender.Substring(0, len),
					false,
					@"REDBRICK");
				if (aff > 0) {
					msg_ += "\n" + @"This error has been reported.";
					System.Windows.Forms.MessageBox.Show(msg_, @"Redbrick Error",
						System.Windows.Forms.MessageBoxButtons.OK,
						System.Windows.Forms.MessageBoxIcon.Error);
				} else {
#endif
					msg_ += "\n" + @"This error failed to be reported.";
					System.Windows.Forms.MessageBox.Show(msg_, @"Redbrick Error",
						System.Windows.Forms.MessageBoxButtons.OK,
						System.Windows.Forms.MessageBoxIcon.Error);
#if !DEBUG
				}
#endif
			}
		}

		/// <summary>
		/// Get a lookup value from the drawing we're looking at.
		/// </summary>
		/// <param name="sw">A reference to a live <see cref="SldWorks"/>.</param>
		/// <returns>A lookup <see cref="string"/>.</returns>
		public static string GetModelNameFromDrawing(SldWorks sw) {
			View v_ = GetFirstView(sw);
			try {
				string lu_ = FileInfoToLookup(new System.IO.FileInfo(v_.ReferencedDocument.GetPathName()));
				return lu_;
			} catch (NullReferenceException) {
				return Properties.Settings.Default.ValErr;
			}
		}

		/// <summary>
		/// Attempt to resolve SolidWorks' strange dimension varables.
		/// </summary>
		/// <param name="md">The ModelDoc2 of the variable string in question.</param>
		/// <param name="prp">The variable string.</param>
		/// <returns>A string of a number hopefully. Properties.Settings.Default.ValErr if we can't figure it out.</returns>
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
		/// <returns>A string of a number hopefully. Properties.Settings.Default.ValErr if we can't figure it out.</returns>
		public static string DimensionByEquation(ModelDoc2 md, string equation) {
			string res = string.Empty;
			EquationMgr eqm = md.GetEquationMgr();
			for (int i = 0; i < eqm.GetCount(); i++) {
				if (eqm.get_Equation(i).Contains(equation)) {
					return eqm.get_Value(i).ToString();
				}
			}
			return Properties.Settings.Default.ValErr;
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
		/// Another color
		/// </summary>
		/// <param name="c">The <see cref="System.Windows.Forms.Control"/> you want to color.</param>
		public static void Alert(System.Windows.Forms.Control c) {
			c.BackColor = Properties.Settings.Default.AlertBackground;
			c.ForeColor = Properties.Settings.Default.NormalForeground;
		}

		/// <summary>
		/// Color only the title of a <see cref="System.Windows.Forms.GroupBox"/>.
		/// </summary>
		/// <param name="g">A <see cref="System.Windows.Forms.GroupBox"/> object.</param>
		/// <param name="c">The desired forground <see cref="System.Drawing.Color"/>.</param>
		public static void SetGroupBoxColor(System.Windows.Forms.GroupBox g, System.Drawing.Color c) {
			g.ForeColor = c;
			foreach (System.Windows.Forms.Control control in g.Controls) {
				control.ForeColor = Properties.Settings.Default.NormalForeground;
			}
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
			if (_fi == null) {
				return Properties.Settings.Default.ValErr;
			}
			string lookup_ =  System.IO.Path.GetFileNameWithoutExtension(_fi.FullName);
			lookup_ = lookup_.Split(new char[] { '^' }, StringSplitOptions.RemoveEmptyEntries)[0];

			if (IsConformingPartnumber(lookup_) && !lookup_.StartsWith(@"Z")) {
				return lookup_.Trim().ToUpper();
			}

			// Z #
			System.Text.RegularExpressions.Regex r_ =
				new System.Text.RegularExpressions.Regex(@"Z[0-9]{5,6}");
			System.Text.RegularExpressions.Match m_ = r_.Match(lookup_);
			if (m_.Groups[0].Value != string.Empty) {
				return m_.Groups[0].Value.ToUpper();
			}
			
			// Raw part #
			r_ = new System.Text.RegularExpressions.Regex(@"[0-9]{6}");
			m_ = r_.Match(lookup_);
			if (m_.Groups[0].Value != string.Empty) {
				return m_.Groups[0].Value;
			}
			
			// McMaster-Carr #
			r_ = new System.Text.RegularExpressions.Regex(@"[0-9]{4,5}[A-Z]{1}[0-9]+");
			m_ = r_.Match(lookup_);
			if (m_.Groups[0].Value != string.Empty) {
				return m_.Groups[0].Value.ToUpper();
			}

			return lookup_;
		}

		/// <summary>
		/// Check whether there's a REV in the string we're looking at.
		/// </summary>
		/// <param name="partLookup">A <see cref="string"/>.</param>
		/// <returns></returns>
		static public bool ContainsRev(string partLookup) {
			System.Text.RegularExpressions.Regex r_ = new System.Text.RegularExpressions.Regex(@".*REV\ ?1[0-9]{2}.*");
			return r_.IsMatch(partLookup.ToUpper());
		}

		/// <summary>
		/// Check if a partnumber conforms to Amstore's usual standards.
		/// </summary>
		/// <param name="_part">A <see cref="String"/> to test.</param>
		/// <returns>A <see cref="Boolean"/> indicating conformity.</returns>
		static public bool IsConformingPartnumber(string _part) {
			if (_part == null) {
				return false;
			}
			System.Text.RegularExpressions.Regex r_ =
				new System.Text.RegularExpressions.Regex(Redbrick.BOMFilter[0]);
			return r_.IsMatch(_part);
		}

		/// <summary>
		/// Filter a string to make sure strings don't violate Chris' /Index Librorum Prohibitorum/.
		/// </summary>
		/// <param name="input">Any old <see cref="object"/> for some reason.</param>
		/// <returns>A scantified <see cref="string"/>.</returns>
		static public string FilterString(object input) {
			string filtered = input.ToString();
			char[,] chars = new char[,] {
					{'\u0027', '\u2032'},
					{'\u0022', '\u2033'},
					{';', '\u037E'},
					{'%', '\u066A'},
					{'*', '\u2217'}
				};

			for (int j = 0; j < chars.GetLength(0); j++) {
				filtered = filtered.Replace(chars[j, 0], chars[j, 1]);
			}

			return filtered.Trim();
		}
		/// <summary>
		/// Wrap text to a number of characters.
		/// </summary>
		/// <param name="_text">Text to wrap.</param>
		/// <param name="_length">Number of characters.</param>
		/// <returns></returns>
		public static List<string> WrapText(string _text, int _length) {
			string[] originalLines = _text.Split(new string[] { " " },
					StringSplitOptions.None);

			List<string> wrappedLines = new List<string>();

			StringBuilder actualLine = new StringBuilder();
			int linelength_ = 0;
			foreach (var item in originalLines) {
				actualLine.Append(item + @" ");
				linelength_ += item.Length;
				if (linelength_ > _length) {
					wrappedLines.Add(actualLine.ToString());
					actualLine.Clear();
					linelength_ = 0;
				}
			}

			if (actualLine.Length > 0)
				wrappedLines.Add(actualLine.ToString());

			return wrappedLines;
		}

		/// <summary>
		/// Take a long <see cref="string"/>, and return it wrapped according to character count.
		/// </summary>
		/// <param name="_text">The <see cref="string"/> you wish to wrap.</param>
		/// <param name="_length">A formatted string.</param>
		/// <returns></returns>
		public static string WrapTextReturnString(string _text, int _length) {
			string[] originalLines = _text.Split(new string[] { " " },
					StringSplitOptions.None);

			List<string> wrappedLines = new List<string>();

			StringBuilder actualLine = new StringBuilder();
			int linelength_ = 0;
			foreach (var item in originalLines) {
				actualLine.Append(item + @" ");
				linelength_ += item.Length;
				if (linelength_ > _length) {
					wrappedLines.Add(actualLine.ToString());
					actualLine.Clear();
					linelength_ = 0;
				}
			}

			if (actualLine.Length > 0)
				wrappedLines.Add(actualLine.ToString());
			string str_ = string.Empty;
			foreach (var item in wrappedLines) {
				str_ += item + "\n";
			}
			return str_;
		}

		/// <summary>
		/// Figure out related names, fill out a Groupbox title, and tooltip.
		/// </summary>
		/// <param name="lookup">A Lookup <see cref="string"/></param>
		/// <param name="cust">An <see cref="int"/> to be populated with a customer id.</param>
		/// <param name="descr">A <see cref="string"/> to be populated with a description from the db.</param>
		/// <param name="tooltip">A <see cref="System.Windows.Forms.ToolTip"/> to be populated.</param>
		/// <param name="control">A <see cref="System.Windows.Forms.GroupBox"/> whose title will be updated.</param>
		static public void GetCustAndDescr(string lookup, ref int cust, ref string descr,
			ref System.Windows.Forms.ToolTip tooltip, ref System.Windows.Forms.GroupBox control) {
			cust = 0;
			descr = string.Empty;
			string tooltip_ = string.Empty;
			tooltip.RemoveAll();

			void get_correct_customer_(string l_, ref int c_, ref string d_) {
				ENGINEERINGDataSet.SCH_PROJECTSRow r =
					(new ENGINEERINGDataSet.SCH_PROJECTSDataTable()).GetCorrectCustomer(l_);
				if (r != null) {
					c_ = r.CUSTID;
					d_ = TitleCase(r.DESCRIPTION);
				} else {
					c_ = 0;
					d_ = string.Empty;
				}
			}

			void get_item_data_(string l_, ref string d_) {
			if (lookup == null) {
				return;
			}

				using (ENGINEERINGDataSetTableAdapters.CustToAmsTableAdapter cta_ =
					new ENGINEERINGDataSetTableAdapters.CustToAmsTableAdapter()) {
					using (ENGINEERINGDataSet.CustToAmsDataTable ctadt_ = cta_.GetDataByPart(lookup)) {
						if (ctadt_.Rows.Count > 0) {
							d_ = ctadt_[0].FIXCUST;
						}
					}
				}
			}

			if (IsConformingPartnumber(lookup)) {
				get_correct_customer_(lookup, ref cust, ref tooltip_);
				get_item_data_(lookup, ref descr);
				tooltip.SetToolTip(control, tooltip_);
			} else {
				using (ENGINEERINGDataSetTableAdapters.CustToAmsTableAdapter cta_ =
					new ENGINEERINGDataSetTableAdapters.CustToAmsTableAdapter()) {
					using (ENGINEERINGDataSet.CustToAmsDataTable ctadt_ = cta_.GetDataByItem(lookup)) {
						if (ctadt_.Rows.Count > 0) {
							//cust = ctadt_[0].CUSTID;
							if (!ctadt_[0].IsFIXAMSNull()) {
								descr = ctadt_[0].FIXAMS.Trim();
								string dummy = string.Empty;
								get_correct_customer_(lookup, ref cust, ref dummy);
							}
							int cc_ = 0;
							get_correct_customer_(descr, ref cc_, ref tooltip_);
							tooltip.SetToolTip(control, tooltip_);
						}
					}
				}
			}
			if (descr == string.Empty) {
				descr = tooltip_;
			}
		}

		/// <summary>
		/// Compare floating point numbers for equality.
		/// </summary>
		/// <param name="_left"></param>
		/// <param name="_right"></param>
		/// <returns></returns>
		static public bool FloatEquals(float _left, float _right) {
			return Math.Abs(_left - _right) < Properties.Settings.Default.Epsilon;
		}

		/// <summary>
		/// Compare floating point numbers for equality.
		/// </summary>
		/// <param name="_left"></param>
		/// <param name="_right"></param>
		/// <returns></returns>
		static public bool FloatEquals(double _left, double _right) {
			return Math.Abs(_left - _right) < Properties.Settings.Default.Epsilon;
		}

		/// <summary>
		/// Compare floating point numbers for equality.
		/// </summary>
		/// <param name="_left"></param>
		/// <param name="_right"></param>
		/// <returns></returns>
		static public bool FloatEquals(string _left, string _right) {
			if (double.TryParse(_left, out double left_) && double.TryParse(_right, out double right_)) {
				return Math.Abs(left_ - right_) < Properties.Settings.Default.Epsilon;
			}
			return false;
		}

		/// <summary>
		/// Collect dimension and equation names eligable for the dimension TextBoxes.
		/// </summary>
		/// <param name="_md">A <see cref="ModelDoc2"/> of the part we're looking at.</param>
		/// <returns>A <see cref="List{T}"/> of <see cref="string"/>s.</returns>
		static public List<string> CollectDimNames(ModelDoc2 _md) {
			int limit = 10;
			int count = 0;
			List<string> res_ = new List<string>();
			Feature f_ = (Feature)_md.FirstFeature();
			DisplayDimension dd_ = null;
			while (count < limit && f_ != null) {
				dd_ = (DisplayDimension)f_.GetFirstDisplayDimension();
				while (count < limit && dd_ != null) {
					if (dd_.GetType() != (int)swDimensionType_e.swLinearDimension) {
						dd_ = (DisplayDimension)f_.GetNextDisplayDimension(dd_);
						continue;
					}
					Dimension d_ = dd_.GetDimension2(0);
					string name_ = d_.GetNameForSelection();
					if (!res_.Contains(name_)) {
						count++;
						res_.Add(name_);
					}
					dd_ = (DisplayDimension)f_.GetNextDisplayDimension(dd_);
				}
				f_ = (Feature)f_.GetNextFeature();
			}

			EquationMgr em_ = _md.GetEquationMgr();
			int eq_count_ = em_.GetCount();
			int eq_limit = 5;
			for (int i = 0; i < eq_count_ && i < eq_limit; i++) {
				string eq_ = string.Format(@"{0}@{1}",
					em_.Equation[i].Split('=')[0].Replace("\"", string.Empty),
					System.IO.Path.GetFileName(_md.GetPathName()));
				if (!res_.Contains(eq_)) {
					res_.Add(eq_); 
				}
			}
			return res_;
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

		[ComVisible(true)]
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

		[ComVisible(true)]
		[ComUnregisterFunction()]
		private static void ComUnregister(Type t) {
			string keyPath = String.Format(@"SOFTWARE\SolidWorks\AddIns\{0:b}", t.GUID);
			Microsoft.Win32.Registry.LocalMachine.DeleteSubKeyTree(keyPath);
		}
	}
}
