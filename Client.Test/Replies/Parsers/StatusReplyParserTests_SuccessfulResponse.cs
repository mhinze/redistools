using System.IO;
using Client.Replies;
using Client.Replies.Parsers;
using NUnit.Framework;
using Should;

namespace Client.Test.Replies.Parsers
{
    [TestFixture]
    public class StatusReplyParserTests_SuccessfulResponse
    {
        StatusReply reply;

        [TestFixtureSetUp]
        public void When_parsing_a_successful_response()
        {
            const string replyString = "+OK\r\n";

            var stream = new MemoryStream(replyString.ToBytes());

            reply = new StatusReplyParser().Parse(stream);
        }

        [Test]
        public void Should_create_reply_with_status()
        {
            reply.Status.ShouldEqual("OK");
        }
    }
}