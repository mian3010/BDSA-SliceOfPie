using System.Threading;
using System.Net;

namespace SliceOfPie_Server
{
    public class NetworkServer
    {
        //Default port = 8080
      private readonly int _port;
        HttpListener _listener;
        bool _isActive = true;
        private static NetworkServer _server;
      readonly RequestHandler _handler;

        public static NetworkServer GetInstance()
        {
          return _server ?? (_server = new NetworkServer(80, RequestHandler.Instance));
        }

      private NetworkServer(int port, RequestHandler handler) {
            _port = port;
            _handler = handler;
        }
    
        /// <summary>
        /// Start the listening loop. Listens for requests from clients.
        /// </summary>
        public void Listen() {
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://localhost:"+_port+"/");
            //_listener.Prefixes.Add("http://10.25.243.118:" + _port + "/");
            _listener.Start();
            while (_isActive) {
                HttpListenerContext context = _listener.GetContext();
                var processor = new HttpProcessor(context, _handler);
                var thread = new Thread(processor.Process);
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
