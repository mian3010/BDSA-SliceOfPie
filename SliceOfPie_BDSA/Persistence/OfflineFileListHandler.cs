using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SliceOfPie_Model.Persistence;

namespace SliceOfPie_Model
{
    public class OfflineFileListHandler : IFileListHandler
    {
        public readonly String logpath = @".\Files\log";
        public readonly String logfile = "filelist.xml";

        public FileList FileList         {
            get
            {
                return FileList;
            }
            private set
            {
                String fullLogPath = System.IO.Path.Combine(logpath, logfile);
                if (!System.IO.Directory.Exists(logpath))
                {
                    System.IO.Directory.CreateDirectory(logpath);
                }
                if (System.IO.File.Exists(fullLogPath))
                {
                    String logXML = System.IO.File.ReadAllText(fullLogPath);
                    FileList fileList = HtmlMarshalUtil.UnmarshallFileList(logXML);
                
                }
                else
                {
                    FileList.List = new Dictionary<long, FileListEntry>();
                    FileList.incrementCounter = -1;
                }
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
        }

        public void FilePulled(File file)
        {
            FileListEntry entry = StandardFileEntry(file);
            entry.Id = file.id;
            FileList.List.Add(entry.Id, entry); 
            // Version from File ?
        }

        private FileListEntry StandardFileEntry(File file)
        {
            FileListEntry entry = new FileListEntry();
            entry.Name = file.name;
            entry.Path = file.serverpath;
            entry.IsDeleted = false;
            return entry;
        }

        public void FileAdded(File file)
        {
            FileListEntry entry =  StandardFileEntry(file);
            entry.Id = FileList.incrementCounter--;
            entry.Version = 0.001m;
            file.id = entry.Id;
            FileList.List.Add(entry.Id, entry);  
        }

        public void FileDeleted(File file)
        {
            FileList.List.Remove(file.id);
        }

        public void FileRenamed(File file)
        {
            FileList.List[file.id].Name = file.name;
        }


        public void PersistFileList()
        {
            String fullPath = System.IO.Path.Combine(logpath, logfile);
            String logXML = HtmlMarshalUtil.MarshallFileList(FileList);
            System.IO.File.WriteAllText(fullPath, logXML);
        }

        public void FileChangedOnDisk(File file)
        {
            FileList.List[file.id].Version += 0.001m;          
        }

        public void FileChangedOnServer(File file)
        {
            // We need version, possibly passed on from file?
        }

        public void FileMoved(File file)
        {
            FileList.List[file.id].Path = file.serverpath;
        }



        public string FilesAsXML()
        {
            throw new NotImplementedException();
        }
    }
}
