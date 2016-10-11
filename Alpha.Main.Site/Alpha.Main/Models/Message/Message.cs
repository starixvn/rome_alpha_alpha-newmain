namespace Alpha.Main.Models.Message
{
    using System;

    public class Message
    {
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
            LastUpdatorId = lastUpdator;
            CreatorName = creatorName;
            LastUpdatorName = lastUpdateName;
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
            CreatorId = creator;
            LastUpdatorId = lastUpdator;
            CreatorName = creatorName;
            LastUpdatorName = lastUpdateName;
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
            CreatorId = creator;
            LastUpdatorId = lastUpdator;
            CreatorName = creatorName;
            LastUpdatorName = lastUpdateName;
            IsBatch = isBatch;
            BetTransId = bettransId;
            UserId = userId;
            UserName = userName;
            RowNum = rowNum;
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

        public DateTime PostTime { get; }

        public DateTime CreateTime { get; }

        public DateTime LastUpdateTime { get; }

        public byte MessageType { get; }

        public bool IsSticky { get; }

        public bool IsTest { get; }

        public int MatchId { get; }

        public bool IsPublic { get; }

        public int CreatorId { get; }

        public int LastUpdatorId { get; }

        public string CreatorName { get; }

        public string LastUpdatorName { get; }

        public bool IsBatch { get; }

        public string BetTransId { get; }

        public int UserId { get; }

        public string UserName { get; }

        public long RowNum { get; }
    }
}