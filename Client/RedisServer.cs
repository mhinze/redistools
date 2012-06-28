namespace Client
{
    public class RedisServer
    {
        public RedisServer(string host = "127.0.0.1", int port = 6379, string password = null, int sendTimeout = -1)
        {
            Host = host;
            Port = port;
            Password = password;
            SendTimeout = sendTimeout;
        }

        public string Host { get; private set; }
        public int Port { get; private set; }
        public string Password { get; set; }
        public int SendTimeout { get; set; }
    }
}