namespace Alpha.Main.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Resources;
    using System.Threading;
    using System.Threading.Tasks;
    using Alpha.Main.AppResources;
    using Alpha.Main.Constants;
    using Alpha.Main.Criteria.UMMessage;
    using Alpha.Main.Extensions;
    using Alpha.Main.Models.UMMessage;
    using Alpha.Main.Params.UMMessage;
    using Fanex.Caching;
    using Fanex.Data.Repository;

    public class UMMessageServiceImpl : UMMessageService
    {
        private const int CacheInSeconds = 60;
        private const string UMMessageCacheKey = "Alpha.Main.Services.Impl.UMMessageService";

        private readonly IDynamicRepository dynamicRepository;
        private readonly CacheRefService cacheRefService;
        private readonly ResourceManager settingTypeResource;

        public UMMessageServiceImpl(IDynamicRepository dynamicRepository, CacheRefService cacheRefService)
        {
            this.dynamicRepository = dynamicRepository;
            this.cacheRefService = cacheRefService;
            settingTypeResource = new ResourceManager(typeof(SettingType));
        }

        public async Task<IEnumerable<UMMessageDTO>> GetMessagesAsync(UMMessageParam param)
        {
            IEnumerable<UMMessageDTO> messages = CacheService.Get<List<UMMessageDTO>>(GetUMMessageCacheKey(param.Language));

            if (messages == null)
            {
                using (var semaphoreSlim = new SemaphoreSlim(1, 1))
                {
                    await semaphoreSlim.WaitAsync();

                    try
                    {
                        messages = await GetMessagesDTOAsync(param);
                    }
                    finally
                    {
                        semaphoreSlim.Release();
                    }
                }
            }

            return messages;
        }

        private async Task<IEnumerable<UMMessageDTO>> GetMessagesDTOAsync(UMMessageParam param)
        {
            var rawMessages = await dynamicRepository.FetchAsync<UMMessageItem>(new GetUMMessageCriteria(param));
            var messages = BuildMessages(rawMessages).ToList();

            CacheService.Set(
                GetUMMessageCacheKey(param.Language),
                messages,
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(CacheInSeconds)));

            return messages;
        }

        private IEnumerable<UMMessageDTO> BuildMessages(IEnumerable<UMMessageItem> messages)
        {
            foreach (var message in messages)
            {
                if (message.FunctionType == UMFunctionType.LockEdit)
                {
                    yield return BuildCustomerSettingsMessage(message);
                }
            }
        }

        private UMMessageDTO BuildCustomerSettingsMessage(UMMessageItem message)
            => new UMMessageDTO(
                BitConverter.ToInt64(message.Time_Stamp, 0),
                string.Format(
                    "{0}: {1} {2} {3} {4} {5}",
                    GetProductOrSportType(message.ProductId, message.SportType),
                    settingTypeResource.GetString($"_{message.SettingType}"),
                    Resources.UMFrom,
                    message.BeginTime.ToLongDateWithSlash(),
                    Resources.UMTo,
                    message.EndTime.ToLongDateWithSlash()));

        private string GetProductOrSportType(int productId, int sportTypeId)
        {
            if (sportTypeId > 0)
            {
                return cacheRefService.GetSportName(sportTypeId);
            }

            return productId == 0 ? Resources.AllProducts : cacheRefService.GetProductName(productId);
        }

        private static string GetUMMessageCacheKey(string language)
            => $"{UMMessageCacheKey}-{language}";
    }
}
