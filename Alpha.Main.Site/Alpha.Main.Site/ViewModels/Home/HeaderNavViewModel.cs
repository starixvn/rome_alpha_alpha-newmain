namespace Alpha.Main.Site.ViewModels
{
    using Sunplus.Agent.Model.Entities;

    public class HeaderNavViewModel
    {
       public string LanguageKey
        {
            get
            {
                return UserSession.Current.LangKey;
            }
        }
    }
}