using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RedBrick2 {
  public partial class EditRev : Form {
    private int index = 0;
    private Revs RevSet;
    private ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter guta =
      new ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter();

    public EditRev(int idx, Revs revSet) {
      InitializeComponent();

      if (Properties.Settings.Default.OnlyActiveAuthors) {
        gENUSERSBindingSource.Filter = @"ACTIVE = True AND DEPT = 6";
      } else {
        gENUSERSBindingSource.Filter = @"DEPT = 6";
      }

      index = idx;
      RevSet = revSet;
    }

    public EditRev(Revs revSet) {
      InitializeComponent();

      if (Properties.Settings.Default.OnlyActiveAuthors) {
	      gENUSERSBindingSource.Filter = @"ACTIVE = True AND DEPT = 6";
	    } else {
	      gENUSERSBindingSource.Filter = @"DEPT = 6";
	    }

      RevSet = revSet;
      int? uid = guta.GetUID(Environment.UserName);
      if (uid == null) {
        throw new NullReferenceException(@"Hmm... Maybe you're not in the DB.");
      }
      index = RevSet.NewRev(string.Empty, string.Empty, (int)uid, DateTime.Now);
      comboBox1.Text = RevSet[index].Level;
      comboBox2.SelectedText = RevSet[index].AuthorFullName;
    }

    private void button1_Click(object sender, EventArgs e) {
      Rev r = RevSet[index];
      r.ECO = textBox1.Text;
      r.Description = textBox2.Text;
      r.Date = dateTimePicker1.Value;
    }

    private void EditOp_Load(object sender, EventArgs e) {
      // TODO: This line of code loads data into the 'eNGINEERINGDataSet.GEN_USERS' table. You can move, or remove it, as needed.
      this.gEN_USERSTableAdapter.Fill(this.eNGINEERINGDataSet.GEN_USERS);

    }

    private void button2_Click(object sender, EventArgs e) {
      Close();
    }
  }
}