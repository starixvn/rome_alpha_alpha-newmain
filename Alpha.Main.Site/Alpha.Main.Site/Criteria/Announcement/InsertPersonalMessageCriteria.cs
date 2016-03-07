namespace Alpha.Main.Site.Criteria.Announcement
{
    using Fanex.Data.Repository;

    public class InsertPersonalMessageCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "content_en")]
        public string ContentEn { get; set; }

        [SpParam(SqlParamName = "content_ch")]
        public string ContentCh { get; set; }

        [SpParam(SqlParamName = "content_cs")]
        public string ContentCs { get; set; }

        [SpParam(SqlParamName = "content_jp")]
        public string ContentJp { get; set; }

        [SpParam(SqlParamName = "content_it")]
        public string ContentIt { get; set; }

        [SpParam(SqlParamName = "content_th")]
        public string ContentTh { get; set; }

        [SpParam(SqlParamName = "content_ko")]
        public string ContentKo { get; set; }

        [SpParam(SqlParamName = "content_es")]
        public string ContentEs { get; set; }

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
    }
}