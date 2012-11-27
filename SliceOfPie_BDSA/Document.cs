using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceOfPie_OnlineGUI
{
    /// <summary>
    /// Document class. Emulates a simple document.
    /// Author morr&msta.
    /// </summary>
    public class Document : File
    {
        public String Title { get; set; }
        public StringBuilder Content { get; set; }
        public IList<String> Authors { get; set; }

        public override string ToString() {
          StringBuilder output = new StringBuilder();
          output.Append("<div class=\"document\">");
          output.Append("<h2 class=\"document-name\">" + Title + "</h2>");
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

        static internal Document createTestDocument(String s)
        {
            Document d = new Document();
            d.Content = new StringBuilder(s);
            return d;
        }

    }
}
