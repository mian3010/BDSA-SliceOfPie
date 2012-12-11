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
      private int _port;
        HttpListener _listener;
        bool _isActive = true;
        private static NetworkServer _server;
      readonly RequestHandler handler;

        public static NetworkServer GetInstance()
        {
            if (_server == null)
                _server = new NetworkServer(8080, RequestHandler.Instance);
            return _server;
        }
   
        private NetworkServer(int port, RequestHandler handler) {
            this._port = port;
            this.handler = handler;
        }
    
        /// <summary>
        /// Start the listening loop. Listens for requests from clients.
        /// </summary>
        public void Listen() {
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://localhost:8080/");
            _listener.Start();
            while (_isActive) {
                HttpListenerContext context = _listener.GetContext();
                HttpProcessor processor = new HttpProcessor(context, handler);
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
            _isActive = false;
        }
    }
}
