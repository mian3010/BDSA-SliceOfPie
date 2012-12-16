using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using SliceOfPie_Model.Persistence;
using File = System.IO.File;

namespace SliceOfPie_Model
{
    /// <summary>
    ///     The offline adapter for the ICommunicator interface. Implements persistent storage on disk.
    ///     Also distinguishes between adding files on disk that's retrieved from server or just created offline.
    /// </summary>
    public class CommunicatorOfflineAdapter : ICommunicator
    {
        // The object takes a path for the root folder of the SoP documents. Each document will be automatically saved from there.
        private static CommunicatorOfflineAdapter _adapter;
        private readonly OfflineFileListHandler _fileListHandler;

        private readonly HashSet<FileInstance> _cache;

        private CommunicatorOfflineAdapter()
        {
            _fileListHandler = new OfflineFileListHandler(this);
            _cache = new HashSet<FileInstance>();
        }

        public IFileListHandler FileListHandler
        {
            get { return _fileListHandler; }
        }

        public event FileInstanceEventHandler FileAdded ,
            FileChanged ,
            FileDeleted ,
            FileMoved ,
            FileRenamed ,
            FilePulled;

        public void AddFile(FileInstance file)
        {
            if (file.id <= 0)
            {
                AddOfflineCreatedFile(file);
            }
            else
            {
                AddFileFromRemote(file);
            }
        }

        /// <summary>
        ///     Tries to modify the existing file given as param. If the file does not exist, it will create a new file
        ///     with the specified path or at the rootpath.
        /// </summary>
        /// <param name="fileInstance">The file to modify</param>
        /// <returns></returns>
        public bool ModifyFile(FileInstance fileInstance)
        {
            if (fileInstance == null)
            {
                throw new ArgumentException();
            }

            if (AddNewFile(fileInstance))
            {
                if (FileChanged != null)
                    FileChanged(fileInstance);
                return true;
            }
            return false;
        }

        /// <summary>
        ///     Tries to delete a file. Throws exception if the file is already flagged for deletion.
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
                String deletePath = Path.Combine(fileInstance.File.serverpath, fileInstance.File.name);
                Debug.WriteLine(deletePath);
                File.Delete(deletePath);

                if (FileDeleted != null)
                    FileDeleted(fileInstance);
                return true;
            }
            catch (IOException)
            {
                throw new IOException("Deleting File on disk :" + fileInstance.File.name + " failed.");
            }
        }

        /// <summary>
        ///     Tries to rename the file specified in the param with the newName param.
        ///     Overwrites any file existing at the new name.
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
            string fullPath = Path.Combine(fileInstance.File.serverpath, fileInstance.File.name);
            string newFullPath = Path.Combine(fileInstance.File.serverpath, newName);

            MoveFile(fullPath, newFullPath);
            fileInstance.File.name = newName;

            if (FileRenamed != null)
                FileRenamed(fileInstance);
        }

        /// <summary>
        ///     Moves a file from the files old path to the newPath.
        /// </summary>
        /// <param name="fileInstance">The file to move and it's old path</param>
        /// <param name="newPath">The new path</param>
        public void MoveFile(FileInstance fileInstance, string newPath)
        {
            if (fileInstance.deleted < 0)
            {
                throw new ArgumentException();
            }
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            string fullPath = Path.Combine(fileInstance.File.serverpath, fileInstance.File.name);
            string newFullPath = Path.Combine(newPath, fileInstance.File.name);

            MoveFile(fullPath, newFullPath);

            // Change the file path... in the old file [good idea yet?]
            fileInstance.File.serverpath = newPath;
            if (FileMoved != null)
                FileMoved(fileInstance);
        }


        /// <summary>
        ///     Retrieves a file from storage using the File's path. Also loads the history of the file using FileListHandler
        /// </summary>
        /// <param name="id">The id of the file to retrieve</param>
        /// <returns></returns>
        public FileInstance GetFile(int id)
        {
            foreach (FileInstance fileInstance in _cache)
            {
                if (fileInstance.id == id)
                    return fileInstance;
            }
            FileListEntry fileInfo = _fileListHandler.FileList.List[id];
            String fullPath = Path.Combine(fileInfo.Path, fileInfo.Name);
            String html = File.ReadAllText(fullPath);

            FileInstance loadedFile = HtmlMarshalUtil.UnmarshallDocument(html);
            loadedFile.File = new Persistence.File();
            loadedFile.File.serverpath = fileInfo.Path;
            loadedFile.path = fileInfo.Path;
            loadedFile.File.name = fileInfo.Name;

            // Load changes
            if (_fileListHandler.ChangeList.ContainsKey(id))
            {
                foreach (Change change in _fileListHandler.ChangeList[id])
                {
                    loadedFile.File.Changes.Add(change);
                }
            }



            _cache.Add(loadedFile);
            return loadedFile;
        }


        public void UpdateFileId(FileInstance file, int newId)
        {
            int oldID = file.id;
            FileListHandler.ChangeIdOnFile(file, newId);

            // We might not have the same fileInstance object so we're gonna check and compare on id.
            foreach (FileInstance f in _cache)
            {
                if (f.id == oldID)
                    f.id = newId;
            }
        }

        public static CommunicatorOfflineAdapter GetCommunicatorInstance()
        {
            return _adapter ?? (_adapter = new CommunicatorOfflineAdapter());
        }

        /// <summary>
        ///     Adds a file from remote storage. Should be used during synchronization.
        /// </summary>
        /// <param name="file">The file to add from a remote location</param>
        /// <returns>True if successful, false otherwise</returns>
        private bool AddFileFromRemote(FileInstance file)
        {
            if (AddNewFile(file))
            {
                if (FilePulled != null)
                    FilePulled(file);
                return true;
            }
            return false;
        }

        private bool AddNewFile(FileInstance file)
        {
            _cache.Add(file);
            if (!Directory.Exists(file.path))
            {
                Directory.CreateDirectory(file.path);
            }
            string fullpath = Path.Combine(file.path, file.File.name);
            String fileHtml = HtmlMarshalUtil.MarshallFile(file);
            if (!File.Exists(fullpath))
            {
                File.WriteAllText(fullpath, fileHtml);
                return true;
            }
            // TO-DO Maybe do some other semantic than just doing it anyways -> can we overwrite?
            File.WriteAllText(fullpath, fileHtml);

            return true;
        }

        /// <summary>
        ///     Add a new file to the disk with either the serverpath specified in the file or the default rootpath.
        ///     Will overwrite existing files. Must be run with administrator rights. Also saves a LogEntry describing the action
        /// </summary>
        /// <param name="fileInstance">
        ///     The file to be added. Does not need a id or a path. Has to be created with an offline client.
        /// </param>
        /// <returns>Boolean indicating whether the creation was succesful</returns>
        private bool AddOfflineCreatedFile(FileInstance fileInstance)
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

        private void MoveFile(String fullOldPath, String fullNewPath)
        {
            File.Move(fullOldPath, fullNewPath);
        }

        /// <summary>
        ///     Checks for a file in the root folder or the given path
        /// </summary>
        /// <param name="fileInstance">The file to search for</param>
        /// <returns>True if the file is found, false if not existing (at least not in root path)</returns>
        public Boolean FindFile(FileInstance fileInstance)
        {
            String searchPath = fileInstance.File.serverpath;
            searchPath = Path.Combine(searchPath, fileInstance.File.name);

            if (File.Exists(searchPath))
            {
                return true;
            }
            return false;
        }
    }
}