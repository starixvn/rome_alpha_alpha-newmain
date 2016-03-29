namespace Alpha.Main.Site.ViewModels
{
    using App_Start;
    using Helpers;
    using Models.Enumerations;
    using Services;
    using Sunplus.Agent.Model.Entities;

    public class IndexViewModel
    {
        private readonly IUserSession userSession;
        private readonly string defaultUrl;

        public IndexViewModel(IUserSession userSession = null, string defaultUrl = "")
        {
            this.userSession = userSession ?? UserSession.Current;
            this.defaultUrl = string.IsNullOrWhiteSpace(defaultUrl) ? FunctionRegistry.GetDefaultUrl() : defaultUrl;
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

        public string DefaultUrl
        {
            get
            {
                return defaultUrl;
            }
        }

        public string OldMainRootPath
        {
            get
            {
                return AppPathHelper.GetOldMainPath("~");
            }
        }

        public string MainRootPath
        {
            get
            {
                return AppPathHelper.GetAppPath(AppSettings.VirtualDirectory, "~");
            }
        }
    }
}
