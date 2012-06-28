using System.IO;
using System.Text;

namespace Client.Replies.Parsers
{
    public class StatusReplyParser : IReplyParser<StatusReply>
    {
        public StatusReply Parse(Stream reply)
        {
            var firstByte = reply.ReadByte();

            if (firstByte == -1) throw new RedisReplyException("No response");

            var remainingResponse = ReadLine(reply);

            if (firstByte == '-') throw new RedisReplyException(remainingResponse);

            return new StatusReply(remainingResponse);
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