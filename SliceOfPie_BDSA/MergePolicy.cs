using SliceOfPie_Model.Persistence;

namespace SliceOfPie_Model {
  public abstract class MergePolicy {
    private static readonly MergePolicy Policy = new SimpleMergePolicy();
    public static Document Merge(Document original, Document latest) {
      return Policy.MergeDocuments(original, latest);
    }

    protected abstract Document MergeDocuments(Document original, Document latest);
  }
}
