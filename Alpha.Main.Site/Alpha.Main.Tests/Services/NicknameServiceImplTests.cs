namespace Alpha.Main.Tests.Services
{
    using Alpha.Main.Criteria.Nickname;
    using Alpha.Main.Models;
    using Alpha.Main.Params.Nickname;
    using Alpha.Main.Services.Impl;
    using Fanex.Data.Repository;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class NicknameServiceImplTests
    {
        private IDynamicRepository dynamicRepository;
        private NicknameServiceImpl service;
        private CompareLogic compareLogic;

        [SetUp]
        public void SetUp()
        {
            dynamicRepository = Substitute.For<IDynamicRepository>();
            service = new NicknameServiceImpl(dynamicRepository);
            compareLogic = new CompareLogic();
        }

        [TestCase(0, 0, "Your nickname was configured successfully")]
        [TestCase(1, 0, "Your nickname was configured successfully")]
        [TestCase(0, 1, "")]
        [TestCase(1, 1, "")]
        public void CreateNickname_NotEnableUpdateNickname_ReturnsCorrectResult(
           int subAccId, int errorCode, string message)
        {
            // Arrange
            dynamicRepository.When(x => x.Execute(Arg.Any<NicknameNonQueryCommand>())).Do(x =>
            {
                ((NicknameNonQueryCommand)x[0]).ErrCode = errorCode;
                ((NicknameNonQueryCommand)x[0]).ErrMsg = message;
            });

            var param = new CreateNicknameParam(1, subAccId, string.Empty, string.Empty, "en", false);
            var expected = new Result { Code = errorCode, Message = message };

            // Act
            var result = service.CreateNickname(param, false);

            // Assert
            Assert.True(compareLogic.Compare(expected, result).AreEqual);
        }

        [Test]
        public void CreateNickname_EnableUpdateNicknameAndNotValidNickname_ReturnsErrorNicknameRule()
        {
            // Arrange
            var param = new CreateNicknameParam(1, 0, "Nick", string.Empty, "en", true);
            var expected = new Result { Code = 100, Message = "Nickname must be unique, only alphabet or numeric and should have at least a minimum of 5 characters and maximum of 20 characters." };

            // Act
            var result = service.CreateNickname(param, false);

            // Assert
            Assert.True(compareLogic.Compare(expected, result).AreEqual);
        }

        [TestCase(0, 0, "Your nickname was configured successfully")]
        [TestCase(1, 0, "Your nickname was configured successfully")]
        [TestCase(0, 1, "")]
        [TestCase(1, 1, "")]
        public void CreateNickname_EnableUpdateNicknameAndValidNickname_ReturnsCorrectResult(
          int subAccId, int errorCode, string message)
        {
            // Arrange
            dynamicRepository.When(x => x.Execute(Arg.Any<NicknameNonQueryCommand>())).Do(x =>
            {
                ((NicknameNonQueryCommand)x[0]).ErrCode = errorCode;
                ((NicknameNonQueryCommand)x[0]).ErrMsg = message;
            });

            var param = new CreateNicknameParam(1, subAccId, "Nickname", string.Empty, "en", true);
            var expected = new Result { Code = errorCode, Message = message };

            // Act
            var result = service.CreateNickname(param, false);

            // Assert
            Assert.True(compareLogic.Compare(expected, result).AreEqual);
        }
    }
}
