namespace Alpha.Main.Site.Requests
{
    using Constants;
    using MediatR;
    using Sunplus.Agent.Model.Entities;

    public class ChangeLanguageCommand : IRequest<string>
    {
        private readonly IUserSession userSession;
        private readonly string languageCode;

        public ChangeLanguageCommand(IUserSession userSession, string languageCode)
        {
            this.userSession = userSession;
            this.languageCode = languageCode;
        }

        public string LanguageCode
        {
            get
            {
                return languageCode;
            }
        }

        public virtual void ChangeLanguage(string languageCode)
        {
            if (string.IsNullOrWhiteSpace(languageCode))
            {
                return;
            }

            userSession.Language = languageCode;
            userSession.LangKey = Language.CodeKeys[languageCode];
        }
    }
}