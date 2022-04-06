#pragma warning disable 612, 618

namespace Alpha.Main.Site.Requests
{
    using System.Diagnostics.CodeAnalysis;
    using Alpha.Main.Constants;
    using MediatR;
    using Sunplus.Agent.Model.Entities;

    public class ChangeLanguageCommand : IRequest<string>
    {
        private readonly IUserSession userSession;
        private readonly bool disabledVietnamese;

        public ChangeLanguageCommand(IUserSession userSession, string languageCode, bool disabledVietnamese)
        {
            this.userSession = userSession;
            LanguageCode = languageCode;
            this.disabledVietnamese = disabledVietnamese;
        }

        public string LanguageCode { get; }

        [SuppressMessage(
            "Microsoft.Maintainability",
            "CA1500:VariableNamesShouldNotMatchFieldNames",
            MessageId = "languageCode",
            Justification = "Reviewed!")]
        public virtual void ChangeLanguage(string languageCode)
        {
            if (string.IsNullOrWhiteSpace(languageCode) ||
                !Language.CodeKeys.ContainsKey(languageCode))
            {
                return;
            }

            if (this.disabledVietnamese && languageCode == Language.Vietnamese)
            {
                languageCode = Language.English;
            }

            userSession.Language = languageCode;
            userSession.LangKey = Language.CodeKeys[languageCode];
        }
    }
}