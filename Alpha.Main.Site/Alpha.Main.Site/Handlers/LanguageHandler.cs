namespace Alpha.Main.Site.Handlers
{
    using System.Linq;
    using Constants;
    using MediatR;
    using Models.Requests;
    using ViewModels;

    public class LanguageHandler :
        IRequestHandler<AllLanguagesQuery, LanguageViewModel>,
        IRequestHandler<ChangeLanguageCommand, string>
    {
        public string Handle(ChangeLanguageCommand message)
        {
            var languageCode = Standardize(message.LanguageCode);

            message.ChangeLanguage(languageCode);

            return languageCode;
        }

        public LanguageViewModel Handle(AllLanguagesQuery message)
        {
            var viewModel = new LanguageViewModel(Language.CodeNames, Standardize(message.LanguageCode));

            return viewModel;
        }

        private string Standardize(string languageCode)
        {
            if (string.IsNullOrWhiteSpace(languageCode)
              || !Language.CodeNames.Keys.Contains(languageCode))
            {
                languageCode = Language.Default;
            }

            return languageCode;
        }
    }
}