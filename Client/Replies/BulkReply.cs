namespace Client.Replies
{
    public class BulkReply
    {
        public BulkReply(byte[] value)
        {
            Value = value;
        }

        public byte[] Value { get; private set; }
    }
}