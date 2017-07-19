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
    private bool NewRev = false;
    private Revs RevSet;
    private Rev ThisRev;
    private ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter guta =
      new ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter();

    public EditRev(int idx, Revs revSet) {
      InitializeComponent();
      index = idx;
      RevSet = revSet;
      NewRev = false;
    }

    public EditRev(Revs revSet) {
      InitializeComponent();
      RevSet = revSet;
      NewRev = true;
    }

    private void button1_Click(object sender, EventArgs e) {
      ThisRev.ECO = textBox1.Text;
      ThisRev.Description = textBox2.Text;
      ThisRev.SetAuthor((int)comboBox2.SelectedValue);
      ThisRev.Date = dateTimePicker1.Value;
      Close();
    }

    private void EditOp_Load(object sender, EventArgs e) {
      Location = Properties.Settings.Default.EditWindowLocation;
      Size = Properties.Settings.Default.EditRevSize;
      // TODO: This line of code loads data into the 'eNGINEERINGDataSet.GEN_USERS' table. You can move, or remove it, as needed.
      this.gEN_USERSTableAdapter.Fill(this.eNGINEERINGDataSet.GEN_USERS);

      if (Properties.Settings.Default.OnlyActiveAuthors) {
        gENUSERSBindingSource.Filter = @"ACTIVE = True AND DEPT = 6";
      } else {
        gENUSERSBindingSource.Filter = @"DEPT = 6";
      }

      if (NewRev) {
        int? uid = guta.GetUID(Environment.UserName);
        if (uid == null) {
          throw new NullReferenceException(@"Hmm... Maybe you're not in the DB.");
        }
        index = RevSet.NewRev(string.Empty, string.Empty, (int)uid, DateTime.Now);
        ThisRev = RevSet[RevSet.Count - 1];
        comboBox1.Text = ThisRev.Level;
        Text = string.Format(@"Creating Rev Level {0}", comboBox1.Text);
        comboBox2.SelectedIndex = comboBox2.FindString(ThisRev.AuthorFullName);
      } else {
        ThisRev = RevSet[index];
        comboBox1.Text = ThisRev.Level;
        Text = string.Format(@"Editing Rev Level {0}", comboBox1.Text);
        comboBox2.SelectedIndex = comboBox2.FindString(ThisRev.AuthorFullName);
        textBox1.Text = ThisRev.ECO;
        textBox2.Text = ThisRev.Description;
        dateTimePicker1.Value = ThisRev.Date;
      }

      comboBox1.Enabled = false;
    }

    private void button2_Click(object sender, EventArgs e) {
      if (NewRev) {
        RevSet.Remove(ThisRev);
      }

      Close();
    }

    private void comboBox1_TextUpdate(object sender, EventArgs e) {
      Text = string.Format(@"Creating Revision Level {0}", comboBox1.Text);
    }

    private void EditRev_FormClosing(object sender, FormClosingEventArgs e) {
      Properties.Settings.Default.EditWindowLocation = Location;
      Properties.Settings.Default.EditRevSize = Size;
      Properties.Settings.Default.Save();
    }

    private void combobox_Resize(object sender, EventArgs e) {
      ComboBox _me = (sender as ComboBox);
      _me.SelectionLength = 0;
    }
  }
}