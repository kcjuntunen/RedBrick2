using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace RedBrick2.Drawings {
	public partial class Drawings : Form {
		public Drawings() {
			InitializeComponent();
			Size = Properties.Settings.Default.DrawingsSize;
			Location = Properties.Settings.Default.DrawingsLocation;

			listView1.FullRowSelect = true;
			listView1.HideSelection = false;
			listView1.MultiSelect = false;
			listView1.View = View.Details;
			listView1.SmallImageList = Redbrick.TreeViewIcons;
		}

		private void textBox1_TextChanged(object sender, EventArgs e) {
			string text = (sender as TextBox).Text.Trim();
			string search_string = string.Format(@"FName LIKE '{0}%'", text);
			gENDRAWINGSBindingSource.Filter = search_string;
			gENDRAWINGSEDRWBindingSource.Filter = search_string;
			gENDRAWINGSMTLBindingSource.Filter = search_string;
		}

		private void Drawings_Load(object sender, EventArgs e) {
			gEN_DRAWINGS_MTLTableAdapter.Fill(drawingDataSet.GEN_DRAWINGS_MTL);
			gEN_DRAWINGS_EDRWTableAdapter.Fill(drawingDataSet.GEN_DRAWINGS_EDRW);
			gEN_DRAWINGSTableAdapter.Fill(drawingDataSet.GEN_DRAWINGS);
		}

		private void open_std_drw_Click(object sender, EventArgs e) {
			if (std_drw_listBox.SelectedItem == null) {
				return;
			}
			DataRowView drv = std_drw_listBox.SelectedItem as DataRowView;
			string path = string.Format(@"{0}{1}", drv["FPath"], drv["FName"]);
			System.Diagnostics.Process.Start(path);
		}

		private void open_std_mdl_Click(object sender, EventArgs e) {
			if (std_mdl_listBox.SelectedItem == null) {
				return;
			}
			DataRowView drv = std_mdl_listBox.SelectedItem as DataRowView;
			string path = string.Format(@"{0}{1}", drv["FPath"], drv["FName"]);
			System.Diagnostics.Process.Start(path);
		}

		private void open_mtl_drw_Click(object sender, EventArgs e) {
			if (mtl_drw_listBox.SelectedItem == null) {
				return;
			}
			DataRowView drv = mtl_drw_listBox.SelectedItem as DataRowView;
			string path = string.Format(@"{0}{1}", drv["FPath"], drv["FName"]);
			System.Diagnostics.Process.Start(path);
		}

		private void open_sw_std_drw_Click(object sender, EventArgs e) {
			if (std_drw_listBox.SelectedItem == null) {
				return;
			}
			DataRowView drv = std_drw_listBox.SelectedItem as DataRowView;
			string path = string.Format(@"{0}{1}", drv["FPath"].ToString().ToUpper()
				.Replace("K:", "G:"),
				drv["FName"].ToString().ToUpper()
				.Replace(".PDF", ".SLDDRW"));
				
			if (!(new FileInfo(path)).Exists) {
				MessageBox.Show(this, string.Format(@"Couldn't find '{0}'", path), "IDK", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			System.Diagnostics.Process.Start(path);
			Close();
		}

		private void open_sw_mtl_drw_Click(object sender, EventArgs e) {
			if (mtl_drw_listBox.SelectedItem == null) {
				return;
			}
			DataRowView drv = mtl_drw_listBox.SelectedItem as DataRowView;
			string path = string.Format(@"{0}{1}", drv["FPath"].ToString().ToUpper(),
				drv["FName"].ToString().ToUpper()
				.Replace(".PDF", ".SLDDRW"));
			if (!(new FileInfo(path)).Exists) {
				MessageBox.Show(this, string.Format(@"Couldn't find '{0}'", path), "IDK", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			System.Diagnostics.Process.Start(path);
			Close();
		}

		private void Drawings_FormClosing(object sender, FormClosingEventArgs e) {
			Properties.Settings.Default.DrawingsSize = Size;
			Properties.Settings.Default.DrawingsLocation = Location;
			Properties.Settings.Default.Save();
		}

		private void SearchQuery() {
			if (srch_tb == null || srch_tb.Text.Trim() == string.Empty) {
				return;
			}
			string srch_term = string.Format("{0}", srch_tb.Text.Trim());
			Cursor = Cursors.WaitCursor;
			itemsTableAdapter1.FillByDescription(drawingDataSet.Items, srch_term);
			listView1.Items.Clear();
			if (drawingDataSet.Items.Count < 1) {
				Cursor = Cursors.Default;
				return;
			}
			try {
				foreach (DrawingDataSet.ItemsRow row in drawingDataSet.Items) {
					string[] data = {
					row.PARTNUM,
					Convert.ToString(row.DESCR),
					row.LENGTH < .001 ? "--" : string.Format(@"{0:0.000}", row.LENGTH),
					row.WIDTH < .001 ? "--" : string.Format(@"{0:0.000}", row.WIDTH),
					row.HEIGHT < .001 ? "--" : string.Format(@"{0:0.000}", row.HEIGHT)
				};
					listView1.Items.Add(new ListViewItem(data));
				}
				listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			} catch (Exception e) {
				Redbrick.ProcessError(e);
			} finally {
				Cursor = Cursors.Default;
			}
		}

		private void srch_btn_Click(object sender, EventArgs e) {
			SearchQuery();
		}

		private void button1_Click(object sender, EventArgs e) {
			if (listView1.SelectedItems.Count < 1) {
				return;
			}
			if (listView1.SelectedItems[0] == null) {
				return;
			}
		}

		private void srch_tb_KeyDown(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Enter) {
				SearchQuery();
			}
		}

		private void listView1_SelectedIndexChanged(object sender, EventArgs e) {
			if (listView1.SelectedItems.Count < 1) {
				return;
			}

			if (listView1.SelectedItems[0] == null) {
				return;
			}

			ListViewItem lvi = listView1.SelectedItems[0];
			textBox1.Text = lvi.Text;
		}
	}
}
