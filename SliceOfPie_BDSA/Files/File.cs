using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SliceOfPie_Model.Persistence {
  public partial class File {

    public String UserEmail {
      get;
      set;
    }

    public override string ToString() {
      return "<p> System cannot open this filetype </p>";
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