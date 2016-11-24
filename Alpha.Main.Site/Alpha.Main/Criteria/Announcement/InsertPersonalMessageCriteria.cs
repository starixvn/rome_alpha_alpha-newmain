namespace Alpha.Main.Criteria.Announcement
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class InsertPersonalMessageCriteria : CriteriaBase
    {
        public InsertPersonalMessageCriteria()
        {
        }

        public InsertPersonalMessageCriteria(string content, int type)
        {
            ContentEN = content;
            ContentCH = content;
            ContentCS = content;
            ContentJP = content;
            ContentIT = string.Empty;
            ContentTH = content;
            ContentKO = content;
            ContentES = string.Empty;
            Type = type;
            IsSticky = false;
            MatchId = 0;
            IsBatch = true;

            Creator = 0;
            IsTest = false;
            CreatorName = string.Empty;
        }

        [SpParam(SqlParamName = "content_en")]
        public string ContentEN { get; set; }

        [SpParam(SqlParamName = "content_ch")]
        public string ContentCH { get; set; }

        [SpParam(SqlParamName = "content_cs")]
        public string ContentCS { get; set; }

        [SpParam(SqlParamName = "content_jp")]
        public string ContentJP { get; set; }

        [SpParam(SqlParamName = "content_it")]
        public string ContentIT { get; set; }

        [SpParam(SqlParamName = "content_th")]
        public string ContentTH { get; set; }

        [SpParam(SqlParamName = "content_ko")]
        public string ContentKO { get; set; }

        [SpParam(SqlParamName = "content_es")]
        public string ContentES { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Justification = "Reviewed!")]
        [SpParam(SqlParamName = "msgtype")]
        public int Type { get; set; }

        public bool IsSticky { get; set; }

        public int Creator { get; set; }

        public string CreatorName { get; set; }

        public bool IsTest { get; set; }

        public int MatchId { get; set; }

        public bool IsBatch { get; set; }

        public override string GetSettingKey()
        {
            return "Dot_Msg_SetMessage";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}