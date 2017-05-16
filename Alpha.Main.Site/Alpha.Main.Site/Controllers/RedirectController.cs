#pragma warning disable 618, 612

namespace Alpha.Main.Site.Controllers
{
    using System;
    using Configurations;
    using Main.Helpers;
    using Web;

    public class RedirectController : BaseController
    {
        private void Index(string siteAppPath, string url)
        {
            var absolutPath = $"{AppPathHelper.GetAppPath(siteAppPath, "~")}{url}";
            Response.Redirect(absolutPath, true);
        }

        public void SiteMemberInfo(Uri url) => Index(AppSettings.MemberInfoAppPath, url.ToString());
    }
}