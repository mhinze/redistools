using System;
using System.IO;
using System.Net.Sockets;

namespace Client
{
    public class RedisClientSocket
    {
        readonly RedisServer _server;
        Socket _socket;
        Stream responseStream;

        public RedisClientSocket(RedisServer server)
        {
            _server = server;
        }

        public TReply Send<TReply>(byte[] data, Func<Stream, TReply> handleResponse)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (handleResponse == null)
            {
                throw new ArgumentNullException("handleResponse");
            }

            if (_socket == null)
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                    {
                        NoDelay = true,
                        SendTimeout = _server.SendTimeout
                    };

                _socket.Connect(_server.Host, _server.Port);
                responseStream = new BufferedStream(new NetworkStream(_socket), 16*1024);
            }
            try
            {
                _socket.Send(data);
            }
            catch (SocketException)
            {
                _socket.Close();
                _socket = null;
                throw;
            }
            return handleResponse(responseStream);
        }
    }
}