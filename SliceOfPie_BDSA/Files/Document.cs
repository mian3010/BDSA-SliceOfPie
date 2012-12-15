using System;
using System.Collections.Generic;
using System.Text;

namespace SliceOfPie_Model.Persistence {
  /// <summary>
  /// Document class. Emulates a PARTIAL html document. 
  /// Needs enclosing HTML tags when saved and displayed in system.
  /// Author morr & msta.
  /// </summary>
  public partial class Document {
    public String Title {
      get { return GetMetadata("Title").value; }
      set { GetMetadata("Title").value = value; }
    }

    public IList<User> Authors { get { return Context.GetUsers(File); } }

    public new string Content {
      get { if (File.Content == null) File.Content = new byte[0]; return File.Content.Length == 0 ? "" : Encoding.UTF8.GetString(File.Content, 0, File.Content.Length); }
      set { File.Content = Encoding.UTF8.GetBytes(value); }
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
      foreach (var metaData in File.FileMetaDatas) {
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

    static internal Document CreateTestDocument(String s) {
      var d = new Document { Content = s };
      return d;
    }
  }
}
