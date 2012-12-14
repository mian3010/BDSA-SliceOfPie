using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceOfPie_Model.CompositeStructure {
  public class File : CompositeStructure {
    public string viewLink { get; set; }
    public string editLink { get; set; }
    public new string ToString() {
      return "<div class=\"FileActions\">"+viewLink+editLink+"</div><span class=\"file\">"+Label+"</span>";
    }
  }
}
