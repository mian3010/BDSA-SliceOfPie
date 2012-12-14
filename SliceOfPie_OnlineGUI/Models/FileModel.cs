using System;
using System.Collections.Generic;
using SliceOfPie_Model;
using SliceOfPie_Model.Persistence;
using SliceOfPie_Model.CompositeStructure;
using System.Windows.Data;

namespace SliceOfPie_OnlineGUI.Models {
  public class FileModel {
    public static Document GetDocument(int id) {
      try {
        return Document.CreateDocument(Context2.GetFileInstance(1));
      } catch (NotADocumentException) {
        return null;
      }
    }
    public static FileInstance GetFile(int id) {
      return Context2.GetFileInstance(id);

    }
    public static string FileListToTree(IEnumerable<FileInstance> list) {
      var structure = new Folder { Label = "File list" };
      if (list != null)
        foreach (var currentFile in list) {
          var currentStructure = structure;
          var pathSplit = currentFile.path.Split('\\');
          foreach (var folder in pathSplit) {
            if (folder.Length == 0) continue;
            var found = false;
            while (!found) {
              try {
                foreach (Folder child in currentStructure.Children) {
                  if (child.Label.Equals(folder)) {
                    currentStructure = child;
                    found = true;
                  }
                }
              } catch (InvalidCastException) {
              }
              if (!found) {
                var newFolder = new Folder { Label = folder };
                currentStructure.Children.Add(newFolder);
                currentStructure = newFolder;
                found = true;
              }
            }
          }
          var viewLink = "<a href=\"/Default/Viewer?id="+currentFile.id+"\">View</a>";
          var editLink = "<a href=\"/Default/Editor?id=" + currentFile.id + "\">Edit</a>";
          currentStructure.Children.Add(new SliceOfPie_Model.CompositeStructure.File { Label = currentFile.File.name, viewLink = viewLink, editLink = editLink});
        }
      return structure.ToString();
    }
  }
}
