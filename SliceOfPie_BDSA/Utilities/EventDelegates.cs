using System;
using SliceOfPie_Model.Persistence;

namespace SliceOfPie_Model
{
    public delegate void FileInstanceEventHandler(FileInstance file);

    public delegate void DocumentHandler(Document doc);
    public delegate void FileInstanceRequestHandler(object sender, FileEventArgs args);
}
