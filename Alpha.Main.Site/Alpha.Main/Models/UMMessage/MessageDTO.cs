namespace Alpha.Main.Models.UMMessage
{
    public class MessageDTO
    {
        public MessageDTO(long id, string message)
        {
            Id = id;
            Message = message;
        }

        public long Id { get; }

        public string Message { get; }
    }
}
