namespace Alpha.Main.Site.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    public class PasswordController : BaseController
    {
        // GET: Password
        public ActionResult ChangePassword()
        {
            return View();
        }

        public ActionResult ForceChangePassword()
        {
            return View();
        }

        public ActionResult ResetMainAccountPassword()
        {
            return View();
        }

        public ActionResult ResetSubAccountPassword()
        {
            return View();
        }
    }
}