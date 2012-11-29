using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceOfPie_Model {
  partial class File {
    public override string ToString() {
      return "<p>Editor cannot open this filetype</p>";
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
