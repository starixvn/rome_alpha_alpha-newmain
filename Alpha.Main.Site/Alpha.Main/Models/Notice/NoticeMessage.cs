namespace Alpha.Main.Models.Notice
{
    using System;

    public class NoticeMessage
    {
        public NoticeMessage(
            int id,
            string contentEn,
            string contentCs,
            string contentCh,
            string contentJp,
            string contentTh,
            string contentIt,
            string contentKo,
            string contentEs,
            DateTime postTime,
            byte type,
            bool isSticky,
            DateTime lastUpdateTime,
            string betTransId,
            bool isRead)
        {
            Id = id;
            ContentEn = contentEn;
            ContentCs = contentCs;
            ContentCh = contentCh;
            ContentJp = contentJp;
            ContentTh = contentTh;
            ContentIt = contentIt;
            ContentKo = contentKo;
            ContentEs = contentEs;
            PostTime = postTime;
            Type = type;
            IsSticky = isSticky;
            LastUpdateTime = lastUpdateTime;
            BetTransId = betTransId;
            IsRead = isRead;
        }

        public int Id { get; private set; }

        public string ContentEn { get; private set; }

        public string ContentCs { get; private set; }

        public string ContentCh { get; private set; }

        public string ContentJp { get; private set; }

        public string ContentTh { get; private set; }

        public string ContentIt { get; private set; }

        public string ContentKo { get; private set; }

        public string ContentEs { get; private set; }

        public DateTime PostTime { get; private set; }

        public byte Type { get; private set; }

        public bool IsSticky { get; private set; }

        public DateTime LastUpdateTime { get; private set; }

        public string BetTransId { get; private set; }

        public bool IsRead { get; private set; }
    }
}