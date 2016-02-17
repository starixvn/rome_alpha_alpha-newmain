namespace Alpha.NewMain.Site.Handlers
{
    using System;
    using MediatR;
    using Models.Requests;
    using Models.Views;

    public class MainHandler :
        IRequestHandler<MainIndexQuery, MainIndexViewModel>
    {
        public MainIndexViewModel Handle(MainIndexQuery message)
        {
            return new MainIndexViewModel();
        }
    }
}