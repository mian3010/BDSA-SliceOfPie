using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SliceOfPie_Model.Persistence {
  /// <summary>
  /// Context has methods for operating on the database
  /// </summary>
  public static class Context {
    private static readonly SliceOfLifeEntities DbContext = new SliceOfLifeEntities();

    //TODO If serverpath == null, add serverpath

    public static User[] GetUsers() {
      var query = from u in DbContext.Users
                  select u;
      return !query.Any() ? null : query.ToArray();
    }

    public static List<User> GetUsers(File file) {
      if (file == null) return null;
      var query = from f in DbContext.FileInstances
                  where f.File_id == file.id
                  select f;
      if (!query.Any()) return null;
      return query.Select(fileInstance => User.CreateUser(fileInstance.UserEmail)).ToList();
    }

    public static User GetUser(string email) {
      if (email == null || email.Trim() == "") return null;
      var query = from u in DbContext.Users
                  where u.email == email
                  select u;
      return !query.Any() ? null : query.First();
    }

    public static long AddUser(string email) {
      if (email == null || email.Trim() == "") return -2;
      var user = User.CreateUser(email);
      return AddUser(user);
    }

    public static long AddUser(User user) {
      if (user == null) return -2;
      if (GetUser(user.email) != null) {
        return 1;
      }
      DbContext.Users.AddObject(user);
      try {
        return DbContext.SaveChanges();
      } catch (UpdateException) {
        return -1;
      }
    }

    public static List<FileMetaData> GetMetaDataFromFile(long fileId) {
      var query = from meta in DbContext.FileMetaDatas
                  where meta.File_id == fileId
                  select meta;
      if (!query.Any()) return null;

      var metaDataList = new List<FileMetaData>();
      foreach (var fileMetaData in query) {
        var metaData = FileMetaData.CreateFileMetaData(fileMetaData.id, fileMetaData.MetaDataType.ToString(), fileMetaData.File_id);
        metaData.value = fileMetaData.value;
        metaDataList.Add(metaData);
      }
      return metaDataList;
    }

    public static FileMetaData GetFileMetaData(File file, string metaDataType) {
      var query = from meta in file.FileMetaDatas
                  where meta.MetaDataType_Type.Equals(metaDataType)
                  select meta;
      return !query.Any() ? null : query.First();
    }

    public static long ModifyMetaData(FileMetaData fileMetaData) {
      if (fileMetaData == null) return -2;
      DeleteFileMetaData(fileMetaData);
      return AddFileMetaData(fileMetaData);
    }

    public static long DeleteFileMetaData(FileMetaData fileMetaData) {
      if (fileMetaData == null) return -2;
      return DeleteFileMetaData(fileMetaData.id);
    }

    public static long DeleteFileMetaData(long fileMetaDataId) {
      var query = from fmd in DbContext.FileMetaDatas
                  where fmd.id == fileMetaDataId
                  select fmd;
      if (!query.Any()) return -2;
      DbContext.FileMetaDatas.DeleteObject(query.First());
      try {
        return DbContext.SaveChanges();
      } catch (UpdateException) {
        return -1;
      }
    }

    public static FileMetaData GetMetaData(long metaDataId) {
      var query = from md in DbContext.FileMetaDatas
                  where md.id == metaDataId
                  select md;
      return !query.Any() ? null : query.First();
    }

    public static long AddFileMetaData(FileMetaData fileMetaData) {
      if (fileMetaData == null) return -2;
      if (fileMetaData.MetaDataType_Type == null) return -2;
      if (GetMetaData(fileMetaData.id) != null) return ModifyMetaData(fileMetaData);
      AddMetaDataType(fileMetaData.MetaDataType_Type);
      DbContext.FileMetaDatas.AddObject(fileMetaData);
      try {
        return DbContext.SaveChanges();
      } catch (UpdateException) {
        return -1;
      }
    }

    public static long AddFileInstance(FileInstance fileInstance) {
      if (fileInstance == null || fileInstance.File == null) return -2;
      if (GetFileInstance(fileInstance.id) != null) {
      }
      AddUser(fileInstance.User);
      AddFile(fileInstance.File);
      DbContext.FileInstances.AddObject(fileInstance);
      try {
        return DbContext.SaveChanges();
      } catch (UpdateException) {
        return -1;
      }
    }

    public static long ModifyFileInstance(FileInstance fileInstance) {
      if (fileInstance == null || fileInstance.File == null) return -2;
      DeleteFileInstance(fileInstance);
      return AddFileInstance(fileInstance);
    }

    public static long DeleteFileInstance(FileInstance fileInstance) {
      if (fileInstance == null || fileInstance.File == null) return -2;
      return DeleteFileInstance(fileInstance.id);
    }

    public static long DeleteFileInstance(long fileInstanceId) {
      var query = from fi in DbContext.FileInstances
                  where fi.id == fileInstanceId
                  select fi;
      if (!query.Any()) return -1;
      DbContext.FileInstances.DeleteObject(query.First());
      try {
        return DbContext.SaveChanges();
      } catch (UpdateException) {
        return -1;
      }
    }

    public static FileInstance GetFileInstance(long fileInstanceId) {
      var query = from fi in DbContext.FileInstances
                  where fi.id == fileInstanceId
                  select fi;
      return !query.Any() ? null : query.First();
    }

    public static FileList GetFileList(string userEmail) {
      var usersFilesOnServer = new FileList();
      var list = usersFilesOnServer.List;
      var query = from f in DbContext.FileInstances
                  where f.User_email == userEmail
                  select f;
      foreach (FileInstance fi in query) {
        list.Add(fi.File_id, FileListEntry.EntryFromFile(fi));
      }
      return usersFilesOnServer;
    }

    public static FileInstance GetFile(long fileId) {
      var query = from f in DbContext.FileInstances
                  where f.id == fileId
                  select f;
      return !query.Any() ? null : query.First();
    }


    public static long AddFile(File file) {
      if (file == null) return -2;
      if (GetFile(file.id) != null) {

      }
      DbContext.Files.AddObject(file);
      try {
        return DbContext.SaveChanges();
      } catch (UpdateException) {
        return -1;
      }
    }

    public static long UpdateFile(FileInstance file) {
      if (file == null || file.File == null) return -2;
      DbContext.Files.DeleteObject(file.File);
      return AddFile(file.File);
    }

    private static MetaDataType GetMetaDataType(string type) {
      var inputType = MetaDataType.CreateMetaDataType(type);
      var query = from mt in DbContext.MetaDataTypes
                  where mt.Type == inputType.Type
                  select mt;
      return !query.Any() ? null : query.First();
    }

    private static void AddMetaDataType(string type) {
      if (GetMetaDataType(type) != null) return;
      var metaType = MetaDataType.CreateMetaDataType(type);
      DbContext.MetaDataTypes.AddObject(metaType);
      try {
        DbContext.SaveChanges();
      } catch (UpdateException) {

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
        } catch (UpdateException) {}
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
    }
  }
}
