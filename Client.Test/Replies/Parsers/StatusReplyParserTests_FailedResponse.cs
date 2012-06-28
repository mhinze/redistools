using System.IO;
using Client.Replies;
using Client.Replies.Parsers;
using NUnit.Framework;

namespace Client.Test.Replies.Parsers
{
    [TestFixture]
    public class StatusReplyParserTests_FailedResponse
    {
        [Test, ExpectedException(typeof (RedisReplyException), ExpectedMessage = "ERR")]
        public void When_parsing_a_failed_response_should_throw_an_exception_with_error_message()
        {
            const string replyString = "-ERR\r\n";
            var stream = new MemoryStream(replyString.ToBytes());

            new StatusReplyParser().Parse(stream);
        }

        [Test, ExpectedException(typeof (RedisReplyException), ExpectedMessage = "No response")]
        public void When_parsing_a_non_response_should_throw_an_exception()
        {
            var stream = new MemoryStream();
            new StatusReplyParser().Parse(stream);
        }
    }
}