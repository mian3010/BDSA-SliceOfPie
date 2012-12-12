﻿using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SliceOfPie_Model.Persistence {
  class Context2 {
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
      //TODO Throw exceptions
      if (fileInstance == null) return -2;
      bool deleteBeforeAdd = false;
      // Check for lots of constraints
      // Id
      if (fileInstance.id < 1) return -1;
      if (GetFileInstance(fileInstance.id) != null) deleteBeforeAdd = true;

      // Path
      if (fileInstance.path == null || fileInstance.path.Trim().Equals("")) return -1;

      // User
      if (fileInstance.UserEmail == null || fileInstance.UserEmail.Trim().Equals("")) return -1;
      if (GetUser(fileInstance.UserEmail) != null) return -1;

      // File
      if (fileInstance.File == null) return -1;

      // File id
      if (fileInstance.File_id < 0) return -1;

      // File name
      if (fileInstance.File.name == null || fileInstance.File.name.Trim().Equals("")) return -1;

      // File serverpath
      if (fileInstance.File.serverpath == null || fileInstance.File.serverpath.Trim().Equals("")) return -1;

      // File Version
      if (fileInstance.File.Version < 0) return -1;

      if (deleteBeforeAdd) {
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
                  where f.UserEmail.Equals(useremail)
                  select f;
      return !query.Any() ? null : query.ToList();
    }
  }
}