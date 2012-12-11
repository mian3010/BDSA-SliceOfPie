using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SliceOfPie_Model;
using System.Threading;
using SliceOfPie_Model.Persistence;

namespace SliceOfPie_Server {
  public class RequestHandler {
    private RequestHandler() { }
    public static void Main(string[] args) {
      FileInstance fi = FileInstance.CreateFileInstance(0, "test", "test", 42);
      new SliceOfLifeEntities().AddToFileInstances(fi);
      Context.GetServerFileList("test");
    }

    /// <summary>
    /// Get the list of approved to receive new files
    /// </summary>
    private List<long> _newFileList;
    public List<long> PendingNewFileList {
      get {
        if (_newFileList == null) {
          _newFileList = new List<long>();
        }
        return _newFileList;
      }
    }

    /// <summary>
    /// Get the list of approved to receive modified files
    /// </summary>
    private List<long> _modFileList;
    public List<long> PendingModFileList {
      get {
        if (_modFileList == null) {
          _modFileList = new List<long>();
        }
        return _modFileList;
      }
    }

    /// <summary>
    /// Get this singleon instance
    /// </summary>
    private static RequestHandler _tinstance;
    public static RequestHandler Instance {
      get {
        if (_tinstance == null) {
          _tinstance = new RequestHandler();
        }
        return _tinstance;
      }
    }

    /// <summary>
    /// Receive a file. This will initialize a new thread
    /// </summary>
    /// <param name="file"></param>
    /// <param name="hp"></param>
    public void ReceiveFile(File file, HttpProcessor hp) {
      FileReceiver fr = new FileReceiver(file, hp);
      Thread thread = new Thread(() => fr.Receive());
      thread.Start();
    }

    /// <summary>
    /// Receive a FileList. This will initialize a new thread
    /// </summary>
    /// <param name="fileList"></param>
    /// <param name="hp"></param>
    public void ReceiveFileList(FileList fileList, HttpProcessor hp) {
      ReviewFileList(fileList, hp);
    }

    private void ReviewFileList(FileList fileList, HttpProcessor hp) {
      FileListReviewer fr = new FileListReviewer(fileList, hp);
      Thread thread = new Thread(() => fr.Review());
      thread.Start();
    }

    /// <summary>
    /// Get a file from the server
    /// </summary>
    /// <param name="id"></param>
    /// /// <param name="processor"></param>
    /// <returns></returns>
    public void GetFile(long id, HttpProcessor processor) {
      //TODO: Test this
      File file = Context.GetFile(id);
      processor.RecieveFile(file);
    }
  }

  /// <summary>
  /// This class handles file receives.
  /// This will usually be started in a seperate thread.
  /// </summary>
  class FileReceiver {
    readonly HttpProcessor _hp;
    readonly File _file;

    /// <summary>
    /// Constructor. 
    /// This will call a method upon the HTTPProcessor, to tell wether this was succesful
    /// </summary>
    /// <param name="file"></param>
    /// <param name="hp"></param>
    public FileReceiver(File file, HttpProcessor hp) {
      this._file = file;
      this._hp = hp;
    }

    public void Receive() {
      long succes = -2; // will be returned
      // succes == id if succes
      // -1 if failed
      // -2 if reject

      // Determin new or mod
      // If new file
      if (RequestHandler.Instance.PendingNewFileList.Contains(_file.id)) {
         succes = Context.SaveFile(_file);

        // else if mod file
      } else if (RequestHandler.Instance.PendingModFileList.Contains(_file.id)) {
        succes = Context.UpdateFile(_file);

        // else reject
      } else {

      }
      _hp.RecieveConfirmation(succes);
    }
  }

  /// <summary>
  /// This class handles FileList receives.
  /// This will usually be started in a seperate thread.
  /// </summary>
  class FileListReviewer {
    private readonly FileList _fileList;
    private HttpProcessor _hp;

    /// <summary>
    /// Constructor. 
    /// This will call a method upon the HTTPProcessor, to tell wether this was succesful
    /// </summary>
    /// <param name="fileList"></param>
    /// <param name="hp"></param>
    public FileListReviewer(FileList fileList, HttpProcessor hp) {
      this._fileList = fileList;
      this._hp = hp;
    }

    public void Review() {
      foreach (FileListEntry entry in _fileList.List.Values) {
        // if file exists
        if (Context.GetFile(entry.Id) != null) {

        } else {

        }
      }
      //hp.something(fileList);
    }

    //Get the servers filelist for compare
    private FileList ServerFileList() {
      throw new NotImplementedException();
    }

    private void HandleFileRename(FileListEntry entry) {
      // Add change to change table in db
      // Do rename
      // Add change to server log
      throw new NotImplementedException();
    }

    private void HandleFileMove(FileListEntry entry) {
      // Add change to change table in db
      // Change FileInstance path
      // Add change to server log
      throw new NotImplementedException();
    }

    private void HandleFileModify(FileList entry) {
      // Check if okay
      // Add to okay to modify list
      // Program.instance.AddToModifyList(Entry.id); // TODO file id
      // Tell client to PUT file
      throw new NotImplementedException();
    }

    private void HandleMergeReady(FileList entry) {
      //Context.
      throw new NotImplementedException();
    }

    private void HandleDeleteFile(FileList entry) {
      // Add change to change table in db
      // Do delete
      // Add change to server log
      throw new NotImplementedException();
    }

    private void HandleAddFile(FileList entry) {
      // Check if okay
      // Add to okay to add list // Temp ID?
      // Tell client to PUT file
      throw new NotImplementedException();
    }
  }
}
