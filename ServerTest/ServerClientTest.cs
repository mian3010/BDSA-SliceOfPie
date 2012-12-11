using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SliceOfPie_Model;
using SliceOfPie_Model.Persistence;
using SliceOfPie_Server;
using System.Threading;
using System.Data.Entity;
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
            NetworkClient client = new NetworkClient();
            long id = new long();
            Thread serverT = new Thread(() => server.listen());
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
            NetworkClient client = new NetworkClient();
            Document doc = new Document();
            FileMetaData data = new FileMetaData();
            data.value = "HALLO";
            MetaDataType type = new MetaDataType();
            type.Type = "test metadatatype";
            data.MetaDataType = type;
            doc.id = 1;
            doc.name = "Testfile";
            Thread serverT = new Thread(() => server.listen());
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
            NetworkClient client = new NetworkClient();
            FileList list = new FileList();
            list.List = new Dictionary<long, FileListEntry>();
            FileListEntry e1 = new FileListEntry();
            FileListEntry e2 = new FileListEntry();
            FileListEntry e3 = new FileListEntry();
            e1.Id = 1;
            e2.Id = 2;
            e3.Id = 3;
            list.List.Add(e1.Id, e1);
            list.List.Add(e2.Id, e2);
            list.List.Add(e3.Id, e3);
            Thread serverT = new Thread(() => server.listen());
            serverT.Start();
            Thread.Sleep(1000);
            FileList returnList = client.SyncServer(list);
        }
    }
}
