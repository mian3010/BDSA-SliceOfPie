namespace SliceOfPie_Model {
  /// <summary>
  /// Exception cast in administrator, if an persistence exception has occured.
  /// Author: mian3010 - msoa@itu.dk
  /// </summary>
  public class MergeImpossibleException : System.Exception {
    public MergeImpossibleException() { }
    public MergeImpossibleException(string message) { }
    public MergeImpossibleException(string message, System.Exception inner) { }

    // Constructor needed for serialization 
    // when exception propagates from a remoting server to the client.
    protected MergeImpossibleException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) { }
  }
}
