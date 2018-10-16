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
		}

		/// <summary>
		/// Constructor. Instantiate the RenameCutlist form with <see cref="int"/> selected.
		/// </summary>
		public RenameCutlist(int clid) {
			pre_selected_clid = clid;
			Init();
		}

		/// <summary>
		/// Constructor. Instantiate the RenameCutlist form with <see cref="int"/> selected.
		/// </summary>
		public RenameCutlist(int clid, int cust, string drw) {
			pre_selected_clid = clid;
			pre_selected_cust = cust;
			drw_ref = drw;
			Init();
		}

		private void Init() {
			InitializeComponent();
			
			rename_button.Enabled = names_OK;
			from_cbx.DrawMode = DrawMode.OwnerDrawFixed;
			cust_cbx.DrawMode = DrawMode.OwnerDrawFixed;
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
			using (ENGINEERINGDataSet.CUT_CUTLISTSDataTable dt_ = new ENGINEERINGDataSet.CUT_CUTLISTSDataTable()) {
				return dt_.Rename(Convert.ToInt32(from_cbx.SelectedValue),
					to_tbx.Text.Trim(),
					rev_cbx.Text.Trim(),
					drw_tb.Text.Trim(),
					Convert.ToInt32(cust_cbx.SelectedValue)) == 1;
			}
		}

		private void RenameCutlist_Load(object sender, EventArgs e) {
			// TODO: This line of code loads data into the 'eNGINEERINGDataSet.GEN_CUSTOMERS' table. You can move, or remove it, as needed.
			this.gEN_CUSTOMERSTableAdapter.Fill(this.eNGINEERINGDataSet.GEN_CUSTOMERS);
			Location = Properties.Settings.Default.RenameLocation;
			Size = Properties.Settings.Default.RenameSize;
			cUT_CUTLISTSTableAdapter.Fill(eNGINEERINGDataSet.CUT_CUTLISTS);
			revListTableAdapter.Fill(eNGINEERINGDataSet.RevList);
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

			if (from_cbx.SelectedItem == null) {
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
	}
}
