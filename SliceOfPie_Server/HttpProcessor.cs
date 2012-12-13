using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.IO;
using System.Web;
using SliceOfPie_Model;
using SliceOfPie_Model.Persistence;
using System.Runtime.Serialization.Formatters.Binary;

namespace SliceOfPie_Server {
  /// <summary>
  /// Class responsible for processing the HTTP requests from the Network Server.
  /// Is created in a new thread for every new client.
  /// </summary>
  public class HttpProcessor {
    private readonly HttpListenerRequest _request;
    private readonly HttpListenerResponse _response;
    private readonly RequestHandler _handler;

    public HttpProcessor(HttpListenerContext context, RequestHandler handler) {
      _request = context.Request;
      _response = context.Response;
      _handler = handler;
    }

    /// <summary>
    /// Processes the HTTP request. Determines the method and sends the request to the 
    /// right place.
    /// </summary>
    public void Process() {
      Console.Out.WriteLine("starting to process");
      string httpMethod = _request.HttpMethod;
      Stream inputStream = _request.InputStream;

      // Determines which http-method is called.
      var formatter = new BinaryFormatter();
      //try {
        if (httpMethod == "PUT") {
          var file = (SliceOfPie_Model.Persistence.FileInstance)formatter.Deserialize(inputStream);
          _handler.ReceiveFile(file, this);
        } else if (httpMethod == "POST") {
          var fileList = (FileList)formatter.Deserialize(inputStream);
          _handler.ReceiveFileList(fileList, this);
        } else if (httpMethod == "GET")
        {
          NameValueCollection queryDict = HttpUtility.ParseQueryString(_request.Url.Query);
          var id = int.Parse(queryDict.Get("id"));
          _handler.GetFile(id, this);
        } else {
          throw new ArgumentException("Illegal XML method");
        }
      /*} catch (Exception e) {
        Exception ex = new ArgumentException("Error in Process()", e);
        throw ex;
      }*/
    }

    /// <summary>
    /// Responsible for sending the FileList back to the client
    /// </summary>
    /// <param name="list">FileList</param>
    public void RecieveFileList(FileList list) {
      var formatter = new BinaryFormatter();
      var fileListStream = new MemoryStream();
      formatter.Serialize(fileListStream, list);
      Stream stream = _response.OutputStream;
      byte[] data = fileListStream.ToArray();
      stream.Write(data, 0, data.Length);
      stream.Close();
    }

    /// <summary>
    /// Responsible for sending the Files back to the client
    /// </summary>
    /// <param name="file">File</param>
    public void RecieveFile(SliceOfPie_Model.Persistence.FileInstance file) {
      var formatter = new BinaryFormatter();
      var fileListStream = new MemoryStream();
      formatter.Serialize(fileListStream, file);
      Stream stream = _response.OutputStream;
      byte[] data = fileListStream.ToArray();
      stream.Write(data, 0, data.Length);
      stream.Close();
    }


    /// <summary>
    /// Recieves a id on the file that has been pulled to the server.
    /// </summary>
    /// <param name="id"></param>
    public void RecieveConfirmation(FileInstance file) {
      var formatter = new BinaryFormatter();
      var fileListStream = new MemoryStream();
      formatter.Serialize(fileListStream, file);
      Stream stream = _response.OutputStream;
      byte[] data = fileListStream.ToArray();
      stream.Write(data, 0, data.Length);
      stream.Close();
    }
  }
}
