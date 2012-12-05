using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SliceOfPie_Model;
using SliceOfPie_Network;
using System.Collections.Generic;

namespace SliceOfPie_Testing
{
    [TestClass]
    public class NetworkTest
    {
        [TestMethod]
        public void TestServer()
        {
            NetworkServer server = new NetworkServer(8080);
            NetworkClient client = new NetworkClient();
            // Testdata
            List<LogEntry> loglist = new List<LogEntry>();
            LogEntry log1 = new LogEntry(1, "file1", "/local", DateTime.Now, FileModification.Add);
            LogEntry log2 = new LogEntry(2, "file2", "/local", DateTime.Now, FileModification.Delete);
            LogEntry log3 = new LogEntry(3, "file3", "/local", DateTime.Now, FileModification.Modify);
            LogEntry log4 = new LogEntry(4, "file4", "/local", DateTime.Now, FileModification.Add);
            Console.Out.WriteLine("LOG TIME BEFORE MARSHALL" + log1.timeStamp);
            loglist.Add(log1);
            loglist.Add(log2);
            loglist.Add(log3);
            loglist.Add(log4);
            server.listen();
            client.SendLog(loglist);
            server.Close();
        }
    }
}
