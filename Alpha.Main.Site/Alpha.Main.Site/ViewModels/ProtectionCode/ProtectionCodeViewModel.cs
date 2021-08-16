namespace Alpha.Main.Site.ViewModels.ProtectionCode
{
    using System.Linq;

    public class ProtectionCodeViewModel
    {
        public ProtectionCodeViewModel(
            int custId,
            string userLevel,
            string language,
            string hashKey,
            bool isShowNicknameLink,
            bool isExistedProtectionCode)
        {
            CustId = custId;
            UserLevel = userLevel;
            Language = language;
            HashKey = hashKey;
            IsShowNicknameLink = isShowNicknameLink;
            IsExistedProtectionCode = isExistedProtectionCode;
        }

        public int CustId { get; }

        public string UserLevel { get; }

        public string Language { get; }

        public string HashKey { get; }

        public bool IsShowNicknameLink { get; }

        public bool IsExistedProtectionCode { get; }

        public string GetLearnMoreViewLang()
            => new[] { "th-TH", "vi-VN", "zh-CN", "zh-TW" }.FirstOrDefault(l => l.Equals(Language)) ?? "en-US";
    }
}