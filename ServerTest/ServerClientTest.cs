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
      FileInstance instance = client.PushFile(doc);
      Assert.AreEqual(-2, instance.id);
      server.Close();

    }

    [TestMethod]
    public void TestSynchronize() {
      Context2.CleanUp("VerySecretPasswordYoureNeverGonnaGuess");
      NetworkServer server = NetworkServer.GetInstance();
      var client = new NetworkClient();
      var list = new FileList { List = new Dictionary<int, FileListEntry>() };
      var e1 = new FileListEntry();
      var e2 = new FileListEntry();
      var e3 = new FileListEntry();
      e1.Id = -40;
      e2.Id = 40;
      e3.Id = 350;
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
      data.value = "testd234atatype";
      MetaDataType type = new MetaDataType();
      type.Type = "He234y";
      data.MetaDataType_Type = type.Type;
      data.MetaDataType = type;
      User user = new User();
      user.email = "superman123@gm44ail.com";
      Context2.AddUser(user);
      File file = new File();
      file.name = "TES34123TFIL"; 
      file.serverpath = "test123Se234rverpath"; 
      file.Version = 0.0m;
      //file.FileMetaDatas.Add(data);
      var fileInstance = new FileInstance();
      fileInstance.id = -40;
      fileInstance.path = "//test";
      fileInstance.User = user;
      fileInstance.File = file;
      FileInstance instance = client.PushFile(fileInstance);
      if (instance != null)
      {
        Assert.AreEqual(instance.id, fileInstance.id);
        Assert.AreEqual(instance.File.id, file.id);
        Assert.AreEqual(instance.User.email, user.email);
      }
      server.Close();
      

    }
  }
}
