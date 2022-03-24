namespace Alpha.Main.Site.Handlers
{
    using Alpha.Main.Models;
    using Alpha.Main.Site.Helpers;
    using Alpha.Main.Site.Requests.TelegramChatBot;
    using Alpha.Main.Site.ViewModels.TelegramChatBot;
    using MediatR;
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public class TelegramChatBotHandler : 
        IAsyncRequestHandler<GenerateQRCodeCommand, VerifyAccountViewModel>,
        IAsyncRequestHandler<VerifyOTPCommand, Result>,
        IAsyncRequestHandler<ResendOTPCommand, Result>
    {
        public async Task<VerifyAccountViewModel> Handle(GenerateQRCodeCommand message)
        {
            // Call API generate url with token

            var url = "https://www.telegram.org/";
            var logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Assets/styles/telegramchatbot/images/telegram-sm.png");
            var qrCodeDataUrl = QRCodeAdapter.Generate(url, logoPath);

            return await Task.FromResult(new VerifyAccountViewModel(url, qrCodeDataUrl, true));
        }

        public async Task<Result> Handle(VerifyOTPCommand message)
        {
            // Call API verify OTP

            return await Task.FromResult(new Result { Code = 0 });
        }

        public async Task<Result> Handle(ResendOTPCommand message)
        {
            // Call API resend OTP

            return await Task.FromResult(new Result { Code = 0 });
        }
    }
}