using System.IO;
using Client.Replies;
using Client.Replies.Parsers;
using NUnit.Framework;
using Should;

namespace Client.Test.Replies.Parsers
{
    public class IntegerReplyParserTests_SuccessfulResponse
    {
        IntegerReply reply;

        [TestFixtureSetUp]
        public void When_parsing_a_successful_response()
        {
            const string replyString = ":1000\r\n";

            var stream = new MemoryStream(replyString.ToBytes());

            reply = new IntegerReplyParser().Parse(stream);
        }

        [Test]
        public void Should_create_reply_with_status()
        {
            reply.Value.ShouldEqual(1000);
        }
    }

    public class IntegerReplyParserTests_Error
    {
        
        
        [Test, ExpectedException(typeof(RedisReplyException), ExpectedMessage = "ERR")]
        public void Should_create_reply_with_status()
        {
            const string replyString = "-ERR";

            var stream = new MemoryStream(replyString.ToBytes());

            new IntegerReplyParser().Parse(stream);

        }
    }
}