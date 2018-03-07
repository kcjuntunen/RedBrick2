using System;
using System.Data;
using System.Windows.Forms;

namespace RedBrick2 {
	/// <summary>
	/// A form to display a table of data. This is really for QuikTrac lookups.
	/// </summary>
	public partial class DataDisplay : Form {
		private DataView dv_;

		/// <summary>
		/// Constructor.
		/// </summary>
		public DataDisplay() : this(new DataView()) {
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="_dv">A <see cref="DataView"/> object to display.</param>
		public DataDisplay(DataView _dv) : this(_dv, string.Empty) {
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="_dv">A <see cref="DataView"/> object to display.</param>
		/// <param name="_title">A <see cref="string"/> for the titlebar.</param>
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
