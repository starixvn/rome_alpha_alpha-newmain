namespace Alpha.Main.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Resources;
    using System.Threading;
    using System.Threading.Tasks;
    using Alpha.Main.AppResources;
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

        public async Task<IEnumerable<MessageDTO>> GetMessagesAsync(UMMessageParam param)
        {
            IEnumerable<MessageDTO> messages = CacheService.Get<List<MessageDTO>>(GetUMMessageCacheKey(param.Language));

            if (messages == null)
            {
                using (var semaphoreSlim = new SemaphoreSlim(1, 1))
                {
                    await semaphoreSlim.WaitAsync();

                    try
                    {
                        messages = await GetRawMessagesAsync(param);
                    }
                    finally
                    {
                        semaphoreSlim.Release();
                    }
                }
            }

            return messages;
        }

        private async Task<IEnumerable<MessageDTO>> GetRawMessagesAsync(UMMessageParam param)
        {
            var rawMessages = await dynamicRepository.FetchAsync<UMMessageItem>(new GetUMMessageCriteria(param));
            var messages = BuildMessages(rawMessages).ToList();

            CacheService.Set(
                GetUMMessageCacheKey(param.Language),
                messages,
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(CacheInSeconds)));

            return messages;
        }

        private IEnumerable<MessageDTO> BuildMessages(IEnumerable<UMMessageItem> messages)
        {
            foreach (var message in messages)
            {
                yield return BuildMessage(message);
            }
        }

        private MessageDTO BuildMessage(UMMessageItem message)
            => new MessageDTO(
                BitConverter.ToInt64(message.Time_Stamp, 0),
                string.Format(
                    "{0}: {1} {2} {3} {4} ({5}) {6} {7} ({5})",
                    cacheRefService.GetProductName(message.ProductId),
                    Resources.UMFor,
                    message.SettingType == 0 ? Resources.UMCustomerSettings : settingTypeResource.GetString($"_{message.SettingType}"),
                    Resources.UMFrom,
                    message.BeginTime.ToLongDateWithSlash(),
                    Resources.GMT8,
                    Resources.UMTo,
                    message.EndTime.ToLongDateWithSlash()));

        private static string GetUMMessageCacheKey(string language)
            => $"{UMMessageCacheKey}-{language}";
    }
}
