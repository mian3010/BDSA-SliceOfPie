namespace SliceOfPie_Model {
  /// <summary>
  /// Exception cast in administrator, if an persistence exception has occured.
  /// Author: mian3010 - msoa@itu.dk
  /// </summary>
  public class FilePersistenceException : System.ApplicationException {
    public FilePersistenceException() { }
    public FilePersistenceException(string message) { }
    public FilePersistenceException(string message, System.Exception inner) { }

    // Constructor needed for serialization 
    // when exception propagates from a remoting server to the client.
    protected FilePersistenceException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) { }
  }
}
