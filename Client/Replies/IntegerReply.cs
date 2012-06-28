namespace Client.Replies
{
    public class IntegerReply
    {
        public IntegerReply(long value)
        {
            Value = value;
        }

        public long Value { get; private set; }
    }
}