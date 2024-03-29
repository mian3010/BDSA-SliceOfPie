﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using System.Reflection;

namespace SliceOfPie_Model.Persistence {

  /// <summary>
  /// Class responsible for connecting to the entity framwork. Has static methods for adding, modifying
  /// and deleting enteties.
  /// </summary>
  public static class Context {
    /// <summary>
    /// Get user instance by email.
    /// Returns null if no user is found
    /// </summary>
    /// <param name="email"></param>
    /// <returns>User instance</returns>
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
    
    private static File GetFileWithContext(int id, SliceOfLifeEntities dbContext) {
      var query = from u in dbContext.Files
                  where u.id == id
                  select u;
      return !query.Any() ? null : query.First();
    }

    /// <summary>
    /// Add user. Takes an user instance. Returns positive int if succeded.
    /// </summary>
    /// <param name="user"></param>
    /// <returns>Returns positive int if succeded</returns>
    public static int AddUser(User user) {
      using (var dbContext = new SliceOfLifeEntities()) {
        if (user == null || user.email == null || user.email.Trim().Equals("")) return -2;
        if (GetUser(user.email) != null) return 0;
        dbContext.Users.Add(user);
        try {
          return dbContext.SaveChanges();
        } catch (UpdateException) {
          return -1;
        }
      }
    }

