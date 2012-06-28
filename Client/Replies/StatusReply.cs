namespace Client.Replies
{
    public class StatusReply
    {
        public StatusReply(string status)
        {
            Status = status;
        }

        public string Status { get; private set; }
    }
}