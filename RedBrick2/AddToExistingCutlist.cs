using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
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
			Text = _pp.PartLookup;
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
			eNGINEERINGDataSet.CUT_PARTS.UpdatePart(props_);
			eNGINEERINGDataSet.CUT_CUTLIST_PARTS.UpdateCutlistPart(props_);
			add_btn.Enabled = false;
			cancel_btn.Text = @"Close";
		}

		private void AddToExistingCutlist_FormClosing(object sender, FormClosingEventArgs e) {
			Properties.Settings.Default.AddToCutlistLocation = Location;
			Properties.Settings.Default.AddToCutlistSize = Size;
			Properties.Settings.Default.Save();
		}
	}
}