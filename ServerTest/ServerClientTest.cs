﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SliceOfPie_Model;
using SliceOfPie_Model.Persistence;
using SliceOfPie_Server;
using System.Threading;
using System.Collections.Generic;

namespace ServerTest {

  [TestClass]
  public class ServerClientTest {

      private FileList list;
      private Document i1;
      private Document i2;
      private Document i3;

      public void CreateTestList()
      {
          list = new FileList { List = new Dictionary<int, FileListEntry>() };
          var e1 = new FileListEntry();
          var e2 = new FileListEntry();
          var e3 = new FileListEntry();
          e1.Id = -40;
          e2.Id = 40;
          e3.Id = 350;
          list.List.Add(e1.Id, e1);
          list.List.Add(e2.Id, e2);
          list.List.Add(e3.Id, e3);
      }

   

      public void CreateTestFiles()
      {
          User user = new User();
          user.email = "superman123@gm44ail.com";
          Context2.AddUser(user);
          i1 = new Document();
          i2 = new Document();
          i3 = new Document();
          i1.path = "docTestPath1";
          i2.path = "docTestPath2";
          i3.path = "docTestPath3";
          File file1 = new File();
          file1.name = "TestName1";
          file1.serverpath = "TestPath2";
          file1.Version = 0.0m;
          File file2 = new File();
          file2.name = "TestName2";
          file2.serverpath = "TestPath2";
          file2.Version = 0.0m;
          File file3 = new File();
          file3.name = "TestName3";
          file3.serverpath = "TestPath3";
          file3.Version = 0.0m;
          i1.File = file1;
          i1.User = user;
          i2.File = file2;
          i2.User = user;
          i3.File = file3;
          i3.User = user;
      }
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
    public void TestSynchronizeSimple() {
      Context2.CleanUp("VerySecretPasswordYoureNeverGonnaGuess");
      NetworkServer server = NetworkServer.GetInstance();
      var client = new NetworkClient();
      CreateTestList();
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
      // data.MetaDataType_Type = type.Type;
      data.MetaDataType = type;
      User user = new User();
        Context2.AddUser(user);
        user.email = "superman123@gmail.com";

      File file = new File();
      file.name = "TES34123TFIL"; 
      file.serverpath = "test123Se234rverpath"; 
      file.Version = 0.0m;
      file.FileMetaDatas.Add(data);
      var fileInstance = FileInstance.CreateFileInstance(32, user.email, @"C:\ClientFiles\Test\", file.id);
       fileInstance.User = user;

      FileInstance instance = client.PushFile(fileInstance);
      if (instance != null)
      {
        Assert.AreEqual(instance.id, fileInstance.id);
        Assert.IsTrue(instance.File.id > 0);
        Assert.AreEqual(instance.User.email, user.email);
      }
      server.Close();
      

    }

    [TestMethod]
    public void TestSynchronizeFull()
    {
        //Context2.CleanUp("VerySecretPasswordYoureNeverGonnaGuess");
        CreateTestList();
        NetworkServer server = NetworkServer.GetInstance();
        var serverT = new Thread(server.Listen);
        serverT.Start();
        Thread.Sleep(1000);
        OfflineAdministrator admin = OfflineAdministrator.GetInstance();
        CreateTestFiles();
        admin.AddFile(i1);
        admin.AddFile(i2);
        admin.AddFile(i3);
        admin.Synchronize();
    }
  }
}
