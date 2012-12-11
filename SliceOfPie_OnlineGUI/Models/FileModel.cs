using System.Collections.Generic;
using SliceOfPie_Model.Persistence;

namespace SliceOfPie_OnlineGUI.Models {
  public class FileModel {
    private IDictionary<int, File> _fileList;
    public static Document GetFile(int id) {
      var d = new Document();

      var metaDataType1 = new MetaDataType {Type = "Created date"};
      var fileMetaData1 = new FileMetaData {MetaDataType = metaDataType1, value = "2012-11-27 10:23:11"};

      var metaDataType2 = new MetaDataType {Type = "Owner"};
      var fileMetaData2 = new FileMetaData {MetaDataType = metaDataType2, value = "Michael Søby Andersen"};

      var metaDataType3 = new MetaDataType {Type = "Type"};
      var fileMetaData3 = new FileMetaData {MetaDataType = metaDataType3, value = "Document"};

      d.File.FileMetaDatas.Add(fileMetaData1);
      d.File.FileMetaDatas.Add(fileMetaData2);
      d.File.FileMetaDatas.Add(fileMetaData3);
      d.Content.Clear();
      d.Content.Append("Awesome text document here!<br /><strong>This should be bold</strong><br />OMG PIE:<br /><img src=\"http://www.seriouseats.com/images/potd_pi-pie.jpg\" />");
      d.Content.Append("<br /><br />Testing wrappingggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggg");
      d.Title = "The awesome title";
      return d;
    }
  }
}
