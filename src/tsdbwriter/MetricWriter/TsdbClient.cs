/*  
Copyright (c) Microsoft.  All rights reserved.  Licensed under the MIT License.  See LICENSE in the root of the repository for license information 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;

namespace MetricWriter
{
    public class TsdbClient
    {
        private static Socket _socket; 
        public static void Write (string dataPoint, string hostName, int port, bool reuseSocket)
        {
            if (_socket == null)
            {
                _socket = GetConnectedSocket(hostName, port);
            }

            int bytecount = _socket.Send(new ASCIIEncoding().GetBytes(dataPoint + Environment.NewLine));
            if (!reuseSocket)
            {
                _socket.Close();
                _socket.Dispose();
            }

        }
        private static Socket GetConnectedSocket(string hostName, int port)
        {
            var hostEntry = Dns.GetHostAddresses(hostName);
            IPEndPoint endPoint = new IPEndPoint(hostEntry.First(), port);

            var socket =  new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(endPoint);
            return socket; 
        }
    }
}
