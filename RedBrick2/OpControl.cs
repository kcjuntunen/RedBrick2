using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedBrick2 {
  public partial class OpControl : UserControl {
    private int partopid = -1;
    private int index = -1;
    private int partid = -1;
    private int department = 1;
    private double tb1val = 0.0f;
    private double tb2val = 0.0f;
    private SwProperties PropertySet;

    private bool initialated = false;

    private ENGINEERINGDataSetTableAdapters.CutPartOpsTableAdapter cpo =
      new ENGINEERINGDataSetTableAdapters.CutPartOpsTableAdapter();
    private ENGINEERINGDataSet.CutPartOpsRow currentrow;

    public OpControl(ENGINEERINGDataSet.CutPartOpsRow row, SwProperties props) {
      InitializeComponent();
      currentrow = row;
      PropertySet = props;
      partopid = (int)row[@"POPID"];
      partid = (int)row[@"POPPART"];
      index = (int)row[@"POPORDER"];
      department = (int)row[@"TYPEID"];
    }
    
    private void OpControl_Load(object sender, EventArgs e) {
      this.friendlyCutOpsTableAdapter.Fill(this.eNGINEERINGDataSet.FriendlyCutOps);
      this.cUT_PART_TYPESTableAdapter.Fill(this.eNGINEERINGDataSet.CUT_PART_TYPES);
      friendlyCutOpsBindingSource.Filter = string.Format(@"OPTYPE = {0}", department);
      label5.Text = currentrow[@"POPORDER"].ToString();
      comboBox1.SelectedValue = (int)currentrow[@"POPOP"];
      comboBox2.SelectedValue = (int)currentrow[@"TYPEID"];
      textBox1.Text = string.Format(@"{0:0.000}", (double)currentrow[@"POPSETUP"] * 60);
      textBox2.Text = string.Format(@"{0:0.000}", (double)currentrow[@"POPRUN"] * 60);
      initialated = true;
    }

    private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) {
      if (initialated) {
        department = (sender as ComboBox).SelectedIndex;
        friendlyCutOpsBindingSource.Filter = string.Format(@"OPTYPE = {0}", department);
      }
    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
      if (initialated) {
        ComboBox cb = (sender as ComboBox);
        if (cb.SelectedItem != null) {
          currentrow[@"POPOP"] = cb.SelectedValue;
          textBox1.Text = string.Format(@"{0:0.000}", (double)currentrow[@"OPSETUP"] * 60);
          textBox2.Text = string.Format(@"{0:0.000}", (double)currentrow[@"OPRUN"] * 60);
        }
          //cpo.Update(currentrow);
      }
    }

    private void textBox1_Validating(object sender, CancelEventArgs e) {
      if (initialated) {
        double res = 0.0f;
        if (!double.TryParse((sender as TextBox).Text, out res)) {
          e.Cancel = true;
        } else {
          tb1val = res;
        }
      }
    }

    private void textBox1_Validated(object sender, EventArgs e) {
      if (initialated) {
        currentrow[@"POPSETUP"] = tb1val;
        //cpo.Update(currentrow);
      }
    }

    private void textBox2_Validating(object sender, CancelEventArgs e) {
      if (initialated) {
        double res = 0.0f;
        if (!double.TryParse((sender as TextBox).Text, out res)) {
          e.Cancel = true;
        } else {
          tb2val = res;
        }
      }
    }

    private void textBox2_Validated(object sender, EventArgs e) {
      if (initialated) {
        currentrow[@"POPRUN"] = tb2val;
        //cpo.Update(currentrow);
      }
    }

    public ENGINEERINGDataSet.CutPartOpsRow MyRow { 
      get { return currentrow; }
      private set { currentrow = value; }
    }

    private void label5_Click(object sender, EventArgs e) {

    }

    private void button1_Click(object sender, EventArgs e) {
      EditOp eo = new EditOp(MyRow);
      eo.ShowDialog(this);
    }

    private void GetOps() {
      FlowLayoutPanel flowLayoutPanel1 = Parent as FlowLayoutPanel;
      flowLayoutPanel1.Controls.Clear();
      foreach (ENGINEERINGDataSet.CutPartOpsRow row in cpo.GetDataByPartID(partid)) {
        OpControl opc = new OpControl(row, PropertySet);
        opc.Width = flowLayoutPanel1.Width - 25;
        flowLayoutPanel1.Controls.Add(opc);
        flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        opc.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      }
    }

    private void button2_Click(object sender, EventArgs e) {
      // UP
      int _me_ordr = MyRow.POPORDER;
      int _me_idx = _me_ordr - 1;
      int _othr_idx = _me_ordr - 2;
      if (_me_ordr > 1) {
        ENGINEERINGDataSet.CutPartOpsRow _other = (ENGINEERINGDataSet.CutPartOpsRow)MyRow.Table.Rows[_othr_idx];
        int _other_ordr = _other.POPORDER;
        _other.POPORDER = _me_ordr;
        MyRow.POPORDER = _other_ordr;
        (Parent.Controls[_othr_idx] as OpControl).label5.Text = _me_ordr.ToString();
        label5.Text = _other_ordr.ToString();
        cpo.Update(MyRow);
        cpo.Update(_other);
      }
      GetOps();
    }

    private void button3_Click(object sender, EventArgs e) {
      // DOWN
      int _me_ordr = MyRow.POPORDER;
      int _me_idx = _me_ordr - 1;
      int _othr_idx = _me_ordr;
      int count = MyRow.Table.Rows.Count;
      if (_me_ordr + 1 <= count) {
        ENGINEERINGDataSet.CutPartOpsRow _other = (ENGINEERINGDataSet.CutPartOpsRow)MyRow.Table.Rows[_othr_idx];
        int _other_ordr = _other.POPORDER;
        _other.POPORDER = _me_ordr;
        MyRow.POPORDER = _other_ordr;
        (Parent.Controls[_othr_idx] as OpControl).label5.Text = _me_ordr.ToString();
        label5.Text = _other_ordr.ToString();
        cpo.Update(MyRow);
        cpo.Update(_other);
      }
      GetOps();
    }

  }
}
