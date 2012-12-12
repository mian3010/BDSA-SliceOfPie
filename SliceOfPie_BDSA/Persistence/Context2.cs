using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SliceOfPie_Model.Persistence {
  public class Context2 {
    private static readonly SliceOfLifeEntities DbContext = new SliceOfLifeEntities();

    // User
    public static User GetUser(string email) {
      if (email == null || email.Trim().Equals("")) return null;
      var query = from u in DbContext.Users
                  where u.email == email
                  select u;
      return !query.Any() ? null : query.First();
    }

    public static long AddUser(User user) {
      if (user == null || user.email == null || user.email.Trim().Equals("")) return -2;
      if (GetUser(user.email) != null) return 0;
      DbContext.Users.AddObject(user);
      try {
        return DbContext.SaveChanges();
      } catch (UpdateException) {
        return -1;
      }
    }

    // FileInstance
    public static FileInstance GetFileInstance(long fileInstanceId) {
      if (fileInstanceId < 1) return null;
      var query = from f in DbContext.FileInstances
                  where f.id == fileInstanceId
                  select f;
      return !query.Any() ? null : query.First();
    }

    public static long AddFileInstance(FileInstance fileInstance) {
      if (fileInstance == null) return -2;
      // Check for lots of constraints
      if (GetFileInstance(fileInstance.id) != null) {
        DbContext.FileInstances.DeleteObject(fileInstance);
      }
      DbContext.FileInstances.AddObject(fileInstance);
      try {
        return DbContext.SaveChanges();
      } catch (UpdateException) {
        return -1;
      }
    }

    public static List<FileInstance> GetFiles(string useremail) {
      if (useremail == null || useremail.Trim().Equals("")) return null;
      var query = from f in DbContext.FileInstances
                  where f.User_email.Equals(useremail)
                  select f;
      return !query.Any() ? null : query.ToList();
    }
  }
}