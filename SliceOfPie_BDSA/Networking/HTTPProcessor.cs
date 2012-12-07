using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace SliceOfPie_Model
{
    /// <summary>
    /// Class responsible for processing the HTTP requests from the Network Server.
    /// Is created in a new thread for every new client.
    /// </summary>
    class HTTPProcessor
    {
        private HttpListenerRequest request;
        private HttpListenerResponse response;

        public HTTPProcessor(HttpListenerContext context)
        {
            request = context.Request;
            response = context.Response;
        }

        /// <summary>
        /// Processes the HTTP request. Determines the method and sends the request to the 
        /// right place.
        /// </summary>
        public void Process()
        {
            string http_method = request.HttpMethod;
            string responseString = "";
            // Determines which http-method is called.

            try
            {
                if (http_method == "PUT")
                {

                }
                else if (http_method == "POST")
                {

                }
                else if (http_method == "PUT")
                {

                }
                else
                {

                }
            }
            catch (Exception e)
            {
                Exception ex = new System.ArgumentException("Error processing XML", e);
                throw ex;
            }

            StreamReader content = new StreamReader(request.InputStream);
            Console.Out.WriteLine(content.ReadToEnd());
            response.ContentLength64 = responseString.Length;
            byte[] byteVersion = Encoding.ASCII.GetBytes(responseString);
            Stream stream = response.OutputStream;
            stream.Write(byteVersion, 0, byteVersion.Length);
            stream.Close();
        }
    }
}
