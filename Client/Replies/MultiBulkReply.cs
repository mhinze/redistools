using System.Collections.Generic;
using System.Linq;

namespace Client.Replies
{
    public class MultiBulkReply : IReply
    {
        readonly List<byte[]> elements = new List<byte[]>();

        public void AddElement(byte[] element)
        {
            elements.Add(element);
        }

        public IEnumerable<BulkReply> GetElements()
        {
            return elements.Select(x => x == null ? null : new BulkReply(x));
        }

        public static implicit operator string[](MultiBulkReply self)
        {
            return self.elements.Select(x => x.ToUtf8String()).ToArray();
        }
    }
}