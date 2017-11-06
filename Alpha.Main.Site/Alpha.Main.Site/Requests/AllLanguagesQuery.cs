namespace Alpha.Main.Site.Models.Requests
{
    using Configurations;
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using ViewModels;

    public class AllLanguagesQuery : IRequest<LanguageViewModel>
    {
        private string languageCode;

        public AllLanguagesQuery(string languageCode, bool? disabledVietnamese)
        {
            this.languageCode = languageCode ?? new UserSessionExt().Language;
            DisabledVietnamese = disabledVietnamese ?? AppSettings.DisabledVietnamese;
        }

        public string LanguageCode
        {
            get
            {
                return languageCode;
            }
        }

        public bool DisabledVietnamese { get; }
    }
}
