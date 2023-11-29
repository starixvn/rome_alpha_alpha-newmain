namespace Alpha.Main.Site.Requests
{
    using Alpha.Main.Configurations;
    using Alpha.Main.Site.ViewModels;
    using AlphaPlus.User.Models;
    using MediatR;

    public class TelegramBotLanguageQuery : IRequest<LanguageViewModel>
    {
        private readonly string languageCode;

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