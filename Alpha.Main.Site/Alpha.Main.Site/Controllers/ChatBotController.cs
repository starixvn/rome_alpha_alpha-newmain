namespace Alpha.Main.Site.Controllers
{
    using System.Web.Mvc;
    using ViewModels.ChatBot;
    using Web;

    public class ChatBotController : BaseController
    {
        public ActionResult Index()
            => View(new ChatBotViewModel());

        public ActionResult ChatBotSetting()
            => View(new ChatBotViewModel());
    }
}