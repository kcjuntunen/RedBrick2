﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace RedBrick2 {
	/// <summary>
	/// A form for adding a selected part to an existing Cutlist.
	/// </summary>
	public partial class AddToExistingCutlist : Form {
		private SwProperties props_;
		private List<int> alreadyAddedTo = new List<int>();

		/// <summary>
		/// Constructor.
		/// </summary>
		public AddToExistingCutlist() {
			InitializeComponent();
			cutlist_cbx.DrawMode = DrawMode.OwnerDrawFixed;
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
				alreadyAddedTo.Add(cutlist_cbx.SelectedIndex);
				add_btn.Enabled = false;
				cancel_btn.Text = @"Close";
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
				add_btn.Enabled = true;
			}
		}

		private void cutlist_cbx_KeyDown(object sender, KeyEventArgs e) {
			(sender as ComboBox).DroppedDown = false;
		}

		private void cutlist_cbx_DrawItem(object sender, DrawItemEventArgs e) {
			ComboBox cb_ = sender as ComboBox;
			int index = e.Index >= 0 ? e.Index : 0;
			DataRowView drv_ = cb_.Items[index] as DataRowView;

			if (alreadyAddedTo.Contains(e.Index)) {
				e.DrawBackground();
				Font x = new Font(e.Font, FontStyle.Italic);
				e.Graphics.DrawString(drv_[@"CutlistDisplayName"].ToString(), x, SystemBrushes.GrayText,
					e.Bounds, StringFormat.GenericDefault);
				e.DrawFocusRectangle();
			} else {
				e.DrawBackground();
				e.Graphics.DrawString(drv_[@"CutlistDisplayName"].ToString(), e.Font, SystemBrushes.ControlText,
					e.Bounds, StringFormat.GenericDefault);
				e.DrawFocusRectangle();
			}
		}
	}
}