using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Text;
using System.Web.Security;
using SliceOfPie_Model;

namespace SliceOfPie_OnlineGUI.Models {
  public class FileModel {
    private IDictionary<int, File> FileList;
    public static Document getFile(int id) {
      Document d = new Document();

      MetaDataType MetaDataType1 = new MetaDataType();
      MetaDataType1.Type = "Created date";
      FileMetaData FileMetaData1 = new FileMetaData();
      FileMetaData1.MetaDataType = MetaDataType1;
      FileMetaData1.value = "2012-11-27 10:23:11";

      MetaDataType MetaDataType2 = new MetaDataType();
      MetaDataType2.Type = "Owner";
      FileMetaData FileMetaData2 = new FileMetaData();
      FileMetaData2.MetaDataType = MetaDataType2;
      FileMetaData2.value = "Michael Søby Andersen";

      MetaDataType MetaDataType3 = new MetaDataType();
      MetaDataType3.Type = "Type";
      FileMetaData FileMetaData3 = new FileMetaData();
      FileMetaData3.MetaDataType = MetaDataType3;
      FileMetaData3.value = "Document";

      d.FileMetaDatas.Add(FileMetaData1);
      d.FileMetaDatas.Add(FileMetaData2);
      d.FileMetaDatas.Add(FileMetaData3);
      d.Content = new StringBuilder("Awesome text document here!<br /><strong>This should be bold</strong><br />OMG PIE:<br /><img src=\"http://www.seriouseats.com/images/potd_pi-pie.jpg\" />");
      d.Content.Append("<br /><br />Testing wrappingggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggg");
      d.Title = "The awesome title";
      return d;
    }
  }
}
