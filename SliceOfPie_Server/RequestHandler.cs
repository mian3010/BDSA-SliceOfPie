using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SliceOfPie_Model;
using System.Threading;
using SliceOfPie_Model.Persistence;

namespace SliceOfPie_Server {
  /// <summary>
  /// Author: Claus35-DK - clih@itu.dk
  /// </summary>
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
    private Dictionary<long, FileListEntry> ModFileList;
    public Dictionary<long, FileListEntry> PendingModFileList {
      get {
        if (ModFileList == null) {
          ModFileList = new Dictionary<long, FileListEntry>();
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
    /// <returns></returns>
    public File GetFile(long id) {
      //TODO: Test this
      return Context.GetFile(id);
    }
  }
}
