using System.Collections.Generic;

namespace Client.Replies
{
    public class MultiBulkReply
    {
        readonly List<byte[]> elements = new List<byte[]>();

        public void AddElement(byte[] element)
        {
            elements.Add(element);
        }

        public IEnumerable<byte[]> GetElements()
        {
            return elements;
        }
    }
}