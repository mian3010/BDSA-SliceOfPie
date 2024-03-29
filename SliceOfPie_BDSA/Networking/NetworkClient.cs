﻿using System.Globalization;
using System.Net.Sockets;
using System.Text;
using System.Net;
using SliceOfPie_Model.Persistence;
using System;
using System.Runtime.Serialization.Formatters.Binary;

namespace SliceOfPie_Model {
  public class NetworkClient : INetClient
  {
      private const int Port = 80;
      private const String TestIP = "http://10.25.243.118:";

    public NetworkClient() {

    }

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
    public FileInstance PullFile(int fileId) {
      var fileIdByte = Encoding.UTF8.GetBytes("id=" + fileId.ToString(CultureInfo.InvariantCulture));
      var responseFromServer = Send(fileIdByte, "GET");
      //Read the file instance object from response
      return HandleFileResponse(responseFromServer);
    }

    /// <summary>
    /// Pushes a File from the Client to the Server
    /// </summary>
    /// <param name="file">File</param>
    /// <returns>ID</returns>
    public FileInstance PushFile(FileInstance file) {
      var formatter = new BinaryFormatter();
      var fileObjectStream = new System.IO.MemoryStream();
      formatter.Serialize(fileObjectStream, file);
      //Write file object after this
      var fileObject = fileObjectStream.ToArray();

      //Send byte array to server
      var responseFromRequest = Send(fileObject, "PUT");
      return HandleFileResponse(responseFromRequest);
    }

    /// <summary>
    /// Send the XML to the server using a HTTP request.
    /// </summary>
    /// <param name="data">The data to send</param>
    /// <param name="method">The REST method</param>
    /// <returns>A response from the server to be returned</returns>
    private static System.IO.Stream Send(byte[] data, string method) {

      HttpWebRequest request;
      if (method != "GET") {
        int length = data.Length;
        request = (HttpWebRequest)WebRequest.Create(TestIP + Port + "/");
     
        request.Method = method;
        request.ContentType = "application/octet-stream";
        request.ContentLength = length;
        var requestStream = request.GetRequestStream();
        requestStream.Write(data, 0, length);
        requestStream.Flush();
        requestStream.Close();
      }
      else {
        request = (HttpWebRequest)WebRequest.Create(TestIP + Port + "/?" + Encoding.UTF8.GetString(data));
        request.Method = method;
      }
      return request.GetResponse().GetResponseStream();
    }

    /// <summary>
    /// Handles the resultcomming from the server.
    /// </summary>
    /// <param name="response">The response from the server</param>
    private static FileList HandleFileListResponse(System.IO.Stream response) {
      if (response != null) {
        var formatter = new BinaryFormatter();
        var fileList = (FileList)formatter.Deserialize(response);
        response.Dispose();
        response.Close();
        return fileList;
      }
      return null;
    }

    /// <summary>
    /// Handles the result comming from the server
    /// </summary>
    /// <param name="response">The response from the server</param>
    private static FileInstance HandleFileResponse(System.IO.Stream response) {
      try {
        if (response != null) {
          var formatter = new BinaryFormatter();
          FileInstance file = (FileInstance)formatter.Deserialize(response);
          response.Dispose();
          response.Close();
          return file;
        }
        return null;
      }
      catch (InvalidCastException) {
        return null;
      }
    }

    /// <summary>
    /// Handles the ID comming from the Server
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    private static int HandleIdResponse(System.IO.Stream response) {
      if (response != null) {
        var formatter = new BinaryFormatter();
        return (int)formatter.Deserialize(response);
      }
      return 0;
    }
  }
}
