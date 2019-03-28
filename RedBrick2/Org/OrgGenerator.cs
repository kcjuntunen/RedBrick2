using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBrick2.Org {
	public class OrgGenerator {
		StringBuilder sb = new StringBuilder();

		public OrgGenerator() {

		}

		public string Generate() {
			StringBuilder blk = new StringBuilder();
			using (OrgDataSetTableAdapters.JobsDataTableAdapter j = new OrgDataSetTableAdapters.JobsDataTableAdapter()) {
				using (OrgDataSet.JobsDataDataTable dt = j.GetData(@"200")) {
					foreach (OrgDataSet.JobsDataRow row in dt.Rows) {
						if (!row.IsIGNIDNull()) {
							continue;
						}
						blk.AppendFormat("* {0}\n", row.PartNumber.Trim());
						blk.Append      ("  :PROPERTIES:\n");
						blk.AppendFormat("  :Captured: [{0:yyyy-MM-dd ddd HH:mm}]\n", DateTime.Now);
						blk.AppendFormat("  :Description: {0}\n", row.PartDescription.Trim());
						blk.AppendFormat("  :Complete: {0}\n", row.QtyComp);
						blk.AppendFormat("  :Quantity: {0}\n", row.JobQty);
						blk.AppendFormat("  :Rev: {0}\n", row.PartRev.Trim());
						if (!row.IsCUSTOMERNull()) {
							blk.AppendFormat("  :Customer: {0}\n", row.CUSTOMER.Trim());
						}
						blk.Append      ("  :END:\n");
						blk.AppendFormat("  - Job Number: {0}\n", row.JobNumber.Trim());
						blk.AppendFormat("    - Op: {0}\n", row.OpNumber);
						blk.AppendFormat("    - Status: {0}\n", row.JobStatus.Trim());
						blk.AppendFormat("    - Due: <{0:yyyy-MM-dd ddd}>", row.OpDue);
						sb.AppendLine(blk.ToString());
						blk = new StringBuilder();
					}
				}
			}

			return sb.ToString();
		}
	}
}
