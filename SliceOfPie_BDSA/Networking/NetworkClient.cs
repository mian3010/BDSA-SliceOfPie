using System.Globalization;
using System.Text;
using System.Net;
using SliceOfPie_Model.Persistence;
using System;
using System.Runtime.Serialization.Formatters.Binary;

namespace SliceOfPie_Model {
  public class NetworkClient : INetClient {
    private const int Port = 8080;

    /// <summary>
    /// Sends the FileList to the Server and initializes the synchronization process
    /// </summary>
    /// <param name="list">FileList</param>
    /// <returns>FileList</returns>
    public FileList SyncServer(FileList list) {
      var formatter = new BinaryFormatter();
      var fileListStream = new System.IO.MemoryStream();
      formatter.Serialize(fileListStream, list);
      System.IO.Stream response = Send(fileListStream.ToArray(), "POST");
      return HandleFileListResponse(response);
    }

    /// <summary>
    /// Gets a File from the server
    /// </summary>
    /// <param name="fileId">ID</param>
    /// <returns>File</returns>
    public FileInstance PullFile(long fileId) {
      var fileIdByte = Encoding.ASCII.GetBytes(fileId.ToString(CultureInfo.InvariantCulture));
      var responseFromServer = Send(fileIdByte, "GET");

      //Read the file instance object from response
      return HandleFileResponse(responseFromServer);
    }

    /// <summary>
    /// Pushes a File from the Client to the Server
    /// </summary>
    /// <param name="file">File</param>
    /// <returns>ID</returns>
    public long PushFile(FileInstance file) {
      var formatter = new BinaryFormatter();
      var fileObjectStream = new System.IO.MemoryStream();
      formatter.Serialize(fileObjectStream, file);
      //Write file object after this
      var fileObject = new byte[fileObjectStream.Length];
      fileObjectStream.Read(fileObject, 0, fileObject.Length);

      //Send byte array to server
      var responseFromRequest = Send(fileObject, "POST");
      return HandleIdResponse(responseFromRequest);
    }

    /// <summary>
    /// Send the XML to the server using a HTTP request.
    /// </summary>
    /// <param name="data">The data to send</param>
    /// <param name="method">The REST method</param>
    /// <returns>A response from the server to be returned</returns>
    private System.IO.Stream Send(byte[] data, string method) {
      var request = WebRequest.Create("http://localhost:" + Port);
      request.Method = method;
      var requestStream = request.GetRequestStream();
      requestStream.Write(data, 0, data.Length);
      requestStream.Close();
      return request.GetResponse().GetResponseStream();
    }

    /// <summary>
    /// Handles the resultcomming from the server.
    /// </summary>
    /// <param name="response">The response from the server</param>
    private static FileList HandleFileListResponse(System.IO.Stream response) {
      if (response != null) {
        var formatter = new BinaryFormatter();
        return (FileList)formatter.Deserialize(response);
      }
      return null;
    }

    /// <summary>
    /// Handles the result comming from the server
    /// </summary>
    /// <param name="response">The response from the server</param>
    private static FileInstance HandleFileResponse(System.IO.Stream response) {
      if (response != null) {
        var formatter = new BinaryFormatter();
        return (FileInstance)formatter.Deserialize(response);
      }
      return null;
    }

    /// <summary>
    /// Handles the ID comming from the Server
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    private static long HandleIdResponse(System.IO.Stream response) {
      if (response != null) {
        var streamToLong = new System.IO.MemoryStream();
        response.CopyTo(streamToLong);
        return BitConverter.ToInt64(streamToLong.ToArray(), 0);
      }
      return 0;
    }
  }
}
