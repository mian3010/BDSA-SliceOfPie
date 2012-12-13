using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SliceOfPie_Model.Persistence;

namespace ServerTest {
  [TestClass]
  public class ContextTest {
    private static readonly User TestUser = User.CreateUser("testcase@user.dk");
    private static readonly File File1 = File.CreateFile(1, "test1.txt", @"C:\ServerFiles\Test\", 0.0m);
    private static readonly File File2 = File.CreateFile(2, "test2.txt", @"C:\ServerFiles\Test\", 0.0m);
    private static readonly File File3 = File.CreateFile(3, "test3.txt", @"C:\ServerFiles\Test\", 0.0m);
    private static readonly FileInstance Fi1 = FileInstance.CreateFileInstance(1, TestUser.email, @"C:\ClientFiles\Test\", File1.id);
    private static readonly FileInstance Fi2 = FileInstance.CreateFileInstance(2, TestUser.email, @"C:\ClientFiles\Test\", File2.id);
    private static readonly FileInstance Fi3 = FileInstance.CreateFileInstance(3, TestUser.email, @"C:\ClientFiles\Test\", File3.id);
    private static readonly MetaDataType MetaDataType1 = MetaDataType.CreateMetaDataType("Test type 1");
    private static readonly MetaDataType MetaDataType2 = MetaDataType.CreateMetaDataType("Test type 2");
    private static readonly MetaDataType MetaDataType3 = MetaDataType.CreateMetaDataType("Test type 3");
    private static readonly FileMetaData FileMetaData11 = FileMetaData.CreateFileMetaData(1, MetaDataType1.ToString(), Fi1.File.id);
    private static readonly FileMetaData FileMetaData12 = FileMetaData.CreateFileMetaData(1, MetaDataType2.ToString(), Fi1.File.id);
    private static readonly FileMetaData FileMetaData13 = FileMetaData.CreateFileMetaData(1, MetaDataType3.ToString(), Fi1.File.id);
    private static readonly FileMetaData FileMetaData21 = FileMetaData.CreateFileMetaData(1, MetaDataType1.ToString(), Fi2.File.id);
    private static readonly FileMetaData FileMetaData22 = FileMetaData.CreateFileMetaData(1, MetaDataType2.ToString(), Fi2.File.id);
    private static readonly FileMetaData FileMetaData23 = FileMetaData.CreateFileMetaData(1, MetaDataType3.ToString(), Fi2.File.id);
    private static readonly FileMetaData FileMetaData31 = FileMetaData.CreateFileMetaData(1, MetaDataType1.ToString(), Fi3.File.id);
    private static readonly FileMetaData FileMetaData32 = FileMetaData.CreateFileMetaData(1, MetaDataType2.ToString(), Fi3.File.id);
    private static readonly FileMetaData FileMetaData33 = FileMetaData.CreateFileMetaData(1, MetaDataType3.ToString(), Fi3.File.id);



    [TestInitialize] [TestCleanup]
    public static void CleanUp() {
      //Context.CleanUp("VerySecretPasswordYoureNeverGonnaGuess");
      

    }
    
    // User test 
    [TestMethod]
    public void AddAndGetUser() {
      Console.WriteLine("Adding user " + TestUser.email);
      var success = Context.AddUser(TestUser);
      Console.WriteLine(success);
      var tempUser = Context.GetUser(TestUser.email);
      Assert.AreEqual(TestUser.email, tempUser.email);
    }

    [TestMethod]
    public void AddAndGetUser2() {
      Console.WriteLine("Adding user " + "");
      var success = Context.AddUser("");
      Console.WriteLine(success);
      Assert.AreEqual(-2, success);
    }

    // MetaData test
    [TestMethod]
    public void AddFileMetaData() {
      Context.AddFileMetaData(FileMetaData11);
      var temp = Context.GetMetaData(FileMetaData11.id);
      Assert.AreEqual(FileMetaData11, temp);
    }

    [TestMethod]
    public void AddFileMetaData2() {
      FileMetaData toAdd = FileMetaData11;
      toAdd.id = -200;
      var success = Context.AddFileMetaData(toAdd);
      Assert.AreEqual(success, -1);
    }

    [TestMethod]
    public void AddSomething() {
    }
    [TestMethod]
    public void GetSomething() {
    }
    [TestMethod]
    public void ModifySomething() {
    }
    [TestMethod]
    public void DeleteSomething() {
    }
  }
}
