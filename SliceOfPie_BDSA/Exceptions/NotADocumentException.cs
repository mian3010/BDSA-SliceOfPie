using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceOfPie_Model {
  /// <summary>
  /// Exception cast in administrator, if an persistence exception has occured.
  /// Author: mian3010 - msoa@itu.dk
  /// </summary>
  public class NotADocumentException : System.Exception {
    public NotADocumentException() { }
    public NotADocumentException(string message) { }
    public NotADocumentException(string message, System.Exception inner) { }

    // Constructor needed for serialization 
    // when exception propagates from a remoting server to the client.
    protected NotADocumentException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) { }
  }
}
