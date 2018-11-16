using System.Text;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace RedBrick2.RelatedCutlistReport.ExistingCutlistReportDataSetTableAdapters {
	public partial class CUT_CUTLISTSTableAdapter {
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		private string GetQuery(HashSet<string> items) {
			string query_ = @"SELECT CUT_CUTLISTS.CLID, CUT_CUTLISTS.PARTNUM, CUT_CUTLISTS.REV, CUT_CUTLISTS.DRAWING,
CUT_CUTLISTS.CUSTID, CUT_CUTLISTS.CDATE, CUT_CUTLISTS.DESCR,
(USER_SETUP_BY.FIRST + ' ' + USER_SETUP_BY.LAST + ' ' + ISNULL(USER_SETUP_BY.SUFFIX, '')) AS SETUP_BY,
(USER_STATE_BY.FIRST + ' ' + USER_STATE_BY.LAST + ' ' + ISNULL(USER_STATE_BY.SUFFIX, '')) AS STATE_BY,
CUT_STATES.STATE
FROM
((CUT_CUTLISTS INNER JOIN GEN_USERS AS USER_SETUP_BY ON CUT_CUTLISTS.SETUP_BY = USER_SETUP_BY.UID)
INNER JOIN GEN_USERS AS USER_STATE_BY ON CUT_CUTLISTS.STATE_BY = USER_STATE_BY.UID)
INNER JOIN CUT_STATES ON CUT_CUTLISTS.STATEID = CUT_STATES.ID ";
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

		/// <summary>
		/// Get a list of string arrays to feed into a ListView.
		/// </summary>
		/// <param name="items">A hashset of items to look up.</param>
		/// <returns>A <see cref="List{T}"/> of <see cref="string"/> arrays.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
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
						string setup_by = !rdr_.IsDBNull(7) ? Redbrick.TitleCase(rdr_.GetString(7)).Trim() : @"NULL";
						string state_by = !rdr_.IsDBNull(8) ? Redbrick.TitleCase(rdr_.GetString(8)).Trim() : @"NULL";
						string state = !rdr_.IsDBNull(9) ? Redbrick.TitleCase(rdr_.GetString(9)) : @"NULL";
						result.Add(new string[] { item_, rev_, descr_, dt_, setup_by, state_by, state });
					}
				}
			} catch (Exception ex) {
				Redbrick.ProcessError(ex);
			}
			return result;
		}
	}
}

