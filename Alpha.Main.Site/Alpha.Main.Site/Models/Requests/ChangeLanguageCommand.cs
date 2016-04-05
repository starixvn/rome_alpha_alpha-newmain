namespace Alpha.Main.Site.Models.Requests
{
    using System;
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

        public Action<string> ChangeLanguage
        {
            get
            {
                return (code) => 
                {
                    userSession.Language = code;
                    userSession.LangKey = Language.CodeKeys[code];
                };
            }
        }
    }
}