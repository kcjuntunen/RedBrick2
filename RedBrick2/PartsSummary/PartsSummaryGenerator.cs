using OfficeOpenXml;

namespace RedBrick2.PartsSummary {
	/// <summary>
	/// A class to dump cutlist data to a excel sheet.
	/// </summary>
	public class PartsSummaryGenerator {
		private System.Collections.Specialized.OrderedDictionary _partsList;
		private string _path;

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="partsList">An <see cref="System.Collections.Specialized.OrderedDictionary"/> of
		/// <see cref="System.String"/> keys, and <see cref="SwProperties"/> values.</param>
		/// <param name="path"></param>
		public PartsSummaryGenerator(System.Collections.Specialized.OrderedDictionary partsList, string path) {
			_partsList = partsList;
			_path = path;
		}

		/// <summary>
		/// Generate and save the Excel sheet. It pops up a SaveAs dialogue.
		/// </summary>
		public void Generate() {
			using (ExcelPackage xlp_ = new ExcelPackage(new System.IO.FileInfo(_path))) {
				ExcelWorksheet wksht_ = xlp_.Workbook.Worksheets[1];
				int max_row = wksht_.Dimension.End.Row - 2;
				int row_offset = 2;
				int row = 1 + row_offset;
				foreach (string key in _partsList.Keys) {
					SwProperties p = (SwProperties)_partsList[key];
					wksht_.Cells[row, 2].Value = p.PartLookup;
					wksht_.Cells[row, 3].Value = p[@"Description"].ResolvedValue;
					wksht_.Cells[row, 4].Value = p.CutlistQty;
					wksht_.Cells[row, 5].Value = p[@"CUTLIST MATERIAL"].ResolvedValue;
					wksht_.Cells[row, 6].Value = p[@"LENGTH"].Data;
					wksht_.Cells[row, 7].Value = p[@"WIDTH"].Data;
					wksht_.Cells[row, 8].Value = p[@"THICKNESS"].Data;
					wksht_.Cells[row, 9].Value = p[@"BLANK QTY"].Data;
					wksht_.Cells[row, 10].Value = p[@"OVERL"].Data;
					wksht_.Cells[row, 11].Value = p[@"OVERW"].Data;
					wksht_.Cells[row, 12].Value = p[@"CNC1"].ResolvedValue;
					wksht_.Cells[row, 13].Value = p[@"CNC2"].ResolvedValue;
					for (int i = 0; i < Properties.Settings.Default.OpCount; i++) {
						string op_ = string.Format(@"OP{0}", i + 1);
						wksht_.Cells[row, 14 + i].Value = p[op_].ResolvedValue;
					}
					if (++row >= max_row) {
						break;
					}
				}
				using (System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog()) {
					sfd.Filter = @"Excel File (*.xlsx)|*.xlsx";
					if (SuggestedName.Trim() != string.Empty) {
						sfd.FileName = SuggestedName;
					}
					if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
						System.IO.FileInfo saveas = new System.IO.FileInfo(sfd.FileName);
						xlp_.SaveAs(saveas);
						System.Diagnostics.Process.Start(saveas.FullName);
					}
				}
			}
		}

		/// <summary>
		/// We can suggest a name from the outside.
		/// </summary>
		public string SuggestedName { get; set; }

	}
}
