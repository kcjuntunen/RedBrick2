﻿using System;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace RedBrick2 {
	partial class ManageCutlistTimeDataSet {
		public List<string[]> QueryCutlistTime(int clid) {
			using (ManageCutlistTimeDataSetTableAdapters.CUT_CUTLISTS_TIMETableAdapter ta_ =
				new ManageCutlistTimeDataSetTableAdapters.CUT_CUTLISTS_TIMETableAdapter()) {
				List<string[]> list_ = new List<string[]>();
				foreach (CUT_CUTLISTS_TIMERow row_ in ta_.GetDataByCLID(clid)) {
					string type_ = !row_.IsOPNAMENull() ? row_.OPNAME : row_.CTNOTE;
					string op_ = row_.CTISOP ? "Y" : "N";
					string setupTime_ = row_.CTSETUP.ToString(@"0.00");
					string runTime_ = row_.CTRUN.ToString(@"0.000000");
					string[] d_ = new string[] { type_, op_, setupTime_, runTime_, row_.CTID.ToString() };
						list_.Add(d_);
				}
				return list_;
			}
		}
	}
}

namespace RedBrick2.ManageCutlistTimeDataSetTableAdapters {
	public partial class CUT_CUTLISTS_TIMETableAdapter {
		public double GetCutlistRunTime(int clID, int[] types) {
			double total = 0.0f;
			using (ENGINEERINGDataSetTableAdapters.CUT_PARTSTableAdapter ta_ =
				new ENGINEERINGDataSetTableAdapters.CUT_PARTSTableAdapter()) {
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