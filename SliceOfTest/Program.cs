using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SliceOfPie_Model;

namespace SliceOfTest {
  class Program {
    public static void Main(string[] args) {
      Console.WriteLine("Adding user");
      Context.AddUser("Hejsa");
      User user = Context.GetUsers().First();
      Console.WriteLine("User: " + user.email);
    }
  }
}
