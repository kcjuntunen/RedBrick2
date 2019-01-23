using System;

namespace RedBrick2.DrawingCollector {
	/// <summary>
	/// A serializable class for saving DrawingCollector steps.
	/// </summary>
	[Serializable]
	public class Steps {
		/// <summary>
		/// Path of Packet PDF.
		/// </summary>
		public string PathName { get; set; }
		/// <summary>
		/// Drawing Items.
		/// </summary>
		public PacketItems Items { get; set; }
		/// <summary>
		/// How long it took to save.
		/// </summary>
		public string Duration { get; set; } = "0";
		/// <summary>
		/// When it was saved.
		/// </summary>
		public DateTime Timestamp { get; set; } = DateTime.Now;
	}
}
