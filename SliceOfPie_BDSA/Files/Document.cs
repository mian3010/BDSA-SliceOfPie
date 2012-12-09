using System;
using System.Collections.Generic;
using System.Text;

namespace SliceOfPie_Model.Persistence {
  /// <summary>
  /// Document class. Emulates a PARTIAL html document. 
  /// Needs enclosing HTML tags when saved and displayed in system.
  /// Author morr&msta.
  /// </summary>
  public class Document : File {
    public String Title { get; set; }
  
    public IList<String> Authors { get; set; }


        public Document()
        {
        }

        public override string GetContent()
        {
            return Content.ToString();
        }

        public override string ToString() {
          StringBuilder output = new StringBuilder();
          output.Append("<div class=\"document\">");
          output.Append("<h2 class=\"document-title\">" + Title + "</h2>");
          output.Append("<div class=\"document-view\">");
          output.Append("<ul class=\"metadata-view\">");
          foreach (FileMetaData MetaData in FileMetaDatas) {
            output.Append("<li>"+MetaData.MetaDataType+": "+MetaData+"</li>");
          }
          output.Append("</ul>");
          output.Append(Content);
          output.Append("</div>");
          return output.ToString();
        }

    public new string HistoryToString() {
      StringBuilder output = new StringBuilder();
      output.Append("<ol>");
      output.Append("<li>Document created</li>");
      output.Append("<li>Document saved</li>");
      output.Append("</ol>");
      return output.ToString();
    }

    static internal Document createTestDocument(String s) {
      Document d = new Document();
      d.Content.Append(s);
      return d;
    }

  }
}
