using System;
using System.Drawing;
using System.Windows.Forms;

namespace RedBrick2 {
	/// <summary>
	/// A form for renaming cutlists.
	/// </summary>
	public partial class RenameCutlist : Form {
		int pre_selected_clid = 0;
		int pre_selected_cust = 0;
		string drw_ref = string.Empty;
		string selected_rev = string.Empty;

		ToolTip descr_tt = new ToolTip();

		/// <summary>
		/// Constructor. Instantiate the RenameCutlist form.
		/// </summary>
		public RenameCutlist() {
			Init();
			PrePopulate();
		}

		/// <summary>
		/// Constructor. Instantiate the RenameCutlist form with <see cref="int"/> selected.
		/// </summary>
		public RenameCutlist(int clid) {
			pre_selected_clid = clid;
			Init();
			PrePopulate();
		}

		/// <summary>
		/// Constructor. Instantiate the RenameCutlist form with <see cref="int"/> selected.
		/// </summary>
		public RenameCutlist(int clid, int cust, string drw) {
			pre_selected_clid = clid;
			pre_selected_cust = cust;
			drw_ref = drw;
			Init();
			PrePopulate();
		}

		/// <summary>
		/// Constructor. Instantiate the RenameCutlist form with <see cref="int"/> selected.
		/// </summary>
		public RenameCutlist(ENGINEERINGDataSet.CUT_CUTLISTSRow _r) {
			pre_selected_clid = _r.CLID;
			pre_selected_cust = _r.CUSTID;
			drw_ref = _r.DRAWING;
			Init();
			Text = string.Format(@"Rename {0}...", _r.DESCR);
			from_cbx.SelectedValue = _r.CLID;
			from_cbx.Text = _r.PARTNUM;
			rev_cbx.Text = _r.REV;
			selected_rev = _r.REV;
			drw_tb.Text = _r.DRAWING;
			cust_cbx.SelectedValue = _r.CUSTID;
		}

		private void Init() {
			InitializeComponent();
			gEN_CUSTOMERSTableAdapter.Fill(eNGINEERINGDataSet.GEN_CUSTOMERS);
			revListTableAdapter.Fill(eNGINEERINGDataSet.RevList);
			
			rename_button.Enabled = names_OK;
			from_cbx.DrawMode = DrawMode.OwnerDrawFixed;
			cust_cbx.DrawMode = DrawMode.OwnerDrawFixed;
		}

		private void PrePopulate() {
			if (from_cbx.Items.Count < 1) {
				Cursor = Cursors.WaitCursor;
				cUT_CUTLISTSTableAdapter.Fill(eNGINEERINGDataSet.CUT_CUTLISTS);
				Cursor = Cursors.Default;
			}
			if (pre_selected_clid > 0) {
				from_cbx.SelectedValue = pre_selected_clid;
			}
			rev_cbx.SelectedValue = @"100";

			if (from_cbx.SelectedItem != null) {
				System.Data.DataRowView drv_ = from_cbx.SelectedItem as System.Data.DataRowView;
				selected_rev = Convert.ToString(drv_[@"REV"]);
			}

			if (pre_selected_cust > 0) {
				cust_cbx.SelectedValue = pre_selected_cust;
			}

			drw_tb.Text = drw_ref;
		}

		private bool CheckCutlistExists(string cl_, string rev_) {
			using (ENGINEERINGDataSet.CUT_CUTLISTSDataTable cdt_ =
				cUT_CUTLISTSTableAdapter.GetDataByName(cl_, rev_)) {
				if (cdt_.Count > 0) {
					return true;
				}
			}
			return false;
		}

		private bool DoRename() {
			int cl_ = pre_selected_clid;
			if (from_cbx.SelectedItem != null) {
				cl_ = Convert.ToInt32(from_cbx.SelectedValue);
			}
			using (ENGINEERINGDataSet.CUT_CUTLISTSDataTable dt_ = new ENGINEERINGDataSet.CUT_CUTLISTSDataTable()) {
				return dt_.Rename(cl_,
					to_tbx.Text.Trim(),
					rev_cbx.Text.Trim(),
					drw_tb.Text.Trim(),
					Convert.ToInt32(cust_cbx.SelectedValue)) == 1;
			}
		}

		private void RenameCutlist_Load(object sender, EventArgs e) {
			Location = Properties.Settings.Default.RenameLocation;
			Size = Properties.Settings.Default.RenameSize;
		}

		private void close_btn_Click(object sender, EventArgs e) {
			Close();
		}

		private void tbx_TextChanged(object sender, EventArgs e) {
			rename_button.Enabled = names_OK;
		}

