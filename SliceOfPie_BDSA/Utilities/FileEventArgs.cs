using System;

namespace SliceOfPie_Model
{
    public class FileEventArgs : EventArgs
    {
        public readonly int FileId;

        public FileEventArgs(int fileId)
        {
            FileId = fileId;
        }
    }
}
