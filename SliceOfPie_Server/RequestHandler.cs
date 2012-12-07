﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SliceOfPie_Model;
using SliceOfPie_Model.FileList;
using SliceOfPie_Network;

namespace SliceOfPie_Server {
  class RequestHandler {
    private RequestHandler();

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

    public void ReceiveNewFile(File File) {

    }

    public void ReceiveModifiedFile(File File) {

    }

    public void ReceiveFileList(FileList fileList) {

    }

    public void ReviewFileList(FileList fileList) {
      FileListReviewer.Review(fileList);
    }
  }

  class FileListReviewer {
    private FileList fileList;
    public FileListReviewer();

    public void Review(FileList fileList) {
      foreach (FileListEntry Entry in fileList.List.Values) {

      }
    }

    private FileList ServerFileList() {
      
      return null;
    }

    private void HandleFileRename(LogEntry Entry) {
      // Add change to change table in db
      // Do rename
      // Add change to server log
      throw new NotImplementedException();
    }

    private void HandleFileMove(LogEntry Entry) {
      // Add change to change table in db
      // Change FileInstance path
      // Add change to server log
      throw new NotImplementedException();
    }

    private void HandleFileModify(LogEntry Entry) {
      // Check if okay
      // Add to okay to modify list
      Program.instance.AddToModifyList(Entry.id); // TODO file id
      // Tell client to PUT file
      throw new NotImplementedException();
    }

    private void HandleMergeReady(LogEntry Entry) {
      throw new NotImplementedException();
    }

    private void HandleDeleteFile(LogEntry Entry) {
      // Add change to change table in db
      // Do delete
      // Add change to server log
      throw new NotImplementedException();
    }

    private void HandleAddFile(LogEntry Entry) {
      // Check if okay
      // Add to okay to add list // Temp ID?
      // Tell client to PUT file
      throw new NotImplementedException();
    }
  }
}
