using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;

namespace RedBrick2.Menus {
	class MenuItems {
		private CommandManager iCmdMgr;
		private int mainCmdGroupID;

		private SldWorks SwApp;
		private int Cookie;

		public MenuItems(SldWorks sw, int cookie) {
			SwApp = sw;
			Cookie = cookie;
		}

		public void add_menu_items() {
			//System.Windows.Forms.MessageBox.Show("Add menu items");
			System.Reflection.Assembly thisAssembly = default(System.Reflection.Assembly);
			int mId_ = 0;
			string[] images = new string[]
				{
					string.Format(@"{0}{1}", Properties.Settings.Default.NetPath, Properties.Settings.Default.RefreshIcon),
					string.Format(@"{0}{1}", Properties.Settings.Default.NetPath, Properties.Settings.Default.RefreshIcon),
					string.Format(@"{0}{1}", Properties.Settings.Default.NetPath, Properties.Settings.Default.RefreshIcon)
				};
			thisAssembly = System.Reflection.Assembly.GetAssembly(this.GetType());
			mId_ = SwApp.AddMenu((int)swDocumentTypes_e.swDocDRAWING, @"Redbrick Tools", 0);
			mId_ = SwApp.AddMenuItem5((int)swDocumentTypes_e.swDocDRAWING, Cookie, @"Reformat Fixture Book@Redbrick Tools", 0,
				@"RenumberFB", @"RenumberFBEnableMethod",
				@"Renumber drawings in a fixture book according to a formatted Excel document.",
				images);
			mId_ = SwApp.AddMenuItem5((int)swDocumentTypes_e.swDocDRAWING, Cookie, @"Drawings@Redbrick Tools", 1,
				@"Drawings", @"DrawingsEnableMethod",
				@"Search drawings.",
				images);
			mId_ = SwApp.AddMenuItem5((int)swDocumentTypes_e.swDocDRAWING, Cookie, @"Time Entry@Redbrick Tools", 2,
				@"TimeEntry", @"TimeEntryEnableMethod",
				@"Time entry",
				images);

			mId_ = SwApp.AddMenu((int)swDocumentTypes_e.swDocPART, "MyMenu", 0);
			mId_ = SwApp.AddMenuItem5((int)swDocumentTypes_e.swDocPART, Cookie, "MyMenuItem@MyMenu", 0, "MyMenuCallback", "MyMenuEnableMethod", "My menu item", images);

		}

		public void MyMenuCallback() {
			MessageBox.Show("Callback function called.");
		}

		private void AttachEventHandlers() {
			AttachSWEvents();
		}

		private void DetachEventHandlers() {
			DetachSwEvents();
		}

		private void AttachSWEvents() {
			try {
				SwApp.ActiveDocChangeNotify += SwApp_ActiveDocChangeNotify;
				SwApp.DocumentLoadNotify2 += SwApp_DocumentLoadNotify2;
				SwApp.FileNewNotify2 += SwApp_FileNewNotify2;
				SwApp.ActiveModelDocChangeNotify += SwApp_ActiveModelDocChangeNotify;
				SwApp.FileOpenPostNotify += SwApp_FileOpenPostNotify;
			} catch (Exception e) {
				Redbrick.ProcessError(e);
			}
		}

		private void DetachSwEvents() {
			try {
				SwApp.ActiveDocChangeNotify -= SwApp_ActiveDocChangeNotify;
				SwApp.DocumentLoadNotify2 -= SwApp_DocumentLoadNotify2;
				SwApp.FileNewNotify2 -= SwApp_FileNewNotify2;
				SwApp.ActiveModelDocChangeNotify -= SwApp_ActiveModelDocChangeNotify;
				SwApp.FileOpenPostNotify -= SwApp_FileOpenPostNotify;
			} catch (Exception e) {
				Redbrick.ProcessError(e);
			}
		}
		private int SwApp_FileOpenPostNotify(string FileName) {
			throw new NotImplementedException();
		}

		private int SwApp_ActiveModelDocChangeNotify() {
			throw new NotImplementedException();
		}

		private int SwApp_FileNewNotify2(object NewDoc, int DocType, string TemplateName) {
			throw new NotImplementedException();
		}

		private int SwApp_DocumentLoadNotify2(string docTitle, string docPath) {
			throw new NotImplementedException();
		}

		private int SwApp_ActiveDocChangeNotify() {
			throw new NotImplementedException();
		}

		public void TimeEntry() {
			using (Time_Entry.TimeEntry te = new Time_Entry.TimeEntry()) {
				te.ShowDialog();
			}
		}

		public void Drawings() {
			Thread t = new Thread(new ThreadStart(LaunchDrawings));
			t.SetApartmentState(ApartmentState.STA);
			t.Start();
		}

		static void LaunchDrawings() {
			using (Drawings.Drawings d = new Drawings.Drawings()) {
				d.ShowInTaskbar = true;
				try {
					d.ShowDialog();
				} catch(Exception e) {
					Redbrick.ProcessError(e);
				}
			}
		}

		public void RenumberFB() {
			using (FormatFixtureBk ffb = new FormatFixtureBk(SwApp)) {
				ffb.ShowDialog();
			}
		}

		private bool CompareIDs(int storedIDs, int addinIDs) {
			List<int> storeList = new List<int>(storedIDs);
			List<int> addinList = new List<int>(addinIDs);

			if (!(addinList.Count == storeList.Count)) {
				return false;
			} else {
				for (int i = 0; i < addinList.Count - 1; i++) {
					if (!(addinList[i] == storeList[i])) {
						return false;
					}
				}
			}
			return true;
		}
	}
}