    /// <summary>
    /// Responsible for getting a fileinstance from the database
    /// </summary>
    /// <param name="fileInstanceId">instance id</param>
    /// <returns>FileInstance</returns>
    public static FileInstance GetFileInstance(int fileInstanceId) {
      FileInstance instance;
      using (var dbContext = new SliceOfLifeEntities()) {
        instance = GetFileInstanceWithContext(fileInstanceId, dbContext);
      }
      return instance;
    }
    /// <summary>
    /// Responsible for getting a fileinstance from the database
    /// </summary>
    /// <param name="fileInstanceId">instance id</param>
    /// <returns>FileInstance</returns>
    public static Document GetDocument(int fileInstanceId) {
      Document instance;
      using (var dbContext = new SliceOfLifeEntities()) {
        instance = GetDocumentWithContext(fileInstanceId, dbContext);
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

    // FileInstance
    private static Document GetDocumentWithContext(int fileInstanceId, SliceOfLifeEntities dbContext) {
      if (fileInstanceId < 1) return null;
      var query = from f in dbContext.FileInstances.OfType<Document>()
                                     .Include("File.Changes")
                                     .Include("File.FileMetaDatas.MetaDataType")
                  where f.id == fileInstanceId
                  select f;
      return !query.Any() ? null : query.First();

    }
    /// <summary>
    /// Responsible for modifying an existing frilInstance
    /// </summary>
    /// <param name="fileInstance">FileInstance</param>
    /// <returns>modified FileInstance</returns>
    public static FileInstance ModifyFileInstance(FileInstance fileInstance) {
      using (var dbContext = new SliceOfLifeEntities()) {
        if (fileInstance == null)
          throw new ConstraintException("FileInstance is null");
        if (fileInstance.File == null)
          throw new ConstraintException("Database handler received an empty file reference");
        if (fileInstance.path == null || fileInstance.path.Trim().Equals(""))
          throw new ConstraintException("Invalid file path");
        FileInstance dbInstance = dbContext.FileInstances.First(i => i.id == fileInstance.id);
        if (dbInstance == null) return AddFileInstance(fileInstance);
        dbContext.FileInstances.Remove(dbInstance);
        try {
          dbContext.SaveChanges();
        } catch (UpdateException e) {
          throw new ConstraintException("Problem with adding entries", e);
        }
      }
      return AddFileInstance(fileInstance);
    }

    public static void ModifyDocument(int fileInstanceId, string title, string content) {
      if (fileInstanceId < 0 || title == null || content == null) throw new ConstraintException("Invalid arguments");
      using (var dbContext = new SliceOfLifeEntities()) {
        var document = GetDocumentWithContext(fileInstanceId, dbContext);
        document.Title = title;
        document.Content = content;
        document.File.Version += 1;
        try {
          dbContext.SaveChanges();
        } catch (UpdateException e) {
          throw new ConstraintException("Problem with modifying entries", e);
        }
      }
    }

    private static FileInstance FileWithoutUser(FileInstance file) {
      FileInstance newFile = new FileInstance();
      newFile.File = file.File;
      newFile.Content = file.Content;
      newFile.File_id = file.File_id;
      newFile.path = file.path;
      newFile.User_email = file.User_email;
      return newFile;
    }

    /// <summary>
    /// Responsible for adding a FileInstance to the database
    /// </summary>
    /// <param name="fileInstance">fileinstance</param>
    /// <returns>fileinstance with new id from db</returns>
    public static FileInstance AddFileInstance(FileInstance fileInstanceInput) {

      using (var dbContext = new SliceOfLifeEntities()) {
        //CONSTRAINTCHECKS
        if (fileInstanceInput == null)
          throw new ConstraintException("Database handler received an empty reference");
        FileInstance fileInstance;
        var update = false;
        if (fileInstanceInput.id > 0) {
          fileInstance = dbContext.FileInstances.Find(fileInstanceInput.id);
          dbContext.Entry(fileInstance).CurrentValues.SetValues(fileInstance);
          fileInstanceInput.File.id = fileInstance.File_id;
          fileInstance.File = dbContext.Files.Find(fileInstance.File_id);
          dbContext.Entry(fileInstance.File).CurrentValues.SetValues(fileInstanceInput.File);
          foreach (var change in from change in fileInstanceInput.File.Changes let tmp = dbContext.Changes.Find(change.id) where tmp == null select change) {
            fileInstance.File.Changes.Add(change);
          }
          update = true;

        } else fileInstance = fileInstanceInput;

        if (fileInstance.File_id != 0 && fileInstance.File == null) fileInstance.File = dbContext.Files.Find(fileInstance.File_id);
        if (fileInstance.User_email != null) fileInstance.User = dbContext.Users.Find(fileInstance.User_email);

        // File
        if (fileInstance.File == null && fileInstance.File_id == 0)
          throw new ConstraintException("Database handler received an empty file reference");

        // Path
        if (fileInstance.path == null || fileInstance.path.Trim().Equals(""))
          throw new ConstraintException("Invalid file path");

        // File name
        if (fileInstance.File.name == null || fileInstance.File.name.Trim().Equals(""))
          throw new ConstraintException("Invalid file name");

        // File serverpath
        if (fileInstance.File.serverpath == null || fileInstance.File.serverpath.Trim().Equals(""))
          fileInstance.File.serverpath = @"c:\ServerPathUnknown\";

        // File Version
        if (fileInstance.File.Version < 0) throw new ConstraintException("Invalid file version");

        if (fileInstance.User == null) {
          User u = fileInstance.User ?? new User() { email = fileInstance.User_email };
          fileInstance.User = u;
          dbContext.Users.Add(fileInstance.User);
        }

        fileInstance.File.Version += 1;
        if (!update) dbContext.FileInstances.Add(fileInstance);

        try {
          dbContext.SaveChanges();
        } catch (UpdateException e) {
          throw new ConstraintException(
            "Database handler received an error when trying saving changes to the database", e);
        }
      }
      return fileInstanceInput;
    }

    public static List<FileInstance> GetFiles(string useremail) {
      using (var dbContext = new SliceOfLifeEntities()) {
        if (useremail == null || useremail.Trim().Equals("")) return null;
        var query = from f in dbContext.FileInstances.Include("File")
                    where f.User_email.Equals(useremail)
                    select f;
        return !query.Any() ? null : query.ToList();
      }
    }

    public static List<User> GetUsers(int fileInstanceId) {
      using (var dbContext = new SliceOfLifeEntities()) {
        var fileInstance = GetFileInstanceWithContext(fileInstanceId, dbContext);
        if (fileInstance == null) return null;
        var query = from f in dbContext.FileInstances.Include("User")
                    where f.File_id == fileInstance.File.id
                    select f;
        return !query.Any() ? null : query.Select(instance => instance.User).ToList();
      }
    }

    public static FileList GetFileList(string useremail) {
      using (var dbContext = new SliceOfLifeEntities()) {
        var usersFilesOnServer = new FileList();
        var query = from f in dbContext.FileInstances
                    .Include("File.Changes")
                    .Include("User")
                    .Include("File.FileMetaDatas.MetaDataType")

                    where f.User_email == useremail
                    select f;
        if (query.Any()) {
          foreach (FileInstance fi in query) {
            usersFilesOnServer.List.Add(fi.id, FileListEntry.EntryFromFile(fi));
          }
        }
        return usersFilesOnServer;
      }
    }

    public static void AddChange(int fileId, Change change) {
      using (var dbContext = new SliceOfLifeEntities()) {
        var file = GetFileWithContext(fileId, dbContext);
        file.Changes.Add(change);
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
            dbContext.Changes.Remove(element);
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
            dbContext.Projects.Remove(element);
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
            dbContext.MetaDataTypes.Remove(element);
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
            dbContext.FileMetaDatas.Remove(element);
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
            dbContext.FileInstances.Remove(element);
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
            dbContext.Files.Remove(element);
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
            dbContext.Users.Remove(element);
          }
          try {
            dbContext.SaveChanges();
          } catch (UpdateException) { }
        }

        // Reset AI

        // Test user
        User user1 = new User();
        user1.email = "superman123456@gm44ail.com";
        Context.AddUser(user1);

        // Add MetaType
        var metaType = new MetaDataType { Type = "Title" };
        const string metaValue = "SomeTitle";

        for (int i = 0; i < 3; i++) {
          // Add Users
          var user = new User { email = "testuser" + i + "@example.com" };
          dbContext.Users.Add(user);

          var count = 1;
          for (int k = 0; k < 10; k++) {
            // Add Files
            var file = new File { id = i, name = "Testfile" + i + "" + k, serverpath = @"C:\ServerTestFiles\", Version = 0.0m };
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
            Document document = new Document { File = file, Content = "Some content" + i + k, id = i, path = @"C:\ClientTestFiles\" };
             if (k % 2 == 0) document.path += @"Subfolder\";
             if (k % 3 == 0) document.path += @"AnotherSubFolder\";
             if (k % 7 == 0) document.path += @"YetAnotherSubFolder\";
             if (k % 5 == 0) document.path += @"SomeSubFolder\";
            document.File = file;
            document.User = user;
            dbContext.FileInstances.Add(document);

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