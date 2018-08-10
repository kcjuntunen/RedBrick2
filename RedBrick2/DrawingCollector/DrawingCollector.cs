using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedBrick2.DrawingCollector {
	public partial class DrawingCollector : Form {
		public DrawingCollector(SldWorks sldWorks) {
			SwApp = sldWorks;
			InitializeComponent();
			listView1.FullRowSelect = true;
			listView1.HideSelection = false;
			listView1.MultiSelect = true;
			listView1.View = System.Windows.Forms.View.Details;
			listView1.SmallImageList = Redbrick.TreeViewIcons;
			Go();
		}

		private void Go() {
			Traverser tr_ = new Traverser(SwApp, true);
			ModelDoc2 md_ = SwApp.ActiveDoc as ModelDoc2;
			if (md_ is DrawingDoc) {
				SolidWorks.Interop.sldworks.View v_ = Redbrick.GetFirstView(SwApp);
				md_ = v_.ReferencedDocument;
			}
			Configuration c_ = md_.GetActiveConfiguration();
			tr_.TraverseComponent(c_.GetRootComponent3(true), 1);
			foreach (var item in tr_.PartList) {
				ItemInfo ii = new ItemInfo();
				ii.PropertySet = item.Value;
				infos.Add(item.Key, ii);
				listView1.Items.Add(ii.Node);
			}
			listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
		}

		private void CreateDwg(FileInfo p) {
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
			SwApp.OpenDocSilent(p.FullName, dt, ref odo);
			SwApp.ActivateDoc3(p.FullName,
				true, (int)swRebuildOnActivation_e.swDontRebuildActiveDoc, ref err);
			success = (SwApp.ActiveDoc as ModelDoc2).SaveAs4(tmpFile, saveVersion, saveOptions, ref err, ref warn);
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

		}
	}
}
