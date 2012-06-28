using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Client.Replies;
using Client.Replies.Parsers;

namespace Client
{
    public static class ReplyParsers
    {
        static readonly Dictionary<Type, object> table =
            new Dictionary<Type, object>
                {
                    {typeof (BulkReply), new BulkReplyParser()},
                    {typeof (IntegerReply), new IntegerReplyParser()},
                    {typeof (MultiBulkReply), new MultiBulkReplyParser()},
                    {typeof (StatusReply), new StatusReplyParser()}
                };

        public static IReplyParser<T> Get<T>()
        {
            return (IReplyParser<T>) table[typeof (T)];
        }
    }

    public class RedisConnection
    {
        static readonly byte[] crlf = new[] {(byte) '\r', (byte) '\n'};
        readonly IConnectionLog _log;
        readonly RedisClientSocket _socket;

        public RedisConnection(RedisServer server, IConnectionLog log)
        {
            _log = log ?? new NoopLog();
            _socket = new RedisClientSocket(server);
        }

        public TReply Send<TReply>(params byte[][] arguments)
        {
            return send(arguments, ReplyParsers.Get<TReply>().Parse);
        }

        public TReply Send<TReply>(params string[] arguments)
        {
            return Send<TReply>(arguments.Select(x => x.ToBytes()).ToArray());
        }

        public TReply Send<TReply>(byte[] command, params string[] arguments)
        {
            var sources = new[] {command}.Concat(arguments.Select(x => x.ToBytes())).ToArray();

            return Send<TReply>(sources);
        }

        static byte[] getRequestBytes(byte[][] arguments)
        {
            var bytes = new List<byte> {(byte) '*'};

            bytes.AddRange(arguments.Length.ToString().ToBytes());
            bytes.AddRange(crlf);
            foreach (var argument in arguments)
            {
                bytes.Add((byte) '$');
                bytes.AddRange(argument.Length.ToString().ToBytes());
                bytes.AddRange(crlf);
                bytes.AddRange(argument);
                bytes.AddRange(crlf);
            }
            return bytes.ToArray();
        }
        
        TReply send<TReply>(byte[][] arguments, Func<Stream, TReply> handleResponse)
        {
            var commandBytes = getRequestBytes(arguments);
            _log.LogRequest(commandBytes);
            var response = handleResponse;
            handleResponse = stream =>
                {
                    var reply = response(new LoggingStream(stream, _log));
                    _log.FlushReply();
                    return reply;
                };

            return _socket.Send(commandBytes, handleResponse);
        }

        class NoopLog : IConnectionLog
        {
            public void LogRequest(byte[] request) {}

            public void LogReply(params byte[] reply) {}
            public void FlushReply() {}
        }
    }
}