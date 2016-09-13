using System;

namespace MiniRedis
{
    public sealed class MemoryStorageInstance
    {
        private static readonly Lazy<MemoryStorage> Lazy = new Lazy<MemoryStorage>(() => new MemoryStorage());

        public static MemoryStorage Instance { get { return Lazy.Value; } }

        private MemoryStorageInstance()
        {
        }
    }
}