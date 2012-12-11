using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceOfPie_Model
{
    public class FileEventArgs : EventArgs
    {
        public readonly long FileId;

        public FileEventArgs(long fileId)
        {
            this.FileId = fileId;
        }
    }
}
