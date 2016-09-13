using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace MiniRedisClient
{
    public class MiniRedisClient
    {
        private readonly TcpClient _client;

        private StreamReader _sReader;
        private StreamWriter _sWriter;

        private Boolean _isConnected;

        public MiniRedisClient(String ipAddress, int portNum)
        {
            _client = new TcpClient();
            _client.Connect(ipAddress, portNum);

            HandleCommunication();
        }

        private void HandleCommunication()
        {
            _sReader = new StreamReader(_client.GetStream(), Encoding.ASCII);
            _sWriter = new StreamWriter(_client.GetStream(), Encoding.ASCII);

            _isConnected = true;
            String sData = null;
            while (_isConnected)
            {
                Console.Write("mRedis: ");
                sData = Console.ReadLine();

                _sWriter.WriteLine(sData);
                _sWriter.Flush();

                String sDataIncomming = _sReader.ReadLine();
                Console.WriteLine("Responce: {0}", sDataIncomming);
            }
        }
    }
}