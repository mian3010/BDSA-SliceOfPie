using SliceOfPie_Model.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SliceOfPie_Model {
  /// <summary>
  /// Context has methods for operating on the database
  /// </summary>
  public static class Context {
    private static readonly SliceOfLifeEntities DbContext = new SliceOfLifeEntities();

    public static User[] GetUsers() {
      var query = from u in DbContext.Users
                  select u;
      return !query.Any() ? null : query.ToArray();
    }

    public static User GetUsers(string email) {
      if (email == null || email.Trim() == "") return null;
      var query = from u in DbContext.Users
                  where u.email == email
                  select u;
      return !query.Any() ? null : query.First();
    }

    // Does this return object hold a list of MetaDataTypes?
    public static FileMetaData[] GetMetaData(long fileId) {
      var query = from meta in DbContext.FileMetaDatas
                  where meta.File_id == fileId
                  select meta;
      return !query.Any() ? null : query.ToArray();
    }

    public static void AddUser(string email){
      if (email == null || email.Trim() == "") return;
      User user = User.CreateUser(email);
      DbContext.AddToUsers(user);
      DbContext.SaveChanges();
    }

    public static void DeleteUser(string email) {
      if (email == null || email.Trim() == "") return;
      var query = from u in DbContext.Users
                  where u.email == email
                  select u;
      if (!query.Any()) return;
      DbContext.DeleteObject(query.First());
      DbContext.SaveChanges();
    }

    public static Dictionary<long, FileListEntry> GetServerFileList(string email)
    {
      if (email == null || email.Trim() == "") return null;
      var query = from e in DbContext.FileInstances
                  where e.User_email == email
                  select e;
      if (!query.Any()) return null;
      foreach (var e in query.ToArray()) {
        Console.WriteLine(e.ToString());
      }
      throw new NotImplementedException();
    }

    public static File GetFile(long fileId){
      var query = from f in DbContext.Files
                  where f.id == fileId
                  select f;
      if (!query.Any<File>()) return null;
      return query.First();
    }

    public static long SaveFile(File file)
    {
      if (file == null) return -2;
      DbContext.Files.AddObject(file);
      DbContext.SaveChanges();

      // Make sure it was added correctly
      var query = from f in DbContext.Files
                  where f.id == file.id
                  select f;
      if (!query.Any()) return -1;
      var tempFile = query.First();
      if(tempFile.Equals(file)) return file.id;
      return -1;
    }

    public static long UpdateFile(File file) {
      //TODO Change instaead of delete'n'add
      if (file == null) return -2;
      DbContext.Files.DeleteObject(file);
      DbContext.Files.AddObject(file);
      DbContext.SaveChanges();

      // Make sure it was added correctly
      var query = from f in DbContext.Files
                  where f.id == file.id
                  select f;
      if (!query.Any()) return -1;
      var tempFile = query.First();
      if (tempFile.Equals(file)) return file.id;
      return -1;
    }

    public static FileList GetFileList(string userEmail) {
      var usersFilesOnServer = new FileList();
      IDictionary<long, FileListEntry> list = usersFilesOnServer.List;

      var query = from f in DbContext.FileInstances
                  where f.User_email == userEmail
                  select f;
      foreach (FileInstance fi in query) {
        list.Add(fi.File_id, FileListEntry.EntryFromFile(fi));
      }
      return usersFilesOnServer;
    }
  }
}
