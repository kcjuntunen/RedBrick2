using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RedBrick2 {


	public partial class ENGINEERINGDataSet {
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

			private int update_part_(SwProperties _pp) {
				int affected_ = 0;
				string sql_ = @"UPDATE CUT_PARTS SET DESCR = @descr, FIN_L = @finl, FIN_W = @finw, THICKNESS = @thk, " +
					"CNC1 = cnc1, CNC2 = cnc2, BLANKQTY = @blnkqty, " +
					"OVER_L = @ovrl, OVER_W = @ovrw, OP1ID = @op1, " +
					"OP2ID = @op2, OP3ID = @op3, OP4ID = @op4, OP5ID = @op5, COMMENT = ?, " +
					"UPDATE_CNC = ?, TYPE = ? WHERE PARTNUM=? AND (HASH=? OR HASH IS NULL)";
				ENGINEERINGDataSetTableAdapters.CUT_PARTSTableAdapter ta_ =
					new ENGINEERINGDataSetTableAdapters.CUT_PARTSTableAdapter();
				using (SqlCommand comm = new SqlCommand(sql_, ta_.Connection)) {
					comm.Parameters.AddWithValue("@descr", _pp[@"Description"].Data);
					comm.Parameters.AddWithValue("@finl", Convert.ToDouble(_pp[@"LENGTH"].Data));
					comm.Parameters.AddWithValue("@finw", Convert.ToDouble(_pp[@"WIDTH"].Data));
					comm.Parameters.AddWithValue("@thk", Convert.ToDouble(_pp[@"THICKNESS"].Data));
					comm.Parameters.AddWithValue("@cnc1", _pp[@"CNC1"].Data);
					comm.Parameters.AddWithValue("@cnc2", _pp[@"CNC1"].Data);
					comm.Parameters.AddWithValue("@blnkqty", Convert.ToInt32(_pp[@"BLANK QTY"].Data));
					comm.Parameters.AddWithValue("@ovrl", Convert.ToDouble(_pp[@"OVERL"].Data));
					comm.Parameters.AddWithValue("@ovrw", Convert.ToDouble(_pp[@"OVERW"].Data));

					for (ushort i = 0; i < 5; i++) {
						string lkup_ = string.Format("@op{0}", i + 1);
						comm.Parameters.AddWithValue(lkup_, Convert.ToInt32(_pp[lkup_].Data));
					}

					comm.Parameters.AddWithValue("@comment", _pp[@"COMMENT"].Data);
					comm.Parameters.AddWithValue("@updCnc", ((bool)_pp[@"UPDATE CNC"].Data ? 1 : 0));
					comm.Parameters.AddWithValue("@type", Convert.ToInt32(_pp[@"DEPARTMENT"].Data));
					comm.Parameters.AddWithValue("@prtNo", _pp.PartLookup);
					comm.Parameters.AddWithValue("@hash", Convert.ToInt32(_pp[@"HASH"].Data));
					affected_ = comm.ExecuteNonQuery();
				}
				object id_ = ta_.GetPartIDByPartnum(_pp.PartLookup);
				return id_ != null ? (int)id_ : 0;
			}

			private int insert_part_(SwProperties _pp) {
				int affected_ = 0;
				ENGINEERINGDataSetTableAdapters.CUT_PARTSTableAdapter ta_ =
					new ENGINEERINGDataSetTableAdapters.CUT_PARTSTableAdapter();
				string sql_ = @"INSERT INTO CUT_PARTS (PARTNUM, DESCR, FIN_L, FIN_W, THICKNESS, CNC1, CNC2, BLANKQTY, OVER_L, " +
					@"OVER_W, OP1ID, OP2ID, OP3ID, OP4ID, OP5ID, COMMENT, UPDATE_CNC, TYPE, HASH) VALUES " +
					@"(@prtNo, @descr, @finl, @finw, @thk, @cnc1, " +
					"@cnc2, @blnkqty, @ovrl, @ovrw, @op1, @op2, " +
					"@op3, @op4, @op5, @comment, @updCnc, @type, @hash)";
				using (SqlCommand comm = new SqlCommand(sql_, ta_.Connection)) {
					comm.Parameters.AddWithValue("@descr", _pp[@"Description"].Data);
					comm.Parameters.AddWithValue("@finl", Convert.ToDouble(_pp[@"LENGTH"].Data));
					comm.Parameters.AddWithValue("@finw", Convert.ToDouble(_pp[@"WIDTH"].Data));
					comm.Parameters.AddWithValue("@thk", Convert.ToDouble(_pp[@"THICKNESS"].Data));
					comm.Parameters.AddWithValue("@cnc1", _pp[@"CNC1"].Data);
					comm.Parameters.AddWithValue("@cnc2", _pp[@"CNC1"].Data);
					comm.Parameters.AddWithValue("@blnkqty", Convert.ToInt32(_pp[@"BLANK QTY"].Data));
					comm.Parameters.AddWithValue("@ovrl", Convert.ToDouble(_pp[@"OVERL"].Data));
					comm.Parameters.AddWithValue("@ovrw", Convert.ToDouble(_pp[@"OVERW"].Data));

					for (ushort i = 0; i < Properties.Settings.Default.OpCount; i++) {
						int seq_ = i + 1;
						string key_ = string.Format(@"@op{0}", seq_);
						string lkup_ = string.Format(@"OP{0}", seq_);
						comm.Parameters.AddWithValue(key_, Convert.ToInt32(_pp[lkup_].Data));
					}

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

				int clid_ = update_cutlist_(_itemNo, _drawing, _rev, _descr, _custid, _date, _state, _auth, _ppp);
				if (clid_ < 1)
					clid_ = insert_cutlist_(_itemNo, _drawing, _rev, _descr, _custid, _date, _state, _auth, _ppp);

				if (clid_ > 0) {
					CUT_PARTSDataTable pdt_ = new CUT_PARTSDataTable();
					CUT_CUTLIST_PARTSDataTable cpdt_ = new CUT_CUTLIST_PARTSDataTable();
					foreach (SwProperties pp_ in _ppp) {
						pp_.CutlistID = clid_;
						int partID_ = pdt_.UpdatePart(pp_);
						pp_.PartID = partID_;
						cpdt_.UpdateCutlistPart(pp_);
					}
				}
				return affected_;
			}

			private int insert_cutlist_(string _itemNo, string _drawing, string _rev, string _descr,
				int _custid, DateTime _date, int _state, int _auth, List<SwProperties> _ppp) {
				int affected_ = 0;
				string sql_ = @"INSERT INTO CUT_CUTLISTS (PARTNUM, REV, DRAWING, CUSTID, CDATE, DESCR, " +
					"SETUP_BY, STATE_BY, STATEID) VALUES (@itemno, @rev, @drawing, @custid, @date, @descr, @setupby, @stateby, @stateid)";
				ENGINEERINGDataSetTableAdapters.CUT_CUTLISTSTableAdapter ta_ =
					new ENGINEERINGDataSetTableAdapters.CUT_CUTLISTSTableAdapter();
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
					affected_ = comm.ExecuteNonQuery();
				}
				object id_ = (int)ta_.GetCutlistID(_itemNo, _rev);
				return id_ != null ? (int)id_ : 0;
			}

			private int update_cutlist_(string _itemNo, string _drawing, string _rev, string _descr,
				int _custid, DateTime _date, int _state, int _auth, List<SwProperties> _ppp) {
				int affected_ = 0;
				string sql_ = @"UPDATE CUT_CUTLISTS SET DRAWING = @drawing, CUSTID = @custid, CDATE = @date, DESCR = @descr, " +
					@"SETUP_BY = @setupby, STATE_BY = @stateby, STATEID = @stateid WHERE PARTNUM=@itemno AND REV=@rev;";
				ENGINEERINGDataSetTableAdapters.CUT_CUTLISTSTableAdapter ta_ =
					new ENGINEERINGDataSetTableAdapters.CUT_CUTLISTSTableAdapter();
				using (SqlCommand comm = new SqlCommand(sql_, ta_.Connection)) {
					comm.Parameters.AddWithValue(@"@drawing", _drawing);
					comm.Parameters.AddWithValue(@"@custid", _custid);
					comm.Parameters.AddWithValue(@"@date", _date);
					comm.Parameters.AddWithValue(@"@descr", _descr);
					comm.Parameters.AddWithValue(@"@setupby", _auth);
					comm.Parameters.AddWithValue(@"@stateby", _auth);
					comm.Parameters.AddWithValue(@"@stateid", _state);
					comm.Parameters.AddWithValue(@"@itemno", _itemNo);
					comm.Parameters.AddWithValue(@"@rev", _rev);
					affected_ = comm.ExecuteNonQuery();
				}
				object id_ = (int)ta_.GetCutlistID(_itemNo, _rev);
				return id_ != null ? (int)id_ : 0;
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
				return affected_ > 0 ? affected_ : update_cutlist_part_(_pp);
			}

			private int update_cutlist_part_(SwProperties _pp) {
				int affected = 0;
				Properties.Settings.Default.LastCutlist = _pp.CutlistID;
				string sql_ = @"UPDATE CUT_CUTLIST_PARTS " +
					@"SET MATID = @matid, EDGEID_LF = @efid, EDGEID_LB = @ebid, EDGEID_WR = @erid, EDGEID_WL = @elid, " +
					@"QTY = @qty WHERE CLID = @clid AND PARTID = @partid";
				ENGINEERINGDataSetTableAdapters.CUT_CUTLIST_PARTSTableAdapter cpta_ =
					new ENGINEERINGDataSetTableAdapters.CUT_CUTLIST_PARTSTableAdapter();
				using (SqlCommand comm = new SqlCommand(sql_, cpta_.Connection)) {
					comm.Parameters.AddWithValue(@"@matid", Convert.ToInt32(_pp[@"CUTLIST MATERIAL"].Data));
					comm.Parameters.AddWithValue(@"@efid", Convert.ToInt32(_pp[@"EDGE FRONT (L)"].Data));
					comm.Parameters.AddWithValue(@"@ebid", Convert.ToInt32(_pp[@"EDGE BACK (L)"].Data));
					comm.Parameters.AddWithValue(@"@elid", Convert.ToInt32(_pp[@"EDGE LEFT (W)"].Data));
					comm.Parameters.AddWithValue(@"@erid", Convert.ToInt32(_pp[@"EDGE RIGHT (W)"].Data));
					comm.Parameters.AddWithValue(@"@qty", _pp.CutlistQty);
					comm.Parameters.AddWithValue(@"@clid", _pp.CutlistID);
					comm.Parameters.AddWithValue(@"@partid", Convert.ToInt32(_pp.PartID));
					affected = comm.ExecuteNonQuery();
				}
				return affected;
			}

			private int insert_cutlist_part_(SwProperties _pp) {
				int affected = 0;
				Properties.Settings.Default.LastCutlist = _pp.CutlistID;
				string sql_ = @"INSERT INTO CUT_CUTLIST_PARTS (CLID, PARTID, MATID, EDGEID_LF, EDGEID_LB, EDGEID_WR, EDGEID_WL, QTY) " +
					@"VALUES (@clid, @partid, @matid, @efid, @ebid, @erid, @elid, @qty)";
				ENGINEERINGDataSetTableAdapters.CUT_CUTLIST_PARTSTableAdapter cpta_ =
					new ENGINEERINGDataSetTableAdapters.CUT_CUTLIST_PARTSTableAdapter();
				using (SqlCommand comm = new SqlCommand(sql_, cpta_.Connection)) {
					comm.Parameters.AddWithValue(@"@clid", _pp.CutlistID);
					comm.Parameters.AddWithValue(@"@partid", _pp.PartID);
					comm.Parameters.AddWithValue(@"@matid", Convert.ToInt32(_pp[@"CUTLIST MATERIAL"].Data));
					comm.Parameters.AddWithValue(@"@efid", Convert.ToInt32(_pp[@"EDGE FRONT (L)"].Data));
					comm.Parameters.AddWithValue(@"@ebid", Convert.ToInt32(_pp[@"EDGE BACK (L)"].Data));
					comm.Parameters.AddWithValue(@"@elid", Convert.ToInt32(_pp[@"EDGE LEFT (W)"].Data));
					comm.Parameters.AddWithValue(@"@erid", Convert.ToInt32(_pp[@"EDGE RIGHT (W)"].Data));
					comm.Parameters.AddWithValue(@"@qty", _pp.CutlistQty);
					affected = comm.ExecuteNonQuery();
				}
				return affected;
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
				ENGINEERINGDataSetTableAdapters.GEN_DRAWINGSTableAdapter gd =
					new ENGINEERINGDataSetTableAdapters.GEN_DRAWINGSTableAdapter();
				string lookup = string.Format(@"{0}.PDF", partno);
				if (partno.StartsWith(@"Z")) {
					lookup = string.Format(@"{0}%.PDF", partno);
				}
				return gd.GetDataByFName(lookup);
			}

			/// <summary>
			/// A union query of GEN_DRAWINGS, and GEN_DRAWINGS_MTL.
			/// </summary>
			/// <param name="partno">A part number.</param>
			/// <returns>Hopefully, no more than two rows of useful data.</returns>
			public GEN_DRAWINGSDataTable GetAllPDFData(string partno) {
				ENGINEERINGDataSetTableAdapters.GEN_DRAWINGSTableAdapter gd =
					new ENGINEERINGDataSetTableAdapters.GEN_DRAWINGSTableAdapter();
				string lookup = string.Format(@"{0}%.PDF", partno);
				GEN_DRAWINGSDataTable dt = gd.GetDrwgDataByFName(lookup, lookup);
				return dt;
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
				ENGINEERINGDataSetTableAdapters.inmastTableAdapter ita =
					new ENGINEERINGDataSetTableAdapters.inmastTableAdapter();
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
					ENGINEERINGDataSetTableAdapters.CUT_CUTLISTSTableAdapter ccl =
						new ENGINEERINGDataSetTableAdapters.CUT_CUTLISTSTableAdapter();
					if (ccl.GetDataByName(prtno, prtrv).Rows.Count > 0) {
						parttype = 5;
					} else {
						ENGINEERINGDataSetTableAdapters.CUT_PARTSTableAdapter cp =
							new ENGINEERINGDataSetTableAdapters.CUT_PARTSTableAdapter();
						if (cp.GetDataByPartnum(prtno).Rows.Count > 0)
							parttype = 6;
					}
				}
				return parttype;
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
				ENGINEERINGDataSetTableAdapters.ECR_ITEMSTableAdapter eita =
					new ENGINEERINGDataSetTableAdapters.ECR_ITEMSTableAdapter();
				int? r = eita.GetECRItem((int)ecrno, partnum, rev);
				if (r != null) {
					exists = true;
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
				ENGINEERINGDataSetTableAdapters.ECRObjLookupTableAdapter eolta =
					new ENGINEERINGDataSetTableAdapters.ECRObjLookupTableAdapter();
				if (eolta.GetDataByECO(econumber).Rows.Count > 0) {
					bogus = false;
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
				if (descr != string.Empty) {
					ENGINEERINGDataSetTableAdapters.CUT_EDGESTableAdapter ceta =
						new ENGINEERINGDataSetTableAdapters.CUT_EDGESTableAdapter();
					ENGINEERINGDataSetTableAdapters.CUT_EDGES_XREFTableAdapter cexta =
						new ENGINEERINGDataSetTableAdapters.CUT_EDGES_XREFTableAdapter();
					int id = (int)cexta.GetEdgeID(descr);
					ENGINEERINGDataSet.CUT_EDGESDataTable dt = ceta.GetDataByEdgeID(id);
					if (dt.Rows.Count > 0) {
						return dt[0].EDGEID;
					}
				}
				return -1;
			}
		}

		partial class CUT_MATERIALSDataTable {
			/// <summary>
			/// Find an ID from a material description.
			/// </summary>
			/// <param name="descr">A material description.</param>
			/// <returns>An int ID, or -1 if nothing is found.</returns>
			public int GetMaterialIDByDescr(string descr) {
				if (descr != string.Empty) {
					ENGINEERINGDataSetTableAdapters.CUT_MATERIALSTableAdapter cmta =
						new ENGINEERINGDataSetTableAdapters.CUT_MATERIALSTableAdapter();
					ENGINEERINGDataSet.CUT_MATERIALSDataTable dt = cmta.GetDataByDescr(descr);
					if (dt.Rows.Count > 0)
						return dt[0].MATID;
				}
				return -1;
			}

			/// <summary>
			/// Get a row of data from a material ID.
			/// </summary>
			/// <param name="id">A material ID.</param>
			/// <returns>A CUT_MATERIALSRow or null.</returns>
			public CUT_MATERIALSRow GetMaterial(int id) {
				ENGINEERINGDataSetTableAdapters.CUT_MATERIALSTableAdapter cmta =
					new ENGINEERINGDataSetTableAdapters.CUT_MATERIALSTableAdapter();
				ENGINEERINGDataSet.CUT_MATERIALSDataTable dt = cmta.GetDataByMatID(id);
				if (dt.Rows.Count > 0)
					return dt[0];
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
					ENGINEERINGDataSetTableAdapters.SCH_PROJECTSTableAdapter spta =
						new ENGINEERINGDataSetTableAdapters.SCH_PROJECTSTableAdapter();
					ENGINEERINGDataSet.SCH_PROJECTSRow row = spta.GetDataByProject(matches.Groups[1].ToString())[0];
					return row;
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
				ENGINEERINGDataSetTableAdapters.GEN_ODOMETERTableAdapter gota =
					new ENGINEERINGDataSetTableAdapters.GEN_ODOMETERTableAdapter();
				ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter guta =
					new ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter();
				int rowsAffected = 0;
				int uid = (int)guta.GetUID(System.Environment.UserName);
				string SQL = @"UPDATE GEN_ODOMETER SET ODO = ODO + 1 WHERE (FUNCID = @funcid AND USERID = @userid);";
				if (gota.Connection.State != System.Data.ConnectionState.Open) {
					gota.Connection.Open();
				}
				using (SqlCommand comm = new SqlCommand(SQL, gota.Connection)) {
					comm.Parameters.AddWithValue(@"@funcid", func);
					comm.Parameters.AddWithValue(@"@userid", uid);

					try {
						rowsAffected = comm.ExecuteNonQuery();
					} catch (System.InvalidOperationException ioe) {
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
						} catch (System.InvalidOperationException ioe) {
							throw ioe;
						}
					}
				}
			}
		}
	}
}
