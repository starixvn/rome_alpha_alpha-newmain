namespace Alpha.Main.Tests.Services
{
    using Alpha.Main.Criteria.Theme;
    using Alpha.Main.Services;
    using Alpha.Main.Services.Impl;
    using Fanex.Data.Repository;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class ThemeServiceImplTests
    {
        private IDynamicRepository dynamicRepository;
        private ThemeService themeService;

        [SetUp]
        public void SetUp()
        {
            dynamicRepository = Substitute.For<IDynamicRepository>();
            themeService = new ThemeServiceImpl(dynamicRepository);
        }

        [Test]
        public void WriteLog_ValidParams_CallsExecute()
        {
            // Arrange
            var custId = 1;
            var subAccId = 2;
            var isDarkMode = true;
            var ip = "::1";
            var location = "HeaderMenu";

            // Arrange & Act
            themeService.WriteLog(custId, subAccId, isDarkMode, ip, location);

            // Assert
            dynamicRepository.Received().Execute(Arg.Is<LogChangeThemeCriteria>(
                c => c.CustId == custId
                && c.SubAccId == subAccId
                && c.IsDarkMode == isDarkMode
                && c.IP == ip
                && c.Location == location));
        }
    }
}
