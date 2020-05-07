namespace Alpha.Main.Criteria.Banner
{
    using Fanex.Data.Repository;

    public class GetBannerCriteria : CriteriaBase
    {
        private const byte Desktop = 1;
        private const byte Mobile = 2;

        public GetBannerCriteria(
            string languageCode,
            string environments,
            string systems,
            byte type)
        {
            LanguageCode = languageCode;
            Environments = environments;
            Systems = systems;
            Type = type;
        }

        public string LanguageCode { get; }

        public string Environments { get; }

        public string Systems { get; }

        public byte Type { get; }

        public override string GetSettingKey()
            => "Age_Banner_SelectRandomBanner";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(LanguageCode)
            && !string.IsNullOrWhiteSpace(Environments)
            && !string.IsNullOrWhiteSpace(Systems)
            && Type == Desktop || Type == Mobile;
    }
}
