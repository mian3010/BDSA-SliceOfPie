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
using System.Threading;

namespace SliceOfPie_Model
{
    public class NetworkClient : INetClient
    {
        private bool is_active;
        private int port = 8080;

        /// <summary>
        /// Sends HTML using a HTTP protocol.
        /// </summary>
        /// <param name="msg"></param>
        public void SendLog(FileList log)
        {
            string xml = HTMLMarshalUtil.MarshallFileList(log);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost");
            //request.Credentials = new NetworkCredential("test", "test");
            request.Accept = "text/xml,text/html";
            request.Method = "POST";

            byte[] _byteVersion = Encoding.ASCII.GetBytes(string.Concat("content=", xml));

            request.ContentLength = _byteVersion.Length;

            Stream stream = request.GetRequestStream();
            stream.Write(_byteVersion, 0, _byteVersion.Length);
            Debug.WriteLine("stream has written");
            stream.Close();
            HttpWebResponse resp = (HttpWebResponse)request.GetResponse();
            HandleLogResponse(resp);
        }

        public void SendFile(SliceOfPie_Model.File file)
        {
            string xml = HTMLMarshalUtil.MarshallFile(file);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.itu.dk/people/dpacino/test/");
            request.Credentials = new NetworkCredential("test", "test");
            request.Accept = "text/xml,text/html";
            request.Method = "ADD";

            byte[] _byteVersion = Encoding.ASCII.GetBytes(string.Concat("content=", xml));

            request.ContentLength = _byteVersion.Length;

            Stream stream = request.GetRequestStream();
            stream.Write(_byteVersion, 0, _byteVersion.Length);
            Debug.WriteLine("stream has written");
            stream.Close();
            HttpWebResponse resp = (HttpWebResponse)request.GetResponse();
            HandleFileResponse(resp);
        }


        public void HandleLogResponse(HttpWebResponse response)
        { 
            
        }

        public void HandleFileResponse(HttpWebResponse response)
        { 
            
        }


        public static void Main(String[] args)
        {
            NetworkServer server = new NetworkServer(8080);
            NetworkClient client = new NetworkClient();
            // Testdata
     
            Thread thread = new Thread(() => server.listen());
            // client.SendLog(loglist);
            server.Close();
        }

        public SliceOfPie_Model.FileList SyncServer()
        {
            throw new NotImplementedException();
        }

        public long PushFile(File file)
        {
            throw new NotImplementedException();
        }


        public File PullFile(File file)
        {
            throw new NotImplementedException();
        }

        public File PullFile(long fileID)
        {
            throw new NotImplementedException();
        }
    }
}
