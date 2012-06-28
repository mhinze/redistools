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
        int DbSize();
        bool Exists(string key);
    }
}