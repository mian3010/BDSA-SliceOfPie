using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace SliceOfPie_Connection
{
    class NetworkCommunicator
    {
        public void Send(string msg)
        { 
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("localhost");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "Accept=text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";

            byte[] _byteVersion = Encoding.ASCII.GetBytes(string.Concat("content=", msg));

            request.ContentLength = _byteVersion.Length;

            Stream stream = request.GetRequestStream();
            stream.Write(_byteVersion, 0, _byteVersion.Length);
            stream.Close();
        }

        public static void Main(String[] args)
        {
            NetworkCommunicator nc = new NetworkCommunicator();
            nc.Send("tokeloke");
        }
    }
}
