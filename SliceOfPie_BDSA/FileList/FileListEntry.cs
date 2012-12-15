using System;
using SliceOfPie_Model.Persistence;

namespace SliceOfPie_Model {
  [Serializable()]
  public class FileListEntry {
    public int Id { get; set; }

    public decimal Version { get; set; }
    private string _path;
    public string Path {
      get {
        if (_path == null) return "none";
        return _path;
      }
      set { _path = value; }
    }
    private string _name;
    public string Name {
      get {
        if (_name == null) return "none";
        return _name;
      }
      set {
        _name = value;
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
      return new FileListEntry { Id = fromFile.id, Path = fromFile.path, IsDeleted = fromFile.deleted == 1, Type = FileListType.Pull
                                 , Name = fromFile.File.name };
    }
  }
}
