namespace Alpha.Main.Criteria.Message
{
    using System;
    using System.Collections.Generic;
    using Constants;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetMessageCriteria : CriteriaBase
    {
        private static readonly Dictionary<int, string> SettingKeys = new Dictionary<int, string>
        {
            [MessageType.Private] = "Dot_Msg_GetPrivateMessage",
            [MessageType.Public] = "Dot_Msg_GetPublicMessage",
            [MessageType.Special] = "Dot_Msg_GetSpecialMessage"
        };

        private readonly string settingKey;

        public GetMessageCriteria(
            int custId,
            string parentSites,
            int specialType,
            int totalMessage,
            DateTime startDate,
            DateTime endDate,
            bool isTest,
            int messageType)
        {
            settingKey = SettingKeys[messageType];
            CustId = custId;
            ParentSites = parentSites;
            Special = specialType;
            TotalMessasge = totalMessage;
            StartDate = startDate;
            EndDate = endDate;
            IsTest = isTest;
        }

        [SpParam(SqlParamName = "targetId")]
        public int CustId { get; }

        [SpParam(SqlParamName = "targetId2")]
        public string ParentSites { get; }

        public int Special { get; }

        [SpParam(SqlParamName = "record")]
        public int TotalMessasge { get; }

        [SpParam(SqlParamName = "sdate")]
        public DateTime StartDate { get; }

        [SpParam(SqlParamName = "edate")]
        public DateTime EndDate { get; }

        public bool IsTest { get; }

        public override string GetSettingKey() => settingKey;

        public override bool IsValid() => CustId > 0 && !string.IsNullOrEmpty(ParentSites);
    }
}