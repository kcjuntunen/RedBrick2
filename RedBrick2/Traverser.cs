using System.Collections.Generic;
using System.IO;
using System.Collections.Specialized;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;

namespace RedBrick2 {
	class Traverser {
		public SortedDictionary<string, int> Dict = new SortedDictionary<string, int>();
		public OrderedDictionary PartList = new OrderedDictionary();
		//public SortedDictionary<string, SwProperties> PartList = new SortedDictionary<string, SwProperties>();
		public HashSet<string> PartHash = new HashSet<string>();
		private SldWorks _swApp;
		private ModelDoc2 _activeDoc;
		private FileInfo PartFileInfo;
		private string topName = string.Empty;
		private string partLookup = string.Empty;
		private UserProgressBar pb;
		private bool CollectAssemblies;

		public Traverser(SldWorks s, bool collectAssemblies) {
			_swApp = s;
			CollectAssemblies = collectAssemblies;
			_activeDoc = _swApp.ActiveDoc;
			PartFileInfo = new FileInfo(_activeDoc.GetPathName());
			topName = Path.GetFileNameWithoutExtension(_activeDoc.GetPathName());
			partLookup = topName;
			_swApp.GetUserProgressBar(out pb);
		}

		public Traverser(SldWorks s) : this(s, false) {

		}

		public void TraverseComponent(Component2 swComp, long nLevel) {
			int pos = 0;
			object[] vChildComp;
			Component2 swChildComp;
			string sPadStr = " ";
			long i = 0;

			for (i = 0; i <= nLevel - 1; i++) {
				sPadStr = sPadStr + " ";
			}

			vChildComp = (object[])swComp.GetChildren();

			if (vChildComp == null) {
				GetPart(swComp);
				return;
			}

			if (nLevel == 1) {
				pb.Start(0, vChildComp.Length, @"Enumerating parts...");
			}
			for (i = 0; i < vChildComp.Length; i++) {
				FileInfo fi_;
				swChildComp = (Component2)vChildComp[i];
				string name = swChildComp.Name2.Substring(0, swChildComp.Name2.LastIndexOf('-'));
				if (name.Contains("/")) {
					name = name.Substring(name.LastIndexOf('/') + 1);
				}
				pb.UpdateTitle(name);

				ModelDoc2 md = (swChildComp.GetModelDoc2() as ModelDoc2);
				if (md != null && (md is AssemblyDoc)) {
					name = Redbrick.FileInfoToLookup(new FileInfo(md.GetPathName()));
					PartHash.Add(name);
					if (CollectAssemblies) {
						if (!Dict.ContainsKey(name)) {
							Dict.Add(name, 1);
							SwProperties s = new SwProperties(_swApp, md);
							s.Configuration = swChildComp.ReferencedConfiguration;
							s.GetProperties(md);
							PartList.Add(name, s);
						} else {
							Dict[name] = Dict[name] + 1;
							(PartList[name] as SwProperties).CutlistQty = Dict[name];
						}
					}
				}
				if (md != null && md.GetType() == (int)swDocumentTypes_e.swDocPART) {
					fi_ = new FileInfo(md.GetPathName());
					name = Redbrick.FileInfoToLookup(fi_);
					PartHash.Add(name);
					if (!Dict.ContainsKey(name)) {
						SwProperties s = new SwProperties(_swApp, md);
						s.Configuration = swChildComp.ReferencedConfiguration;
						s.GetProperties(md);
						Dict.Add(name, 1);
						PartList.Add(name, s);
					} else {
						Dict[name] = Dict[name] + 1;
						(PartList[name] as SwProperties).CutlistQty = Dict[name];
					}
					if (nLevel == 1) {
						pb.UpdateProgress(++pos);
					}
				}

				TraverseComponent(swChildComp, nLevel + 1);
			}
			pb.End();
		}

		private void GetPart(Component2 swComp) {
			ModelDoc2 md_ = swComp.GetModelDoc2();
			string name_ = Redbrick.FileInfoToLookup(new FileInfo(md_.GetPathName()));
			SwProperties p_ = new SwProperties(_swApp, md_);
			p_.Configuration = swComp.ComponentReference;
			p_.GetProperties(swComp);
			PartList.Add(name_, p_);
			Dict.Add(name_, 1);
			PartHash.Add(name_);
		}
	}
}
