#define PAGE_NUMBERS
using System;
using System.IO;
using System.Collections.Generic;

using iTextSharp.text;
using iTextSharp.text.pdf;


namespace RedBrick2.DrawingCollector {
	class PDFMerger {

		public PDFMerger(List<FileInfo> lfi, FileInfo target) {
			Target = target;
			PDFCollection.AddRange(lfi);
		}

		public PDFMerger(List<ItemInfo> lfi, FileInfo target) {
			Target = target;
			foreach (ItemInfo item in lfi) {
				if (item.Pdf.Exists) {
					PDFCollection.Add(item.Pdf);
				}
			}
		}

		public PDFMerger(Dictionary<string, ItemInfo> lfi, FileInfo target) {
			Target = target;
			foreach (KeyValuePair<string, ItemInfo> item in lfi) {
				if (item.Value.Pdf.Exists) {
					PDFCollection.Add(item.Value.Pdf);
				}
			}
		}

		public void Merge() {
			byte[] ba = merge_files(PDFCollection);
			using (FileStream fs = File.Create(Target.FullName)) {
				for (int i = 0; i < ba.Length; i++) {
					fs.WriteByte(ba[i]);
				}
				fs.Close();
			}
		}

		public void DeletePDFs() {
			foreach (FileInfo fi in PDFCollection) {
				if (!fi.Name.Contains(Properties.Settings.Default.DrawingCollectorSuffix)
					&& fi.Exists) {
					OnAppend(new AppendEventArgs(string.Format(@"Deleting {0}...", fi.Name)));
					try {
						fi.Delete();
					} catch (IOException ex) {
						OnAppend(new AppendEventArgs(string.Format(@"Couldn't delete {0}: {1}", fi.Name, ex.Message)));
					}
				}
			}
		}

		public static event EventHandler deleting_file;
		public delegate void AppendEvent(object o, EventArgs e);

		public static void OnAppend(EventArgs e) {
			deleting_file?.Invoke(new object(), e);
		}

		public static void delete_pdfs(List<FileInfo> docs) {
			foreach (FileInfo fi in docs) {
				if (!fi.Name.Contains(Properties.Settings.Default.DrawingCollectorSuffix)
					&& fi.Exists) {
					OnAppend(new AppendEventArgs(string.Format(@"Deleting {0}...", fi.Name)));
					try {
						fi.Delete();
					} catch (IOException ex) {
						OnAppend(new AppendEventArgs(string.Format(@"Couldn't delete {0}: {1}", fi.Name, ex.Message)));
					}
				}
			}
		}

		public static void delete_pdfs(Dictionary<string, ItemInfo> docs) {
			foreach (KeyValuePair<string, ItemInfo> item in docs) {
				FileInfo fi = item.Value.Pdf;
				if (!fi.Name.Contains(Properties.Settings.Default.DrawingCollectorSuffix)
					&& fi.Exists) {
					OnAppend(new AppendEventArgs(string.Format(@"Deleting {0}...", fi.Name)));
					try {
						fi.Delete();
					} catch (IOException ex) {
						OnAppend(new AppendEventArgs(string.Format(@"Couldn't delete {0}: {1}", fi.Name, ex.Message)));
					}
				}
			}
		}

		public static int count_pages(List<FileInfo> docs) {
			int total = 0;
			foreach (FileInfo fi in docs) {
				PdfReader reader = new PdfReader(fi.FullName);
				total += reader.NumberOfPages;
			}
			return total;
		}

		public static byte[] merge_files(List<FileInfo> docs) {
			using (Document document = new Document()) {
				using (MemoryStream ms = new MemoryStream()) {
					PdfCopy copy = new PdfCopy(document, ms);
					document.Open();
					int document_page_counter = 0;
					int total_pages = count_pages(docs);
					int font_size = Properties.Settings.Default.PageStampSize;
					foreach (FileInfo fi in docs) {
						using (PdfReader reader = new PdfReader(fi.FullName)) {

							for (int i = 1; i <= reader.NumberOfPages; i++) {
								document_page_counter++;
								PdfImportedPage ip = copy.GetImportedPage(reader, i);
#if PAGE_NUMBERS
								PdfCopy.PageStamp ps = copy.CreatePageStamp(ip);
								PdfContentByte cb = ps.GetOverContent();
								System.Drawing.Rectangle sdr = Properties.Settings.Default.PageStampWhiteoutRectangle;
								System.Drawing.Point location = Properties.Settings.Default.PageStampLoc;
								//System.Drawing.Rectangle sdr = new System.Drawing.Rectangle(1154, 20, 47, 16);
								Rectangle size = reader.GetPageSize(i);
								if ((size.Height * size.Width) < (1224 * 792)) {
									sdr = Properties.Settings.Default.PageStampWhiteoutRectangleA4;
									//sdr = new System.Drawing.Rectangle(720, 20, 47, 16);
									location = Properties.Settings.Default.PageStampLocA4;
								}
								Rectangle r = new Rectangle(sdr.Left, sdr.Bottom, sdr.Right, sdr.Top);
								//OnAppend(new AppendEventArgs(string.Format(@"{0} x {1}", size.Width, size.Height)));
								r.BackgroundColor = BaseColor.WHITE;
								cb.Rectangle(r);
								Font f = FontFactory.GetFont("Century Gothic", font_size);
								Chunk c = new Chunk(string.Format("{0} OF {1}", document_page_counter, total_pages), f);
								c.SetBackground(BaseColor.WHITE);
								ColumnText.ShowTextAligned(ps.GetOverContent(),
									Element.ALIGN_CENTER,
									new Phrase(c),
									location.X, location.Y,
									ip.Width < ip.Height ? 0 : 1);
								ps.AlterContents();
#endif
								copy.AddPage(ip);
							}

							//copy.FreeReader(reader);
							//reader.Close();
						}
					}
					document.Close();
					return ms.GetBuffer();
				}
			}
		}

		public FileInfo Target { get; set; }

		public List<FileInfo> PDFCollection { get; set; } = new List<FileInfo>();

		class AppendEventArgs : EventArgs {
			public AppendEventArgs() {
				Message = string.Empty;
			}


			public AppendEventArgs(string msg) {
				Message = msg;
			}

			public override string ToString() {
				return Message;
			}

			public string Message { get; set; }
		}
	}
}
