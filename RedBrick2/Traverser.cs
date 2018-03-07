using System.Collections.Generic;
using System.IO;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
	class Traverser {
		private SortedDictionary<string, int> _dict = new SortedDictionary<string, int>();
		private SortedDictionary<string, SwProperties> _partlist = new SortedDictionary<string, SwProperties>();
		private SldWorks _swApp;
		private ModelDoc2 _activeDoc;
		private FileInfo PartFileInfo;
		private string topName = string.Empty;
		private string partLookup = string.Empty;
		private UserProgressBar pb;

		public Traverser(SldWorks s) {
			_swApp = s;
			_activeDoc = _swApp.ActiveDoc;
			PartFileInfo = new FileInfo(_activeDoc.GetPathName());
			topName = Path.GetFileNameWithoutExtension(_activeDoc.GetPathName());
			partLookup = topName;
		}

		private void TraverseComponent(Component2 swComp, long nLevel) {
			int pos = 0;
			object[] vChildComp;
			Component2 swChildComp;
			string sPadStr = " ";
			long i = 0;

			for (i = 0; i <= nLevel - 1; i++) {
				sPadStr = sPadStr + " ";
			}

			vChildComp = (object[])swComp.GetChildren();
			if (nLevel == 1) {
				pb.Start(0, vChildComp.Length, @"Enumerating parts...");
			}
			for (i = 0; i < vChildComp.Length; i++) {
				swChildComp = (Component2)vChildComp[i];
				string name = swChildComp.Name2.Substring(0, swChildComp.Name2.LastIndexOf('-'));
				if (name.Contains("/")) {
					name = name.Substring(name.LastIndexOf('/') + 1);
				}
				pb.UpdateTitle(name);

				ModelDoc2 md = (swChildComp.GetModelDoc2() as ModelDoc2);
				if (md != null && md.GetType() == (int)swDocumentTypes_e.swDocPART) {
					SwProperties s = new SwProperties(_swApp, md);
					s.GetProperties(md);
					if (!_dict.ContainsKey(name)) {
						_dict.Add(name, 1);
						_partlist.Add(name, s);
					} else {
						_dict[name] = _dict[name] + 1;
						_partlist[name][@"BLANK QTY"].Data = _dict[name];
					}
					if (nLevel == 1) {
						pb.UpdateProgress(++pos);
					}
				}

				TraverseComponent(swChildComp, nLevel + 1);
			}
		}
	}
}
