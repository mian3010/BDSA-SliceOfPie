using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SliceOfPie_Server
{
    class ServerLauncher
    {

        public static void Main(String[] args)
        {
            NetworkServer server = NetworkServer.GetInstance();
            server.Listen();
        }
    }
}
