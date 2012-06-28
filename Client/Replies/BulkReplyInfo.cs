using System.Collections.Generic;
using System.Linq;

namespace Client.Replies
{
    public class BulkReplyInfo
    {
        readonly BulkReply _reply;
        Dictionary<string, string> dictionary;

        public BulkReplyInfo(BulkReply reply)
        {
            _reply = reply;
        }

        public string this[string key]
        {
            get
            {
                dictionary = dictionary ?? CreateDictionary();
                return dictionary[key];
            }
        }

        Dictionary<string, string> CreateDictionary()
        {
            var s = _reply.ToString();
            var strings = s.Split('\r', '\n').Where(x => !string.IsNullOrWhiteSpace(x));
            return strings.ToDictionary(x => x.Split(':')[0], x => x.Split(':')[1]);
        }
    }
}