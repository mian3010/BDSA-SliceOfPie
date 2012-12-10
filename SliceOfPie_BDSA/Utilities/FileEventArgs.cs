using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceOfPie_Model
{
    public class FileEventArgs : EventArgs
    {
        public long FileID;

        public FileEventArgs(long FileID)
        {
            this.FileID = FileID;
        }
    }
}
