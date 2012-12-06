using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SliceOfPie_Model;
using SliceOfPie_Network;

namespace SliceOfPie_Server
{
    class Program
    {

        private Queue<LogEntry> LogQue = new Queue<LogEntry>();

        static void Main(string[] args)
        {
            File file = File.CreateFile(1, "Hejsa.txt", @"Here", 0);
            SaveFile(file);
        }

        public static void SaveFile(File File)
        {
            CommunicatorOfflineAdapter adapter = new CommunicatorOfflineAdapter(@".\Files");
            if (adapter.FindFile(File)) //File exists
            {
                adapter.ModifyFile(File);
            }
            else //New file
            {
                adapter.AddFile(File);
            }
        }

        public void ReceiveLog(List<LogEntry> LogEntries)
        {
            foreach(LogEntry entry in LogEntries){
                LogQue.Enqueue(entry);
            }
        }
    }
}
