using System;
using System.Collections;
using System.Collections.Generic;

namespace RedBrick2.DrawingCollector {
	[Serializable]
	public class PacketItems : IList<PacketItem> {
		public PacketItems() {

		}

		private List<PacketItem> _innerArray = new List<PacketItem>();

		public PacketItem this[int index] { get => ((IList<PacketItem>)_innerArray)[index]; set => ((IList<PacketItem>)_innerArray)[index] = value; }

		public int Count => ((IList<PacketItem>)_innerArray).Count;

		public bool IsReadOnly => ((IList<PacketItem>)_innerArray).IsReadOnly;

		public void Add(PacketItem item) {
			((IList<PacketItem>)_innerArray).Add(item);
		}

		public void Clear() {
			((IList<PacketItem>)_innerArray).Clear();
		}

		public bool Contains(PacketItem item) {
			return ((IList<PacketItem>)_innerArray).Contains(item);
		}

		public void CopyTo(PacketItem[] array, int arrayIndex) {
			((IList<PacketItem>)_innerArray).CopyTo(array, arrayIndex);
		}

		public IEnumerator<PacketItem> GetEnumerator() {
			return ((IList<PacketItem>)_innerArray).GetEnumerator();
		}

		public int IndexOf(PacketItem item) {
			return ((IList<PacketItem>)_innerArray).IndexOf(item);
		}

		public void Insert(int index, PacketItem item) {
			((IList<PacketItem>)_innerArray).Insert(index, item);
		}

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
