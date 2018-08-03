using System.Text;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace RedBrick2.ExistingCutlistReportDataSetTableAdapters {
	public partial class CUT_CUTLISTSTableAdapter {
		private string GetQuery(HashSet<string> items) {
			string query_ = @"SELECT * FROM CUT_CUTLISTS ";
			StringBuilder criteria_ = new StringBuilder();
			criteria_.Append(@"WHERE ");
			HashSet<string>.Enumerator e_ = items.GetEnumerator();
			e_.MoveNext();
			for (int i = 0; i < items.Count; i++) {
				if (i > 0) {
					criteria_.Append(@"OR ");
				}
				criteria_.Append(string.Format(@"PARTNUM = '{0}' ", e_.Current));
				e_.MoveNext();
			}
			criteria_.Append(@";");
			return query_ + criteria_.ToString();
		}

		public List<string[]> Lookup(HashSet<string> items) {
			string qry_ = GetQuery(items);
			//StringBuilder result = new StringBuilder();
			List<string[]> result = new List<string[]>();
			if (Connection.State != System.Data.ConnectionState.Open) {
				Connection.Open();
			}
			SqlCommand cmd_ = new SqlCommand(qry_, Connection);
			try {
				using (SqlDataReader rdr_ = cmd_.ExecuteReader()) {
					while (rdr_.Read()) {
						string item_ = !rdr_.IsDBNull(1) ? rdr_.GetString(1) : @"NULL";
						string rev_ = !rdr_.IsDBNull(2) ? rdr_.GetString(2) : @"100";
						string descr_ = !rdr_.IsDBNull(6) ? Redbrick.TitleCase(rdr_.GetString(6)) : @"NULL";
						string dt_ = !rdr_.IsDBNull(5) ? 
							string.Format(@"{0:yyyy-MM-dd ddd HH:mm:ss}", rdr_.GetDateTime(5))
							: @"NULL";
						result.Add(new string[] { item_, rev_, descr_, dt_ });
					}
				}
			} catch (Exception) {

				throw;
			}
			return result;
		}
	}
}

