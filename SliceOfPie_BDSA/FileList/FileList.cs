using System;
using System.Collections.Generic;

namespace SliceOfPie_Model {
  [Serializable()]
  public class FileList {
    public IDictionary<long, FileListEntry> List { get; set; }

    public Int64 IncrementCounter {
      get;
      set;
    }
  }
}
