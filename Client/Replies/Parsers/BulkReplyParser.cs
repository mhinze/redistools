using System.IO;
using System.Text;

namespace Client.Replies.Parsers
{
    public class BulkReplyParser
    {
        public BulkReply Parse(Stream reply)
        {
            reply.ReadByte();
            var line = ReadLine(reply);
            if (line[0] == '-' && line[1] == '1' && line.Length == 2) return new BulkReply(null);
            var i = int.Parse(line);

            var buffer = new byte[i];
            reply.Read(buffer, 0, buffer.Length);
            reply.ReadByte();
            reply.ReadByte();
            return new BulkReply(buffer);
        }

        string ReadLine(Stream stream)
        {
            var sb = new StringBuilder();
            int c;

            while ((c = stream.ReadByte()) != -1)
            {
                if (c == '\r')
                    continue;
                if (c == '\n')
                    break;
                sb.Append((char) c);
            }
            return sb.ToString();
        }
    }
}