namespace Alpha.Main.Site.Handlers
{
    using Alpha.Main.Site.Models.Requests;
    using Alpha.Main.Site.ViewModels;
    using App_Start;
    using MediatR;
    using Sunplus.Agent.Model.Entities;

    public class DefaultHandler :
        IRequestHandler<EmptyQuery, IndexViewModel>
    {
        private readonly IUserSession userSession;

        public DefaultHandler(IUserSession userSession)
        {
            this.userSession = userSession;
        }

        public IndexViewModel Handle(EmptyQuery message)
        {
            var viewModel = new IndexViewModel(userSession, FunctionRegistry.GetDefaultUrl);

            return viewModel;
        }
    }
}