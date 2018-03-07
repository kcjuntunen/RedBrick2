using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using FormatFixtureBook;
using SolidWorks.Interop.sldworks;

namespace RedBrick2 {
	/// <summary>
	/// A form to control the FormatFixtureBook library.
	/// </summary>
	public partial class FormatFixtureBk : Form {
		private string initalDir = @"G:\ZALES\FIXTURE BOOK\SECTIONS";
		private ToolTip masterXLS_tooltip = new ToolTip();
		private ToolTip saveAs_tooltip = new ToolTip();
		private SldWorks SwApp;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="sldWorks">A <see cref="SldWorks"/> object. Needed if we're going
		/// to open and close drawings.</param>
		public FormatFixtureBk(SldWorks sldWorks) {
			SwApp = sldWorks;
			InitializeComponent();
		}

		private string check_for_missing_files(LinkedList<PageInfo> _ll) {
			var nd_ = _ll.First;
			string missing_ = string.Empty;
			while (nd_ != null) {
				if (nd_.Value.fileInfo == null) {
					missing_ += string.Format(@"Item: {0}, Row: {1}{2}",
						nd_.Value.Name, nd_.Value.CellAddress, System.Environment.NewLine);
				}
				nd_ = nd_.Next;
			}
			return missing_;
		}

		private void CheckForMissingFiles(LinkedList<PageInfo> _ll) {
			string missing_ = check_for_missing_files(_ll);
			if (missing_ != string.Empty) {
				MessageBox.Show(string.Format(@"Found no files for the following rows:{0}{1}",
					System.Environment.NewLine, missing_),
					@"Missing files.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		private void CheckForMissingFilesAndThrow(LinkedList<PageInfo> _ll) {
			string missing_ = check_for_missing_files(_ll);
			if (missing_ != string.Empty) {
				throw new System.Exception(string.Format(@"Found no files for the following rows:{0}{1}",
					System.Environment.NewLine, missing_));
			}
		}

		private LinkedList<PageInfo> maybe_create_from_slddrws(ExcelReader _er) {
			var ll_ = _er.ReadFile();
			CheckForMissingFilesAndThrow(ll_);
			return do_create_from_slddrw(ll_);
		}

		private LinkedList<PageInfo> do_create_from_slddrw(LinkedList<PageInfo> _ll) {
			PDFCreator p = new PDFCreator();
			p.Opening += P_Opening;
			p.Closing += P_Closing;
			var new_ll_ = p.CreateDrawings(SwApp, _ll);
			toggle_lbls(false);
			p.Opening -= P_Opening;
			p.Closing -= P_Closing;
			return (new_ll_);
		}

		private void do_merge_temp_files(LinkedList<PageInfo> _ll) {
			toggle_lbls(false);
			PDFCreator.Merge(_ll, new FileInfo(saveAs_tb.Text));
			toggle_lbls(true);
			action_lbl.Text = @"Done";
			target_lbl.Text = @"(ﾉ◕ヮ◕)ﾉ*:･ﾟ✧ ✧ﾟ･: *ヽ(◕ヮ◕ヽ)";
		}

		private void create_from_SLDDRW() {
			ExcelReader.ExcelReaderExtensionOptions extOpt =
				slddrw_rb.Checked ?
				ExcelReader.ExcelReaderExtensionOptions.SLDDRW :
				ExcelReader.ExcelReaderExtensionOptions.PDF;

			ExcelReader.ExcelReaderSearchOptions srchOpt =
				recurse_cb.Checked ?
				ExcelReader.ExcelReaderSearchOptions.RECURSE | ExcelReader.ExcelReaderSearchOptions.THIS_DIR :
				ExcelReader.ExcelReaderSearchOptions.THIS_DIR;
			ExcelReader er_ = new ExcelReader(masterXLS_tb.Text, extOpt, srchOpt);
			try {
				er_.NewDir += Er__NewDir;
				do_merge_temp_files(maybe_create_from_slddrws(er_));
				er_.NewDir -= Er__NewDir;
			} catch (ExcelReaderException ere) {
				MessageBox.Show(ere.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			} catch (ExcelReaderFoundNoFilesException nf) {
				MessageBox.Show(nf.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			} catch (System.Exception x) {
				string msg_ = string.Format(@"{0}{1}Continue?", x.Message, System.Environment.NewLine);
				DialogResult dr_ = MessageBox.Show(x.Message, @"Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
				if (dr_ == DialogResult.OK) {
					er_ = new ExcelReader(masterXLS_tb.Text, extOpt, srchOpt);
					do_merge_temp_files(do_create_from_slddrw(er_.ReadFile()));
				}
			}
		}

		private void create_from_PDF() {
			ExcelReader.ExcelReaderExtensionOptions extOpt =
				slddrw_rb.Checked ?
				ExcelReader.ExcelReaderExtensionOptions.SLDDRW :
				ExcelReader.ExcelReaderExtensionOptions.PDF;

			ExcelReader.ExcelReaderSearchOptions srchOpt =
				recurse_cb.Checked ?
				ExcelReader.ExcelReaderSearchOptions.RECURSE | ExcelReader.ExcelReaderSearchOptions.THIS_DIR :
				ExcelReader.ExcelReaderSearchOptions.THIS_DIR;
			try {
				ExcelReader er_ = er_ = new ExcelReader(masterXLS_tb.Text,
						extOpt,
						srchOpt);
				er_.NewDir += Er__NewDir;
				var ll_ = er_.ReadFile();
				PDFCreator.Merge(ll_, new FileInfo(saveAs_tb.Text));
				toggle_lbls(false);
				er_.NewDir -= Er__NewDir;
				CheckForMissingFiles(ll_);
			} catch (ExcelReaderException ere) {
				MessageBox.Show(ere.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			} catch (ExcelReaderFoundNoFilesException nf) {
				MessageBox.Show(nf.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			} catch (IOException ioe) {
				MessageBox.Show(ioe.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			} catch (System.Exception x) {
				if (x.InnerException != null) {
					MessageBox.Show(x.InnerException.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void go_Click(object sender, System.EventArgs e) {
			Cursor = Cursors.WaitCursor;
			if (saveAs_tb.Text == string.Empty) {
				Redbrick.Err(saveAs_tb);
				saveAs_tooltip.SetToolTip(saveAs_tb, @"You must name a valid target file.");
				Cursor = Cursors.Default;
				return;
			}
			FileInfo fi_ = null;
			try {
				fi_ = new FileInfo(masterXLS_tb.Text);
			} catch (System.ArgumentException ae) {
				MessageBox.Show(ae.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Redbrick.Err(masterXLS_tb);
				masterXLS_tooltip.SetToolTip(masterXLS_tb, @"You must specify an Excel file.");
				Cursor = Cursors.Default;
				return;
			}

			if (fi_.Exists) {
				if (slddrw_rb.Checked) {
					create_from_SLDDRW();
				} else {
					create_from_PDF();
				}				
			} else {
				Redbrick.Err(masterXLS_tb);
				masterXLS_tooltip.SetToolTip(masterXLS_tb, string.Format(@"'{0}' does not exist.", fi_.FullName));
			}

			if (saveAs_tb.Text.Length > 0 && new FileInfo(saveAs_tb.Text).Exists) {
				openbtn.Enabled = true;
			}
			Cursor = Cursors.Default;
		}

		private void toggle_lbls(bool b) {
			action_lbl.Visible = b;
			target_lbl.Visible = b;
		}

		private void P_Closing(object sender, System.EventArgs e) {
			toggle_lbls(true);
			action_lbl.Text = @"Processing";
			target_lbl.Text = (e as FileSystemEventArgs).Name;
		}

		private void P_Opening(object sender, System.EventArgs e) {
			toggle_lbls(true);
			action_lbl.Text = @"Processing";
			target_lbl.Text = (e as FileSystemEventArgs).Name;
		}

		private void Er__NewDir(object sender, System.EventArgs e) {
			toggle_lbls(true);
			target_lbl.Text = (e as FileSystemEventArgs).Name;
		}

		private void xls_browse_Click(object sender, System.EventArgs e) {
			OpenFileDialog ofd_ = new OpenFileDialog();
			ofd_.Filter = @"Excel Files (*.xlsx, *.xls)|*.xlsx;*.xls";
			ofd_.FilterIndex = 0;
			ofd_.InitialDirectory = Properties.Settings.Default.FBLastPath;

			if (ofd_.ShowDialog() == DialogResult.OK) {
				masterXLS_tb.Text = ofd_.FileName;
				Redbrick.UnErr(masterXLS_tb);
				masterXLS_tooltip.RemoveAll();
			}
		}

		private void saveAs_browse_Click(object sender, System.EventArgs e) {
			SaveFileDialog sfd_ = new SaveFileDialog();
			sfd_.Filter = @"PDF Files (*.pdf)|*.pdf";
			string path_ = string.Empty;
			try {
				path_ = Path.GetDirectoryName(masterXLS_tb.Text);
			} catch (System.ArgumentException) {
				path_ = Properties.Settings.Default.FBLastPath;
			}
			sfd_.InitialDirectory = path_;
			sfd_.SupportMultiDottedExtensions = false;
			if (sfd_.ShowDialog() == DialogResult.OK) {
				saveAs_tb.Text = sfd_.FileName;
				Redbrick.UnErr(saveAs_tb);
				saveAs_tooltip.RemoveAll();
			}
		}

		private void masterXLS_tb_TextChanged(object sender, System.EventArgs e) {
			Redbrick.UnErr(sender as Control);
			masterXLS_tooltip.RemoveAll();
		}

		private void saveAs_tb_TextChanged(object sender, System.EventArgs e) {
			Redbrick.UnErr(sender as Control);
			saveAs_tooltip.RemoveAll();
		}

		private void cancel_Click(object sender, System.EventArgs e) {
			Close();
		}

		private void openbtn_Click(object sender, System.EventArgs e) {
			if (new FileInfo(saveAs_tb.Text).Exists) {
				System.Diagnostics.Process.Start(saveAs_tb.Text);
			}
		}

		private void FormatFixtureBk_FormClosing(object sender, FormClosingEventArgs e) {
			Properties.Settings.Default.FBLocation = Location;
			Properties.Settings.Default.FBLastPath = initalDir;
			Properties.Settings.Default.Save();
		}

		private void FormatFixtureBk_Load(object sender, System.EventArgs e) {
			Location = Properties.Settings.Default.FBLocation;
			initalDir = Properties.Settings.Default.FBLastPath;
		}
	}
}
