using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using SliceOfPie_Model.Persistence;
using System.Diagnostics;
using SliceOfPie_Model;
using System.Threading;

namespace SliceOfPie_Model
{
    public class NetworkClient : INetClient
    {
        private bool _isActive;
        private const int Port = 8080;

      /// <summary>
        /// Sends the FileList to the Server and initializes the synchronization process
        /// </summary>
        /// <param name="list">FileList</param>
        /// <returns>FileList</returns>
        public FileList SyncServer(FileList list)
        {
            string xml = HtmlMarshalUtil.MarshallFileList(list);
            HttpWebResponse response = Send(xml, "POST");
            return HandleFileListResponse(response);
        }

        /// <summary>
        /// Gets a File from the server
        /// </summary>
        /// <param name="fileId">ID</param>
        /// <returns>File</returns>
        public File PullFile(long fileId)
        {
            string xml = HtmlMarshalUtil.MarshallId(fileId);
            HttpWebResponse response = Send(xml, "POST");
            return HandleFileResponse(response);
        }

        /// <summary>
        /// Pushes a File from the Client to the Server
        /// </summary>
        /// <param name="file">File</param>
        /// <returns>ID</returns>
        public long PushFile(File file)
        {
            string xml = HtmlMarshalUtil.MarshallFile(file);
            HttpWebResponse response = Send(xml, "PUT");
            return HandleIdResponse(response);
        }

        /// <summary>
        /// Send the XML to the server using a HTTP request.
        /// </summary>
        /// <param name="xml">The xml to send</param>
        /// <param name="method">The REST method</param>
        /// <returns>A response from the server to be returned</returns>
        private HttpWebResponse Send(string xml, string method)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:"+ Port + "/");
            request.Accept = "text/xml,text/html";
            request.Method = method;
            // Creates a byteversion of the XML string
            byte[] byteVersion = Encoding.ASCII.GetBytes(xml);

            request.ContentLength = xml.Length;

            System.IO.Stream stream = request.GetRequestStream();
            stream.Write(byteVersion, 0, byteVersion.Length);
            stream.Close();
            // Waist for the HTTP response from the server
            return (HttpWebResponse)request.GetResponse();
        }

        /// <summary>
        /// Handles the resultcomming from the server.
        /// </summary>
        /// <param name="response">The response from the server</param>
        private FileList HandleFileListResponse(HttpWebResponse response)
        {
          if (response != null)
          {
            System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream());
            string xml = reader.ReadToEnd();
            FileList list = HtmlMarshalUtil.UnMarshallFileList(xml);
            return list;
          }
          return null;
        }

      /// <summary>
        /// Handles the result comming from the server
        /// </summary>
        /// <param name="response">The response from the server</param>
        private SliceOfPie_Model.Persistence.File HandleFileResponse(HttpWebResponse response)
      {
        if (response != null)
          {
            System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream());
            string xml = reader.ReadToEnd();
            File file = HtmlMarshalUtil.UnmarshallFile(xml);
            return file;
          }
        return null;
      }

      /// <summary>
        /// Handles the ID comming from the Server
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private long HandleIdResponse(HttpWebResponse response)
      {
        if (response != null)
          {
            System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream());
            string xml = reader.ReadToEnd();
            long id = HtmlMarshalUtil.UnMarshallId(xml);
            return id;
          }
        return 0;
      }
    }
}
