using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
  class Rev {
    private SldWorks SwApp;
    private ModelDoc2 ActiveDoc;
    private StringProperty level;
    private StringProperty eco;
    private StringProperty description;
    private AuthorProperty author;
    private DateProperty date;
    private bool translationSetup = false;
    private Dictionary<string, string> ecoData = new Dictionary<string, string>();
    private ENGINEERINGDataSetTableAdapters.ECRObjLookupTableAdapter eol =
      new ENGINEERINGDataSetTableAdapters.ECRObjLookupTableAdapter();
    private ENGINEERINGDataSetTableAdapters.LegacyECRObjLookupTableAdapter leol =
      new ENGINEERINGDataSetTableAdapters.LegacyECRObjLookupTableAdapter();

    public Rev(int lvl, string ecrno, string descr, SldWorks sw, ModelDoc2 md) {
      SwApp = sw;
      ActiveDoc = md;

      level = new StringProperty(string.Format(@"REVISION {0}", (char)(lvl + 64)), true, SwApp, ActiveDoc, string.Empty);
      level.Data = string.Format(@"A{0}", (char)(lvl + 64));

      eco = new StringProperty(string.Format(@"ECO {0}", lvl), true, SwApp, ActiveDoc, string.Empty);
      ECO = ecrno;

      description = new StringProperty(string.Format(@"DESCRIPTION {0}", lvl), true, SwApp, ActiveDoc, string.Empty);
      Description = descr;

      author = new AuthorProperty(string.Format(@"LIST {0}", lvl), true, SwApp, ActiveDoc);

      date = new DateProperty(string.Format(@"DATE {0}", lvl), true, SwApp, ActiveDoc);
      Date = DateTime.Now;

      GetECOData();
    }

    public Rev(int lvl, string ecrno, string descr, int aut, DateTime dt, SldWorks sw, ModelDoc2 md) {
      SwApp = sw;
      ActiveDoc = md;
      int idx = lvl + 1;
      int ltr = lvl + 65;

      level = new StringProperty(string.Format(@"REVISION {0}", (char)ltr), true, SwApp, ActiveDoc, string.Empty);
      level.Data = string.Format(@"A{0}", (char)ltr);

      eco = new StringProperty(string.Format(@"ECO {0}", idx), true, SwApp, ActiveDoc, string.Empty);
      ECO = ecrno;

      description = new StringProperty(string.Format(@"DESCRIPTION {0}", idx), true, SwApp, ActiveDoc, string.Empty);
      Description = descr;

      author = new AuthorProperty(string.Format(@"LIST {0}", idx), true, SwApp, ActiveDoc);
      SetAuthor(aut);

      date = new DateProperty(string.Format(@"DATE {0}", idx), true, SwApp, ActiveDoc);
      Date = dt;

      GetECOData();
    }

    public Rev(StringProperty lvl, StringProperty ecr, StringProperty descr, AuthorProperty aut, DateProperty dt) {
      level = lvl;
      eco = ecr;
      description = descr;
      author = aut;
      date = dt;
      GetECOData();
    }

    private void GetECOData() {
      int _ecrn = 0;
      if (ecoData.Count > 0) {
        ecoData.Clear();
      }
      if (int.TryParse(ECO, out _ecrn)) {
        int _maxecr = 0;
        if (int.TryParse(leol.GetLastLegactECR(), out _maxecr)) {
          if (_ecrn > _maxecr) {
            ENGINEERINGDataSet.ECRObjLookupDataTable dt = eol.GetDataByECO(_ecrn);
            ENGINEERINGDataSet.ECRObjLookupRow r = (ENGINEERINGDataSet.ECRObjLookupRow)dt.Rows[0];
            foreach (System.Data.DataColumn col in dt.Columns) {
              if (col.ToString().Contains(@"ReqBy")) {
                ecoData.Add(col.ToString(), Redbrick.TitleCase(r[col.ToString()].ToString()));
              } else {
                ecoData.Add(col.ToString(), r[col.ToString()].ToString());
              }
            }
          } else {
            ENGINEERINGDataSet.LegacyECRObjLookupDataTable dt = leol.GetDataByECO(ECO);
            ENGINEERINGDataSet.LegacyECRObjLookupRow r = (ENGINEERINGDataSet.LegacyECRObjLookupRow)dt.Rows[0];
            foreach (System.Data.DataColumn col in dt.Columns) {
              if (col.ToString().Contains(@"Engineer") || col.ToString().Contains(@"Holder")) {
                ecoData.Add(col.ToString(), Redbrick.TitleCase(r[col.ToString()].ToString()));
              } else {
                ecoData.Add(col.ToString(), r[col.ToString()].ToString());
              }
            }
          }
        }
      }
    }

    private string ProperCase(string allCapsInput) {
      string fixedOutput = string.Empty;
      fixedOutput = allCapsInput.ToLower();
      if (allCapsInput.Length > 1) {
        return char.ToUpper(fixedOutput[0]) + fixedOutput.Substring(1);
      } else {
        return string.Empty;
      }
    }
    
    public void Write() {
      level.Write();
      eco.Write();
      description.Write();
      author.Write();
      date.Write();
    }

    public void SetAuthor(int id) {
      author.Data = id;
    }

    public string Level {
      get { return level.Data.ToString(); }
      private set { level.Data = value; }
    }

    public string ECO {
      get { return eco.Data.ToString(); }
      set { eco.Data = value; }
    }

    public string Description {
      get { return description.Data.ToString(); }
      set { description.Data = value; }
    }

    public string AuthorFullName {
      get { return author.FullName; }
      private set { AuthorFullName = value; }
    }

    public string Author {
      get { return author.Initials; }
      private set { author.Data = value; }
    }

    public DateTime Date {
      get { return (DateTime)date.Data; }
      set { date.Data = value; }
    }

    public TreeNode Node {
      get {
        TreeNode topNode = new TreeNode(Level);
        TreeNode ecoNode = new TreeNode(string.Format(@"ECR #: {0}", ECO));
        TreeNode lNode = new TreeNode(string.Format(@"By: {0}", Redbrick.TitleCase(AuthorFullName)));
        TreeNode dNode = new TreeNode(string.Format(@"Date: {0}", Date.ToShortDateString()));
        foreach (KeyValuePair<string, string> kvp in ecoData) {
          switch (Redbrick.action[kvp.Key]) {
            case Redbrick.Format.NAME:
              ecoNode.Nodes.Add(
                new TreeNode(string.Format(@"{0}: {1}", Redbrick.translation[kvp.Key], Redbrick.TitleCase(kvp.Value))));
              break;
            case Redbrick.Format.STRING:
              if (kvp.Value.Contains("\n")) {
                TreeNode subNode = new TreeNode(Redbrick.translation[kvp.Key]);
                foreach (string subs in kvp.Value.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries)) {
                  TreeNode subsubNode = new TreeNode(subs);
                  subNode.Nodes.Add(subsubNode);
                }
                ecoNode.Nodes.Add(subNode);
              } else {
                ecoNode.Nodes.Add(
                  new TreeNode(string.Format(@"{0}: {1}", Redbrick.translation[kvp.Key], kvp.Value)));
              }
              break;
            case Redbrick.Format.DATE:
              DateTime _dt = new DateTime();
              if (DateTime.TryParse(kvp.Value, out _dt)) {
                ecoNode.Nodes.Add(
                  new TreeNode(string.Format(@"{0}: {1}", Redbrick.translation[kvp.Key], _dt.ToShortDateString())));
              }
              break;
            case Redbrick.Format.SKIP:
              continue;
            default:
              break;
            //TreeNode subNode = new TreeNode(string.Format(@"{0}: {1}", Redbrick.TitleCase(kvp.Key), kvp.Value));
            //ecoNode.Nodes.Add(subNode);
          }
        }
        topNode.Nodes.AddRange(new TreeNode[] { ecoNode, lNode, dNode });
        return topNode;
      }
      set { Node = value; }
    }
  }
}
