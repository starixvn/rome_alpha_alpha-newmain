#pragma warning disable 618, 612
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage(
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

            userSession.Language = languageCode;
            userSession.LangKey = Language.CodeKeys[languageCode];
        }
    }
}