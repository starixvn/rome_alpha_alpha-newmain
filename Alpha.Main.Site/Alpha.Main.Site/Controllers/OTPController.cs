using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Alpha.Main.Site.Controllers
{
    public class OTPController : Controller
    {
        // GET: OTP
        public ActionResult Index()
        {
            return View();
        }

        // GET: OTP
        public ActionResult Config()
        {
            return View();
        }
    }
}