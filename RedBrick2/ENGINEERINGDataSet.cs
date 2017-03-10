using System.Data.SqlClient;
namespace RedBrick2 {


  public partial class ENGINEERINGDataSet {

    public void IncrementOdometer(Redbrick.Functions func) {
      ENGINEERINGDataSetTableAdapters.GEN_ODOMETERTableAdapter gota =
        new ENGINEERINGDataSetTableAdapters.GEN_ODOMETERTableAdapter();
      ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter guta =
        new ENGINEERINGDataSetTableAdapters.GEN_USERSTableAdapter();
      int rowsAffected = 0;
      int uid = (int)guta.GetUID(System.Environment.UserName);
      string SQL = @"UPDATE GEN_ODOMETER SET ODO = ODO + 1 WHERE (FUNCID = @funcid AND USERID = @userid);";
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
