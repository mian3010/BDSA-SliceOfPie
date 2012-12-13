using System;
using System.Collections.Generic;

namespace SliceOfPie_Model {
  [Serializable()]
  public class FileList {
    public IDictionary<int, FileListEntry> List { get; set; }

    public int IncrementCounter {
      get;
      set;
    }
  }
}
