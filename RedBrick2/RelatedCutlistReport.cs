using System.Collections.Generic;
using System.Windows.Forms;

namespace RedBrick2 {
	public partial class RelatedCutlistReport : Form {
		public RelatedCutlistReport(HashSet<string> items) {
			InitializeComponent();

			Location = Properties.Settings.Default.RelatedCutlistLocation;
			Size = Properties.Settings.Default.RelatedCutlistSize;

			listView1.FullRowSelect = true;
			listView1.HideSelection = false;
			listView1.MultiSelect = false;
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

		private void RelatedCutlistReport_FormClosing(object sender, FormClosingEventArgs e) {
			Properties.Settings.Default.RelatedCutlistLocation = Location;
			Properties.Settings.Default.RelatedCutlistSize = Size;
			Properties.Settings.Default.Save();
		}
	}
}
