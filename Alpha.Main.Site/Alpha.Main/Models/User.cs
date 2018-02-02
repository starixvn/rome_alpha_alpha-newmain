namespace Alpha.Main.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public interface IUser
    {
        bool IsEnabledJackpot { get; }

        IList<int> GetEnableProductsForGame(string gameName);
    }

    public class User : IUser
    {
        private const string Jackpot = "Jackpot";
        private const string GameEnableSessionKey = "_GameEnable_";

        public bool IsEnabledJackpot => GetEnableProductsForGame(Jackpot).Any();

        public IList<int> GetEnableProductsForGame(string gameName)
            => (IList<int>)HttpContext.Current.Session[HttpContext.Current.Session.SessionID + GameEnableSessionKey + gameName];
    }
}