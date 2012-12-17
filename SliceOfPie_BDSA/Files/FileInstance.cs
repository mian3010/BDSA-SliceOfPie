using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceOfPie_Model.Persistence {

  [Serializable()]
  public partial class FileInstance {

    public byte[] Content {
      get { return File.Content; }
      set { File.Content = value; }
    }

    public virtual String GetContent() {
      return "If this message is shown, the object displaying it is a File base class and have no content"
             + "to display";
    }


    public override string ToString() {
      return "<p>File with id " + id + " is not a document. It cannot be opened in this program</p>";
    }
    public string ChangesToString() {
      var output = new StringBuilder();
      output.Append("<ol>");
      foreach (var change in File.Changes) {
        output.Append("<li>User : " + change.User_email + " " + change.change1 + " @ the time : " +
                      new DateTime((long)change.timestamp) + "</li>");
      }
      output.Append("</ol>");
      return output.ToString();
    }
    internal FileMetaData GetMetadata(string metaDataType) {
      var query = from meta in File.FileMetaDatas
                  where meta.MetaDataType_Type != null && meta.MetaDataType_Type.Equals(metaDataType)
                  select meta;
      var fileMetaDatas = query as IList<FileMetaData> ?? query.ToList();
      if (!fileMetaDatas.Any()) { return CreateMetadata("Title", ""); }
      return fileMetaDatas.First();
    }

    internal FileMetaData CreateMetadata(string strType, string value) {
      var meta = new FileMetaData { MetaDataType_Type = strType, value = value };
      File.FileMetaDatas.Add(meta);
      return meta;
    }
  }
}
