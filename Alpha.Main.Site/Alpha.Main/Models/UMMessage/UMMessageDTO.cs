namespace Alpha.Main.Models.UMMessage
{
    public class UMMessageDTO
    {
        public UMMessageDTO(long id, string message)
        {
            Id = id;
            Message = message;
        }

        public long Id { get; }

        public string Message { get; }
    }
}
