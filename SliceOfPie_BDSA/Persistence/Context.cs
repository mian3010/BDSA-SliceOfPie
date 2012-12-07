
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceOfPie_Model {
  /// <summary>
  /// Context has methods for operating on the database (in this case ITU MySQL).
  /// </summary>
  public static class Context {

    public static User[] GetUsers() {
      var DbContext = new SliceOfLifeEntities();
      var query = from u in DbContext.Users
                  select u;
      return query.ToArray<User>();
    }

    public static User GetUsers(string email) {
      var DbContext = new SliceOfLifeEntities();
      var query = from u in DbContext.Users
                  where u.email == email
                  select u;
      return (User)query;
    }

    // Does this return object hold a list of MetaDataTypes?
    public static FileMetaData[] GetMetaData(long FileId) {
      var DbContext = new SliceOfLifeEntities();
      var query = from meta in DbContext.FileMetaDatas
                  where meta.File_id == FileId
                  select meta;
      return query.ToArray<FileMetaData>();
    }

    public static void AddUser(string email){
      var DbContext = new SliceOfLifeEntities();
      User user = User.CreateUser(email);
      DbContext.AddToUsers(user);
      DbContext.SaveChanges();
    }

    public static void DeleteUser(string email) {
      var DbContext = new SliceOfLifeEntities();
      var query = from u in DbContext.Users
                  where u.email == email
                  select u;
      DbContext.DeleteObject(query.First());
      DbContext.SaveChanges();
    }

    public static Dictionary<long, FileListEntry> GetServerFileList(string email) {
      var DbContext = new SliceOfLifeEntities();
      var query = from e in DbContext.FileInstances
                  where e.User_email == email
                  select e;
      // Do stuff
      return null;
    }
  }
}
