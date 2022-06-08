namespace Alpha.Main.Site.Requests
{
    using Alpha.Main.Site.ViewModels;
    using MediatR;
    using Sunplus.Agent.Model.Base;

    public class TelegramBotLanguageQuery : IRequest<LanguageViewModel>
    {
        private string languageCode;

        public TelegramBotLanguageQuery(string languageCode)
        {
            this.languageCode = languageCode ?? new UserSessionExt().Language;
        }

        public string LanguageCode
        {
            get
            {
                return languageCode;
            }
        }
    }
}