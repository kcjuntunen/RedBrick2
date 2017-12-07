using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
	/// <summary>
	/// A set of properties that makes up a revision LVL.
	/// </summary>
	public class Rev {
		private SldWorks SwApp;
		private ModelDoc2 ActiveDoc;
		private StringProperty level;
		private StringProperty eco;
		private StringProperty description;
		private AuthorProperty author;
		private DateProperty date;
		private Dictionary<string, string> ecoData = new Dictionary<string, string>();
		private ENGINEERINGDataSetTableAdapters.ECRObjLookupTableAdapter eol =
			new ENGINEERINGDataSetTableAdapters.ECRObjLookupTableAdapter();
		private ENGINEERINGDataSetTableAdapters.LegacyECRObjLookupTableAdapter leol =
			new ENGINEERINGDataSetTableAdapters.LegacyECRObjLookupTableAdapter();

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="lvl">Index.</param>
		/// <param name="ecrno">ECR value.</param>
		/// <param name="descr">A description.</param>
		/// <param name="sw">The connected application.</param>
		/// <param name="md">The relevant ModelDoc2.</param>
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

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="lvl">Index of LVL.</param>
		/// <param name="ecrno">ECR value.</param>
		/// <param name="descr">A description.</param>
		/// <param name="aut">Author ID.</param>
		/// <param name="dt">A DateTime.</param>
		/// <param name="sw">The connected application.</param>
		/// <param name="md">The relevant ModelDoc2.</param>
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

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="lvl">String Property of LVL.</param>
		/// <param name="ecr">StringProperty of ECR value.</param>
		/// <param name="descr">StringProperty of a description.</param>
		/// <param name="aut">AuthorProperty.</param>
		/// <param name="dt">DateProperty.</param>
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
				if (_ecrn > Redbrick.LastLegacyECR) {
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
					if (dt.Rows.Count > 0) {
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

		/// <summary>
		/// Write the LVL to SolidWorks.
		/// </summary>
		public void Write() {
			level.Write();
			eco.Write();
			description.Write();
			author.Write();
			date.Write();
		}

		/// <summary>
		/// Delete this LVL from SolidWorks.
		/// </summary>
		public void Delete() {
			level.Delete();
			eco.Delete();
			description.Delete();
			author.Delete();
			date.Delete();
		}

		/// <summary>
		/// Set Author ID.
		/// </summary>
		/// <param name="id"></param>
		public void SetAuthor(int id) {
			author.Data = id;
		}

		/// <summary>
		/// The LVL.
		/// </summary>
		public string Level {
			get { return level.Data.ToString(); }
			private set { level.Data = value; }
		}

		/// <summary>
		/// An ECR value.
		/// </summary>
		public string ECO {
			get { return eco.Data.ToString(); }
			set {
				eco.Data = value;
				GetECOData();
			}
		}

		/// <summary>
		/// A description of a LVL.
		/// </summary>
		public string Description {
			get { return description.Data.ToString(); }
			set { description.Data = value; }
		}

		/// <summary>
		/// The full name of an author.
		/// </summary>
		public string AuthorFullName {
			get { return author.FullName; }
			private set { AuthorFullName = value; }
		}

		/// <summary>
		/// The initials of an author.
		/// </summary>
		public string Author {
			get { return author.Initials; }
			private set { author.Data = value; }
		}

		/// <summary>
		/// The Date of the LVL.
		/// </summary>
		public DateTime Date {
			get { return (DateTime)date.Data; }
			set { date.Data = value; }
		}

		/// <summary>
		/// The drawing file we're looking at.
		/// </summary>
		public System.IO.FileInfo ReferencedFile {
			get { return new System.IO.FileInfo((SwApp.ActiveDoc as ModelDoc2).GetPathName()); }
			private set { ReferencedFile = value; }
		}

		/// <summary>
		/// The part number we're looking at.
		/// </summary>
		public string PartNumber {
			get {
				return System.IO.Path.GetFileNameWithoutExtension(ReferencedFile.FullName).Split(' ')[0];
			}
			private set { PartNumber = value; }
		}

		/// <summary>
		/// A representative node for inserting into a TreeView.
		/// </summary>
		public TreeNode Node {
			get {
				ENGINEERINGDataSetTableAdapters.LegacyECRObjLookupTableAdapter leol =
					new ENGINEERINGDataSetTableAdapters.LegacyECRObjLookupTableAdapter();
				int eco = 0;
				string legacy = string.Empty;
				if (int.TryParse(ECO, out eco)) {
					legacy = Redbrick.LastLegacyECR < eco ? string.Empty : @"(Legacy)";
				}
				TreeNode topNode = new TreeNode(Level, 0, 0);
				TreeNode ecoNode = new TreeNode(string.Format(@"ECR #: {0} {1}", ECO, legacy), 1, 1);
				TreeNode dNode = new TreeNode(string.Format(@"Date: {0}", Date.ToLongDateString()), 2, 2);
				TreeNode lNode = new TreeNode(string.Format(@"By: {0}", Redbrick.TitleCase(AuthorFullName)), 3, 3);
				TreeNode descr = new TreeNode(string.Format(@"Description: {0}", Description), 4, 4);
				foreach (KeyValuePair<string, string> kvp in ecoData) {
					int iconidx = 0;
					if (kvp.Key.ToUpper().Contains(@"DESCR") ||
						kvp.Key.ToUpper().Contains(@"REVISION") ||
						kvp.Key.ToUpper().Contains(@"CHANGE")) {
						iconidx = 4;
					}
					switch (Redbrick.action[kvp.Key]) {
						case Redbrick.Format.NAME:
							ecoNode.Nodes.Add(
								new TreeNode(string.Format(@"{0}: {1}", Redbrick.translation[kvp.Key], Redbrick.TitleCase(kvp.Value)), 3, 3));
							break;
						case Redbrick.Format.STRING:
							if (kvp.Value == string.Empty) {
								continue;
							}
							if (kvp.Value.Contains("\n")) {
								TreeNode subNode = new TreeNode(Redbrick.translation[kvp.Key], iconidx, iconidx);
								foreach (string subs in kvp.Value.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries)) {
									if (subs.ToUpper().Contains(@"ITEM"))
										iconidx = 5;
									if (subs.ToUpper().Contains(@"DESCR"))
										iconidx = 4;
									TreeNode subsubNode = new TreeNode(subs, iconidx, iconidx);
									subNode.Nodes.Add(subsubNode);
								}
								ecoNode.Nodes.Add(subNode);
							} else {
								ecoNode.Nodes.Add(
									new TreeNode(string.Format(@"{0}: {1}", Redbrick.translation[kvp.Key], kvp.Value), iconidx, iconidx));
							}
							break;
						case Redbrick.Format.DATE:
							DateTime _dt = new DateTime();
							if (DateTime.TryParse(kvp.Value, out _dt)) {
								ecoNode.Nodes.Add(
									new TreeNode(string.Format(@"{0}: {1}", Redbrick.translation[kvp.Key], _dt.ToLongDateString()), 2, 2));
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
				topNode.Nodes.AddRange(new TreeNode[] { ecoNode, descr, dNode, lNode });
				topNode.Expand();
				return topNode;
			}
			set { Node = value; }
		}
	}
}
