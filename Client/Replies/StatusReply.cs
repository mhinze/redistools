namespace Client.Replies
{
    public class StatusReply : IReply
    {
        public StatusReply(string status)
        {
            Status = status;
        }

        public string Status { get; private set; }
    }
}