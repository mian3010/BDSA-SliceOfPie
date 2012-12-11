using System.Collections.Generic;
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
        return ModifyUser(user);
      }
      DbContext.Users.AddObject(user);
      return DbContext.SaveChanges();
    }

    public static long DeleteUser(User user) {
      if (user == null) return -2;
      return DeleteUser(user.email);
    }

    public static long DeleteUser(string email) {
      if (email == null || email.Trim() == "") return -2;
      var query = from u in DbContext.Users
                  where u.email == email
                  select u;
      if (!query.Any()) return -2;
      DbContext.Users.DeleteObject(query.First());
      return DbContext.SaveChanges();
    }

    public static long ModifyUser(User user) {
      if (user == null) return -2;
      DeleteUser(user);
      return AddUser(user);
    }

    public static List<FileMetaData> GetMetaData(long fileId) {
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
      return DbContext.SaveChanges();
    }

    public static long AddFileMetaData(FileMetaData fileMetaData) {
      if (fileMetaData == null) return -2;
      if (GetMetaData(fileMetaData.id) != null) return ModifyMetaData(fileMetaData);
      AddMetaDataType(fileMetaData.MetaDataType.ToString());
      DbContext.FileMetaDatas.AddObject(fileMetaData);
      return DbContext.SaveChanges();
    }

    public static long AddFileInstance(FileInstance fileInstance) {
      if (fileInstance == null) return -2;
      if (GetFileInstance(fileInstance.id) != null) {
        return ModifyFileInstance(fileInstance);
      }
      DbContext.FileInstances.AddObject(fileInstance);
      return DbContext.SaveChanges();
    }

    public static long ModifyFileInstance(FileInstance fileInstance) {
      if (fileInstance == null) return -2;
      DeleteFileInstance(fileInstance);
      return AddFileInstance(fileInstance);
    }

    public static long DeleteFileInstance(FileInstance fileInstance) {
      if (fileInstance == null) return -2;
      return DeleteFileInstance(fileInstance.id);
    }

    public static long DeleteFileInstance(long fileInstanceId) {
      var query = from fi in DbContext.FileInstances
                  where fi.id == fileInstanceId
                  select fi;
      if (!query.Any()) return -1;
      DbContext.FileInstances.DeleteObject(query.First());
      return DbContext.SaveChanges();
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

    public static long AddFile(FileInstance file) {
      if (file == null) return -2;
      if (GetFile(file.id) != null) {
        return UpdateFile(file);
      }
      DbContext.Files.AddObject(file.File);
      return DbContext.SaveChanges();
    }

    public static long UpdateFile(FileInstance file) {
      if (file == null) return -2;
      DbContext.Files.DeleteObject(file.File);
      return AddFile(file);
    }

    private static MetaDataType GetMetaDataType(string type) {
      var query = from mt in DbContext.MetaDataTypes
                  where mt.ToString() == type
                  select mt;
      return !query.Any() ? null : query.First();
    }

    private static void AddMetaDataType(string type) {
      if(GetMetaDataType(type) != null) return;
      var metaType = MetaDataType.CreateMetaDataType(type);
      DbContext.MetaDataTypes.AddObject(metaType);
    }
  }
}
