using System.Collections.Generic;

namespace Client
{
    public interface IRedisClient
    {
        void Del(string key);
        void Set(string key, string value);
        byte[] Get(string key);
        string GetString(string key);
        void FlushAll();
        string[] Keys(string pattern);
    }
}