using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SliceOfPie_Model;
using System.Collections.Generic;

namespace SliceOfPie_Testing
{
    [TestClass]
    public class LoggingTests
    {
        private List<LogEntry> BasicLogFunctions(bool saveLog)
        {
            List<File> rig = FileCommunicatorTests.GetTestRig();
            CommunicatorOfflineAdapter ts = new CommunicatorOfflineAdapter();
            OfflineLogger l = new OfflineLogger(ts);
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
            return l.OfflineLog;

        }

        /// <summary>
        /// Test for legitimate log entries. When we're passing delegates where confirming the right values of the return.
        /// So we only assert that is it not null.
        /// </summary>
        [TestMethod]
        public void TestLoggingFunctions()
        {
            List<LogEntry> entries = BasicLogFunctions(false);
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
            BasicLogFunctions(true);
            CommunicatorOfflineAdapter ts = new CommunicatorOfflineAdapter();
            OfflineLogger l = new OfflineLogger(ts);
            List<LogEntry> persistedLog = l.OfflineLog;
            Assert.AreEqual(true, persistedLog.Count > 0);

            // Test that the log is really saved as empty as well.
            l.OfflineLog.Clear();
            l.PersistLogOnDisk();

            l = new OfflineLogger(ts);
            Assert.AreEqual(true, l.OfflineLog.Count == 0);

        }
    }
}
