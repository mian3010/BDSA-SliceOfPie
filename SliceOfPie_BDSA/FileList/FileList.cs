using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceOfPie_Model.FileList {
  public class FileList {
    public IDictionary<long, FileListEntry> List { get; set; }
  }
}
