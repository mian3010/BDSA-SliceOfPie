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

namespace SliceOfPie_Network
{
    public class NetworkClient
    {
        private bool is_active;
        private int port = 8080;

        /// <summary>
        /// Sends HTML using a HTTP protocol.
        /// </summary>
        /// <param name="msg"></param>
        public void SendLog(List<LogEntry> log)
        {
            string xml = HTMLMarshaller.MarshallLog(log);
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
            string xml = HTMLMarshaller.MarshallFile(file);
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
            List<LogEntry> loglist = new List<LogEntry>();
            LogEntry log1 = new LogEntry(1, "file1", "/local", DateTime.Now, FileModification.Add);
            LogEntry log2 = new LogEntry(2, "file2", "/local", DateTime.Now, FileModification.Delete);
            LogEntry log3 = new LogEntry(3, "file3", "/local", DateTime.Now, FileModification.Modify);
            LogEntry log4 = new LogEntry(4, "file4", "/local", DateTime.Now, FileModification.Add);
            Console.Out.WriteLine("LOG TIME BEFORE MARSHALL" + log1.timeStamp);
            loglist.Add(log1);
            loglist.Add(log2);
            loglist.Add(log3);
            loglist.Add(log4);
            Thread thread = new Thread(() => server.listen());
            client.SendLog(loglist);
            server.Close();
        }
    }
}
