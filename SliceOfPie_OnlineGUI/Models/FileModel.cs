using System;
using System.Collections.Generic;
using SliceOfPie_Model.Persistence;
using SliceOfPie_Model.CompositeStructure;

namespace SliceOfPie_OnlineGUI.Models {
  public static class FileModel {
    public static Document GetDocument(int id) {
      return Context2.GetDocument(id);
    }
    public static void ModifyDocument(int id, string title, string content) {
      Context2.ModifyDocument(id, title, content);
    }
    public static FileInstance CreateDocument(string email, string name, decimal version, string path, string title, string content)
    {
      var file = new SliceOfPie_Model.Persistence.File {name = name, Version = version};
      var document = new Document {User_email = email, File = file, path = path, Content = content, Title = title};
      return Context2.AddFileInstance(document);
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
          var viewLink = "<a class=\"FileLink\" href=\"/Default/Viewer?id=" + currentFile.id + "\" title=\"View\">";
          const string viewLinkEnd = "</a>";
          const string viewImage = "<img src=\"/Images/open.png\" />";
          var editLink = "<a href=\"/Default/Editor?id=" + currentFile.id + "\" title=\"Edit\"><img src=\"/Images/edit.png\" /></a>";
          currentStructure.Children.Add(new SliceOfPie_Model.CompositeStructure.File { Label = currentFile.File.name, viewLink = viewLink, editLink = editLink, viewLinkEnd = viewLinkEnd, viewImage = viewImage });
        }
      return structure.ToString();
    }
  }
}
