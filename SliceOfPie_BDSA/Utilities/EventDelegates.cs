using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SliceOfPie_Model.Persistence;

namespace SliceOfPie_Model
{
    public delegate void DocumentHandler(object sender, String newContent);
    public delegate void FileRequestHandler(object sender, FileEventArgs args);

    /// <summary>
    /// Custom delegate to handle file events. 
    /// </summary>
    /// <param name="file"></param>
    public delegate void FileEventHandler(File file);
}
