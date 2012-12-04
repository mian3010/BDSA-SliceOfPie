using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SliceOfPie_Model;
using SliceOfPie_Network;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;

namespace SliceOfPie_Testing
{
    [TestClass]
    public class HTMLMarshallerTest
    {
        [TestMethod]
        public void TestMarshallLog()
        {
            List<LogEntry> log = new List<LogEntry>();

            LogEntry log1 = new LogEntry(1, "file1", "/local", DateTime.Now, FileModification.Add);
            LogEntry log2 = new LogEntry(2, "file2", "/local", DateTime.Now, FileModification.Delete);
            LogEntry log3 = new LogEntry(3, "file3", "/local", DateTime.Now, FileModification.Modify);
            LogEntry log4 = new LogEntry(4, "file4", "/local", DateTime.Now, FileModification.Add);
            log.Add(log1);
            log.Add(log2);
            log.Add(log3);
            log.Add(log4);

            string xml= HTMLMarshaller.MarshallLog(log);
            Console.Out.WriteLine("XML: " + xml);
            Assert.AreNotEqual(null, xml);
        }

        [TestMethod]
        public void TestUnMarshallLog()
        {
            List<LogEntry> log = new List<LogEntry>();

            LogEntry log1 = new LogEntry(1, "file1", "/local", DateTime.Now, FileModification.Add);
            LogEntry log2 = new LogEntry(2, "file2", "/local", DateTime.Now, FileModification.Delete);
            LogEntry log3 = new LogEntry(3, "file3", "/local", DateTime.Now, FileModification.Modify);
            LogEntry log4 = new LogEntry(4, "file4", "/local", DateTime.Now, FileModification.Add);
            log.Add(log1);
            log.Add(log2);
            log.Add(log3);
            log.Add(log4);

            string xml= HTMLMarshaller.MarshallLog(log);
            List<LogEntry> list = HTMLMarshaller.UnMarshallLog(xml);

            for (int i = 0; i < list.Count; i++ )
            {
                LogEntry entry = list[i];
                LogEntry refEntry = log[i];
                Assert.AreEqual(entry.id, refEntry.id);
                Console.Out.WriteLine("entry: " + entry.id);
            }
        }
    }
}
