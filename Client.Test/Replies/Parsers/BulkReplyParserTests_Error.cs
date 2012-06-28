using System.IO;
using Client.Replies;
using Client.Replies.Parsers;
using NUnit.Framework;

namespace Client.Test.Replies.Parsers
{
    [TestFixture]
    public class BulkReplyParserTests_Error
    {
        [Test, ExpectedException(typeof(RedisReplyException), ExpectedMessage = "ERR")]
        public void Should_create_reply_with_status()
        {
            const string replyString = "-ERR";

            var stream = new MemoryStream(replyString.ToBytes());

            new BulkReplyParser().Parse(stream);
        }
    }
}