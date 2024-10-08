using Alpha.Main.Models;
using Alpha.Main.Services;
using Alpha.Web;
using AlphaPlus.User.Models;
using NovaCasino.API.Client;
using NovaCasino.API.Client.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Alpha.Main.Site.Controllers
{
    public class NovaCasinoController : BaseController
    {
        private readonly IUser user;
        private readonly IUserSession userSession;
        private readonly CustomerService customerService;
        private readonly INovaCasinoService novaCasinoService;

        public NovaCasinoController(
            IUser user,
            IUserSession userSession,
            CustomerService customerService,
            INovaCasinoService novaCasinoService)
        {
            this.user = user;
            this.userSession = userSession;
            this.customerService = customerService;
            this.novaCasinoService = novaCasinoService;
        }

        [HttpPost]
        public async Task<ActionResult> SSO()
        {
            var uplines = await customerService.GetUplines(userSession.CustId);
            var request = new NovaCasinoSSORequest
            {
                UserInfo = new UserInfo
                {
                    CustId = userSession.CustId,
                    Username = userSession.UserName,
                    CurrencyId = userSession.CurrencyId,
                    RoleId = userSession.RoleId,
                    Upline = uplines.ToArray()
                }
            };
            var response = await novaCasinoService.SSO(request);

            if (response.CustIds != null && response.CustIds.Any())
            {
                await customerService.UpdateSyncedNovaCasino(response.CustIds, userSession.UserLoginIp);
            }

            if (!user.IsSyncedNovaCasino)
            {
                user.IsSyncedNovaCasino = true;
                await customerService.InsertPersonalMessage(request.UserInfo);
            }

            return Json(new { response.LaunchUrl }, JsonRequestBehavior.DenyGet);
        }
    }
}