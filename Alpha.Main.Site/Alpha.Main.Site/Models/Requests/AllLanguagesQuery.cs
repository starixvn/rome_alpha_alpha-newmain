namespace Alpha.Main.Site.Models.Requests
{
    using MediatR;
    using ViewModels;

    public class AllLanguagesQuery : IRequest<LanguageViewModel>
    {
        private string languageCode;

        public AllLanguagesQuery(string languageCode)
        {
            this.languageCode = languageCode;
        }

        public string LanguageCode
        {
            get
            {
                return this.languageCode;
            }
        }
    }
}