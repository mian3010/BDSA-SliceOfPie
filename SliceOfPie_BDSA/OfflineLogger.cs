using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SliceOfPie_Model
{
    public class OfflineLogger
    {
        public readonly String logpath = @".\Files\log";
        public readonly String logfile = "log.xml";

        public List<LogEntry> OfflineLog
        {
            get
            {
                return OfflineLog;
            }
            private set
            {
                String fullLogPath = System.IO.Path.Combine(logpath, logfile);
                if (!System.IO.Directory.Exists(logpath))
                {
                    System.IO.Directory.CreateDirectory(logpath);
                }
                if (System.IO.File.Exists(fullLogPath))
                {
                    String logXML = System.IO.File.ReadAllText(fullLogPath);
                    OfflineLog = HTMLMarshalUtil.UnMarshallLog(logXML);
                }
                else
                {
                    OfflineLog = new List<LogEntry>();
                }
            }
        }

        public OfflineLogger(CommunicatorOfflineAdapter cm)
        {
            cm.FileAdded += FileAdded;
            cm.FileChanged += FileChanged;
            cm.FileDeleted += FileDeleted;
            cm.FileMoved += FileMoved;
            cm.FileRenamed += FileRenamed;
        }

        public void FileAdded(File file)
        {
            SaveToLog(file.id, file.name, file.serverpath, DateTime.Now, FileModification.Add);
        }

        public void FileDeleted(File file)
        {
            SaveToLog(file.id, file.name, file.serverpath, DateTime.Now, FileModification.Delete);
        }

        public void FileRenamed(File file)
        {
            SaveToLog(file.id, file.name, file.serverpath, DateTime.Now, FileModification.Rename);
        }

        private void SaveToLog(long id, string filename, string filepath, DateTime timeStamp, FileModification modification)
        {
            OfflineLog.Add(new LogEntry(id, filename, filepath, timeStamp, modification));
        }

        public void PersistLogOnDisk()
        {
            String fullPath = System.IO.Path.Combine(logpath, logfile);
            String logXML = HTMLMarshalUtil.MarshallLog(OfflineLog);
            System.IO.File.WriteAllText(fullPath, logXML);
        }

        public void FileChanged(File file)
        {
            SaveToLog(file.id, file.name, file.serverpath, DateTime.Now, FileModification.Modify);
        }

        public void FileMoved(File file)
        {
            SaveToLog(file.id, file.name, file.serverpath, DateTime.Now, FileModification.Move);
        }

    }
}
