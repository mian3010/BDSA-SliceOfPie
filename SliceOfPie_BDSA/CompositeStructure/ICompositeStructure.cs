using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceOfPie_Model.CompositeStructure {
  public abstract class CompositeStructure {
    public string Label { get; set; }
    public override string ToString() {
      try {
        return ((File)this).ToString();
      } catch (InvalidCastException) {
        return ((Folder)this).ToString();
      }
    }
  }
}
