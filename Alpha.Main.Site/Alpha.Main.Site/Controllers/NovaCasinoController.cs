using Alpha.Main.AppResources;
using Alpha.Main.Models;
using Alpha.Main.Models.NovaCasino;
using Alpha.Main.Services;
using Alpha.Main.Site.ViewModels.NovaCasino;
using Alpha.Web;
using Alpha.Web.Models;
using AlphaPlus.User.Models;
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
            if (userSession.IsInternal || userSession.IsSubAcc)
            {
                return Json(Main.Models.Result.CreateErrorResult(Security.NotAuthorized), JsonRequestBehavior.DenyGet);
            }

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
            if (response.IsFailed)
            {
                return Json(response, JsonRequestBehavior.DenyGet);
            }

            if (response.CustIds != null && response.CustIds.Any())
            {
                await customerService.UpdateSyncedNovaCasino(response.CustIds, userSession.UserLoginIp, userSession.LangKey);
            }

            if (!user.IsSyncedNovaCasino && !Enumeration.FromValue<UserLevel>(userSession.RoleId).IsSuperLevel())
            {
                user.IsSyncedNovaCasino = true;
                await customerService.InsertPersonalMessage(request.UserInfo);
            }

            return Json(new NovaCasinoSSOViewModel { Code = 0, LaunchUrl = response.LaunchUrl }, JsonRequestBehavior.DenyGet);
        }
    }
}