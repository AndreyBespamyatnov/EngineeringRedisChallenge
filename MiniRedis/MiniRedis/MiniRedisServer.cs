using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MiniRedis
{
    internal class MiniRedisServer
    {
        private TcpListener _server;
        private Boolean _isRunning;

        public MiniRedisServer(int port)
        {
            _server = new TcpListener(IPAddress.Any, port);
            _server.Start();
 
            _isRunning = true;
 
            LoopClients();
        }

        private void LoopClients()
        {
            while (_isRunning)
            {
                // wait for client connection
                TcpClient newClient = _server.AcceptTcpClient();
 
                // client found.
                // create a thread to handle communication
                var t = new Thread(HandleClient);
                t.Start(newClient);
            }
        }

        private void HandleClient(object obj)
        {
            // retrieve client from parameter passed to thread
            TcpClient client = (TcpClient)obj;
 
            // sets two streams
            StreamWriter sWriter = new StreamWriter(client.GetStream(), Encoding.ASCII);
            StreamReader sReader = new StreamReader(client.GetStream(), Encoding.ASCII);
 
            Boolean bClientConnected = true;
            String sData = null;
 
            while (bClientConnected)
            {
                // reads from stream
                sData = sReader.ReadLine();
 
                // shows content on the console.
                Console.WriteLine("Mini Redis Server: " + sData);
 
                // to write something back.
                sWriter.WriteLine("Server responce.... Some data");
                sWriter.Flush();
            }
        }

        ~MiniRedisServer()
        {
            if (_server != null)
            {
                _server.Stop();
            }
        }
    }
}
