using System;
using System.Drawing;
using System.Windows.Forms;

namespace RedBrick2.Time_Entry {
	public partial class EditTimeEntry : Form {

		public int RecID { get; set; }

		public EditTimeEntry() {
			InitializeComponent();
		}

		public EditTimeEntry(int recid) {
			InitializeComponent();
			RecID = recid;

			proj_cbx.DrawMode = DrawMode.OwnerDrawFixed;
			proc_cbx.DrawMode = DrawMode.OwnerDrawFixed;
		}

		private void EditTimeEntry_Load(object sender, EventArgs e) {
			this.sCH_PROCESSTableAdapter.Fill(this.timeEntryDataSet.SCH_PROCESS);
			this.sCH_PROJECTSTableAdapter.Fill(this.timeEntryDataSet.SCH_PROJECTS);
			using (TimeEntryDataSetTableAdapters.SCH_RECORDSTableAdapter ta =
				new TimeEntryDataSetTableAdapters.SCH_RECORDSTableAdapter()) {
				ta.FillByID(timeEntryDataSet.SCH_RECORDS, RecID);
				proj_cbx.SelectedValue = timeEntryDataSet.SCH_RECORDS[0].PID;
				dateTimePicker1.Value = timeEntryDataSet.SCH_RECORDS[0].PDATE;
				proc_cbx.SelectedValue = timeEntryDataSet.SCH_RECORDS[0].PROCID;
				hrs_tb.Text = timeEntryDataSet.SCH_RECORDS[0].QTY.ToString("0.00");
			}
		}

		private void discard_btn_Click(object sender, EventArgs e) {
			Close();
		}

		private void cbx_KeyDown(object sender, KeyEventArgs e) {
			(sender as ComboBox).DroppedDown = false;
		}

		private void save_btn_Click(object sender, EventArgs e) {
			if (proj_cbx.SelectedItem == null || proc_cbx.SelectedItem == null) {
				return;
			}
			if (!double.TryParse(hrs_tb.Text, out double total_time)) {
				if (hrs_tb.Text.Contains(":")) {
					string[] time_ = hrs_tb.Text.Split(':');
					double.TryParse(time_[0], out double hrs);
					double.TryParse(time_[1], out double min);
					total_time = hrs + (min / 60);
				}
			}
			if (total_time < 1 / 60) {
				return;
			}
			timeEntryDataSet.SCH_RECORDS[0].PID = Convert.ToInt32(proj_cbx.SelectedValue);
			timeEntryDataSet.SCH_RECORDS[0].PDATE = dateTimePicker1.Value;
			timeEntryDataSet.SCH_RECORDS[0].PROCID = Convert.ToInt32(proc_cbx.SelectedValue);
			timeEntryDataSet.SCH_RECORDS[0].QTY = Convert.ToDecimal(total_time);
			using (TimeEntryDataSetTableAdapters.SCH_RECORDSTableAdapter ta =
				new TimeEntryDataSetTableAdapters.SCH_RECORDSTableAdapter()) {
				ta.Update(timeEntryDataSet.SCH_RECORDS[0]);
			}
			Close();
		}

		private void proj_cbx_DrawItem(object sender, DrawItemEventArgs e) {
			ComboBox cb_ = sender as ComboBox;
			int index = e.Index >= 0 ? e.Index : 0;
			System.Data.DataRowView drv_ = cb_.Items[index] as System.Data.DataRowView;
			e.DrawBackground();
			string item = string.Format("{0} - {1}",
				drv_["PROJECT"].ToString(), Redbrick.TitleCase(drv_["DESCRIPTION"].ToString()));
			e.Graphics.DrawString(item, e.Font, SystemBrushes.ControlText, e.Bounds, StringFormat.GenericDefault);
			e.DrawFocusRectangle();
		}

		private void proc_cbx_DrawItem(object sender, DrawItemEventArgs e) {
			ComboBox cb_ = sender as ComboBox;
			int index = e.Index >= 0 ? e.Index : 0;
			System.Data.DataRowView drv_ = cb_.Items[index] as System.Data.DataRowView;
			e.DrawBackground();

			string str = drv_["PROCESS"].ToString();
			str = Redbrick.TitleCase(str).Replace("Cad", "CAD").Replace("Cnc", "CNC").Replace("M2m", "M2M");

			e.Graphics.DrawString(str, e.Font, SystemBrushes.ControlText, e.Bounds, StringFormat.GenericDefault);
			e.DrawFocusRectangle();
		}
	}
}
