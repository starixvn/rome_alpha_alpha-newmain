namespace Alpha.Main.Sessions
{
    using System.Web;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class CookieLessSessionFactory
    {
        public static SecurityCodeSession SecurityCodeSession 
        {
            get
            {
                return new SecurityCodeSessionImpl(HttpContext.Current.Session.SessionID, null);
            }
        }
    }
}
