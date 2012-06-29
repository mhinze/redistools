using System;
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
            return _connection.Send<StatusReply>(RedisCommands.DEL, key);
        }

        public StatusReply Set(string key, string value)
        {
            return _connection.Send<StatusReply>(RedisCommands.SET, key, value);
        }

        public byte[] Get(string key)
        {
            return _connection.Send<BulkReply>(RedisCommands.GET, key);
        }

        public string GetString(string key)
        {
            return Get(key).ToUtf8String();
        }

        public StatusReply FlushAll()
        {
            return _connection.Send<StatusReply>(RedisCommands.FLUSHALL);
        }

        public string[] Keys(string pattern)
        {
            return _connection.Send<MultiBulkReply>(RedisCommands.KEYS, pattern);
        }

        public long DbSize()
        {
            return _connection.Send<IntegerReply>(RedisCommands.DBSIZE);
        }

        public bool Exists(string key)
        {
            return _connection.Send<IntegerReply>(RedisCommands.EXISTS, key.ToBytes());
        }

        public bool Expire(string key, int seconds)
        {
            return _connection.Send<IntegerReply>(RedisCommands.EXPIRE, key, seconds.ToString());
        }

        public long Ttl(string key)
        {
            return _connection.Send<IntegerReply>("TTL", key);
        }

        public bool ExpireAt(string key, DateTime expireDate)
        {
            var unixTimestamp = expireDate.ToUnixTimestamp();
            return _connection.Send<IntegerReply>("EXPIREAT", key, unixTimestamp);
        }

        public StatusReply Auth(string password)
        {
            return _connection.Send<StatusReply>("AUTH", password);
        }

        public string Echo(string message)
        {
            return _connection.Send<BulkReply>("ECHO", message);
        }

        public StatusReply Ping()
        {
            return _connection.Send<StatusReply>("PING");
        }

        public StatusReply Quit()
        {
            return _connection.Send<StatusReply>("QUIT");
        }

        public StatusReply Select(int db)
        {
            return _connection.Send<StatusReply>("SELECT", db.ToString());
        }

        public BulkReplyInfo Info()
        {
            return _connection.Send<BulkReply>("INFO");
        }

        public StatusReply BgRewriteAof()
        {
            return _connection.Send<StatusReply>("BGREWRITEAOF");
        }

        public StatusReply BgSave()
        {
            return _connection.Send<StatusReply>("BGSAVE");
        }
    }
}