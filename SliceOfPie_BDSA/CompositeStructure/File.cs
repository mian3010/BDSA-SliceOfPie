using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceOfPie_Model.CompositeStructure {
  public class File : CompositeStructure {
    public string viewLink { get; set; }
    public string viewLinkEnd { get; set; }
    public string viewImage { get; set; }
    public string editLink { get; set; }
    public new string ToString() {
      return "<span class=\"file\">" + viewLink + Label + viewLinkEnd + "<div class=\"FileActions\">" + editLink + viewImage + "</div></span>";
    }
  }
}
