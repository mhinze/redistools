using System.IO;
using System.Text;

namespace Client.Replies.Parsers
{
    public class MultiBulkReplyParser
    {
        public MultiBulkReply Parse(Stream reply)
        {
            var result = new MultiBulkReply();
            reply.ReadByte();
            var length = ReadLine(reply);
            if (length[0] == '-' && length[1] == '1' && length.Length == 2) return null;
            var i = int.Parse(length);
            var elementParser = new BulkReplyParser();
            for (int j = 0; j < i; j++)
            {
                var bulkReply = elementParser.Parse(reply);
                if (bulkReply == null)
                    result.AddElement(null);
                else
                result.AddElement(bulkReply.Value);
            }
            reply.ReadByte();
            reply.ReadByte();
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