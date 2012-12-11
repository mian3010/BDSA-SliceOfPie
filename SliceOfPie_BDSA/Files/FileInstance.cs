﻿using System;
using System.Text;

namespace SliceOfPie_Model.Persistence {
  
  public partial class FileInstance {

    public String UserEmail {
      get;
      set;
    }

    internal byte[] PrivContent;
    public byte[] Content {
      get { return PrivContent; }
      set { PrivContent = value; }
    }

    public virtual String GetContent() {
      return "If this message is shown, the object displaying it is a File base class and have no content"
             + "to display";
    }


    public override string ToString() {
      return Content.ToString();
    }
    public string HistoryToString() {
      var output = new StringBuilder();
      output.Append("<ol>");
      output.Append("<li>File created</li>");
      output.Append("<li>File saved</li>");
      output.Append("</ol>");
      return output.ToString();
    }
  }
}