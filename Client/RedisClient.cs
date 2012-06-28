using System.Linq;
using System.Text;
using Client.Replies;

namespace Client
{
    public class RedisClient : IRedisClient
    {
        readonly RedisConnection _connection;

        public RedisClient(RedisServer server, IConnectionLog log = null)
        {
            _connection = new RedisConnection(server, log);
        }

        public RedisClient(string host = "127.0.0.1", int port = 6379, string password = null)
            : this(new RedisServer(host, port, password)) {}

        public StatusReply Del(string key)
        {
            return _connection.SendExpectSuccess(RedisCommands.DEL, key.ToBytes());
        }

        public StatusReply Set(string key, string value)
        {
            return _connection.SendExpectSuccess(RedisCommands.SET, key.ToBytes(), value.ToBytes());
        }

        public byte[] Get(string key)
        {
            var bulkReply = _connection.SendExpectBulkReply(RedisCommands.GET, key.ToBytes());
            return bulkReply.Value;
        }

        public string GetString(string key)
        {
            return Get(key).ToUtf8String();
        }

        public StatusReply FlushAll()
        {
            return _connection.SendExpectSuccess(RedisCommands.FLUSHALL);
        }

        public string[] Keys(string pattern)
        {
            var reply = _connection.SendExpectMultiBulkReply(RedisCommands.KEYS, pattern.ToBytes());
            return reply.GetElements().Select(x => Encoding.UTF8.GetString(x)).ToArray();
        }

        public int DbSize()
        {
            return _connection.SendExpectInt(RedisCommands.DBSIZE);
        }
    }
}