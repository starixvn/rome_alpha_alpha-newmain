namespace Alpha.Main.Site.Handlers
{
    using Alpha.Main.Site.Models.Requests;
    using Alpha.Main.Site.ViewModels;
    using App_Start;
    using MediatR;

    public class DefaultHandler : IRequestHandler<IndexQuery, IndexViewModel>
    {
        public IndexViewModel Handle(IndexQuery message)
        {
            var viewModel = new IndexViewModel(
                message.UserId,
                message.OnUserId,
                message.RoleId,
                message.Language,
                message.LanguageKey,
                FunctionRegistry.GetDefaultUrl);

            return viewModel;
        }
    }
}