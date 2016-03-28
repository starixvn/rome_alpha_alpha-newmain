namespace Alpha.Main.Site.ViewModels
{
    using Models.Enumerations;
    using Sunplus.Agent.Model.Entities;

    public class IndexViewModel
    {
        private readonly IUserSession userSession;        

        public IndexViewModel(IUserSession userSession = null)
        {
            this.userSession = userSession ?? UserSession.Current;
        }

        public string UserLevel
        {
            get
            {
                return Enumeration.FromValue<UserLevel>(userSession.RoleId).DisplayName.ToLower();
            }
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
    }
}
