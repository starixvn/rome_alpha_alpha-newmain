#pragma warning disable 612, 618

namespace Alpha.Main.Site.Requests
{
    using Alpha.Main.Constants;
    using MediatR;
    using Sunplus.Agent.Model.Entities;

    public class ChangeLanguageCommand : IRequest<string>
    {
        private readonly IUserSession userSession;
        private readonly bool disabledVietnamese;

        public ChangeLanguageCommand(
            IUserSession userSession,
            string languageCode,
            bool disabledVietnamese,
            bool isUpdateDisplayLanguage)
        {
            this.userSession = userSession;
            LanguageCode = languageCode;
            this.disabledVietnamese = disabledVietnamese;
            IsUpdateDisplayLanguage = isUpdateDisplayLanguage;
        }

        public string LanguageCode { get; }

        public bool IsUpdateDisplayLanguage { get; }

        public int CustId => userSession.CustId;

        public int SubAccId => userSession.SubAccId;

        public virtual string ChangeLanguage(string languageCode)
        {
            if (disabledVietnamese && languageCode == Language.Vietnamese)
            {
                languageCode = Language.English;
            }

            userSession.Language = languageCode;
            var languageKey = Language.CodeKeys[languageCode];
            userSession.LangKey = languageKey;

            return languageKey;
        }
    }
}