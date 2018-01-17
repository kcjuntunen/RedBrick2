using System;
using System.Data;
using System.Windows.Forms;

namespace RedBrick2 {
	public partial class DataDisplay : Form {
		private DataView dv_;

		public DataDisplay() : this(new DataView()) {
		}

		public DataDisplay(DataView _dv) : this(_dv, string.Empty) {
		}

		public DataDisplay(DataView _dv, string _title) {
			InitializeComponent();
			toolStripStatusLabel1.Text = @"Total quantity: 0";
			dv_ = _dv;
			dataGridView1.DataSource = dv_;
			Text = _title;
			foreach (DataGridViewColumn col_ in dataGridView1.Columns) {
				if (col_.Name == @"LOC" ||
					col_.Name == @"QTY" ||
					col_.Name == @"UofM" ||
					col_.Name == @"REV") {
					col_.Visible = true;
				} else {
					col_.Visible = false;
				}
			}
		}

		private void _sum_qty() {
			double sum_ = 0.0f;
			foreach (DataGridViewRow r_ in dataGridView1.Rows) {
				sum_ += Convert.ToDouble(r_.Cells[@"QTY"].Value);
			}
			toolStripStatusLabel1.Text = string.Format(@"Total quantity: {0}", sum_);
		}

		private void DataDisplay_Load(object sender, EventArgs e) {
			Location = Properties.Settings.Default.QTLocation;
			Size = Properties.Settings.Default.QTSize;
			_sum_qty();
		}

		private void DataDisplay_FormClosing(object sender, FormClosingEventArgs e) {
			Properties.Settings.Default.QTLocation = Location;
			Properties.Settings.Default.QTSize = Size;
			Properties.Settings.Default.Save();
		}
	}
}
