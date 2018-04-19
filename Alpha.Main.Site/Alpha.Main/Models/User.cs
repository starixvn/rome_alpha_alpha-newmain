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
    }

    public class User : IUser
    {
        private const string Jackpot = "Jackpot";
        private const string GameEnableSessionKey = "_GameEnable_";

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
    }
}
