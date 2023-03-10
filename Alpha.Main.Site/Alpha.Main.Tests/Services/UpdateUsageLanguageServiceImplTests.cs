namespace Alpha.Main.Tests.Services
{
    using Alpha.Main.Criteria.Language;
    using Alpha.Main.Services.Impl;
    using Fanex.Data.Repository;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class UpdateUsageLanguageServiceImplTests
    {
        private IDynamicRepository dynamicRepository;
        private UpdateUsageLanguageService updateUsageLanguageService;

        [SetUp]
        public void SetUp()
        {
            dynamicRepository = Substitute.For<IDynamicRepository>();
            updateUsageLanguageService = new UpdateUsageLanguageServiceImpl(dynamicRepository);
        }

        [Test]
        public void UpdateUsageLanguage_InvalidCustId_ReturnNull()
        {
            // Arrange
            var custId = 100;
            var subAccId = 0;
            var languageCode = "E";
            var ip = "::1";

            // Act
            updateUsageLanguageService.UpdateUsageLanguage(custId, subAccId, languageCode, ip);

            // Assert
            dynamicRepository.Received().Execute(Arg.Is<UpdateUsageLanguageCommand>(
                c => c.CustId == custId && c.SubAccId == subAccId && c.LanguageUsage == languageCode && c.IP == ip
            ));
        }
    }
}
