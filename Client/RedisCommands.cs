namespace Client
{
    public class RedisCommands
    {
        public static readonly byte[] SET = "SET".ToBytes();
        public static readonly byte[] DEL = "DEL".ToBytes();
        public static readonly byte[] GET = "GET".ToBytes();
        public static readonly byte[] FLUSHALL = "FLUSHALL".ToBytes();
        public static readonly byte[] KEYS = "KEYS".ToBytes();
        public static readonly byte[] DBSIZE = "DBSIZE".ToBytes();
        public static readonly byte[] EXISTS = "EXISTS".ToBytes();
    }
}