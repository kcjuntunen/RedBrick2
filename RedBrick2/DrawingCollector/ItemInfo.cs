using SolidWorks.Interop.sldworks;
using System;
using System.IO;
using System.Windows.Forms;

namespace RedBrick2.DrawingCollector {
	public class ItemInfo {
		public ItemInfo() {

		}
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

		public bool Checked { get; set; }
		public bool CloseSldDrw { get; set; }
		public bool	DeletePdf { get; set; }

		private string name;
		public string Name {
			get {
				if (name == null || name == string.Empty) {
					return PropertySet.PartLookup;
				}
				return name;
			}
			set {
				name = value;
			}
		}
		public PacketItem pItem {
			get {
				PacketItem p = new PacketItem();
				p.Checked = Checked;
				p.CloseSldDrw = CloseSldDrw;
				p.DeletePdf = DeletePdf;

				p.Name = Name;
				p.Configuration = PropertySet.Configuration;
				p.DocType = PropertySet.ActiveDoc is AssemblyDoc ? "Assembly" : "Part";
				p.Department = Redbrick.TitleCase(PropertySet[@"DEPARTMENT"].Value);
				p.Description = PropertySet[@"Description"].Value;
				p.SldDoc = SldDoc.FullName;
				p.SldDrw = SldDrw.FullName;
				p.Pdf = Pdf.FullName;
				return p;
			}
			private set {
				pItem = value;
			}
		}

		public ListViewItem Node {
			get {
				string[] data_ = new string[] {
					Name,
					PropertySet.Configuration,
					PropertySet.ActiveDoc is AssemblyDoc ? "Assembly" : "Part",
					Redbrick.TitleCase(PropertySet[@"DEPARTMENT"].Value),
					PropertySet[@"Description"].Value,
					SldDoc.FullName,
					SldDrw.FullName,
					Pdf.FullName };
				ListViewItem lvi_ = new ListViewItem(data_);
				lvi_.Checked = SldDrw.Exists && !SldDrw.FullName.ToUpper().Contains(@"PART");
				return lvi_;
			}
			private set {; }
		}
	}
}
