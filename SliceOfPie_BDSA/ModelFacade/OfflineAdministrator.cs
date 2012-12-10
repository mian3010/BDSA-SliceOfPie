using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SliceOfPie_Model.Persistence;

namespace SliceOfPie_Model {
  public class OfflineAdministrator : IAdministrator {

    private ICommunicator communicator;
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
     
        netClient = new NetworkClient();
    }

    public static OfflineAdministrator GetInstance() {
      if (administrator == null)
        administrator = new OfflineAdministrator();
      return administrator;
    }

    public File GetFile(long id)
    {
        return communicator.GetFile(id);
    }


    public void AddFile(File file)
    {
        if (file.id <= 0)
        {
            communicator.AddOfflineCreatedFile(file);
        }
        else
        {
            communicator.AddFileFromRemote(file);
        }

    }
   
    public void Synchronize()
    {
        // Get fileList from Communicator
        FileList oFileList = communicator.FileListHandler.FileList;
        // Send filelist to Server via Client
        FileList responseList = netClient.SyncServer(oFileList);
        // Receive fileList

        List<File> conflictFiles = new List<File>();
        foreach (FileListEntry entry in responseList.List.Values)
        {
            switch (entry.Type)
            {
                case FileListType.Conflict: 
                    conflictFiles.Add(netClient.PullFile(entry.Id)); break;
                case FileListType.Pull:
                    communicator.AddOfflineCreatedFile(netClient.PullFile(entry.Id)); break;
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
        communicator.ModifyFile(file);
    }

  
    public Dictionary<String, long> GetPathsAndIDs()
    {
        return communicator.FileListHandler.GetPathsWithID();
    }





    public void ExitGracefully(object sender, EventArgs e)
    {
        communicator.FileListHandler.PersistFileList();

    }
  }
}
