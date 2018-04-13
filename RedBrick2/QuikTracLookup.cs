using System;
using System.Data;
using System.Windows.Forms;

namespace RedBrick2 {
	/// <summary>
	/// A form to display a table of data. This is really for QuikTrac lookups.
	/// </summary>
	public partial class QuickTracLookup : Form {
		private DataView dv_;

		public QuickTracLookup(string lookup) {
			InitializeComponent();
			toolStripStatusLabel1.Text = @"Total quantity: 0";
			LookUp(lookup);
			lookup_item.Text = lookup;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public QuickTracLookup() : this(new DataView()) {
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="_dv">A <see cref="DataView"/> object to display.</param>
		public QuickTracLookup(DataView _dv) : this(_dv, string.Empty) {
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="_dv">A <see cref="DataView"/> object to display.</param>
		/// <param name="_title">A <see cref="string"/> for the titlebar.</param>
		public QuickTracLookup(DataView _dv, string _title) {
			InitializeComponent();
			toolStripStatusLabel1.Text = @"Total quantity: 0";
			dv_ = _dv;
			dataGridView1.DataSource = dv_;
			Text = _title;
			set_visibility();
		}

		private void LookUp(string lookup) {
			dataGridView1.DataSource = null;
			dataGridView1.Rows.Clear();
			textBox1.Text = string.Empty;
			if (lookup != string.Empty) {
				using (ENGINEERINGDataSetTableAdapters.CLIENT_STUFFTableAdapter ta_ =
					new ENGINEERINGDataSetTableAdapters.CLIENT_STUFFTableAdapter()) {
					DataView dv_ = ta_.GetData(lookup).DefaultView;
					dataGridView1.DataSource = dv_;
				}
				set_visibility();
			}
			using (ENGINEERINGDataSetTableAdapters.inmastdisplayTableAdapter ta_ =
				new ENGINEERINGDataSetTableAdapters.inmastdisplayTableAdapter()) {
				if (lookup_item.SelectedItem != null) {
					DataRowView drv_ = lookup_item.SelectedItem as DataRowView;
					textBox1.Text = ta_.GetFDescript(lookup, Convert.ToString(drv_[@"frev"]));
				} else {
					textBox1.Text = ta_.GetFDescript(lookup, @"100");
				}
			}
			Text = lookup != string.Empty ? string.Format(@"{0} - QuikTrac Lookup", lookup) : @"QuikTrac Lookup";
			_sum_qty();
		}

		private void set_visibility() {
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
		}

		private void DataDisplay_FormClosing(object sender, FormClosingEventArgs e) {
			Properties.Settings.Default.QTLocation = Location;
			Properties.Settings.Default.QTSize = Size;
			Properties.Settings.Default.Save();
		}

		private void lookup_item_DropDown(object sender, EventArgs e) {
			ComboBox cb_ = sender as ComboBox;
			if (cb_.Items.Count < 1) {
				Cursor = Cursors.WaitCursor;
				inmastdisplayTableAdapter.Fill(eNGINEERINGDataSet.inmastdisplay);
				Cursor = Cursors.Default;
			}
		}

		private void lookup_item_SelectedIndexChanged(object sender, EventArgs e) {
			LookUp((sender as ComboBox).SelectedValue.ToString().Trim());
		}

		private void lookup_item_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) {
			(sender as ComboBox).DroppedDown = false;
			if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab) {
				LookUp((sender as ComboBox).Text.Trim());
			}
		}
	}
}
