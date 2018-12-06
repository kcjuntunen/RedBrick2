using System;

namespace RedBrick2.DrawingCollector {
	[Serializable]
	public class Steps {
		public string PathName { get; set; }
		public PacketItems Items { get; set; }
		public string Duration { get; set; } = "0";
		public DateTime Timestamp { get; set; } = DateTime.Now;
	}
}
