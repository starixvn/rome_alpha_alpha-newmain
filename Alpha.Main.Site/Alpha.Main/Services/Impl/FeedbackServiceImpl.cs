using System.Threading.Tasks;
using Alpha.Main.Criteria.Feedback;
using Fanex.Data.Repository;

namespace Alpha.Main.Services.Impl
{
    public class FeedbackServiceImpl : FeedbackService
    {
        private readonly IDynamicRepository dynamicRepository;

        public FeedbackServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<bool> CheckExistsAsync(int custId)
        {
            var criteria = new CheckExistsCriteria(custId);

            await dynamicRepository.ExecuteAsync(criteria);

            return criteria.IsExists;
        }
    }
}