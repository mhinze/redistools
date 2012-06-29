using System.IO;
using Client.Replies;
using Client.Replies.Parsers;
using NUnit.Framework;
using System.Linq;
using Should;

namespace Client.Test.Replies.Parsers
{
    public class MultiBulkReplyParserTests_SuccessfulResponse
    {
        MultiBulkReply reply;

        [TestFixtureSetUp]
        public void When_parsing_a_successful_response()
        {
            const string replyString = "*3\r\n$3\r\nfoo\r\n$-1\r\n$3\r\nbar\r\n";

            var stream = new MemoryStream(replyString.ToBytes());

            reply = new MultiBulkReplyParser().Parse(stream);
        }

        [Test]
        public void Should_parse_first_element()
        {
            reply.GetElements().ElementAt(0).ToString().ShouldEqual("foo");
        }

        [Test]
        public void Should_parse_second_element_as_null()
        {
            reply.GetElements().ElementAt(1).ShouldBeNull();
        }

        [Test]
        public void Should_parse_last_element()
        {
            reply.GetElements().ElementAt(2).ToString().ShouldEqual("bar");
        }

        [Test]
        public void Should_parse_three_elements()
        {
            reply.GetElements().Count().ShouldEqual(3);
        }
    }
}