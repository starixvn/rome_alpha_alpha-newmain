namespace Alpha.Main.Models.UMMessage
{
    public class UMMessageItem
    {
        public UMMessageItem()
        {
        }

        public UMMessageItem(int id, string message)
        {
            Id = id;
            Message = message;
        }

        public UMMessageItem(
            int id,
            string message_EN,
            string message_CS,
            string message_CH,
            string message_TH,
            string message_JP,
            string message_KO,
            string message_VI)
        {
            Id = id;
            Message_EN = message_EN;
            Message_CS = message_CS;
            Message_CH = message_CH;
            Message_TH = message_TH;
            Message_JP = message_JP;
            Message_KO = message_KO;
            Message_VI = message_VI;
        }

        public int Id { get; }

        public string Message { get; }

        public string Message_EN { get; }

        public string Message_CS { get; }

        public string Message_CH { get; }

        public string Message_TH { get; }

        public string Message_JP { get; }

        public string Message_KO { get; }

        public string Message_VI { get; }
    }
}
