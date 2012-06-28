namespace Client.Replies
{
    public class IntegerReply
    {
        public IntegerReply(int value)
        {
            Value = value;
        }

        public int Value { get; private set; }
    }
}