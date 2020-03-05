namespace Alpha.Main.Models
{
    using Alpha.Main.Configurations;
    using Constants;
    using Sunplus.Agent.Model.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public interface IUser
    {
        bool IsEnabledJackpot { get; }

        bool IsAdminCanUpdateSubAccount { get; }

        void RemoveNewCaptchaByCurrency();
    }

    public class User : IUser
    {
        private const string Jackpot = "Jackpot";
        private const string GameEnableSessionKey = "_GameEnable_";
        private const string IsAdminCanUpdateSubAccountSessionKey = "IsAdminCanUpdateSubAccount";

        public bool IsEnabledJackpot
        {
            get
            {
                var products = GetEnableProductsByGameName(Jackpot);

                return products.Count == 1 && products.Contains(ProductId.RNGCasino)
                    ? UserSession.Current.IsCasinoSync
                    : products.Any();
            }
        }

        private static IList<int> GetEnableProductsByGameName(string gameName)
            => (IList<int>)HttpContext.Current.Session[HttpContext.Current.Session.SessionID + GameEnableSessionKey + gameName];

        public bool IsAdminCanUpdateSubAccount
            => GetSessionValue<bool>(IsAdminCanUpdateSubAccountSessionKey);

        public void RemoveNewCaptchaByCurrency()
        {
            HttpContext.Current.Session[AppSettings.CreatedSettingLastDateSessionKey] = DateTime.MinValue;
        }

        private static T GetSessionValue<T>(string sessionKey)
        {
            var value = GetSessionValue(sessionKey);

            return value == null ? default(T) : (T)value;
        }

        private static object GetSessionValue(string sessionKey)
        {
            var session = HttpContext.Current.Session;

            return session[$"{session.SessionID}_{sessionKey}"];
        }
    }
}
