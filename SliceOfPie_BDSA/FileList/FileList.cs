using System;
using System.Collections.Generic;

namespace SliceOfPie_Model {
  
    /// <summary>
    /// Contains a dictionary that maps fileInstance ID's to fileListEntries. Maps a list of all files and their information.
    /// </summary>
    [Serializable()]
    public class FileList {
    public IDictionary<int, FileListEntry> List { get; set; }


    /// <summary>
    /// Counts the number of id's that has not been allocated by the database. Should be reset on synchronize.
    /// </summary>
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
