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

namespace SliceOfPie_Network
{
    public class NetworkCommunicator
    {
        private bool is_active;
        private int port = 8080;
        /// <summary>
        /// Sends HTML using a HTTP protocol.
        /// </summary>
        /// <param name="msg"></param>
        private void SendLog(List<LogEntry> log)
        {
            string xml = HTMLMarshaller.MarshallLog(log);
            HttpWebRequest request = (HttpWebRequest)WebRequest.CreateHttp("http://www.itu.dk/people/dpacino/test/");
            request.Credentials = new NetworkCredential("test", "test");
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

        private void SendFile(SliceOfPie_Model.File file)
        {
            string xml = HTMLMarshaller.MarshallFile(file);
            HttpWebRequest request = (HttpWebRequest)WebRequest.CreateHttp("http://www.itu.dk/people/dpacino/test/");
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

        public void SynchronizeLog(List<LogEntry> list)
        {
        }

        public void HandleLogResponse(HttpWebResponse response)
        { 
            
        }

        public void HandleFileResponse(HttpWebResponse response)
        { 
            
        }


        public static void Main(String[] args)
        {

        }
    }
}
