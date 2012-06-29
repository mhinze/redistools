using System.IO;
using Client.Replies;
using Client.Replies.Parsers;
using NUnit.Framework;

namespace Client.Test.Replies.Parsers
{
    public class MultiBulkReplyParserTests_Error
    {
        [Test, ExpectedException(typeof (RedisReplyException), ExpectedMessage = "ERR")]
        public void Should_return_null()
        {
            const string replyString = "-ERR";

            var stream = new MemoryStream(replyString.ToBytes());

            new MultiBulkReplyParser().Parse(stream);
        }
    }
}