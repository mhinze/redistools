using System.Collections.Generic;
using Client.Replies;
using Client.Replies.Parsers;

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
            return _socket.Send(GetCommandBytes(arguments), stream => statusReplyParser.Parse(stream));
        }

        byte[] GetCommandBytes(byte[][] arguments)
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
            return _socket.Send(GetCommandBytes(arguments), stream => multiBulkReplyParser.Parse(stream));
        }

        public BulkReply SendExpectBulkReply(params byte[][] arguments)
        {
            return _socket.Send(GetCommandBytes(arguments), stream => bulkReplyParser.Parse(stream));
        }

        public int SendExpectInt(params byte[][] arguments)
        {
            return _socket.Send(GetCommandBytes(arguments), stream => integerReplyParser.Parse(stream)).Value;
        }

        class NoopLog : IConnectionLog
        {
            public void LogRequest(byte[] request) {}

            public void LogReply(params byte[] reply) {}
            public void FlushReply() {}
        }
    }
}