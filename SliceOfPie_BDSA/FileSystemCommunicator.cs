using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceOfPie_Model {
  public class FileSystemCommunicator : IAdministrator {
    private ICommunicator communicator;
    public delegate void FileEventHandler(object sender, File file);
    public event FileEventHandler FilesUpdated, ContentAdded, FileSaved;

    //singleton instance
    public static FileSystemCommunicator administrator;

    private FileSystemCommunicator() {

    }

    public static FileSystemCommunicator GetInstance() {
      if (administrator == null)
        administrator = new FileSystemCommunicator();
      return administrator;
    }

    public void SaveFile(File file) {
      bool b = communicator.SaveFile(file);
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
