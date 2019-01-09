using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace RedBrick2.Time_Entry {
	public partial class TimeEntry : Form {
		private List<ListViewItem> ListViewItems { get; set; } = new List<ListViewItem>();
		private DateTime LastEnteredDate { get; set; } = DateTime.Now;

		public TimeEntry() {
			InitializeComponent();
			Setup();
		}

		private void Setup() {
			listView1.FullRowSelect = true;
			listView1.HideSelection = false;
			listView1.MultiSelect = false;
			listView1.View = View.Details;
			listView1.SmallImageList = Redbrick.TreeViewIcons;

			uid_cbx.DrawMode = DrawMode.OwnerDrawFixed;
			cust_cbx.DrawMode = DrawMode.OwnerDrawFixed;
			proj_cbx.DrawMode = DrawMode.OwnerDrawFixed;
			proc_cbx.DrawMode = DrawMode.OwnerDrawFixed;
			add_proj_cbx.DrawMode = DrawMode.OwnerDrawFixed;
			add_proc_cbx.DrawMode = DrawMode.OwnerDrawFixed;

			dateTimePicker1.Value = DateTime.Now.AddMonths(-3);
			dateTimePicker2.Value = DateTime.Now.AddDays(1);
		}

		private void TimeEntry_Load(object sender, EventArgs e) {
			Location = Properties.Settings.Default.TimeEntryLocation;
			Size = Properties.Settings.Default.TimeEntrySize;
			this.sCH_PROCESSTableAdapter.Fill(this.timeEntryDataSet.SCH_PROCESS);
			this.sCH_PROJECTSTableAdapter.Fill(this.timeEntryDataSet.SCH_PROJECTS);
			this.gEN_CUSTOMERSTableAdapter.Fill(this.timeEntryDataSet.GEN_CUSTOMERS);
			this.gEN_USERSTableAdapter.Fill(this.timeEntryDataSet.GEN_USERS, 6);

			topCustomersTableAdapter.Fill(timeEntryDataSet.TopCustomers, Convert.ToInt32(uid_cbx.SelectedValue));

			uid_cbx.SelectedValue = Redbrick.UID;
			uid_cbx.Enabled = !Redbrick.IsSuperAdmin();
			cust_cbx.SelectedIndex = -1;
			proj_cbx.SelectedIndex = -1;
			proc_cbx.SelectedIndex = -1;
			Clear();

			PopulateList();
			Filter();
			PopulateTop();
		}

		private void Clear() {
			add_proj_cbx.SelectedIndex = -1;
			add_proc_cbx.SelectedIndex = -1;
			descr_tb.Text = string.Empty;
			cust_tb.Text = string.Empty;
			hrs_tbx.Text = string.Empty;
		}

		private void PopulateList() {
			ListViewItems.Clear();
			using (TimeEntryDataSetTableAdapters.ScheduleListView s =
				new TimeEntryDataSetTableAdapters.ScheduleListView()) {
				s.FillByUID(timeEntryDataSet.ScheduleListView, Convert.ToInt32(uid_cbx.SelectedValue),
					dateTimePicker1.Value, dateTimePicker2.Value);
				try {
					foreach (TimeEntryDataSet.ScheduleListViewRow row in timeEntryDataSet.ScheduleListView.Rows) {
						int qty = Convert.ToInt32(row.QTY * 60);
						string time = string.Format("{0}:{1:D2}", (qty / 60), (qty % 60));
						string[] data = new string[] {
						row.DATE.ToString("yyyy-MM-dd"),
						row.PROJECT,
						Redbrick.TitleCase(row.DESCRIPTION),
						Redbrick.TitleCase(row.PROCESS).Replace("Cad", "CAD").Replace("Cnc", "CNC").Replace("M2m", "M2M"),
						time,
						row.CUSTID.ToString(),
						row.RECID.ToString(),
						row.QTY.ToString()
					};
						ListViewItems.Add(new ListViewItem(data));
					}
				} catch (Exception ex) {
					Redbrick.ProcessError(ex);
				}
			}
		}

		private void Filter() {
			listView1.Items.Clear();
			double total = 0.0;
			foreach (ListViewItem lvi in ListViewItems) {
				if (cust_cbx.SelectedIndex != -1 &&
					lvi.SubItems[5].Text.Trim() != Convert.ToString(cust_cbx.SelectedValue).Trim()) {
					continue;
				}
				if (proj_cbx.SelectedIndex != -1 &&
					lvi.SubItems[1].Text.Trim() != Convert.ToString(proj_cbx.SelectedValue).Trim()) {
					continue;
				}
				if (proc_cbx.SelectedIndex != -1 &&
					lvi.SubItems[3].Text.Trim().ToUpper() != Convert.ToString(proc_cbx.SelectedValue).Trim().ToUpper()) {
					continue;
				}
				if (double.TryParse(lvi.SubItems[7].Text, out double test)) {
					total += test;
				}
				listView1.Items.Add(lvi);
			}
			total_label.Text = string.Format("{0:0.0} hours total", total);
			listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
		}

		private void PopulateTop() {
			chart1.Series.Clear();
			topCustomersTableAdapter.Fill(timeEntryDataSet.TopCustomers, Convert.ToInt32(uid_cbx.SelectedValue));

			foreach (TimeEntryDataSet.TopCustomersRow row in timeEntryDataSet.TopCustomers) {
				Series series = chart1.Series.Add(Redbrick.TitleCase(row.CUSTOMER));
				series.AxisLabel = "Customers";
				series.Points.Add(Convert.ToDouble(row.SumOfQTY));
			}
			chart1.ChartAreas[0].AxisX.Minimum = 0.1;
		}

		private void uid_cbx_SelectedIndexChanged(object sender, EventArgs e) {
			PopulateList();
			Filter();
			PopulateTop();
		}

		private void cust_cbx_SelectedIndexChanged(object sender, EventArgs e) {
			ComboBox c = sender as ComboBox;
			if (c.SelectedItem != null) {
				sCHPROJECTSBindingSource.Filter =
					string.Format("CUSTID = {0}", (sender as ComboBox).SelectedValue);
			} else {
				sCHPROJECTSBindingSource.Filter = string.Empty;
			}
			Filter();
		}

		private void proj_cbx_SelectedIndexChanged(object sender, EventArgs e) {
			Filter();
		}

		private void proc_cbx_SelectedIndexChanged(object sender, EventArgs e) {
			Filter();
		}

		private void dateTimePicker1_ValueChanged(object sender, EventArgs e) {
			PopulateList();
			Filter();
		}

		private void dateTimePicker2_ValueChanged(object sender, EventArgs e) {
			PopulateList();
			Filter();
		}

		private void cbx_TextUpdate(object sender, EventArgs e) {
			if ((sender as ComboBox).Text.Trim() == string.Empty) {
				(sender as ComboBox).SelectedIndex = -1;
			}
		}

		private void dateTimePicker3_ValueChanged(object sender, EventArgs e) {
			LastEnteredDate = (sender as DateTimePicker).Value;
		}

		private void uid_cbx_DrawItem(object sender, DrawItemEventArgs e) {
			ComboBox cb_ = sender as ComboBox;
			int index = e.Index >= 0 ? e.Index : 0;
			System.Data.DataRowView drv_ = cb_.Items[index] as System.Data.DataRowView;
			e.DrawBackground();
			e.Graphics.DrawString(Redbrick.TitleCase(drv_["Fullname"].ToString()), e.Font, SystemBrushes.ControlText,
				e.Bounds, StringFormat.GenericDefault);
			e.DrawFocusRectangle();
		}

		private void cust_cbx_DrawItem(object sender, DrawItemEventArgs e) {
			ComboBox cb_ = sender as ComboBox;
			int index = e.Index >= 0 ? e.Index : 0;
			System.Data.DataRowView drv_ = cb_.Items[index] as System.Data.DataRowView;
			e.DrawBackground();
			string item = string.Format("{0} - {1}",
				Redbrick.TitleCase(drv_["CUSTOMER"].ToString()), drv_["CUSTNUM"].ToString());
			e.Graphics.DrawString(item, e.Font, SystemBrushes.ControlText, e.Bounds, StringFormat.GenericDefault);
			e.DrawFocusRectangle();
		}

		private void proj_cbx_DrawItem(object sender, DrawItemEventArgs e) {
			ComboBox cb_ = sender as ComboBox;
			int index = e.Index >= 0 ? e.Index : 0;
			DataRowView drv_ = cb_.Items[index] as DataRowView;
			e.DrawBackground();
			string item = string.Format("{0} - {1}",
				drv_["PROJECT"].ToString(), Redbrick.TitleCase(drv_["DESCRIPTION"].ToString()));
			e.Graphics.DrawString(item, e.Font, SystemBrushes.ControlText, e.Bounds, StringFormat.GenericDefault);
			e.DrawFocusRectangle();
		}

		private void add_proc_cbx_DrawItem(object sender, DrawItemEventArgs e) {
			ComboBox cb_ = sender as ComboBox;
			int index = e.Index >= 0 ? e.Index : 0;
			DataRowView drv_ = cb_.Items[index] as DataRowView;
			e.DrawBackground();

			string str = drv_["PROCESS"].ToString();
			str = Redbrick.TitleCase(str).Replace("Cad", "CAD").Replace("Cnc", "CNC").Replace("M2m", "M2M");

			e.Graphics.DrawString(str, e.Font, SystemBrushes.ControlText, e.Bounds, StringFormat.GenericDefault);
			e.DrawFocusRectangle();
		}

		private void add_btn_Click(object sender, EventArgs e) {
			if (add_proj_cbx.SelectedItem == null || add_proc_cbx.SelectedItem == null) {
				return;
			}
			double total_time = 0.0;
			if (hrs_tbx.Text.Contains(":")) {
				string[] time_ = hrs_tbx.Text.Split(':');
				double.TryParse(time_[0], out double hrs);
				double.TryParse(time_[1], out double min);
				total_time = hrs + (min / 60);
			}
			if (total_time < 1 / 60) {
				return;
			}
			using (TimeEntryDataSetTableAdapters.SCH_RECORDSTableAdapter ta =
				new TimeEntryDataSetTableAdapters.SCH_RECORDSTableAdapter()) {
				try {
					ta.Insert(Convert.ToDecimal(total_time),
						dateTimePicker3.Value, Convert.ToInt32(add_proj_cbx.SelectedValue),
						Convert.ToInt32(add_proc_cbx.SelectedValue),
						Redbrick.UID);
				} catch (Exception ex) {
					Redbrick.ProcessError(ex);
				}
				Clear();
				PopulateList();
				Filter();
			}
		}

		private void clear_btn_Click(object sender, EventArgs e) {
			Clear();
		}

		private void listView1_MouseClick(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Right) {
				if ((sender as ListView).FocusedItem.Bounds.Contains(e.Location)) {
					contextMenuStrip1.Show(System.Windows.Forms.Cursor.Position);
				}
			}
		}

		private void editToolStripMenuItem_Click(object sender, EventArgs e) {
			edit();
		}

		private void edit() {
			if (listView1.SelectedItems.Count < 1) {
				return;
			}
			string recid = string.Empty;
			if (!int.TryParse(listView1.SelectedItems[0].SubItems[6].Text, out int test_)) {
				return;
			}
			if (test_ < 1) {
				return;
			}
			using (EditTimeEntry ete = new EditTimeEntry(test_)) {
				ete.ShowDialog(this);
			}
			PopulateList();
			Filter();
		}

		private void deleteToolStripMenuItem_Click(object sender, EventArgs e) {
			if (listView1.SelectedItems.Count < 1) {
				return;
			}
			string recid = string.Empty;
			if (!int.TryParse(listView1.SelectedItems[0].SubItems[6].Text, out int test_)) {
				return;
			}
			if (test_ < 1) {
				return;
			}
			using (TimeEntryDataSetTableAdapters.SCH_RECORDSTableAdapter ta =
				new TimeEntryDataSetTableAdapters.SCH_RECORDSTableAdapter()) {
				try {
					ta.DeleteByRecID(test_);
				} catch (Exception ex) {
					Redbrick.ProcessError(ex);
				}
			}
		}

		private void cbx_KeyDown(object sender, KeyEventArgs e) {
			(sender as ComboBox).DroppedDown = false;
		}

		private void listView1_MouseDoubleClick(object sender, MouseEventArgs e) {
			edit();
		}

		private void TimeEntry_FormClosing(object sender, FormClosingEventArgs e) {
			Properties.Settings.Default.TimeEntryLocation = Location;
			Properties.Settings.Default.TimeEntrySize = Size;
			Properties.Settings.Default.Save();
		}
	}
}
