namespace Alpha.Main.Services.Impl
{
    using Alpha.Main.Criteria.Language;
    using Fanex.Data.Repository;

    public interface UpdateUsageLanguageService
    {
        void UpdateUsageLanguage(int custId, int subAccId, string languageKey, string ip);
    }

    public class UpdateUsageLanguageServiceImpl : UpdateUsageLanguageService
    {
        private readonly IDynamicRepository dynamicRepository;

        public UpdateUsageLanguageServiceImpl(IDynamicRepository dynamicRepository)
            => this.dynamicRepository = dynamicRepository;

        public void UpdateUsageLanguage(int custId, int subAccId, string languageKey, string ip)
            => dynamicRepository.Execute(new UpdateUsageLanguageCommand(custId, subAccId, languageKey, ip));
    }
}