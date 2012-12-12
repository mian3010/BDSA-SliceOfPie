using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SliceOfPie_Model.Persistence;

namespace ServerTest {
  [TestClass]
  public class ContextTest {
    private static readonly User _testUser = User.CreateUser("testcase@user.dk");
    private static readonly File _file1 = File.CreateFile(1, "test1.txt", @"C:\ServerFiles\Test\", 0.0m);
    private static readonly File _file2 = File.CreateFile(2, "test2.txt", @"C:\ServerFiles\Test\", 0.0m);
    private static readonly File _file3 = File.CreateFile(3, "test3.txt", @"C:\ServerFiles\Test\", 0.0m);
    private static readonly FileInstance _fi1 = FileInstance.CreateFileInstance(1, _testUser.email, @"C:\ClientFiles\Test\", _file1.id);
    private static readonly FileInstance _fi2 = FileInstance.CreateFileInstance(2, _testUser.email, @"C:\ClientFiles\Test\", _file2.id);
    private static readonly FileInstance _fi3 = FileInstance.CreateFileInstance(3, _testUser.email, @"C:\ClientFiles\Test\", _file3.id);
    private static readonly MetaDataType _metaDataType1 = MetaDataType.CreateMetaDataType("Test type 1");
    private static readonly MetaDataType _metaDataType2 = MetaDataType.CreateMetaDataType("Test type 2");
    private static readonly MetaDataType _metaDataType3 = MetaDataType.CreateMetaDataType("Test type 3");
    private static readonly FileMetaData _fileMetaData11 = FileMetaData.CreateFileMetaData(1, _metaDataType1.ToString(), _fi1.File.id);
    private static readonly FileMetaData _fileMetaData12 = FileMetaData.CreateFileMetaData(1, _metaDataType2.ToString(), _fi1.File.id);
    private static readonly FileMetaData _fileMetaData13 = FileMetaData.CreateFileMetaData(1, _metaDataType3.ToString(), _fi1.File.id);
    private static readonly FileMetaData _fileMetaData21 = FileMetaData.CreateFileMetaData(1, _metaDataType1.ToString(), _fi2.File.id);
    private static readonly FileMetaData _fileMetaData22 = FileMetaData.CreateFileMetaData(1, _metaDataType2.ToString(), _fi2.File.id);
    private static readonly FileMetaData _fileMetaData23 = FileMetaData.CreateFileMetaData(1, _metaDataType3.ToString(), _fi2.File.id);
    private static readonly FileMetaData _fileMetaData31 = FileMetaData.CreateFileMetaData(1, _metaDataType1.ToString(), _fi3.File.id);
    private static readonly FileMetaData _fileMetaData32 = FileMetaData.CreateFileMetaData(1, _metaDataType2.ToString(), _fi3.File.id);
    private static readonly FileMetaData _fileMetaData33 = FileMetaData.CreateFileMetaData(1, _metaDataType3.ToString(), _fi3.File.id);

    [TestInitialize] [TestCleanup]
    public static void CleanUp() {
      
    }
    
    // User test 
    [TestMethod]
    public void AddAndGetUser() {
      Console.WriteLine("Adding user " + _testUser.email);
      var success = Context.AddUser(_testUser);
      Console.WriteLine(success);
      var tempUser = Context.GetUser(_testUser.email);
      Assert.AreEqual(_testUser.email, tempUser.email);
    }

    // MetaData test
    [TestMethod]
    public void AddFileMetaData() {

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

    [TestMethod]
    public void TestGetMetaDataFromFile();

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
