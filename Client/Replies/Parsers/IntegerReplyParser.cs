﻿using System.IO;
using System.Text;

namespace Client.Replies.Parsers
{
    public class IntegerReplyParser
    {
         public IntegerReply Parse(Stream reply)
         {
             reply.ReadByte();
             var readLine = ReadLine(reply);
             var i = int.Parse(readLine);
             return new IntegerReply(i);
         }

         string ReadLine(Stream stream)
         {
             var sb = new StringBuilder();
             int c;

             while ((c = stream.ReadByte()) != -1)
             {
                 if (c == '\r')
                     continue;
                 if (c == '\n')
                     break;
                 sb.Append((char)c);
             }
             return sb.ToString();
         }
    }
}