namespace Alpha.Main.Site.Controllers
{
    using Alpha.Main.AppResources;
    using Main.Encryption;
    using Main.Models.SecurityCode;
    using Main.Services;
    using Main.Site.Filter;
    using SecurityCode.Entities;
    using SecurityCode.Services;
    using SecurityCode.Sessions;
    using Sunplus.Agent.Model.Entities;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Threading;
    using System.Web.Mvc;
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SecurityCodeController : BaseController
    {
        private readonly SecurityCodeService _securityCodeService;

        private readonly IUserSession userSession;
        private readonly ISecurityCodeSession securityCodeSession;

        public SecurityCodeController(SecurityCodeService securityCodeService, 
            IUserSession userSession,
            ISecurityCodeSession securityCodeSession)
        {
            this.userSession = userSession;
            this.securityCodeSession = securityCodeSession;
            _securityCodeService = securityCodeService;
        }

        private string PH
        {
            get
            {
                return System.Web.HttpContext.Current.Session["ph"] + string.Empty;
            }
        }

        protected override bool DisableAsyncSupport
        {
            get { return true; }
        }

        /// <summary>
        /// Invokes the action in the current controller context.
        /// </summary>
        protected override void ExecuteCore()
        {
            // Modify current thread's cultures
            Thread.CurrentThread.CurrentCulture = new CultureInfo(this.securityCodeSession.Language);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            base.ExecuteCore();
        }

        private string ClientIP
        {
            get
            {
                string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]
                    ?? System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                string[] ips = string.IsNullOrEmpty(ip) ? new string[] { string.Empty } : ip.Split(',');

                return ips[0];
            }
        }

        /// <summary>
        /// Forces the change security code.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpGet]
        public ActionResult ForceChangeSecurityCode()
        {
            string securityCodeToken = _securityCodeService.GenerateSecurityToken();
            this.securityCodeSession.SecurityCodeToken = securityCodeToken;
            ViewBag.SecurityCodeToken = securityCodeToken;
            ViewBag.SecuritySalt = this.securityCodeSession.SubAccId > 0
                ? _securityCodeService.MD5Salt(this.securityCodeSession.SubAccId)
                : _securityCodeService.MD5Salt(this.securityCodeSession.CustId);

            return View("~/Views/SecurityCode/ForceChangeSecurityCode.cshtml");
        }

        /// <summary>
        /// Forces the change security code without validation.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpGet]
        public ActionResult ForceChangeSecurityCodeWithoutValidation()
        {
            return View("~/Views/SecurityCode/ForceChangeSecurityCodeWithoutValidation.cshtml");
        }

        /// <summary>
        /// Verifies the security code.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpGet]
        public ActionResult VerifySecurityCode()
        {
            string securityCodeToken = _securityCodeService.GenerateSecurityToken();
            this.securityCodeSession.SecurityCodeToken = securityCodeToken;
            ViewBag.SecurityCodeToken = securityCodeToken;
            ViewBag.SecuritySalt = this.securityCodeSession.SubAccId > 0
                ? _securityCodeService.MD5Salt(this.securityCodeSession.SubAccId)
                : _securityCodeService.MD5Salt(this.securityCodeSession.CustId);

            return View("~/Views/SecurityCode/VerifySecurityCode.cshtml");
        }

        /// <summary>
        /// Changes the security code.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpGet]
        public ActionResult ChangeSecurityCode()
        {
            string redirectURL = SecurityCodeChecker.GetRedirectUrl();
            if (redirectURL != null)
            {
                return Redirect(redirectURL);
            }

            string securityCodeToken = _securityCodeService.GenerateSecurityToken();
            this.securityCodeSession.SecurityCodeToken = securityCodeToken;
            ViewBag.SecurityCodeToken = securityCodeToken;
            ViewBag.SecuritySalt = this.securityCodeSession.SubAccId > 0
                ? _securityCodeService.MD5Salt(this.securityCodeSession.SubAccId)
                : _securityCodeService.MD5Salt(this.securityCodeSession.CustId);
            ViewBag.Language = this.securityCodeSession.Language;
            ViewBag.VirtualDirectory = ConfigurationManager.AppSettings["VirtualDirectory"];

            return View("~/Views/SecurityCode/ChangeSecurityCode.cshtml");
        }

