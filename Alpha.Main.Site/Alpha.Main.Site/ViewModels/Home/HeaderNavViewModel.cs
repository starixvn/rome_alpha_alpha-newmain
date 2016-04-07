namespace Alpha.Main.Site.ViewModels
{
    using Alpha.Main.Site.Helpers;

    public class HeaderNavViewModel
    {
        public int CustId { get; set; }

        public string OldMainRootPath
        {
            get
            {
                return MicroSites.OldMainUrl("~/");
            }
        }

        public string HowToUseRootPath
        {
            get
            {
                return MicroSites.HowToUseUrl("~/");
            }
        }
    }
}