using Alpha.Main.Criteria.Language;
using Fanex.Data.Repository;
using System.Threading.Tasks;

namespace Alpha.Main.DataAccesses.Impl
{
    public interface IUpdateUsageLanguageDataAccess
    {
        Task UpdateUsageLanguage(UpdateUsageLanguageCriteria updateUsageLanguageCriteria);
    }

    public class UpdateUsageLanguageDataAccess : IUpdateUsageLanguageDataAccess
    {
        private readonly IDynamicRepository dynamicRepository;

        public UpdateUsageLanguageDataAccess(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task UpdateUsageLanguage(UpdateUsageLanguageCriteria updateUsageLanguageCriteria)
        {
            await dynamicRepository.GetAsync<int>(updateUsageLanguageCriteria);
        }
    }
}