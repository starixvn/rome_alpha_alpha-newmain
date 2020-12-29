namespace Alpha.Main.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.Main.Constants;
    using Alpha.Main.Criteria.UMMessage;
    using Alpha.Main.Models.UMMessage;
    using Alpha.Main.Params.UMMessage;
    using Alpha.Main.Services;
    using Alpha.Main.Services.Impl;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class UMMessageServiceImplTests
    {
        private IDynamicRepository dynamicRepository;
        private CacheRefService cacheRefService;
        private UMMessageService umMessageService;
        private static readonly UMMessageParam ParamTest = new UMMessageParam(1, "Sitename", 2, 3);

        [SetUp]
        public void SetUp()
        {
            dynamicRepository = Substitute.For<IDynamicRepository>();
            cacheRefService = Substitute.For<CacheRefService>();
            umMessageService = new UMMessageServiceImpl(dynamicRepository, cacheRefService);
            CacheService.Remove($"Alpha.Main.Services.Impl.UMMessageService_{ParamTest.Site}_{ParamTest.CurrencyId}_{ParamTest.SuperId}");
        }

        [Test]
        public async Task GetMessagesAsync_EmptyResult_ReturnsEmptyMessageDTOList()
        {
            // Arrange
            var rawMessages = Enumerable.Empty<UMMessageItem>();
            dynamicRepository
                .FetchAsync<UMMessageItem>(Arg.Any<GetUMMessageCriteria>())
                .Returns(rawMessages);

            // Act
            var messages = await umMessageService.GetMessagesAsync(ParamTest);

            // Assert
            Assert.IsEmpty(messages);
        }

        [Test]
        public async Task GetMessagesAsync_AllProductsUM_ReturnsCorrectMessageDTOList()
        {
            // Arrange
            var rawMessages = new List<UMMessageItem>
            {
                new UMMessageItem(0, 0, 0, UMFunctionType.LockEdit, DateTime.Now, DateTime.Now.AddHours(1), new byte[] { 0, 0, 0, 0, 18, 206, 158, 173 })
            };
            dynamicRepository
                .FetchAsync<UMMessageItem>(Arg.Any<GetUMMessageCriteria>())
                .Returns(rawMessages);

            // Act
            var messages = (await umMessageService.GetMessagesAsync(ParamTest)).ToList();

            // Assert
            Assert.IsTrue(messages[0].Message.StartsWith("<strong>All products</strong>: Customer Settings"));
        }

        [Test]
        public async Task GetMessagesAsync_AllProductsUM_NewAccount_ReturnsCorrectMessageDTOList()
        {
            // Arrange
            var rawMessages = new List<UMMessageItem>
            {
                new UMMessageItem(0, 0, 0, UMFunctionType.NewAccount, DateTime.Now, DateTime.Now.AddHours(1), new byte[] { 0, 0, 0, 0, 18, 206, 158, 173 })
            };
            dynamicRepository
                .FetchAsync<UMMessageItem>(Arg.Any<GetUMMessageCriteria>())
                .Returns(rawMessages);

            // Act
            var messages = (await umMessageService.GetMessagesAsync(ParamTest)).ToList();

            // Assert
            Assert.IsTrue(messages[0].Message.StartsWith("<strong>New Account</strong>: Under maintenance from"));
        }

        [Test]
        public async Task GetMessagesAsync_AllProductsUM_Transfer_ReturnsCorrectMessageDTOList()
        {
            // Arrange
            var rawMessages = new List<UMMessageItem>
            {
                new UMMessageItem(0, 0, 0, UMFunctionType.Transfer, DateTime.Now, DateTime.Now.AddHours(1), new byte[] { 0, 0, 0, 0, 18, 206, 158, 173 })
            };
            dynamicRepository
                .FetchAsync<UMMessageItem>(Arg.Any<GetUMMessageCriteria>())
                .Returns(rawMessages);

            // Act
            var messages = (await umMessageService.GetMessagesAsync(ParamTest)).ToList();

            // Assert
            Assert.IsTrue(messages[0].Message.StartsWith("<strong>Transfer</strong>: Under maintenance from"));
        }

        [Test]
        public async Task GetMessagesAsync_AllSportsAndAllSettingsUM_ReturnsCorrectMessageDTOList()
        {
            // Arrange
            var rawMessages = new List<UMMessageItem>
            {
                new UMMessageItem(5, 0, 0, UMFunctionType.LockEdit, DateTime.Now, DateTime.Now.AddHours(1), new byte[] { 0, 0, 0, 0, 18, 206, 158, 173 })
            };
            dynamicRepository
                .FetchAsync<UMMessageItem>(Arg.Any<GetUMMessageCriteria>())
                .Returns(rawMessages);
            cacheRefService.GetProductName(5).Returns("Live Casino");

            // Act
            var messages = (await umMessageService.GetMessagesAsync(ParamTest)).ToList();

            // Assert
            Assert.IsTrue(messages[0].Message.StartsWith("<strong>Live Casino</strong>: Customer Settings"));
        }

        [Test]
        public async Task GetMessagesAsync_AllSportsAndOneSettingUM_ReturnsCorrectMessageDTOList()
        {
            // Arrange
            var rawMessages = new List<UMMessageItem>
            {
                new UMMessageItem(5, 5, 9, UMFunctionType.LockEdit, DateTime.Now, DateTime.Now.AddHours(1), new byte[] { 0, 0, 0, 0, 18, 206, 158, 173 })
            };
            dynamicRepository
                .FetchAsync<UMMessageItem>(Arg.Any<GetUMMessageCriteria>())
                .Returns(rawMessages);
            cacheRefService.GetSportName(5).Returns("Live Casino");

            // Act
            var messages = (await umMessageService.GetMessagesAsync(ParamTest)).ToList();

            // Assert
            Assert.IsTrue(messages[0].Message.StartsWith("<strong>Live Casino</strong>: Bet Setting"));
        }
    }
}
