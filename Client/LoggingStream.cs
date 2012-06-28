using System;
using System.IO;

namespace Client
{
    internal class LoggingStream : Stream
    {
        readonly Stream _stream;
        readonly IConnectionLog _log;

        public LoggingStream(Stream stream, IConnectionLog log)
        {
            _stream = stream;
            _log = log;
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var result = _stream.Read(buffer, offset, count);
            _log.LogReply(buffer);
            return result;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override bool CanRead
        {
            get { throw new NotImplementedException(); }
        }

        public override bool CanSeek
        {
            get { throw new NotImplementedException(); }
        }

        public override bool CanWrite
        {
            get { throw new NotImplementedException(); }
        }

        public override long Length
        {
            get { throw new NotImplementedException(); }
        }

        public override long Position
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override int ReadByte()
        {
            var readByte = _stream.ReadByte();
            _log.LogReply((byte)readByte);
            return readByte;
        }
    }
}