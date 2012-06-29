using System.IO;
using Client.Replies;
using Client.Replies.Parsers;
using NUnit.Framework;
using Should;

namespace Client.Test.Replies.Parsers
{
    public class MultiBulkReplyParserTests_NullResponse
    {
        MultiBulkReply reply;

        [TestFixtureSetUp]
        public void When_parsing_a_null_response()
        {
            const string replyString = "*-1";

            var stream = new MemoryStream(replyString.ToBytes());

            reply = new MultiBulkReplyParser().Parse(stream);
        }

        [Test]
        public void Should_return_null()
        {
            reply.ShouldBeNull();
        }
    }

    public class MultiBulkReplyParserTests_Error
    {
        MultiBulkReply reply;

        [Test, ExpectedException(typeof(RedisReplyException), ExpectedMessage = "ERR")]
        public void Should_return_null()
        {
            const string replyString = "-ERR";

            var stream = new MemoryStream(replyString.ToBytes());

            new MultiBulkReplyParser().Parse(stream);
        }
    }


}