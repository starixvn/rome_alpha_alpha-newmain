namespace Alpha.Main.Helpers
{
    using Fanex.Logging.Extensions.Web;
    using System;
    using System.Web;

    public class IPHelper
    {
        public static string CustomerIp()
        {
            var ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            if (string.IsNullOrWhiteSpace(ip))
            {
                ExceptionHandler.Handle(new Exception("After login: ip is null or empty"), System.Diagnostics.TraceEventType.Error);
            }

            var ips = string.IsNullOrWhiteSpace(ip) || ip.Equals("::1") ?
                new[] { "127.0.0.1" } : ip.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (string.IsNullOrWhiteSpace(ips[0]))
            {
                ExceptionHandler.Handle(new Exception("After login: HTTP_X_FORWARDED_FOR ip is null or empty"), System.Diagnostics.TraceEventType.Error);
            }

            return ips[0].Trim();
        }
    }
}
