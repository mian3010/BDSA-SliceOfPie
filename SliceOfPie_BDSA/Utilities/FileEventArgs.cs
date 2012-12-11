using System;

namespace SliceOfPie_Model
{
    public class FileEventArgs : EventArgs
    {
        public readonly long FileId;

        public FileEventArgs(long fileId)
        {
            FileId = fileId;
        }
    }
}
