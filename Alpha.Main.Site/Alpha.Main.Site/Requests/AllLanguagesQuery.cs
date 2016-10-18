namespace Alpha.Main.Site.Models.Requests
{
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using ViewModels;

    public class AllLanguagesQuery : IRequest<LanguageViewModel>
    {
        private string languageCode;

        public AllLanguagesQuery() : this(null)
        {
        }

        public AllLanguagesQuery(string languageCode)
        {
            this.languageCode = languageCode ?? new UserSessionExt().Language;
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