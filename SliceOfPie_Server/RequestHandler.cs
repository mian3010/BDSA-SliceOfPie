using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SliceOfPie_Model;
using System.Threading;

namespace SliceOfPie_Server {
  class RequestHandler {
    private RequestHandler() { }
    public static void Main(string[] args) {
      FileInstance fi = FileInstance.CreateFileInstance(0, "test", "test", 42);
      new SliceOfLifeEntities().AddToFileInstances(fi);
      Context.GetServerFileList("test");
    }

    private static RequestHandler tinstance;
    public static RequestHandler instance {
      get {
        if (tinstance == null) {
          tinstance = new RequestHandler();
        }
        return tinstance;
      }
    }

    private MergeDaemon mergeDaemon = new MergeDaemon();

    public void ReceiveFile(File File) {
      // Determin new or mod
    }

    public void ReceiveFileList(FileList fileList) {

    }

    public void ReviewFileList(FileList fileList) {
      FileListReviewer fr = new FileListReviewer(fileList);
      Thread thread = new Thread(() => fr.Review());
      thread.Start();
    }

    public File GetFile(long id) {
      return Context.GetFile(id);
    }
  }

  class FileListReviewer {
    private FileList fileList;
    public FileListReviewer(FileList fileList) {
      this.fileList = fileList;
    }

    public void Review() {
      foreach (FileListEntry Entry in fileList.List.Values) {

      }
    }

    private FileList ServerFileList() {

      return null;
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
