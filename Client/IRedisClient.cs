using System;
using Client.Replies;

namespace Client
{
    public interface IRedisClient
    {
        StatusReply Del(string key);
        StatusReply Set(string key, string value);
        byte[] Get(string key);
        string GetString(string key);
        StatusReply FlushAll();
        string[] Keys(string pattern);
        long DbSize();
        bool Exists(string key);
        bool Expire(string key, int seconds);
        long Ttl(string key);
        bool ExpireAt(string key, DateTime expireDate);
        StatusReply Auth(string password);
        string Echo(string message);
        StatusReply Ping();
        StatusReply Quit();
        StatusReply Select(int db);
    }
}