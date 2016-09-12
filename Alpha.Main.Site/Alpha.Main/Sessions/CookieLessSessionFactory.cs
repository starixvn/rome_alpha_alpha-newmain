namespace Alpha.Main.Sessions
{
    using System.Web;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class CookieLessSessionFactory
    {
        public static ISecurityCodeSession SecurityCodeSession 
        {
            get
            {
                return new SecurityCodeSession(HttpContext.Current.Session.SessionID);
            }
        }
    }
}
