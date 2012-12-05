using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceOfPie_Model
{
    /// <summary>
    /// Struct to define a log entry. Contains an enum FileModification that maps whatever action should be done.
    /// </summary>
   public struct LogEntry
    {
        public long id;
        public String fileName;
        public String filePath;
        public DateTime timeStamp;
        public FileModification modification;

        public LogEntry(long id, String filename, String filePath, DateTime timeStamp, FileModification modification)
        {
            this.id = id;
            this.fileName = filename;
            this.filePath = filePath;
            this.timeStamp = timeStamp;
            this.modification = modification;
        }
    }
}
