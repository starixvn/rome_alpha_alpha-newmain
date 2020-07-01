namespace Alpha.Main.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Alpha.Main.Criteria.UMMessage;
    using Alpha.Main.Models.UMMessage;
    using Fanex.Caching;
    using Fanex.Data.Repository;

    public class UMMessageServiceImpl : UMMessageService
    {
        private const int CacheInSeconds = 60;
        private const string UMMessageCacheKey = "Alpha.Main.Services.Impl.UMMessageService";

        private readonly IDynamicRepository dynamicRepository;

        public UMMessageServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<IEnumerable<UMMessageItem>> GetMessagesAsync(string language)
        {
            var messages = CacheService.Get<List<UMMessageItem>>(UMMessageCacheKey);

            if (messages == null)
            {
                using (var semaphoreSlim = new SemaphoreSlim(1, 1))
                {
                    await semaphoreSlim.WaitAsync();

                    try
                    {
                        await GetMessagesAsync();
                    }
                    finally
                    {
                        semaphoreSlim.Release();
                    }
                }
            }

            return messages.Select(message => GetMessage(message, language));
        }

        private async Task<List<UMMessageItem>> GetMessagesAsync()
        {
            var messages = (await dynamicRepository.FetchAsync<UMMessageItem>(new GetUMMessageCriteria())).ToList();
            CacheService.Set(
                UMMessageCacheKey,
                messages,
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(CacheInSeconds)));

            return messages;
        }

        private static UMMessageItem GetMessage(UMMessageItem message, string language)
        {
            string content;

            switch (language)
            {
                case "ja-JP":
                    content = message.Message_JP;
                    break;

                case "ko-KR":
                    content = message.Message_KO;
                    break;

                case "th-TH":
                    content = message.Message_TH;
                    break;

                case "vi-VN":
                    content = message.Message_VI;
                    break;

                case "zh-CN":
                    content = message.Message_CS;
                    break;

                case "zh-TW":
                    content = message.Message_CH;
                    break;

                default:
                    content = message.Message_EN;
                    break;
            }

            return new UMMessageItem(message.Id, content);
        }
    }
}
