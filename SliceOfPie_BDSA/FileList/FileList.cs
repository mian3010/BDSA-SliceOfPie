using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceOfPie_Model {
  public class FileList {
    public IDictionary<long, FileListEntry> List { get; set; }

    public Int64 incrementCounter
    {
        get;
        set; 
    }

    /*
     * <xml>
     *  <rootdirectory>
     *     <subdirectory>
     *         file
     *          </subdirectory>
     *          <subdirectory>
     *            
     * <file>
     **/
    public List<String> GetAllFilePaths()
    {
        List<String> paths = new List<String>();
        foreach (FileListEntry tmp in List.Values)
        {
            paths.Add(System.IO.Path.Combine(tmp.Path, tmp.Name));
        }
        return paths;
    }

  }
}
