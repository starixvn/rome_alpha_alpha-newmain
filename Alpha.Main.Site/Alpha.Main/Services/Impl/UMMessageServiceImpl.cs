namespace Alpha.Main.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Resources;
    using System.Text;
    using System.Threading.Tasks;
    using Alpha.Main.AppResources;
    using Alpha.Main.Configurations;
    using Alpha.Main.Constants;
    using Alpha.Main.Criteria.UMMessage;
    using Alpha.Main.Extensions;
    using Alpha.Main.Models.UMMessage;
    using Alpha.Main.Params.UMMessage;
    using Fanex.Caching;
    using Fanex.Data.Repository;

    public class UMMessageServiceImpl : UMMessageService
    {
        private const string UMMessageCacheKey = "Alpha.Main.Services.Impl.UMMessageService";

        private readonly IDynamicRepository dynamicRepository;
        private readonly CacheRefService cacheRefService;
        private readonly ResourceManager settingTypeResource;
        private readonly ICacheService cacheService;

        public UMMessageServiceImpl(IDynamicRepository dynamicRepository, CacheRefService cacheRefService)
            : this(dynamicRepository, cacheRefService, new CacheService())
        {
        }

        internal UMMessageServiceImpl(IDynamicRepository dynamicRepository, CacheRefService cacheRefService, ICacheService cacheService)
        {
            this.dynamicRepository = dynamicRepository;
            this.cacheRefService = cacheRefService;
            this.cacheService = cacheService;
            settingTypeResource = new ResourceManager(typeof(SettingType));
        }

        public async Task<IEnumerable<UMMessageDTO>> GetMessagesAsync(UMMessageParam param)
            => BuildMessages(await GetUMMessageItemAsync(param));

        private async Task<IEnumerable<UMMessageItem>> GetUMMessageItemAsync(UMMessageParam param)
        {
            var cacheKey = $"{UMMessageCacheKey}_{param.Site}_{param.CurrencyId}_{param.SuperId}";
            var messages = cacheService.Get<IEnumerable<UMMessageItem>>(cacheKey);

            if (messages == null)
            {
                messages = await dynamicRepository.FetchAsync<UMMessageItem>(new GetUMMessageCriteria(param.CustId));

                 cacheService.Set(
                    cacheKey,
                    messages,
                    new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(AppSettings.UMAnnouncementPopupServerCacheInMinutes)));
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

                    case UMFunctionType.NewAccount:
                        yield return BuildUMPatternFromTo(Resources.UMNewAccountPattern, message);
                        break;

                    case UMFunctionType.Transfer:
                        yield return BuildUMPatternFromTo(Resources.UMTransferPattern, message);
                        break;

                    default:
                        continue;
                }
            }
        }

        private UMMessageDTO BuildCustomerSettingsMessage(UMMessageItem message)
        {
            var builder = new StringBuilder(
                string.Format(Resources.UMCustomerSettingsPattern, message.BeginTime.ToLongDateWithSlash(), message.EndTime.ToLongDateWithSlash()));
            builder
                .Replace("_SettingType_", settingTypeResource.GetString($"_{message.SettingType}"))
                .Replace("_Product_", GetProductOrSportType(message.ProductId, message.SportType));

            return new UMMessageDTO(BitConverter.ToInt64(message.Time_Stamp, 0), builder.ToString());
        }

        private string GetProductOrSportType(int productId, int sportTypeId)
        {
            if (productId == 1)
            {
                return cacheRefService.GetProductName(productId) + " " + cacheRefService.GetSportName(sportTypeId);
            }

            if (sportTypeId > 0)
            {
                return cacheRefService.GetSportName(sportTypeId);
            }

            return productId == 0 ? Resources.AllProducts : cacheRefService.GetProductName(productId);
        }

        private static UMMessageDTO BuildUMPatternFromTo(string pattern, UMMessageItem message)
            => new UMMessageDTO(
                BitConverter.ToInt64(message.Time_Stamp, 0),
                string.Format(
                    pattern,
                    message.BeginTime.ToLongDateWithSlash(),
                    message.EndTime.ToLongDateWithSlash()));
    }
}
