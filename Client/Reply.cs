namespace Client
{
    public class Reply
    {
        public Reply(byte[] allBytes)
        {
            AllBytes = allBytes;
        }

        public byte[] AllBytes { get; private set; }
    }
}