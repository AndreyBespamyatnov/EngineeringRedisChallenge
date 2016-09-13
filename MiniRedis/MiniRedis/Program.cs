using System;

namespace MiniRedis
{

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Multi-Threaded Mini Redis TCP Server");
            MiniRedisServer server = new MiniRedisServer(5555);
        }
    }
}
