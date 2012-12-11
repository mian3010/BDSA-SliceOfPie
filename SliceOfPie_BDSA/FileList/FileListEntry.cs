using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceOfPie_Model {
  public class FileListEntry {
    public long Id { get; set; }
    public float Version { get; set; }
    private string path;
    public string Path { 
        get { 
            if(path == null) return "none";
            else return Path;
            }
        set { path = Path; }
    }
    private string name;
    public string Name { 
        get {
            if (name == null) return "none";
            else return name;       
            }
        set
        {
            name = Name;
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
