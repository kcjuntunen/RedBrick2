using System;
using System.Collections;
using System.Collections.Generic;

namespace RedBrick2.DrawingCollector {
	/// <summary>
	/// An item to be exported to PDF.
	/// </summary>
	[Serializable]
	public class PacketItems : IList<PacketItem> {
		/// <summary>
		/// Constructor. Nothing happens in here.
		/// </summary>
		public PacketItems() {

		}

		private List<PacketItem> _innerArray = new List<PacketItem>();

		/// <summary>
		/// Indexed access.
		/// </summary>
		/// <param name="index">Integer.</param>
		/// <returns>A PacketItem.</returns>
		public PacketItem this[int index] { get => ((IList<PacketItem>)_innerArray)[index]; set => ((IList<PacketItem>)_innerArray)[index] = value; }

		/// <summary>
		/// The number of PacketItems in this list.
		/// </summary>
		public int Count => ((IList<PacketItem>)_innerArray).Count;

		/// <summary>
		/// Can it be changed?
		/// </summary>
		public bool IsReadOnly => ((IList<PacketItem>)_innerArray).IsReadOnly;

		/// <summary>
		/// Add a PacketItem.
		/// </summary>
		/// <param name="item"></param>
		public void Add(PacketItem item) {
			((IList<PacketItem>)_innerArray).Add(item);
		}

		/// <summary>
		/// Dump all the PacketItems.
		/// </summary>
		public void Clear() {
			((IList<PacketItem>)_innerArray).Clear();
		}

		/// <summary>
		/// Do we already have this one?
		/// </summary>
		/// <param name="item">A PacketItem</param>
		/// <returns>True if it's in there.</returns>
		public bool Contains(PacketItem item) {
			return ((IList<PacketItem>)_innerArray).Contains(item);
		}

		/// <summary>
		/// Copy PacketItems to an array.
		/// </summary>
		/// <param name="array">Array of PacketItems.</param>
		/// <param name="arrayIndex">Where to start.</param>
		public void CopyTo(PacketItem[] array, int arrayIndex) {
			((IList<PacketItem>)_innerArray).CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Get an Enumerator.
		/// </summary>
		/// <returns>An Enumerator that iterates through the list.</returns>
		public IEnumerator<PacketItem> GetEnumerator() {
			return ((IList<PacketItem>)_innerArray).GetEnumerator();
		}

		/// <summary>
		/// Get the index of a particular PacketItem.
		/// </summary>
		/// <param name="item">A PacketItem to look for.</param>
		/// <returns>An index.</returns>
		public int IndexOf(PacketItem item) {
			return ((IList<PacketItem>)_innerArray).IndexOf(item);
		}

		/// <summary>
		/// Insert a PacketItem.
		/// </summary>
		/// <param name="index">Where to insert.</param>
		/// <param name="item">What to insert.</param>
		public void Insert(int index, PacketItem item) {
			((IList<PacketItem>)_innerArray).Insert(index, item);
		}

		/// <summary>
		/// Remove the first instance of a PacketItem.
		/// </summary>
		/// <param name="item">A packetitem to remove.</param>
		/// <returns>IDK. ¯\_(ツ)_/¯</returns>
		public bool Remove(PacketItem item) {
			return ((IList<PacketItem>)_innerArray).Remove(item);
		}

		public void RemoveAt(int index) {
			((IList<PacketItem>)_innerArray).RemoveAt(index);
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return ((IList<PacketItem>)_innerArray).GetEnumerator();
		}
	}
}
