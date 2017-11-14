using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
  public partial class EditRev : Form {
    private int index = 0;
    private bool NewRev = false;
    private Revs RevSet;
    private Rev ThisRev;
    private StringProperty level;
    private ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter guta =
      new ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter();

    public EditRev(int idx, Revs revSet) {
      InitializeComponent();
      index = idx;
      RevSet = revSet;
      NewRev = false;
      level = new StringProperty(@"REVISION LEVEL", true, RevSet.SwApp, (RevSet.SwApp.ActiveDoc as ModelDoc2), @"REV");
      level.Get();
      ToggleFlameWar(Properties.Settings.Default.FlameWar);
    }

    public EditRev(Revs revSet) {
      InitializeComponent();
      RevSet = revSet;
      NewRev = true;
      level = new StringProperty(@"REVISION LEVEL", true, RevSet.SwApp, (RevSet.SwApp.ActiveDoc as ModelDoc2), @"REV");
      level.Get();
      ToggleFlameWar(Properties.Settings.Default.FlameWar);
    }

    public void ToggleFlameWar(bool on) {
    if (on) {
      textBox1.CharacterCasing = CharacterCasing.Upper;
      textBox2.CharacterCasing = CharacterCasing.Upper;
    } else {
      textBox1.CharacterCasing = CharacterCasing.Normal;
      textBox2.CharacterCasing = CharacterCasing.Normal;
    }
  }

    private void button1_Click(object sender, EventArgs e) {
      ThisRev.ECO = textBox1.Text;
      ThisRev.Description = textBox2.Text;
      ThisRev.SetAuthor((int)comboBox2.SelectedValue);
      ThisRev.Date = dateTimePicker1.Value;
      if (NewRev) {
        AddECRItem();
      }
      Close();
    }

    private void AddECRItem() {
      string question = string.Format(Properties.Resources.InsertIntoEcrItems, ThisRev.PartNumber, ThisRev.ECO);

      DialogResult mbr = DialogResult.No;
      ENGINEERINGDataSet.ECRObjLookupDataTable eoldt =
        new ENGINEERINGDataSet.ECRObjLookupDataTable();
      ENGINEERINGDataSet.ECR_ITEMSDataTable eidt =
        new ENGINEERINGDataSet.ECR_ITEMSDataTable();
      int en = 0;
      if (int.TryParse(ThisRev.ECO, out en) &&
        !eoldt.ECRIsBogus(en) &&
        !eidt.ECRItemExists(en, ThisRev.PartNumber, level.Value)) {
          mbr = (DialogResult)MessageBox.Show(this, question, @"Insert ECR?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
      }
      if (mbr == DialogResult.Yes) {
        ENGINEERINGDataSet.inmastDataTable idt =
          new ENGINEERINGDataSet.inmastDataTable();
        ENGINEERINGDataSetTableAdapters.ECR_ITEMSTableAdapter eit =
          new ENGINEERINGDataSetTableAdapters.ECR_ITEMSTableAdapter();
        int parttype = idt.GetPartType(ThisRev.PartNumber, level.Value);
        //int ecr_item_id = eit.InsertECRItem(en, ThisRev.PartNumber, level.Value, parttype);
        ENGINEERINGDataSet.GEN_DRAWINGSDataTable dt =
          new ENGINEERINGDataSet.GEN_DRAWINGSDataTable();
        ENGINEERINGDataSetTableAdapters.GEN_DRAWINGSTableAdapter gd =
          new ENGINEERINGDataSetTableAdapters.GEN_DRAWINGSTableAdapter();
        ENGINEERINGDataSet.GEN_DRAWINGSDataTable ddt = gd.GetDataByFName(string.Format(@"{0}%", ThisRev.PartNumber));
        if (ddt.Rows.Count > 0) {
          System.Data.DataRow r = ddt.Rows[0];
          FileInfo f = new FileInfo(string.Format(@"{0}\{1}", r[@"FPath"].ToString(), r[@"FName"].ToString()));
          FileInfo ff = new FileInfo(string.Format(@"{0}\{1}_{2}-{3}.PDF", f.DirectoryName, en, ThisRev.PartNumber, ThisRev.Level));
          MessageBox.Show(string.Format("{0}\\{1}\nparttype = {2}", ff.DirectoryName, ff.Name, parttype));
        }
      }
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
        int? uid = guta.GetUID(System.Environment.UserName);
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