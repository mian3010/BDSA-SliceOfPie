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
      private const String Logpath = @"C:\test\log";
      private const String Logfile = "filelist.xml";

      private readonly FileList p_fileList;
        public FileList FileList   {
            get
            {
                return p_fileList;
            }
        }

        public FileList FileList1
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
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
                p_fileList = HtmlMarshalUtil.UnMarshallFileList(logXml);

            }
            else
            {
                p_fileList = new FileList();
                FileList.List = new Dictionary<long, FileListEntry>();
                FileList.IncrementCounter = -1;
            }
        }

      private void FilePulled(File file)
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

      private void FileAdded(File file)
        {
            FileListEntry entry =  StandardFileEntry(file);
            entry.Id = file.id;
            entry.Version = 0.001m;
            FileList.List.Add(entry.Id, entry);  
        }

      private void FileDeleted(File file)
        {
            FileList.List.Remove(file.id);
        }

      private void FileRenamed(File file)
        {
            FileList.List[file.id].Name = file.name;
        }


        public void PersistFileList()
        {
            String fullPath = System.IO.Path.Combine(Logpath, Logfile);
            String logXml = HtmlMarshalUtil.MarshallFileList(FileList);
            System.IO.File.WriteAllText(fullPath, logXml);
        }

      private void FileChangedOnDisk(File file)
        {
            FileList.List[file.id].Version += 0.001m;          
        }

        public void FileChangedOnServer(File file)
        {
            // We need version, possibly passed on from file?
        }

      private void FileMoved(File file)
        {
            FileList.List[file.id].Path = file.serverpath;
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
