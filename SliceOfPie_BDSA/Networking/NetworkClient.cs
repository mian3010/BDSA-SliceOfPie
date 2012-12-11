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
        private bool is_active;
        private const int port = 8080;


        public FileList SyncServer(FileList list)
        {
            return SendFileList(list);
        }

        public File PullFile(long id)
        {
            return GetFile(id);
        }

        public long PushFile(File file)
        {
            return SendFile(file);
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

        private SliceOfPie_Model.Persistence.File GetFile(long id)
        {
            string xml = HTMLMarshalUtil.MarshallId(id);
            HttpWebResponse response = Send(xml, "POST");
            return HandleFileResponse(response);
        }

        /// <summary>
        /// Sends HTML using a HTTP protocol.
        /// </summary>
        /// <param name="msg"></param>
        private FileList SendFileList(FileList log)
        {
            string xml = HTMLMarshalUtil.MarshallFileList(log);
            HttpWebResponse response = Send(xml, "POST");
            return HandleFileListResponse(response);
        }

        private long SendFile(SliceOfPie_Model.Persistence.File file)
        {
            string xml = HTMLMarshalUtil.MarshallFile(file);
            HttpWebResponse response = Send(xml, "PUT");
            return HandleIdResponse(response);
        }

        /// <summary>
        /// Handles the resultcomming from the server.
        /// </summary>
        /// <param name="response">The response from the server</param>
        private FileList HandleFileListResponse(HttpWebResponse response)
        {
            Console.Out.WriteLine(response.Server);
            System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream());
            Console.Out.WriteLine(reader.ReadToEnd());
            return null;
        }

        /// <summary>
        /// Handles the result comming from the server
        /// </summary>
        /// <param name="response">The response from the server</param>
        private SliceOfPie_Model.Persistence.File HandleFileResponse(HttpWebResponse response)
        {
            Console.Out.WriteLine(response.Server);
            System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream());
            Console.Out.WriteLine(reader.ReadToEnd());
            return null;
        }

        private long HandleIdResponse(HttpWebResponse response)
        {
            return 0;
        }


        public static void Main(String[] args)
        {
            //NetworkServer server = NetworkServer.GetInstance();
           // NetworkClient client = new NetworkClient();
            // Testdata
           // Thread thread = new Thread(() => server.listen());
           // thread.Start();
            //client.SendLog(list);
           // server.Close();
        }
    }
}
