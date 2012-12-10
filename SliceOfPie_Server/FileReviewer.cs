using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SliceOfPie_Model;
using SliceOfPie_Model.Persistence;

namespace SliceOfPie_Server {
  /// <summary>
  /// This class handles FileList receives.
  /// This will usually be started in a seperate thread.
  /// Author: Claus35-DK - clih@itu.dk
  /// Author: mian3010 - msoa@itu.dk
  /// </summary>
  class FileListReviewer {
    private FileList fileList;
    private HttpProcessor hp;

    /// <summary>
    /// Constructor. 
    /// This will call a method upon the HTTPProcessor, to tell wether this was succesful
    /// </summary>
    /// <param name="fileList"></param>
    /// <param name="hp"></param>
    public FileListReviewer(FileList fileList, HttpProcessor hp) {
      this.fileList = fileList;
      this.hp = hp;
    }

    public void Review() {
      FileList UsersFilesFromServer = Context.GetFileList("lala"); //hp.user
      foreach (FileListEntry Entry in fileList.List.Values) {
        // if file exists
        File FileFromDb = Context.GetFile(Entry.Id);
        if (FileFromDb != null) {
          int MajorEntryVersion = (int)Math.Truncate(Entry.Version);
          int MinorEntryVersion = (int)(Entry.Version - Math.Truncate(Entry.Version) * 10);
          int MajorDbVersion = (int)Math.Truncate((decimal)FileFromDb.version);
          int MinorDbVersion = (int)(Entry.Version - Math.Truncate((decimal)FileFromDb.version) * 10);
          if ((MajorEntryVersion == MajorDbVersion && MinorEntryVersion != MinorDbVersion) || (MajorEntryVersion < MajorDbVersion && MinorEntryVersion > 0)) {
            //Client must push their file for merging
            RequestHandler.instance.PendingModFileList.Add(Entry.Id, Entry);
            UsersFilesFromServer.List[Entry.Id].Type = FileListType.Push;
          } else if (MajorEntryVersion < MajorDbVersion && MinorEntryVersion == 0) {
            //Client does not have the latest file, and must pull it.
            RequestHandler.instance.PendingModFileList.Add(Entry.Id, Entry);
            UsersFilesFromServer.List[Entry.Id].Type = FileListType.Pull;
          } else UsersFilesFromServer.List.Remove(Entry.Id); //File on client and server is the same
        } else {
          //Server does not have file, Client must therefore push it.
          RequestHandler.instance.PendingNewFileList.Add(Entry.Id);
          Entry.Type = FileListType.Push;
          UsersFilesFromServer.List.Add(Entry.Id, Entry);
        }
      }
      //hp.something(fileList);
    }
  }
}
