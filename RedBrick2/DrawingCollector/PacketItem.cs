using System;

namespace RedBrick2.DrawingCollector {
	[Serializable]
	public class PacketItem {
		public PacketItem() {

		}
		public string Name { get; set; } = @"NULL";
		public string Configuration { get; set; } = string.Empty;
		public string DocType { get; set; } = string.Empty;
		public string Department { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public string SldDoc { get; set; } = string.Empty;
		public string SldDrw { get; set; } = string.Empty;
		public string Pdf { get; set; } = string.Empty;

		public bool Checked { get; set; } = true;
		public bool CloseSldDrw { get; set; } = true;
		public bool DeletePdf { get; set; } = true;
	}
}
