namespace Alpha.Main.Site.ViewModels
{
    using Sunplus.Agent.Model.Entities;

    public class HeaderNavViewModel
    {
       public string Language
        {
            get
            {
                return UserSession.Current.Language;
            }
        }
    }
}