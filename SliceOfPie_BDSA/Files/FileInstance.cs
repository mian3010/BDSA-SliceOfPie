using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceOfPie_Model.Persistence {

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
    public string HistoryToString() {
      var output = new StringBuilder();
      output.Append("<ol>");
      output.Append("<li>File created</li>");
      output.Append("<li>File saved</li>");
      output.Append("</ol>");
      return output.ToString();
    }
    internal FileMetaData GetMetadata(string metaDataType) {
      var query = from meta in this.File.FileMetaDatas
                  where meta.MetaDataType_Type.Equals(metaDataType)
                  select meta;
      var fileMetaDatas = query as IList<FileMetaData> ?? query.ToList();
      return !fileMetaDatas.Any() ? null : fileMetaDatas.First();
    }
  }
}
