using SliceOfPie_Model.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceOfPie_Model {
  /// <summary>
  /// Context has methods for operating on the database
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
      DbContext.DeleteObject(query.First<User>());
      DbContext.SaveChanges();
    }

    public static Dictionary<long, FileListEntry> GetServerFileList(string email) {
      var DbContext = new SliceOfLifeEntities();
      var query = from e in DbContext.FileInstances
                  where e.User_email == email
                  select e;
      foreach (var e in query.ToArray()) {
        Console.WriteLine(e.ToString());
      }
      throw new NotImplementedException();
    }

    public static File GetFile(long fileId){
      var DbContext = new SliceOfLifeEntities();
      var query = from f in DbContext.Files
                  where f.id == fileId
                  select f;
      return query.First<File>();
    }

    public static long SaveFile(File file) {
      var DbContext = new SliceOfLifeEntities();
      DbContext.Files.AddObject(file);
      DbContext.SaveChanges();

      // Make sure it was added correctly
      var query = from f in DbContext.Files
                  where f.id == file.id
                  select f;
      File tempFile = query.First<File>();
      if(tempFile.Equals(file)) return file.id;
      else return -1;
    }

    public static long UpdateFile(File file) {
      //TODO Change instaead of delete'n'add
      var DbContext = new SliceOfLifeEntities();
      DbContext.Files.DeleteObject(file);
      DbContext.Files.AddObject(file);
      DbContext.SaveChanges();

      // Make sure it was added correctly
      var query = from f in DbContext.Files
                  where f.id == file.id
                  select f;
      File tempFile = query.First<File>();
      if (tempFile.Equals(file)) return file.id;
      else return -1;
    }
  }
}
