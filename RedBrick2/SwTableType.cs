using System.Collections.Generic;
using System.IO;

using SolidWorks.Interop.sldworks;

namespace RedBrick2 {
	public class SwTableType {
		private ModelDoc2 part;
		private SelectionMgr swSelMgr;
		private ITableAnnotation swTable;
		private System.Collections.Specialized.StringCollection _masterHashes;
		private bool initialated = false;
		private Dictionary<string, FileInfo> path_dict = new Dictionary<string, FileInfo>();
		public IBomFeature found_bom = null;

		public SwTableType(ModelDoc2 md, string tablehash) {
			MasterHash = tablehash;
			_masterHashes[0] = MasterHash;
			part = md;
			swSelMgr = (SelectionMgr)part.SelectionManager;
			if (part != null && swSelMgr != null) {
				BomFeature swBom = null;
				try {
					swBom = (BomFeature)swSelMgr.GetSelectedObject6(1, -1);
					found_bom = swBom;
				} catch {
					// 
				}
				if (swBom != null) {
					fill_table(swBom);
				} else {
					find_bom();
				}
			}
		}

		public SwTableType(ModelDoc2 md, System.Collections.Specialized.StringCollection sc) {
			_masterHashes = sc;
			part = md;
			swSelMgr = (SelectionMgr)part.SelectionManager;
			if (part != null && swSelMgr != null) {
				BomFeature swBom = null;
				try {
					swBom = (BomFeature)swSelMgr.GetSelectedObject6(1, -1);
				} catch {
					// 
				}
				if (swBom != null) {
					fill_table(swBom);
				} else {
					find_bom();
				}
			}
		}

		public SwTableType(ModelDoc2 md, System.Collections.Specialized.StringCollection sc, string part_column)
			: this(md, sc) {
			PartColumn = part_column;
		}

		private void fill_table(BomFeature bom) {
			found_bom = bom;
			string itno = string.Empty;
			string ptno = string.Empty;
			Columns.Clear();
			Parts.Clear();
			swTable = (ITableAnnotation)bom.IGetTableAnnotations(1);
			part.ClearSelection2(true);

			ColumnCount = swTable.ColumnCount;
			find_part_column();
			RowCount = swTable.RowCount;
			for (int i = 0; i < ColumnCount; i++) {
				Columns.Add(swTable.get_DisplayedText(0, i));
			}
			object[] bomtaa = (object[])bom.GetTableAnnotations();
			BomTableAnnotation bta = (BomTableAnnotation)bomtaa[0];
			int prtcol = get_column_by_name(PartColumn);
			for (int i = 0; i < RowCount; i++) {
				string prt_ = swTable.get_DisplayedText(i, prtcol);
				Parts.Add(prt_);
				string[] pathnames = (string[])bta.GetModelPathNames(i, out itno, out ptno);
				if (pathnames != null) {
					foreach (string pathname in pathnames) {
						FileInfo fi_ = new FileInfo(pathname);
						PathList.Add(fi_);
						path_dict.Add(prt_.ToUpper(), fi_);
					}
				}
			}
			initialated = true;
		}

		public FileInfo get_path(string doc) {
			if (PathList != null) {
				foreach (FileInfo fi in PathList) {
					if (fi.Name.ToUpper().Contains(doc.Trim().ToUpper())) {
						return fi;
					}
				}
			}
			return null;
		}

		public FileInfo get_path2(string doc) {
			string key_ = doc.ToUpper();
			if (path_dict != null && path_dict.ContainsKey(key_)) {
				return path_dict[key_];
			}
			return null;
		}

		private int get_column_by_name(string prop) {
			if (!initialated) {
				for (int i = 0; i < ColumnCount; i++) {
					if (swTable.get_DisplayedText(0, i).Trim().ToUpper().Equals(prop.ToUpper())) {
						return i;
					}
				}
			} else {
				int count = 0;
				foreach (string s in Columns) {
					if (s.Trim().ToUpper().Equals(prop.Trim().ToUpper())) {
						return count;
					}
					count++;
				}
			}
			return -1;
		}

		private void find_part_column() {
			for (int i = 0; i < ColumnCount; i++) {
				if (swTable.get_DisplayedText(0, i).Trim().ToUpper().Contains("PART")) {
					PartColumn = swTable.get_DisplayedText(0, i).Trim();
				}
			}
		}

		private int get_row_by_partname(string prt) {
			if (!initialated) {
				int prtcol = get_column_by_name(PartColumn);
				for (int i = 0; i < RowCount; i++) {
					if (swTable.get_DisplayedText(i, prtcol).Trim().ToUpper().Equals(prt.Trim().ToUpper())) {
						return i;
					}
				}
			}
			return Parts.IndexOf(prt);
		}

		private string get_property_by_part(string prt, string prop, string part_column_name) {
			int prtrow = get_row_by_partname(prt);
			int prpcol = get_column_by_name(prop);
			return (prpcol < 1 || prtrow < 1) ? string.Empty : swTable.get_DisplayedText(prtrow, prpcol);
		}

		private string get_property_by_part(int row, string prop, string part_column_name) {
			int prpcol = get_column_by_name(prop);
			return (prpcol < 1 || row < 1) ? string.Empty : swTable.get_DisplayedText(row, prpcol);
		}

		private void find_bom() {
			bool found = false;
			Feature feature = (Feature)part.FirstFeature();
			if (part != null) {
				while (feature != null) {
					if (feature.GetTypeName2().ToUpper() == "BOMFEAT") {
						feature.Select2(false, -1);
						BomFeature bom = (BomFeature)swSelMgr.GetSelectedObject6(1, -1);
						fill_table(bom);
						if (identify_table(Columns, _masterHashes)) {
							found = true;
							System.Diagnostics.Debug.WriteLine("Found a table.");
							break;
						}
					}
					feature = (Feature)feature.GetNextFeature();
				}
			}
			if (!found) {
				throw new SWTableTypeException("I couldn't find the correct table.");
			}
		}

		private bool identify_table(List<string> table, System.Collections.Specialized.StringCollection tablehash) {
			bool match = false;
			string str = string.Empty;
			string[] ss = new string[table.Count];
			table.CopyTo(ss);
			System.Array.Sort(ss);
			foreach (string s in ss) {
				str += string.Format("{0}|", s.ToUpper().Replace(@".", string.Empty));
			}

			Stream columns = new MemoryStream();
			columns.Write(System.Text.Encoding.UTF8.GetBytes(str), 0, str.Length - 1);

			string hash = System.BitConverter.ToString(System.Security.Cryptography.MD5.Create().ComputeHash(to_byte_array(str)));

			foreach (string h in tablehash) {
				match |= hash == h;
			}

			return match;
		}

		private byte[] to_byte_array(string s) {
			byte[] ba = new byte[s.Length];
			int count = 0;
			foreach (char c in s) {
				ba[count] = (byte)c;
				count++;
			}
			return ba;
		}

		public string GetProperty(string part, string prop) {
			return get_property_by_part(part, prop, PartColumn);
		}

		public string GetProperty(int row, string prop) {
			return get_property_by_part(row, prop, PartColumn);
		}

		public string PartColumn { get; set; } = "PART NUMBER";

		public int ColumnCount { get; set; } = 0;

		public int RowCount { get; set; } = 0;
		public List<FileInfo> PathList { get; set; } = new List<FileInfo>();

		public List<string> Columns { get; set; } = new List<string>();

		public List<string> Parts { get; set; } = new List<string>();
		public string MasterHash { get; set; } = "00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00";
	}
}
