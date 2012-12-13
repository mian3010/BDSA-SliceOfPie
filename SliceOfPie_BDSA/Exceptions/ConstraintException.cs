namespace SliceOfPie_Model {
  /// <summary>
  /// Exception cast in Contex, if an illegal parameters have been given
  /// Author: Claus35-DK - clih@itu.dk
  /// </summary>
  public class ConstraintException : System.ApplicationException {
    public ConstraintException() { }
    public ConstraintException(string message) { }
    public ConstraintException(string message, System.Exception inner) { }

    // Constructor needed for serialization 
    // when exception propagates from a remoting server to the client.
    protected ConstraintException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) { }
  }
}
