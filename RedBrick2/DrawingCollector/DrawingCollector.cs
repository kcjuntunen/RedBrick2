using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace RedBrick2.DrawingCollector {
	public partial class DrawingCollector : Form {
		private string TopLevel = string.Empty;
		private string Here;

		public DrawingCollector(SldWorks sldWorks) {
			SwApp = sldWorks;
			Here = Path.GetDirectoryName((SwApp.ActiveDoc as ModelDoc2).GetPathName());
			InitializeComponent();
			listView1.FullRowSelect = true;
			listView1.HideSelection = false;
			listView1.MultiSelect = true;
			listView1.View = System.Windows.Forms.View.Details;
			listView1.SmallImageList = Redbrick.TreeViewIcons;
			FindDrawings();
		}

		private void FindDrawings() {
			Traverser tr_ = new Traverser(SwApp, true);
			ModelDoc2 md_ = SwApp.ActiveDoc as ModelDoc2;
			FileInfo tlfi = new FileInfo(md_.GetPathName());

			ItemInfo tii = new ItemInfo {
				Name = Redbrick.FileInfoToLookup(tlfi),
				SldDrw = new FileInfo(tlfi.FullName.Replace(tlfi.Extension, @".SLDDRW")),
				CloseSldDrw = false,
				DeletePdf = true
			};

			if (md_ is DrawingDoc) {
				SolidWorks.Interop.sldworks.View v_ = Redbrick.GetFirstView(SwApp);
				md_ = v_.ReferencedDocument;
			}

			Configuration c_ = md_.GetActiveConfiguration();
			tr_.TraverseComponent(c_.GetRootComponent3(true), 1);

			SwProperties s = new SwProperties(SwApp, md_);
			s.GetProperties(c_.GetRootComponent3(true));
			tii.PropertySet = s;

			TopLevel = tii.Name;
			infos.Add(tii.Name, tii);
			listView1.Items.Add(tii.Node);
			foreach (DictionaryEntry item in tr_.PartList) {
				ItemInfo ii = new ItemInfo {
					PropertySet = item.Value as SwProperties,
					CloseSldDrw = true,
					DeletePdf = true
				};
				infos.Add(item.Key.ToString(), ii);
				listView1.Items.Add(ii.Node);
			}
			listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
		}

		private FileInfo CreateDwg(FileInfo p) {
			int dt = (int)swDocumentTypes_e.swDocDRAWING;
			int odo = (int)swOpenDocOptions_e.swOpenDocOptions_Silent;
			int err = 0;
			int warn = 0;
			string newName = p.Name.Replace(p.Extension, @".PDF");
			string tmpFile = string.Format(@"{0}\{1}", Path.GetTempPath(), newName);
			string fileName = p.FullName.Replace(p.Extension, @".PDF");
			int saveVersion = (int)swSaveAsVersion_e.swSaveAsCurrentVersion;
			int saveOptions = (int)swSaveAsOptions_e.swSaveAsOptions_Silent;
			bool success;
			//OnAppend(new AppendEventArgs(string.Format("Creating {0}...",
			//	p.Name.Replace(@".SLDDRW", targetExt))));
			toolStripStatusLabel1.Text = @"Opening";
			toolStripStatusLabel2.Text = p.FullName;
			SwApp.OpenDocSilent(p.FullName, dt, ref odo);
			SwApp.ActivateDoc3(p.FullName,
				true, (int)swRebuildOnActivation_e.swDontRebuildActiveDoc, ref err);
			toolStripProgressBar1.PerformStep();

			toolStripStatusLabel1.Text = @"Saving";
			toolStripStatusLabel2.Text = tmpFile;
			bool layerPrint = SwApp.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swPDFExportIncludeLayersNotToPrint);
			SwApp.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swPDFExportIncludeLayersNotToPrint, true);
			success = (SwApp.ActiveDoc as ModelDoc2).SaveAs4(tmpFile, saveVersion, saveOptions, ref err, ref warn); 
			SwApp.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swPDFExportIncludeLayersNotToPrint, layerPrint);
			toolStripProgressBar1.PerformStep();

			//string name = p.Name.Replace(p.Extension, string.Empty);
			//if (infos[name].CloseSldDrw) {
			//	toolStripStatusLabel1.Text = @"Closing";
			//	toolStripStatusLabel2.Text = fileName;
			//	SwApp.CloseDoc(p.Name.Replace(p.Extension, string.Empty));
			//}
			//toolStripProgressBar1.PerformStep();

			return new FileInfo(tmpFile);
		}

		private void CreateDrawings() {
			toolStripProgressBar1.Maximum = infos.Count * 3;
			toolStripProgressBar1.Step = 1;
			foreach (ListViewItem item in listView1.Items) {
				string name = item.SubItems[0].Text;
				if (item.Checked && infos[name].SldDrw.Exists) {
					textBox1.Text = Redbrick.TitleCase(infos[item.Text].PropertySet[@"Description"].Data.ToString());
					textBox2.Text = infos[item.Text].SldDoc.FullName;
					textBox3.Text = infos[item.Text].SldDrw.FullName;
					textBox4.Text = infos[item.Text].Pdf.FullName;

					checkBox1.Checked = infos[item.Text].SldDoc.Exists;
					checkBox2.Checked = infos[item.Text].SldDrw.Exists;
					checkBox3.Checked = infos[item.Text].Pdf.Exists;
					FileInfo sldDrw_ = new FileInfo(item.SubItems[5].Text);
					infos[item.SubItems[0].Text].Pdf = CreateDwg(sldDrw_);
				}
			}
		}

		private void CloseSLDDRWsDeletePDFs() {
			foreach (KeyValuePair<string, ItemInfo> item in infos) {
				if (item.Value.CloseSldDrw) {
					toolStripStatusLabel1.Text = @"Closing";
					toolStripStatusLabel2.Text = item.Value.SldDrw.Name;
					SwApp.CloseDoc(item.Value.SldDrw.FullName);
				}
				toolStripProgressBar1.PerformStep();
				if (item.Value.DeletePdf) {
					if (!item.Value.Pdf.Name.Contains(Properties.Settings.Default.DrawingCollectorSuffix)
					&& item.Value.Pdf.Exists) {
						toolStripStatusLabel1.Text = @"Deleting";
						toolStripStatusLabel2.Text = item.Value.Pdf.Name;
						try {
							File.Delete(item.Value.Pdf.FullName);
						} catch (Exception e) {
							toolStripStatusLabel1.Text = string.Format(@"I couldn't. LAME!!1! {0}", e.Message);
						}
					}
				}
				toolStripProgressBar1.PerformStep();
			}
		}

		private Dictionary<string, ItemInfo> infos = new Dictionary<string, ItemInfo>();
		public SldWorks SwApp { get; set; }

		private void listView1_SelectedIndexChanged(object sender, EventArgs e) {
			ListView lv_ = sender as ListView;
			if (lv_.SelectedItems.Count < 1 || lv_.SelectedItems[0] == null) {
				return;
			}
			ListViewItem lvi_ = lv_.SelectedItems[0];
			textBox1.Text = Redbrick.TitleCase(infos[lvi_.Text].PropertySet[@"Description"].Data.ToString());
			textBox2.Text = infos[lvi_.Text].SldDoc.FullName;
			textBox3.Text = infos[lvi_.Text].SldDrw.FullName;
			textBox4.Text = infos[lvi_.Text].Pdf.FullName;

			checkBox1.Checked = infos[lvi_.Text].SldDoc.Exists;
			checkBox2.Checked = infos[lvi_.Text].SldDrw.Exists;
			checkBox3.Checked = infos[lvi_.Text].Pdf.Exists;
		}

		private void go_btn_Click(object sender, EventArgs e) {
			var stopWatch = System.Diagnostics.Stopwatch.StartNew();
			CreateDrawings();
			string tmpFile = Path.GetTempFileName().Replace(".tmp", ".PDF");
			string fileName = string.Format(@"{0}\{1}{2}.PDF",
				Here,
				TopLevel,
				suffixTbx.Text.Trim());
			toolStripStatusLabel1.Text = @"Merging PDFs...";
			toolStripStatusLabel2.Text = string.Empty;
			PDFMerger pm_ = new PDFMerger(infos, new FileInfo(tmpFile));
			pm_.Merge();

			try {
				File.Copy(tmpFile, fileName, true);
			} catch (UnauthorizedAccessException uae) {
				throw new Exceptions.BuildPDFException(
						String.Format("You don't have the reqired permission to access '{0}'.", fileName),
						uae);
			} catch (ArgumentException ae) {
				throw new Exceptions.BuildPDFException(
						String.Format("Either '{0}' or '{1}' is not a proper file name.", tmpFile, fileName),
						ae);
			} catch (PathTooLongException ptle) {
				throw new Exceptions.BuildPDFException(
						String.Format("Source='{0}'; Dest='{1}' <= One of these is too long.", tmpFile, fileName),
						ptle);
			} catch (DirectoryNotFoundException dnfe) {
				throw new Exceptions.BuildPDFException(
						String.Format("Source='{0}'; Dest='{1}' <= One of these is invalid.", tmpFile, fileName),
						dnfe);
			} catch (FileNotFoundException fnfe) {
				throw new Exceptions.BuildPDFException(
						String.Format("Crap! I lost '{0}'!", tmpFile),
						fnfe);
			} catch (IOException) {
				MessageBox.Show(
						String.Format("If you have the file, '{0}', selected in an Explorer window, " +
						"you may have to close it.", fileName), "This file is open somewhere.",
						MessageBoxButtons.OK,
						MessageBoxIcon.Error);
			} catch (NotSupportedException nse) {
				throw new Exceptions.BuildPDFException(
						String.Format("Source='{0}'; Dest='{1}' <= One of these is an invalid format.",
						tmpFile, fileName), nse);
			}
			//PDFMerger.deleting_file += PDFMerger_deleting_file;
			toolStripStatusLabel1.Text = @"Deleting PDFs...";
			toolStripStatusLabel2.Text = string.Empty;
			CloseSLDDRWsDeletePDFs();
			//PDFMerger.delete_pdfs(pm_.PDFCollection);
			stopWatch.Stop();
			toolStripStatusLabel1.Text = @"Saved";
			toolStripStatusLabel2.Text = string.Format(@"{0} in {1} seconds", fileName, stopWatch.Elapsed.TotalSeconds);
			toolStripProgressBar1.Value = toolStripProgressBar1.Maximum;
			//PDFMerger.deleting_file -= PDFMerger_deleting_file;
			System.Diagnostics.Process.Start(fileName);
		}

		public delegate void PerformProgressBarStep();


		private void PDFMerger_deleting_file(object sender, EventArgs e) {
			PerformProgressBarStep ppbs = toolStripProgressBar1.PerformStep;
			Invoke(ppbs);
		}

		private void DrawingCollector_Load(object sender, EventArgs e) {
			Location = Properties.Settings.Default.DrawingCollectorLocation;
			Size = Properties.Settings.Default.DrawingCollectorSize;
			suffixTbx.Text = Properties.Settings.Default.DrawingCollectorSuffix;
		}

		private void DrawingCollector_FormClosed(object sender, FormClosedEventArgs e) {
			Properties.Settings.Default.DrawingCollectorLocation = Location;
			Properties.Settings.Default.DrawingCollectorSize = Size;
			Properties.Settings.Default.DrawingCollectorSuffix = suffixTbx.Text.Trim();
		}

		private void ColumnClick(object sender, ColumnClickEventArgs e) {
			ListView lv_ = sender as ListView;
			if (lv_.Columns[e.Column].ListView.Sorting == SortOrder.Ascending) {
				lv_.Columns[0].ListView.Sorting = SortOrder.Descending;
			} else {
				lv_.Columns[0].ListView.Sorting = SortOrder.Ascending;
			}
		}

		private void listView1_MouseClick(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Middle) {
				if (listView1.View == System.Windows.Forms.View.Details) {
					listView1.View = System.Windows.Forms.View.SmallIcon;
				} else {
					listView1.View = System.Windows.Forms.View.Details;
				}
			}
		}

		private void close_btn_Click(object sender, EventArgs e) {
			Close();
		}
	}
}
