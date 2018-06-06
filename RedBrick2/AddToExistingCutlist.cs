using System;
using System.Data;
using System.Windows.Forms;

namespace RedBrick2 {
	/// <summary>
	/// A form for adding a selected part to an existing Cutlist.
	/// </summary>
	public partial class AddToExistingCutlist : Form {
		private SwProperties props_;

		/// <summary>
		/// Constructor.
		/// </summary>
		public AddToExistingCutlist() {
			InitializeComponent();
			Location = Properties.Settings.Default.AddToCutlistLocation;
			Size = Properties.Settings.Default.AddToCutlistSize;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="_pp">An SwProperties object.</param>
		public AddToExistingCutlist(SwProperties _pp) : this() {
			props_ = _pp;
			Text = string.Format(@"Adding {0}...", _pp.PartLookup);
		}

		private void cancel_btn_Click(object sender, EventArgs e) {
			Properties.Settings.Default.AddToCutlistSize = Size;
			Properties.Settings.Default.AddToCutlistLocation = Location;
			Properties.Settings.Default.Save();
			Close();
		}

		private void AddToExistingCutlist_Load(object sender, EventArgs e) {
			Location = Properties.Settings.Default.AddToCutlistLocation;
			Size = Properties.Settings.Default.AddToCutlistSize;
			this.cutlistsTableAdapter.Fill(this.eNGINEERINGDataSet.Cutlists);
			this.revListTableAdapter.Fill(this.eNGINEERINGDataSet.RevList);
			cutlist_cbx.SelectedValue = Properties.Settings.Default.LastCutlist;
		}

		private void add_btn_Click(object sender, EventArgs e) {
			if (cutlist_cbx.SelectedItem != null) {
				props_.CutlistID = Convert.ToInt32((cutlist_cbx.SelectedItem as DataRowView)[@"CLID"]);
				props_.CutlistQty = (float)Convert.ToDouble(partq_nud.Value);
				eNGINEERINGDataSet.CUT_PARTS.UpdatePart(props_);
				eNGINEERINGDataSet.CUT_CUTLIST_PARTS.UpdateCutlistPart(props_);
				add_btn.Enabled = false;
				cancel_btn.Text = @"Close";
				cancel_btn.BackColor = System.Drawing.Color.Green;
			}
		}

		private void AddToExistingCutlist_FormClosing(object sender, FormClosingEventArgs e) {
			Properties.Settings.Default.AddToCutlistLocation = Location;
			Properties.Settings.Default.AddToCutlistSize = Size;
			Properties.Settings.Default.Save();
		}

		private void cutlist_cbx_SelectedIndexChanged(object sender, EventArgs e) {
			ComboBox c_ = sender as ComboBox;
			if (c_.SelectedItem != null) {
				DataRowView d_ = c_.SelectedItem as DataRowView;
				Text = string.Format(@"Adding {0} to {1}", props_.PartLookup, d_[@"CutlistDisplayName"]);
			}
		}

		private void cutlist_cbx_KeyDown(object sender, KeyEventArgs e) {
			(sender as ComboBox).DroppedDown = false;
		}
	}
}