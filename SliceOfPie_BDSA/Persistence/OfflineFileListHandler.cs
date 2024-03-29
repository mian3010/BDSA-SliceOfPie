﻿using System;
using System.Collections.Generic;
using SliceOfPie_Model.Persistence;

namespace SliceOfPie_Model
{
    public class OfflineFileListHandler : IFileListHandler
    {

      public const String Logpath = @"C:\test\log";
      public const String Logfile = "filelist.xml";

      public const String Changefile = "changes.xml";
    
      private FileList _pFileList;
      private Dictionary<int, List<Change>> _pChangeList; 

        public FileList FileList   {
            get
            {
                return _pFileList;
            }
        }
        
        public Dictionary<int, List<Change>> ChangeList
        {
            get { return _pChangeList; }
        }

        public OfflineFileListHandler(ICommunicator cm)
        {
            cm.FileAdded += FileAdded;
            cm.FileChanged += FileChangedOnDisk;
            cm.FileDeleted += FileDeleted;
            cm.FileMoved += FileMoved;
            cm.FileRenamed += FileRenamed;
            cm.FilePulled += FilePulled;

            InitializeLog();

            InitializeChanges();
        }

        private void InitializeChanges()
        {
            String fullChangePath = System.IO.Path.Combine(Logpath, Changefile);
            if (!System.IO.Directory.Exists(Logpath))
            {
                System.IO.Directory.CreateDirectory(Logpath);
            }
            if (System.IO.File.Exists(fullChangePath))
            {
                String logXml = System.IO.File.ReadAllText(fullChangePath);
                _pChangeList = HtmlMarshalUtil.UnMarshallChangeList(logXml);
            }
            else
            {
                _pChangeList = new Dictionary<int, List<Change>>();
            }
        }

        private void InitializeLog()
        {
            String fullLogPath = System.IO.Path.Combine(Logpath, Logfile);
            if (!System.IO.Directory.Exists(Logpath))
            {
                System.IO.Directory.CreateDirectory(Logpath);
            }
            if (System.IO.File.Exists(fullLogPath))
            {
                String logXml = System.IO.File.ReadAllText(fullLogPath);
                _pFileList = HtmlMarshalUtil.UnMarshallFileList(logXml);
            }
            else
            {
                _pFileList = new FileList();
                FileList.List = new Dictionary<int, FileListEntry>();
                FileList.IncrementCounter = -1;
            }
        }





        public void FilePulled(FileInstance file)
        {
            FileListEntry entry = StandardFileEntry(file);
            entry.Id = file.id;
            FileList.List.Add(entry.Id, entry); 

            foreach (Change change in file.File.Changes)
            {
                AddChange(change);
            }

            // Version from File ?
        }

        private FileListEntry StandardFileEntry(FileInstance file)
        {
            var entry = new FileListEntry {Name = file.File.name, Path = file.path, IsDeleted = false};
            return entry;
        }

        public void FileAdded(FileInstance file)
        {
            
            FileListEntry entry =  StandardFileEntry(file);
            entry.Id = file.id;
            entry.Version = 0.001m;
            FileList.List.Add(entry.Id, entry);
            Change c = MakeChangeFromFile(file, "created");
            file.File.Changes.Add(c);
            AddChange(c);
        }

        public void FileDeleted(FileInstance file)
        {
            FileList.List.Remove(file.id);
        }

        public void FileRenamed(FileInstance file)
        {
            FileList.List[file.id].Name = file.File.name;
        }

        /// <summary>
        /// Persists the FileList to the path given in Logfile
        /// </summary>
        public void PersistFileList()
        {
            String fullPath = System.IO.Path.Combine(Logpath, Logfile);
            String logXML = HtmlMarshalUtil.MarshallFileList(FileList);
            System.IO.File.WriteAllText(fullPath, logXML);

        }

        /// <summary>
        /// Persists the Changelist to the path given in Changefile.
        /// </summary>
        public void PersistChangeList()
        {
            String fullPath = System.IO.Path.Combine(Logpath, Changefile);
            String logXML = HtmlMarshalUtil.MarshallChangeList(ChangeList);
            System.IO.File.WriteAllText(fullPath, logXML);
        }

        public void FileChangedOnDisk(FileInstance file)
        {

            FileList.List[file.id].Version += 0.001m;
           
            Change change = MakeChangeFromFile(file, "modified");
            file.File.Changes.Add(change);
            AddChange(change);

        }

        public Change MakeChangeFromFile(FileInstance file, string action)
        {
            var change = new Change();
            if (file.User_email == null)
                change.User_email = "Unknown";
            else
                change.User_email = file.User_email;
            change.timestamp = DateTime.Now.Ticks;
            change.change1 = action;
            change.File_id = file.id;
            return change;
        }

        private void AddChange(Change change)
        {
            int file_id = change.File_id;
            List<Change> list = null;
          
            if (ChangeList.TryGetValue(file_id, out list))
            {
                list.Add(change);
                ChangeList[file_id] = list;
            }
            else
            {
                list = new List<Change>();
                list.Add(change);
                ChangeList.Add(file_id, list);
            }
        }

        public void FileChangedOnServer(FileInstance file)
        {
            // We need version, possibly passed on from file?
        }

        public void FileMoved(FileInstance file)
        {
            FileList.List[file.id].Path = file.File.serverpath;
        }

        public Dictionary<String, int> GetPathsWithId()
        {
            var dic = new Dictionary<String, int>();

            foreach (FileListEntry entry in FileList.List.Values)
            {
                dic[System.IO.Path.Combine(entry.Path, entry.Name)] = entry.Id;
            }

            return dic;
        }

        public void ChangeIdOnFile(FileInstance file, int newID)
        {
            FileListEntry entry = null;
            if(FileList.List.TryGetValue(file.id, out entry))
            {
                entry.Id = newID;
            }
            List<Change> log = null;
            if (ChangeList.TryGetValue(file.id, out log))
            {
                foreach (Change c in log)
                {
                    c.File_id = newID;
                }
            }
            file.id = newID;

        }



    }
}
