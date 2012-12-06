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
            HandleLogResponse(resp);
        }

        public void SendFile(SliceOfPie_Model.File file)
        {
            string xml = HTMLMarshaller.MarshallFile(file);
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
            HandleFileResponse(resp);
        }

        /// <summary>
        /// Handles the resultcomming from the server.
        /// </summary>
        /// <param name="response">The response from the server</param>
        private void HandleLogResponse(HttpWebResponse response)
        {
            Console.Out.WriteLine(response.Server);
            StreamReader reader = new StreamReader(response.GetResponseStream());
            Console.Out.WriteLine(reader.ReadToEnd());
        }

        /// <summary>
        /// Handles the result comming from the server
        /// </summary>
        /// <param name="response">The response from the server</param>
        private void HandleFileResponse(HttpWebResponse response)
        {
            Console.Out.WriteLine(response.Server);
            StreamReader reader = new StreamReader(response.GetResponseStream());
            Console.Out.WriteLine(reader.ReadToEnd());
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
            thread.Start();
            client.SendLog(loglist);
            server.Close();
        }
    }
}
