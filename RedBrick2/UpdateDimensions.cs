using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedBrick2 {
	/// <summary>
	/// A warning to inform users that dimensions on the part and the database do not match.
	/// </summary>
	public partial class UpdateDimensions : Form {
		private SwProperties PropertySet;
		private double db_length = 0.0f;
		private double db_width = 0.0f;
		private double db_thickness = 0.0f;
		private double length = 0.0f;
		private double width = 0.0f;
		private double thickness = 0.0f;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="pp">A <see cref="SwProperties"/> object with dimension information from the part.</param>
		/// <param name="db_l">Length from the db.</param>
		/// <param name="db_w">Width from the db.</param>
		/// <param name="db_t">Thickness from the db.</param>
		public UpdateDimensions(SwProperties pp, double db_l, double db_w, double db_t) {
			PropertySet = pp;
			db_length = db_l;
			db_width = db_w;
			db_thickness = db_t;
			length = Convert.ToDouble(pp[@"LENGTH"].Data);
			width = Convert.ToDouble(pp[@"WIDTH"].Data);
			thickness = Convert.ToDouble(pp[@"THICKNESS"].Data);

			InitializeComponent();
			Location = Properties.Settings.Default.UpdateDimensionsLocation;
			Size = Properties.Settings.Default.UpdateDimensionsSize;

			label6.MaximumSize = new Size(panel1.Width - 10, panel1.Height);
			label6.Text = Properties.Resources.WannaUpdateDimensions;

			db_length_label.Text = Redbrick.enforce_number_format(db_length);
			db_width_label.Text = Redbrick.enforce_number_format(db_width);
			db_thickness_label.Text = Redbrick.enforce_number_format(db_thickness);

			length_label.Text = Redbrick.enforce_number_format(length);
			width_label.Text = Redbrick.enforce_number_format(width);
			thickness_label.Text = Redbrick.enforce_number_format(thickness);

			compare();
		}

		private void compare() {
			if (!Redbrick.FloatEquals(db_length, length)) {
				length_label.BackColor = Properties.Settings.Default.WarnBackground;
				length_label.ForeColor = Properties.Settings.Default.WarnForeground;
			}
			if (!Redbrick.FloatEquals(db_width, width)) {
				width_label.BackColor = Properties.Settings.Default.WarnBackground;
				width_label.ForeColor = Properties.Settings.Default.WarnForeground;
			}
			if (!Redbrick.FloatEquals(db_thickness, thickness)) {
				thickness_label.BackColor = Properties.Settings.Default.WarnBackground;
				thickness_label.ForeColor = Properties.Settings.Default.WarnForeground;
			}
		}

		private void button1_Click(object sender, EventArgs e) {
			using (ENGINEERINGDataSet.CUT_PARTSDataTable cp_ = new ENGINEERINGDataSet.CUT_PARTSDataTable()) {
				cp_.update_general_properties_(PropertySet);
			}
			Properties.Settings.Default.UpdateDimensionsLocation = Location;
			Properties.Settings.Default.UpdateDimensionsSize = Size;
			Close();
		}

		private void button2_Click(object sender, EventArgs e) {
			Properties.Settings.Default.UpdateDimensionsLocation = Location;
			Properties.Settings.Default.UpdateDimensionsSize = Size;
			Close();
		}

		private void UpdateDimensions_FormClosing(object sender, FormClosingEventArgs e) {
			Properties.Settings.Default.Save();
		}

		private void panel1_SizeChanged(object sender, EventArgs e) {
			label6.MaximumSize = new Size(panel1.Width - 10, panel1.Height);
		}
	}
}
