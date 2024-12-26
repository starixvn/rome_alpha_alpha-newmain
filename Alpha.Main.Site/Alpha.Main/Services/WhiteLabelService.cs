using Alpha.Main.Criteria.WhiteLabel;
using Fanex.Data.Repository;
using System.Threading.Tasks;

namespace Alpha.Main.Services
{
    public interface WhiteLabelService
    {
        Task<bool> IsOfferedWhiteLabel(int custId);
    }

    public class WhiteLabelServiceImpl : WhiteLabelService
    {
        private readonly IDynamicRepository dynamicRepository;

        public WhiteLabelServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<bool> IsOfferedWhiteLabel(int custId)
        {
            var commmand = new CheckExistsWhiteLabelCommand(custId);
            await dynamicRepository.ExecuteAsync(commmand);
            return commmand.IsExists;
        }
    }
}