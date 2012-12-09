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
    private List<long> NewFileList;
    public List<long> PendingNewFileList {
      get {
        if (NewFileList == null) {
          NewFileList = new List<long>();
        }
        return NewFileList;
      }
    }

    /// <summary>
    /// Get the list of approved to receive modified files
    /// </summary>
    private List<long> ModFileList;
    public List<long> PendingModFileList {
      get {
        if (ModFileList == null) {
          ModFileList = new List<long>();
        }
        return ModFileList;
      }
    }

    /// <summary>
    /// Get this singleon instance
    /// </summary>
    private static RequestHandler tinstance;
    public static RequestHandler instance {
      get {
        if (tinstance == null) {
          tinstance = new RequestHandler();
        }
        return tinstance;
      }
    }

    /// <summary>
    /// Receive a file. This will initialize a new thread
    /// </summary>
    /// <param name="file"></param>
    /// <param name="hp"></param>
    public void ReceiveFile(File file, HTTPProcessor hp) {
      FileReceiver fr = new FileReceiver(file, hp);
      Thread thread = new Thread(() => fr.Receive());
      thread.Start();
    }

    /// <summary>
    /// Receive a FileList. This will initialize a new thread
    /// </summary>
    /// <param name="fileList"></param>
    /// <param name="hp"></param>
    public void ReceiveFileList(FileList fileList, HTTPProcessor hp) {
      ReviewFileList(fileList, hp);
    }

    private void ReviewFileList(FileList fileList, HTTPProcessor hp) {
      FileListReviewer fr = new FileListReviewer(fileList, hp);
      Thread thread = new Thread(() => fr.Review());
      thread.Start();
    }

    /// <summary>
    /// Get a file from the server
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public File GetFile(long id) {
      //TODO: Test this
      return Context.GetFile(id);
    }
  }

  /// <summary>
  /// This class handles file receives.
  /// This will usually be started in a seperate thread.
  /// </summary>
  class FileReceiver {
    HTTPProcessor hp;
    File file;

    /// <summary>
    /// Constructor. 
    /// This will call a method upon the HTTPProcessor, to tell wether this was succesful
    /// </summary>
    /// <param name="file"></param>
    /// <param name="hp"></param>
    public FileReceiver(File file, HTTPProcessor hp) {
      this.file = file;
      this.hp = hp;
    }

    public void Receive() {
      long succes = -2; // will be returned
      // succes == id if succes
      // -1 if failed
      // -2 if reject

      // Determin new or mod
      // If new file
      if (RequestHandler.instance.PendingNewFileList.Contains(file.id)) {
         succes = Context.SaveFile(file);

        // else if mod file
      } else if (RequestHandler.instance.PendingModFileList.Contains(file.id)) {
        succes = Context.UpdateFile(file);

        // else reject
      } else {

      }
      // hp.something(succes);
    }
  }

  /// <summary>
  /// This class handles FileList receives.
  /// This will usually be started in a seperate thread.
  /// </summary>
  class FileListReviewer {
    private FileList fileList;
    private HTTPProcessor hp;

    /// <summary>
    /// Constructor. 
    /// This will call a method upon the HTTPProcessor, to tell wether this was succesful
    /// </summary>
    /// <param name="fileList"></param>
    /// <param name="hp"></param>
    public FileListReviewer(FileList fileList, HTTPProcessor hp) {
      this.fileList = fileList;
      this.hp = hp;
    }

    public void Review() {
      foreach (FileListEntry Entry in fileList.List.Values) {
        // if file exists
        if (Context.GetFile(Entry.Id) != null) {

        } else {

        }
      }
      //hp.something(fileList);
    }

    //Get the servers filelist for compare
    private FileList ServerFileList() {
      throw new NotImplementedException();
    }

    private void HandleFileRename(FileListEntry Entry) {
      // Add change to change table in db
      // Do rename
      // Add change to server log
      throw new NotImplementedException();
    }

    private void HandleFileMove(FileListEntry Entry) {
      // Add change to change table in db
      // Change FileInstance path
      // Add change to server log
      throw new NotImplementedException();
    }

    private void HandleFileModify(FileList Entry) {
      // Check if okay
      // Add to okay to modify list
      // Program.instance.AddToModifyList(Entry.id); // TODO file id
      // Tell client to PUT file
      throw new NotImplementedException();
    }

    private void HandleMergeReady(FileList Entry) {
      //Context.
      throw new NotImplementedException();
    }

    private void HandleDeleteFile(FileList Entry) {
      // Add change to change table in db
      // Do delete
      // Add change to server log
      throw new NotImplementedException();
    }

    private void HandleAddFile(FileList Entry) {
      // Check if okay
      // Add to okay to add list // Temp ID?
      // Tell client to PUT file
      throw new NotImplementedException();
    }
  }
}
