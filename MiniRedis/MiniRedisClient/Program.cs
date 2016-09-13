using System;

namespace MiniRedisClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var ipAddress = "127.0.0.1";
            int port = 5555;

            Console.WriteLine("Multi-Threaded Mini Redis TCP Client");
            var miniRedisClient = new MiniRedisClient(ipAddress, port);
        }
    }
}
