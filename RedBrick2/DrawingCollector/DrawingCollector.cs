using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace RedBrick2.DrawingCollector {
	public partial class DrawingCollector : Form {
		private ItemInfo rootItem = new ItemInfo();
		private ModelDoc2 rootDoc;
		Traverser traverser;
		private string TopLevel = string.Empty;
		private string Here;

		public DrawingCollector(SldWorks sldWorks) {
			SwApp = sldWorks;
			traverser = new Traverser(SwApp);
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
			rootDoc = SwApp.ActiveDoc as ModelDoc2;
			FileInfo tlfi = new FileInfo(rootDoc.GetPathName());

			rootItem = new ItemInfo {
				Name = Redbrick.FileInfoToLookup(tlfi),
				SldDrw = new FileInfo(tlfi.FullName.Replace(tlfi.Extension, @".SLDDRW")),
				CloseSldDrw = false,
				DeletePdf = true
			};

			if (rootDoc is DrawingDoc) {
				SolidWorks.Interop.sldworks.View v_ = Redbrick.GetFirstView(SwApp);
				rootDoc = v_.ReferencedDocument;
			}

			Configuration c_ = rootDoc.GetActiveConfiguration();
			traverser.TraverseComponent(c_.GetRootComponent3(true), 1);

			config_cbx.Items.AddRange(rootDoc.GetConfigurationNames());
			config_cbx.SelectedItem = c_.Name;
			PopulateListViewAndItemInfo(c_, traverser);
		}

		private void PopulateListViewAndItemInfo(Configuration c_, Traverser tr_) {
			listView1.Items.Clear();
			infos.Clear();
			SwProperties s = new SwProperties(SwApp, rootDoc);
			s.GetProperties(c_.GetRootComponent3(true));
			rootItem.PropertySet = s;

			TopLevel = rootItem.Name;
			infos.Add(rootItem.Name, rootItem);

			listView1.Items.Add(rootItem.Node);
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

			return new FileInfo(tmpFile);
		}

		private FileInfo CreateDXF(FileInfo p) {
			int dt = (int)swDocumentTypes_e.swDocDRAWING;
			int odo = (int)swOpenDocOptions_e.swOpenDocOptions_Silent;
			int err = 0;
			int warn = 0;
			string newName = p.Name.Replace(p.Extension, @".DXF");
			string tmpFile = string.Format(@"{0}\{1}", Path.GetTempPath(), newName);
			string fileName = p.FullName.Replace(p.Extension, @".DXF");
			int saveVersion = (int)swSaveAsVersion_e.swSaveAsCurrentVersion;
			int saveOptions = (int)swSaveAsOptions_e.swSaveAsOptions_Silent;
			bool success;

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

		private void CreateDXFs() {
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
					infos[item.SubItems[0].Text].Pdf = CreateDXF(sldDrw_);
					if (infos[item.Text].CloseSldDrw) {
						toolStripStatusLabel1.Text = @"Closing";
						toolStripStatusLabel2.Text = infos[item.Text].SldDrw.Name;
						SwApp.CloseDoc(infos[item.Text].SldDrw.FullName);
					}
				}
			}
		}

		private void DeletePDFs() {
			foreach (KeyValuePair<string, ItemInfo> item in infos) {
				//if (item.Value.CloseSldDrw) {
				//	toolStripStatusLabel1.Text = @"Closing";
				//	toolStripStatusLabel2.Text = item.Value.SldDrw.Name;
				//	SwApp.CloseDoc(item.Value.SldDrw.FullName);
				//}
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
			DeletePDFs();
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
			cUT_PART_TYPESTableAdapter.Fill(manageCutlistTimeDataSet.CUT_PART_TYPES);
			Location = Properties.Settings.Default.DrawingCollectorLocation;
			Size = Properties.Settings.Default.DrawingCollectorSize;
			suffixTbx.Text = Properties.Settings.Default.DrawingCollectorSuffix;
			splitContainer1.SplitterDistance = Properties.Settings.Default.DrawingCollectorSplitterDistance;
		}

		private void DrawingCollector_FormClosed(object sender, FormClosedEventArgs e) {
			Properties.Settings.Default.DrawingCollectorLocation = Location;
			Properties.Settings.Default.DrawingCollectorSize = Size;
			Properties.Settings.Default.DrawingCollectorSuffix = suffixTbx.Text.Trim();
			Properties.Settings.Default.DrawingCollectorSplitterDistance = splitContainer1.SplitterDistance;
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

		private void select_all_btn_Click(object sender, EventArgs e) {
			foreach (ListViewItem item in listView1.Items) {
				if (infos[item.Text].SldDrw.Exists) {
					item.Checked = true;
				}
			}
		}

		private void select_none_btn_Click(object sender, EventArgs e) {
			foreach (ListViewItem item in listView1.Items) {
				item.Checked = false;
			}
		}

		private void select_only_btn_Click(object sender, EventArgs e) {
			if (select_only_cbx.SelectedItem == null) {
				return;
			}

			if (listView1.Items.Count < 1) {
				return;
			}

			foreach (ListViewItem item in listView1.Items) {
				string dept_ = item.SubItems[2].Text.ToUpper().Trim();
				bool exists_ = infos[item.Text].SldDrw.Exists;
				item.Checked = exists_ && dept_ == select_only_cbx.Text.ToUpper().Trim();
			}
		}

		private void deselect_raw_parts_btn_Click(object sender, EventArgs e) {
			foreach (ListViewItem item in listView1.Items) {
				if (infos[item.Text].SldDoc.FullName.ToUpper().Contains(@"PART")) {
					item.Checked = false;
				}
			}
		}

		private void select_only_assemblies_btn_Click(object sender, EventArgs e) {
			foreach (ListViewItem item in listView1.Items) {
				string type_ = item.SubItems[1].Text.ToUpper().Trim();
				bool exists_ = infos[item.Text].SldDrw.Exists;
				item.Checked = exists_ && type_ == @"ASSEMBLY";
			}
		}

		private void button1_Click(object sender, EventArgs e) {
			var stopWatch = System.Diagnostics.Stopwatch.StartNew();
			CreateDXFs();

			FolderBrowserDialog fbd_ = new FolderBrowserDialog();
			fbd_.RootFolder = System.Environment.SpecialFolder.Desktop;
			fbd_.ShowDialog(this);

			foreach (ListViewItem item in listView1.Items) {
				string name = item.SubItems[0].Text;
				if (item.Checked && infos[name].Pdf.Exists) {
					string tmpFile = infos[name].Pdf.FullName;
					string fileName = string.Format(@"{0}\{1}", fbd_.SelectedPath, infos[name].Pdf.Name);
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
				}
			}
			DeletePDFs();
			stopWatch.Stop();
			toolStripStatusLabel1.Text = string.Empty;
			toolStripStatusLabel2.Text = string.Format(@"Saved a bunch of DXFs in {0} seconds", stopWatch.Elapsed.TotalSeconds);
			toolStripProgressBar1.Value = toolStripProgressBar1.Maximum;
		}

		private void config_cbx_SelectedIndexChanged(object sender, EventArgs e) {
			ComboBox cbx_ = sender as ComboBox;
			if (cbx_.SelectedItem == null) {
				return;
			}
			Configuration c_ = rootDoc.GetConfigurationByName(cbx_.SelectedItem.ToString());
			if (c_ == null) {
				return;
			}
			traverser = new Traverser(SwApp);
			traverser.TraverseComponent(c_.GetRootComponent3(true), 1);
			PopulateListViewAndItemInfo(c_, traverser);
		}
	}
}
