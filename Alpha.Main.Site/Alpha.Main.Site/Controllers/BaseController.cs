namespace Alpha.Main.Site.Controllers
{
    using System.Globalization;
    using System.Text;
    using System.Threading;
    using System.Web.Mvc;
    using System.Web.Mvc.Filters;
    using System.Web.Routing;
    using System.Web.Security;
    using Constants;
    using Sunplus.Agent.Model.Entities;
    using Utils;

    public class BaseController : Controller
    {
        public CultureInfo DefaultCulture = CultureInfo.CreateSpecificCulture(LanguageConstant.DefaultLanguage);

        protected override void OnAuthentication(AuthenticationContext filterContext)
        {
            base.OnAuthentication(filterContext);

            if (UserSession.Current.CustId == 0)
            {
                Response.Redirect(FormsAuthentication.LoginUrl, true);

                return;
            }
        }

        protected override JsonResult Json(
            object data,
            string contentType,
            Encoding contentEncoding,
            JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = int.MaxValue
            };
        }

        protected override void Initialize(RequestContext requestContext)
        {
            var cultureName = UserSession.Current.Language;

            if (string.IsNullOrEmpty(cultureName))
            {
                cultureName = LanguageConstant.DefaultLanguage;
                UserSession.Current.Language = cultureName;
            }

            // Modify current thread's cultures
            Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            Thread.CurrentThread.CurrentCulture.DateTimeFormat = DefaultCulture.DateTimeFormat;
            Thread.CurrentThread.CurrentCulture.NumberFormat = DefaultCulture.NumberFormat;

            base.Initialize(requestContext);
        }
    }
}