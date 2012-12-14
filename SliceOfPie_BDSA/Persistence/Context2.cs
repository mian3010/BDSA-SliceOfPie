using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace SliceOfPie_Model.Persistence {
  public static class Context2 {


    private static FileInstance CreateFileInstance(FileInstance doc) {
      FileInstance instance = FileInstance.CreateFileInstance(doc.id, doc.UserEmail, doc.path, doc.File_id);
      instance.File = doc.File;
      instance.User = doc.User;
      return instance;
    }
    // User
    public static User GetUser(string email) {
      using (var dbContext = new SliceOfLifeEntities()) {
        return GetUserWithContext(email, dbContext);
      }
    }

    private static User GetUserWithContext(string email, SliceOfLifeEntities dbContext) {
      if (email == null || email.Trim().Equals(""))
        return null;
      var query = from u in dbContext.Users
                  where u.email == email
                  select u;
      return !query.Any() ? null : query.First();
    }

    public static int AddUser(User user) {
      using (var dbContext = new SliceOfLifeEntities()) {
        if (user == null || user.email == null || user.email.Trim().Equals("")) return -2;
        if (GetUser(user.email) != null) return 0;
        dbContext.Users.AddObject(user);
        try {
          return dbContext.SaveChanges();
        } catch (UpdateException) {
          return -1;
        }
      }
    }

    public static FileMetaData GetFileMetaData(File file, string metaDataType) {
      var query = from meta in file.FileMetaDatas
                  where meta.MetaDataType_Type.Equals(metaDataType)
                  select meta;
      return !query.Any() ? null : query.First();
    }

    public static FileInstance GetFileInstance(int fileInstanceId)
    {
        FileInstance instance;
        using (var dbContext = new SliceOfLifeEntities()) {
            instance =  GetFileInstanceWithContext(fileInstanceId, dbContext);
        }
        return instance;
    }
    // FileInstance
    private static FileInstance GetFileInstanceWithContext(int fileInstanceId, SliceOfLifeEntities dbContext) {
      
        if (fileInstanceId < 1) return null;
        var query = from f in dbContext.FileInstances
                                       .Include("File")
                                       .Include("File.Changes")
                                       .Include("File.FileMetaDatas")
                    where f.id == fileInstanceId
                    select f;
        return !query.Any() ? null : query.First();
      
    }

    public static FileInstance AddFileInstance(FileInstance fileInstance) {
      //FileInstance fileInstance = CreateFileInstance(Instance);
      using (var dbContext = new SliceOfLifeEntities()) {
        //Assembly a = typeof(Document).Assembly;
        //dbContext.MetadataWorkspace.LoadFromAssembly(a);
        if (fileInstance == null) throw new ConstraintException("Database handler received an empty reference");
        

        // Check for lots of constraints

        // File
        if (fileInstance.File == null) throw new ConstraintException("Database handler received an empty file reference");

        // Path
        if (fileInstance.path == null || fileInstance.path.Trim().Equals(""))
          throw new ConstraintException("Invalid file path");

        User tmp = GetUserWithContext(fileInstance.User_email, dbContext);
        if (tmp == null) throw new ConstraintException("User not registered in the database");
        fileInstance.User = tmp;

        // File name
        if (fileInstance.File.name == null || fileInstance.File.name.Trim().Equals(""))
          throw new ConstraintException("Invalid file name");

        // File serverpath
        if (fileInstance.File.serverpath == null || fileInstance.File.serverpath.Trim().Equals(""))
          throw new ConstraintException("Invalid server file path");

        // File Version
        if (fileInstance.File.Version < 0) throw new ConstraintException("Invalid file version");
        dbContext.FileInstances.AddObject(fileInstance);
        try {
          dbContext.SaveChanges();
        } catch (UpdateException e) {
          throw new ConstraintException(
            "Database handler received an error when trying saving changes to the database", e);
        }
      }
      return fileInstance;
    }

    public static List<FileInstance> GetFiles(string useremail) {
      using (var dbContext = new SliceOfLifeEntities()) {
        if (useremail == null || useremail.Trim().Equals("")) return null;
        var query = from f in dbContext.FileInstances
                    where f.User_email.Equals(useremail)
                    select f;
        return !query.Any() ? null : query.ToList();
      }
    }

    public static FileList GetFileList(string useremail) {
      using (var dbContext = new SliceOfLifeEntities()) {
        var usersFilesOnServer = new FileList();
        var list = usersFilesOnServer.List;
        var query = from f in dbContext.FileInstances
                    where f.User_email == useremail
                    select f;
        if (!query.Any()) {
          foreach (FileInstance fi in query) {
            list.Add(fi.File_id, FileListEntry.EntryFromFile(fi));
          }
        }
        return usersFilesOnServer;
      }
    }

    //TODO Should be private or removed
    public static File GetFile(int fileId) {
      if (fileId < 0) return null;
      using (var dbContext = new SliceOfLifeEntities()) {
        var query = from f in dbContext.Files
                    where f.id == fileId
                    select f;
        return !query.Any() ? null : query.First();
      }
    }

    //TODO Should be private or removed
    public static void AddFile(File file) {
      using (var dbContext = new SliceOfLifeEntities()) {
        file.FileMetaDatas.Clear();
        dbContext.Files.AddObject(file);
        try {
          dbContext.SaveChanges();
        } catch (UpdateException e) {
          throw new ConstraintException(
            "Database handler received an error when trying saving changes to the database", e);
        }
      }
    }

    public static void CleanUp(string password) {
      using (var dbContext = new SliceOfLifeEntities()) {
        if (!password.Equals("VerySecretPasswordYoureNeverGonnaGuess")) return;
        // Change
        var query = from e in dbContext.Changes
                    select e;
        if (query.Any()) {
          foreach (var element in query) {
            dbContext.Changes.DeleteObject(element);
          }
          try {
            dbContext.SaveChanges();
          } catch (UpdateException) { }
        }

        // Project
        var query2 = from e in dbContext.Projects
                     select e;
        if (query2.Any()) {
          foreach (var element in query2) {
            dbContext.Projects.DeleteObject(element);
          }
          try {
            dbContext.SaveChanges();
          } catch (UpdateException) { }
        }

        // MetaDataType
        var query3 = from e in dbContext.MetaDataTypes
                     select e;
        if (query3.Any()) {
          foreach (var element in query3) {
            dbContext.MetaDataTypes.DeleteObject(element);
          }
          try {
            dbContext.SaveChanges();
          } catch (UpdateException) { }
        }

        // FileMetaData
        var query4 = from e in dbContext.FileMetaDatas
                     select e;
        if (query4.Any()) {
          foreach (var element in query4) {
            dbContext.FileMetaDatas.DeleteObject(element);
          }
          try {
            dbContext.SaveChanges();
          } catch (UpdateException) { }
        }

        // FileInstace
        var query5 = from e in dbContext.FileInstances
                     select e;
        if (query5.Any()) {
          foreach (var element in query5) {
            dbContext.FileInstances.DeleteObject(element);
          }
          try {
            dbContext.SaveChanges();
          } catch (UpdateException) { }
        }

        // File
        var query6 = from e in dbContext.Files
                     select e;
        if (query6.Any()) {
          foreach (var element in query6) {
            dbContext.Files.DeleteObject(element);
          }
          try {
            dbContext.SaveChanges();
          } catch (UpdateException) { }
        }

        // Users
        var query7 = from e in dbContext.Users
                     select e;
        if (query7.Any()) {
          foreach (var element in query7) {
            dbContext.Users.DeleteObject(element);
          }
          try {
            dbContext.SaveChanges();
          } catch (UpdateException) { }
        }

        // Reset AI

        // Test user
        User user1 = new User();
        user1.email = "superman123456@gm44ail.com";
        Context2.AddUser(user1);

        // Add MetaType
        var metaType = MetaDataType.CreateMetaDataType("Type");
        const string metaValue = "Document";

        for (int i = 0; i < 1; i++) {
          // Add Users
          var user = User.CreateUser("testuser" + i + "@example.com");
          dbContext.Users.AddObject(user);

          var count = 1;
          for (int k = 0; k < 10; k++) {
            // Add Files
            var file = File.CreateFile(i, "Testfile" + i + "" + k, @"C:\ServerTestFiles\", 0.0m);
            if (i % 2 == 0) file.serverpath += "Subfolder";

            // Meta
            var meta = new FileMetaData() {
              id = count,
              value = metaValue,
              MetaDataType = metaType
            };
            file.FileMetaDatas.Add(meta);
            //dbContext.Files.AddObject(file);

            // Add FileInstances
            var fileInstance = FileInstance.CreateFileInstance(count++, "testuser" + i + "" + k, @"C:\ClientTestFiles\", file.id);
            if (k % 2 == 0) fileInstance.path += @"Subfolder\";
            if (k % 3 == 0) fileInstance.path += @"AnotherSubFolder\";
            if (k % 7 == 0) fileInstance.path += @"YetAnotherSubFolder\";
            if (k % 5 == 0) fileInstance.path += @"SomeSubFolder\";
            fileInstance.File = file;
            fileInstance.User = user;
            dbContext.FileInstances.AddObject(fileInstance);

            try {
              dbContext.SaveChanges();
            } catch (UpdateException e) {
              throw new ConstraintException("Problem with adding test entries", e);
            }
          }
        }

      }
    }
  }
}