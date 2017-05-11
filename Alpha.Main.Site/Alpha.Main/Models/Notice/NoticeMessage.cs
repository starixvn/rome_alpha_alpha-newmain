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
            string contentVN,
            DateTime postTime,
            byte type,
            bool isSticky,
            DateTime lastUpdateTime,
            string betTransId,
            bool isRead,
            byte topLevel)
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
            ContentVN = contentVN;
            PostTime = postTime;
            MessageType = type;
            IsSticky = isSticky;
            LastUpdateTime = lastUpdateTime;
            BetTransId = betTransId;
            IsRead = isRead;
            TopLevel = topLevel;
        }

        public int Id { get; }

        public string ContentEN { get; }

        public string ContentCS { get; }

        public string ContentCH { get; }

        public string ContentJP { get; }

        public string ContentTH { get; }

        public string ContentIT { get; }

        public string ContentKO { get; }

        public string ContentES { get; }

        public string ContentVN { get; }

        public DateTime PostTime { get; }

        public byte MessageType { get; }

        public bool IsSticky { get; }

        public DateTime LastUpdateTime { get; }

        public string BetTransId { get; }

        public bool IsRead { get; }

        public byte TopLevel { get; }
    }
}