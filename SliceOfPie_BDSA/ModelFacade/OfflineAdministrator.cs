﻿using System;
using System.Collections.Generic;
using SliceOfPie_Model.Persistence;

namespace SliceOfPie_Model {
  public class OfflineAdministrator : IAdministrator {

    private readonly ICommunicator _communicator;
    private readonly INetClient _netClient;
     
    //singleton instance
    private static OfflineAdministrator _administrator;

    /// <summary>
    /// Constructs offlineAdministrator with a static rootpath for now.
    /// </summary>
    private OfflineAdministrator() {
        // This is not very smart I think. Perhaps logger should be a composite object in offline adapter.
        _communicator = CommunicatorOfflineAdapter.GetCommunicatorInstance();
     
        _netClient = new NetworkClient();
    }


    public static OfflineAdministrator GetInstance()
    {
      return _administrator ?? (_administrator = new OfflineAdministrator());
    }

    public FileInstance GetFile(int id)
    {
        return _communicator.GetFile(id);
    }


    public void AddFile(FileInstance file)
    {

        _communicator.AddFile(file);
    }
   
    public void Synchronize(string userEmail)
    {
        // Get fileList from Communicator
        FileList oFileList = _communicator.FileListHandler.FileList;
        oFileList.User = userEmail;
        // Send filelist to Server via Client
        FileList responseList = _netClient.SyncServer(oFileList);
        // Receive fileList

        var conflictFiles = new List<FileInstance>();
        foreach (var entry in responseList.List.Values)
        {
            switch (entry.Type)
            {
                case FileListType.Conflict: 
                    conflictFiles.Add(_netClient.PullFile(entry.Id)); break;
                case FileListType.Pull:
                    _communicator.AddFile(_netClient.PullFile(entry.Id)); break;
                case FileListType.Push:
                    FileInstance toPush = _communicator.GetFile(entry.Id);
                    _communicator.UpdateFileId(toPush, _netClient.PushFile(toPush).id); 
                    break;
                    
            }
        }
        HandleConflictedFiles(conflictFiles);
    
    }


    private void HandleConflictedFiles(List<FileInstance> conflictedFiles)
    {
        // Here we should alert the user and let him fix conflicted files.
    }
       
    public void SaveFile(FileInstance file) {
        _communicator.ModifyFile(file);
    }

  
    public Dictionary<String, int> GetPathsAndIDs()
    {
        return _communicator.FileListHandler.GetPathsWithId();
    }





    public void ExitGracefully(object sender, EventArgs e)
    {
        _communicator.FileListHandler.PersistFileList();
        _communicator.FileListHandler.PersistChangeList();

    }
  }
}
