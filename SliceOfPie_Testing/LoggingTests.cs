using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SliceOfPie_Model;
using System.Collections.Generic;

namespace SliceOfPie_Testing
{
    [TestClass]
    public class LoggingTests
    {
        private FileList BasicFileList(bool saveLog)
        {
            List<File> rig = FileCommunicatorTests.GetTestRig();
            CommunicatorOfflineAdapter ts = new CommunicatorOfflineAdapter();
            OfflineFileList l = new OfflineFileList(ts);
            foreach (File file in rig)
            {
                ts.AddFile(file);
            }
            ts.DeleteFile(rig[0]);
            ts.RenameFile(rig[1], "renamefile.html");
            Document ds = (Document) rig[3];
            ds.Content.Append("Changing something");
            ts.ModifyFile(rig[3]);
            if (saveLog) {
                l.PersistLogOnDisk(); 
            }
            return l.FileList;

        }

        /// <summary>
        /// Test for legitimate log entries. When we're passing delegates where confirming the right values of the return.
        /// So we only assert that is it not null.
        /// </summary>
        [TestMethod]
        public void TestLoggingFunctions()
        {
            List<LogEntry> entries = BasicFileList(false);
            LogEntry one = entries.Find((e) => { return (e.id == 1 && e.modification == FileModification.Delete);});
            LogEntry two = entries.Find((e) => { return (e.id == 2 && e.modification == FileModification.Rename); });
            LogEntry three = entries.Find((e) => { return (e.id == 4 && e.modification == FileModification.Modify); });
            
            Assert.IsNotNull(one);
            Assert.IsNotNull(two);
            Assert.IsNotNull(three);
        }

        [TestMethod]
        public void TestPersistentLog()
        {
            BasicFileList(true);
            CommunicatorOfflineAdapter ts = new CommunicatorOfflineAdapter();
            OfflineFileList l = new OfflineFileList(ts);
            List<LogEntry> persistedLog = l.FileList;
            Assert.AreEqual(true, persistedLog.Count > 0);

            // Test that the log is really saved as empty as well.
            l.FileList.Clear();
            l.PersistLogOnDisk();

            l = new OfflineFileList(ts);
            Assert.AreEqual(true, l.FileList.Count == 0);

        }
    }
}
