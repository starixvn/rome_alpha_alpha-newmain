namespace Alpha.Main.Site.Controllers
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Alpha.Web;
    using Alpha.Web.Helpers;
    using Sunplus.Agent.Model.Entities;

    public abstract class ManualCheckProtectedModeController : BaseController
    {
        protected readonly IUserSession userSession;

        protected ManualCheckProtectedModeController(IUserSession userSession)
        {
            this.userSession = userSession;
        }

        protected bool IsNotProtectedModeInternal()
            => userSession.IsProtectedMode && !userSession.IsInternal;

        protected static ActionResult UnauthorizedAction(HttpContextBase httpContext)
        {
            var result = new RedirectResult(MicroSites.OldMainUrl(FormsAuthentication.LoginUrl));

            TerminateSession(httpContext.Session);

            return result;
        }

        protected static ActionResult UnauthorizedAjaxAction(
            HttpContextBase httpContext,
            ControllerContext controllerContext)
        {
            var result = new HttpUnauthorizedResult();
            result.ExecuteResult(controllerContext);

            var response = httpContext.Response;
            response.Clear();
            response.TrySkipIisCustomErrors = true;
            response.End();

            TerminateSession(httpContext.Session);

            return result;
        }

        private static void TerminateSession(HttpSessionStateBase session)
        {
            session.Clear();
            session.Abandon();

            if (!session.IsCookieless)
            {
                FormsAuthentication.SignOut();
            }
        }
    }
}