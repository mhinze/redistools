using System.IO;

namespace Client.Replies.Parsers
{
    public interface IReplyParser<T>
    {
        T Parse(Stream stream);
    }
}