        /// <summary>
        /// Resets the security code.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpGet]
        public ActionResult ResetSecurityCode()
        {
            string redirectURL = SecurityCodeChecker.GetRedirectUrl();
            if (redirectURL != null)
            {
                return Redirect(redirectURL);
            }

            ViewBag.Language = this.securityCodeSession.Language;
            ViewBag.VirtualDirectory = ConfigurationManager.AppSettings["VirtualDirectory"];
            return View("~/Views/SecurityCode/ResetSecurityCode.cshtml");
        }

        [HttpGet]
        public ActionResult HowToUse()
        {
            string language = this.securityCodeSession.Language;
            return View("~/Views/SecurityCode/HowToUse/SecurityCode_" + language + ".cshtml");
        }

        /// <summary>
        /// Forces the change security code.
        /// </summary>
        /// <param name="oldSecurityCode">The old security code.</param>
        /// <param name="securityCode">The security code.</param>
        /// <param name="confirmSecurityCode">The confirm security code.</param>
        /// <returns>The JSON result.</returns>
        [HttpPost]
        [AntiXSRFFilter]
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public ActionResult ForceChangeSecurityCode(string oldSecurityCode, string securityCode, string confirmSecurityCode)
        {
            JsonRequestBehavior jsonBehavior = JsonRequestBehavior.DenyGet;
            if (this.securityCodeSession.IsInternal)
            {
                return Json(new Result { ErrorCode = -1, ErrorMessage = SecurityCode_Resources.NotAuthorized }, jsonBehavior);
            }

            securityCode = GetRC4Decryption(securityCode);
            confirmSecurityCode = GetRC4Decryption(confirmSecurityCode);

            //// validate request
            string message = string.Empty;
            if (string.IsNullOrEmpty(oldSecurityCode))
            {
                message = SecurityCode_Resources.alertOldSecCodeEmpty;
            }
            else if (string.IsNullOrEmpty(securityCode))
            {
                message = SecurityCode_Resources.alertSecCodeEmpty;
            }
            else if (!_securityCodeService.ValidateSecurityCode(securityCode))
            {
                message = SecurityCode_Resources.lblSecCodeWrongRule;
            }
            else if (securityCode != confirmSecurityCode)
            {
                message = SecurityCode_Resources.alertErrorSecCodeConfirm;
            }

            if (message.Length != 0)
            {
                return Json(new Result { ErrorCode = -1, ErrorMessage = message }, jsonBehavior);
            }

            Result result = _securityCodeService.ChangeSecurityCode(
                this.securityCodeSession.CustId,
                this.securityCodeSession.UserId,
                oldSecurityCode,
                securityCode,
                this.securityCodeSession.SecurityCodeToken,
                this.securityCodeSession.Language,
                ClientIP,
                this.securityCodeSession.SubAccId);
            if (result.ErrorCode == 0)
            {
                // reset random key
                this.securityCodeSession.SecurityCodeToken = string.Empty;
                this.securityCodeSession.SecurityCodeResult = (int)SecurityCodeStatus.PROVIDED_SC_SUCCESS;
            }

            return Json(result, jsonBehavior);
        }

