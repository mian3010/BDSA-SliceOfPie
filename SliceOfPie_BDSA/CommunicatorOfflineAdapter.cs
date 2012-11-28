using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceOfPie_Model
{
    public class CommunicatorOfflineAdapter : ICommunicator
    {
        

        public int AddFile(File file)
        {
            throw new NotImplementedException();
        }

        public File ChangePath(File old, string newPath)
        {
            throw new NotImplementedException();
        }

        public List<LogEntry> GetLog()
        {
            throw new NotImplementedException();
        }

        public void SaveLog()
        {
            throw new NotImplementedException();
        }
    }
}
