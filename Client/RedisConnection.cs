using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Client.Replies;
using Client.Replies.Parsers;

namespace Client
{
    public class RedisConnection
    {
        static readonly byte[] crlf = new[] { (byte) '\r', (byte) '\n' };
        readonly IConnectionLog _log;
        readonly RedisServer _server;
        readonly StatusReplyParser statusReplyParser = new StatusReplyParser();
        readonly IntegerReplyParser integerReplyParser = new IntegerReplyParser();
        readonly BulkReplyParser bulkReplyParser = new BulkReplyParser();
        readonly MultiBulkReplyParser multiBulkReplyParser = new MultiBulkReplyParser();

        public RedisConnection(RedisServer server, IConnectionLog log)
        {
            _server = server;
            _log = log ?? new NoopLog();
        }

        public StatusReply SendExpectSuccess(params byte[][] arguments)
        {
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                socket.Connect(_server.Host, _server.Port);
                var bstream = new BufferedStream(new NetworkStream(socket), 16 * 1024);
                var commandBytes = GetCommandBytes(arguments);

                SocketSend(commandBytes, socket);

                return statusReplyParser.Parse(bstream);    
            }
        }

        void SocketSend(byte[] commandBytes, Socket socket)
        {
            _log.LogRequest(commandBytes);
            socket.Send(commandBytes);
        }
        
        byte[] GetCommandBytes(byte[][] arguments)
        {
            var bytes = new List<byte> { (byte) '*' };

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
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                socket.Connect(_server.Host, _server.Port);
                var bstream = new BufferedStream(new NetworkStream(socket), 16 * 1024);
                var commandBytes = GetCommandBytes(arguments);
                SocketSend(commandBytes, socket);

                var multiBulkReply = multiBulkReplyParser.Parse(bstream);
                socket.Disconnect(false);
                socket.Close();
                socket.Dispose();
                return multiBulkReply;    
            }
        }

        public BulkReply SendExpectBulkReply(params byte[][] arguments)
        {
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                socket.Connect(_server.Host, _server.Port);
                var bstream = new BufferedStream(new NetworkStream(socket), 16 * 1024);
                var commandBytes = GetCommandBytes(arguments);
                SocketSend(commandBytes, socket);

                var reply = bulkReplyParser.Parse(bstream);
                
                return reply;    
            }
        }

        public int SendExpectInt(params byte[][] arguments)
        {
            throw new NotImplementedException();
        }

        class NoopLog : IConnectionLog
        {
            public void LogRequest(byte[] request) {}

            public void LogReply(params byte[] reply) {}
            public void FlushReply() {}
        }
    }
}