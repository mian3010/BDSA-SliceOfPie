using System;
using SliceOfPie_Model;
using SliceOfPie_Model.Persistence;
using System.Collections.Generic;

namespace SliceOfPie_Server {
  /// <summary>
  /// This class handles FileList receives.
  /// This will usually be started in a seperate thread.
  /// Author: Claus35-DK - clih@itu.dk
  /// Author: mian3010 - msoa@itu.dk
  /// </summary>
  class FileListReviewer {
    private readonly FileList _fileList;
    private readonly HttpProcessor _hp;

    /// <summary>
    /// Constructor. 
    /// This will call a method upon the HttpProcessor, to tell wether this was succesful
    /// </summary>
    /// <param name="fileList"></param>
    /// <param name="hp"></param>
    public FileListReviewer(FileList fileList, HttpProcessor hp) {
      _fileList = fileList;
      _hp = hp;
    }

    public void Review() {
      FileList usersFilesFromServer = Context2.GetFileList("lala"); //hp.user
      if (usersFilesFromServer.List == null) usersFilesFromServer.List = new Dictionary<int, FileListEntry>();
      foreach (FileListEntry entry in _fileList.List.Values) {
        // if file exists
        FileInstance fileFromDb = Context2.GetFileInstance(entry.Id);
        if (fileFromDb != null) {
          var majorEntryVersion = (int)Math.Truncate(entry.Version);
          var minorEntryVersion = (int)(entry.Version - Math.Truncate(entry.Version) * 10);
          var majorDbVersion = (int)Math.Truncate(fileFromDb.File.Version);
          var minorDbVersion = (int)(entry.Version - Math.Truncate(fileFromDb.File.Version) * 10);
          if ((majorEntryVersion == majorDbVersion && minorEntryVersion != minorDbVersion) || (majorEntryVersion < majorDbVersion && minorEntryVersion > 0)) {
            //Client must push their file for merging
            RequestHandler.Instance.PendingModFileList.Add(entry.Id, entry);
            usersFilesFromServer.List[entry.Id].Type = FileListType.Push;
          } else if (majorEntryVersion < majorDbVersion && minorEntryVersion == 0) {
            //Client does not have the latest file, and must pull it.
            RequestHandler.Instance.PendingModFileList.Add(entry.Id, entry);
            usersFilesFromServer.List[entry.Id].Type = FileListType.Pull;
          } else usersFilesFromServer.List.Remove(entry.Id); //File on client and server is the same
        } else {
          //Server does not have file, Client must therefore push it.
          RequestHandler.Instance.PendingNewFileList.Add(entry.Id);
          entry.Type = FileListType.Push;
          usersFilesFromServer.List.Add(entry.Id, entry);
        }
      }
      _hp.RecieveFileList(usersFilesFromServer);
    }
  }
}
