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
    private static readonly SliceOfLifeEntities DbContext = new SliceOfLifeEntities();

    public static User[] GetUsers() {
      var query = from u in DbContext.Users
                  select u;
      return !query.Any<User>() ? null : query.ToArray<User>();
    }

    public static User GetUsers(string email) {
      if (email == null || email.Trim() == "") return null;
      var DbContext = new SliceOfLifeEntities();
      var query = from u in DbContext.Users
                  where u.email == email
                  select u;
      return !query.Any<User>() ? null : (User) query.First<User>();
    }

    // Does this return object hold a list of MetaDataTypes?
    public static FileMetaData[] GetMetaData(long FileId) {
      var query = from meta in DbContext.FileMetaDatas
                  where meta.File_id == FileId
                  select meta;
      return !query.Any<FileMetaData>() ? null : query.ToArray<FileMetaData>();
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
      if (!query.Any<User>()) return;
      DbContext.DeleteObject(query.First<User>());
      DbContext.SaveChanges();
    }

    public static Dictionary<long, FileListEntry> GetServerFileList(string email)
    {
      if (email == null || email.Trim() == "") return null;
      var query = from e in DbContext.FileInstances
                  where e.User_email == email
                  select e;
      if (!query.Any<FileInstance>()) return null;
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
      return query.First<File>();
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
      if (!query.Any<File>()) return -1;
      File tempFile = query.First<File>();
      if(tempFile.Equals(file)) return file.id;
      else return -1;
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
      if (!query.Any<File>()) return -1;
      File tempFile = query.First<File>();
      if (tempFile.Equals(file)) return file.id;
      else return -1;
    }

    public static FileList GetFileList(string userEmail) {
      FileList UsersFilesOnServer = new FileList();
      IDictionary<long, FileListEntry> List = UsersFilesOnServer.List;

      var query = from f in DbContext.FileInstances
                  where f.User_email == userEmail
                  select f;
      foreach (FileInstance fi in query) {
        List.Add(fi.File_id, FileListEntry.EntryFromFile(fi));
      }
      return UsersFilesOnServer;
    }
  }
}
