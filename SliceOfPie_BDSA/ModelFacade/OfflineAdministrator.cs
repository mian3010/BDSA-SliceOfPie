using System;
using System.Collections.Generic;
using SliceOfPie_Model.Persistence;

namespace SliceOfPie_Model {
  public class OfflineAdministrator : IAdministrator {

    private readonly ICommunicator _communicator;
    private readonly INetClient _netClient;
      
    public delegate void FileEventHandler(object sender, File file);
    public event FileEventHandler FilesUpdated, ContentAdded, FileSaved;

    //singleton instance
    private static OfflineAdministrator _administrator;

    /// <summary>
    /// Constructs offlineAdministrator with a static rootpath for now.
    /// </summary>
    private OfflineAdministrator() {
        // This is not very smart I think. Perhaps logger should be a composite object in offline adapter.
        _communicator = new CommunicatorOfflineAdapter();
     
        _netClient = new NetworkClient();
    }

    public static OfflineAdministrator GetInstance() {
      if (_administrator == null)
        _administrator = new OfflineAdministrator();
      return _administrator;
    }

    public File GetFile(long id)
    {
        return _communicator.GetFile(id);
    }


    public void AddFile(File file)
    {
        if (file.id <= 0)
        {
            _communicator.AddOfflineCreatedFile(file);
        }
        else
        {
            _communicator.AddFileFromRemote(file);
        }

    }
   
    public void Synchronize()
    {
        // Get fileList from Communicator
        FileList oFileList = _communicator.FileListHandler.FileList;
        // Send filelist to Server via Client
        FileList responseList = _netClient.SyncServer(oFileList);
        // Receive fileList

        List<File> conflictFiles = new List<File>();
        foreach (FileListEntry entry in responseList.List.Values)
        {
            switch (entry.Type)
            {
                case FileListType.Conflict: 
                    conflictFiles.Add(_netClient.PullFile(entry.Id)); break;
                case FileListType.Pull:
                    _communicator.AddOfflineCreatedFile(_netClient.PullFile(entry.Id)); break;
                case FileListType.Push: 
                    File toPush = _communicator.GetFile(entry.Id);
                    _communicator.UpdateFileId(toPush, _netClient.PushFile(toPush)); 
                    break;
                    
            }
        }
        HandleConflictedFiles(conflictFiles);
    
    }


    private void HandleConflictedFiles(List<File> conflictedFiles)
    {
        // Here we should alert the user and let him fix conflicted files.
    }
       
    public void SaveFile(File file) {
        _communicator.ModifyFile(file);
    }

  
    public Dictionary<String, long> GetPathsAndIDs()
    {
        return _communicator.FileListHandler.GetPathsWithId();
    }





    public void ExitGracefully(object sender, EventArgs e)
    {
        _communicator.FileListHandler.PersistFileList();

    }
  }
}
