namespace Alpha.NewMain.Site.Utils
{
    using System.Globalization;
    using System.Threading;
    using Extensions;

    public static class LanguageUtil
    {
        public static void SetLanguage()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(SessionExtension.Language);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }
    }
}