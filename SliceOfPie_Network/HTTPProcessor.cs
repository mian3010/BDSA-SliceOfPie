﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace SliceOfPie_Network
{
    class HTTPProcessor
    {
        private TcpClient tcp;
        private NetworkCommunicator com;

        public HTTPProcessor(TcpClient tcp, NetworkCommunicator com)
        {
            this.tcp = tcp;
            this.com = com;
        }

        public void Process()
        { 
            
        }
    }
}
