using System;

namespace Client
{
    public class ResponseException : Exception
    {
        public ResponseException(string message) : base(message) {}
    }
}