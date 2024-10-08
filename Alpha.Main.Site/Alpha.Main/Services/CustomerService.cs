using Alpha.Main.Criteria.Customer;
using Fanex.Data.Repository;
using NovaCasino.API.Client.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alpha.Main.Services
{
    public interface CustomerService
    {
        Task<IEnumerable<User>> GetUplines(int custId);

        Task UpdateSyncedNovaCasino(int custId);
    }

    public class CustomerServiceImpl : CustomerService
    {
        private readonly IDynamicRepository dynamicRepository;

        public CustomerServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public Task<IEnumerable<User>> GetUplines(int custId)
            => dynamicRepository.FetchAsync<User>(new GetUplineInfoCriteria(custId));

        public Task UpdateSyncedNovaCasino(int custId)
        {
            var command = new UpdateSyncedNovaCasinoCommand(custId);
            return dynamicRepository.ExecuteAsync(command);
        }
    }
}