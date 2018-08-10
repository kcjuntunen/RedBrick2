using System.Data.SqlClient;

namespace RedBrick2.ECRViewer.ECRViewerDataSetTableAdapters {
	partial class GEN_DRAWINGSTableAdapter {
	}

	public partial class ECR_LEGACYTableAdapter {
		/// <summary>
		/// Get legacy ECR numbers based on a lookup.
		/// </summary>
		/// <param name="itemNum">A <see cref="string"/> to lookup.</param>
		/// <returns>A <see cref="ECRViewerDataSet.ECR_LEGACYDataTable"/> filled with related legacy ECR numbers.</returns>
		public ECRViewerDataSet.ECR_LEGACYDataTable GetDataByItemNum(string itemNum) {
			ECRViewerDataSet.ECR_LEGACYDataTable dt_ = new ECRViewerDataSet.ECR_LEGACYDataTable();
			string SQL = @"SELECT * FROM ECR_LEGACY WHERE AffectedParts LIKE @part ORDER BY ECRNum DESC";
			using (SqlCommand comm_ = new SqlCommand(SQL, Connection)) {
				comm_.Parameters.AddWithValue(@"@part", string.Format(@"%{0}%", itemNum));
				using (SqlDataAdapter da_ = new SqlDataAdapter(comm_)) {
					da_.Fill(dt_);
				}
			}
			return dt_;
		}
	}
}

namespace RedBrick2.ECRViewer {


	partial class ECRViewerDataSet {
	}
}