		private void rename_button_Click(object sender, EventArgs e) {
			string from_ = from_cbx.Text;
			string to_item_ = to_tbx.Text.Trim();
			string to_rev_ = rev_cbx.Text.Trim();
			string query_ = string.Format(@"Really rename `{0} REV {1}' to `{2} REV {3}'?", from_, selected_rev, to_item_, to_rev_);
			if (MessageBox.Show(this, query_, @"﴾͡๏̯͡๏﴿ RLY?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) {
				return;
			}

			if (from_cbx.SelectedItem == null && pre_selected_clid < 1) {
				string err_msg_ = string.Format(@"You didn't specify which one to rename!", to_item_, to_rev_);
				MessageBox.Show(this, err_msg_, @"But you can't! ಠ_ಠ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			if (CheckCutlistExists(to_item_, to_rev_)) {
				string err_msg_ = string.Format(@"A cutlist called `{0} REV {1}' already exists.", to_item_, to_rev_);
				MessageBox.Show(this, err_msg_, @"But you can't! ಠ_ಠ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			if (!DoRename()) {
				string err_msg_ = @"Something went wrong.";
				MessageBox.Show(this, err_msg_, @"Aww, man!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}

			rename_button.Enabled = false;
			close_btn.Text = @"Close";
			close_btn.BackColor = Color.Green;
		}

		private bool names_OK {
			get
			{ return (to_tbx.Text.Trim() != string.Empty) &&
					(rev_cbx.Text.Trim() != string.Empty) &&
					(drw_tb.Text.Trim() != string.Empty) &&
					(cust_cbx.SelectedItem != null); }
			set {; }
		}

		private void cbx_TextChanged(object sender, EventArgs e) {
			rename_button.Enabled = names_OK;
		}

		private void from_cbx_DrawItem(object sender, DrawItemEventArgs e) {
			ComboBox cb_ = sender as ComboBox;
			int index = e.Index >= 0 ? e.Index : 0;
			System.Data.DataRowView drv_ = cb_.Items[index] as System.Data.DataRowView;
			string display_string_ = string.Format(@"{0} REV {1}",
				Convert.ToString(drv_[@"PARTNUM"]),
				Convert.ToString(drv_[@"REV"]));
			e.Graphics.DrawString(display_string_, e.Font, SystemBrushes.ControlText, e.Bounds, StringFormat.GenericDefault);
		}

		private void cust_cbx_DrawItem(object sender, DrawItemEventArgs e) {
			ComboBox cb_ = sender as ComboBox;
			int index = e.Index >= 0 ? e.Index : 0;
			System.Data.DataRowView drv_ = cb_.Items[index] as System.Data.DataRowView;
			string custnum_ = Convert.ToString(drv_[@"CUSTNUM"]);
			string cust_ = Redbrick.TitleCase(Convert.ToString(drv_[@"CUSTOMER"]));
			if (custnum_ != string.Empty) {
				e.Graphics.DrawString(string.Format(@"{0} - {1}", cust_, custnum_),
					e.Font, SystemBrushes.ControlText, e.Bounds, StringFormat.GenericDefault);
			} else {
				e.Graphics.DrawString(cust_, e.Font, SystemBrushes.ControlText, e.Bounds, StringFormat.GenericDefault);
			}
		}

		private void from_cbx_SelectedIndexChanged(object sender, EventArgs e) {
			descr_tt.RemoveAll();
			ComboBox cb_ = sender as ComboBox;
			if (cb_.SelectedItem == null) {
				return;
			}
			System.Data.DataRowView drv_ = cb_.SelectedItem as System.Data.DataRowView;
			string username_ = @"¯\_(ツ)_/¯";
			using (ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter ta_ = new ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter()) {
				ta_.FillByUID(eNGINEERINGDataSet.GEN_USERS, Convert.ToInt32(drv_[@"SETUP_BY"]));
				if (eNGINEERINGDataSet.GEN_USERS.Count > 0) {
					ENGINEERINGDataSet.GEN_USERSRow r_ = eNGINEERINGDataSet.GEN_USERS[0];
					username_ = r_.IsFullnameNull() ?
						@"no one in particular" :
						Redbrick.TitleCase(Convert.ToString(eNGINEERINGDataSet.GEN_USERS[0].Fullname));
				}
			}
			DateTime.TryParse(Convert.ToString(drv_[@"CDATE"]), out DateTime dt_);
			string tt_ = string.Format(@"`{0}', created by {1} on {2}.",
				Convert.ToString(drv_[@"DESCR"]), username_, dt_.ToShortDateString());
			selected_rev = Convert.ToString(drv_[@"REV"]);
			Text = string.Format(@"Rename {0}...", Convert.ToString(drv_[@"DESCR"]));
			descr_tt.SetToolTip(cb_, tt_);
		}

		private void cbx_KeyDown(object sender, KeyEventArgs e) {
			(sender as ComboBox).DroppedDown = false;
		}

		private void RenameCutlist_FormClosing(object sender, FormClosingEventArgs e) {
			Properties.Settings.Default.RenameLocation = Location;
			Properties.Settings.Default.RenameSize = Size;
			Properties.Settings.Default.Save();
		}

		private void from_cbx_DropDown(object sender, EventArgs e) {
			ComboBox cb_ = sender as ComboBox;
			if (cb_.Items.Count < 1) {
				Cursor = Cursors.WaitCursor;
				cUT_CUTLISTSTableAdapter.Fill(eNGINEERINGDataSet.CUT_CUTLISTS);
				Cursor = Cursors.Default;
			}
		}
	}
}
