using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SliceOfPie_Model.Persistence;

namespace SliceOfPie_Model {
  public class FileListEntry {
    public long Id { get; set; }
    public decimal Version { get; set; }
    public string Path { get; set; }
    public string Name { get; set; }
    public FileListType Type { get; set; }
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Author: Claus35-DK - clih@itu.dk
    /// </summary>
    /// <param name="fromFile"></param>
    /// <returns></returns>
    public static FileListEntry EntryFromFile(FileInstance fromFile) {
      FileListEntry Entry = new FileListEntry();
      Entry.Id = fromFile.File_id;
      Entry.Path = fromFile.path;
      if (fromFile.deleted == 1) Entry.IsDeleted = true;
      else Entry.IsDeleted = false;
      return Entry;
    }
  }
}
