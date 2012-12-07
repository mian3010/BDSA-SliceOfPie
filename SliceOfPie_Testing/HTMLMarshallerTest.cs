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
            List<FileListEntry> log = new List<FileListEntry>();

            FileListEntry log1 = new FileListEntry(1, "file1", "/local", DateTime.Now, FileModification.Add);
            FileListEntry log2 = new FileListEntry(2, "file2", "/local", DateTime.Now, FileModification.Delete);
            FileListEntry log3 = new FileListEntry(3, "file3", "/local", DateTime.Now, FileModification.Modify);
            FileListEntry log4 = new FileListEntry(4, "file4", "/local", DateTime.Now, FileModification.Add);
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
            List<FileListEntry> log = new List<FileListEntry>();

            FileListEntry log1 = new FileListEntry(1, "file1", "/local", DateTime.Now, FileModification.Add);
            FileListEntry log2 = new FileListEntry(2, "file2", "/local", DateTime.Now, FileModification.Delete);
            FileListEntry log3 = new FileListEntry(3, "file3", "/local", DateTime.Now, FileModification.Modify);
            FileListEntry log4 = new FileListEntry(4, "file4", "/local", DateTime.Now, FileModification.Add);
            Console.Out.WriteLine("LOG TIME BEFORE MARSHALL" + log1.timeStamp);
            log.Add(log1);
            log.Add(log2);
            log.Add(log3);
            log.Add(log4);

            string xml= HTMLMarshaller.MarshallLog(log);
            List<FileListEntry> list = HTMLMarshaller.UnMarshallLog(xml);

            for (int i = 0; i < list.Count; i++ )
            {
                FileListEntry entry = list[i];
                FileListEntry refEntry = log[i];
                Assert.AreEqual(entry.id, refEntry.id);
                Assert.AreEqual(entry.fileName, refEntry.fileName);
                Assert.AreEqual(entry.filePath, refEntry.filePath);
                Assert.AreEqual(entry.timeStamp.ToString(), refEntry.timeStamp.ToString());
                Assert.AreEqual(entry.modification, refEntry.modification);
                Console.Out.WriteLine("entry: " + entry.id);
            }
        }
    }
}
