namespace Alpha.Main.Site.Controllers
{
    using Alpha.Web;
    using Fanex.ActivityTracking.Models;
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    public class ApiKeyController : BaseController
    {
        private readonly IMediator mediator;

        public ApiKeyController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public ActionResult CreateApiKey()
        {
            return View();
        }

        public ActionResult ApiKeyManagement()
        {
            return View();
        }

        public ActionResult ApiKeyStatistics()
        {
            return View();
        }

        public ActionResult EditApiKey()
        {
            return View();
        }
    }
}