namespace Alpha.Main.Site.ViewModels.Advertisement
{
    using Alpha.Main.Configurations;
    using Alpha.Web.Helpers;

    public class MyWatchlistViewModel
    {
        public string UserGuideLink => AppSettings.IsMBCSite
            ? MicroSites.HowToUseUrl("~/Index.aspx?linktomessagesite=170467")
            : MicroSites.MessageUrl("~/StaticMessage/Guide?id=170467");
    }
}