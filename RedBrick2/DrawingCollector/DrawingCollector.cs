using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace RedBrick2.DrawingCollector {
	/// <summary>
	/// Collect and combine drawings into a PDF.
	/// </summary>
	public partial class DrawingCollector : Form {
		private ItemInfo rootItem = new ItemInfo();
		private ModelDoc2 rootDoc;
		Traverser traverser;
		private string TopLvl = string.Empty;
		private string Here;
		private List<ItemInfo> reordered_by_boms = new List<ItemInfo>();
		private int sortColumn = 0;
		private double ts;
		private string pathname = string.Empty;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="sldWorks">A live <see cref="SldWorks"/> object.</param>
		public DrawingCollector(SldWorks sldWorks) {
			SwApp = sldWorks;
			traverser = new Traverser(SwApp, true);
			Here = Path.GetDirectoryName((SwApp.ActiveDoc as ModelDoc2).GetPathName());
			InitializeComponent();


			listView2.FullRowSelect = true;
			listView2.HideSelection = false;
			listView2.MultiSelect = true;
			listView2.View = System.Windows.Forms.View.Details;
			listView2.SmallImageList = Redbrick.TreeViewIcons;

			listView1.FullRowSelect = true;
			listView1.HideSelection = false;
			listView1.MultiSelect = true;
			listView1.View = System.Windows.Forms.View.Details;
			listView1.SmallImageList = Redbrick.TreeViewIcons;
			listView2.ItemDrag += ListView1_ItemDrag;
			new ToolTip().SetToolTip(textBox3, @"This filename was automatically guessed; doubleclick here to change it.");
			new ToolTip().SetToolTip(listView2, "Click the column headings to sort.\nDrag and drop to manually reorder.");
			Cursor = Cursors.WaitCursor;
			FindDrawings();
			Cursor = Cursors.Default;
		}

		private void ListView1_ItemDrag(object sender, ItemDragEventArgs e) {
			listView2.DoDragDrop(listView2.SelectedItems, DragDropEffects.Move);
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

			reordered_by_boms.Add(rootItem);

			if (rootDoc is DrawingDoc) {
				(rootDoc as DrawingDoc).ResolveOutOfDateLightWeightComponents();
				SolidWorks.Interop.sldworks.View v_ = Redbrick.GetFirstView(SwApp);
				rootDoc = v_.ReferencedDocument;
			}

			if (rootDoc is AssemblyDoc) {
				(rootDoc as AssemblyDoc).ResolveAllLightWeightComponents(true);
			}

			Configuration c_ = rootDoc.GetActiveConfiguration();
			traverser.TraverseComponent(c_.GetRootComponent3(true), 1);

			config_cbx.Items.AddRange(rootDoc.GetConfigurationNames());
			config_cbx.SelectedItem = c_.Name;
			PopulateListViewAndItemInfo(c_, traverser);
		}

		private void PopulateListViewAndItemInfo(Configuration c_, Traverser tr_) {
			listView1.Items.Clear();
			//infos.Clear();
			SwProperties s = new SwProperties(SwApp, rootDoc);
			s.GetProperties(c_.GetRootComponent3(true));
			rootItem.PropertySet = s;

			TopLvl = rootItem.Name;
			if (!infos.ContainsKey(rootItem.Name)) {
				infos.Add(rootItem.Name, rootItem);
			}

			if (!(rootDoc as ModelDoc2).GetPathName().ToUpper().EndsWith(@".SLDPRT")) {
				listView1.Items.Add(rootItem.Node);
			}
			foreach (DictionaryEntry item in tr_.PartList) {
				ItemInfo ii = new ItemInfo {
					PropertySet = item.Value as SwProperties,
					CloseSldDrw = true,
					DeletePdf = true
				};
				if (!infos.ContainsKey(item.Key.ToString())) {
					infos.Add(item.Key.ToString(), ii);
				}
				listView1.Items.Add(ii.Node);
			}
			listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
		}

		private FileInfo CreateDwg(FileInfo p) {
			if (p.Extension.ToUpper().EndsWith(@"PDF")) {
				return null;
			}
			int dt = (int)swDocumentTypes_e.swDocDRAWING;
			int odo = (int)swOpenDocOptions_e.swOpenDocOptions_Silent;
			int err = 0;
			int warn = 0;
			string newName = p.Name.Replace(p.Extension, @".PDF");
			string tmpFile = string.Format(@"{0}{1}", Path.GetTempPath(), newName);
			FileInfo tmp = new FileInfo(tmpFile);
			string baseName = tmp.Name.Replace(tmp.Extension, string.Empty);
			string fileName = p.FullName.Replace(p.Extension, @".PDF");
			int saveVersion = (int)swSaveAsVersion_e.swSaveAsCurrentVersion;
			int saveOptions = (int)swSaveAsOptions_e.swSaveAsOptions_Silent;
			bool success;

			toolStripStatusLabel1.Text = @"Opening";
			toolStripStatusLabel2.Text = p.FullName;
			SwApp.OpenDocSilent(p.FullName, dt, ref odo);
			SwApp.ActivateDoc3(p.FullName,
				true, (int)swRebuildOnActivation_e.swDontRebuildActiveDoc, ref err);
			//if (Redbrick.FileInfoToLookup(tmp) != rootItem.Name) {
				SwTableType tt_ = new SwTableType(SwApp.ActiveDoc as ModelDoc2, Properties.Settings.Default.MasterTableHashes, @"PART NUMBER");
				foreach (string item in tt_.Parts) {
					if (infos.ContainsKey(item) && infos[item].Checked && !reordered_by_boms.Contains(infos[item])) {
						reordered_by_boms.Add(infos[item]);
					}
				}
			//}
			toolStripProgressBar1.PerformStep();

			toolStripStatusLabel1.Text = @"Saving";
			toolStripStatusLabel2.Text = tmpFile;
			bool layerPrint = SwApp.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swPDFExportIncludeLayersNotToPrint);
			bool viewOnSave = SwApp.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swPDFViewOnSave);
			bool highQuality = SwApp.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swPDFExportHighQuality);
			SwApp.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swPDFExportIncludeLayersNotToPrint, true);
			SwApp.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swPDFViewOnSave, false);
			SwApp.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swPDFExportHighQuality, true);
			success = (SwApp.ActiveDoc as ModelDoc2).SaveAs4(tmpFile, saveVersion, saveOptions, ref err, ref warn);
			if (infos.ContainsKey(baseName) && infos[baseName].Checked && !reordered_by_boms.Contains(infos[baseName])) {
				reordered_by_boms.Add(infos[baseName]);
			}
			SwApp.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swPDFExportIncludeLayersNotToPrint, layerPrint);
			SwApp.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swPDFViewOnSave, viewOnSave);
			SwApp.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swPDFExportHighQuality, highQuality);
			toolStripProgressBar1.PerformStep();

			return tmp;
		}

		private FileInfo CreateDXF(FileInfo p) {
			int dt = (int)swDocumentTypes_e.swDocDRAWING;
			int odo = (int)swOpenDocOptions_e.swOpenDocOptions_Silent;
			int err = 0;
			int warn = 0;
			string newName = p.Name.Replace(p.Extension, @".DXF");
			string tmpFile = string.Format(@"{0}{1}", Path.GetTempPath(), newName);
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

			string key = Path.GetFileNameWithoutExtension(p.Name);
			if (infos.ContainsKey(key) && infos[key].CloseSldDrw) {
				SwApp.CloseDoc(p.FullName);
				toolStripProgressBar1.PerformStep();
			}
			return new FileInfo(tmpFile);
		}

		private void CreateDrawings() {
			toolStripProgressBar1.Maximum = infos.Count * 3;
			toolStripProgressBar1.Step = 1;

			foreach (ListViewItem itm in listView2.Items) {
				string name = itm.Text;
				if (!infos.ContainsKey(name)) {
					continue;
				}
				infos[name].Checked = itm.Checked;
				if (infos[name].Checked && infos[name].SldDrw.Exists) {
					if (infos[name].Checked && !reordered_by_boms.Contains(infos[name])) {
						reordered_by_boms.Add(infos[name]);
					}
					textBox1.Text = Redbrick.TitleCase(infos[name].PropertySet[@"Description"].Data.ToString());
					textBox2.Text = infos[name].SldDoc.FullName;
					textBox3.Text = infos[name].SldDrw.FullName;
					textBox4.Text = infos[name].Pdf.FullName;

					checkBox1.Checked = infos[name].SldDoc.Exists;
					checkBox2.Checked = infos[name].SldDrw.Exists;
					checkBox3.Checked = infos[name].Pdf.Exists;
					infos[name].Pdf = CreateDwg(infos[name].SldDrw);
					if (infos[name].CloseSldDrw) {
						toolStripStatusLabel1.Text = @"Closing";
						toolStripStatusLabel2.Text = infos[name].SldDrw.Name;
						SwApp.CloseDoc(infos[name].SldDrw.FullName);
						toolStripProgressBar1.PerformStep();
					}
				}
			}
		}

		private int CreateDXFs() {
			int count = 0;
			toolStripProgressBar1.Maximum = infos.Count * 3;
			toolStripProgressBar1.Step = 1;
			foreach (ListViewItem item in listView2.Items) {
				string name = item.SubItems[0].Text;
				if (item.Checked && infos[name].SldDrw.Exists) {
					textBox1.Text = Redbrick.TitleCase(infos[item.Text].PropertySet[@"Description"].Data.ToString());
					textBox2.Text = infos[item.Text].SldDoc.FullName;
					textBox3.Text = infos[item.Text].SldDrw.FullName;
					textBox4.Text = infos[item.Text].Pdf.FullName;

					checkBox1.Checked = infos[item.Text].SldDoc.Exists;
					checkBox2.Checked = infos[item.Text].SldDrw.Exists;
					checkBox3.Checked = infos[item.Text].Pdf.Exists;
					FileInfo sldDrw_ = new FileInfo(item.SubItems[6].Text);
					infos[item.SubItems[0].Text].Pdf = CreateDXF(sldDrw_);
					count++;
				}
			}
			return count;
		}

		private void DeletePDFs() {
			foreach (KeyValuePair<string, ItemInfo> item in infos) {
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
		private SldWorks SwApp { get; set; }

		private void listView1_SelectedIndexChanged(object sender, EventArgs e) {
			ListView lv_ = sender as ListView;
			if (lv_.SelectedItems.Count < 1 || lv_.SelectedItems[0] == null) {
				return;
			}
			ListViewItem lvi_ = lv_.SelectedItems[0];
			if (!infos.ContainsKey(lvi_.Text)) {
				return;
			}
			textBox1.Text = Redbrick.TitleCase(infos[lvi_.Text].PropertySet[@"Description"].Data.ToString());
			textBox2.Text = infos[lvi_.Text].SldDoc.FullName;
			textBox3.Text = infos[lvi_.Text].SldDrw.FullName;
			textBox4.Text = infos[lvi_.Text].Pdf.FullName;

			checkBox1.Checked = infos[lvi_.Text].SldDoc.Exists;
			checkBox2.Checked = infos[lvi_.Text].SldDrw.Exists;
			checkBox3.Checked = infos[lvi_.Text].Pdf.Exists;
		}

		private void go_btn_Click(object sender, EventArgs e) {
			if (listView2.Items.Count < 1 && listView2.CheckedItems.Count < 1) {
				return;
			}
			var stopWatch = System.Diagnostics.Stopwatch.StartNew();
			toolStripProgressBar1.Maximum = infos.Count * 3;
			toolStripProgressBar1.Step = 1;

			foreach (ListViewItem item in listView2.Items) {
				infos[item.Text].Checked = item.Checked;
			}
			CreateDrawings();
			string tmpFile = Path.GetTempFileName().Replace(".tmp", ".PDF");

			string fileName = string.Format(@"{0}{1}.PDF",
				TopLvl,
				suffixTbx.Text.Trim());

			toolStripStatusLabel1.Text = @"Merging PDFs...";
			toolStripStatusLabel2.Text = string.Empty;
			if (manualOrder_chb.Checked) {
				ManualOrderedMerge(tmpFile);
			} else {
				ReorderedMerge(tmpFile);
			}

			stopWatch.Stop();
			ts = stopWatch.Elapsed.TotalSeconds;
			button1.Enabled = true;
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.FileName = fileName;
			if (pathname != string.Empty) {
				saveFileDialog.FileName = pathname;
			}
			saveFileDialog.InitialDirectory = Properties.Settings.Default.DrawingCollectorLastSaveLocation;

			if (saveFileDialog.ShowDialog(this) != DialogResult.OK) {
				toolStripStatusLabel1.Text = @"Deleting PDFs...";
				toolStripStatusLabel2.Text = string.Empty;
				DeletePDFs();
				toolStripStatusLabel1.Text = @"Aborted saving.";
				toolStripStatusLabel2.Text = string.Format(@"You wasted {0} seconds", stopWatch.Elapsed.TotalSeconds);
				toolStripProgressBar1.Value = toolStripProgressBar1.Maximum;
				return;
			}

			pathname = saveFileDialog.FileName;
			fileName = saveFileDialog.FileName;
			Properties.Settings.Default.DrawingCollectorLastSaveLocation = Path.GetDirectoryName(saveFileDialog.FileName);
			Properties.Settings.Default.Save();

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
			PDFMerger.deleting_file += PDFMerger_deleting_file;
			toolStripStatusLabel1.Text = @"Deleting PDFs...";
			toolStripStatusLabel2.Text = string.Empty;
			DeletePDFs();
			//PDFMerger.delete_pdfs(pm_.PDFCollection);
			toolStripStatusLabel1.Text = @"Saved";
			toolStripStatusLabel2.Text = string.Format(@"{0} in {1} seconds", fileName, stopWatch.Elapsed.TotalSeconds);
			toolStripProgressBar1.Value = toolStripProgressBar1.Maximum;
			PDFMerger.deleting_file -= PDFMerger_deleting_file;
			System.Diagnostics.Process.Start(fileName);
		}

		private void ManualOrderedMerge(string tmpFile) {
			List<ItemInfo> itemInfos = new List<ItemInfo>();
			foreach (ListViewItem item in listView2.Items) {
				itemInfos.Add(infos[item.Text]);
			}
			PDFMerger pm_ = new PDFMerger(itemInfos, new FileInfo(tmpFile));
			pm_.Merge();
		}

		private void ReorderedMerge(string tmpFile) {
			PDFMerger pm_ = new PDFMerger(reordered_by_boms, new FileInfo(tmpFile));
			pm_.Merge();
		}

		/// <summary>
		/// A delegate to perform a progress bar step.
		/// </summary>
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
			if (e.Column != sortColumn) {
				sortColumn = e.Column;
				lv_.Sorting = SortOrder.Ascending;
			} else {
				if (lv_.Columns[e.Column].ListView.Sorting == SortOrder.Ascending) {
					lv_.Columns[e.Column].ListView.Sorting = SortOrder.Descending;
				} else {
					lv_.Columns[e.Column].ListView.Sorting = SortOrder.Ascending;
				}
			}
			lv_.Sort();
			lv_.ListViewItemSorter = new ListViewItemComparer(e.Column, lv_.Sorting);
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
			foreach (ListViewItem item in listView2.Items) {
				if (infos[item.Text].SldDrw.Exists) {
					item.Checked = true;
				}
			}
		}

		private void select_none_btn_Click(object sender, EventArgs e) {
			foreach (ListViewItem item in listView2.Items) {
				item.Checked = false;
			}
		}

		private void select_only_btn_Click(object sender, EventArgs e) {
			if (select_only_cbx.SelectedItem == null) {
				return;
			}

			if (listView2.Items.Count < 1) {
				return;
			}

			foreach (ListViewItem item in listView2.Items) {
				string dept_ = item.SubItems[3].Text.ToUpper().Trim();
				bool exists_ = infos[item.Text].SldDrw.Exists;
				item.Checked = exists_ && dept_ == select_only_cbx.Text.ToUpper().Trim();
			}
		}

		private void select_raw_parts_btn_Click(object sender, EventArgs e) {
			foreach (ListViewItem item in listView2.Items) {
				if (infos[item.Text].SldDoc.FullName.ToUpper().Contains(@"PART") && infos[item.Text].SldDrw.Exists) {
					item.Checked = true;
				}
			}
		}

		private void select_only_assemblies_btn_Click(object sender, EventArgs e) {
			foreach (ListViewItem item in listView2.Items) {
				string type_ = item.SubItems[2].Text.ToUpper().Trim();
				bool exists_ = infos[item.Text].SldDrw.Exists;
				item.Checked = exists_ && type_ == @"ASSEMBLY";
			}
		}

		private void button1_Click(object sender, EventArgs e) {
			if (listView2.Items.Count < 1 && listView2.CheckedItems.Count < 1) {
				return;
			}
			var stopWatch = System.Diagnostics.Stopwatch.StartNew();
			int count = 0;
			count = CreateDXFs();

			FolderBrowserDialog fbd_ = new FolderBrowserDialog();
			fbd_.RootFolder = System.Environment.SpecialFolder.Desktop;
			fbd_.ShowDialog(this);

			foreach (ListViewItem item in listView2.Items) {
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
				toolStripProgressBar1.PerformStep();
			}
			DeletePDFs();
			stopWatch.Stop();
			toolStripStatusLabel1.Text = string.Empty;
			string plural = count > 1 ? @"s" : string.Empty;
			toolStripStatusLabel2.Text = string.Format(@"Saved {0} DXF{1} in {2} seconds", count, plural, stopWatch.Elapsed.TotalSeconds);
			toolStripProgressBar1.Value = toolStripProgressBar1.Maximum;
			if (fbd_.SelectedPath != string.Empty &&  new DirectoryInfo(fbd_.SelectedPath).Exists) {
				System.Diagnostics.Process.Start(fbd_.SelectedPath);
			}
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
			traverser = new Traverser(SwApp, true);
			traverser.TraverseComponent(c_.GetRootComponent3(true), 1);
			PopulateListViewAndItemInfo(c_, traverser);
		}

		private void textBox3_MouseDoubleClick(object sender, MouseEventArgs e) {
			if (listView2.SelectedItems.Count < 1) {
				return;
			}

			if (listView2.SelectedItems[0] == null) {
				return;
			}

			string item = listView2.SelectedItems[0].Text;

			OpenFileDialog ofd_ = new OpenFileDialog();
			ofd_.Title = string.Format(@"Select drawing for {0} ({1})", item, infos[item].PropertySet.Configuration);
			ofd_.InitialDirectory = Path.GetFullPath(textBox3.Text);
			ofd_.Filter = @"SolidWorks Drawings (*.slddrw)|*.slddrw";

			if (ofd_.ShowDialog() == DialogResult.OK) {
				FileInfo selectedFile = new FileInfo(ofd_.FileName);
				infos[item].SldDrw = selectedFile;
				textBox3.Text = infos[item].SldDrw.FullName;
				checkBox2.Checked = selectedFile.Exists;
				listView2.SelectedItems[0].Checked = selectedFile.Exists;
			}
		}

			// Code lifted directly from 
			// <https://support.microsoft.com/en-us/help/822483/the-listview-control-does-not-support-drag-and-drop-functionality-for>
		private void listView1_DragEnter(object sender, DragEventArgs e) {
			ListView listView = sender as ListView;
			int len = e.Data.GetFormats().Length - 1;
			int i;
			listView.Sorting = SortOrder.None;
			for (i = 0; i <= len; i++) {
				if (e.Data.GetFormats()[i].Equals("System.Windows.Forms.ListView+SelectedListViewItemCollection")) {
					//The data from the drag source is moved to the target.
					e.Effect = DragDropEffects.Move;
				}
			}
		}

		private void listView1_DragDrop(object sender, DragEventArgs e) {
			//Return if the items are not selected in the ListView control.
			ListView listView = sender as ListView;
			if (listView.SelectedItems.Count == 0) {
				return;
			}
			//Returns the location of the mouse pointer in the ListView control.
			Point cp = listView.PointToClient(new Point(e.X, e.Y));
			//Obtain the item that is located at the specified location of the mouse pointer.
			ListViewItem dragToItem = listView.GetItemAt(cp.X, cp.Y);
			if (dragToItem == null) {
				return;
			}
			//Obtain the index of the item at the mouse pointer.
			int dragIndex = dragToItem.Index;
			ListViewItem[] sel = new ListViewItem[listView.SelectedItems.Count];
			for (int i = 0; i <= listView.SelectedItems.Count - 1; i++) {
				sel[i] = listView.SelectedItems[i];
			}
			for (int i = 0; i < sel.GetLength(0); i++) {
				//Obtain the ListViewItem to be dragged to the target location.
				ListViewItem dragItem = sel[i];
				int itemIndex = dragIndex;
				if (itemIndex == dragItem.Index) {
					return;
				}
				if (dragItem.Index < itemIndex)
					itemIndex++;
				else
					itemIndex = dragIndex + i;
				//Insert the item at the mouse pointer.
				ListViewItem insertItem = (ListViewItem)dragItem.Clone();
				listView.Items.Insert(itemIndex, insertItem);
				//Removes the item from the initial location while 
				//the item is moved to the new location.
				listView.Items.Remove(dragItem);
			}
		}

		private void add_btn_Click(object sender, EventArgs e) {
			OpenFileDialog ofd_ = new OpenFileDialog();
			ofd_.InitialDirectory = Path.GetDirectoryName(rootItem.SldDrw.FullName);
			ofd_.Filter = @"SolidWorks Drawings and PDFs (*.pdf;*.slddrw)|*.slddrw;*.pdf";

			if (ofd_.ShowDialog() == DialogResult.OK) {
				if (ofd_.FileName.ToUpper().EndsWith(@"SLDDRW")) {
					AddItem(ofd_.FileName);
				} else if (ofd_.FileName.ToUpper().EndsWith(@"PDF")) {
					AddPDF(ofd_.FileName);
				}
			}
		}

		private void AddPDF(string fileName) {
			FileInfo f = new FileInfo(fileName);
			SwProperties p_ = new SwProperties(SwApp);
			DepartmentProperty dp = new DepartmentProperty(@"DEPARTMENT", true, SwApp, (SwApp.ActiveDoc as ModelDoc2), string.Empty);
			StringProperty sp = new StringProperty(@"Description", true, SwApp, (SwApp.ActiveDoc as ModelDoc2), string.Empty);
			dp.Data = @"OTHER";
			sp.Data = @"PDF File";
			p_.Add(dp);
			p_.Add(sp);
			ItemInfo i = new ItemInfo {
				Name = string.Format(@"PDF → {0}", Path.GetFileNameWithoutExtension(fileName)),
				PropertySet = p_,
				SldDoc = f,
				SldDrw = f,
				Pdf = f,
				CloseSldDrw = true,
				DeletePdf = false
			};
			if (!infos.ContainsKey(i.Name)) {
				infos.Add(i.Name, i);
			}
			listView2.Items.Add(infos[i.Name].Node);
		}

		private void AddItem(string fileName) {
			int err = 0;
			SwApp.OpenDocSilent(fileName, (int)swDocumentTypes_e.swDocDRAWING, ref err);
			SwApp.ActivateDoc3(fileName, false, (int)swRebuildOnActivation_e.swDontRebuildActiveDoc, ref err);
			SolidWorks.Interop.sldworks.View v_ = Redbrick.GetFirstView(SwApp);
			if (v_ == null || v_.ReferencedDocument == null) {
				MessageBox.Show(this, @"Couldn't find a model in the drawing.", @"Error",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			SwProperties p_ = new SwProperties(SwApp, v_.ReferencedDocument);
			p_.GetProperties(v_.ReferencedDocument);
			ItemInfo i = new ItemInfo {
				PropertySet = p_,
				SldDrw = new FileInfo(fileName),
				CloseSldDrw = true,
				DeletePdf = true
			};
			if (!infos.ContainsKey(i.Name)) {
				infos.Add(i.Name, i);
			}
			listView2.Items.Add(infos[i.Name].Node);
		}

		private void saveListBox(string fileName) {
			try {
				Steps steps = new Steps();
				PacketItems itemInfos = new PacketItems();
				foreach (ListViewItem item in listView2.Items) {
					PacketItem pi = infos[item.Text].pItem;
					pi.Checked = item.Checked;
					itemInfos.Add(pi);
				}
				steps.Items = itemInfos;
				using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate)) {
					XmlSerializer xs = new XmlSerializer(typeof(Steps));
					xs.Serialize(fs, steps);
				}
			} catch (IOException ie) {
				Redbrick.ProcessError(ie);
			} catch (Exception ex) {
				Redbrick.ProcessError(ex);
			}
		}

		private void saveOrdered(string fileName) {
			try {
				Steps steps = new Steps();
				PacketItems itemInfos = new PacketItems();
				steps.Duration = ts.ToString();
				steps.Timestamp = DateTime.Now;
				steps.PathName = pathname;
				foreach (ItemInfo item in reordered_by_boms) {
					itemInfos.Add(item.pItem);
				}
				steps.Items = itemInfos;
				using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate)) {
					XmlSerializer xs = new XmlSerializer(typeof(Steps));
					xs.Serialize(fs, steps);
				}
			} catch (IOException ie) {
				Redbrick.ProcessError(ie);
			} catch (Exception ex) {
				Redbrick.ProcessError(ex);
			}
		}

		private void save_Click_1(object sender, EventArgs e) {
			using (SaveFileDialog sfd = new SaveFileDialog()) {
				sfd.Filter = @"XML Files (*.xmldc)|*.xmldc";
				if (sfd.ShowDialog() != DialogResult.OK) {
					return;
				}
				if (manualOrder_chb.Checked) {
					saveListBox(sfd.FileName);
				} else {
					saveOrdered(sfd.FileName);
				}
			}
		}

		private void load_Click(object sender, EventArgs e) {
			using (OpenFileDialog ofd = new OpenFileDialog()) {
				ofd.Filter = @"XML Files (*.xmldc)|*.xmldc";
				if (ofd.ShowDialog() != DialogResult.OK) {
					return;
				}
				//infos.Clear();
				listView2.Items.Clear();
				reordered_by_boms.Clear();
				try {
					using (FileStream fs = new FileStream(ofd.FileName, FileMode.Open)) {
						XmlSerializer xs = new XmlSerializer(typeof(Steps));
						Steps steps = (Steps)xs.Deserialize(fs);
						toolStripStatusLabel2.Text = string.Format(@"Takes around {0} seconds.", steps.Duration);
						pathname = steps.PathName;
						foreach (PacketItem item in steps.Items) {
							string[] data_ = new string[] { item.Name, item.Configuration, item.DocType, item.Department, item.Description, item.SldDoc, item.SldDrw, item.Pdf };
							ListViewItem lvi_ = new ListViewItem(data_);
							lvi_.Checked = item.Checked;
							listView2.Items.Add(lvi_);
							if (!infos.ContainsKey(item.Name)) {
								ItemInfo ii = new ItemInfo(item, SwApp);
								ii.PropertySet = new SwProperties(SwApp);
								ii.PropertySet.Add(new StringProperty(@"Description", true, SwApp, (SwApp as ModelDoc2), string.Empty));
								ii.PropertySet.Add(new DepartmentProperty(@"DEPARTMENT", true, SwApp, (SwApp as ModelDoc2), string.Empty));
								ii.PropertySet[@"Description"].Data = item.Description;
								ii.PropertySet[@"DEPARTMENT"].Data = item.Department;
								infos.Add(item.Name, ii);
							}
							for (int i = 0; i < listView1.Items.Count; i++) {
								if (listView1.Items[i].Text == lvi_.Text) {
									listView1.Items.RemoveAt(i);
								}
							}
						}
					}
					listView2.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
				} catch (Exception ex) {
					Redbrick.ProcessError(ex);
				}
			}
		}

		private void include_btn_Click(object sender, EventArgs e) {
			if (listView1.SelectedItems.Count < 1) {
				return;
			}
			foreach (ListViewItem item_ in listView1.SelectedItems) {
				listView2.Items.Add((ListViewItem)item_.Clone());
				listView1.Items.Remove(item_);
				listView2.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			}
		}

		private void exclude_btn_Click(object sender, EventArgs e) {
			if (listView2.SelectedItems.Count < 1) {
				return;
			}
			foreach (ListViewItem item_ in listView2.SelectedItems) {
				listView1.Items.Add((ListViewItem)item_.Clone());
				listView2.Items.Remove(item_);
			}
		}

		private void createPartsSummary_Click(object sender, EventArgs e) {
			try {
				OrderedDictionary pl = new OrderedDictionary();
				foreach (ListViewItem item in listView2.Items) {
					string key = item.Text;
					if (!infos.ContainsKey(key)) {
						continue;
					}
					if (item.Checked) {
						if (infos[key].PropertySet.CutlistQty == 0) {
							infos[key].PropertySet.CutlistQty++;
						}
						pl.Add(key, infos[key].PropertySet);
					}
				}
				PartsSummary.PartsSummaryGenerator p = new PartsSummary.PartsSummaryGenerator(pl,
					Properties.Settings.Default.PartsSummaryTemplate);
				p.SuggestedName = pathname;
				p.Generate();
			} catch (Exception ex) {
				Redbrick.ProcessError(ex);
			}
		}
	}
}
