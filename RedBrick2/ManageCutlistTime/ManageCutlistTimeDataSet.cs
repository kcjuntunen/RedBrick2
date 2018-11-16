using System;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace RedBrick2.ManageCutlistTime {
	partial class ManageCutlistTimeDataSet {
		/// <summary>
		/// Update cutlist time by cutlist time ID.
		/// </summary>
		/// <param name="ctid">Cutlist time ID.</param>
		/// <param name="clid">Cutlist ID.</param>
		/// <param name="op">Op ID.</param>
		/// <param name="op_method">Op method ID.</param>
		/// <param name="op_setup">Setup time.</param>
		public void UpdCLTimeByID(int ctid, int clid, int op, int op_method, double op_setup) {
			using (ManageCutlistTimeDataSetTableAdapters.CUT_CUTLISTS_TIMETableAdapter ta_ =
				new ManageCutlistTimeDataSetTableAdapters.CUT_CUTLISTS_TIMETableAdapter()) {
				using (CUT_CUTLISTS_TIMEDataTable dt_ = ta_.GetDataByCLID(clid)) {
					using (ManageCutlistTimeDataSetTableAdapters.CutlistPartsTableAdapter cpta_ =
						new ManageCutlistTimeDataSetTableAdapters.CutlistPartsTableAdapter()) {
						foreach (CUT_CUTLISTS_TIMERow item in dt_.Rows) {
							switch (op_method) {
								case 2: // per cl
									break;
								case 3: // flat
									op_setup = op_setup * Convert.ToInt32(cpta_.CountFlat(op, clid));
									break;
								case 4: // edge
									op_setup = op_setup * cpta_.CountEdges(clid, op);
									break;
								default:
									break;
							}
							if (op_setup == 0) {
								ta_.DeleteByCTID(ctid);
							} else {
								ta_.UpdateGDSetupByCTID(op_setup, ctid);
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Get a structure ready to insert into a <see cref="System.Windows.Forms.ListView"/>.
		/// </summary>
		/// <param name="clid">A <see cref="int"/> of a cutlist ID.</param>
		/// <returns>A <see cref="List{String}"/> of arrays of <see cref="string"/>s.</returns>
		public List<string[]> QueryCutlistTime(int clid) {
			using (ManageCutlistTimeDataSetTableAdapters.CUT_CUTLISTS_TIMETableAdapter ta_ =
				new ManageCutlistTimeDataSetTableAdapters.CUT_CUTLISTS_TIMETableAdapter()) {
				List<string[]> list_ = new List<string[]>();
				foreach (CUT_CUTLISTS_TIMERow row_ in ta_.GetDataByCLID(clid)) {
					string type_ = !row_.IsOPNAMENull() ? row_.OPNAME : row_.CTNOTE;
					string op_ = row_.CTISOP ? "Y" : "N";
					string setupTime_ = row_.CTSETUP.ToString(@"0.00");
					string runTime_ = row_.CTRUN.ToString(@"0.000000");
					string ctnote_ = row_.IsCTNOTENull() ? string.Empty : row_.CTNOTE;
					string ctop_ = row_.CTOP.ToString();
					string[] d_ = new string[] { type_, op_, setupTime_, runTime_, row_.CTID.ToString(), ctnote_, ctop_, row_.CLID.ToString() };
					list_.Add(d_);
				}
				return list_;
			}
		}
	}
}

namespace RedBrick2.ManageCutlistTime.ManageCutlistTimeDataSetTableAdapters {
	partial class CutlistPartsTableAdapter {
		/// <summary>
		/// Return the unique edges on a part.
		/// </summary>
		/// <param name="clid">An <see cref="int"/> of a cutlist ID.</param>
		/// <param name="op">A POPOP <see cref="int"/></param>
		/// <returns></returns>
		public int CountEdges(int clid, int op) {
			HashSet<int> hs_ = new HashSet<int>();
			foreach (string edge in new string[] { @"EDGEID_LF", @"EDGEID_LB", @"EDGEID_WR", @"EDGEID_WL" }) {
				foreach (int item in CountEdge(clid, op, edge)) {
					hs_.Add(item);
				}
			}
			return hs_.Count;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		private HashSet<int> CountEdge(int clid, int op, string edge) {
			HashSet<int> hs_ = new HashSet<int>();
			string sql_ = string.Format(@"SELECT CUT_CUTLIST_PARTS.{0} FROM CUT_PART_OPS INNER JOIN CUT_CUTLIST_PARTS ON CUT_PART_OPS.POPPART = 
CUT_CUTLIST_PARTS.PARTID 
GROUP BY CUT_CUTLIST_PARTS.{0}, CUT_CUTLIST_PARTS.CLID, CUT_PART_OPS.POPOP 
HAVING (((CUT_CUTLIST_PARTS.CLID)=@clid) AND ((CUT_PART_OPS.POPOP)=@glop));", edge);
			using (CUT_CUTLISTS_TIMETableAdapter cctta_ = new CUT_CUTLISTS_TIMETableAdapter()) {
				using (SqlCommand cmd_ = new SqlCommand(sql_, cctta_.Connection)) {
					cmd_.Parameters.AddWithValue(@"@clid", clid);
					cmd_.Parameters.AddWithValue(@"@glop", op);
					if (cctta_.Connection.State != System.Data.ConnectionState.Open) {
						cctta_.Connection.Open();
					}
					using (SqlDataReader dr_ = cmd_.ExecuteReader()) {
						while (dr_.Read()) {
							if (dr_[0] != null && dr_.GetInt32(0) != 0) {
								hs_.Add(dr_.GetInt32(0));
							}
						}
					}
				}
			}
			return hs_;
		}
	}

	public partial class CUT_CUTLISTS_TIMETableAdapter {
		/// <summary>
		/// Get the total run time of a cutlist with types selected.
		/// </summary>
		/// <param name="clID">The cutlist ID. An <see cref="int"/>.</param>
		/// <param name="types">The part types to sum up. An array of <see cref="int"/></param>
		/// <returns>A <see cref="double"/> of hours.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		public double GetCutlistRunTime(int clID, int[] types) {
			double total = 0.0f;
			using (CUT_PARTSTableAdapter ta_ =
				new CUT_PARTSTableAdapter()) {
				StringBuilder sb_ = new StringBuilder(@"SELECT Sum([POPRUN]*[QTY]) AS TotalRun
FROM(CUT_OPS RIGHT JOIN(CUT_PART_OPS RIGHT JOIN CUT_CUTLIST_PARTS ON CUT_PART_OPS.POPPART = CUT_CUTLIST_PARTS.PARTID) ON CUT_OPS.OPID = CUT_PART_OPS.POPOP) INNER JOIN CUT_PARTS ON CUT_CUTLIST_PARTS.PARTID = CUT_PARTS.PARTID
WHERE(((CUT_CUTLIST_PARTS.CLID) = @cutlistID) AND((CUT_PARTS.TYPE)In(");
				SqlCommand cmd_ = new SqlCommand(string.Empty, ta_.Connection);
				cmd_.Parameters.AddWithValue(@"@cutlistID", clID);
				for (int i = 0; i < types.Length; i++) {
					string par_ = string.Format(@"@type{0}", i);
					sb_.Append(par_);
					if (i + 1 < types.Length) {
						sb_.Append(@", ");
					}
					cmd_.Parameters.AddWithValue(par_, types[i]);
				}
				sb_.Append(@")));");
				cmd_.CommandText = sb_.ToString();
				//System.Windows.Forms.MessageBox.Show(sb_.ToString());
				if (ta_.Connection.State == System.Data.ConnectionState.Closed) {
					ta_.Connection.Open();
				}
				try {
					using (SqlDataReader rdr_ = cmd_.ExecuteReader()) {
						while (rdr_.Read()) {
							if (!rdr_.IsDBNull(0)) {
								total += rdr_.GetDouble(0);
							}
						}
					}
				} catch (Exception) {

					throw;
				} finally {
					ta_.Connection.Close();
					cmd_.Dispose();
				}
			}
			return total;
		}

		/// <summary>
		/// Get the total setup time of a cutlist with types selected.
		/// </summary>
		/// <param name="clID">The cutlist ID. An <see cref="int"/>.</param>
		/// <param name="types">The part types to sum up. An array of <see cref="int"/></param>
		/// <returns>A <see cref="double"/> of hours.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		public double GetCutlistSetupTime(int clID, int[] types) {
			double total = 0.0f;
			using (ENGINEERINGDataSetTableAdapters.CUT_PARTSTableAdapter ta_ =
				new ENGINEERINGDataSetTableAdapters.CUT_PARTSTableAdapter()) {
				SqlCommand cmd_ = new SqlCommand(string.Empty, ta_.Connection);
				StringBuilder sb_ = new StringBuilder(@"SELECT Sum([POPSETUP]) AS TotalSetup
FROM(CUT_OPS RIGHT JOIN(CUT_PART_OPS RIGHT JOIN CUT_CUTLIST_PARTS ON CUT_PART_OPS.POPPART = CUT_CUTLIST_PARTS.PARTID) ON CUT_OPS.OPID = CUT_PART_OPS.POPOP) INNER JOIN CUT_PARTS ON CUT_CUTLIST_PARTS.PARTID = CUT_PARTS.PARTID
WHERE(((CUT_CUTLIST_PARTS.CLID) = @cutlistID) AND((CUT_PARTS.TYPE)In(");
				cmd_.Parameters.AddWithValue(@"@cutlistID", clID);
				for (int i = 0; i < types.Length; i++) {
					string par_ = string.Format(@"@type{0}", i);
					sb_.Append(par_);
					if (i + 1 < types.Length) {
						sb_.Append(@", ");
					}
					cmd_.Parameters.AddWithValue(par_, types[i]);
				}
				sb_.Append(@")));");
				//System.Windows.Forms.MessageBox.Show(sb_.ToString());
				cmd_.CommandText = sb_.ToString();
				if (ta_.Connection.State == System.Data.ConnectionState.Closed) {
					ta_.Connection.Open();
				}
				try {
					using (SqlDataReader rdr_ = cmd_.ExecuteReader()) {
						while (rdr_.Read()) {
							if (!rdr_.IsDBNull(0)) {
								total += rdr_.GetDouble(0);
							}
						}
					}
				} catch (Exception) {

					throw;
				} finally {
					ta_.Connection.Close();
					cmd_.Dispose();
				}
			}
			return total;
		}
	}
}


