using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using System.IO;
using SolidWorks.Interop.sldworks;

namespace RedBrick2 {
  public partial class ModelRedbrick : UserControl {
    public ModelRedbrick(ModelDoc2 md) {
      ActiveDoc = md;
      PartFileInfo = new FileInfo(ActiveDoc.GetPathName());
      Hash = Redbrick.GetHash(PartFileInfo.FullName);
      InitializeComponent();
    }



    private System.Windows.Forms.BindingSource cptBindingSource;

    private void GetCutlistData() {
      ENGINEERINGDataSetTableAdapters.CutlistPartsTableAdapter cpt =
        new ENGINEERINGDataSetTableAdapters.CutlistPartsTableAdapter();
      cptBindingSource = new System.Windows.Forms.BindingSource(components);
      ((System.ComponentModel.ISupportInitialize)(cptBindingSource)).BeginInit();
      cptBindingSource.DataSource = cpt.GetDataByPartNum(Path.GetFileNameWithoutExtension(PartFileInfo.Name));
      comboBox6.DataSource = cptBindingSource;
    }


    public int PartID { get; set; }
    public int Hash { get; set; }
    public FileInfo PartFileInfo { get; set; }
    public ModelDoc2 ActiveDoc { get; set; }
    public SldWorks SwApp { get; set; }

    private void ModelRedbrick_Load(object sender, EventArgs e) {
      cUT_MATERIALSTableAdapter.Fill(eNGINEERINGDataSet.CUT_MATERIALS);
      cUT_EDGESTableAdapter.Fill(eNGINEERINGDataSet.CUT_EDGES);
      GetCutlistData();
    }

    private void comboBox_KeyDown(object sender, KeyEventArgs e) {
      (sender as ComboBox).DroppedDown = false;
    }

    private void comboBox_TextChanged(object sender, EventArgs e) {
      if ((sender as ComboBox).Text == string.Empty) {
        (sender as ComboBox).SelectedIndex = -1;
      }
    }
  }
}
