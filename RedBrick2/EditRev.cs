using System;
using System.IO;
using System.Windows.Forms;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swcommands;
using ArchivePDF.csproj;

namespace RedBrick2 {
	/// <summary>
	/// A form for editing LVL data.
	/// </summary>
	public partial class EditRev : Form {
		private int index = 0;
		private bool NewRev = false;
		private bool del_flag_ = true;
		private Revs RevSet;
		private Rev ThisRev;
		private StringProperty level;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="idx">Index of the Rev to be edited.</param>
		/// <param name="revSet">The set of sets of rev properties we're operating on.</param>
		public EditRev(int idx, Revs revSet) {
			InitializeComponent();
			index = idx;
			RevSet = revSet;
			NewRev = false;
			level = new StringProperty(@"REVISION LEVEL", true, RevSet.SwApp, (RevSet.SwApp.ActiveDoc as ModelDoc2), @"REV");
			level.Get();
			ToggleFlameWar(Properties.Settings.Default.FlameWar);
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="revSet">Set of sets of LVL properties.</param>
		public EditRev(Revs revSet) {
			InitializeComponent();
			RevSet = revSet;
			NewRev = true;
			level = new StringProperty(@"REVISION LEVEL", true, RevSet.SwApp, (RevSet.SwApp.ActiveDoc as ModelDoc2), @"REV");
			level.Get();
			ToggleFlameWar(Properties.Settings.Default.FlameWar);
		}

		/// <summary>
		/// Make text ugly in certain fields.
		/// </summary>
		/// <param name="on">A bool.</param>
		public void ToggleFlameWar(bool on) {
			if (on) {
				textBox1.CharacterCasing = CharacterCasing.Upper;
				textBox2.CharacterCasing = CharacterCasing.Upper;
			} else {
				textBox1.CharacterCasing = CharacterCasing.Normal;
				textBox2.CharacterCasing = CharacterCasing.Normal;
			}
		}

		/// <summary>
		/// A handler for an event when a routing is added.
		/// </summary>
		public event EventHandler Added;
		/// <summary>
		/// A handler for an event when a routing is added.
		/// </summary>
		public event EventHandler AddedLvl;

		/// <summary>
		/// Fire events.
		/// </summary>
		/// <param name="e">EventArgs.</param>
		protected virtual void OnAdded(EventArgs e) {
			if (Added != null) {
				Added(this, e);
			}
		}

		/// <summary>
		/// Fire events.
		/// </summary>
		/// <param name="e">EventArgs.</param>
		protected virtual void OnAddedLvl(EventArgs e) {
			if (AddedLvl != null) {
				AddedLvl(this, e);
			}
		}

		private void button1_Click(object sender, EventArgs e) {
			ThisRev.ECO = textBox1.Text;
			ThisRev.Description = textBox2.Text;
			ThisRev.AuthorID = Convert.ToInt32(comboBox2.SelectedValue);
			ThisRev.Date = dateTimePicker1.Value;
			if (NewRev) {
				AddECRItem();
			}
			del_flag_ = false;
			Close();
		}

		private void CheckSaved() {
			ModelDoc2 md = (ModelDoc2)RevSet.SwApp.ActiveDoc;
			string fn = md.GetPathName();
			if (fn == string.Empty) {
				string partfilename = md.GetTitle();
				md.Extension.RunCommand((int)swCommands_e.swCommands_SaveAs, partfilename);
				fn = md.GetPathName();
				if (fn == string.Empty) {
					throw new Exception("Unsaved drawings cannot be added to an ECR.");
				}
			}
		}

		private void AddECRItem() {
			CheckSaved();
			string question = string.Format(Properties.Resources.InsertIntoEcrItems, ThisRev.PartNumber, ThisRev.ECO);

			DialogResult mbr = DialogResult.No;
			int en = 0;
			using (ENGINEERINGDataSet.ECRObjLookupDataTable eoldt =
				new ENGINEERINGDataSet.ECRObjLookupDataTable()) {
				using (ENGINEERINGDataSet.ECR_ITEMSDataTable eidt =
					new ENGINEERINGDataSet.ECR_ITEMSDataTable()) {
					if (int.TryParse(ThisRev.ECO, out en) &&
						!eoldt.ECRIsBogus(en) &&
						!eidt.ECRItemExists(en, ThisRev.PartNumber, level.Value)) {
						mbr = (DialogResult)MessageBox.Show(this, question, @"Insert ECR?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
					}
				}
			}
			if (mbr == DialogResult.Yes) {
				OnAdded(EventArgs.Empty);
				ArchivePDFWrapper apw = new ArchivePDFWrapper(RevSet.SwApp, Redbrick.GeneratePathSet());
				apw.Archive();
				using (ENGINEERINGDataSet.inmastDataTable idt =
					new ENGINEERINGDataSet.inmastDataTable()) {
						string pdf_lookup = Path.GetFileNameWithoutExtension(ThisRev.ReferencedFile.Name);

					using (ENGINEERINGDataSet.GEN_DRAWINGSDataTable gdt =
						new ENGINEERINGDataSet.GEN_DRAWINGSDataTable()) {
						ENGINEERINGDataSetTableAdapters.ECR_ITEMSTableAdapter eit =
							new ENGINEERINGDataSetTableAdapters.ECR_ITEMSTableAdapter();
						int parttype = idt.GetECRItemType(ThisRev.PartNumber, level.Value);
						eit.InsertECRItem(en, ThisRev.PartNumber, level.Value, parttype);
						int ecr_item_id = (int)eit.GetECRItem(en, ThisRev.PartNumber, level.Value);
						using (ENGINEERINGDataSet.GEN_DRAWINGSDataTable gdtt = gdt.GetPDFData(pdf_lookup)) {
							if (gdtt.Rows.Count > 0) {
								System.Data.DataRow r = gdtt.Rows[0];
								FileInfo orig_path = gdtt.GetPDFLocation(pdf_lookup);
								FileInfo drw_file = new FileInfo(string.Format(@"{0}\{1}_{2}-{3}.PDF", orig_path.DirectoryName, en, ThisRev.PartNumber, ThisRev.Level));
								FileInfo dest_file = new FileInfo(string.Format(@"{0}\{1}", Properties.Settings.Default.ECRDrawingsDestination, drw_file.Name));
								ENGINEERINGDataSetTableAdapters.ECR_DRAWINGSTableAdapter edta =
									new ENGINEERINGDataSetTableAdapters.ECR_DRAWINGSTableAdapter();
								edta.InsertECRDrawing(ecr_item_id, (int)gdtt.Rows[0][@"FileID"], ThisRev.Level, drw_file.Name, orig_path.FullName);
								if (!dest_file.Exists) { // Doublecheck
									orig_path.CopyTo(dest_file.FullName, false); // Triplecheck
								}
								eit.SetNewECRWIP(en);
							}
						}
					}
				}
				eNGINEERINGDataSet.GEN_ODOMETER.IncrementOdometer(Redbrick.Functions.InsertECR);
			}
			OnAddedLvl(EventArgs.Empty);
		}

		private void EditOp_Load(object sender, EventArgs e) {
			Location = Properties.Settings.Default.EditWindowLocation;
			Size = Properties.Settings.Default.EditRevSize;
			// TODO: This line of code loads data into the 'eNGINEERINGDataSet.GEN_USERS' table. You can move, or remove it, as needed.
			this.gEN_USERSTableAdapter.Fill(this.eNGINEERINGDataSet.GEN_USERS);

			if (Properties.Settings.Default.OnlyActiveAuthors) {
				gENUSERSBindingSource.Filter = @"ACTIVE = True AND DEPT = 6";
			} else {
				gENUSERSBindingSource.Filter = @"DEPT = 6";
			}

			if (NewRev) {
				using (ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter guta =
				new ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter()) {
					int? uid = guta.GetUID(System.Environment.UserName);
					if (uid == null) {
						throw new NullReferenceException(@"Hmm... Maybe you're not in the DB.");
					}
					index = RevSet.NewRev(string.Empty, string.Empty, (int)uid, DateTime.Now);
					ThisRev = RevSet[RevSet.Count - 1];
					comboBox1.Text = ThisRev.Level;
					Text = string.Format(@"Creating Rev Level {0}", comboBox1.Text);
					comboBox2.SelectedIndex = comboBox2.FindString(ThisRev.AuthorFullName);
					if (ThisRev.Level == @"AA") {
						textBox1.Text = @"NA";
						textBox2.Text = @"RELEASED";
					}
				}
			} else {
				ThisRev = RevSet[index];
				comboBox1.Text = ThisRev.Level;
				Text = string.Format(@"Editing Rev Level {0}", comboBox1.Text);
				if (Properties.Settings.Default.OnlyActiveAuthors) {
					gENUSERSBindingSource.Filter = string.Format(@"(ACTIVE = True AND DEPT = 6) OR UID = {0}", ThisRev.AuthorID);
				} else {
					gENUSERSBindingSource.Filter = @"DEPT = 6";
				}
				comboBox2.SelectedIndex = comboBox2.FindString(ThisRev.AuthorFullName);
				textBox1.Text = ThisRev.ECO;
				textBox2.Text = ThisRev.Description;
				dateTimePicker1.Value = ThisRev.Date;
			}

			comboBox1.Enabled = false;
		}

		private void button2_Click(object sender, EventArgs e) {
			Close();
		}

		private void comboBox1_TextUpdate(object sender, EventArgs e) {
			Text = string.Format(@"Creating Revision Level {0}", comboBox1.Text);
		}

		private void EditRev_FormClosing(object sender, FormClosingEventArgs e) {
			if (del_flag_ && NewRev) {
				RevSet.Remove(ThisRev);
			}
			Properties.Settings.Default.EditWindowLocation = Location;
			Properties.Settings.Default.EditRevSize = Size;
			Properties.Settings.Default.Save();
		}

		private void combobox_Resize(object sender, EventArgs e) {
			ComboBox _me = (sender as ComboBox);
			_me.SelectionLength = 0;
		}
	}
}