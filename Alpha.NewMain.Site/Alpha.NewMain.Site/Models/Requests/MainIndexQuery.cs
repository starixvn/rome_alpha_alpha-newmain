using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Alpha.NewMain.Site.Models.Views;
using MediatR;

namespace Alpha.NewMain.Site.Models.Requests
{
    public class MainIndexQuery : IRequest<MainIndexViewModel>
    {
    }
}