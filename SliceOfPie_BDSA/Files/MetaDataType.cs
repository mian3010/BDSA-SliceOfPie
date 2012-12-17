using System;

namespace SliceOfPie_Model.Persistence {
  [Serializable()]
  public partial class MetaDataType {
    public override string ToString() {
      return Type;
    }
  }
}
