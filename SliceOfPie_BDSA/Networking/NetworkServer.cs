using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace SliceOfPie_Model
{
    public class NetworkServer
    {
        //Default port = 8080
        protected int port;
        HttpListener listener;
        bool is_active = true;
        private static NetworkServer server;

        public static NetworkServer GetInstance()
        {
            if (server == null)
                server = new NetworkServer(8080);
            return server;
        }

        private NetworkServer(int port) {
            this.port = port;
        }
    
        /// <summary>
        /// Start the listening loop. Listens for requests from clients.
        /// </summary>
        public void listen() {
            listener = new HttpListener();
            listener.AuthenticationSchemes = AuthenticationSchemes.None;
            listener.Start();
            while (is_active) {
                HttpListenerContext context = listener.GetContext();
                HTTPProcessor processor = new HTTPProcessor(context);
                Thread thread = new Thread(new ThreadStart(processor.Process));
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
