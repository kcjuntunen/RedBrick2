using SolidWorks.Interop.sldworks;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace RedBrick2.DrawingCollector {
	class ItemInfo {
		public SwProperties PropertySet { get; set; }

		private FileInfo sldDoc;
		public FileInfo SldDoc {
			get {
				if (sldDoc == null) {
					sldDoc = PropertySet.PartFileInfo;
				}
				return sldDoc;
			}
			set { sldDoc = value; }
		}

		private FileInfo sldDrw;
		public FileInfo SldDrw {
			get {
				if (sldDrw == null || sldDrw == SldDoc) {
					sldDrw = new FileInfo(SldDoc.FullName.Replace(SldDoc.Extension, @".SLDDRW"));
				}
				return sldDrw;
			}
			set { sldDrw = value; }
		}


		private FileInfo pdf;
		public FileInfo Pdf {
			get {
				if (pdf == null || pdf == SldDoc) {
					pdf = new FileInfo(SldDoc.FullName.Replace(SldDoc.Extension, @".PDF"));
				}
				return pdf;
			}
			set { pdf = value; }
		}

		public ListViewItem Node {
			get {
				string[] data_ = new string[] {
					PropertySet.PartLookup,
					PropertySet.ActiveDoc is AssemblyDoc ? "Assembly" : "Part",
					Redbrick.TitleCase(PropertySet[@"DEPARTMENT"].Value),
					PropertySet[@"Description"].Value,
					SldDoc.FullName,
					SldDrw.FullName,
					Pdf.FullName };
				ListViewItem lvi_ = new ListViewItem(data_);
				lvi_.Checked = SldDrw.Exists;
				return lvi_;
			}
			private set {; }
		}
	}
}
