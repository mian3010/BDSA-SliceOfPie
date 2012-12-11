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
      /* FileInstance fi = FileInstance.CreateFileInstance(0, "test", "test", 42);
      new SliceOfLifeEntities().AddToFileInstances(fi);
      Context.GetServerFileList("test"); */

      // Fill database with test content
      // Add user
      User user = User.CreateUser("test@example.com");
      //Context.AddUser(user);

      // Add files
      File file = File.CreateFile(1, "test file.txt", @"C:\ServerFiles\", 0);
      file.Content.Append("This is a test file. Does this work? \n New line");
      Context.SaveFile(file);

      // Add FileInstance, bind to user


      // Add MetaData and MetaDataType to file

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
    private Dictionary<long, FileListEntry> _modFileList;
    public Dictionary<long, FileListEntry> PendingModFileList {
      get {
        if (_modFileList == null) {
          _modFileList = new Dictionary<long, FileListEntry>();
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
}
