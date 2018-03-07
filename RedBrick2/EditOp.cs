using System;
using System.Data;
using System.Windows.Forms;

namespace RedBrick2 {
	/// <summary>
	/// A window for editing routing data.
	/// </summary>
	public partial class EditOp : Form {
		private ENGINEERINGDataSet.CutPartOpsRow currentRow;
		private SwProperties PropertySet;
		private int PartID;
		private bool NewOp = false;
		private bool user_edit = false;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="props">The property set from a given part.</param>
		public EditOp(SwProperties props) {
			InitializeComponent();
			PropertySet = props;
			PartID = PropertySet[@"DEPARTMENT"].PartID;
			NewOp = true;

			using (ENGINEERINGDataSet.CutPartOpsDataTable dt = new ENGINEERINGDataSet.CutPartOpsDataTable()) {
				int _next = 1;
				currentRow = (ENGINEERINGDataSet.CutPartOpsRow)dt.NewRow();
				currentRow.POPPART = props.PartsData.PARTID;

				if (PartID > 0) {
					try {
						using (ENGINEERINGDataSetTableAdapters.CutPartOpsTableAdapter cpota =
							new ENGINEERINGDataSetTableAdapters.CutPartOpsTableAdapter()) {
							_next = (int)cpota.GetNextInOrder(PartID);
						}
					} catch (Exception) {
						// We found no rows.
					}
					currentRow.POPORDER = _next;
					currentRow.POPPART = props.PartsData.PARTID;
				} else {
					currentRow.POPORDER = 1;
				}
				currentRow.TYPEID = (int)PropertySet[@"DEPARTMENT"].Data;
			}
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="row">A row of routing data.</param>
		public EditOp(ENGINEERINGDataSet.CutPartOpsRow row) {
			InitializeComponent();
			currentRow = row;
			PartID = currentRow.POPPART;
		}

		private void EditOp_Load(object sender, EventArgs e) {
			Location = Properties.Settings.Default.EditWindowLocation;
			Size = Properties.Settings.Default.EditOpSize;
			// TODO: This line of code loads data into the 'eNGINEERINGDataSet.FriendlyCutOps' table. You can move, or remove it, as needed.
			this.friendlyCutOpsTableAdapter.Fill(this.eNGINEERINGDataSet.FriendlyCutOps);
			// TODO: This line of code loads data into the 'eNGINEERINGDataSet.CUT_PART_TYPES' table. You can move, or remove it, as needed.
			this.cUT_PART_TYPESTableAdapter.Fill(this.eNGINEERINGDataSet.CUT_PART_TYPES);

			Text = string.Format(@"Edit Operation {0}", currentRow.POPORDER);
			comboBox1.SelectedValue = currentRow.TYPEID;
			comboBox1.Enabled = currentRow.POPORDER == 1 && currentRow.Table.Rows.Count < 1;
			friendlyCutOpsBindingSource.Filter = string.Format(@"TYPEID = {0}", currentRow.TYPEID);

			if (NewOp) {
				comboBox2.SelectedIndex = 0;
				//DataRowView cb2 = (comboBox2.SelectedItem as DataRowView);
				currentRow.POPOP = (int)comboBox2.SelectedValue;
				//currentRow.POPOP = (int)cb2[@"OPID"];
				textBox1.Text = string.Format(Properties.Settings.Default.NumberFormat, 0.0F);
				textBox2.Text = string.Format(Properties.Settings.Default.NumberFormat, 0.0F);
			} else {
				comboBox2.SelectedValue = currentRow.POPOP;
				textBox1.Text = string.Format(Properties.Settings.Default.NumberFormat, currentRow.POPSETUP * 60);
				textBox2.Text = string.Format(Properties.Settings.Default.NumberFormat, currentRow.POPRUN * 60);
			}
			Unselect(comboBox2);
		}

		private void button2_Click(object sender, EventArgs e) {
			Close();
		}

		private void button1_Click(object sender, EventArgs e) {
			double _pops = 0.0F;
			double _popr = 0.0F;
			if (double.TryParse(textBox1.Text, out _pops)) {
				currentRow.POPSETUP = _pops / 60;
			} else {
				currentRow.POPSETUP = _pops;
			}
			if (double.TryParse(textBox2.Text, out _popr)) {
				currentRow.POPRUN = _popr / 60;
			} else {
				currentRow.POPRUN = _popr;
			}

			//DataRowView cb2 = (comboBox2.SelectedItem as DataRowView);
			currentRow.POPOP = (int)comboBox2.SelectedValue;
			//currentRow.POPOP = (int)cb2[@"OPID"];
			currentRow.POPPART = PartID;

			currentRow.PARTNUM = string.Empty;

			currentRow.OPPROG = false;
			currentRow.OPRUN = 0.0F;
			currentRow.OPSETUP = 0.0F;

			using (ENGINEERINGDataSetTableAdapters.CutPartOpsTableAdapter cpota =
				new ENGINEERINGDataSetTableAdapters.CutPartOpsTableAdapter()) {
				if (NewOp && PartID > 0) {
					currentRow.Table.Rows.Add(currentRow);
					foreach (ENGINEERINGDataSet.CutPartOpsRow row in currentRow.Table.Rows) {
						cpota.Update(row);
					}
				} else {
					cpota.Update(currentRow);
				}
			}
			Close();
		}

		private void EditOp_FormClosing(object sender, FormClosingEventArgs e) {
			Properties.Settings.Default.EditWindowLocation = Location;
			Properties.Settings.Default.EditOpSize = Size;
			Properties.Settings.Default.Save();
		}

		private void Unselect(ComboBox sender) {
			sender.SelectionLength = 0;
		}

		private void comboBox_Resize(object sender, EventArgs e) {
			ComboBox _me = (sender as ComboBox);
			_me.SelectionLength = 0;
		}

		private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) {
			if (user_edit) {
				ComboBox cbx = sender as ComboBox;
				DataRowView drv = cbx.SelectedItem as DataRowView;
				float tmp = 0.0F;
				if (float.TryParse(drv[4].ToString(), out tmp)) {
					textBox1.Text = string.Format(Properties.Settings.Default.NumberFormat, tmp * 60);
				}
				if (float.TryParse(drv[5].ToString(), out tmp)) {
					textBox2.Text = string.Format(Properties.Settings.Default.NumberFormat, tmp * 60);
				}
				user_edit = false;
			}
		}

		private void user_editing(object sender, MouseEventArgs e) {
			user_edit = true;
		}

		private void user_editing(object sender, KeyEventArgs e) {
			user_edit = true;
		}
	}
}
