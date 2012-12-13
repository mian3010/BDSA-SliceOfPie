using System;
using SliceOfPie_Model.Persistence;

namespace SliceOfPie_Model
{
    public delegate void DocumentHandler(object sender, Document doc);
    public delegate void FileRequestHandler(object sender, FileEventArgs args);
}
