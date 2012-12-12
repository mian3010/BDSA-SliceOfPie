using System;
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

        private static CommunicatorOfflineAdapter _adapter;

        public IFileListHandler FileListHandler
        {
            get { return _fileListHandler; } 
        }
    
      public event FileEventHandler FileAdded, FileChanged, FileDeleted, FileMoved, FileRenamed, FilePulled;

        public static CommunicatorOfflineAdapter GetCommunicatorInstance()
        {
            return _adapter ?? (_adapter = new CommunicatorOfflineAdapter());
        }

        private CommunicatorOfflineAdapter()
      {
          _fileListHandler = new OfflineFileListHandler(this);
      }

        /// <summary>
        /// Adds a file from remote storage. Should be used during synchronization.
        /// </summary>
        /// <param name="file">The file to add from a remote location</param>
        /// <returns>True if successful, false otherwise</returns>
      public bool AddFileFromRemote(FileInstance file)
        {
          if (AddNewFile(file))
          {
              if (FilePulled != null)
                  FilePulled(file);
              return true;
          }
          return false;
        }

      private bool AddNewFile(FileInstance fileInstance) {

        if(!System.IO.Directory.Exists(fileInstance.File.serverpath)) {
            System.IO.Directory.CreateDirectory(fileInstance.File.serverpath);
        }
        string fullpath = System.IO.Path.Combine(fileInstance.File.serverpath, fileInstance.File.name);
        String fileHtml = HtmlMarshalUtil.MarshallFile(fileInstance);
        if (!System.IO.File.Exists(fullpath))
        {
            System.IO.File.WriteAllText(fullpath, fileHtml);
            return true;
        }
        // TO-DO Maybe do some other semantic than just doing it anyways -> can we overwrite?
        System.IO.File.WriteAllText(fullpath, fileHtml);
        return true;
      }

      /// <summary>
      /// Add a new file to the disk with either the serverpath specified in the file or the default rootpath.
      /// Will overwrite existing files. Must be run with administrator rights. Also saves a LogEntry describing the action
      /// </summary>
      /// <param name="fileInstance">The file to be added. Does not need a id or a path. Has to be created with an offline client.
      /// </param>
      /// <returns>Boolean indicating whether the creation was succesful</returns>
    public bool AddOfflineCreatedFile(FileInstance fileInstance)
    {
        fileInstance.id = FileListHandler.FileList.IncrementCounter--;
        if (AddNewFile(fileInstance))
        {
            if (FileAdded != null)
                FileAdded(fileInstance);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Tries to modify the existing file given as param. If the file does not exist, it will create a new file
    /// with the specified path or at the rootpath.
    /// </summary>
    /// <param name="fileInstance">The file to modify</param>
    /// <returns></returns>
    public bool ModifyFile(FileInstance fileInstance) {

        if(fileInstance == null){
            throw new ArgumentException();
        }

        if (AddNewFile(fileInstance))
        {
            if(FileChanged != null) 
                FileChanged(fileInstance);
            return true;
        }
        return false;
    
    }

    /// <summary>
    /// Tries to delete a file. Throws exception if the file is already flagged for deletion.
    /// 
    /// </summary>
    /// <param name="fileInstance">The file to be deleted. Need a full path to work</param>
    /// <returns>True if sucessful, failure in any exception case</returns>
    public bool DeleteFile(FileInstance fileInstance)
    {
        //Is metadata availible.
        if (fileInstance.deleted <= 0)
        {
            throw new ArgumentException();
        }
    
        try
        {
    
            //Delete the file through System.IO.File class, not Slice Of Pie File class.
            String deletePath = System.IO.Path.Combine(fileInstance.File.serverpath, fileInstance.File.name);
            Debug.WriteLine(deletePath); 
            System.IO.File.Delete(deletePath);

            if (FileDeleted != null)
                FileDeleted(fileInstance);
            return true;
        }
        catch (System.IO.IOException)
        {
            throw new System.IO.IOException("Deleting File on disk :" + fileInstance.File.name + " failed."); 
        }
      
    }

      /// <summary>
      /// Tries to rename the file specified in the param with the newName param. 
      /// Overwrites any file existing at the new name.
      /// </summary>
      /// <param name="fileInstance">The old file path and name</param>
      /// <param name="newName">The new name of the file. NB: THIS IS ONLY THE FILENAME NOT PATH</param>
    public void RenameFile(FileInstance fileInstance, string newName)
    {
        // Should be less than zero, flag for deleted. If zero might not be initialized.
        if (fileInstance.deleted < 0)
        {
            throw new ArgumentException();
        }
        string fullPath = System.IO.Path.Combine(fileInstance.File.serverpath, fileInstance.File.name);
        string newFullPath = System.IO.Path.Combine(fileInstance.File.serverpath, newName);
       
        MoveFile(fullPath, newFullPath);
        fileInstance.File.name = newName;

        if (FileRenamed != null)
            FileRenamed(fileInstance);

    }

    private void MoveFile(String fullOldPath, String fullNewPath)
    {
        System.IO.File.Move(fullOldPath, fullNewPath);
    }

    /// <summary>
    /// Moves a file from the files old path to the newPath.
    /// </summary>
    /// <param name="fileInstance">The file to move and it's old path</param>
    /// <param name="newPath">The new path</param>
    public void MoveFile(FileInstance fileInstance, string newPath) {
        if (fileInstance.deleted < 0)
        {
            throw new ArgumentException();
        }
        if (!System.IO.Directory.Exists(newPath))
        {
            System.IO.Directory.CreateDirectory(newPath);
        }
        string fullPath = System.IO.Path.Combine(fileInstance.File.serverpath, fileInstance.File.name);
        string newFullPath = System.IO.Path.Combine(newPath, fileInstance.File.name);

        MoveFile(fullPath, newFullPath);

        // Change the file path... in the old file [good idea yet?]
        fileInstance.File.serverpath = newPath;
        if (FileMoved != null)
            FileMoved(fileInstance);
    }
      /// <summary>
      /// Checks for a file in the root folder or the given path
      /// </summary>
      /// <param name="fileInstance">The file to search for</param>
      /// <returns>True if the file is found, false if not existing (at least not in root path)</returns>
    public Boolean FindFile(FileInstance fileInstance)
    {
        String searchPath = fileInstance.File.serverpath;
        searchPath = System.IO.Path.Combine(searchPath, fileInstance.File.name);

        if (System.IO.File.Exists(searchPath))
        {
            return true;
        }
        return false;
    }



    /// <summary>
    /// Retrieves a file from storage using the File's path
    /// </summary>
    /// <param name="id">The id of the file to retrieve</param>
    /// <returns></returns>
    public FileInstance GetFile(long id)
    {
        FileListEntry fileInfo = _fileListHandler.FileList.List[id];
        String fullPath = System.IO.Path.Combine(fileInfo.Path, fileInfo.Name);
        String html = System.IO.File.ReadAllText(fullPath);

        FileInstance loadedFile = HtmlMarshalUtil.UnmarshallDocument(html);
        loadedFile.File.serverpath = fileInfo.Path;
        loadedFile.File.name = fileInfo.Name;

        return loadedFile;
    
    }


    public void UpdateFileId(FileInstance fileInstance, long newId)
    {
        throw new NotImplementedException();
    }
  }
}
