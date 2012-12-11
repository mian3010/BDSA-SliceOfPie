using System;

namespace SliceOfPie_Model
{
    public delegate void DocumentHandler(object sender, String newContent);
    public delegate void FileRequestHandler(object sender, FileEventArgs args);
}
