using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceOfPie_Model {
  public class OfflineAdministrator : IAdministrator {

    private ICommunicator communicator;
    private OfflineLogger logger;
    public delegate void FileEventHandler(object sender, File file);
    public event FileEventHandler FilesUpdated, ContentAdded, FileSaved;

    //singleton instance
    public static OfflineAdministrator administrator;

    /// <summary>
    /// Constructs offlineAdministrator with a static rootpath for now.
    /// </summary>
    private OfflineAdministrator() {
        /// This is not very smart I think. Perhaps logger should be a composite object in offline adapter.
        communicator = new CommunicatorOfflineAdapter();
        logger = new OfflineLogger((CommunicatorOfflineAdapter) communicator);
    }

    public static OfflineAdministrator GetInstance() {
      if (administrator == null)
        administrator = new OfflineAdministrator();
      return administrator;
    }

    public void Synchronize()
    {
        // Get log from Logger
        List<LogEntry> offlineLog = logger.OfflineLog;

        foreach (LogEntry entry in offlineLog)
        {
            switch (entry.modification)
            {
                //case FileModification.Add:  break;
                //case FileModification.Add: break;
                //case FileModification.Add: break;
                //case FileModification.Add: break;
                //case FileModification.Add: break;
                //case FileModification.Add: break;
                //case FileModification.Add: break;

            }
        }

    }

    public void SaveFile(File file) {
      // bool b = communicator.SaveFile(file);
        bool b = true;
      if (b)
        FileSaved(this, file);
    }

    public void OpenFile(File file) {

    }

    public void GetAllFiles() {
      throw new NotImplementedException();
    }

  }
}
