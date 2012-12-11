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
        private int port = 8080;

        /// <summary>
        /// Sends the FileList to the Server and initializes the synchronization process
        /// </summary>
        /// <param name="list">FileList</param>
        /// <returns>FileList</returns>
        public FileList SyncServer(FileList list)
        {
            string xml = HTMLMarshalUtil.MarshallFileList(list);
            HttpWebResponse response = Send(xml, "POST");
            return HandleFileListResponse(response);
        }

        /// <summary>
        /// Gets a File from the server
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>File</returns>
        public File PullFile(long id)
        {
            string xml = HTMLMarshalUtil.MarshallId(id);
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
            string xml = HTMLMarshalUtil.MarshallFile(file);
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
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:"+ port + "/");
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
            System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream());
            string xml = reader.ReadToEnd();
            FileList list = HTMLMarshalUtil.UnMarshallFileList(xml);
            return list;
        }

        /// <summary>
        /// Handles the result comming from the server
        /// </summary>
        /// <param name="response">The response from the server</param>
        private SliceOfPie_Model.Persistence.File HandleFileResponse(HttpWebResponse response)
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream());
            string xml = reader.ReadToEnd();
            File file = HTMLMarshalUtil.UnmarshallFile(xml);
            return file;
        }

        /// <summary>
        /// Handles the ID comming from the Server
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private long HandleIdResponse(HttpWebResponse response)
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream());
            string xml = reader.ReadToEnd();
            long id = HTMLMarshalUtil.UnMarshallId(xml);
            return id;
        }


        public static void Main(String[] args)
        {
           
        }
    }
}
