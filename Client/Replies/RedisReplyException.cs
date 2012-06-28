using System;

namespace Client.Replies
{
    public class RedisReplyException : Exception
    {
        public RedisReplyException(string message) : base(message) {}
    }
}