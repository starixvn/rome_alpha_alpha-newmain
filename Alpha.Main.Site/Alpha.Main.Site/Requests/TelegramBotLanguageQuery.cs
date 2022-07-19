namespace Alpha.Main.Site.Requests
{
    using Alpha.Main.Configurations;
    using Alpha.Main.Site.ViewModels;
    using MediatR;
    using Sunplus.Agent.Model.Base;

    public class TelegramBotLanguageQuery : IRequest<LanguageViewModel>
    {
        private string languageCode;

        public TelegramBotLanguageQuery(string languageCode, bool? disabledVietnamese)
        {
            this.languageCode = languageCode ?? new UserSessionExt().Language;
            DisabledVietnamese = disabledVietnamese ?? AppSettings.DisabledVietnamese;
        }

        public string LanguageCode
        {
            get
            {
                return languageCode;
            }
        }

        public bool DisabledVietnamese { get; }
    }
}