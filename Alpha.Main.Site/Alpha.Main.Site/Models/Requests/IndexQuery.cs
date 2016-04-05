namespace Alpha.Main.Site.Models.Requests
{
    using MediatR;
    using Sunplus.Agent.Model.Entities;
    using ViewModels;

    public class IndexQuery : IRequest<IndexViewModel>
    {
        private readonly IUserSession userSession;

        public IndexQuery(IUserSession userSession)
        {
            this.userSession = userSession;
        }

        public string Language
        {
            get
            {
                return userSession.Language;
            }
        }

        public string LanguageKey
        {
            get
            {
                return userSession.LangKey;
            }
        }

        public int OnUserId
        {
            get
            {
                return userSession.OnUserId;
            }
        }

        public byte RoleId
        {
            get
            {
                return userSession.RoleId;
            }
        }
    }
}