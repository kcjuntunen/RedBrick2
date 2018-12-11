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
	public partial class CncTodo : Form {
		public CncTodo() {
			InitializeComponent();
			InitListView();
		}
		
		private void InitListView() {
			listView1.FullRowSelect = true;
			listView1.HideSelection = false;
			listView1.MultiSelect = false;
			listView1.View = View.Details;
			listView1.SmallImageList = Redbrick.TreeViewIcons;

			listView2.FullRowSelect = true;
			listView2.HideSelection = false;
			listView2.MultiSelect = false;
			listView2.View = View.Details;
			listView2.SmallImageList = Redbrick.TreeViewIcons;
		}

		private void CncTodo_Load(object sender, EventArgs e) {
			// TODO: This line of code loads data into the 'cNCTodoDataSet.CUT_CNC_JOBS_VIEW1' table. You can move, or remove it, as needed.
			this.cUT_CNC_JOBS_VIEW1TableAdapter.Fill(this.cNCTodoDataSet.CUT_CNC_JOBS_VIEW1);
			QueryTab1();
		}

		private void QueryTab1() {
			listView1.Items.Clear();
			using (CNCTodo.CNCTodoDataSetTableAdapters.CUT_CNC_JOBS_VIEWTableAdapter ta_ =
				new CNCTodo.CNCTodoDataSetTableAdapters.CUT_CNC_JOBS_VIEWTableAdapter()) {
				ta_.FillByWorkCenter(cNCTodoDataSet.CUT_CNC_JOBS_VIEW, Convert.ToString(comboBox1.SelectedValue).Trim());
				foreach (CNCTodo.CNCTodoDataSet.CUT_CNC_JOBS_VIEWRow row in cNCTodoDataSet.CUT_CNC_JOBS_VIEW) {
					if (!checkBox1.Checked && row.IgnChk == -1) {
						continue;
					}
					string lastprn = row.LastPrn == "NOT PRINTED" ? "Not Printed" : Convert.ToDateTime(row.LastPrn).ToString(@"yyyy-MM-dd H:m tt");
					string[] data = new string[] {
						row.OpDue.ToString(@"yyyy-MM-dd"),
						row.JobNumber,
						row.JobQty.ToString(),
						row.JobStatus,
						row.PartNumber,
						row.PartRev,
						row.CutSt,
						lastprn,
						row.IgnChk == -1 ? "Yes" : "No"
					};
					ListViewItem l = new ListViewItem(data);
					l.Checked = row.IgnChk == -1;
					listView1.Items.Add(l);
				}
			}
			listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
		}

		private void QueryTab2() {
			using (CNCTodo.CNCTodoDataSetTableAdapters.CUT_CNC_CUTLIST_PARTSTableAdapter ta_ =
				new CNCTodo.CNCTodoDataSetTableAdapters.CUT_CNC_CUTLIST_PARTSTableAdapter()) {
				ta_.Fill(cNCTodoDataSet.CUT_CNC_CUTLIST_PARTS);
				foreach (CNCTodo.CNCTodoDataSet.CUT_CNC_CUTLIST_PARTSRow row in cNCTodoDataSet.CUT_CNC_CUTLIST_PARTS) {
					string[] data = new string[] {
						row.PARTNUM,
						row.DESCR,
						row.ItemNum,
						row.REV,
						row.FIN_L.ToString(),
						row.FIN_W.ToString(),
						row.THICKNESS.ToString(),
						row.OVER_L.ToString(),
						row.OVER_W.ToString(),
						row.UPDATE_CNC ? "Yes" :"No",
						row.CNC1,
						row.CNC2
					};
					ListViewItem l = new ListViewItem(data);
					l.Checked = row.UPDATE_CNC;
					listView2.Items.Add(l);
				}
				listView2.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			}
		}

			private void checkBox1_CheckedChanged(object sender, EventArgs e) {
			QueryTab1();
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
			QueryTab1();
		}

		private void tabControl1_MouseDoubleClick(object sender, MouseEventArgs e) {
			QueryTab2();
		}
	}
}
