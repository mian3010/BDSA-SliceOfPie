﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Diagnostics;
using SliceOfPie_Model.Persistence;

namespace SliceOfPie_Model {
    /// <summary>
    /// The offline adapter for the ICommunicator interface. Implements persistent storage on disk.
    /// Also distinguishes between adding files on disk that's retrieved from server or just created offline.
    /// </summary>
    /// 

   /////// TO DO -  IMPLEMENT CACHE


    public class CommunicatorOfflineAdapter : ICommunicator {

      // The object takes a path for the root folder of the SoP documents. Each document will be automatically saved from there.
        private readonly OfflineFileListHandler _fileListHandler;
        
        public IFileListHandler FileListHandler
        {
            get { return _fileListHandler; } 
        }
    
      public event FileEventHandler FileAdded, FileChanged, FileDeleted, FileMoved, FileRenamed, FilePulled;

      public CommunicatorOfflineAdapter()
      {
          _fileListHandler = new OfflineFileListHandler(this);
      }

        /// <summary>
        /// Adds a file from remote storage. Should be used during synchronization.
        /// </summary>
        /// <param name="file">The file to add from a remote location</param>
        /// <returns>True if successful, false otherwise</returns>
      public bool AddFileFromRemote(File file)
      {
          if (AddNewFile(file))
          {
              if (FilePulled != null)
                  FilePulled(file);
              return true;
          }
          else return false;    

      }

    private bool AddNewFile(File file) {

        if(!System.IO.Directory.Exists(file.serverpath)) {
            System.IO.Directory.CreateDirectory(file.serverpath);
        }
        string fullpath = System.IO.Path.Combine(file.serverpath, file.name);
        String fileHtml = HtmlMarshalUtil.MarshallFile(file);
        if (!System.IO.File.Exists(fullpath))
        {
            System.IO.File.WriteAllText(fullpath, fileHtml);
            return true;
        }
        else
        {
            // TO-DO Maybe do some other semantic than just doing it anyways -> can we overwrite?
            System.IO.File.WriteAllText(fullpath, fileHtml);
            return true;
        }

    }

      /// <summary>
      /// Add a new file to the disk with either the serverpath specified in the file or the default rootpath.
      /// Will overwrite existing files. Must be run with administrator rights. Also saves a LogEntry describing the action
      /// </summary>
      /// <param name="file">The file to be added. Does not need a id or a path. Has to be created with an offline client.
      /// </param>
      /// <returns>Boolean indicating whether the creation was succesful</returns>
    public bool AddOfflineCreatedFile(File file)
    {
        file.id = FileListHandler.FileList.IncrementCounter--;
        if (AddNewFile(file))
        {
            if (FileAdded != null)
                FileAdded(file);
            return true;
        }
        else return false;    

    }

    /// <summary>
    /// Tries to modify the existing file given as param. If the file does not exist, it will create a new file
    /// with the specified path or at the rootpath.
    /// </summary>
    /// <param name="file">The file to modify</param>
    /// <returns></returns>
    public bool ModifyFile(File file) {

        if(file == null){
            throw new ArgumentException();
        }

        if (AddNewFile(file))
        {
            if(FileChanged != null) 
                FileChanged(file);
            return true;
        }
        return false;
    
    }

    /// <summary>
    /// Tries to delete a file. Throws exception if the file is already flagged for deletion.
    /// 
    /// </summary>
    /// <param name="file">The file to be deleted. Need a full path to work</param>
    /// <returns>True if sucessful, failure in any exception case</returns>
    public bool DeleteFile(File file)
    {
        //Is metadata availible.
        if (file.deleted <= 0)
        {
            throw new ArgumentException();
        }
    
        try
        {
    
            //Delete the file through System.IO.File class, not Slice Of Pie File class.
            String deletePath = System.IO.Path.Combine(file.serverpath, file.name);
            Debug.WriteLine(deletePath); 
            System.IO.File.Delete(deletePath);

            if (FileDeleted != null)
                FileDeleted(file);
            return true;
        }
        catch (System.IO.IOException e)
        {
            throw new System.IO.IOException("Deleting File on disk :" + file.name + " failed."); 
        }
      
    }

      /// <summary>
      /// Tries to rename the file specified in the param with the newName param. 
      /// Overwrites any file existing at the new name.
      /// </summary>
      /// <param name="file">The old file path and name</param>
      /// <param name="newName">The new name of the file. NB: THIS IS ONLY THE FILENAME NOT PATH</param>
    public void RenameFile(File file, string newName)
    {
        // Should be less than zero, flag for deleted. If zero might not be initialized.
        if (file.deleted < 0)
        {
            throw new ArgumentException();
        }
        string fullPath = System.IO.Path.Combine(file.serverpath, file.name);
        string newFullPath = System.IO.Path.Combine(file.serverpath, newName);
       
        MoveFile(fullPath, newFullPath);
        file.name = newName;

        if (FileRenamed != null)
            FileRenamed(file);

    }

    private void MoveFile(String fullOldPath, String fullNewPath)
    {
        System.IO.File.Move(fullOldPath, fullNewPath);
    }

    /// <summary>
    /// Moves a file from the files old path to the newPath.
    /// </summary>
    /// <param name="file">The file to move and it's old path</param>
    /// <param name="newPath">The new path</param>
    public void MoveFile(File file, string newPath) {
        if (file.deleted < 0)
        {
            throw new ArgumentException();
        }
        if (!System.IO.Directory.Exists(newPath))
        {
            System.IO.Directory.CreateDirectory(newPath);
        }
        string fullPath = System.IO.Path.Combine(file.serverpath, file.name);
        string newFullPath = System.IO.Path.Combine(newPath, file.name);

        MoveFile(fullPath, newFullPath);

        // Change the file path... in the old file [good idea yet?]
        file.serverpath = newPath;
        if (FileMoved != null)
            FileMoved(file);
    }
      /// <summary>
      /// Checks for a file in the root folder or the given path
      /// </summary>
      /// <param name="file">The file to search for</param>
      /// <returns>True if the file is found, false if not existing (at least not in root path)</returns>
    public Boolean FindFile(File file)
    {
        String searchPath = file.serverpath;
        searchPath = System.IO.Path.Combine(searchPath, file.name);

        if (System.IO.File.Exists(searchPath))
        {
            return true;
        }
        else
        {
            return false;
        }
    }



    /// <summary>
    /// Retrieves a file from storage using the File's path
    /// </summary>
    /// <param name="id">The id of the file to retrieve</param>
    /// <returns></returns>
    public File GetFile(long id)
    {
        FileListEntry fileInfo = _fileListHandler.FileList.List[id];
        String fullPath = System.IO.Path.Combine(fileInfo.Path, fileInfo.Name);
        String html = System.IO.File.ReadAllText(fullPath);

        File loadedFile = HtmlMarshalUtil.UnmarshallFile(html);
        loadedFile.serverpath = fileInfo.Path;
        loadedFile.name = fileInfo.Name;

        return loadedFile;
    
    }


    public void UpdateFileId(File file, long newId)
    {
        throw new NotImplementedException();
    }
  }
}
