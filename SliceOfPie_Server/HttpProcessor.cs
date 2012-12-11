using System;
using System.Globalization;
using System.Text;
using System.Net;
using System.IO;
using SliceOfPie_Model;
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
      try {
        if (httpMethod == "PUT") {
          var file = (SliceOfPie_Model.Persistence.FileInstance)formatter.Deserialize(inputStream);
          _handler.ReceiveFile(file, this);
        } else if (httpMethod == "POST") {
          var fileList = (FileList)formatter.Deserialize(inputStream);
          _handler.ReceiveFileList(fileList, this);
        } else if (httpMethod == "GET") {
          var idStream = new MemoryStream();
          inputStream.CopyTo(idStream);
          byte[] idByte = idStream.ToArray();
          long id = BitConverter.ToInt64(idByte, 0);
          _handler.GetFile(id, this);
        } else {
          throw new ArgumentException("Illegal XML method");
        }
      } catch (Exception e) {
        Exception ex = new ArgumentException("Error in Process()", e);
        throw ex;
      }
    }

    /// <summary>
    /// Responsible for sending the FileList back to the client
    /// </summary>
    /// <param name="list">FileList</param>
    public void RecieveFileList(FileList list) {
      string responseString = HtmlMarshalUtil.MarshallFileList(list);
      var content = new StreamReader(_request.InputStream);
      Console.Out.WriteLine(content.ReadToEnd());
      _response.ContentLength64 = responseString.Length;
      byte[] byteVersion = Encoding.ASCII.GetBytes(responseString);
      Stream stream = _response.OutputStream;
      stream.Write(byteVersion, 0, byteVersion.Length);
      stream.Close();
    }

    /// <summary>
    /// Responsible for sending the Files back to the client
    /// </summary>
    /// <param name="file">File</param>
    public void RecieveFile(SliceOfPie_Model.Persistence.FileInstance file) {
      string responseString = HtmlMarshalUtil.MarshallFile(file);
      var content = new StreamReader(_request.InputStream);
      Console.Out.WriteLine(content.ReadToEnd());
      _response.ContentLength64 = responseString.Length;
      byte[] byteVersion = Encoding.ASCII.GetBytes(responseString);
      Stream stream = _response.OutputStream;
      stream.Write(byteVersion, 0, byteVersion.Length);
      stream.Close();
    }


    /// <summary>
    /// Recieves a id on the file that has been pulled to the server.
    /// </summary>
    /// <param name="id"></param>
    public void RecieveConfirmation(long id) {
      string responseString = id.ToString(CultureInfo.InvariantCulture);
      var content = new StreamReader(_request.InputStream);
      Console.Out.WriteLine(content.ReadToEnd());
      _response.ContentLength64 = responseString.Length;
      byte[] byteVersion = Encoding.ASCII.GetBytes(responseString);
      Stream stream = _response.OutputStream;
      stream.Write(byteVersion, 0, byteVersion.Length);
      stream.Close();
    }
  }
}
