using System.IO;
using Client.Replies;
using Client.Replies.Parsers;
using NUnit.Framework;
using Should;

namespace Client.Test.Replies.Parsers
{
    [TestFixture]
    public class BulkReplyParserTests_SuccessfulResponse
    {
        BulkReply reply;

        [TestFixtureSetUp]
        public void When_parsing_a_successful_response()
        {
            const string replyString = "$6\r\nfoobar\r\n";

            var stream = new MemoryStream(replyString.ToBytes());

            reply = new BulkReplyParser().Parse(stream);
        }

        [Test]
        public void Should_create_reply_with_status()
        {
            reply.Value.ToUtf8String().ShouldEqual("foobar");
        }
    }

    [TestFixture]
    public class BulkReplyParserTests_Error
    {
        [Test, ExpectedException(typeof(RedisReplyException), ExpectedMessage = "-ERR")]
        public void Should_create_reply_with_status()
        {
            const string replyString = "-ERR";

            var stream = new MemoryStream(replyString.ToBytes());

            new BulkReplyParser().Parse(stream);
        }
    }
}