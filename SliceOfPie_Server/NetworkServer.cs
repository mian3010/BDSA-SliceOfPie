using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace SliceOfPie_Server
{
    public class NetworkServer
    {
        //Default port = 8080
        protected int port;
        HttpListener listener;
        bool is_active = true;
        private static NetworkServer server;
        RequestHandler handler;

        public static NetworkServer GetInstance()
        {
            Console.Out.WriteLine("Building server");
            if (server == null)
                server = new NetworkServer(8080, RequestHandler.instance);
            return server;
        }
   
        private NetworkServer(int port, RequestHandler handler) {
            this.port = port;
            this.handler = handler;
        }
    
        /// <summary>
        /// Start the listening loop. Listens for requests from clients.
        /// </summary>
        public void listen() {
            listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8080/");
            listener.Start();
            while (is_active) {
                HttpListenerContext context = listener.GetContext();
                HTTPProcessor processor = new HTTPProcessor(context, handler);
                Thread thread = new Thread(() => processor.Process());
                thread.Start();
                Thread.Sleep(1);
             }
         }

        /// <summary>
        /// Closes the server.
        /// </summary>
        public void Close()
        {
            is_active = false;
        }
    }
}
