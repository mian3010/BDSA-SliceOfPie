using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SliceOfPie_Model.Persistence;

namespace ServerTest
{
  [TestClass]
  public class ContextTest
  {
    private static readonly User TestUser = new User { email = "testcase@user.dk" };

    [TestInitialize]
    [TestCleanup]
    public void CleanUp()
    {
      //Context.CleanUp("VerySecretPasswordYoureNeverGonnaGuess");
    }

    // User test 
    [TestMethod]
    public void AddAndGetUser()
    {
      Console.WriteLine("Adding user " + TestUser.email);
      var success = Context.AddUser(TestUser);
      Console.WriteLine(success);
      var tempUser = Context.GetUser(TestUser.email);
      Assert.AreEqual(TestUser.email, tempUser.email);
    }
  }
}
