namespace Alpha.Main.DataAccesses.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
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
                       id: !Convert.IsDBNull(reader["MessageId"]) ? Convert.ToInt32(reader["MessageId"]) : 0,
                       contentEN: !Convert.IsDBNull(reader["Content_en"]) ? Convert.ToString(reader["Content_en"]) : string.Empty,
                       contentCS: !Convert.IsDBNull(reader["Content_cs"]) ? Convert.ToString(reader["Content_cs"]) : string.Empty,
                       contentCH: !Convert.IsDBNull(reader["Content_ch"]) ? Convert.ToString(reader["Content_ch"]) : string.Empty,
                       contentJP: !Convert.IsDBNull(reader["Content_jp"]) ? Convert.ToString(reader["Content_jp"]) : string.Empty,
                       contentTH: !Convert.IsDBNull(reader["Content_th"]) ? Convert.ToString(reader["Content_th"]) : string.Empty,
                       contentIT: !Convert.IsDBNull(reader["Content_it"]) ? Convert.ToString(reader["Content_it"]) : string.Empty,
                       contentKO: !Convert.IsDBNull(reader["Content_ko"]) ? Convert.ToString(reader["Content_ko"]) : string.Empty,
                       contentES: !Convert.IsDBNull(reader["Content_es"]) ? Convert.ToString(reader["Content_es"]) : string.Empty,
                       contentVN: !Convert.IsDBNull(reader["Content_vn"]) ? Convert.ToString(reader["Content_vn"]) : string.Empty,
                       postTime: !Convert.IsDBNull(reader["PostTime"]) ? Convert.ToDateTime(reader["PostTime"]) : DateTime.Today,
                       createTime: !Convert.IsDBNull(reader["CreateTime"]) ? Convert.ToDateTime(reader["CreateTime"]) : DateTime.Today,
                       lastUpdateTime: !Convert.IsDBNull(reader["LastUpdateTime"]) ? Convert.ToDateTime(reader["LastUpdateTime"]) : DateTime.Today,
                       messageType: !Convert.IsDBNull(reader["MsgType"]) ? Convert.ToByte(reader["MsgType"]) : (byte)0,
                       isSticky: !Convert.IsDBNull(reader["IsSticky"]) ? Convert.ToBoolean(reader["IsSticky"]) : false,
                       isTest: !Convert.IsDBNull(reader["IsTest"]) ? Convert.ToBoolean(reader["IsTest"]) : false,
                       matchId: !Convert.IsDBNull(reader["MatchId"]) ? Convert.ToInt32(reader["MatchId"]) : 0,
                       isBatch: !Convert.IsDBNull(reader["IsBatch"]) ? Convert.ToBoolean(reader["IsBatch"]) : false,
                       creator: !Convert.IsDBNull(reader["Creator"]) ? Convert.ToInt32(reader["Creator"]) : 0,
                       lastUpdator: !Convert.IsDBNull(reader["LastUpdator"]) ? Convert.ToInt32(reader["LastUpdator"]) : 0,
                       creatorName: !Convert.IsDBNull(reader["CreatorName"]) ? Convert.ToString(reader["CreatorName"]) : string.Empty,
                       lastUpdateName: !Convert.IsDBNull(reader["LastUpdateName"]) ? Convert.ToString(reader["LastUpdateName"]) : string.Empty,
                       userName: !Convert.IsDBNull(reader["UserName"]) ? Convert.ToString(reader["UserName"]) : string.Empty,
                       bettransId: !Convert.IsDBNull(reader["BettransID"]) ? Convert.ToString(reader["BettransID"]) : string.Empty,
                       userId: !Convert.IsDBNull(reader["TargetId"]) ? Convert.ToInt32(reader["TargetId"]) : 0,
                       rowNum: !Convert.IsDBNull(reader["rownum"]) ? Convert.ToInt32(reader["rownum"]) : 0);

            return message;
        }
    }
}