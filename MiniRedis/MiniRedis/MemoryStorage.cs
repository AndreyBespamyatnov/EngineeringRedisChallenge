using System;
using System.Linq;
using System.Runtime.Caching;

namespace MiniRedis
{
    public class MemoryStorage : IDisposable
    {
        private ObjectCache _cache = MemoryCache.Default;

        // Add new element with Expiration
        public void Set(string key, object value, int seconds = 0)
        {
            DateTime absoluteExpiration = DateTime.MaxValue;
            if (seconds != 0)
            {
                absoluteExpiration = DateTime.Now.AddSeconds(seconds);
            }

            _cache.Set(key, value, absoluteExpiration);
        }

        // Get the value of key
        public object Get(string key)
        {
            var contains = _cache.Contains(key);
            if (!contains)
            {
                return null;
            }

            var o = _cache.Get(key);
            return o;
        }

        // Removes the specified keys
        public void Del(params string[] keys)
        {
            foreach (var key in keys)
            {
                var contains = _cache.Contains(key);
                if (contains)
                {
                    _cache.Remove(key);
                }
            }
        }

        // Return the number of keys in the database
        public int Dbsize()
        {
            var count = _cache.Count();
            return count;
        }

        // Increments the number stored at key by one
        public void Incr(string key)
        {
            var contains = _cache.Contains(key);
            if (!contains)
            {
                this.Set(key, 0);
            }

            var o = this.Get(key);
            if (!(o is int))
            {
                return;
            }
            
            var i = (int)o;
            i++;

            this.Set(key, i);
        }

        // Adds all the specified members with the specified scores to the sorted set stored at key
        public void Zadd(string key, object objects)
        {
            // TODO: need somethink like sortedset + dictionary
            throw new NotImplementedException();
        }

        // Returns the sorted set cardinality (number of elements) of the sorted set stored at key
        public int Zcard()
        {
            throw new NotImplementedException();
        }

        // Returns the rank of member in the sorted set stored at key, with the scores ordered from low to high
        public int Zrank()
        {
            throw new NotImplementedException();
        }

        // ReturnsReturns the specified range of elements in the sorted set stored at key
        public int Zrange()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _cache = null;
        }
    }
}
