using System.IO;
using Client.Replies;
using Client.Replies.Parsers;
using NUnit.Framework;
using Should;

namespace Client.Test.Replies.Parsers
{
    [TestFixture]
    public class BulkReplyParserTests_NullResponse
    {
        BulkReply reply;

        [TestFixtureSetUp]
        public void When_parsing_a_Redis_null_response()
        {
            const string replyString = "$-1";

            var stream = new MemoryStream(replyString.ToBytes());

            reply = new BulkReplyParser().Parse(stream);
        }

        [Test]
        public void Should_return_a_null_reply()
        {
            const string replyString = "$-1";

            var stream = new MemoryStream(replyString.ToBytes());

            reply = new BulkReplyParser().Parse(stream);

            reply.ShouldBeNull();
        }
    }
}