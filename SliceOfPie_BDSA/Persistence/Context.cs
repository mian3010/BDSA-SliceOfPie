using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SliceOfPie_Model.Persistence {
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
      } catch (UpdateException e) {
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
      } catch (UpdateException e) {
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
      } catch (UpdateException e) {
        return -1;
      }
    }

    public static long AddFileInstance(FileInstance fileInstance) {
      if (fileInstance == null || fileInstance.File == null) return -2;
      if (GetFileInstance(fileInstance.id) != null) {
        return ModifyFileInstance(fileInstance);
      }
      DbContext.FileInstances.AddObject(fileInstance);
      try {
        return DbContext.SaveChanges();
      } catch (UpdateException e) {
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
      } catch (UpdateException e) {
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

    public static long AddFile(FileInstance fileInstance) {
      if (fileInstance == null || fileInstance.File == null) return -2;
      if (GetFile(fileInstance.id) != null) {
        return UpdateFile(fileInstance);
      }
      DbContext.Files.AddObject(fileInstance.File);
      try {
        return DbContext.SaveChanges();
      } catch (UpdateException e) {
        return -1;
      }
    }

    public static long UpdateFile(FileInstance fileInstance) {
      if (fileInstance == null || fileInstance.File == null) return -2;
      DbContext.Files.DeleteObject(fileInstance.File);
      return AddFile(fileInstance);
    }

    private static MetaDataType GetMetaDataType(string type) {
      var inputType = MetaDataType.CreateMetaDataType(type);
      var query = from mt in DbContext.MetaDataTypes
                  where mt.Type == inputType.Type
                  select mt;
      return !query.Any() ? null : query.First();
    }

    private static void AddMetaDataType(string type) {
      if(GetMetaDataType(type) != null) return;
      var metaType = MetaDataType.CreateMetaDataType(type);
      DbContext.MetaDataTypes.AddObject(metaType);
      try {
        DbContext.SaveChanges();
      } catch (UpdateException e) {
        
      }
    }
  }
}
