using Alpha.Main.Configurations;
using Fanex.Data.Repository;

namespace Alpha.Main.Criteria.Customer
{
    public class InsertPersonalMessageCommand : NonQueryCommand
    {
        public InsertPersonalMessageCommand(
            string custList,
            string english,
            string chineseSimplified = null,
            string chineseTraditional = null,
            string thai = null,
            string vietnamese = null)
        {
            CustList = custList;
            Content_en = english;
            Content_cs = chineseSimplified;
            Content_ch = chineseTraditional;
            Content_th = thai;
            Content_vn = vietnamese;
        }

        public string CustList { get; }

        public string Content_en { get; }

        public string Content_cs { get; }

        public string Content_ch { get; }

        public string Content_th { get; }

        public string Content_vn { get; }

        public bool IsTest => AppSettings.LocalTest;

        public override string GetSettingKey() => "Age_SetMessage_Personal";

        public override bool IsValid() => !string.IsNullOrWhiteSpace(CustList) && !string.IsNullOrWhiteSpace(Content_en);
    }
}