using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SliceOfPie_Model;
using SliceOfPie_Network;

namespace SliceOfPie_Server
{
    class FileListHandler
    {
      private FileListHandler();

      private static FileListHandler tinstance;
      public static FileListHandler instance
        {
            get
            {
                if (tinstance == null)
                {
                  tinstance = new FileListHandler();
                }
                return tinstance;
            }
        }

        private List<long> ListFilesToAdd = new List<long>();
        private List<long> ListFilesToMod = new List<long>();
        
        private Execute tExecuter;
        private Execute Executer
        {
            get
            {
                if (tExecuter == null)
                {
                    //tExecuter = new Execute(LogQue);
                }
                return tExecuter;
            }
        }

        public void ReceiveNewFile(File File)
        {
            
        }

        public void ReceiveModifiedFile(File File)
        {
            if (ListFilesToMod.Contains(File.id))
            {

            }
        }

        public void ReceiveFileList(FileList fileList)
        {
            
        }

        public void ExecuteLogQue()
        {
            Executer.Execute();
        }

        public void AddToModifyList(long id){
            //make private
            ListFilesToMod.Add(id);
        }

        public void AddToAddList(string s){
            //make private
        }
    }

    class Execute
    {
        private Queue<LogEntry> Que;
        public Execute(Queue<LogEntry> Que)
        {
            this.Que = Que;
        }
        public void Execute()
        {
            while (Que.Count > 0)
            {
                LogEntry Entry = Que.Dequeue();
                switch (Entry.modification)
                {
                    case FileModification.Add:
                        HandleAddFile(Entry);
                        break;
                    case FileModification.Delete:
                        HandleDeleteFile(Entry);
                        break;
                    case FileModification.MergeReady:
                        HandleMergeReady(Entry);
                        break;
                    case FileModification.Modify:
                        HandleFileModify(Entry);
                        break;
                    case FileModification.Move:
                        HandleFileMove(Entry);
                        break;
                    case FileModification.Rename:
                        HandleFileRename(Entry);
                        break;
                }
            }
        }

        private void HandleFileRename(LogEntry Entry)
        {
            // Add change to change table in db
            // Do rename
            // Add change to server log
            throw new NotImplementedException();
        }

        private void HandleFileMove(LogEntry Entry)
        {
            // Add change to change table in db
            // Change FileInstance path
            // Add change to server log
            throw new NotImplementedException();
        }

        private void HandleFileModify(LogEntry Entry)
        {
            // Check if okay
            // Add to okay to modify list
            Program.instance.AddToModifyList(Entry.id); // TODO file id
            // Tell client to PUT file
            throw new NotImplementedException();
        }

        private void HandleMergeReady(LogEntry Entry)
        {
            throw new NotImplementedException();
        }

        private void HandleDeleteFile(LogEntry Entry)
        {
            // Add change to change table in db
            // Do delete
            // Add change to server log
            throw new NotImplementedException();
        }

        private void HandleAddFile(LogEntry Entry)
        {
            // Check if okay
            // Add to okay to add list // Temp ID?
            // Tell client to PUT file
            throw new NotImplementedException();
        }
    }
}
