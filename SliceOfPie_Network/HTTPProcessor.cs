using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace SliceOfPie_Network
{
    class HTTPProcessor
    {
        private HttpListenerRequest request;
        private NetworkServer com;
        private Stream inputStream;
        private string http_method;
        private string http_url;
        private string http_protocol_versionstring;
        private List<string> httpHeaders;

        public HTTPProcessor(HttpListenerRequest request, NetworkServer com)
        {
            this.request = request;
            this.com = com;
        }

        /// <summary>
        /// Processes the HTTP request
        /// </summary>
        public void Process()
        {
            string http_method = request.HttpMethod;
            StreamReader content = new StreamReader(request.InputStream);
            
        }

        /// <summary>
        /// Parses the HTTP request from the client
        /// </summary>
        public void ParseRequest()
        {
            StreamReader reader = new StreamReader(inputStream);
            String request = reader.ReadLine();
            string[] tokens = request.Split(' ');
            if (tokens.Length != 3)
            {
                throw new Exception("invalid http request line");
            }
            http_method = tokens[0].ToUpper();
            http_url = tokens[1];
            http_protocol_versionstring = tokens[2];

            Console.WriteLine("starting: " + request);
        }

        /// <summary>
        /// Parses the HTTP header
        /// </summary>
        public void ReadHeaders()
        {
            StreamReader reader = new StreamReader(inputStream);
            Console.WriteLine("readHeaders()");
            String line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Equals(""))
                {
                    Console.WriteLine("got headers");
                    return;
                }

                int separator = line.IndexOf(':');
                if (separator == -1)
                {
                    throw new Exception("invalid http header line: " + line);
                }
                String name = line.Substring(0, separator);
                int pos = separator + 1;
                while ((pos < line.Length) && (line[pos] == ' '))
                {
                    pos++; // strip any spaces
                }

                string value = line.Substring(pos, line.Length - pos);
                Console.WriteLine("header: {0}:{1}", name, value);
                httpHeaders.Add(value);
            }
        }
    }
}
