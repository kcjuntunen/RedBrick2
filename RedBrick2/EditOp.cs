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
    private int PartID;
    private bool NewOp = false;

    public EditOp(SwProperties props) {
      InitializeComponent();
      PropertySet = props;
      PartID = PropertySet[@"DEPARTMENT"].PartID;
      NewOp = true;

      ENGINEERINGDataSet.CutPartOpsDataTable dt = new ENGINEERINGDataSet.CutPartOpsDataTable();
      int _next = 1;
      currentRow = (ENGINEERINGDataSet.CutPartOpsRow)dt.NewRow();
      currentRow.POPPART = props.PartsData.PARTID;

      if (PartID > 0) {
        try {
          _next = (int)cpota.GetNextInOrder(PartID);
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
      currentRow.POPPART = PartID;

      currentRow.PARTNUM = string.Empty;

      currentRow.OPPROG = false;
      currentRow.OPRUN = 0.0F;
      currentRow.OPSETUP = 0.0F;

      if (NewOp) {
        currentRow.Table.Rows.Add(currentRow);
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

    private void comboBox_Resize(object sender, EventArgs e) {
      ComboBox _me = (sender as ComboBox);
      _me.SelectionLength = 0;
    }
  }
}
