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

    private StringBuilder _privContent;
    public StringBuilder Content {
      get {
        if (_privContent == null) {
          _privContent = new StringBuilder();
        }
        return _privContent;
      }
      set {
        _privContent = value;
      }
    }

    public virtual String GetContent() {
      return "If this message is shown, the object displaying it is a File base class and have no content"
             + "to display";
    }


    public override string ToString() {
        return Content.ToString();
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
