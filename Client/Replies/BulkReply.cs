namespace Client.Replies
{
    public class BulkReply
    {
        public BulkReply(byte[] value)
        {
            Value = value;
        }

        public byte[] Value { get; private set; }

        public override string ToString()
        {
            if (Value == null) return "(null)";
            return Value.ToUtf8String();
        }

        public static implicit operator string(BulkReply self)
        {
            return self.ToString();
        }

        public static implicit operator byte[](BulkReply self)
        {
            return self.Value;
        }
    }
}