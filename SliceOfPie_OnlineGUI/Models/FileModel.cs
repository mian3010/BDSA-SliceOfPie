using System;
using System.Collections.Generic;
using SliceOfPie_Model.Persistence;
using SliceOfPie_Model.CompositeStructure;
using System.Windows.Data;

namespace SliceOfPie_OnlineGUI.Models {
  public class FileModel {
    public static Document GetFile(int id) {
      var d = new Document();

      var metaDataType1 = new MetaDataType { Type = "Created date" };
      var fileMetaData1 = new FileMetaData { MetaDataType = metaDataType1, value = "2012-11-27 10:23:11" };

      var metaDataType2 = new MetaDataType { Type = "Owner" };
      var fileMetaData2 = new FileMetaData { MetaDataType = metaDataType2, value = "Michael Søby Andersen" };

      var metaDataType3 = new MetaDataType { Type = "Type" };
      var fileMetaData3 = new FileMetaData { MetaDataType = metaDataType3, value = "Document" };

      d.File.FileMetaDatas.Add(fileMetaData1);
      d.File.FileMetaDatas.Add(fileMetaData2);
      d.File.FileMetaDatas.Add(fileMetaData3);
      d.Content = "Awesome text document here!<br /><strong>This should be bold</strong><br />OMG PIE:<br /><img src=\"http://www.seriouseats.com/images/potd_pi-pie.jpg\" />";
      d.Content += "<br /><br />Testing wrappingggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggg";
      d.Title = "The awesome title";
      return d;
    }
    public static string FileListToTree(IEnumerable<FileInstance> list) {
      CompositeStructure structure = new Folder { Label = "FilesSomthing" };
      foreach (var currentFile in list) {
        var currentStructure = (Folder)structure;
        string[] pathSplit = currentFile.path.Split('\\');
        foreach (var folder in pathSplit) {
          if (folder.Length == 0) continue;
          bool found = false;
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
        currentStructure.Children.Add(new SliceOfPie_Model.CompositeStructure.File { Label = currentFile.File.name });
      }
      return structure.ToString();
    }
  }
}
