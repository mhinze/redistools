namespace Client.Replies
{
    public class IntegerReply : IReply
    {
        public IntegerReply(long value)
        {
            Value = value;
        }

        public long Value { get; private set; }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static implicit operator bool(IntegerReply self)
        {
            return self.Value == 1;
        }

        public static implicit operator string(IntegerReply self)
        {
            return self.ToString();
        }

        public static implicit operator long(IntegerReply self)
        {
            return self.Value;
        }
    }
}