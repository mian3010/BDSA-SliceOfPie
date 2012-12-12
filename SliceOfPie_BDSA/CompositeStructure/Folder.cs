using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceOfPie_Model.CompositeStructure {
  public class Folder : CompositeStructure {
    public IList<CompositeStructure> Children = new List<CompositeStructure>();
    public new string ToString() {
      var output = new StringBuilder();
      output.Append("<span>" + Label + "</span>" +
                    "<ul>");

      foreach (var child in Children) {
        output.Append("<li>" + child.ToString() + "</li>");
      }
      output.Append("</ul>");
      return output.ToString();
    }
  }
}
