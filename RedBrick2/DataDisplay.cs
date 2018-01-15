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
			dv_ = _dv;
			dataGridView1.DataSource = dv_;
			Text = _title;
			foreach (DataGridViewColumn col_ in dataGridView1.Columns) {
				if (col_.Name == @"LOC" ||
					col_.Name == @"QTY" ||
					col_.Name == @"UofM") {
					col_.Visible = true;
				} else {
					col_.Visible = false;
				}
			}
		}

	}
}
