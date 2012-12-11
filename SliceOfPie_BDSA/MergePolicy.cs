using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SliceOfPie_Model.Persistence;

namespace SliceOfPie_Model {
  public abstract class MergePolicy {
    private static MergePolicy Policy = new SimpleMergePolicy();
    public static Document Merge(Document original, Document latest) {
      return Policy.MergeDocuments(original, latest);
    }
    internal abstract Document MergeDocuments(Document original, Document latest);
  }
}
