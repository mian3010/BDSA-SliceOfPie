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
     *         <file>
     *          <subdirectory>
     *            
     * <file>
     **/  

    public String ListAsXml()
    {
        String xml = "";



        return xml;
    }


  }
}
