namespace Alpha.Main.Models.Message
{
    using System;

    public class Message
    {
        public Message()
        {
        }

        public Message(
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
            DateTime createTime,
            DateTime lastUpdateTime,
            byte messageType,
            bool isSticky,
            bool isTest,
            int matchId,
            bool isPublic,
            int creator,
            int lastUpdator,
            string creatorName,
            string lastUpdateName,
            bool isBatch,
            string bettransId,
            int userId,
            string userName,
            long rowNum)
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
            CreateTime = createTime;
            LastUpdateTime = lastUpdateTime;
            MessageType = messageType;
            IsSticky = isSticky;
            IsTest = isTest;
            MatchId = matchId;
            IsPublic = isPublic;
            CreatorId = creator;
            LastUpdator = lastUpdator;
            CreatorName = creatorName;
            LastUpdateName = lastUpdateName;
            IsBatch = isBatch;
            BetTransId = bettransId;
            UserId = userId;
            UserName = userName;
            RowNum = rowNum;
        }

        public Message(
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
            string contentID,
            string contentEL,
            string contentMX,
            DateTime postTime,
            DateTime createTime,
            DateTime lastUpdateTime,
            byte messageType,
            bool isSticky,
            bool isTest,
            int matchId,
            bool isBatch,
            int creator,
            int lastUpdator,
            string creatorName,
            string lastUpdateName,
            string userName,
            string bettransId,
            int userId,
            long rowNum) : this(
                id,
                contentEN,
                contentCS,
                contentCH,
                contentJP,
                contentTH,
                contentIT,
                contentKO,
                contentES,
                postTime,
                createTime,
                lastUpdateTime,
                messageType,
                isSticky,
                isTest,
                matchId,
                false,
                creator,
                lastUpdator,
                creatorName,
                lastUpdateName,
                isBatch,
                bettransId,
                userId,
                userName,
                rowNum)
        {
        }

        public Message(
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
            DateTime createTime,
            DateTime lastUpdateTime,
            byte messageType,
            bool isSticky,
            bool isTest,
            int matchId,
            bool isBatch,
            int creator,
            int lastUpdator,
            int userId,
            int batchId,
            string bingoContentEN,
            string bingoContentCS,
            string bingoContentCH,
            string bingoContentJP,
            string bingoContentTH,
            string bingoContentIT,
            string bingoContentKO,
            string bingoContentES,
            string creatorName,
            string lastUpdateName,
            string userName,
            string bettransId,
            long rowNum) : this(
                id,
                contentEN,
                contentCS,
                contentCH,
                contentJP,
                contentTH,
                contentIT,
                contentKO,
                contentES,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                postTime,
                createTime,
                lastUpdateTime,
                messageType,
                isSticky,
                isTest,
                matchId,
                isBatch,
                creator,
                lastUpdator,
                creatorName,
                lastUpdateName,
                userName,
                bettransId,
                userId,
                rowNum)
        {
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

        public DateTime CreateTime { get; private set; }

        public DateTime LastUpdateTime { get; private set; }

        public byte MessageType { get; private set; }

        public bool IsSticky { get; private set; }

        public bool IsTest { get; private set; }

        public int MatchId { get; private set; }

        public bool IsPublic { get; private set; }

        public int CreatorId { get; private set; }

        public int LastUpdator { get; private set; }

        public string CreatorName { get; private set; }

        public string LastUpdateName { get; private set; }

        public bool IsBatch { get; private set; }

        public string BetTransId { get; private set; }

        public int UserId { get; private set; }

        public string UserName { get; private set; }

        public long RowNum { get; private set; }
    }
}