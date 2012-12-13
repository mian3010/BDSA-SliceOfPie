using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SliceOfPie_Model.Persistence {
  public static class Context2 {
    private static readonly SliceOfLifeEntities DbContext = new SliceOfLifeEntities();

    // User
    public static User GetUser(string email) {
      if (email == null || email.Trim().Equals("")) return null;
      var query = from u in DbContext.Users
                  where u.email == email
                  select u;
      return !query.Any() ? null : query.First();
    }

    public static int AddUser(User user) {
      if (user == null || user.email == null || user.email.Trim().Equals("")) return -2;
      if (GetUser(user.email) != null) return 0;
      DbContext.Users.AddObject(user);
      try {
        return DbContext.SaveChanges();
      } catch (UpdateException) {
        return -1;
      }
    }

    public static FileMetaData GetFileMetaData(File lol, string merlol)
    {
        throw new NotImplementedException();
    }

    // FileInstance
    public static FileInstance GetFileInstance(int fileInstanceId) {
      if (fileInstanceId < 1) return null;
      var query = from f in DbContext.FileInstances
                  .Include("File")
                  .Include("File.Change")
                  where f.id == fileInstanceId
                  select f;
      return !query.Any() ? null : query.First();
    }

    public static int AddFileInstance(FileInstance fileInstance) {
      if (fileInstance == null) throw new ConstraintException("Database handler received an empty reference");
      bool deleteBeforeAdd = false;
      // Check for lots of constraints
      // Id
      if (fileInstance.id < 1) throw new ConstraintException("ID must be greater than 0");
      if (GetFileInstance(fileInstance.id) != null) deleteBeforeAdd = true;
        
      // Path
      if (fileInstance.path == null || fileInstance.path.Trim().Equals("")) throw new ConstraintException("Invalid file path");

      // User
      if (fileInstance.User_email == null || fileInstance.User_email.Trim().Equals("")) throw new ConstraintException("Invalid user");
      if (GetUser(fileInstance.User_email) == null) throw new ConstraintException("No user known under that name");
        //Sets the user from fileInstance to the user from the database
      fileInstance.User = GetUser(fileInstance.User_email);

      // File
      if (fileInstance.File == null) throw new ConstraintException("Database handler received an empty file reference");

      // File id
      if (fileInstance.File_id < 0) throw new ConstraintException("FileID must be greater than 0");

      // File name
      if (fileInstance.File.name == null || fileInstance.File.name.Trim().Equals("")) throw new ConstraintException("Invalid file name");

      // File serverpath
      if (fileInstance.File.serverpath == null || fileInstance.File.serverpath.Trim().Equals("")) throw new ConstraintException("Invalid server file path");

      // File Version
      if (fileInstance.File.Version < 0) throw new ConstraintException("Invalid file version");

      if (deleteBeforeAdd) {
        DbContext.FileInstances.DeleteObject(fileInstance);
      }
      DbContext.FileInstances.AddObject(fileInstance);
      try {
        return DbContext.SaveChanges();
      } catch (UpdateException e) {
        throw new ConstraintException("Database handler received an error when trying saving changes to the database", e);
      }
    }

    public static List<FileInstance> GetFiles(string useremail) {
      if (useremail == null || useremail.Trim().Equals("")) return null;
      var query = from f in DbContext.FileInstances
                  where f.User_email.Equals(useremail)
                  select f;
      return !query.Any() ? null : query.ToList();
    }

    public static FileList GetFileList(string useremail) {
      var usersFilesOnServer = new FileList();
      var list = usersFilesOnServer.List;
      var query = from f in DbContext.FileInstances
                  where f.User_email == useremail
                  select f;
      if (!query.Any()) {
      foreach (FileInstance fi in query) {
        list.Add(fi.File_id, FileListEntry.EntryFromFile(fi));
      }
          }
      return usersFilesOnServer;
    }

    public static int GetNextFileId() {
      var query = (from f in DbContext.Files
                   select f.id).Max();
      return query;
    }

    public static int GetNextFileInstanceId() {
      var query = (from f in DbContext.FileInstances
                   select f.id).Max();
      return query;
    }

    public static int GetNextChangeId() {
      var query = (from e in DbContext.Changes
                   select e.id).Max();
      return query;
    }

    public static int GetNextProjectId() {
      var query = (from e in DbContext.Projects
                   select e.id).Max();
      return query;
    }

    //TODO Should be private or removed
    public static File GetFile(int fileId) {
      if (fileId < 0) return null;
      var query = from f in DbContext.Files
                  where f.id == fileId
                  select f;
      return !query.Any() ? null : query.First();
    }

    //TODO Should be private or removed
    public static void AddFile(File file) {
      file.FileMetaDatas.Clear();
      DbContext.Files.AddObject(file);
      try {
        DbContext.SaveChanges();
      } catch (UpdateException e) {
        throw new ConstraintException("Database handler received an error when trying saving changes to the database", e);
      }
    }

    public static void CleanUp(string password) {
      if (!password.Equals("VerySecretPasswordYoureNeverGonnaGuess")) return;
      // Change
      var query = from e in DbContext.Changes
                  select e;
      if (query.Any()) {
        foreach (var element in query) {
          DbContext.Changes.DeleteObject(element);
        }
        try {
          DbContext.SaveChanges();
        } catch (UpdateException) { }
      }

      // Project
      var query2 = from e in DbContext.Projects
                   select e;
      if (query2.Any()) {
        foreach (var element in query2) {
          DbContext.Projects.DeleteObject(element);
        }
        try {
          DbContext.SaveChanges();
        } catch (UpdateException) { }
      }

      // MetaDataType
      var query3 = from e in DbContext.MetaDataTypes
                   select e;
      if (query3.Any()) {
        foreach (var element in query3) {
          DbContext.MetaDataTypes.DeleteObject(element);
        }
        try {
          DbContext.SaveChanges();
        } catch (UpdateException) { }
      }

      // FileMetaData
      var query4 = from e in DbContext.FileMetaDatas
                   select e;
      if (query4.Any()) {
        foreach (var element in query4) {
          DbContext.FileMetaDatas.DeleteObject(element);
        }
        try {
          DbContext.SaveChanges();
        } catch (UpdateException) { }
      }

      // FileInstace
      var query5 = from e in DbContext.FileInstances
                   select e;
      if (query5.Any()) {
        foreach (var element in query5) {
          DbContext.FileInstances.DeleteObject(element);
        }
        try {
          DbContext.SaveChanges();
        } catch (UpdateException) { }
      }

      // File
      var query6 = from e in DbContext.Files
                   select e;
      if (query6.Any()) {
        foreach (var element in query6) {
          DbContext.Files.DeleteObject(element);
        }
        try {
          DbContext.SaveChanges();
        } catch (UpdateException) { }
      }

      // Users
      var query7 = from e in DbContext.Users
                   select e;
      if (query7.Any()) {
        foreach (var element in query7) {
          DbContext.Users.DeleteObject(element);
        }
        try {
          DbContext.SaveChanges();
        } catch (UpdateException) { }
      }

      // Add Users
      for (int i = 0; i < 10; i++) {
        var user = User.CreateUser("testuser" + i + "@example.com");
        DbContext.Users.AddObject(user);
      }

      // Add Files
      for (int i = 0; i < 10; i++) {
        var file = File.CreateFile(i, "Testfile" + i, @"C:\ServerTestFiles\", 0.0m);
        if (i % 2 == 0) file.serverpath += "Subfolder";
        DbContext.Files.AddObject(file);
      }

      // Add FileInstances
      for (int i = 0; i < 10; i++) {
        var fileInstance = FileInstance.CreateFileInstance(i, "testuser" + i, @"C:\ClientTestFiles\", i);
        if (i % 2 == 0) fileInstance.path += "Subfolder";
        DbContext.FileInstances.AddObject(fileInstance);
      }
      DbContext.SaveChanges();
    }
  }
}