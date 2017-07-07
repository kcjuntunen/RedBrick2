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
  public partial class EditOp : Form {
    private ENGINEERINGDataSetTableAdapters.CutPartOpsTableAdapter cpota =
      new ENGINEERINGDataSetTableAdapters.CutPartOpsTableAdapter();
    private ENGINEERINGDataSet.CutPartOpsRow currentRow;
    private SwProperties PropertySet;
    private bool NewOp = false;

    public EditOp(SwProperties props) {
      InitializeComponent();
      PropertySet = props;
      NewOp = true;
      ENGINEERINGDataSet.CutPartOpsDataTable dt = new ENGINEERINGDataSet.CutPartOpsDataTable();
      currentRow = (ENGINEERINGDataSet.CutPartOpsRow)dt.NewRow();
      currentRow.POPPART = props.PartsData.PARTID;
      if (props.PartsData.PARTID > 0) {
        currentRow.POPORDER = (int)cpota.GetNextInOrder(props.PartsData.PARTID);
      } else {
        currentRow.POPORDER = 1;
      }
      currentRow.TYPEID = (int)PropertySet[@"DEPARTMENT"].Data;
    }

    public EditOp(ENGINEERINGDataSet.CutPartOpsRow row) {
      InitializeComponent();
      currentRow = row;
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
      friendlyCutOpsBindingSource.Filter = string.Format(@"OPTYPE = {0}", currentRow.TYPEID);

      if (NewOp) {
        comboBox2.SelectedIndex = 0;
        currentRow.POPOP = (int)comboBox2.SelectedValue;
        textBox1.Text = string.Format(@"{0:0.000}", 0.0F);
        textBox2.Text = string.Format(@"{0:0.000}", 0.0F);
      } else {
        comboBox2.SelectedValue = currentRow.POPOP;
        textBox1.Text = string.Format(@"{0:0.000}", currentRow.POPSETUP * 60);
        textBox2.Text = string.Format(@"{0:0.000}", currentRow.POPRUN * 60);
      }
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

      currentRow.POPOP = (int)comboBox2.SelectedValue;
      if (NewOp) {
        cpota.Update((ENGINEERINGDataSet.CutPartOpsDataTable)currentRow.Table);
      } else {
        cpota.Update(currentRow);
      }
      Close();
    }

    private void EditOp_FormClosing(object sender, FormClosingEventArgs e) {
      Properties.Settings.Default.EditWindowLocation = Location;
      Properties.Settings.Default.EditOpSize = Size;
      Properties.Settings.Default.Save();
    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
      if ((sender as ComboBox).SelectedValue != null) {
        friendlyCutOpsBindingSource.Filter = string.Format(@"OPTYPE = {0}", (sender as ComboBox).SelectedValue); 
      }
    }
  }
}
