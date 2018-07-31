namespace Alpha.Main.Site.ViewModels.ChatBot
{
    using System.Collections.Generic;
    using Main.Models;

    public class CaptchaErrorResult : Result
    {
        private const int InvalidCaptchaCode = 2;

        public bool IsCaptchaInvalid => Code == InvalidCaptchaCode;

        public IDictionary<string, string> CaptchaInfo { get; set; }

        public static CaptchaErrorResult CreateErrorResult(string message, IDictionary<string, string> captchaInfo, int? code = null)
            => new CaptchaErrorResult
            {
                Code = code.HasValue ? code.Value : ErrorCode,
                Message = message,
                CaptchaInfo = captchaInfo
            };

        public static CaptchaErrorResult CreateInvalidCaptchaResult(IDictionary<string, string> captchaInfo)
            => CreateErrorResult(AppResources.ChatBot.InvalidCaptcha, captchaInfo, InvalidCaptchaCode);
    }
}