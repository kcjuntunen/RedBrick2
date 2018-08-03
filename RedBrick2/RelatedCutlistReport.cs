using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace RedBrick2 {
	public partial class RelatedCutlistReport : Form {
		public RelatedCutlistReport(HashSet<string> items) {
			InitializeComponent();

			Location = Properties.Settings.Default.RelatedCutlistLocation;
			Size = Properties.Settings.Default.RelatedCutlistSize;

			listView1.FullRowSelect = true;
			listView1.HideSelection = false;
			listView1.MultiSelect = true;
			listView1.View = View.Details;
			listView1.SmallImageList = Redbrick.TreeViewIcons;

			using (ExistingCutlistReportDataSetTableAdapters.CUT_CUTLISTSTableAdapter ta_ =
				new ExistingCutlistReportDataSetTableAdapters.CUT_CUTLISTSTableAdapter()) {
				foreach (string[] item in ta_.Lookup(items)) {
					ListViewItem i_ = new ListViewItem(item, 0);
					listView1.Items.Add(i_);
				}
			}
			listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
		}

		private void listView1_ColumnClick(object sender, ColumnClickEventArgs e) {
			ListView lv_ = sender as ListView;
			if (lv_.Columns[e.Column].ListView.Sorting == SortOrder.Ascending) {
				lv_.Columns[0].ListView.Sorting = SortOrder.Descending;
			} else {
				lv_.Columns[0].ListView.Sorting = SortOrder.Ascending;
			}
		}

		private void RelatedCutlistReport_FormClosed(object sender, FormClosedEventArgs e) {
			Properties.Settings.Default.RelatedCutlistLocation = Location;
			Properties.Settings.Default.RelatedCutlistSize = Size;
			Properties.Settings.Default.Save();
		}

		private void listView1_KeyDown(object sender, KeyEventArgs e) {
			ListView lv_ = sender as ListView;
			if (lv_.SelectedItems.Count < 1) {
				return;
			}
			if (e.Control && e.KeyCode == Keys.C) {
				StringBuilder sb_ = new StringBuilder();
				foreach (ListViewItem item in lv_.SelectedItems) {
					sb_.AppendLine(string.Format("{0}\t{1}\t{2}\t[{3}]",
						item.SubItems[0].Text,
						item.SubItems[1].Text,
						item.SubItems[2].Text,
						item.SubItems[3].Text));
				}
				Clipboard.SetText(sb_.ToString());
			}
		}
	}
}
