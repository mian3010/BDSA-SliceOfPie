using System;
using System.Collections.Generic;

namespace SliceOfPie_Model {
  
    /// <summary>
    /// Contains a 
    /// </summary>
    [Serializable()]
    public class FileList {
    public IDictionary<int, FileListEntry> List { get; set; }

    public int IncrementCounter {
      get;
      set;
    }

      /// <summary>
      /// User emai connected to the file mail
      /// </summary>
    public String User { get; set; }

      public FileList()
      {
          List = new Dictionary<int, FileListEntry>();
      }
  }
}