        /// <summary>
        /// Forces the change security code without validation.
        /// </summary>
        /// <param name="securityCode">The security code.</param>
        /// <param name="confirmSecurityCode">The confirm security code.</param>
        /// <returns>The JSON result.</returns>
        [HttpPost]
        [AntiXSRFFilter]
        public ActionResult ForceChangeSecurityCodeWithoutValidation(string securityCode, string confirmSecurityCode)
        {
            JsonRequestBehavior jsonBehavior = JsonRequestBehavior.DenyGet;
            if (this.securityCodeSession.IsInternal)
            {
                return Json(new Result { ErrorCode = -1, ErrorMessage = SecurityCode_Resources.NotAuthorized }, jsonBehavior);
            }

            securityCode = GetRC4Decryption(securityCode);
            confirmSecurityCode = GetRC4Decryption(confirmSecurityCode);

            //// validate request
            string message = string.Empty;
            if (string.IsNullOrEmpty(securityCode))
            {
                message = SecurityCode_Resources.alertSecCodeEmpty;
            }
            else if (!_securityCodeService.ValidateSecurityCode(securityCode))
            {
                message = SecurityCode_Resources.lblSecCodeWrongRule;
            }
            else if (securityCode != confirmSecurityCode)
            {
                message = SecurityCode_Resources.alertErrorSecCodeConfirm;
            }

            if (message.Length != 0)
            {
                return Json(new Result { ErrorCode = -1, ErrorMessage = message }, jsonBehavior);
            }

            Result result = _securityCodeService.ChangeSecurityCodeWithoutValidation(
                this.securityCodeSession.CustId,
                this.securityCodeSession.UserId,
                securityCode,
                this.securityCodeSession.Language,
                ClientIP,
                this.securityCodeSession.SubAccId);
            if (result.ErrorCode == 0)
            {
                this.securityCodeSession.SecurityCodeResult = (int)SecurityCodeStatus.PROVIDED_SC_SUCCESS;
            }

            return Json(result, jsonBehavior);
        }

        /// <summary>
        /// Verifies the security code.
        /// </summary>
        /// <param name="securityCode">The security code.</param>
        /// <returns>The JSON result.</returns>
        [HttpPost]
        [AntiXSRFFilter]
        public ActionResult VerifySecurityCode(string securityCode)
        {
            JsonRequestBehavior jsonBehavior = JsonRequestBehavior.DenyGet;
            if (this.securityCodeSession.IsInternal)
            {
                return Json(new Result { ErrorCode = -1, ErrorMessage = SecurityCode_Resources.NotAuthorized }, jsonBehavior);
            }

            if (string.IsNullOrEmpty(securityCode))
            {
                return Json(new Result { ErrorCode = -1, ErrorMessage = SecurityCode_Resources.informSecCode }, jsonBehavior);
            }

            Result result = _securityCodeService.CheckSecurityCode(
                this.securityCodeSession.CustId,
                securityCode,
                this.securityCodeSession.SecurityCodeToken,
                this.securityCodeSession.Language,
                ClientIP,
                this.securityCodeSession.SubAccId);
            if (result.ErrorCode == 0)
            {
                // reset random key
                this.securityCodeSession.SecurityCodeToken = string.Empty;
                this.securityCodeSession.SecurityCodeResult = (int)SecurityCodeStatus.PROVIDED_SC_SUCCESS;
            }

            return Json(result, jsonBehavior);
        }

        /// <summary>
        /// Changes the security code.
        /// </summary>
        /// <param name="oldSecurityCode">The old security code.</param>
        /// <param name="securityCode">The security code.</param>
        /// <param name="confirmSecurityCode">The confirm security code.</param>
        /// <returns>The JSON result.</returns>
        [HttpPost]
        [AntiXSRFFilter]
        public ActionResult ChangeSecurityCode(string oldSecurityCode, string securityCode, string confirmSecurityCode)
        {
            JsonRequestBehavior jsonBehavior = JsonRequestBehavior.DenyGet;
            if (this.securityCodeSession.IsInternal)
            {
                return Json(new Result { ErrorCode = -1, ErrorMessage = SecurityCode_Resources.NotAuthorized }, jsonBehavior); 
            }

            securityCode = GetRC4Decryption(securityCode);
            confirmSecurityCode = GetRC4Decryption(confirmSecurityCode);

            //// validate request
            string message = string.Empty;
            if (string.IsNullOrEmpty(oldSecurityCode))
            {
                message = SecurityCode_Resources.alertOldSecCodeEmpty;
            }
            else if (string.IsNullOrEmpty(securityCode))
            {
                message = SecurityCode_Resources.alertSecCodeEmpty;
            }
            else if (!_securityCodeService.ValidateSecurityCode(securityCode))
            {
                message = SecurityCode_Resources.lblSecCodeWrongRule;
            }
            else if (securityCode != confirmSecurityCode)
            {
                message = SecurityCode_Resources.alertErrorSecCodeConfirm;
            }

            if (message.Length != 0)
            {
                return Json(new Result { ErrorCode = -1, ErrorMessage = message }, jsonBehavior);
            }

            Result result = _securityCodeService.ChangeSecurityCode(
                this.securityCodeSession.CustId,
                this.securityCodeSession.UserId,
                oldSecurityCode,
                securityCode,
                this.securityCodeSession.SecurityCodeToken,
                this.securityCodeSession.Language,
                ClientIP,
                this.securityCodeSession.SubAccId);
            if (result.ErrorCode == 0)
            {
                this.securityCodeSession.SecurityCodeToken = string.Empty;
            }

            return Json(result, jsonBehavior);
        }

