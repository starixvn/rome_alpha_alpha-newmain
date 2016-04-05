namespace Alpha.Main.Site.ViewModels
{
    public class HeaderNavViewModel
    {
        private readonly string language;

        public HeaderNavViewModel(string language)
        {
            this.language = language;
        }

        public string Language
        {
            get
            {
                return language;
            }
        }
    }
}