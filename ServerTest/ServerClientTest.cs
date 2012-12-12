using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SliceOfPie_Model;
using SliceOfPie_Model.Persistence;
using SliceOfPie_Server;
using System.Threading;
using System.Collections.Generic;

namespace ServerTest {

  [TestClass]
  public class ServerClientTest {
    [TestMethod]
    public void TestGetFile() {
      NetworkServer server = NetworkServer.GetInstance();
      var client = new NetworkClient();
      const int id = 1;
      var serverT = new Thread(server.Listen);
      serverT.Start();
      Thread.Sleep(1000);
      var file = client.PullFile(id);
      Console.Out.WriteLine(file.id);
      server.Close();
    }

      [TestMethod]
      public void TestAddFile()
      {
          File f = new File();
          f.name = " Newfile";
          f.serverpath = "Lolpath";
          Context2.AddFile(f);
      

      }

      [TestMethod]
      public void TestAddUser()
      {
          User u = new User();
          u.email = "yo@yo.dk";
          Context2.AddUser(u);
      }

    /// <summary>
    /// Tests that you cannot save a file that is not in the FileList
    /// </summary>
    [TestMethod]
    public void TestSaveFileNegative() {
      var server = NetworkServer.GetInstance();
      var client = new NetworkClient();
      var file = File.CreateFile(10, "TestFileName", @"C:\Some\Path\", 1.0M);
      var doc = new Document {File = file};
      var data = new FileMetaData { value = "HALLO" };
      var type = new MetaDataType { Type = "test metadatatype" };
      data.MetaDataType = type;
      doc.id = 2;
      doc.File.name = "Testfile";
      var serverT = new Thread(server.Listen);
      serverT.Start();
      Thread.Sleep(1000);
      int id = client.PushFile(doc);
      Assert.AreEqual(-2, id);
      server.Close();

    }

    [TestMethod]
    public void TestSynchronize() {
      NetworkServer server = NetworkServer.GetInstance();
      var client = new NetworkClient();
      var list = new FileList { List = new Dictionary<int, FileListEntry>() };
      var e1 = new FileListEntry();
      var e2 = new FileListEntry();
      var e3 = new FileListEntry();
      e1.Id = 30;
      e2.Id = 31;
      e3.Id = 32;
      list.List.Add(e1.Id, e1);
      list.List.Add(e2.Id, e2);
      list.List.Add(e3.Id, e3);
      var serverT = new Thread(server.Listen);
      serverT.Start();
      //Thread.Sleep(5000);
      FileList returnList = client.SyncServer(list);
      ICollection<int> col = returnList.List.Keys;
      foreach (int l in col)
        Assert.AreEqual(returnList.List[l].Id, list.List[l].Id);
      var data = new FileMetaData();
      data.value = "testdatatype";
      MetaDataType type = new MetaDataType();
      type.Type = "Hey";
      data.MetaDataType_Type = type.Type;
      User user = new User();
      user.email = "superman123@gmail.com";
      File file = new File();
      file.id = 31;
      file.name = "TES123TFIL"; 
      file.serverpath = "test123Serverpath"; 
      file.Version = 0.0m;
 //      file.FileMetaDatas.Add(data);
      var fileInstance = FileInstance.CreateFileInstance(31, user.email, @"C:\ClientFiles\Test\", file.id);
        fileInstance.User = user;

      fileInstance.File = file;
      
      int id = client.PushFile(fileInstance);

    }
  }
}
