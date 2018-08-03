namespace Alpha.Main.Site.ViewModels.ChatBot
{
    using System.Collections.Generic;
    using AppResources;
    using Main.Models;

    public class CaptchaErrorResult : Result
    {
        private const int InvalidCaptchaCode = 2;

        public bool IsCaptchaInvalid => Code == InvalidCaptchaCode;

        public IDictionary<string, string> CaptchaInfo { get; set; }

        public static CaptchaErrorResult CreateInvalidCaptchaResult(IDictionary<string, string> captchaInfo)
            => new CaptchaErrorResult
            {
                Code = InvalidCaptchaCode,
                Message = ChatBot.InvalidCaptcha,
                CaptchaInfo = captchaInfo
            };
    }
}