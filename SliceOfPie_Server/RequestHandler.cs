using System.Collections.Generic;
using SliceOfPie_Model;
using System.Threading;
using SliceOfPie_Model.Persistence;

namespace SliceOfPie_Server {
  public class RequestHandler {
    private RequestHandler() { }

    /// <summary>
    /// Get the list of approved to receive new files
    /// </summary>
    private List<int> _newFileList;
    public List<int> PendingNewFileList {
      get { return _newFileList ?? (_newFileList = new List<int>()); }
    }

    /// <summary>
    /// Get the list of approved to receive modified files
    /// </summary>
    private Dictionary<int, FileListEntry> _modFileList;
    public Dictionary<int, FileListEntry> PendingModFileList {
      get { return _modFileList ?? (_modFileList = new Dictionary<int, FileListEntry>()); }
    }

    /// <summary>
    /// Get this singleon instance
    /// </summary>
    private static RequestHandler _instance;
    public static RequestHandler Instance {
      get { return _instance ?? (_instance = new RequestHandler()); }
    }

 
    /// <summary>
    /// Receive a file. This will initialize a new thread
    /// </summary>
    /// <param name="file"></param>
    /// <param name="hp"></param>
    public void ReceiveFile(FileInstance file, HttpProcessor hp) {
      var fr = new FileReceiver(file, hp);
      var thread = new Thread(fr.Receive);
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
      var fr = new FileListReviewer(fileList, hp);
      var thread = new Thread(fr.Review);
      thread.Start();
    }

    /// <summary>
    /// Get a file from the server
    /// </summary>
    /// <param name="id">FileId</param>
    /// /// <param name="processor">processor</param>
    /// <returns></returns>
    public void GetFileInstance(int id, HttpProcessor processor) {
      var file = Context.GetFileInstance(id);
      processor.RecieveFile(file);
    }
  }
}


