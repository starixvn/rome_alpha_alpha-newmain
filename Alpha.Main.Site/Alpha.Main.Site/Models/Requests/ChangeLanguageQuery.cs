namespace Alpha.Main.Site.Models.Requests
{
    using MediatR;

    public class ChangeLanguageQuery : IRequest<string>
    {
        private string language;

        public ChangeLanguageQuery(string language)
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