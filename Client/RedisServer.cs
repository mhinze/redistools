namespace Client
{
    public class RedisServer
    {
        public RedisServer(string host = "127.0.0.1", int port = 6379, string password = null)
        {
            Host = host;
            Port = port;
            Password = password;
        }

        public string Host { get; private set; }
        public int Port { get; private set; }
        public string Password { get; private set; }
    }
}