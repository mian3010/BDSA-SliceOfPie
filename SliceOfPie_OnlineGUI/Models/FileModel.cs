using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Text;
using System.Web.Security;
using SliceOfPie_Model.Persistence;

namespace SliceOfPie_OnlineGUI.Models {
  public class FileModel {
    private IDictionary<int, File> _fileList;
    public static Document GetFile(int id) {
      Document d = new Document();

      MetaDataType metaDataType1 = new MetaDataType();
      metaDataType1.Type = "Created date";
      FileMetaData fileMetaData1 = new FileMetaData();
      fileMetaData1.MetaDataType = metaDataType1;
      fileMetaData1.value = "2012-11-27 10:23:11";

      MetaDataType metaDataType2 = new MetaDataType();
      metaDataType2.Type = "Owner";
      FileMetaData fileMetaData2 = new FileMetaData();
      fileMetaData2.MetaDataType = metaDataType2;
      fileMetaData2.value = "Michael Søby Andersen";

      MetaDataType metaDataType3 = new MetaDataType();
      metaDataType3.Type = "Type";
      FileMetaData fileMetaData3 = new FileMetaData();
      fileMetaData3.MetaDataType = metaDataType3;
      fileMetaData3.value = "Document";

      d.FileMetaDatas.Add(fileMetaData1);
      d.FileMetaDatas.Add(fileMetaData2);
      d.FileMetaDatas.Add(fileMetaData3);
      d.Content.Clear();
      d.Content.Append("Awesome text document here!<br /><strong>This should be bold</strong><br />OMG PIE:<br /><img src=\"http://www.seriouseats.com/images/potd_pi-pie.jpg\" />");
      d.Content.Append("<br /><br />Testing wrappingggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggg");
      d.Title = "The awesome title";
      return d;
    }
  }
}
