namespace Alpha.Main.Site.Filters
{
    using System;
    using System.Web.Mvc;
    using Configurations;
    using Fanex.Logging.Extensions;
    using Newtonsoft.Json;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ChatbotLoggingFilterAttribute : ActionFilterAttribute
    {
        private readonly string actionName;

        public ChatbotLoggingFilterAttribute(string actionName)
        {
            this.actionName = actionName;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var logMessage = new LogMessage(
                string.Join(
                    "\r\n",
                    $"Action Id: {filterContext.ActionDescriptor.UniqueId}",
                    $"Action Name: {actionName}",
                    $"Params: {JsonConvert.SerializeObject(filterContext.ActionParameters["command"])}"))
                    .ToString();

            Fanex.Logging.LogService.Info(logMessage, AppSettings.ChatbotLogCategory);

            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var result = filterContext.Result as JsonResult;
            if (result != null)
            {
                var logMessage = new LogMessage(
                string.Join(
                    "\r\n",
                    $"Action Id: {filterContext.ActionDescriptor.UniqueId}",
                    $"Action Name: {actionName}",
                    $"Result: {JsonConvert.SerializeObject(result.Data)}"))
                    .ToString();

                Fanex.Logging.LogService.Info(logMessage, AppSettings.ChatbotLogCategory);
            }

            base.OnActionExecuted(filterContext);
        }
    }
}