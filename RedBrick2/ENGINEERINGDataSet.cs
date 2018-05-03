using System;
using System.Text;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RedBrick2 {
	public partial class ENGINEERINGDataSet {
		partial class CLIENT_STUFFDataTable {
			/// <summary>
			/// Get a list of locations for an item.
			/// </summary>
			/// <param name="_item">An item to look up.</param>
			/// <returns>A <see cref="List{String}"/> object.</returns>
			public List<string> GetLocations(string _item) {
				List<string> l_ = new List<string>();
				using (ENGINEERINGDataSetTableAdapters.CLIENT_STUFFTableAdapter cs_ =
					new ENGINEERINGDataSetTableAdapters.CLIENT_STUFFTableAdapter()) {
					double sum_ = 0.0F;
					CLIENT_STUFFDataTable dt_ = cs_.GetData(_item);
					for (int i = 0; i < dt_.Count; i++) {
						CLIENT_STUFFRow r_ = dt_[i];
						l_.Add(string.Format("{0,-10}  {1,-9:0.0} {2,-3}", r_.LOC, r_.QTY, r_.UofM));
						sum_ += Convert.ToDouble(r_.QTY);
					}
					l_.Add(string.Format("{0,-10}  {1,-9:0.0} {2,-3}", " ", sum_, " "));
				}
				return l_;
			}
		}

		partial class CUT_PART_OPSDataTable {
			public double GetCutlistRuntime(int clID, int[] types) {
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
					if (ta_.Connection.State == System.Data.ConnectionState.Closed) {
						ta_.Connection.Open();
					}
					try {
						using (SqlDataReader rdr_ = cmd_.ExecuteReader()) {
							while (rdr_.Read()) {
								total += rdr_.GetDouble(0);
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

			public double GetCutlistSetuptime(int clID, int[] types) {
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
					cmd_.CommandText = sb_.ToString();
					if (ta_.Connection.State == System.Data.ConnectionState.Closed) {
						ta_.Connection.Open();
					}
					try {
						using (SqlDataReader rdr_ = cmd_.ExecuteReader()) {
							while (rdr_.Read()) {
								total += rdr_.GetDouble(0);
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
			/// Update new OP tables.
			/// </summary>
			/// <param name="_pp">An SwProperties object.</param>
			/// <returns>Number of affected rows.</returns>
			public int UpdateOps(SwProperties _pp) {
				int total_affected_ = 0;
				using (ENGINEERINGDataSetTableAdapters.CUT_OPSTableAdapter ta_ =
					new ENGINEERINGDataSetTableAdapters.CUT_OPSTableAdapter()) {
					using (CUT_OPSDataTable dt_ = new CUT_OPSDataTable()) {
						ta_.Fill(dt_);
						for (int i = 0; i < Properties.Settings.Default.OpCount; i++) {
							int affected_ = 0;
							int seq_ = i + 1;
							string op_ = string.Format(@"OP{0}", seq_);
							OpProperty p_ = _pp[op_] as OpProperty;
							if (Convert.ToInt32(p_.Data) < 1) {
								affected_ = delete_op_(p_.PartID, seq_);
							} else {
								string filter_ = string.Format(@"OPID = {0}", p_.Data);
								CUT_OPSRow[] rr_ = dt_.Select(filter_) as CUT_OPSRow[];
								double setup_ = rr_.Length > 0 ? Convert.ToDouble(rr_[0].OPSETUP) : 0;
								double run_ = rr_.Length > 0 ? Convert.ToDouble(rr_[0].OPRUN) : 0;
								affected_ = update_op_(p_.PartID, seq_, Convert.ToInt32(p_.Data), setup_, run_);
								if (affected_ < 1) {
									affected_ = insert_op_(p_.PartID, seq_, Convert.ToInt32(p_.Data), setup_, run_);
								}
							}
							total_affected_ += affected_;
						}
					}
				}
				return total_affected_;
			}

			private int delete_op_(int _partid, int _seq) {
				int affected_ = 0;
				using (ENGINEERINGDataSetTableAdapters.CUT_OPSTableAdapter ta_ =
					new ENGINEERINGDataSetTableAdapters.CUT_OPSTableAdapter()) {
					string sql_ = @"DELETE FROM CUT_PART_OPS WHERE POPPART = @partid AND POPORDER = @seq";
					using (SqlCommand comm = new SqlCommand(sql_, ta_.Connection)) {
						comm.Parameters.AddWithValue(@"@partid", _partid);
						comm.Parameters.AddWithValue(@"@seq", _seq);
						if (ta_.Connection.State == System.Data.ConnectionState.Closed) {
							ta_.Connection.Open();
						}
						try {
							affected_ = comm.ExecuteNonQuery();
						} finally {
							ta_.Connection.Close();
						}
					}
				}
				return affected_;
			}

			private int update_op_(int _partid, int _seq, int _popop, double _popsetup, double _poprun) {
				int affected_ = 0;
				using (ENGINEERINGDataSetTableAdapters.CUT_OPSTableAdapter ta_ =
					new ENGINEERINGDataSetTableAdapters.CUT_OPSTableAdapter()) {
					string sql_ = @"UPDATE CUT_PART_OPS SET POPOP = @popop " + //, POPSETUP = @popsetup, POPRUN = @poprun " +
						@"WHERE POPPART = @partid AND POPORDER = @seq";
					using (SqlCommand comm = new SqlCommand(sql_, ta_.Connection)) {
						comm.Parameters.AddWithValue(@"@popop", _popop);
						//comm.Parameters.AddWithValue(@"@popsetup", _popsetup);
						//comm.Parameters.AddWithValue(@"@poprun", _poprun);
						comm.Parameters.AddWithValue(@"@partid", _partid);
						comm.Parameters.AddWithValue(@"@seq", _seq);
						if (ta_.Connection.State == System.Data.ConnectionState.Closed) {
							ta_.Connection.Open();
						}
						try {
							affected_ = comm.ExecuteNonQuery();
						} finally {
							ta_.Connection.Close();
						}
					}
				}
				return affected_;
			}

			private int insert_op_(int _partid, int _seq, int _popop, double _popsetup, double _poprun) {
				int affected_ = 0;
				using (ENGINEERINGDataSetTableAdapters.CUT_OPSTableAdapter ta_ =
					new ENGINEERINGDataSetTableAdapters.CUT_OPSTableAdapter()) {
					string sql_ = @"INSERT INTO CUT_PART_OPS (POPPART, POPORDER, POPOP, POPSETUP , POPRUN) " +
						@"VALUES (@partid, @seq, @popop, @popsetup, @poprun)";
					using (SqlCommand comm = new SqlCommand(sql_, ta_.Connection)) {
						comm.Parameters.AddWithValue(@"@popop", _popop);
						comm.Parameters.AddWithValue(@"@popsetup", _popsetup);
						comm.Parameters.AddWithValue(@"@poprun", _poprun);
						comm.Parameters.AddWithValue(@"@partid", _partid);
						comm.Parameters.AddWithValue(@"@seq", _seq);
						if (ta_.Connection.State == System.Data.ConnectionState.Closed) {
							ta_.Connection.Open();
						}
						try {
							affected_ += comm.ExecuteNonQuery();
						} finally {
							ta_.Connection.Close();
						}
					}
				}
				return affected_;
			}
		}

		partial class CUT_PARTSDataTable {
			/// <summary>
			/// Update or insert part into db.
			/// </summary>
			/// <param name="_pp">An SwProperties object.</param>
			/// <returns>ID from CUT_PARTS</returns>
			public int UpdatePart(SwProperties _pp) {
				int partid_ = update_part_(_pp);
				return partid_ > 0 ? partid_ : insert_part_(_pp);
			}

			/// <summary>
			/// Update only the general properties. It's public so that the
			/// warning box can update dimensions.
			/// </summary>
			/// <param name="_pp">An <see cref="SwProperties"/> object.</param>
			/// <returns>A <see cref="int"/> of affected rows.</returns>
			public int update_general_properties_(SwProperties _pp) {
				int affected_ = 0;
				using (ENGINEERINGDataSetTableAdapters.CUT_PARTSTableAdapter ta_ =
					new ENGINEERINGDataSetTableAdapters.CUT_PARTSTableAdapter()) {
					string sql_ = @"UPDATE CUT_PARTS SET FIN_L = @finl, FIN_W = @finw, THICKNESS = @thk, " +
						@"HASH = @hash WHERE PARTNUM=@prtNo";
					using (SqlCommand comm = new SqlCommand(sql_, ta_.Connection)) {
						comm.Parameters.AddWithValue(@"@prtNo", _pp.PartLookup);
						comm.Parameters.AddWithValue(@"@finl", Convert.ToDouble(_pp[@"LENGTH"].Data));
						comm.Parameters.AddWithValue(@"@finw", Convert.ToDouble(_pp[@"WIDTH"].Data));
						comm.Parameters.AddWithValue(@"@thk", Convert.ToDouble(_pp[@"THICKNESS"].Data));
						comm.Parameters.AddWithValue(@"@hash", Convert.ToInt32(_pp.Hash));
						if (ta_.Connection.State == System.Data.ConnectionState.Closed) {
							ta_.Connection.Open();
						}
						try {
							affected_ = comm.ExecuteNonQuery();
						} finally {
							ta_.Connection.Close();
						}
					}
				}
				return affected_;
			}

			private int update_part_(SwProperties _pp) {
				int affected_ = 0;
				using (ENGINEERINGDataSetTableAdapters.CUT_PARTSTableAdapter ta_ =
					new ENGINEERINGDataSetTableAdapters.CUT_PARTSTableAdapter()) {
					string sql_ = @"UPDATE CUT_PARTS SET DESCR = @descr, FIN_L = @finl, FIN_W = @finw, THICKNESS = @thk, " +
						@"CNC1 = @cnc1, CNC2 = @cnc2, BLANKQTY = @blnkqty, " +
						@"OVER_L = @ovrl, OVER_W = @ovrw, " +
						//"OP1ID = @op1, OP2ID = @op2, OP3ID = @op3, OP4ID = @op4, OP5ID = @op5, " +
						"COMMENT = @comment, HASH = @hash,  UPDATE_CNC = @updCnc, TYPE = @type WHERE PARTNUM=@prtNo";
					using (SqlCommand comm = new SqlCommand(sql_, ta_.Connection)) {
						int descr_limit_ = new ENGINEERINGDataSet.CUT_PARTSDataTable().DESCRColumn.MaxLength;
						if (_pp[@"Description"].Data.ToString().Length > descr_limit_) {
							comm.Parameters.AddWithValue("@descr", _pp[@"Description"].Data.ToString().Substring(0, descr_limit_));
						} else {
							comm.Parameters.AddWithValue("@descr", _pp[@"Description"].Data);
						}
						comm.Parameters.AddWithValue("@finl", Convert.ToDouble(_pp[@"LENGTH"].Data));
						comm.Parameters.AddWithValue("@finw", Convert.ToDouble(_pp[@"WIDTH"].Data));
						comm.Parameters.AddWithValue("@thk", Convert.ToDouble(_pp[@"THICKNESS"].Data));
						comm.Parameters.AddWithValue("@cnc1", Convert.ToString(_pp[@"CNC1"].Data));
						comm.Parameters.AddWithValue("@cnc2", Convert.ToString(_pp[@"CNC2"].Data));
						comm.Parameters.AddWithValue("@blnkqty", Convert.ToInt32(_pp[@"BLANK QTY"].Data));
						comm.Parameters.AddWithValue("@ovrl", Convert.ToDouble(_pp[@"OVERL"].Data));
						comm.Parameters.AddWithValue("@ovrw", Convert.ToDouble(_pp[@"OVERW"].Data));

						//for (ushort i = 0; i < Properties.Settings.Default.OpCount; i++) {
						//	int seq_ = i + 1;
						//	string key_ = string.Format(@"@op{0}", seq_);
						//	string lkup_ = string.Format(@"OP{0}", seq_);
						//	comm.Parameters.AddWithValue(key_, Convert.ToInt32(_pp[lkup_].Data));
						//}

						comm.Parameters.AddWithValue("@comment", _pp[@"COMMENT"].Data);
						comm.Parameters.AddWithValue("@updCnc", ((bool)_pp[@"UPDATE CNC"].Data ? 1 : 0));
						comm.Parameters.AddWithValue("@type", Convert.ToInt32(_pp[@"DEPARTMENT"].Data));
						comm.Parameters.AddWithValue("@prtNo", _pp.PartLookup);
						comm.Parameters.AddWithValue("@hash", Convert.ToInt32(_pp.Hash));
						if (ta_.Connection.State == System.Data.ConnectionState.Closed) {
							ta_.Connection.Open();
						}
						try {
							affected_ = comm.ExecuteNonQuery();
						} finally {
							ta_.Connection.Close();
						}
					}
					object id_ = ta_.GetPartIDByPartnum(_pp.PartLookup);
					return id_ != null ? (int)id_ : 0;
				}
			}

			private int insert_part_(SwProperties _pp) {
				int affected_ = 0;
				using (ENGINEERINGDataSetTableAdapters.CUT_PARTSTableAdapter ta_ =
					new ENGINEERINGDataSetTableAdapters.CUT_PARTSTableAdapter()) {
					string sql_ = @"INSERT INTO CUT_PARTS (PARTNUM, DESCR, FIN_L, FIN_W, THICKNESS, CNC1, CNC2, BLANKQTY, OVER_L,  OVER_W, " +
						//"OP1ID, OP2ID, OP3ID, OP4ID, OP5ID, " + 
						@"COMMENT, UPDATE_CNC, TYPE, HASH) VALUES " +
						@"(@prtNo, @descr, @finl, @finw, @thk, @cnc1, " +
						@"@cnc2, @blnkqty, @ovrl, @ovrw, " +
						//"@op1, @op2, @op3, @op4, @op5, " +
						@"@comment, @updCnc, @type, @hash)";
					using (SqlCommand comm = new SqlCommand(sql_, ta_.Connection)) {
						comm.Parameters.AddWithValue("@descr", _pp[@"Description"].Data);
						comm.Parameters.AddWithValue("@finl", Convert.ToDouble(_pp[@"LENGTH"].Data));
						comm.Parameters.AddWithValue("@finw", Convert.ToDouble(_pp[@"WIDTH"].Data));
						comm.Parameters.AddWithValue("@thk", Convert.ToDouble(_pp[@"THICKNESS"].Data));
						comm.Parameters.AddWithValue("@cnc1", Convert.ToString(_pp[@"CNC1"].Data));
						comm.Parameters.AddWithValue("@cnc2", Convert.ToString(_pp[@"CNC2"].Data));
						comm.Parameters.AddWithValue("@blnkqty", Convert.ToInt32(_pp[@"BLANK QTY"].Data));
						comm.Parameters.AddWithValue("@ovrl", Convert.ToDouble(_pp[@"OVERL"].Data));
						comm.Parameters.AddWithValue("@ovrw", Convert.ToDouble(_pp[@"OVERW"].Data));

						//for (ushort i = 0; i < Properties.Settings.Default.OpCount; i++) {
						//	int seq_ = i + 1;
						//	string key_ = string.Format(@"@op{0}", seq_);
						//	string lkup_ = string.Format(@"OP{0}", seq_);
						//	comm.Parameters.AddWithValue(key_, Convert.ToInt32(_pp[lkup_].Data));
						//}

						comm.Parameters.AddWithValue("@comment", _pp[@"COMMENT"].Data);
						comm.Parameters.AddWithValue("@updCnc", ((bool)_pp[@"UPDATE CNC"].Data ? 1 : 0));
						comm.Parameters.AddWithValue("@type", Convert.ToInt32(_pp[@"DEPARTMENT"].Data));
						comm.Parameters.AddWithValue("@prtNo", _pp.PartLookup);
						comm.Parameters.AddWithValue("@hash", Convert.ToInt32(_pp.Hash));
						if (ta_.Connection.State == System.Data.ConnectionState.Closed) {
							ta_.Connection.Open();
						}
						try {
							affected_ = comm.ExecuteNonQuery();
						} finally {
							ta_.Connection.Close();
						}
					}
					object id_ = ta_.GetPartIDByPartnum(_pp.PartLookup);
					return id_ != null ? (int)id_ : 0;
				}
			}
		}

		partial class CUT_CUTLISTSDataTable {
			/// <summary>
			/// Insert all the data for a complete cutlist.
			/// </summary>
			/// <param name="_itemNo">Item name.</param>
			/// <param name="_drawing">Reference drawing.</param>
			/// <param name="_rev">Rev number.</param>
			/// <param name="_descr">Description of the item.</param>
			/// <param name="_custid">Customer ID from GEN_CUSTOMERS.</param>
			/// <param name="_date">Create date.</param>
			/// <param name="_state">State from CUT_STATES to start in.</param>
			/// <param name="_auth">User ID from GEN_USERS</param>
			/// <param name="_ppp">A dictionary of properties.</param>
			/// <returns></returns>
			public int UpdateCutlist(string _itemNo, string _drawing, string _rev, string _descr,
				int _custid, DateTime _date, int _state, int _auth, List<SwProperties> _ppp) {
				int affected_ = 0;

				empty_cutlist_(_itemNo, _rev);
				int clid_ = update_cutlist_(_itemNo, _drawing, _rev, _descr, _custid, _date, _state, _auth, _ppp);
				if (clid_ < 1)
					clid_ = insert_cutlist_(_itemNo, _drawing, _rev, _descr, _custid, _date, _state, _auth, _ppp);

				if (clid_ > 0) {
					using (CUT_PARTSDataTable pdt_ = new CUT_PARTSDataTable()) {
						using (CUT_CUTLIST_PARTSDataTable cpdt_ = new CUT_CUTLIST_PARTSDataTable()) {
							using (CUT_PART_OPSDataTable cpot_ = new CUT_PART_OPSDataTable()) {
								foreach (SwProperties pp_ in _ppp) {
									pp_.CutlistID = clid_;
									int partID_ = pdt_.UpdatePart(pp_);
									pp_.PartID = partID_;
									cpdt_.UpdateCutlistPart(pp_);
									cpot_.UpdateOps(pp_);
								}
							}
						}
					}
				}
				return affected_;
			}

			private int empty_cutlist_(string _itemNum, string _rev) {
				int affected_ = 0;
				using (ENGINEERINGDataSetTableAdapters.CUT_CUTLISTSTableAdapter ta_ =
					new ENGINEERINGDataSetTableAdapters.CUT_CUTLISTSTableAdapter()) {
					int clid_ = Convert.ToInt32(ta_.GetCutlistID(_itemNum, _rev));

					string SQL = @"DELETE FROM CUT_CUTLIST_PARTS WHERE CLID = @clid";

					if (clid_ > 0) {
						using (SqlCommand comm_ = new SqlCommand(SQL, ta_.Connection)) {
							comm_.Parameters.AddWithValue("@clid", clid_);
							if (ta_.Connection.State == System.Data.ConnectionState.Closed) {
								ta_.Connection.Open();
							}
							try {
								affected_ = comm_.ExecuteNonQuery();
							} finally {
								ta_.Connection.Close();
							}
						}
					}
				}
				return affected_;
			}

			private int insert_cutlist_(string _itemNo, string _drawing, string _rev, string _descr,
				int _custid, DateTime _date, int _state, int _auth, List<SwProperties> _ppp) {
				int affected_ = 0;
				using (ENGINEERINGDataSetTableAdapters.CUT_CUTLISTSTableAdapter ta_ =
					new ENGINEERINGDataSetTableAdapters.CUT_CUTLISTSTableAdapter()) {
					string sql_ = @"INSERT INTO CUT_CUTLISTS (PARTNUM, REV, DRAWING, CUSTID, CDATE, DESCR, " +
						@"SETUP_BY, STATE_BY, STATEID) VALUES (@itemno, @rev, @drawing, @custid, @date, @descr, @setupby, @stateby, @stateid)";
					using (SqlCommand comm = new SqlCommand(sql_, ta_.Connection)) {
						comm.Parameters.AddWithValue(@"@itemno", _itemNo);
						comm.Parameters.AddWithValue(@"@rev", _rev);
						comm.Parameters.AddWithValue(@"@drawing", _drawing);
						comm.Parameters.AddWithValue(@"@custid", _custid);
						comm.Parameters.AddWithValue(@"@date", _date);
						comm.Parameters.AddWithValue(@"@descr", _descr);
						comm.Parameters.AddWithValue(@"@setupby", _auth);
						comm.Parameters.AddWithValue(@"@stateby", _auth);
						comm.Parameters.AddWithValue(@"@stateid", _state);
						if (ta_.Connection.State == System.Data.ConnectionState.Closed) {
							ta_.Connection.Open();
						}
						try {
							affected_ = comm.ExecuteNonQuery();
						} finally {
							ta_.Connection.Close();
						}
					}
					object id_ = ta_.GetCutlistID(_itemNo, _rev);
					return id_ != null ? (int)id_ : 0;
				}
			}

			private int update_cutlist_(string _itemNo, string _drawing, string _rev, string _descr,
				int _custid, DateTime _date, int _state, int _auth, List<SwProperties> _ppp) {
				int affected_ = 0;
				using (ENGINEERINGDataSetTableAdapters.CUT_CUTLISTSTableAdapter ta_ =
					new ENGINEERINGDataSetTableAdapters.CUT_CUTLISTSTableAdapter()) {
					string sql_ = @"UPDATE CUT_CUTLISTS SET DRAWING = @drawing, CUSTID = @custid, CDATE = @date, DESCR = @descr, " +
						@"SETUP_BY = @setupby WHERE PARTNUM=@itemno AND REV=@rev;";
					using (SqlCommand comm = new SqlCommand(sql_, ta_.Connection)) {
						comm.Parameters.AddWithValue(@"@drawing", _drawing);
						comm.Parameters.AddWithValue(@"@custid", _custid);
						comm.Parameters.AddWithValue(@"@date", _date);
						comm.Parameters.AddWithValue(@"@descr", _descr);
						comm.Parameters.AddWithValue(@"@setupby", _auth);
						//comm.Parameters.AddWithValue(@"@stateby", _auth);
						//comm.Parameters.AddWithValue(@"@stateid", _state);
						comm.Parameters.AddWithValue(@"@itemno", _itemNo);
						comm.Parameters.AddWithValue(@"@rev", _rev);
						if (ta_.Connection.State == System.Data.ConnectionState.Closed) {
							ta_.Connection.Open();
						}
						try {
							affected_ = comm.ExecuteNonQuery();
						} finally {
							ta_.Connection.Close();
						}
					}
					object id_ = ta_.GetCutlistID(_itemNo, _rev);
					return id_ != null ? (int)id_ : 0;
				}
			}

			private int[] get_orphan_parts_() {
				List<int> parts_ = new List<int>();
				using (ENGINEERINGDataSetTableAdapters.CUT_CUTLISTSTableAdapter ta_ =
					new ENGINEERINGDataSetTableAdapters.CUT_CUTLISTSTableAdapter()) {
					string sql_ = @"SELECT CUT_PARTS.PARTID FROM CUT_PARTS LEFT JOIN CUT_CUTLIST_PARTS " +
						@"ON CUT_PARTS.PARTID = CUT_CUTLIST_PARTS.PARTID GROUP BY CUT_PARTS.PARTID " +
						@"HAVING (((Count(CUT_CUTLIST_PARTS.CLPARTID))=0));";
					using (SqlCommand comm = new SqlCommand(sql_, ta_.Connection)) {
						if ((ta_.Connection.State & System.Data.ConnectionState.Closed)
							== System.Data.ConnectionState.Closed) {
							ta_.Connection.Open();
						}
						try {
							using (SqlDataReader dr_ = comm.ExecuteReader()) {
								while (dr_.Read()) {
									parts_.Add(dr_.GetInt32(0));
								}
							}
						} finally {
							ta_.Connection.Close();
						}
					}
				}
				return parts_.ToArray();
			}

			/// <summary>
			/// Delete cutlist, then delete any orphaned parts left over.
			/// </summary>
			/// <param name="_clid">A cutlist ID.</param>
			public void DeleteCutlist(int _clid) {
				int affected_ = 0;
				using (ENGINEERINGDataSetTableAdapters.CUT_CUTLIST_PARTSTableAdapter cp_ =
					new ENGINEERINGDataSetTableAdapters.CUT_CUTLIST_PARTSTableAdapter()) {
					affected_ = cp_.DeleteByCLID(_clid);
				}
				if (affected_ > 0) {
					using (ENGINEERINGDataSetTableAdapters.CUT_CUTLISTSTableAdapter c_ =
						new ENGINEERINGDataSetTableAdapters.CUT_CUTLISTSTableAdapter()) {
						affected_ += c_.DeleteByCLID(_clid);
					}
				}

				int[] orp_ = get_orphan_parts_();
				string q_ = string.Format(@"{0} orphaned parts were found. They should also be deleted. Continue?", orp_.Length);
				System.Windows.Forms.DialogResult dr_ = System.Windows.Forms.MessageBox.Show(q_, @"Delete orphaned parts?",
					System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);
				if (dr_ == System.Windows.Forms.DialogResult.Yes) {
					using (ENGINEERINGDataSetTableAdapters.CUT_PART_OPSTableAdapter pota_ =
						new ENGINEERINGDataSetTableAdapters.CUT_PART_OPSTableAdapter()) {
						using (ENGINEERINGDataSetTableAdapters.CUT_PARTSTableAdapter p_ =
							new ENGINEERINGDataSetTableAdapters.CUT_PARTSTableAdapter()) {
							foreach (int part_ in orp_) {
								pota_.DeletePart(part_);
								p_.DeletePart(part_);
							}
						}
					}
				}
			}
		}

		partial class CUT_CUTLIST_PARTSDataTable {
			/// <summary>
			/// Update cutlist specific stuff.
			/// </summary>
			/// <param name="_pp">An SwProperties object.</param>
			/// <returns>Number of rows affected. It should never be more than 1.</returns>
			public int UpdateCutlistPart(SwProperties _pp) {
				int affected_ = update_cutlist_part_(_pp);
				return affected_ > 0 ? affected_ : insert_cutlist_part_(_pp);
			}

			private int update_cutlist_part_(SwProperties _pp) {
				int affected_ = 0;
				Properties.Settings.Default.LastCutlist = _pp.CutlistID;
				using (ENGINEERINGDataSetTableAdapters.CUT_CUTLIST_PARTSTableAdapter ta_ =
					new ENGINEERINGDataSetTableAdapters.CUT_CUTLIST_PARTSTableAdapter()) {
					string sql_ = @"UPDATE CUT_CUTLIST_PARTS " +
						@"SET MATID = @matid, EDGEID_LF = @efid, EDGEID_LB = @ebid, EDGEID_WR = @erid, EDGEID_WL = @elid, " +
						@"QTY = @qty WHERE CLID = @clid AND PARTID = @partid";
					using (SqlCommand comm = new SqlCommand(sql_, ta_.Connection)) {
						comm.Parameters.AddWithValue(@"@matid", Convert.ToInt32(_pp[@"CUTLIST MATERIAL"].Data));
						comm.Parameters.AddWithValue(@"@efid", Convert.ToInt32(_pp[@"EDGE FRONT (L)"].Data));
						comm.Parameters.AddWithValue(@"@ebid", Convert.ToInt32(_pp[@"EDGE BACK (L)"].Data));
						comm.Parameters.AddWithValue(@"@elid", Convert.ToInt32(_pp[@"EDGE LEFT (W)"].Data));
						comm.Parameters.AddWithValue(@"@erid", Convert.ToInt32(_pp[@"EDGE RIGHT (W)"].Data));
						comm.Parameters.AddWithValue(@"@qty", _pp.CutlistQty);
						comm.Parameters.AddWithValue(@"@clid", _pp.CutlistID);
						comm.Parameters.AddWithValue(@"@partid", Convert.ToInt32(_pp.PartID));
						if (ta_.Connection.State == System.Data.ConnectionState.Closed) {
							ta_.Connection.Open();
						}
						try {
							affected_ = comm.ExecuteNonQuery();
						} finally {
							ta_.Connection.Close();
						}
					}
				}
				return affected_;
			}

			private int insert_cutlist_part_(SwProperties _pp) {
				int affected_ = 0;
				Properties.Settings.Default.LastCutlist = _pp.CutlistID;
				string sql_ = @"INSERT INTO CUT_CUTLIST_PARTS (CLID, PARTID, MATID, EDGEID_LF, EDGEID_LB, EDGEID_WR, EDGEID_WL, QTY) " +
					@"VALUES (@clid, @partid, @matid, @efid, @ebid, @erid, @elid, @qty)";
				using (ENGINEERINGDataSetTableAdapters.CUT_CUTLIST_PARTSTableAdapter ta_ =
					new ENGINEERINGDataSetTableAdapters.CUT_CUTLIST_PARTSTableAdapter()) {
					using (SqlCommand comm = new SqlCommand(sql_, ta_.Connection)) {
						comm.Parameters.AddWithValue(@"@clid", _pp.CutlistID);
						comm.Parameters.AddWithValue(@"@partid", _pp.PartID);
						comm.Parameters.AddWithValue(@"@matid", Convert.ToInt32(_pp[@"CUTLIST MATERIAL"].Data));
						comm.Parameters.AddWithValue(@"@efid", Convert.ToInt32(_pp[@"EDGE FRONT (L)"].Data));
						comm.Parameters.AddWithValue(@"@ebid", Convert.ToInt32(_pp[@"EDGE BACK (L)"].Data));
						comm.Parameters.AddWithValue(@"@elid", Convert.ToInt32(_pp[@"EDGE LEFT (W)"].Data));
						comm.Parameters.AddWithValue(@"@erid", Convert.ToInt32(_pp[@"EDGE RIGHT (W)"].Data));
						comm.Parameters.AddWithValue(@"@qty", _pp.CutlistQty);
						if (ta_.Connection.State == System.Data.ConnectionState.Closed) {
							ta_.Connection.Open();
						}
						try {
							affected_ = comm.ExecuteNonQuery();
						} finally {
							ta_.Connection.Close();
						}
					}
				}
				return affected_;
			}

			/// <summary>
			/// Delete part from a selected cutlist. If the part is used nowhere else,
			/// then delete it, and its child rows.
			/// </summary>
			/// <param name="_pp">An SwProperties object with valid CutlistID and PartID.</param>
			/// <returns>Number of rows affected.</returns>
			public int RemovePartFromCutlist(SwProperties _pp) {
				int affected_ = 0;
				if (_pp.CutlistAndPartIDsOK) {
					using (ENGINEERINGDataSetTableAdapters.CUT_CUTLIST_PARTSTableAdapter cpta_ =
						new ENGINEERINGDataSetTableAdapters.CUT_CUTLIST_PARTSTableAdapter()) {
						affected_ = cpta_.DeleteCutlistPart(_pp.CutlistID, _pp.PartID);
						bool part_not_used_ = Convert.ToInt32(cpta_.CutlistPartsCount(_pp.PartID)) < 1;
						if (part_not_used_ && affected_ > 0) {
							using (ENGINEERINGDataSetTableAdapters.CUT_PART_OPSTableAdapter pota_ =
								new ENGINEERINGDataSetTableAdapters.CUT_PART_OPSTableAdapter()) {
								using (ENGINEERINGDataSetTableAdapters.CUT_PARTSTableAdapter p_ =
									new ENGINEERINGDataSetTableAdapters.CUT_PARTSTableAdapter()) {
									affected_ += pota_.DeletePart(_pp.PartID);
									affected_ += p_.DeletePart(_pp.PartID);
								}
							}
						}
					}
				}
				return affected_;
			}
		}

		partial class GEN_DRAWINGSDataTable {
			/// <summary>
			/// Return a location or null from GEN_DRAWINGS.
			/// </summary>
			/// <param name="partno">A part number.</param>
			/// <returns>A FileInfo object.</returns>
			public System.IO.FileInfo GetPDFLocation(string partno) {
				GEN_DRAWINGSDataTable dt = GetPDFData(partno);
				if (dt.Rows.Count > 0) {
					System.Data.DataRow r = dt.Rows[0];
					System.IO.FileInfo f = new System.IO.FileInfo(
						string.Format("{0}{1}",
							r["FPath"].ToString(),
							r["FName"].ToString()));
					return f;
				}
				return null;
			}

			/// <summary>
			/// Return a table of data from GEN_DRAWINGS. If the search term starts with a Z, then
			/// monkey with it because of the descriptive stuff often included in Z-style filenames.
			/// </summary>
			/// <param name="partno">A part number.</param>
			/// <returns>A table of data, hopefully with only one row of useful data.</returns>
			public GEN_DRAWINGSDataTable GetPDFData(string partno) {
				using (ENGINEERINGDataSetTableAdapters.GEN_DRAWINGSTableAdapter gd =
					new ENGINEERINGDataSetTableAdapters.GEN_DRAWINGSTableAdapter()) {
					string lookup = string.Format(@"{0}.PDF", partno);
					if (partno.StartsWith(@"Z")) {
						lookup = string.Format(@"{0}%.PDF", partno);
					}
					return gd.GetDataByFName(lookup);
				}
			}

			/// <summary>
			/// A union query of GEN_DRAWINGS, and GEN_DRAWINGS_MTL.
			/// </summary>
			/// <param name="partno">A part number.</param>
			/// <returns>Hopefully, no more than two rows of useful data.</returns>
			public GEN_DRAWINGSDataTable GetAllPDFData(string partno) {
				using (ENGINEERINGDataSetTableAdapters.GEN_DRAWINGSTableAdapter gd =
					new ENGINEERINGDataSetTableAdapters.GEN_DRAWINGSTableAdapter()) {
					string lookup = string.Format(@"{0}%.PDF", partno);
					using (GEN_DRAWINGSDataTable dt = gd.GetDrwgDataByFName(lookup, lookup)) {
						return dt;
					}
				}
			}
		}

		partial class inmastDataTable {
			/// <summary>
			/// Get the ECR_ITEM_TYPE of a given part.
			/// </summary>
			/// <param name="prtno">A part number.</param>
			/// <param name="prtrv">A rev value.</param>
			/// <returns>An int of ITEMTYPE_ID from ECR_ITEM_TYPE.</returns>
			public int GetECRItemType(string prtno, string prtrv) {
				using (ENGINEERINGDataSetTableAdapters.inmastTableAdapter ita =
					new ENGINEERINGDataSetTableAdapters.inmastTableAdapter()) {
					string prdclass = ita.GetProductClass(prtno, prtrv);
					int parttype = 7;
					if (prdclass != null) {
						switch (prdclass) {
							case "01":
								if (prtno.StartsWith("Z"))
									parttype = 1;
								else
									parttype = 3;
								break;
							case "02":
								parttype = 3;
								break;
							case "03":
								if (prtno.StartsWith("Z"))
									parttype = 1;
								else
									parttype = 3;
								break;
							case "04":
								if (ita.GetPurchased(prtno, prtrv) == @"Y")
									parttype = 4;
								else
									parttype = 3;
								break;
							case "06":
								if (ita.GetPurchased(prtno, prtrv) == @"Y")
									parttype = 4;
								else
									parttype = 3;
								break;
							case "09":
								if (ita.GetPurchased(prtno, prtrv) == @"Y")
									parttype = 2;
								else
									parttype = 1;
								break;
							case "10":
								parttype = 4;
								break;
							default:
								parttype = 7;
								break;
						}
					} else {
						using (ENGINEERINGDataSetTableAdapters.CUT_CUTLISTSTableAdapter ccl =
							new ENGINEERINGDataSetTableAdapters.CUT_CUTLISTSTableAdapter()) {
							if (ccl.GetDataByName(prtno, prtrv).Rows.Count > 0) {
								parttype = 5;
							} else {
								using (ENGINEERINGDataSetTableAdapters.CUT_PARTSTableAdapter cp =
									new ENGINEERINGDataSetTableAdapters.CUT_PARTSTableAdapter()) {
									if (cp.GetDataByPartnum(prtno).Rows.Count > 0)
										parttype = 6;
								}
							}
						}
					}
					return parttype;
				}
			}
		}

		partial class ECR_ITEMSDataTable {
			/// <summary>
			/// Check if this is an existing ECR item.
			/// </summary>
			/// <param name="ecrno">An ECR number.</param>
			/// <param name="partnum">A part number.</param>
			/// <param name="rev">A revision number.</param>
			/// <returns>True or false.</returns>
			public bool ECRItemExists(int ecrno, string partnum, string rev) {
				bool exists = false;
				using (ENGINEERINGDataSetTableAdapters.ECR_ITEMSTableAdapter eita =
					new ENGINEERINGDataSetTableAdapters.ECR_ITEMSTableAdapter()) {
					object r = eita.GetECRItem(ecrno, partnum, rev);
					if (r != null) {
						exists = true;
					}
				}
				return exists;
			}
		}

		partial class ECRObjLookupDataTable {
			/// <summary>
			/// Is this a valid, existing number?
			/// </summary>
			/// <param name="econumber">An ECR number.</param>
			/// <returns>True or false.</returns>
			public bool ECRIsBogus(int econumber) {
				bool bogus = true;
				using (ENGINEERINGDataSetTableAdapters.ECRObjLookupTableAdapter eolta =
					new ENGINEERINGDataSetTableAdapters.ECRObjLookupTableAdapter()) {
					if (eolta.GetDataByECO(econumber).Rows.Count > 0) {
						bogus = false;
					}
				}
				return bogus;
			}
		}

		partial class CUT_EDGESDataTable {
			/// <summary>
			/// Resolve an edge id from an old old description string.
			/// </summary>
			/// <param name="descr">A description string.</param>
			/// <returns>An int ID from CUT_EDGES.</returns>
			public int GetEdgeIDByDescr(string descr) {
				int id = -1;
				if (descr != string.Empty) {
					using (ENGINEERINGDataSetTableAdapters.CUT_EDGESTableAdapter ceta =
						new ENGINEERINGDataSetTableAdapters.CUT_EDGESTableAdapter()) {
						using (ENGINEERINGDataSetTableAdapters.CUT_EDGES_XREFTableAdapter cexta =
							new ENGINEERINGDataSetTableAdapters.CUT_EDGES_XREFTableAdapter()) {
							id = Convert.ToInt32(cexta.GetEdgeID(Convert.ToString(descr)));
							if (id < 1) {
								id = Convert.ToInt32(ceta.GetEdgeID(descr));
							}
						}
					}
				}
				return id;
			}
		}

		partial class CUT_MATERIALSDataTable {
			/// <summary>
			/// Look up last cost of all M2M edging matching the cutlist edging.
			/// </summary>
			/// <param name="edgeid">The EDGEID from the cutlist database.</param>
			/// <returns>A <see cref="List{String}"/> of M2M edges.</returns>
			public List<string> GetEdgePricing(int edgeid) {
				List<string> l_ = new List<string>();
				using (ENGINEERINGDataSetTableAdapters.CUT_EDGESTableAdapter ceta_ =
					new ENGINEERINGDataSetTableAdapters.CUT_EDGESTableAdapter()) {
					using (ENGINEERINGDataSetTableAdapters.inmast1TableAdapter ita_ =
						new ENGINEERINGDataSetTableAdapters.inmast1TableAdapter()) {
						ENGINEERINGDataSet.CUT_EDGESDataTable dt_ = ceta_.GetDataByEdgeID(edgeid);
						for (int i = 0; i < dt_.Rows.Count; i++) {
							if (!dt_[i].IsMATIDNull() && dt_[i].MATID > 0) {
								l_.AddRange(GetMaterialPricing(dt_[i].MATID));
							} else {
								string sql_ = @"SELECT fpartno, fdescript, flastcost, fonhand, fonorder, fmeasure FROM inmast WHERE " +
									string.Format(@"fpartno = '{0}'", dt_[i].PARTNUM);
								using (SqlCommand comm_ = new SqlCommand(sql_, ita_.Connection)) {
									if (ita_.Connection.State == System.Data.ConnectionState.Closed) {
										ita_.Connection.Open();
									}
									try {
										using (SqlDataReader dr_ = comm_.ExecuteReader()) {
											while (dr_.Read()) {
												l_.Add(
													string.Format("{0}: '{1}' last purchsed for {2:C}/{5}.\n\t{3:0.0} {5} on hand, {4:0.0} {5} on order.",
													dr_.GetString(0).Trim(), dr_.GetString(1).Trim(),
													dr_.GetDecimal(2), dr_.GetDecimal(3), dr_.GetDecimal(4),
													dr_.GetString(5).Trim()));
											}
										}
									} finally {
										ita_.Connection.Close();
									}
								}
							}
						}
					}
				}
				return l_;
			}

			/// <summary>
			/// Look up the last cost of all M2M materials matching the cutlist material.
			/// </summary>
			/// <param name="matid">The MATID from the cutlist database.</param>
			/// <returns>A <see cref="List{String}"/> of M2M materials.</returns>
			public List<string> GetMaterialPricing(int matid) {
				List<string> l_ = new List<string>();
				bool fail_ = false;
				using (ENGINEERINGDataSetTableAdapters.CUT_MATERIAL_SIZESTableAdapter cmsta_ =
					new ENGINEERINGDataSetTableAdapters.CUT_MATERIAL_SIZESTableAdapter()) {
					using (ENGINEERINGDataSetTableAdapters.inmast1TableAdapter ita_ =
						new ENGINEERINGDataSetTableAdapters.inmast1TableAdapter()) {
						ENGINEERINGDataSet.CUT_MATERIAL_SIZESDataTable msdt_ =
							cmsta_.GetDataByMatID(matid);
						string sql_ = @"SELECT fpartno, fdescript, flastcost, fonhand, fonorder, fmeasure FROM inmast WHERE ";
						for (int i = 0; i < msdt_.Count; i++) {
							if (msdt_[i].PARTNUM.Trim() != string.Empty) {
								sql_ += string.Format(@"fpartno = '{0}' ", msdt_[i].PARTNUM);
								if (i < msdt_.Count - 1) {
									sql_ += @"OR ";
								}
							} else {
								fail_ = true;
							}
						}
						if (!fail_) {
							using (SqlCommand comm_ = new SqlCommand(sql_, ita_.Connection)) {
								if (ita_.Connection.State == System.Data.ConnectionState.Closed) {
									ita_.Connection.Open();
								}
								try {
									using (SqlDataReader dr_ = comm_.ExecuteReader()) {
										while (dr_.Read()) {
											l_.Add(
												string.Format("{0}: '{1}' last purchsed for {2:C}/{5}.\n\t {3:0.0} {5} on hand, {4:0.0} {5} on order.",
												dr_.GetString(0).Trim(), dr_.GetString(1).Trim(),
												dr_.GetDecimal(2), dr_.GetDecimal(3), dr_.GetDecimal(4),
												dr_.GetString(5).Trim()));
										}
									}
								} finally {
									ita_.Connection.Close();
								}
							}
						}
					}
				}
				return l_;
			}
			/// <summary>
			/// Find an ID from a material description.
			/// </summary>
			/// <param name="descr">A material description.</param>
			/// <returns>An int ID, or -1 if nothing is found.</returns>
			public int GetMaterialIDByDescr(string descr) {
				if (descr != string.Empty) {
					using (ENGINEERINGDataSetTableAdapters.CUT_MATERIALSTableAdapter cmta =
						new ENGINEERINGDataSetTableAdapters.CUT_MATERIALSTableAdapter()) {
						using (CUT_MATERIALSDataTable dt = cmta.GetDataByDescr(descr)) {
							if (dt.Rows.Count > 0)
								return dt[0].MATID;
						}
					}
				}
				return -1;
			}

			/// <summary>
			/// Get a row of data from a material ID.
			/// </summary>
			/// <param name="id">A material ID.</param>
			/// <returns>A CUT_MATERIALSRow or null.</returns>
			public CUT_MATERIALSRow GetMaterial(int id) {
				using (ENGINEERINGDataSetTableAdapters.CUT_MATERIALSTableAdapter cmta =
					new ENGINEERINGDataSetTableAdapters.CUT_MATERIALSTableAdapter()) {
					using (CUT_MATERIALSDataTable dt = cmta.GetDataByMatID(id)) {
						if (dt.Rows.Count > 0)
							return dt[0];
					}
				}
				return null;
			}
		}

		partial class SCH_PROJECTSDataTable {
			/// <summary>
			/// Resolve a four-character project code to a proper customer. I get the whole row
			/// so I can use any additional info.
			/// </summary>
			/// <param name="partLookup">A part number to look up.</param>
			/// <returns>A SCH_PROJECTSRow or null.</returns>
			public SCH_PROJECTSRow GetCorrectCustomer(string partLookup) {
				string pattern = @"([A-Z]{3,4})(\d{4})";
				System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(pattern);
				System.Text.RegularExpressions.Match matches = System.Text.RegularExpressions.Regex.Match(partLookup, pattern);
				if (r.IsMatch(partLookup)) {
					using (ENGINEERINGDataSetTableAdapters.SCH_PROJECTSTableAdapter spta =
						new ENGINEERINGDataSetTableAdapters.SCH_PROJECTSTableAdapter()) {
						using (SCH_PROJECTSDataTable dt_ = spta.GetDataByProject(matches.Groups[1].ToString())) {
							if (dt_.Count > 0) {
								SCH_PROJECTSRow row = spta.GetDataByProject(matches.Groups[1].ToString())[0];
								return row;
							}
						}
					}
				}
				return null;
			}
		}

		partial class GEN_ODOMETERDataTable {
			/// <summary>
			/// Increment my function odometer.
			/// </summary>
			/// <param name="func">A value from the enum Redbrick.Functions.</param>
			public void IncrementOdometer(Redbrick.Functions func) {
				using (ENGINEERINGDataSetTableAdapters.GEN_ODOMETERTableAdapter gota =
					new ENGINEERINGDataSetTableAdapters.GEN_ODOMETERTableAdapter()) {
					using (ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter guta =
						new ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter()) {
						int rowsAffected = 0;
						int uid = (int)guta.GetUID(Environment.UserName);
						string SQL = @"UPDATE GEN_ODOMETER SET ODO = ODO + 1 WHERE (FUNCID = @funcid AND USERID = @userid);";
						if (gota.Connection.State != System.Data.ConnectionState.Open) {
							gota.Connection.Open();
						}
						using (SqlCommand comm = new SqlCommand(SQL, gota.Connection)) {
							comm.Parameters.AddWithValue(@"@funcid", func);
							comm.Parameters.AddWithValue(@"@userid", uid);

							try {
								rowsAffected = comm.ExecuteNonQuery();
							} catch (InvalidOperationException ioe) {
								throw ioe;
							}
						}

						if (rowsAffected == 0) {
							SQL = @"INSERT INTO GEN_ODOMETER (ODO, FUNCID, USERID) VALUES (1, @funcid, @userid);";
							using (SqlCommand comm = new SqlCommand(SQL, gota.Connection)) {
								comm.Parameters.AddWithValue(@"@funcid", func);
								comm.Parameters.AddWithValue(@"@userid", uid);

								try {
									rowsAffected = comm.ExecuteNonQuery();
								} catch (InvalidOperationException ioe) {
									throw ioe;
								}
							}
						}
					}
				}
			}
		}
	}
}

