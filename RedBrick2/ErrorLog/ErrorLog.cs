using System;
using System.Drawing;
using System.Windows.Forms;

namespace RedBrick2.ErrorLog {
	public partial class ErrorLog : Form {
		public string DateFormatString { get; set; } = @"MM/dd/yyyy hh:mm:ss tt";

		public ErrorLog() {
			InitializeComponent();
			InitListView();

		}

		private void InitListView() {
			listView1.FullRowSelect = true;
			listView1.HideSelection = false;
			listView1.MultiSelect = false;
			listView1.View = View.Details;
			listView1.SmallImageList = Redbrick.TreeViewIcons;
		}

		private void Populate(bool showChecked) {
			using (ErrorDataSetTableAdapters.GEN_ERRORSTableAdapter errra =
				new ErrorDataSetTableAdapters.GEN_ERRORSTableAdapter()) {
				using (ErrorDataSet.GEN_ERRORSDataTable dt =
					errra.GetDataByApp(@"REDBRICK", dateTimePicker1.Value, dateTimePicker2.Value)) {
						listView1.Items.Clear();
					foreach (ErrorDataSet.GEN_ERRORSRow row in dt.Rows) {
						if (!showChecked && row.ERRCHK) {
							continue;
						}
						string[] data = new string[] {
							row.ERRDATE.ToString(DateFormatString),
							ErrCode(row.ERRNUM),
							row.ERRMSG,
							row.ERROBJ,
							row.ERRAPP,
							row.FullName,
							row.ERRID.ToString(),
						};
						ListViewItem lvi = new ListViewItem(data);
						lvi.Checked = row.ERRCHK;
						listView1.Items.Add(lvi);
					}
				}
			}
		}

		private void PopulateAll(bool showChecked) {
			using (ErrorDataSetTableAdapters.GEN_ERRORSTableAdapter errra =
				new ErrorDataSetTableAdapters.GEN_ERRORSTableAdapter()) {
				using (ErrorDataSet.GEN_ERRORSDataTable dt =
					errra.GetData(dateTimePicker1.Value, dateTimePicker2.Value)) {
					listView1.Items.Clear();
					foreach (ErrorDataSet.GEN_ERRORSRow row in dt.Rows) {
						if (!showChecked && row.ERRCHK) {
							continue;
						}
						string[] data = new string[] {
							row.ERRDATE.ToString(DateFormatString),
							ErrCode(row.ERRNUM),
							row.ERRMSG,
							row.ERROBJ,
							row.ERRAPP,
							row.FullName,
							row.ERRID.ToString(),
						};
						ListViewItem lvi = new ListViewItem(data);
						lvi.Checked = row.ERRCHK;
						listView1.Items.Add(lvi);
					}
				}
			}
		}

		private static string ErrCode(int errcode) {
			if (errcode < 0) {
				return string.Format(@"0x{0:x}", errcode);
			}
			return errcode.ToString();
		}

		private void ErrorLog_Load(object sender, EventArgs e) {
			Location = Properties.Settings.Default.ErrorLogLocation;
			Size = Properties.Settings.Default.ErrorLogSize;
			dateTimePicker1.Value = DateTime.Now.AddMonths(-1);
			Populate(false);
			WindowState = FormWindowState.Minimized;
			Show();
			WindowState = FormWindowState.Normal;
		}

		private void ReQuery() {
			if (active_app_only_chb.Checked) {
				Populate(!unchecked_only_chb.Checked);
			} else {
				PopulateAll(!unchecked_only_chb.Checked);
			}
		}

		private void listView1_SelectedIndexChanged(object sender, EventArgs e) {
			ListView lv = sender as ListView;
			if (lv.SelectedItems.Count < 1) {
				return;
			}
			Font font = richTextBox1.Font;
			Font labelFont = new Font(font, FontStyle.Italic | FontStyle.Underline);
			richTextBox1.Clear();

			ListViewItem lvi = lv.SelectedItems[0];
			textBox1.Text = Redbrick.TitleCase(lvi.SubItems[4].Text);
			textBox2.Text = Redbrick.TitleCase(lvi.SubItems[5].Text);

			richTextBox1.SelectionFont = labelFont;
			richTextBox1.SelectedText = @"Error message:";
			richTextBox1.SelectionFont = font;
			richTextBox1.AppendText(Environment.NewLine);
			richTextBox1.AppendText(lvi.SubItems[2].Text);

			richTextBox1.AppendText(Environment.NewLine);
			richTextBox1.AppendText(Environment.NewLine);
			richTextBox1.SelectionFont = labelFont;
			richTextBox1.SelectedText = @"Routine:";
			richTextBox1.SelectionFont = font;
			richTextBox1.AppendText(Environment.NewLine);
			richTextBox1.AppendText(lvi.SubItems[3].Text);

			checked_chb.Checked = lvi.Checked;
		}

		private void activeapp_CheckedChanged(object sender, EventArgs e) {
			ReQuery();
		}

		private void unchecked_CheckedChanged(object sender, EventArgs e) {
			ReQuery();
		}

		private void dateTimePicker1_ValueChanged(object sender, EventArgs e) {
			ReQuery();
		}

		private void dateTimePicker2_ValueChanged(object sender, EventArgs e) {
			ReQuery();
		}

		private void button1_Click(object sender, EventArgs e) {
			if (listView1.SelectedItems.Count < 1) {
				return;
			}
			ListViewItem lvi = listView1.SelectedItems[0];
			string erridstring = lvi.SubItems[6].Text;
			if (int.TryParse(erridstring, out int errid)) {
				using (ErrorDataSetTableAdapters.QueriesTableAdapter q =
					new ErrorDataSetTableAdapters.QueriesTableAdapter()) {
					try {
						q.CheckUncheck(!lvi.Checked, errid);
					} catch (Exception ex) {
						Redbrick.ProcessError(ex);
					}
				}
			}
			ReQuery();
		}

		private void ErrorLog_FormClosing(object sender, FormClosingEventArgs e) {
			Properties.Settings.Default.ErrorLogLocation = Location;
			Properties.Settings.Default.ErrorLogSize = Size;
			Properties.Settings.Default.Save();
		}
	}
}
