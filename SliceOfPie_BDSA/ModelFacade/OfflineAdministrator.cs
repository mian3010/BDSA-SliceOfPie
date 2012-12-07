using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceOfPie_Model {
  public class OfflineAdministrator : IAdministrator {

    private ICommunicator communicator;
    private IFileListHandler logger;
    private INetClient netClient;
      
    public delegate void FileEventHandler(object sender, File file);
    public event FileEventHandler FilesUpdated, ContentAdded, FileSaved;

    //singleton instance
    public static OfflineAdministrator administrator;

    /// <summary>
    /// Constructs offlineAdministrator with a static rootpath for now.
    /// </summary>
    private OfflineAdministrator() {
        /// This is not very smart I think. Perhaps logger should be a composite object in offline adapter.
        communicator = new CommunicatorOfflineAdapter();
        logger = new OfflineFileListHandler(communicator);
        netClient = new NetworkClient();
    }

    public static OfflineAdministrator GetInstance() {
      if (administrator == null)
        administrator = new OfflineAdministrator();
      return administrator;
    }

    public void Synchronize()
    {
        // Get fileList from Communicator
        FileList oFileList = logger.FileList;
        // Send filelist to Server via Client
        FileList responseList = netClient.SyncServer();
        // Receive fileList

        List<File> conflictFiles = new List<File>();
        foreach (FileListEntry entry in responseList.List.Values)
        {
            switch (entry.Type)
            {
                case FileListType.Conflict: 
                    conflictFiles.Add(netClient.PullFile(communicator.GetFile(entry.Id))); break;
                case FileListType.Pull:
                    communicator.AddFile(netClient.PullFile(entry.Id)); break;
                case FileListType.Push: 
                    File toPush = communicator.GetFile(entry.Id);
                    communicator.UpdateFileID(toPush, netClient.PushFile(toPush)); 
                    break;
                    
            }
        }
        HandleConflictedFiles(conflictFiles);
    
    }


    private void HandleConflictedFiles(List<File> ConflictedFiles)
    {
        /// Here we should alert the user and let him fix conflicted files.
    }
       
    public void SaveFile(File file) {
      // bool b = communicator.SaveFile(file);
        bool b = true;
      if (b)
        FileSaved(this, file);
    }

    public void OpenFile(File file) {

    }

    public void GetAllFiles() {
      throw new NotImplementedException();
    }

  }
}
