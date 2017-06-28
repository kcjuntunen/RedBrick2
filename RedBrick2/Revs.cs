using System;
using System.Collections.Generic;
using System.Text;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2 {
  class Revs : IList<Rev> {
    private SldWorks SwApp;
    private List<Rev> innerList = new List<Rev>();

    public Revs(SldWorks sw) {
      SwApp = sw;
      Init();
    }

    private void Init() {
      ModelDoc2 md = (ModelDoc2)SwApp.ActiveDoc;

      for (int i = 1; i <= Properties.Settings.Default.LvlLimit; i++) {
        string lvl = string.Format("REVISION {0}", (char)(i + 64));
        string e = string.Format("ECO {0}", i);
        string de = string.Format("DESCRIPTION {0}", i);
        string l = string.Format("LIST {0}", i);
        string da = string.Format("DATE {0}", i);

        StringProperty lvlp = new StringProperty(lvl, true, SwApp, md, string.Empty);
        StringProperty ep = new StringProperty(e, true, SwApp, md, string.Empty);
        StringProperty dep = new StringProperty(de, true, SwApp, md, string.Empty);
        AuthorProperty lp = new AuthorProperty(l, true, SwApp, md);
        DateProperty dap = new DateProperty(da, true, SwApp, md);

        lvlp.Get();
        if (lvlp.Value == string.Empty) {
          break;
        }

        ep.Get();
        dep.Get();
        lp.Get();
        dap.Get();
        Add(new Rev(lvlp, ep, dep, lp, dap));
      }
    }

    public Rev NewRev(string ev, string dev, int aut, DateTime dt) {
      ModelDoc2 md = (ModelDoc2)SwApp.ActiveDoc;
      int idx = Count;
      Rev r = new Rev(idx, ev, dev, aut, dt, SwApp, md);
      Add(r);
      return innerList[idx];
    }

    private Rev newRev2(string ev, string dev, int aut, DateTime dt) {
      ModelDoc2 md = (ModelDoc2)SwApp.ActiveDoc;
      int idx = Count + 1;
      string lvl = string.Format("REVISION {0}", (char)(idx + 65));
      string e = string.Format("ECO {0}", idx);
      string de = string.Format("DESCRIPTION {0}", idx);
      string l = string.Format("LIST {0}", idx);
      string da = string.Format("DATE {0}", idx);

      StringProperty lvlp = new StringProperty(lvl, true, SwApp, md, string.Empty);
      StringProperty ep = new StringProperty(e, true, SwApp, md, string.Empty);
      StringProperty dep = new StringProperty(de, true, SwApp, md, string.Empty);
      AuthorProperty lp = new AuthorProperty(l, true, SwApp, md);
      DateProperty dap = new DateProperty(da, true, SwApp, md);

      Add(new Rev(lvlp, ep, dep, lp, dap));
      return innerList[idx];
    }

    public void Write() {
      foreach (Rev rev in innerList) {
        rev.Write();
      }
    }

    public void Add(Rev item) {
      innerList.Add(item);
    }

    public void Clear() {
      innerList.Clear();
    }

    public bool Contains(Rev item) {
      return innerList.Contains(item);
    }

    public void CopyTo(Rev[] array, int arrayIndex) {
      innerList.CopyTo(array, arrayIndex);
    }

    public int Count {
      get { return innerList.Count; }
    }

    public bool IsReadOnly {
      get { return false; }
    }

    public bool Remove(Rev item) {
      return innerList.Remove(item);
    }

    public IEnumerator<Rev> GetEnumerator() {
      return (new List<Rev>(this).GetEnumerator());
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
      return innerList.GetEnumerator();
    }

    public int IndexOf(Rev item) {
      return innerList.IndexOf(item);
    }

    public void Insert(int index, Rev item) {
      innerList.Insert(index, item);
    }

    public void RemoveAt(int index) {
      innerList.RemoveAt(index);
    }

    public Rev this[int index] {
      get {
        return innerList[index];
      }
      set {
        innerList[index] = value;
      }
    }
  }
}
