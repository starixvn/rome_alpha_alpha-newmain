#pragma warning disable 618, 612

namespace Alpha.Main.Site.Controllers
{
    using Configurations;
    using Main.Helpers;
    using System;
    using System.Web.Mvc;
    using Web;

    public class RedirectController : BaseController
    {
        private static string GetAbsolutePath(string siteAppPath, string url) => $"{AppPathHelper.GetAppPath(siteAppPath, "~")}{url}"; 
      
        public ActionResult SiteMemberInfo(Uri url) => RedirectPermanent(GetAbsolutePath(AppSettings.MemberInfoAppPath, url.ToString()));
    }
}