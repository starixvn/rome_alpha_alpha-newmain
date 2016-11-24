#pragma warning disable 612, 618
namespace Alpha.Main.Site.Requests
{
    using System.Diagnostics.CodeAnalysis;
    using Constants;
    using MediatR;
    using Sunplus.Agent.Model.Entities;

    public class ChangeLanguageCommand : IRequest<string>
    {
        private readonly IUserSession userSession;

        public ChangeLanguageCommand(IUserSession userSession, string languageCode)
        {
            this.userSession = userSession;
            LanguageCode = languageCode;
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

            userSession.Language = languageCode;
            userSession.LangKey = Language.CodeKeys[languageCode];
        }
    }
}