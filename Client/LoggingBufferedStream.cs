using System.IO;

namespace Client
{
    public class LoggingBufferedStream
    {
        readonly IConnectionLog _log;
        readonly BufferedStream _stream;

        public LoggingBufferedStream(IConnectionLog log, BufferedStream stream)
        {
            _log = log;
            _stream = stream;
        }

        public int ReadByte()
        {
            var i = _stream.ReadByte();
            _log.LogReply((byte) i);
            return i;
        }
    }
}