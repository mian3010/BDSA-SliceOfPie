using System.Collections.Generic;
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
      var user = User.CreateUser("test@example.com");
      Context.AddUser(user);

      // Add files
      File file = File.CreateFile(1, "test file.txt", @"C:\ServerFiles\", 0);
      var document = (Document)FileInstance.CreateFileInstance(1, "davs", @"C:\ClientFiles\", file.id);

      document.Content = "This is a test file. Does this work? \n New line";
      Context.AddFile(document);

      // Add FileInstance, bind to user
      var fileInstance = FileInstance.CreateFileInstance(1, user.email, @"C:\ClientFiles\", file.id);
      Context.AddFileInstance(fileInstance);

      // Add MetaData to file
      var metaDataType = MetaDataType.CreateMetaDataType("Test type");
      var fileMetaData = FileMetaData.CreateFileMetaData(1, metaDataType.ToString(), file.id);
      Context.AddFileMetaData(fileMetaData);
    }

    /// <summary>
    /// Get the list of approved to receive new files
    /// </summary>
    private List<long> _newFileList;
    public List<long> PendingNewFileList {
      get { return _newFileList ?? (_newFileList = new List<long>()); }
    }

    /// <summary>
    /// Get the list of approved to receive modified files
    /// </summary>
    private Dictionary<long, FileListEntry> _modFileList;
    public Dictionary<long, FileListEntry> PendingModFileList {
      get { return _modFileList ?? (_modFileList = new Dictionary<long, FileListEntry>()); }
    }

    /// <summary>
    /// Get this singleon instance
    /// </summary>
    private static RequestHandler _tinstance;
    public static RequestHandler Instance {
      get { return _tinstance ?? (_tinstance = new RequestHandler()); }
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
    /// <param name="id"></param>
    /// /// <param name="processor"></param>
    /// <returns></returns>
    public void GetFile(long id, HttpProcessor processor) {
      //TODO: Test this
      FileInstance file = Context.GetFile(id);
      processor.RecieveFile(file);
    }
  }
}


