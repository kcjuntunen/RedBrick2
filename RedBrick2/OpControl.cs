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

    private bool initialated = false;

    private ENGINEERINGDataSetTableAdapters.CutPartOpsTableAdapter cpo =
      new ENGINEERINGDataSetTableAdapters.CutPartOpsTableAdapter();
    private ENGINEERINGDataSet.CutPartOpsRow currentrow;

    public OpControl(ENGINEERINGDataSet.CutPartOpsRow row) {
      InitializeComponent();
      currentrow = row;
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


  }
}
