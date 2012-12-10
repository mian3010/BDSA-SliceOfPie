﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using SliceOfPie_Model;

namespace SliceOfPie_Server {
  /// <summary>
  /// Class responsible for processing the HTTP requests from the Network Server.
  /// Is created in a new thread for every new client.
  /// </summary>
  public class HttpProcessor {
    private HttpListenerRequest request;
    private HttpListenerResponse response;
    private RequestHandler handler;

    public HttpProcessor(HttpListenerContext context, RequestHandler handler) {
      request = context.Request;
      response = context.Response;
      this.handler = handler;
    }

    /// <summary>
    /// Processes the HTTP request. Determines the method and sends the request to the 
    /// right place.
    /// </summary>
    public void Process() {
      string http_method = request.HttpMethod;
      string responseString = "";
      // Determines which http-method is called.
      try {
        if (http_method == "PUT") {

        } else if (http_method == "POST") {
          StreamReader reader = new StreamReader(request.InputStream);
          string s = reader.ReadToEnd();
          FileList list = HtmlMarshalUtil.UnmarshallFileList(s);
          //responseString = handler.ReceiveFileList(list);
        } else if (http_method == "GET") {
          StreamReader reader = new StreamReader(request.InputStream);
          string s = reader.ReadLine();
          handler.GetFile(long.Parse(s));

        } else {
          throw new System.ArgumentException("Illegal XML method");
        }
      } catch (Exception e) {
        Exception ex = new System.ArgumentException("Illegal XML in Process()", e);
        throw ex;
      }
    }

    public void RecieveFileList(FileList list) {
      string responseString = HtmlMarshalUtil.MarshallFileList(list);
      StreamReader content = new StreamReader(request.InputStream);
      Console.Out.WriteLine(content.ReadToEnd());
      response.ContentLength64 = responseString.Length;
      byte[] byteVersion = Encoding.ASCII.GetBytes(responseString);
      Stream stream = response.OutputStream;
      stream.Write(byteVersion, 0, byteVersion.Length);
      stream.Close();
    }
  }
}