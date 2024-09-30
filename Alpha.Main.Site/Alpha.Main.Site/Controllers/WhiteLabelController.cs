using Alpha.Main.Configurations;
using Alpha.Main.Site.ViewModels.WhiteLabel;
using Alpha.Web;
using AlphaPlus.User.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Alpha.Main.Site.Controllers
{
    [SessionState(SessionStateBehavior.Required)]
    public class WhiteLabelController : BaseController
    {
        private readonly IUserSession userSession;
        private readonly HttpClient httpClient;

        public WhiteLabelController(IUserSession userSession)
        {
            this.userSession = userSession;
            httpClient = new HttpClient
            {
                BaseAddress = AppSettings.WhiteLabelBackOfficeApiUri,
                Timeout = TimeSpan.FromSeconds(15)
            };
        }

        [HttpPost]
        public async Task<ActionResult> SSO()
        {
            using (var jsonContent = new StringContent(JsonConvert.SerializeObject(new
            {
                referenceId = "169659405",
                browserUserAgent = Request.UserAgent,
                userIp = userSession.UserLoginIp
            }),
            Encoding.UTF8,
            "application/json"))
            {
                using (var response = await httpClient.PostAsync("Signin", jsonContent))
                {
                    response.EnsureSuccessStatusCode();
                    var jsonResponse = await response.Content.ReadAsAsync<WhiteLabelSSOResponse>();
                    return Json(jsonResponse, JsonRequestBehavior.DenyGet);
                }
            }
        }
    }
}