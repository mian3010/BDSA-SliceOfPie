using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using SliceOfPie_Model;
using SliceOfPie_Model.Persistence;

namespace SliceOfPie_Server
{
    /// <summary>
    /// Class responsible for processing the HTTP requests from the Network Server.
    /// Is created in a new thread for every new client.
    /// </summary>
    public class HttpProcessor
    {
        private readonly HttpListenerRequest request;
        private readonly HttpListenerResponse response;
        private readonly RequestHandler handler;

        public HttpProcessor(HttpListenerContext context, RequestHandler handler)
        {
            request = context.Request;
            response = context.Response;
            this.handler = handler;
        }

        /// <summary>
        /// Processes the HTTP request. Determines the method and sends the request to the 
        /// right place.
        /// </summary>
        public void Process()
        {
            Console.Out.WriteLine("starting to process");
            string httpMethod = request.HttpMethod;
            Stream inputStream = request.InputStream;
            // Determines which http-method is called.
            try
            {
                if (httpMethod == "PUT")
                {
                    StreamReader reader = new StreamReader(inputStream);
                    string xml = reader.ReadToEnd();
                    SliceOfPie_Model.Persistence.File file = HtmlMarshalUtil.UnmarshallFile(xml);
                    handler.ReceiveFile(file, this);
                }
                else if (httpMethod == "POST")
                {
                    StreamReader reader = new StreamReader(inputStream);
                    string s = reader.ReadToEnd();
                    if (s.Contains("FileID"))
                    {
                        long id = HtmlMarshalUtil.UnMarshallId(s);
                        handler.GetFile(id, this);
                    }
                    else
                    {
                        FileList list = HtmlMarshalUtil.UnMarshallFileList(s);
                        handler.ReceiveFileList(list, this);
                    }
                }
                else
                {
                    throw new System.ArgumentException("Illegal XML method");
                }
            }
            catch (Exception e)
            {
                Exception ex = new System.ArgumentException("Error in Process()", e);
                throw ex;
            }
        }

        /// <summary>
        /// Responsible for sending the FileList back to the client
        /// </summary>
        /// <param name="list">FileList</param>
        public void RecieveFileList(FileList list)
        {
            string responseString = HtmlMarshalUtil.MarshallFileList(list);
            StreamReader content = new StreamReader(request.InputStream);
            Console.Out.WriteLine(content.ReadToEnd());
            response.ContentLength64 = responseString.Length;
            byte[] byteVersion = Encoding.ASCII.GetBytes(responseString);
            Stream stream = response.OutputStream;
            stream.Write(byteVersion, 0, byteVersion.Length);
            stream.Close();
        }

        /// <summary>
        /// Responsible for sending the Files back to the client
        /// </summary>
        /// <param name="file">File</param>
        public void RecieveFile(SliceOfPie_Model.Persistence.File file)
        {
            string responseString = HtmlMarshalUtil.MarshallFile(file);
            StreamReader content = new StreamReader(request.InputStream);
            Console.Out.WriteLine(content.ReadToEnd());
            response.ContentLength64 = responseString.Length;
            byte[] byteVersion = Encoding.ASCII.GetBytes(responseString);
            Stream stream = response.OutputStream;
            stream.Write(byteVersion, 0, byteVersion.Length);
            stream.Close();
        }


        /// <summary>
        /// Recieves a id on the file that has been pulled to the server.
        /// </summary>
        /// <param name="id"></param>
        public void RecieveConfirmation(long id)
        {
            string responseString = id.ToString(CultureInfo.InvariantCulture);
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
