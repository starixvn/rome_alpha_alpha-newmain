namespace Alpha.Main.Site.Handlers
{
    using System.Linq;
    using Constants;
    using MediatR;
    using Models.Requests;
    using Sunplus.Agent.Model.Entities;
    using ViewModels;

    public class LanguageHandler :
        IRequestHandler<EmptyQuery, LanguageViewModel>,
        IRequestHandler<ChangeLanguageQuery, string>
    {
        private readonly IUserSession userSession;

        public LanguageHandler(IUserSession userSession)
        {
            this.userSession = userSession;
        }

        public string Handle(ChangeLanguageQuery message)
        {
            var languageCode = message.Language;

            if (string.IsNullOrWhiteSpace(languageCode)
              || !Language.CodeNames.Keys.Contains(languageCode))
            {
                languageCode = Language.Default;
            }

            userSession.Language = languageCode;
            userSession.LangKey = Language.CodeKeys[languageCode];

            return languageCode;
        }

        public LanguageViewModel Handle(EmptyQuery message)
        {
            var viewModel = new LanguageViewModel(Language.CodeNames, userSession.Language);

            return viewModel;
        }
    }
}