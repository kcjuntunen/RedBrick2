using System.Data.SqlClient;
namespace RedBrick2 {


	public partial class ENGINEERINGDataSet {
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
