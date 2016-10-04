namespace Alpha.Main.Models.Notice
{
    using System;

    public class NoticeMessage
    {
        public NoticeMessage(
            int id,
            string contentEN,
            string contentCS,
            string contentCH,
            string contentJP,
            string contentTH,
            string contentIT,
            string contentKO,
            string contentES,
            DateTime postTime,
            byte type,
            bool isSticky,
            DateTime lastUpdateTime,
            string betTransId,
            bool isRead)
        {
            Id = id;
            ContentEN = contentEN;
            ContentCS = contentCS;
            ContentCH = contentCH;
            ContentJP = contentJP;
            ContentTH = contentTH;
            ContentIT = contentIT;
            ContentKO = contentKO;
            ContentES = contentES;
            PostTime = postTime;
            MessageType = type;
            IsSticky = isSticky;
            LastUpdateTime = lastUpdateTime;
            BetTransId = betTransId;
            IsRead = isRead;
        }

        public int Id { get; private set; }

        public string ContentEN { get; private set; }

        public string ContentCS { get; private set; }

        public string ContentCH { get; private set; }

        public string ContentJP { get; private set; }

        public string ContentTH { get; private set; }

        public string ContentIT { get; private set; }

        public string ContentKO { get; private set; }

        public string ContentES { get; private set; }

        public DateTime PostTime { get; private set; }

        public byte MessageType { get; private set; }

        public bool IsSticky { get; private set; }

        public DateTime LastUpdateTime { get; private set; }

        public string BetTransId { get; private set; }

        public bool IsRead { get; private set; }
    }
}