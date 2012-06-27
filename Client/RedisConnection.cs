using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    public class RedisConnection
    {
        static readonly byte[] crlf = new[] {(byte) '\r', (byte) '\n'};
        readonly IConnectionLog _log;
        readonly RedisServer _server;

        public RedisConnection(RedisServer server, IConnectionLog log)
        {
            _server = server;
            _log = log ?? new NoopLog();
        }

        public void SendExpectSuccess(params byte[][] arguments)
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(_server.Host, _server.Port);
            var bstream = new LoggingBufferedStream(_log, new BufferedStream(new NetworkStream(socket), 16*1024));
            var commandBytes = GetCommandBytes(arguments);


            SocketSend(commandBytes, socket);

            var firstByte = bstream.ReadByte();
            if (firstByte == -1)
                throw new ResponseException("No response");
            var response = ReadLine(bstream);
            if (firstByte == '-')
            {
                throw new ResponseException(response);
            }
            _log.FlushReply();
        }

        void SocketSend(byte[] commandBytes, Socket socket)
        {
            _log.LogRequest(commandBytes);
            socket.Send(commandBytes);
        }

        byte[] ReadBulkElement(LoggingBufferedStream bstream)
        {
            var readLine = ReadLine(bstream);

            if (readLine == "$-1") return null;

            return GetBulkElementBytes(bstream).ToArray();
        }

        IEnumerable<byte> GetBulkElementBytes(LoggingBufferedStream bstream)
        {
            int c;
            while ((c = bstream.ReadByte()) != -1)
            {
                if (c == '\r')
                    continue;
                if (c == '\n')
                    break;
                yield return (byte) c;
            }
        }

        string ReadLine(LoggingBufferedStream bstream)
        {
            var sb = new StringBuilder();
            int c;

            while ((c = bstream.ReadByte()) != -1)
            {
                if (c == '\r')
                    continue;
                if (c == '\n')
                    break;
                sb.Append((char) c);
            }
            return sb.ToString();
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
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(_server.Host, _server.Port);
            var bstream = new LoggingBufferedStream(_log, new BufferedStream(new NetworkStream(socket), 16*1024));
            var commandBytes = GetCommandBytes(arguments);
            SocketSend(commandBytes, socket);

            var asterisk = bstream.ReadByte();
            var line = ReadLine(bstream);
            if (asterisk != '*')
            {
                throw new ResponseException(
                    string.Format("Expected the asterisk to begin a MultiBulkReply, instead received:\n{0}",
                                  asterisk + line));
            }
            int argCount;

            if (!int.TryParse(line.TrimEnd('\r', '\n'), out argCount))
            {
                throw new ResponseException(
                    string.Format(
                        "Expected a number after the asterisk to begin a MultiBulkReply, instead recieved:\n{0}", line));
            }

            var reply = new MultiBulkReply();

            for (var i = 0; i < argCount; i++)
            {
                var readBulkElement = ReadBulkElement(bstream);
                reply.AddElement(readBulkElement);
            }

            _log.FlushReply();
            return reply;
        }

        class NoopLog : IConnectionLog
        {
            public void LogRequest(byte[] request) {}

            public void LogReply(params byte[] reply) {}
            public void FlushReply() {}
        }

        public byte[] SendExpectBulkReply(params byte[][] arguments)
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(_server.Host, _server.Port);
            var bstream = new LoggingBufferedStream(_log, new BufferedStream(new NetworkStream(socket), 16 * 1024));
            var commandBytes = GetCommandBytes(arguments);
            SocketSend(commandBytes, socket);

            var reply = ReadBulkElement(bstream);

            _log.FlushReply();
            return reply;

        }
    }
}