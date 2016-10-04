namespace Alpha.Main.Site.Filter
{
    using System.Web.Mvc;
    using Alpha.Auth.Web.Validators;
    using Main.Models;

    public class AntiXSRFFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var antiXSRFResult = AntiForgeryValidator.ValidateRequest();
            if (antiXSRFResult.ErrCode != 0)
            {
                ActionDenied(filterContext, antiXSRFResult);
            }

            base.OnActionExecuting(filterContext);
        }

        private static void ActionDenied(ActionExecutingContext filterContext, Alpha.Auth.Models.Result antiXSRFResult)
        {
            var request = filterContext.HttpContext.Request;
            if (request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new Result
                    {
                        Code = antiXSRFResult.ErrCode,
                        Messsage = antiXSRFResult.ErrMsg
                    }
                };
            }
            else
            {
                filterContext.Result = new ViewResult
                {
                    ViewName = "~/Areas/CustomerSetting/Views/Security/AccessDenied.cshtml"
                };
            }
        }
    }
}