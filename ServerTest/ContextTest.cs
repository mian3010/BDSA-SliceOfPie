using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SliceOfPie_Model.Persistence;

namespace ServerTest
{
  [TestClass]
  public class ContextTest
  {
    private static readonly User TestUser = new User { email = "testcase@user.dk" };
    private static readonly File File1 = new File { name = "test1.txt", serverpath = @"C:\ServerFiles\Test\", Version = 0.0m };
    private static readonly File File2 = new File { id = 2, name = "test2.txt", serverpath = @"C:\ServerFiles\Test\", Version = 0.0m };
    private static readonly File File3 = new File { id = 3, name = "test3.txt", serverpath = @"C:\ServerFiles\Test\", Version = 0.0m };
    private static readonly FileInstance Fi1 = new FileInstance { id = 1, User_email = TestUser.email, path = @"C:\ClientFiles\Test\", File_id = File1.id };
    private static readonly FileInstance Fi2 = new FileInstance { id = 2, User_email = TestUser.email, path = @"C:\ClientFiles\Test\", File_id = File2.id };
    private static readonly FileInstance Fi3 = new FileInstance { id = 3, User_email = TestUser.email, path = @"C:\ClientFiles\Test\", File_id = File3.id };
    private static readonly MetaDataType MetaDataType1 = new MetaDataType { Type = "Test type 1" };
    private static readonly MetaDataType MetaDataType2 = new MetaDataType { Type = "Test type 2" };
    private static readonly MetaDataType MetaDataType3 = new MetaDataType { Type = "Test type 3" };
    private static readonly FileMetaData FileMetaData11 = new FileMetaData { id = 1, MetaDataType_Type = MetaDataType1.ToString(), File_id = Fi1.File.id };
    private static readonly FileMetaData FileMetaData12 = new FileMetaData { id = 1, MetaDataType_Type = MetaDataType2.ToString(), File_id = Fi1.File.id };
    private static readonly FileMetaData FileMetaData13 = new FileMetaData { id = 1, MetaDataType_Type = MetaDataType3.ToString(), File_id = Fi1.File.id };
    private static readonly FileMetaData FileMetaData21 = new FileMetaData { id = 1, MetaDataType_Type = MetaDataType1.ToString(), File_id = Fi2.File.id };
    private static readonly FileMetaData FileMetaData22 = new FileMetaData { id = 1, MetaDataType_Type = MetaDataType2.ToString(), File_id = Fi2.File.id };
    private static readonly FileMetaData FileMetaData23 = new FileMetaData { id = 1, MetaDataType_Type = MetaDataType3.ToString(), File_id = Fi2.File.id };
    private static readonly FileMetaData FileMetaData31 = new FileMetaData { id = 1, MetaDataType_Type = MetaDataType1.ToString(), File_id = Fi3.File.id };
    private static readonly FileMetaData FileMetaData32 = new FileMetaData { id = 1, MetaDataType_Type = MetaDataType2.ToString(), File_id = Fi3.File.id };
    private static readonly FileMetaData FileMetaData33 = new FileMetaData { id = 1, MetaDataType_Type = MetaDataType3.ToString(), File_id = Fi3.File.id };



    [TestInitialize]
    [TestCleanup]
    public static void CleanUp()
    {
      //Context.CleanUp("VerySecretPasswordYoureNeverGonnaGuess");


    }

    // User test 
    /*
    [TestMethod]
    public void AddAndGetUser()
    {
      Console.WriteLine("Adding user " + TestUser.email);
      var success = Context.AddUser(TestUser);
      Console.WriteLine(success);
      var tempUser = Context.GetUser(TestUser.email);
      Assert.AreEqual(TestUser.email, tempUser.email);
    }

    [TestMethod]
    public void AddAndGetUser2()
    {
      Console.WriteLine("Adding user " + "");
      var success = Context.AddUser("");
      Console.WriteLine(success);
      Assert.AreEqual(-2, success);
    }

    // MetaData test
    [TestMethod]
    public void AddFileMetaData()
    {
      Context.AddFileMetaData(FileMetaData11);
      var temp = Context.GetMetaData(FileMetaData11.id);
      Assert.AreEqual(FileMetaData11, temp);
    }

    [TestMethod]
    public void AddFileMetaData2()
    {
      FileMetaData toAdd = FileMetaData11;
      toAdd.id = -200;
      var success = Context.AddFileMetaData(toAdd);
      Assert.AreEqual(success, -1);
    }

    [TestMethod]
    public void AddSomething()
    {
    }
    [TestMethod]
    public void GetSomething()
    {
    }
    [TestMethod]
    public void ModifySomething()
    {
    }
    [TestMethod]
    public void DeleteSomething()
    {
    }
     * */
  }
}
