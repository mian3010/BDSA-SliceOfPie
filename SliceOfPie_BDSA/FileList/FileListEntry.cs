using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SliceOfPie_Model.Persistence;

namespace SliceOfPie_Model {
  public class FileListEntry {
    public long Id { get; set; }

    public decimal Version { get; set; }
    private string _path;
    public string Path { 
        get { 
            if(_path == null) return "none";
            else return Path;
            }
        set { _path = Path; }
    }
    private string _name;
    public string Name { 
        get {
            if (_name == null) return "none";
            else return _name;       
            }
        set
        {
            _name = Name;
        }
    }
    public FileListType Type { get; set; }
    public bool IsDeleted { get; set; }
    
    /// <summary>
    /// Author: Claus35-DK - clih@itu.dk
    /// </summary>
    /// <param name="fromFile"></param>
    /// <returns></returns>
    public static FileListEntry EntryFromFile(FileInstance fromFile) {
      return new FileListEntry {Id = fromFile.File_id, Path = fromFile.path, IsDeleted = fromFile.deleted == 1};
    }
  }
}
