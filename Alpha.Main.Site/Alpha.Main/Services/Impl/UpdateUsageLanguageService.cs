using Alpha.Main.Criteria.Language;
using Alpha.Main.DataAccesses.Impl;
using System.Threading.Tasks;

namespace Alpha.Main.Services.Impl
{

    public interface IUpdateUsageLanguageService
    {
        Task UpdateUsageLanguage(int custId, int subAccId, string usageLanguage);
    }

    public class UpdateUsageLanguageService : IUpdateUsageLanguageService
    {
        private readonly IUpdateUsageLanguageDataAccess updateUsageLanguageDataAccess;

        public UpdateUsageLanguageService(IUpdateUsageLanguageDataAccess updateUsageLanguageDataAccess)
        {
            this.updateUsageLanguageDataAccess = updateUsageLanguageDataAccess;
        }

        public async Task UpdateUsageLanguage(int custId, int subAccId, string usageLanguage)
        {
            await updateUsageLanguageDataAccess.UpdateUsageLanguage(new UpdateUsageLanguageCriteria
            {
                CustId = custId,
                SubAccId = subAccId,
                LanguageUsage = usageLanguage
            });
        }
    }
}