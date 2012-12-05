using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SliceOfPie_Model {
  partial class File {

      public virtual String GetContent()
      {
          return "If this message is shown, the object displaying it is a File base class and have no content"
                 + "to display";
      }


    public override string ToString() {
      return "<p> Editor cannot open this filetype </p>";
    }
    public string HistoryToString() {
      StringBuilder output = new StringBuilder();
      output.Append("<ol>");
      output.Append("<li>File created</li>");
      output.Append("<li>File saved</li>");
      output.Append("</ol>");
      return output.ToString();
    }
  }
}
