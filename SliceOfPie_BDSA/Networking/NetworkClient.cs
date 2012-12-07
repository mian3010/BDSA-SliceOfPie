using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Diagnostics;
using SliceOfPie_Model;
using SliceOfPie_Model;
using System.Threading;

namespace SliceOfPie_Model
{
    public class NetworkClient : INetClient
    {
        private bool is_active;
        private int port = 8080;


        public FileList SyncServer(FileList list)
        {
            return SendFileList(list);
        }

        public File PullFile(long id)
        {
            return null;
        }

        public long PushFile(File file)
        {
            return SendFile(file);
        }

        /// <summary>
        /// Sends HTML using a HTTP protocol.
        /// </summary>
        /// <param name="msg"></param>
        private FileList SendFileList(FileList log)
        {

            string xml = HTMLMarshalUtil.MarshallFileList(log);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:8080/");
            request.Accept = "text/xml,text/html";
            request.Method = "POST";
            // Creates a byteversion of the XML string
            byte[] byteVersion = Encoding.ASCII.GetBytes(xml);

            request.ContentLength = xml.Length;

            Stream stream = request.GetRequestStream();
            stream.Write(byteVersion, 0, byteVersion.Length);
            stream.Close();
            // Waist for the HTTP response from the server
            HttpWebResponse resp = (HttpWebResponse)request.GetResponse();
            return HandleFileListResponse(resp);
        }

        private long SendFile(SliceOfPie_Model.File file)
        {
            string xml = HTMLMarshalUtil.MarshallFile(file);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:8080/");
            request.Accept = "text/xml,text/html";
            request.Method = "POST";
            // Creates a byteversion of the XML string
            byte[] byteVersion = Encoding.ASCII.GetBytes(xml);

            request.ContentLength = xml.Length;

            Stream stream = request.GetRequestStream();
            stream.Write(byteVersion, 0, byteVersion.Length);
            stream.Close();
            // Waist for the HTTP response from the server
            HttpWebResponse resp = (HttpWebResponse)request.GetResponse();
            return HandleFileResponse(resp);
        }

        /// <summary>
        /// Handles the resultcomming from the server.
        /// </summary>
        /// <param name="response">The response from the server</param>
        private FileList HandleFileListResponse(HttpWebResponse response)
        {
            Console.Out.WriteLine(response.Server);
            StreamReader reader = new StreamReader(response.GetResponseStream());
            Console.Out.WriteLine(reader.ReadToEnd());
            return null;
        }

        /// <summary>
        /// Handles the result comming from the server
        /// </summary>
        /// <param name="response">The response from the server</param>
        private long HandleFileResponse(HttpWebResponse response)
        {
            Console.Out.WriteLine(response.Server);
            StreamReader reader = new StreamReader(response.GetResponseStream());
            Console.Out.WriteLine(reader.ReadToEnd());
            return 0;
        }


        public static void Main(String[] args)
        {
            NetworkServer server = NetworkServer.GetInstance();
            NetworkClient client = new NetworkClient();
            // Testdata
            Thread thread = new Thread(() => server.listen());
            thread.Start();
            //client.SendLog(list);
            server.Close();
        }
    }
}
