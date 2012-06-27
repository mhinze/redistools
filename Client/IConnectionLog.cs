using System;
using System.Collections.Generic;
using System.Text;

namespace Client
{
    public interface IConnectionLog
    {
        void LogRequest(byte[] request);
        void LogReply(params byte[] reply);
        void FlushReply();
    }

    public class ConsoleConnectionLog : IConnectionLog
    {
        readonly List<byte> replyBytes = new List<byte>();

        public void LogRequest(byte[] request)
        {
            var utf8 = Encoding.UTF8.GetString(request);
            var formatted = utf8.Replace("\r\n", "\r\n=> ").TrimEnd(' ','>','=');
            Console.Write("=> ");
            Console.Write(formatted);
        }

        public void LogReply(params byte[] reply)
        {
            replyBytes.AddRange(reply);
        }

        public void FlushReply()
        {
            var utf8 = Encoding.UTF8.GetString(replyBytes.ToArray());
            var formatted = utf8.Replace("\r\n", "\r\n<= ").TrimEnd(' ', '<', '=');
            Console.Write("<= ");
            Console.Write(formatted);
            replyBytes.Clear();
        }
    }
}