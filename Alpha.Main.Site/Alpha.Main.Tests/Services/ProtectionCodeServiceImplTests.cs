namespace Alpha.Main.Tests.Services
{
    using System.Threading.Tasks;
    using Alpha.Main.Criteria.ProtectionCode;
    using Alpha.Main.Params.ProtectionCode;
    using Alpha.Main.Services;
    using Alpha.Main.Services.Impl;
    using Fanex.Cryptography.Aes;
    using Fanex.Data.Repository;
    using NSubstitute;
    using NUnit.Framework;
    using Sunplus.Agent.Utilities;

    [TestFixture]
    public class ProtectionCodeServiceImplTests
    {
        private IAesService aes;
        private Encryptor encryptor;
        private IDynamicRepository dynamicRepository;
        private ProtectionCodeService protectionCodeService;

        [SetUp]
        public void SetUp()
        {
            aes = Substitute.For<IAesService>();
            encryptor = Substitute.For<Encryptor>();
            dynamicRepository = Substitute.For<IDynamicRepository>();
            protectionCodeService = new ProtectionCodeServiceImpl(aes, encryptor, dynamicRepository);
        }

        [Test]
        public void GenerateHashKey_Always_CallsRandomString()
        {
            // Arrange & Act
            protectionCodeService.GenerateHashKey();

            // Assert
            encryptor.Received().RandomString(Arg.Is<int>(length => length == 4));
        }

        [Test]
        public async Task GetAsync_Always_CallsGetAsync()
        {
            // Arrange
            var custId = 123;
            var subAccId = 456;

            // Act
            await protectionCodeService.GetAsync(custId, subAccId);

            // Assert
            await dynamicRepository.Received().GetAsync<string>(Arg.Is<GetProtectionCodeCriteria>(
                c => c.CustId == custId && c.SubAccId == subAccId));
        }

        [Test]
        public async Task UpdateAsync_ValidParams_CallsExecuteAsync()
        {
            // Arrange
            var protectionCode = "qwerty";
            var rawProtectionCode = "123457";
            var encodedProtectionCode = "zxcvbn";
            var param = new UpdateProtectionCodeParam(protectionCode, "1234", 1, 0, "E", "::1");
            aes.Decrypt(param.ProtectionCode, param.HashKey).Returns(rawProtectionCode);
            encryptor.CfsEnCode(rawProtectionCode).Returns(encodedProtectionCode);

            // Act
            await protectionCodeService.UpdateAsync(param);

            // Assert
            await dynamicRepository.Received().ExecuteAsync(Arg.Is<UpdateProtectionCodeCriteria>(
                c => c.ProtectionCode == encodedProtectionCode
                && c.CustId == param.CustId
                && c.SubAccId == param.SubAccId
                && c.Language == param.LangKey
                && c.IP == param.IP));
        }

        [Test]
        public async Task UpdateAsync_Successfully_ReturnCorrectMessage()
        {
            // Arrange
            var protectionCode = "qwerty";
            var rawProtectionCode = "123457";
            var encodedProtectionCode = "zxcvbn";
            var param = new UpdateProtectionCodeParam(protectionCode, "1234", 1, 0, "E", "::1");
            aes.Decrypt(param.ProtectionCode, param.HashKey).Returns(rawProtectionCode);
            encryptor.CfsEnCode(rawProtectionCode).Returns(encodedProtectionCode);
            dynamicRepository.ExecuteAsync(Arg.Any<INonQueryCommand>()).Returns(Task.CompletedTask);

            // Act
            var result = await protectionCodeService.UpdateAsync(param);

            // Assert
            Assert.AreEqual("Your Protection Code has been updated successfully.", result.Message);
        }

        [TestCase("    ")]
        [TestCase("")]
        [TestCase(null)]
        public async Task UpdateAsync_EmptyProtectionCode_ReturnErrorMsg(string protectionCode)
        {
            // Arrange
            var param = new UpdateProtectionCodeParam(protectionCode, "1234", 1, 0, "E", "::1");

            // Act
            var result = await protectionCodeService.UpdateAsync(param);

            // Assert
            Assert.AreEqual("New Protection Code is required.", result.Message);
        }

        [Test]
        public async Task UpdateAsync_InvalidLengthOfProtectionCode_ReturnErrorMsg()
        {
            // Arrange
            var param = new UpdateProtectionCodeParam("qwerty", "1234", 1, 0, "E", "::1");
            aes.Decrypt(param.ProtectionCode, param.HashKey).Returns("1111");

            // Act
            var result = await protectionCodeService.UpdateAsync(param);

            // Assert
            Assert.AreEqual("The protection code must be at least 6 characters long.", result.Message);
        }
    }
}