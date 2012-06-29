using System.IO;
using System.Text;

namespace Client.Replies.Parsers
{
    public class MultiBulkReplyParser : IReplyParser<MultiBulkReply>
    {
        public MultiBulkReply Parse(Stream reply)
        {
            var result = new MultiBulkReply();

            var firstByte = reply.ReadByte();
            var line = ReadLine(reply);

            if (firstByte == '-')
                throw new RedisReplyException(line);

            if (line[0] == '-' && line[1] == '1' && line.Length == 2) return null;

            var elementCount = int.Parse(line);

            if (elementCount == 0)
                return result;

            var elementParser = new BulkReplyParser();

            for (int i = 0; i < elementCount; i++)
            {
                var element = elementParser.Parse(reply);
                if (element == null)
                    result.AddElement(null);
                else
                    result.AddElement(element.Value);
            }

            return result;
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