namespace Alpha.Main.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Alpha.Main.Models.UMMessage;
    using Alpha.Main.Site.Requests.UMMessage;
    using Alpha.Web;
    using MediatR;

    public class UMMessageController : BaseController
    {
        private readonly IMediator mediator;

        public UMMessageController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<ActionResult> Get(UMMessageQuery request)
            => Json(GetFakeUMMessage(), JsonRequestBehavior.AllowGet);
        //Json(await mediator.SendAsync(request));

        private static int temp = 0;

        private static UMMessageItem[] GetFakeUMMessage()
        {
            UMMessageItem[] items;

            switch (temp)
            {
                case 1:
                    items = new[]
                    {
                        new UMMessageItem(1, "We are processing the new product launch, please try to create downline after hh:mm AM/PM (GMT+8). Please contact your upline or CSD for further information.")
                    };
                    break;
                case 2:
                    items = new[]
                    {
                        new UMMessageItem(1, "We are processing the new product launch, please try to create downline after hh:mm AM/PM (GMT+8). Please contact your upline or CSD for further information."),
                        new UMMessageItem(2, "Live Casino is UM now")
                    };
                    break;
                case 3:
                    items = new[]
                    {
                        new UMMessageItem(1, "We are processing the new product launch, please try to create downline after hh:mm AM/PM (GMT+8). Please contact your upline or CSD for further information."),
                        new UMMessageItem(2, "Live Casino is UM now"),
                        new UMMessageItem(3, "Saba.Club Product Status is UM now")
                    };
                    break;
                case 4:
                    items = new[]
                    {
                        new UMMessageItem(1, "We are processing the new product launch, please try to create downline after hh:mm AM/PM (GMT+8). Please contact your upline or CSD for further information."),
                        new UMMessageItem(3, "Saba.Club Product Status is UM now")
                    };
                    break;
                case 5:
                    items = new[]
                    {
                        new UMMessageItem(3, "Saba.Club Product Status is UM now"),
                        new UMMessageItem(4, "Allbet Bet Setting is UM now"),
                    };
                    break;
                default:
                    items = new UMMessageItem[0];
                    temp = 0;
                    break;
            }

            temp++;
            return items;
        }
        //=> new[]
        //{
        //    new UMMessageItem(1, "We are processing the new product launch, please try to create downline after hh:mm AM/PM (GMT+8). Please contact your upline or CSD for further information."),
        //    new UMMessageItem(2, "Live Casino is UM now")
        //};
        //=> new[]
        //{
        //    new UMMessageItem(1, "We are processing the new product launch, please try to create downline after hh:mm AM/PM (GMT+8). Please contact your upline or CSD for further information.")
        //};
        //=> new UMMessageItem[0];
    }
}