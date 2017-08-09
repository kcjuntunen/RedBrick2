using System.Data.SqlClient;
namespace RedBrick2 {


  public partial class ENGINEERINGDataSet {
    partial class SCH_PROJECTSDataTable {
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
  
    partial class GEN_CUSTOMERSDataTable {
    }
  
    partial class ECRObjLookupDataTable {
    }
  
    partial class GEN_ODOMETERDataTable {
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
