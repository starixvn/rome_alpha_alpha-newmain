namespace Alpha.Main.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Constants;
    using Sunplus.Agent.Model.Entities;

    public interface IUser
    {
        bool IsEnabledJackpot { get; }

        bool IsAdminCanUpdateSubAccount { get; }

        void SetNewCaptchaByCurrency();
    }

    public class User : IUser
    {
        private const string Jackpot = "Jackpot";
        private const string GameEnableSessionKey = "_GameEnable_";
        private const string IsAdminCanUpdateSubAccountSessionKey = "IsAdminCanUpdateSubAccount";
        private const string NewCaptchaByCurrencySessionKey = "NewCaptchaByCurrency";

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

        public void SetNewCaptchaByCurrency()
        {
            HttpContext.Current.Session[NewCaptchaByCurrencySessionKey] = false;
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
