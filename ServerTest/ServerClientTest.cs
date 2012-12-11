using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SliceOfPie_Model;
using SliceOfPie_Model.Persistence;
using SliceOfPie_Server;
using System.Threading;
using System.Collections.Generic;

namespace ServerTest
{
    [TestClass]
    public class ServerClientTest
    {
        [TestMethod]
        public void TestGetFile()
        {
            NetworkServer server = NetworkServer.GetInstance();
            var client = new NetworkClient();
            const long id = new long();
            var serverT = new Thread(server.Listen);
            serverT.Start();
            Thread.Sleep(1000);
            File file = client.PullFile(id);
            Console.Out.WriteLine(file.id);
            server.Close();
        }

        /// <summary>
        /// Tests that you cannot save a file that is not in the FileList
        /// </summary>
        [TestMethod]
        public void TestSaveFileNegative()
        {
            NetworkServer server = NetworkServer.GetInstance();
            var client = new NetworkClient();
            var doc = new Document();
            var data = new FileMetaData {value = "HALLO"};
          var type = new MetaDataType {Type = "test metadatatype"};
          data.MetaDataType = type;
            doc.id = 1;
            doc.name = "Testfile";
            var serverT = new Thread(server.Listen);
            serverT.Start();
            Thread.Sleep(1000);
            long id = client.PushFile(doc);
            Assert.AreEqual(-2, id);
            server.Close();

        }

        [TestMethod]
        public void TestSynchronize()
        {
            NetworkServer server = NetworkServer.GetInstance();
            var client = new NetworkClient();
            var list = new FileList {List = new Dictionary<long, FileListEntry>()};
            var e1 = new FileListEntry();
            var e2 = new FileListEntry();
            var e3 = new FileListEntry();
            e1.Id = 1;
            e2.Id = 2;
            e3.Id = 3;
            list.List.Add(e1.Id, e1);
            list.List.Add(e2.Id, e2);
            list.List.Add(e3.Id, e3);
            var serverT = new Thread(server.Listen);
            serverT.Start();
            Thread.Sleep(1000);
            FileList returnList = client.SyncServer(list);
            ICollection<long> col = returnList.List.Keys;
            foreach(long l in col)
                Assert.AreEqual(returnList.List[l].Id, list.List[l].Id);
            FileListEntry ref1 = returnList.List[1];
            FileListEntry ref2 = returnList.List[2];
            FileListEntry ref3 = returnList.List[3];
            File file = new File();
            file.id = 1;
            file.name = "TESTFIL";
            FileMetaData data = new FileMetaData();
            long id = client.PushFile(file);
            
        }
    }
}
