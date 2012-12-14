using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using System.Text;

namespace SliceOfPie_Model.Persistence {
  /// <summary>
  /// Document class. Emulates a PARTIAL html document. 
  /// Needs enclosing HTML tags when saved and displayed in system.
  /// Author morr & msta.
  /// </summary>
  [Serializable()]
  public class Document : FileInstance {


    public String Title {
      get { return Context.GetFileMetaData(File, "Title").value; }
      set { Context.GetFileMetaData(File, "Title").value = value; }
    }


    public IList<User> Authors { get { return Context.GetUsers(File); } }

    public new string Content {
      get { return Encoding.UTF8.GetString(PrivContent, 0, PrivContent.Length); }
      set { PrivContent = Encoding.UTF8.GetBytes(value); }
    }

    public override string GetContent() {
      return Content;
    }

    public override string ToString() {
      var output = new StringBuilder();
      output.Append("<div class=\"document\">");
      output.Append("<h2 class=\"document-title\">" + Title + "</h2>");
      output.Append("<div class=\"document-view\">");
      output.Append("<ul class=\"metadata-view\">");
      foreach (FileMetaData metaData in File.FileMetaDatas) {
        output.Append("<li>" + metaData.MetaDataType + ": " + metaData + "</li>");
      }
      output.Append("</ul>");
      output.Append(Content);
      output.Append("</div>");
      return output.ToString();
    }

    public new string HistoryToString() {
      var output = new StringBuilder();
      output.Append("<ol>");
      output.Append("<li>Document created</li>");
      output.Append("<li>Document saved</li>");
      output.Append("</ol>");
      return output.ToString();
    }

    public static Document CreateDocument(FileInstance file) {
      var fileType = Context2.GetFileMetaData(file.File, "Type");
      if (fileType != null && file.File != null && fileType.value == "Document") {
        return file as Document;
      }
      throw new NotADocumentException();
    }

    static internal Document CreateTestDocument(String s) {
      var d = new Document { Content = s };
      return d;
    }

  }
}
