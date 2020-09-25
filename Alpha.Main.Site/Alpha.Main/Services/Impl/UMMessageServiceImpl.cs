namespace Alpha.Main.Services.Impl
{
    using System;
    using System.Collections.Generic;
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
        private const int CacheInSeconds = 60 * 10; // 10 minutes.
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
            => BuildMessages(await GetUMMessageItemAsync(param));

        private async Task<IEnumerable<UMMessageItem>> GetUMMessageItemAsync(UMMessageParam param)
        {
            var cacheKey = $"{UMMessageCacheKey}_{param.Site}_{param.CurrencyId}_{param.SuperId}";
            var messages = CacheService.Get<IEnumerable<UMMessageItem>>(cacheKey);

            if (messages == null)
            {
                using (var semaphoreSlim = new SemaphoreSlim(1, 1))
                {
                    await semaphoreSlim.WaitAsync();

                    try
                    {
                        messages = await dynamicRepository.FetchAsync<UMMessageItem>(new GetUMMessageCriteria(param.CustId));

                        CacheService.Set(
                            cacheKey,
                            messages,
                            new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(CacheInSeconds)));
                    }
                    finally
                    {
                        semaphoreSlim.Release();
                    }
                }
            }

            return messages;
        }

        private IEnumerable<UMMessageDTO> BuildMessages(IEnumerable<UMMessageItem> messages)
        {
            foreach (var message in messages)
            {
                switch (message.FunctionType)
                {
                    case UMFunctionType.LockEdit:
                        yield return BuildCustomerSettingsMessage(message);
                        break;

                    case UMFunctionType.Transfer:
                        yield return BuildTransferMessage(message);
                        break;

                    default:
                        continue;
                }
            }
        }

        private UMMessageDTO BuildCustomerSettingsMessage(UMMessageItem message)
            => new UMMessageDTO(
                BitConverter.ToInt64(message.Time_Stamp, 0),
                string.Format(
                    Resources.UMCustomerSettingsPattern,
                    GetProductOrSportType(message.ProductId, message.SportType),
                    settingTypeResource.GetString($"_{message.SettingType}"),
                    message.BeginTime.ToLongDateWithSlash(),
                    message.EndTime.ToLongDateWithSlash()));

        private string GetProductOrSportType(int productId, int sportTypeId)
        {
            if (sportTypeId > 0)
            {
                return cacheRefService.GetSportName(sportTypeId);
            }

            return productId == 0 ? Resources.AllProducts : cacheRefService.GetProductName(productId);
        }

        private static UMMessageDTO BuildTransferMessage(UMMessageItem message)
            => new UMMessageDTO(
                BitConverter.ToInt64(message.Time_Stamp, 0),
                string.Format(
                    Resources.UMTransferPattern,
                    message.BeginTime.ToLongDateWithSlash(),
                    message.EndTime.ToLongDateWithSlash()));
    }
}
