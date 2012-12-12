using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceOfPie_Model.CompositeStructure {
  public class File : CompositeStructure {
    public new string ToString() {
      return Label;
    }
  }
}
