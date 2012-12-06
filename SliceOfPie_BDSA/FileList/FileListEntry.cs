using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceOfPie_Model.FileList {
  class FileListEntry {
    float Id { get; set; }
    float Version { get; set; }
    string Path { get; set; }
    string Name { get; set; }
    FileListType Type { get; set; }
    bool IsDeleted { get; set; }
  }
}
