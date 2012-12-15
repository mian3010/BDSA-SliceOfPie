using System;
using System.Collections.Generic;
using System.Text;
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
    public static string GetAuthors(int id) {
      var output = new StringBuilder();
      output.Append("<h2>Users that own this file</h2>");
      output.Append("<ul>");
      IList<User> authors = Context2.GetUsers(id);
      foreach (var author in authors) {
        output.Append("<li>" + author.email + "</li>");
      }
      output.Append("</ul>");
      return output.ToString();
    }
    public static void AddAuthor(string email, int id) {
      var origInstance = Context2.GetDocument(id);
      if (origInstance != null) {
        var fileInstance = new Document { User_email = email, path = origInstance.path, File_id = origInstance.File_id };
        Context2.AddFileInstance(fileInstance);
      }
    }
    public static FileInstance CreateDocument(string email, string name, decimal version, string path, string title, string content) {
      var file = new SliceOfPie_Model.Persistence.File { name = name, Version = version };
      var document = new Document { User_email = email, File = file, path = path, Content = content, Title = title };
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
          var actionLinks = "<a href=\"/Default/Editor?id=" + currentFile.id + "\" title=\"Edit\"><img src=\"/Images/edit.png\" /></a>" +
            "<a href=\"/Default/Sharer?id=" + currentFile.id + "\" title=\"Share\"><img src=\"/Images/share.png\" /></a>";
          currentStructure.Children.Add(new SliceOfPie_Model.CompositeStructure.File { Label = currentFile.File.name, viewLink = viewLink, actionLinks = actionLinks, viewLinkEnd = viewLinkEnd, viewImage = viewImage });
        }
      return structure.ToString();
    }
  }
}
