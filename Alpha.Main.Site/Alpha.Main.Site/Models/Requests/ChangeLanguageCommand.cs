namespace Alpha.Main.Site.Models.Requests
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

        public void ChangeLanguage(string languageCode)
        {
            userSession.Language = languageCode;
            userSession.LangKey = Language.CodeKeys[languageCode];            
        }
    }
}