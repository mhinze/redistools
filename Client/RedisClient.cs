using System;
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
            return _connection.SendExpectStatusReply(RedisCommands.DEL, key.ToBytes());
        }

        public StatusReply Set(string key, string value)
        {
            return _connection.SendExpectStatusReply(RedisCommands.SET, key.ToBytes(), value.ToBytes());
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
            return _connection.SendExpectStatusReply(RedisCommands.FLUSHALL);
        }

        public string[] Keys(string pattern)
        {
            var reply = _connection.SendExpectMultiBulkReply(RedisCommands.KEYS, pattern.ToBytes());
            return reply.GetElements().Select(x => Encoding.UTF8.GetString(x)).ToArray();
        }

        public long DbSize()
        {
            return _connection.SendExpectInt(RedisCommands.DBSIZE);
        }

        public bool Exists(string key)
        {
            return _connection.SendExpectInt(RedisCommands.EXISTS, key.ToBytes()) == 1;
        }

        public bool Expire(string key, int seconds)
        {
            return _connection.SendExpectInt(RedisCommands.EXPIRE, key.ToBytes(), seconds.ToString().ToBytes()) == 1;
        }

        public long Ttl(string key)
        {
            return _connection.SendExpectInt("TTL", key);
        }

        public bool ExpireAt(string key, DateTime expireDate)
        {
            var unixTimestamp = expireDate.ToUnixTimestamp();
            return _connection.SendExpectInt("EXPIREAT", key, unixTimestamp) == 1;
        }

        public StatusReply Auth(string password)
        {
            return _connection.SendExpectStatusReply()
        }

        public string Echo(string message)
        {
            throw new NotImplementedException();
        }

        public StatusReply Ping()
        {
            throw new NotImplementedException();
        }

        public StatusReply Quit()
        {
            throw new NotImplementedException();
        }

        public StatusReply Select(int db)
        {
            throw new NotImplementedException();
        }
    }
}