        /// <summary>
        /// Resets the security code.
        /// </summary>
        /// <param name="custId">The customer id.</param>
        /// <param name="subAccId">The sub account id.</param>
        /// <param name="securityCode">The security code.</param>
        /// <param name="confirmSecurityCode">The confirm security code.</param>
        /// <returns>The JSON result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        [HttpPost]
        [AntiXSRFFilter]
        public ActionResult ResetSecurityCode(int? custId, int? subAccId, string securityCode, string confirmSecurityCode)
        {
            JsonRequestBehavior jsonBehavior = JsonRequestBehavior.DenyGet;
            bool isResetSubAccount = subAccId.HasValue && subAccId.Value > 0;
            if (this.securityCodeSession.IsInternal && !isResetSubAccount)
            {
                return Json(new Result { ErrorCode = -1, ErrorMessage = SecurityCode_Resources.NotAuthorized }, jsonBehavior);
            }

            securityCode = GetRC4Decryption(securityCode);
            confirmSecurityCode = GetRC4Decryption(confirmSecurityCode);
            
            //// validate request
            string message = string.Empty;
            if (string.IsNullOrEmpty(securityCode))
            {
                message = SecurityCode_Resources.alertSecCodeEmpty;
            }
            else if (!_securityCodeService.ValidateSecurityCode(securityCode))
            {
                message = SecurityCode_Resources.lblSecCodeWrongRule;
            }
            else if (securityCode != confirmSecurityCode)
            {
                message = SecurityCode_Resources.alertErrorSecCodeConfirm;
            }

            if (message.Length != 0)
            {
                return Json(new Result { ErrorCode = -1, ErrorMessage = message }, jsonBehavior);
            }

            Result result = null;
            if (custId.HasValue && custId.Value > 0)
            {
                result = _securityCodeService.ResetSecurityCode(
                    custId.Value,
                    this.securityCodeSession.UserId,
                    this.securityCodeSession.SubAccId,
                    securityCode,
                    this.securityCodeSession.Language,
                    ClientIP,
                    this.securityCodeSession.RoleId == (byte)UserRole.Agent);
            }
            else if (isResetSubAccount)
            {
                result = _securityCodeService.ResetSecurityCodeOfSubAccount(
                    this.securityCodeSession.CustId,
                    this.securityCodeSession.UserId,
                    subAccId.Value,
                    securityCode,
                    this.securityCodeSession.Language,
                    ClientIP);
            }

            return Json(result, jsonBehavior);
        }

        #region helper methods

        /// <summary>
        /// Gets the R c4 decryption.
        /// </summary>
        /// <param name="encryption">The encryption.</param>
        /// <returns>The encryption string.</returns>
        private string GetRC4Decryption(string encryption)
        {
            if (string.IsNullOrEmpty(encryption))
            {
                return string.Empty;
            }

            return Encryptor.RC4(this.PH, Encryptor.HexDecode(encryption));
        }

        #endregion
    }
}