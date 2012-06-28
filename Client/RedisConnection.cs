using System;
using System.Collections.Generic;
using System.IO;
using Client.Replies;
using Client.Replies.Parsers;
using System.Linq;

namespace Client
{
    public class RedisConnection
    {
        static readonly byte[] crlf = new[] {(byte) '\r', (byte) '\n'};
        readonly IConnectionLog _log;
        readonly RedisServer _server;
        readonly RedisClientSocket _socket;
        readonly BulkReplyParser bulkReplyParser = new BulkReplyParser();
        readonly IntegerReplyParser integerReplyParser = new IntegerReplyParser();
        readonly MultiBulkReplyParser multiBulkReplyParser = new MultiBulkReplyParser();
        readonly StatusReplyParser statusReplyParser = new StatusReplyParser();

        public RedisConnection(RedisServer server, IConnectionLog log)
        {
            _server = server;
            _log = log ?? new NoopLog();
            _socket = new RedisClientSocket(server);
        }

        public StatusReply SendExpectSuccess(params byte[][] arguments)
        {
            return Send(arguments, stream => statusReplyParser.Parse(stream));
        }

        byte[] GetRequestBytes(byte[][] arguments)
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

        public MultiBulkReply SendExpectMultiBulkReply(params byte[][] arguments)
        {
            return Send(arguments, stream => multiBulkReplyParser.Parse(stream));
        }

        TReply Send<TReply>(byte[][] arguments, Func<Stream, TReply> handleResponse)
        {
            var commandBytes = GetRequestBytes(arguments);
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

        public BulkReply SendExpectBulkReply(params byte[][] arguments)
        {
            return Send(arguments, stream => bulkReplyParser.Parse(stream));
        }

        public long SendExpectInt(params byte[][] arguments)
        {
            return Send(arguments, stream => integerReplyParser.Parse(stream)).Value;
        }

        public long SendExpectInt(params string[] arguments)
        {
            return SendExpectInt(arguments.Select(x => x.ToBytes()).ToArray());
        }

        public long SendExpectInt(byte[] command, params string[] arguments)
        {
            return SendExpectInt(new[] {command}.Concat(arguments.Select(x => x.ToBytes())).ToArray());
        }

        class NoopLog : IConnectionLog
        {
            public void LogRequest(byte[] request) {}

            public void LogReply(params byte[] reply) {}
            public void FlushReply() {}
        }
    }
}

