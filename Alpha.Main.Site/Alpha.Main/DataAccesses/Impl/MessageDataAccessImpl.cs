namespace Alpha.Main.DataAccesses.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Constants;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Models.Message;

    public class MessageDataAccessImpl : MessageDataAccess
    {
        public List<Message> GetMessages(ICriteria criteria, int messageType)
        {
            var messages = new List<Message>();

            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
            }

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var reader = db.ExecuteReader(criteria.GetSpParams());

                while (reader.Read())
                {
                    var message = GetMessageFromReader(reader, messageType);
                    messages.Add(message);
                }
            }

            return messages;
        }

        private Message GetMessageFromReader(IDataReader reader, int messageType)
        {
            var message = new Message(
                       id: Convert.ToInt32(reader["MessageId"] ?? 0),
                       contentEN: Convert.ToString(reader["Content_en"] ?? string.Empty),
                       contentCS: Convert.ToString(reader["Content_cs"] ?? string.Empty),
                       contentCH: Convert.ToString(reader["Content_ch"] ?? string.Empty),
                       contentJP: Convert.ToString(reader["Content_jp"] ?? string.Empty),
                       contentTH: Convert.ToString(reader["Content_th"] ?? string.Empty),
                       contentIT: Convert.ToString(reader["Content_it"] ?? string.Empty),
                       contentKO: Convert.ToString(reader["Content_ko"] ?? string.Empty),
                       contentES: Convert.ToString(reader["Content_es"] ?? string.Empty),
                       contentVN: messageType == MessageType.Private ? Convert.ToString(reader["Content_vn"] ?? string.Empty) : string.Empty,
                       postTime: Convert.ToDateTime(reader["PostTime"] ?? DateTime.Today),
                       createTime: Convert.ToDateTime(reader["CreateTime"] ?? DateTime.Today),
                       lastUpdateTime: Convert.ToDateTime(reader["LastUpdateTime"] ?? DateTime.Today),
                       messageType: Convert.ToByte(reader["MsgType"] ?? DateTime.Today),
                       isSticky: Convert.ToBoolean(reader["IsSticky"] ?? false),
                       isTest: Convert.ToBoolean(reader["IsTest"] ?? false),
                       matchId: Convert.ToInt32(reader["MatchId"] ?? 0),
                       isBatch: Convert.ToBoolean(reader["IsBatch"] ?? false),
                       creator: Convert.ToInt32(reader["Creator"] ?? 0),
                       lastUpdator: Convert.ToInt32(reader["LastUpdator"] ?? 0),
                       creatorName: Convert.ToString(reader["CreatorName"] ?? string.Empty),
                       lastUpdateName: Convert.ToString(reader["LastUpdateName"] ?? string.Empty),
                       userName: Convert.ToString(reader["UserName"] ?? string.Empty),
                       bettransId: Convert.ToString(reader["BettransID"] ?? string.Empty),
                       userId: Convert.ToInt32(reader["TargetId"] ?? 0),
                       isPublic: messageType == MessageType.Public ? Convert.ToBoolean(reader["IsPublic"] ?? false) : false,
                       rowNum: Convert.ToInt32(reader["rownum"] ?? 0));

            return message;
        }
    }
}