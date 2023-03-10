namespace Alpha.Main.Site.Handlers
{
    using System;
    using System.Linq;
    using Alpha.Main.Constants;
    using Alpha.Main.Services.Impl;
    using MediatR;
    using Models.Requests;
    using Requests;
    using ViewModels;

    public class LanguageHandler :
        IRequestHandler<AllLanguagesQuery, LanguageViewModel>,
        IRequestHandler<TelegramBotLanguageQuery, LanguageViewModel>,
        IRequestHandler<ChangeLanguageCommand, string>
    {
        private readonly UpdateUsageLanguageService updateUsageLanguageService;

        public LanguageHandler(UpdateUsageLanguageService updateUsageLanguageService)
        {
            this.updateUsageLanguageService = updateUsageLanguageService;
        }

        public string Handle(ChangeLanguageCommand message)
        {
            var languageCode = Standardize(message.LanguageCode);
            var languageKey = message.ChangeLanguage(languageCode);

            if (message.IsUpdateDisplayLanguage)
            {
                updateUsageLanguageService.UpdateUsageLanguage(message.CustId, message.SubAccId, languageKey, message.IP);
            }

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
            => string.IsNullOrWhiteSpace(languageCode) || !Language.CodeNames.Keys.Contains(languageCode)
            ? Language.Default
            : languageCode;

        private string StandardizeTelegramBotCodeNames(string languageCode)
            => string.IsNullOrWhiteSpace(languageCode) || !Language.TelegramChatBotCodeNames.Keys.Contains(languageCode)
            ? Language.Default
            : languageCode;
    }
}