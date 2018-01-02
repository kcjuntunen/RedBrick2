﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace RedBrick2 {
	/// <summary>
	/// Configuration window for optional stuff.
	/// </summary>
	public partial class RedbrickConfiguration : Form {
		//private CutlistData cd = new CutlistData();
		private bool initialated = false;
		private bool sound_clicked = false;
		private DateTime odometerStart;
		private double workDays;

		/// <summary>
		/// Constructor.
		/// </summary>
		public RedbrickConfiguration() {
			InitializeComponent();
			Version cv = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
			string ver = cv.ToString();
#if DEBUG
			ver += "DEBUG";
#endif

			Text = "Redbrick Configuration v" + ver;
			init();
		}

		/// <summary>
		/// Pull data from config resources and populate the form.
		/// </summary>
		private void init() {
			for (int i = 0; i < 26; i++) {
				cbRevLimit.Items.Add("A" + (char)(i + 65));
			}

			ToolTip tt = new ToolTip();
			tt.ShowAlways = true;
			tt.SetToolTip(textBox1, Properties.Resources.RegexHint);
			tt.SetToolTip(textBox8, Properties.Resources.RegexHint);
			tt.SetToolTip(label4, Properties.Resources.RegexHint);
			tt.SetToolTip(label13, Properties.Resources.RegexHint);
			var t = new System.Globalization.CultureInfo("en-US", false).TextInfo;
			ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter guta =
				new ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter();
			string currentUserName = t.ToTitleCase(guta.GetCurrentUserFullname(Environment.UserName));
			dataGridView1.AutoResizeRows();
			dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
			dataGridView1.AutoResizeColumns();
			dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
			dataGridView1.DataSource = get_stats();
			dataGridView1.Columns["Avg Daily Usage"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			dataGridView1.Columns["Total Since Reset"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			dataGridView1.Columns["Your Avg Daily Usage"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			dataGridView1.Columns["Your Total Since Reset"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			dataGridView1.Columns["σ"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

			dataGridView1.Columns["Avg Daily Usage"].ToolTipText = string.Format(@"{0} work days since {1}", workDays, odometerStart.ToShortDateString());
			dataGridView1.Columns["Total Since Reset"].ToolTipText = @"Reset @ " + odometerStart.ToShortDateString();
			dataGridView1.Columns["Your Avg Daily Usage"].ToolTipText = string.Format(@"{0}'s average", currentUserName);
			dataGridView1.Columns["Your Total Since Reset"].ToolTipText =
				string.Format(@"{0}'s total, reset @ {1}", currentUserName, odometerStart.ToShortDateString());
			dataGridView1.Columns["σ"].ToolTipText = @"Standard deviation";

			dataGridView1.Columns["Avg Daily Usage"].DefaultCellStyle.Format = @"#.###";
			dataGridView1.Columns["Your Avg Daily Usage"].DefaultCellStyle.Format = @"#.###";
			dataGridView1.Columns["σ"].DefaultCellStyle.Format = @"#.###";
			initialated = true;
		}

		private double WorkDays() {
			System.IO.FileInfo pi = new System.IO.FileInfo(Properties.Settings.Default.InstallerNetworkPath);
			odometerStart = Redbrick.GetOdometerStart(pi.DirectoryName + @"\version.xml");
			DateTime end = DateTime.Now;

			int dowStart = ((int)odometerStart.DayOfWeek == 0 ? 7 : (int)odometerStart.DayOfWeek);
			int dowEnd = ((int)end.DayOfWeek == 0 ? 7 : (int)end.DayOfWeek);
			TimeSpan tSpan = end - odometerStart;
			if (dowStart <= dowEnd) {
				return (((tSpan.Days / 7) * 5) + Math.Max((Math.Min((dowEnd + 1), 6) - dowStart), 0));
			}
			return (((tSpan.Days / 7) * 5) + Math.Min((dowEnd + 6) - Math.Min(dowStart, 6), 5));
		}

		private DataView get_stats() {
			ENGINEERINGDataSetTableAdapters.GEN_ODOMETERTableAdapter gota =
				new ENGINEERINGDataSetTableAdapters.GEN_ODOMETERTableAdapter();
			ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter guta =
				new ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter();
			DataTable dt = new DataTable();
			dt.Columns.Add("Function");
			dt.Columns.Add("Avg Daily Usage", typeof(double));
			dt.Columns.Add("Total Since Reset", typeof(int));
			dt.Columns.Add("Your Avg Daily Usage", typeof(double));
			dt.Columns.Add("Your Total Since Reset", typeof(int));
			dt.Columns.Add("σ", typeof(double));
			workDays = WorkDays();
			foreach (object item in Enum.GetValues(typeof(Redbrick.Functions))) {
				string x = Enum.GetName(typeof(Redbrick.Functions), (Redbrick.Functions)item);
				int f = 0;
				int mf = 0;

				try {
					f = (int)gota.GetOdometerTotalValue((int)item);
				} catch (Exception) {
					//
				}

				try {
					mf = (int)gota.GetOdometerValue((int)item, guta.GetUID(Environment.UserName));
				} catch (Exception) {
					//
				}

				double y = f / workDays;
				double my = mf / workDays;
				double σ = 0;

				try {
					σ = (double)gota.GetOdometerStdDev((int)item);
				} catch (Exception) {
					//
				}

				if (y > 0) {
					DataRow dr = dt.NewRow();
					dr["Function"] = x;
					dr["Avg Daily Usage"] = y;
					dr["Total Since Reset"] = f;
					dr["Your Avg Daily Usage"] = my;
					dr["Your Total Since Reset"] = mf;
					dr["σ"] = σ;
					dt.Rows.Add(dr);
				}
			}
			dt.DefaultView.Sort = "[Avg Daily Usage] DESC";
			return dt.DefaultView;
		}

		/// <summary>
		/// On load, restore position and size.
		/// </summary>
		/// <param name="sender">Who triggered this event?</param>
		/// <param name="e">Any data come with it?</param>
		private void RedbrickConfiguration_Load(object sender, EventArgs e) {
			// TODO: This line of code loads data into the 'eNGINEERINGDataSet.GEN_DEPTS' table. You can move, or remove it, as needed.
			this.gEN_DEPTSTableAdapter.Fill(this.eNGINEERINGDataSet.GEN_DEPTS);
			// TODO: This line of code loads data into the 'eNGINEERINGDataSet.CUT_MATERIALS' table. You can move, or remove it, as needed.
			this.cUT_MATERIALSTableAdapter.Fill(this.eNGINEERINGDataSet.CUT_MATERIALS);
			Location = Properties.Settings.Default.RBConfigLocation;
			Size = Properties.Settings.Default.RBConfigSize;

			cbDefaultMaterial.SelectedValue = Properties.Settings.Default.DefaultMaterial;
			new ToolTip().SetToolTip(cbDefaultMaterial, @"If a part has no defined material, this material will be used.");
			cbDept.SelectedValue = Properties.Settings.Default.UserDept;
			chbDBEnabled.Checked = true;
			chbFlameWar.Checked = Properties.Settings.Default.FlameWar;
			new ToolTip().SetToolTip(chbFlameWar, @"Toggle all caps in most fields.");
			chbTestingMode.Checked = true;
			cbRevLimit.SelectedIndex = Properties.Settings.Default.LvlLimit - 1;
			new ToolTip().SetToolTip(cbRevLimit, @"The highest possible LVL for drawings.");
			chbSounds.Checked = Properties.Settings.Default.MakeSounds;
			FileInfo fi_ = new FileInfo(Properties.Settings.Default.ClipboardSound);
			chbSounds.Text = string.Format(@"Sounds ( {0} )", fi_.Name);
			new ToolTip().SetToolTip(chbSounds, string.Format(@"Whether Redbrick makes sounds or not.{0}" + 
																												"When checkbox goes from unchecked to checked,{0}" +
																												"a dialog will appear to select a .wav file.",
																												Environment.NewLine));
			chbWarnings.Checked = Properties.Settings.Default.Warn;
			new ToolTip().SetToolTip(chbWarnings, @"Toggle validation warnings.");
			chbOpWarnings.Checked = Properties.Settings.Default.ProgWarn;
			new ToolTip().SetToolTip(chbOpWarnings, @"Toggle routing validation warnings.");
			chbIdiotLight.Checked = Properties.Settings.Default.IdiotLight;
			chbOnlyActive.Checked = Properties.Settings.Default.OnlyActiveAuthors;
			new ToolTip().SetToolTip(chbOnlyActive, @"Only show currently active authors.");
			chbOnlyActiveCustomers.Checked = Properties.Settings.Default.OnlyCurrentCustomers;
			new ToolTip().SetToolTip(chbOnlyActiveCustomers, @"Only show currently active customers.");
			chbRememberCustomer.Checked = Properties.Settings.Default.RememberLastCustomer;
			checkBox1.Checked = Properties.Settings.Default.WarnExcludeAssy;
			checkBox2.Checked = true;
			textBox1.Text = Properties.Settings.Default.BOMFilter[0].ToString();
			textBox2.Text = Properties.Settings.Default.GaugePath;
			new ToolTip().SetToolTip(textBox2, @"XML data file with gauge size information. Double-click to change.");
			textBox3.Text = Properties.Settings.Default.BOMTemplatePath;
			textBox4.Text = Properties.Settings.Default.JPGPath;
			new ToolTip().SetToolTip(textBox4, @"Where to save JPGs. Double-click to change.");
			textBox5.Text = Properties.Settings.Default.KPath;
			new ToolTip().SetToolTip(textBox5, @"K-drive PDF path. Double-click to change.");
			textBox6.Text = Properties.Settings.Default.GPath;
			new ToolTip().SetToolTip(textBox6, @"PDF Archive path. Double-click to change.");
			textBox7.Text = Properties.Settings.Default.MetalPath;
			new ToolTip().SetToolTip(textBox7, @"Metal root folder. Double-click to change.");
			textBox8.Text = Properties.Settings.Default.GaugeRegex;
			checkBox3.Checked = Properties.Settings.Default.SaveFirst;
			new ToolTip().SetToolTip(checkBox3, @"Save before archiving.");
			checkBox4.Checked = Properties.Settings.Default.SilenceGaugeErrors;
			new ToolTip().SetToolTip(checkBox4, @"Ignore gauge discrepancies.");
			checkBox5.Checked = Properties.Settings.Default.ExportEDrw;
			checkBox6.Checked = Properties.Settings.Default.ExportImg;
			checkBox7.Checked = Properties.Settings.Default.CutlistNotSelectedWarning;
			checkBox8.Checked = Properties.Settings.Default.AutoOpenPriority;
			new ToolTip().SetToolTip(checkBox8, string.Format(@"When 'Update CNC' goes from checked to unchecked,{0}" +
																												"the priority window will open at the time the green check is clicked.",
																												Environment.NewLine));
			numericUpDown1.Value = Properties.Settings.Default.SPQ;
			ToolTip tt_ = new ToolTip();
			string nud1_ = @"Number of parts to amortize setup time over.";
			tt_.SetToolTip(label8, nud1_);
			tt_.SetToolTip(numericUpDown1, nud1_);
		}

		/// <summary>
		/// I was saving data here, but it didn't seem to keep.
		/// </summary>
		/// <param name="sender">Who triggered this event?</param>
		/// <param name="e">Any data come with it?</param>
		private void RedbrickConfiguration_FormClosing(object sender, FormClosingEventArgs e) {
		}

		/// <summary>
		/// Update user department data.
		/// </summary>
		/// <param name="sender">Who triggered this event?</param>
		/// <param name="e">Any data come with it?</param>
		private void cbDept_SelectedIndexChanged(object sender, EventArgs e) {
			if (initialated) {
				int tp = 0;
				if (int.TryParse(cbDept.SelectedValue.ToString(), out tp)) {
					Properties.Settings.Default.UserDept = tp;
				}
			}
		}

		/// <summary>
		/// Update rev limit data
		/// </summary>
		/// <param name="sender">Who triggered this event?</param>
		/// <param name="e">Any data come with it?</param>
		private void cbRevLimit_SelectedIndexChanged(object sender, EventArgs e) {
			Properties.Settings.Default.LvlLimit = (int)cbRevLimit.SelectedIndex + 1;
		}

		/// <summary>
		/// Update checkbox data.
		/// </summary>
		/// <param name="sender">Who triggered this event?</param>
		/// <param name="e">Any data come with it?</param>
		private void chbDBEnabled_CheckedChanged(object sender, EventArgs e) {
			//Properties.Settings.Default.EnableDBWrite = chbDBEnabled.Checked;
		}

		/// <summary>
		/// Update checkbox data.
		/// </summary>
		/// <param name="sender">Who triggered this event?</param>
		/// <param name="e">Any data come with it?</param>
		private void chbTestingMode_CheckedChanged(object sender, EventArgs e) {
			//Properties.Settings.Default.Testing = chbTestingMode.Checked;
		}

		/// <summary>
		/// Save data and close.
		/// </summary>
		/// <param name="sender">Who triggered this event?</param>
		/// <param name="e">Any data come with it?</param>
		private void btnOK_Click(object sender, EventArgs e) {
			System.Collections.Specialized.StringCollection sc = new System.Collections.Specialized.StringCollection();
			sc.Add(textBox1.Text);

			Properties.Settings.Default.BOMFilter = sc;
			Properties.Settings.Default.RBConfigLocation = Location;
			Properties.Settings.Default.RBConfigSize = Size;
			Properties.Settings.Default.Save();
			Close();
		}

		/// <summary>
		/// Just close.
		/// </summary>
		/// <param name="sender">Who triggered this event?</param>
		/// <param name="e">Any data come with it?</param>
		private void btnCancel_Click(object sender, EventArgs e) {
			Close();
		}

		/// <summary>
		/// What material should we default to?
		/// </summary>
		/// <param name="sender">Who triggered this event?</param>
		/// <param name="e">Any data come with it?</param>
		private void cbDefaultMaterial_SelectedIndexChanged(object sender, EventArgs e) {
			int tp = Properties.Settings.Default.DefaultMaterial;
			if (initialated && int.TryParse(cbDefaultMaterial.SelectedValue.ToString(), out tp)) {
				Properties.Settings.Default.DefaultMaterial = tp;
			}
		}

		/// <summary>
		/// Update checkbox data.
		/// </summary>
		/// <param name="sender">Who triggered this event?</param>
		/// <param name="e">Any data come with it?</param>
		private void chbFlameWar_CheckedChanged(object sender, EventArgs e) {
			Properties.Settings.Default.FlameWar = chbFlameWar.Checked;
		}

		/// <summary>
		/// Update checkbox data.
		/// </summary>
		/// <param name="sender">Who triggered this event?</param>
		/// <param name="e">Any data come with it?</param>
		private void chbWarnings_CheckedChanged(object sender, EventArgs e) {
			Properties.Settings.Default.Warn = chbWarnings.Checked;
			if (!chbWarnings.Checked) {
				chbOpWarnings.Checked = false;
				checkBox7.Checked = false;
			}
			tableLayoutPanel5.Enabled = chbWarnings.Checked;
		}

		/// <summary>
		/// Update checkbox data. Also triggers a file selection box.
		/// </summary>
		/// <param name="sender">Who triggered this event?</param>
		/// <param name="e">Any data come with it?</param>
		private void chbSounds_CheckedChanged(object sender, EventArgs e) {
			Properties.Settings.Default.MakeSounds = chbSounds.Checked;
			if (chbSounds.Checked && sound_clicked) {
				OpenFileDialog ofd = new OpenFileDialog();
				ofd.InitialDirectory = System.IO.Path.GetDirectoryName(Properties.Settings.Default.ClipboardSound);
				ofd.FileName = System.IO.Path.GetFileName(Properties.Settings.Default.ClipboardSound);
				ofd.Filter = "Audio Files (*.wav)|*.wav";
				if (ofd.ShowDialog() == DialogResult.OK) {
					Properties.Settings.Default.ClipboardSound = ofd.FileName;
					FileInfo fi_ = new FileInfo(ofd.FileName);
					chbSounds.Text = string.Format(@"Sounds ( {0} )", fi_.Name);
				} else {
					chbSounds.Checked = false;
				}
				sound_clicked = false;
			}
		}

		/// <summary>
		/// Update checkbox data.
		/// </summary>
		/// <param name="sender">Who triggered this event?</param>
		/// <param name="e">Any data come with it?</param>
		private void chbSounds_Click(object sender, EventArgs e) {
			sound_clicked = true;
		}

		/// <summary>
		/// Update checkbox data.
		/// </summary>
		/// <param name="sender">Who triggered this event?</param>
		/// <param name="e">Any data come with it?</param>
		private void chbIdiotLight_CheckedChanged(object sender, EventArgs e) {
			Properties.Settings.Default.IdiotLight = chbIdiotLight.Checked;
		}

		/// <summary>
		/// Update checkbox data.
		/// </summary>
		/// <param name="sender">Who triggered this event?</param>
		/// <param name="e">Any data come with it?</param>
		private void chbOnlyActive_CheckedChanged(object sender, EventArgs e) {
			Properties.Settings.Default.OnlyActiveAuthors = chbOnlyActive.Checked;
		}

		/// <summary>
		/// Update checkbox data.
		/// </summary>
		/// <param name="sender">Who triggered this event?</param>
		/// <param name="e">Any data come with it?</param>
		private void chbOnlyActiveCustomers_CheckedChanged(object sender, EventArgs e) {
			Properties.Settings.Default.OnlyCurrentCustomers = chbOnlyActiveCustomers.Checked;
		}

		/// <summary>
		/// Update checkbox data.
		/// </summary>
		/// <param name="sender">Who triggered this event?</param>
		/// <param name="e">Any data come with it?</param>
		private void chbCustomerWarn_CheckedChanged(object sender, EventArgs e) {
			Properties.Settings.Default.RememberLastCustomer = chbRememberCustomer.Checked;
		}

		/// <summary>
		/// Update checkbox data.
		/// </summary>
		/// <param name="sender">Who triggered this event?</param>
		/// <param name="e">Any data come with it?</param>
		private void chbOpWarnings_CheckedChanged(object sender, EventArgs e) {
			Properties.Settings.Default.ProgWarn = chbOpWarnings.Checked;
		}

		/// <summary>
		/// Update checkbox data.
		/// </summary>
		/// <param name="sender">Who triggered this event?</param>
		/// <param name="e">Any data come with it?</param>
		private void checkBox1_CheckedChanged(object sender, EventArgs e) {
			Properties.Settings.Default.WarnExcludeAssy = checkBox1.Checked;
		}

		/// <summary>
		/// Update textbox data.
		/// </summary>
		/// <param name="sender">Who triggered this event?</param>
		/// <param name="e">Any data come with it?</param>
		private void textBox1_Leave(object sender, EventArgs e) {
		}

		/// <summary>
		/// Update checkbox data.
		/// </summary>
		/// <param name="sender">Who triggered this event?</param>
		/// <param name="e">Any data come with it?</param>
		private void checkBox2_CheckedChanged(object sender, EventArgs e) {
			//Properties.Settings.Default.CHIHideLWH = checkBox2.Checked;
		}

		/// <summary>
		/// On doubleclick, select xml file.
		/// </summary>
		/// <param name="sender">Who triggered this event?</param>
		/// <param name="e">Any data come with it?</param>
		private void textBox2_DoubleClick(object sender, EventArgs e) {
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.InitialDirectory = System.IO.Path.GetDirectoryName(Properties.Settings.Default.GaugePath);
			ofd.FileName = System.IO.Path.GetFileName(Properties.Settings.Default.GaugePath);
			ofd.Filter = "XML Data (*.xml)|*.xml";
			if (ofd.ShowDialog() == DialogResult.OK) {
				textBox2.Text = ofd.FileName;
				Properties.Settings.Default.GaugePath = ofd.FileName;
				Properties.Settings.Default.Save();
			} else {

			}
		}

		private void checkBox3_CheckedChanged(object sender, EventArgs e) {
			Properties.Settings.Default.SaveFirst = checkBox3.Checked;
		}

		private void checkBox4_CheckedChanged(object sender, EventArgs e) {
			Properties.Settings.Default.SilenceGaugeErrors = checkBox4.Checked;
		}

		private void checkBox5_CheckedChanged(object sender, EventArgs e) {
			Properties.Settings.Default.ExportEDrw = checkBox5.Checked;
		}

		private void checkBox6_CheckedChanged(object sender, EventArgs e) {
			Properties.Settings.Default.ExportImg = checkBox6.Checked;
		}

		private void checkBox7_CheckedChanged(object sender, EventArgs e) {
			Properties.Settings.Default.CutlistNotSelectedWarning = checkBox7.Checked;
		}

		private void checkBox1_CheckedChanged_1(object sender, EventArgs e) {
			Properties.Settings.Default.WarnExcludeAssy = checkBox1.Checked;
		}

		private void combobox_KeyDown(object sender, KeyEventArgs e) {
			if (sender is ComboBox)
				(sender as ComboBox).DroppedDown = false;
		}

		private void comboBox_Resize(object sender, EventArgs e) {
			ComboBox _me = (sender as ComboBox);
			_me.SelectionLength = 0;
		}

		private void checkBox8_CheckedChanged(object sender, EventArgs e) {
			Properties.Settings.Default.AutoOpenPriority = checkBox8.Checked;
		}

		private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) {

		}

		private void button1_Click(object sender, EventArgs e) {
			AboutBox ab = new AboutBox();
			ab.ShowDialog();
		}

		private void numericUpDown1_ValueChanged(object sender, EventArgs e) {
			Properties.Settings.Default.SPQ = (int)numericUpDown1.Value;
		}

		private void textBox3_MouseDoubleClick(object sender, MouseEventArgs e) {
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.InitialDirectory = System.IO.Path.GetDirectoryName(Properties.Settings.Default.BOMTemplatePath);
			ofd.FileName = System.IO.Path.GetFileName(Properties.Settings.Default.BOMTemplatePath);
			ofd.Filter = "Table Template Files (*.sldbomtbt)|*.sldbomtbt";
			if (ofd.ShowDialog(this) == DialogResult.OK) {
				textBox3.Text = ofd.FileName;
				Properties.Settings.Default.BOMTemplatePath = ofd.FileName;
				Properties.Settings.Default.Save();
			}
		}

		private void textBox4_MouseDoubleClick(object sender, MouseEventArgs e) {
			TextBox tb = (sender as TextBox);
			FolderBrowserDialog fbd = new FolderBrowserDialog();
			fbd.RootFolder = Environment.SpecialFolder.Desktop;
			FileInfo f = new FileInfo(tb.Text);
			fbd.SelectedPath = f.FullName;
			if (fbd.ShowDialog(this) == System.Windows.Forms.DialogResult.OK) {
				tb.Text = fbd.SelectedPath;
				Properties.Settings.Default.JPGPath = fbd.SelectedPath;
				Properties.Settings.Default.Save();
			}
		}

		private void textBox5_MouseDoubleClick(object sender, MouseEventArgs e) {
			TextBox tb = (sender as TextBox);
			FolderBrowserDialog fbd = new FolderBrowserDialog();
			fbd.RootFolder = Environment.SpecialFolder.Desktop;
			FileInfo f = new FileInfo(tb.Text);
			fbd.SelectedPath = f.FullName;
			if (fbd.ShowDialog(this) == System.Windows.Forms.DialogResult.OK) {
				tb.Text = fbd.SelectedPath;
				Properties.Settings.Default.KPath = fbd.SelectedPath;
				Properties.Settings.Default.Save();
			}
		}

		private void textBox6_MouseDoubleClick(object sender, MouseEventArgs e) {
			TextBox tb = (sender as TextBox);
			FolderBrowserDialog fbd = new FolderBrowserDialog();
			fbd.RootFolder = Environment.SpecialFolder.Desktop;
			FileInfo f = new FileInfo(tb.Text);
			fbd.SelectedPath = f.FullName;
			if (fbd.ShowDialog(this) == System.Windows.Forms.DialogResult.OK) {
				tb.Text = fbd.SelectedPath;
				Properties.Settings.Default.GPath = fbd.SelectedPath;
				Properties.Settings.Default.Save();
			}
		}

		private void textBox7_MouseDoubleClick(object sender, MouseEventArgs e) {
			TextBox tb = (sender as TextBox);
			FolderBrowserDialog fbd = new FolderBrowserDialog();
			fbd.RootFolder = Environment.SpecialFolder.Desktop;
			FileInfo f = new FileInfo(tb.Text);
			fbd.SelectedPath = f.FullName;
			if (fbd.ShowDialog(this) == System.Windows.Forms.DialogResult.OK) {
				tb.Text = fbd.SelectedPath;
				Properties.Settings.Default.MetalPath = fbd.SelectedPath;
				Properties.Settings.Default.Save();
			}
		}

		private void textBox8_TextChanged(object sender, EventArgs e) {
			Properties.Settings.Default.GaugeRegex = textBox8.Text;
		}
	}
}