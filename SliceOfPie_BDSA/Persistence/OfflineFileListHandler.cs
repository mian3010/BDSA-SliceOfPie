using System;
using System.Collections.Generic;
using SliceOfPie_Model.Persistence;

namespace SliceOfPie_Model
{
    public class OfflineFileListHandler : IFileListHandler
    {
        public readonly String logpath = @"C:\test\log";
        public readonly String logfile = "filelist.xml";

        private FileList p_fileList;
        public FileList FileList   {
            get
            {
                return p_fileList;
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

            String fullLogPath = System.IO.Path.Combine(logpath, logfile);
            if (!System.IO.Directory.Exists(logpath))
            {
                System.IO.Directory.CreateDirectory(logpath);
            }
            if (System.IO.File.Exists(fullLogPath))
            {
                String logXml = System.IO.File.ReadAllText(fullLogPath);
                p_fileList = HtmlMarshalUtil.UnMarshallFileList(logXml);

            }
            else
            {
                p_fileList = new FileList();
                FileList.List = new Dictionary<long, FileListEntry>();
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
            FileListEntry entry = new FileListEntry();
            entry.Name = file.File.name;
            entry.Path = file.File.serverpath;
            entry.IsDeleted = false;
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
            String fullPath = System.IO.Path.Combine(logpath, logfile);
            String logXml = HtmlMarshalUtil.MarshallFileList(FileList);
            System.IO.File.WriteAllText(fullPath, logXml);
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

        public Dictionary<String, long> GetPathsWithId()
        {
            Dictionary<String, long> dic = new Dictionary<String, long>();

            foreach (FileListEntry entry in FileList.List.Values)
            {
                dic[System.IO.Path.Combine(entry.Path, entry.Name)] = entry.Id;
            }

            return dic;
        }

    }
}
