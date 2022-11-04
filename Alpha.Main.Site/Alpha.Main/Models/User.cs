namespace Alpha.Main.Models
{
    using Alpha.Main.Configurations;
    using Constants;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public interface IUser
    {
        bool IsEnabledJackpot { get; }

        bool ShowProgressiveJackpotGames { get; }

        bool IsAdminCanUpdateSubAccount { get; }

        void RemoveNewCaptchaByCurrency();

        bool IsEnabledTelegramOTP { get; set; }
        
        long TelegramOTPExpirationTime { get; set; }
    }

    public class User : IUser
    {
        private const string Jackpot = "Jackpot";
        private const string GameEnableSessionKey = "_GameEnable_";
        private const string IsAdminCanUpdateSubAccountSessionKey = "IsAdminCanUpdateSubAccount";
        private const string IsEnabledTelegramOTPSessionKey = "IsEnabledTelegramOTP";
        private const string TelegramOTPExpirationTimeSessionKey = " TelegramOTPExpirationTime";

        public bool ShowProgressiveJackpotGames => GetEnableProductsByGameName(Jackpot).Any(i => i != ProductId.Pragmatic);

        public bool IsEnabledJackpot => GetEnableProductsByGameName(Jackpot).Any();

        private static IList<int> GetEnableProductsByGameName(string gameName)
            => (IList<int>)HttpContext.Current.Session[HttpContext.Current.Session.SessionID + GameEnableSessionKey + gameName];

        public bool IsAdminCanUpdateSubAccount
            => GetSessionValue<bool>(IsAdminCanUpdateSubAccountSessionKey);

        public bool IsEnabledTelegramOTP
        {
            get
            {
                return GetSessionValue<bool>(IsEnabledTelegramOTPSessionKey);
            }
            set
            {
                SetSessionValue(IsEnabledTelegramOTPSessionKey, value);
            }
        }

        public long TelegramOTPExpirationTime
        {
            get
            {
                return GetSessionValue<long>(TelegramOTPExpirationTimeSessionKey);
            }
            set
            {
                SetSessionValue(TelegramOTPExpirationTimeSessionKey, value);
            }
        }

        public void RemoveNewCaptchaByCurrency()
        {
            HttpContext.Current.Session[AppSettings.CreatedSettingLastDateSessionKey] = DateTime.MinValue;
        }

        private static T GetSessionValue<T>(string sessionKey)
        {
            var value = GetSessionValue(sessionKey);

            return value == null ? default : (T)value;
        }

        private static object GetSessionValue(string sessionKey)
        {
            var session = HttpContext.Current.Session;

            return session[$"{session.SessionID}_{sessionKey}"];
        }

        private static void SetSessionValue(string sessionKey, object value)
        {
            var session = HttpContext.Current.Session;

            session[$"{session.SessionID}_{sessionKey}"] =value;
        }
    }
}
