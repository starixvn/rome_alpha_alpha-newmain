namespace Alpha.Main.Site.Handlers
{
    using System;
    using System.Linq;
    using Alpha.Main.Constants;
    using MediatR;
    using Models.Requests;
    using Requests;
    using ViewModels;

    public class LanguageHandler :
        IRequestHandler<AllLanguagesQuery, LanguageViewModel>,
        IRequestHandler<TelegramBotLanguageQuery, LanguageViewModel>,
        IRequestHandler<ChangeLanguageCommand, string>
    {
        public string Handle(ChangeLanguageCommand message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Command cannot be null");
            }

            var languageCode = Standardize(message.LanguageCode);

            message.ChangeLanguage(languageCode);

            return languageCode;
        }

        public LanguageViewModel Handle(AllLanguagesQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            var viewModel = new LanguageViewModel(
                message.DisabledVietnamese ? Language.CodeNamesWithoutVietNamese : Language.CodeNames,
                Standardize(message.LanguageCode));

            return viewModel;
        }

        public LanguageViewModel Handle(TelegramBotLanguageQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            var viewModel = new LanguageViewModel(
                message.DisabledVietnamese ? Language.TelegramChatBotCodeNamesWithoutVietNamese :  Language.TelegramChatBotCodeNames,
                StandardizeTelegramBotCodeNames(message.LanguageCode));

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

        private string StandardizeTelegramBotCodeNames(string languageCode)
        {
            if (string.IsNullOrWhiteSpace(languageCode)
             || !Language.TelegramChatBotCodeNames.Keys.Contains(languageCode))
            {
                languageCode = Language.Default;
            }

            return languageCode;
        }
    }
}