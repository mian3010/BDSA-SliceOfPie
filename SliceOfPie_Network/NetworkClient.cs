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
        public void SendLog(FileList log)
        {
<<<<<<< HEAD
            string xml = HTMLMarshalUtil.MarshallFileList(log);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost");
            //request.Credentials = new NetworkCredential("test", "test");
=======
            string xml = HTMLMarshaller.MarshallLog(log);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:8080/");
>>>>>>> c1047c883d7ca24cf6488cff4fda7cab91a1bf69
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
<<<<<<< HEAD
            string xml = HTMLMarshalUtil.MarshallFile(file);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.itu.dk/people/dpacino/test/");
            request.Credentials = new NetworkCredential("test", "test");
=======
            string xml = HTMLMarshaller.MarshallFile(file);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:8080/");
>>>>>>> c1047c883d7ca24cf6488cff4fda7cab91a1bf69
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
     
            Thread thread = new Thread(() => server.listen());
<<<<<<< HEAD
            // client.SendLog(loglist);
=======
            thread.Start();
            client.SendLog(loglist);
>>>>>>> c1047c883d7ca24cf6488cff4fda7cab91a1bf69
            server.Close();
        }
    }
}
