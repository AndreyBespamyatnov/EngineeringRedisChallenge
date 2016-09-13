using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
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

                // validate message by regex
                if (!IsValidMessage(sData))
                {
                    Console.Write("Not allowed string");

                    sWriter.WriteLine("Error with message");
                    sWriter.Flush();
                    continue;
                }

                ProcessMessage(sData, sWriter);

                sWriter.Flush();
            }
        }

        private void ProcessMessage(string sData, StreamWriter sWriter)
        {
            MessageHandler setMessage = new SetMessage();
            MessageHandler getMessage = new GetMessage();
            MessageHandler delMessage = new DelMessage();
            MessageHandler dbsizeMessage = new DbsizeMessage();
            MessageHandler incrMessage = new IncrMessage();
            MessageHandler zaddMessage = new ZaddMessage();
            MessageHandler zcardMessage = new ZcardMessage();
            MessageHandler zrankMessage = new ZrankMessage();
            MessageHandler zrangeMessage = new ZrangeMessage();

            setMessage.SetHandler(getMessage);
            getMessage.SetHandler(delMessage);
            delMessage.SetHandler(dbsizeMessage);
            dbsizeMessage.SetHandler(incrMessage);
            incrMessage.SetHandler(zaddMessage);
            zaddMessage.SetHandler(zcardMessage);
            zcardMessage.SetHandler(zrankMessage);
            zrankMessage.SetHandler(zrangeMessage);

            setMessage.HandleRequest(sData, sWriter);
        }

        private bool IsValidMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return false;
            }

            const string regexRule = "^[a-zA-Z0-9-_ ]*$";
            var r = new Regex(regexRule);
            
            bool isMatch = r.IsMatch(message);
            return isMatch;
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
