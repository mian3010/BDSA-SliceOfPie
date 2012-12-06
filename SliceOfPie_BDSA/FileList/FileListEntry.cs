using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceOfPie_Model.FileList {
  class FileListEntry {
    public long Id { get; set; }
    public float Version { get; set; }
    public string Path { get; set; }
    public string Name { get; set; }
    public FileListType Type { get; set; }
    public bool IsDeleted { get; set; }
  }
}
