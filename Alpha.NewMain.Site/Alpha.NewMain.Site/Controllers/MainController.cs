namespace Alpha.NewMain.Site.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using MediatR;
    using Models.Requests;
    public class MainController : Controller
    {
        private IMediator _mediator;

        public MainController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: Main
        public ActionResult Index(MainIndexQuery query)
        {
            return View(_mediator.Send(query));
        }
    }
}