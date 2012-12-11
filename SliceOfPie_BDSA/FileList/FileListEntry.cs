using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceOfPie_Model {
  public class FileListEntry {
    public long Id { get; set; }
    public float Version { get; set; }
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

    public FileListType FileListType
    {
        get
        {
            throw new System.NotImplementedException();
        }
        set
        {
        }
    }
  }
}
