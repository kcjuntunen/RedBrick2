using System;
using System.Collections.Generic;

using SolidWorks.Interop.sldworks;

namespace RedBrick2 {
	/// <summary>
	/// A set of revision property sets.
	/// </summary>
	public class Revs : IList<Rev> {
		/// <summary>
		/// The connected application.
		/// </summary>
		public SldWorks SwApp;

		/// <summary>
		/// Is this a metal drawing?
		/// </summary>
		public bool IsMetal;
		private List<Rev> innerList = new List<Rev>();

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="sw">The connected application.</param>
		public Revs(SldWorks sw) {
			SwApp = sw;
			Init();
		}

		private void Init() {
			ModelDoc2 md = (ModelDoc2)SwApp.ActiveDoc;
			IsMetal = md.GetPathName().Contains(@"METAL MANUFACTURING");
			for (int i = 1; i <= Properties.Settings.Default.LvlLimit; i++) {
				string lvl = string.Format("REVISION {0}", (char)(i + 64));
				StringProperty lvlp = new StringProperty(lvl, true, SwApp, md, string.Empty);
				lvlp.Get();
				if (lvlp.Value == null || lvlp.Value == string.Empty) {
					break;
				}

				string e = string.Format("ECO {0}", i);
				StringProperty ep = new StringProperty(e, true, SwApp, md, string.Empty);

				string de = string.Format("DESCRIPTION {0}", i);
				StringProperty dep = new StringProperty(de, true, SwApp, md, string.Empty);

				string l = string.Format("LIST {0}", i);
				AuthorProperty lp = new AuthorProperty(l, true, SwApp, md);
				AuthorUIDProperty lpuid = new AuthorUIDProperty(string.Format("{0} UID", l), true, SwApp, md);

				string da = string.Format("DATE {0}", i);
				DateProperty dap = new DateProperty(da, true, SwApp, md);

				ep.Get();
				dep.Get();
				lp.Get();
				lpuid.Get();
				dap.Get();
				if (lpuid.Value != null && lpuid.Value != string.Empty) {
					lp.Data = lpuid.Data;
				} else if (lp.Value != string.Empty) {
					lpuid.Data = lp.Data;
				}
				Add(new Rev(lvlp, ep, dep, lp, lpuid, dap));
			}
		}

		/// <summary>
		/// Generate a set of revision properties. Figure out the correct index.
		/// </summary>
		/// <param name="ev">An ECR Number.</param>
		/// <param name="dev">A description string.</param>
		/// <param name="aut">An int of author ID.</param>
		/// <param name="dt">A DateTime value.</param>
		/// <returns></returns>
		public int NewRev(string ev, string dev, int aut, DateTime dt) {
			ModelDoc2 md = (ModelDoc2)SwApp.ActiveDoc;
			int idx = Count;
			Rev r = new Rev(idx, ev, dev, aut, dt, SwApp, md);
			Add(r);
			return idx - 1;
		}

		/// <summary>
		/// Write all sets of revs to the part.
		/// </summary>
		public void Write() {
			foreach (Rev rev in innerList) {
				rev.Write();
			}
		}

		/// <summary>
		/// Add a rev set item.
		/// </summary>
		/// <param name="item"></param>
		public void Add(Rev item) {
			innerList.Add(item);
		}

		/// <summary>
		/// Dump all rev sets.
		/// </summary>
		public void Clear() {
			innerList.Clear();
		}

		/// <summary>
		/// Indicate whether a rev set is in our list.
		/// </summary>
		/// <param name="item">A Rev item.</param>
		/// <returns>True or false.</returns>
		public bool Contains(Rev item) {
			return innerList.Contains(item);
		}

		/// <summary>
		/// Make a copy of the set of rev sets into "array", starting with "arrayIndex".
		/// </summary>
		/// <param name="array">Array to copy to.</param>
		/// <param name="arrayIndex">Start here.</param>
		public void CopyTo(Rev[] array, int arrayIndex) {
			innerList.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Number of sets in the list.
		/// </summary>
		public int Count {
			get { return innerList.Count; }
		}

		/// <summary>
		/// Always returns false. It's never read only.
		/// </summary>
		public bool IsReadOnly {
			get { return false; }
		}

		/// <summary>
		/// Remove a rev set from the list.
		/// </summary>
		/// <param name="item">A rev set.</param>
		/// <returns></returns>
		public bool Remove(Rev item) {
			item.Delete();
			return innerList.Remove(item);
		}

		/// <summary>
		/// Return a generic IEnumerator should you need one.
		/// </summary>
		/// <returns>A generic IEnumerator.</returns>
		public IEnumerator<Rev> GetEnumerator() {
			return (new List<Rev>(this).GetEnumerator());
		}

		/// <summary>
		/// Get an Enumerator.
		/// </summary>
		/// <returns>IEnumerator</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return innerList.GetEnumerator();
		}

		/// <summary>
		/// Get the index of an item.
		/// </summary>
		/// <param name="item">A Rev item.</param>
		/// <returns>An int.</returns>
		public int IndexOf(Rev item) {
			return innerList.IndexOf(item);
		}

		/// <summary>
		/// Insert a Rev item at "index".
		/// </summary>
		/// <param name="index">An int where to insert.</param>
		/// <param name="item">The Rev item to insert.</param>
		public void Insert(int index, Rev item) {
			innerList.Insert(index, item);
		}

		/// <summary>
		/// Remove a Rev at "index".
		/// </summary>
		/// <param name="index">An int where to remove.</param>
		public void RemoveAt(int index) {
			innerList[index].Delete();
			innerList.RemoveAt(index);
		}

		/// <summary>
		/// Indexing is supported.
		/// </summary>
		/// <param name="index">An int index of the desired Rev.</param>
		/// <returns>A Rev item.</returns>
		public Rev this[int index] {
			get {
				return innerList[index];
			}
			set {
				innerList[index] = value;
			}
		}

		internal void Delete(int p) {
			int startIdx = Count - 1;
			for (int i = startIdx; i >= p; i--) {
				RemoveAt(i);
			}
		}
	}
}
