using System;
using System.Collections.Generic;
using SliceOfPie_Model.Persistence;

namespace SliceOfPie_Model
{
    public class OfflineFileListHandler : IFileListHandler
    {

      public const String Logpath = @"C:\test\log";
      public const String Logfile = "filelist.xml";

      private readonly FileList _pFileList;

        public FileList FileList   {
            get
            {
                return _pFileList;
            }
        }

        public OfflineFileListHandler(ICommunicator cm)
        {
            cm.FileAdded += FileAdded;
            cm.FileChanged += FileChangedOnDisk;
            cm.FileDeleted += FileDeleted;
            cm.FileMoved += FileMoved;
            cm.FileRenamed += FileRenamed;
            cm.FilePulled += FilePulled;

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
            // Version from File ?
        }

        private FileListEntry StandardFileEntry(FileInstance file)
        {
            var entry = new FileListEntry {Name = file.File.name, Path = file.File.serverpath, IsDeleted = false};
          return entry;
        }

        public void FileAdded(FileInstance file)
        {
            FileListEntry entry =  StandardFileEntry(file);
            entry.Id = file.id;
            entry.Version = 0.001m;
            FileList.List.Add(entry.Id, entry);  
        }

        public void FileDeleted(FileInstance file)
        {
            FileList.List.Remove(file.id);
        }

        public void FileRenamed(FileInstance file)
        {
            FileList.List[file.id].Name = file.File.name;
        }


        public void PersistFileList()
        {
            String fullPath = System.IO.Path.Combine(Logpath, Logfile);
            String logXML = HtmlMarshalUtil.MarshallFileList(FileList);
            System.IO.File.WriteAllText(fullPath, logXML);

        }

        public void FileChangedOnDisk(FileInstance file)
        {
            FileList.List[file.id].Version += 0.001m;          
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

    }
}
