using System.IO;
using Client.Replies;
using Client.Replies.Parsers;
using NUnit.Framework;
using Should;
using System.Linq;

namespace Client.Test.Replies.Parsers
{

    public class MultiBulkReplyParserTests_NullElement
    {
        MultiBulkReply reply;

        [TestFixtureSetUp]
        public void When_parsing_a_response_with_a_null_element()
        {
            const string replyString = "*3\r\n$3\r\nfoo\r\n$-1\r\n$3\r\nbar\r\n";

            var stream = new MemoryStream(replyString.ToBytes());

            reply = new MultiBulkReplyParser().Parse(stream);
        }

        [Test]
        public void Should_return_an_empty_result_set()
        {
            reply.GetElements().ElementAt(0).ToString().ShouldEqual("foo");
            reply.GetElements().ElementAt(1).ShouldBeNull();
            reply.GetElements().ElementAt(2).ToString().ShouldEqual("bar");
        }
    }

    public class MultiBulkReplyParserTests_EmptyResponse
    {
        MultiBulkReply reply;

        [TestFixtureSetUp]
        public void When_parsing_an_empty_response()
        {
            const string replyString = "*0\r\n";

            var stream = new MemoryStream(replyString.ToBytes());
            
            reply = new MultiBulkReplyParser().Parse(stream);
        }

        [Test]
        public void Should_return_an_empty_result_set()
        {
            reply.GetElements().ShouldBeEmpty();
        }
    }
}