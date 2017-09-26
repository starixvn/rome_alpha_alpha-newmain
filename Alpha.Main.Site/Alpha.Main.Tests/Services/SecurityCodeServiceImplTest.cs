namespace Alpha.Main.Tests.Services
{
    using System;
    using Alpha.Main.AppResources;
    using Alpha.Main.Criteria.SecurityCode;
    using Alpha.Main.DataAccesses;
    using Alpha.Main.Models;
    using Alpha.Main.Params.SecurityCode;
    using Alpha.Main.Services.Impl;
    using Fanex.Data.Repository;
    using NSubstitute;
    using NUnit.Framework;
    using Sunplus.Agent.Model.Casino;
    using Sunplus.Agent.Utilities;

    [TestFixture]
    public class SecurityCodeServiceImplTest
    {
        private SecurityCodeServiceImpl securityCodeService;
        private SecurityCodeDataAccess securityCodeDataAccess;
        private CasinoService casinoService;
        private Encryptor encryptor;

        [SetUp]
        public void Setup()
        {
            securityCodeDataAccess = Substitute.For<SecurityCodeDataAccess>();
            encryptor = Substitute.For<Encryptor>();
            casinoService = Substitute.For<CasinoService>();

            securityCodeService = Substitute.ForPartsOf<SecurityCodeServiceImpl>(
                securityCodeDataAccess,
                casinoService,
                encryptor);
        }

        #region Verify Security Code

        [Test]
        public void VerifySecurityCode_ParamIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            VerifySecurityCodeParam param = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => securityCodeService.VerifySecurityCode(param));
        }

        [TestCase(null)]
        [TestCase("")]
        public void VerifySecurityCode_SecurityCodeIsNullOrEmpty_ReturnsInformSecurityCodeResult(string securityCode)
        {
            // Arrange
            var param = new VerifySecurityCodeParam();
            param.SecurityCode = securityCode;

            // Act
            var result = securityCodeService.VerifySecurityCode(param);

            // Assert
            Assert.IsTrue(result.Code == 1 && result.Message == Security.informSecCode);
        }

        [TestCase(null)]
        [TestCase("")]
        public void VerifySecurityCode_SaltIsNullOrEmpty_ReturnsInputWrongSecurityCodeResult(string salt)
        {
            // Arrange
            var param = new VerifySecurityCodeParam
            {
                SecurityCode = "SecurityCode",
                Salt = salt
            };

            // Act
            var result = securityCodeService.VerifySecurityCode(param);

            // Assert
            Assert.IsTrue(result.Code == 1 && result.Message == Security.alertInputWrongSecCode);
        }

        [Test]
        public void VerifySecurityCode_SubAccountIdIsEqualZero_CallsGetSecurityCode()
        {
            // Arrange
            var param = new VerifySecurityCodeParam
            {
                SecurityCode = "SecurityCode",
                Salt = "Salt",
                CustId = 1,
                SubAccId = 0
            };
            securityCodeDataAccess.UpdateSecurityCode(Arg.Any<CriteriaBase>()).Returns(new Result { Code = 1 });

            // Act
            securityCodeService.VerifySecurityCode(param);

            // Assert
            securityCodeDataAccess.Received().GetSecurityCode(
                Arg.Is<GetMainAccountSecurityCodeCriteria>(p => p.CustId == param.CustId));
        }

        [Test]
        public void VerifySecurityCode_SubAccountIdIsNotEqualZero_CallsGetSecurityCode()
        {
            // Arrange
            var param = new VerifySecurityCodeParam
            {
                SecurityCode = "SecurityCode",
                Salt = "Salt",
                CustId = 0,
                SubAccId = 1
            };
            securityCodeDataAccess.UpdateSecurityCode(Arg.Any<CriteriaBase>()).Returns(new Result { Code = 1 });

            // Act
            securityCodeService.VerifySecurityCode(param);

            // Assert
            securityCodeDataAccess.Received().GetSecurityCode(
                Arg.Is<GetSubAccountSecurityCodeCriteria>(p => p.CustId == param.CustId && p.SubaccId == param.SubAccId));
        }

        [Test]
        public void VerifySecurityCode_SubAccountIdIsEqualZero_CallsUpdateSecurityCode()
        {
            // Arrange
            var param = new VerifySecurityCodeParam
            {
                SecurityCode = "SecurityCode",
                Salt = "Salt",
                CustId = 1,
                SubAccId = 0,
                IP = "IP",
                Language = "en"
            };

            securityCodeDataAccess.UpdateSecurityCode(Arg.Any<UpdateMainAccountSecurityCodeCounterCriteria>()).Returns(new Result { Code = 1 });
            encryptor.ComputeHash(Arg.Is("SHA256"), Arg.Any<string>(), Arg.Is(param.Salt)).Returns("SecurityCode");

            // Act
            securityCodeService.VerifySecurityCode(param);

            // Assert
            securityCodeDataAccess.Received().UpdateSecurityCode(
                Arg.Is<UpdateMainAccountSecurityCodeCounterCriteria>(
                    p => p.CustId == param.CustId
                        && p.CheckedSecurityCode == true
                        && p.IP == param.IP
                        && p.Language == param.Language));
        }

        [Test]
        public void VerifySecurityCode_SubAccountIdIsNotEqualZero_CallsUpdateSecurityCode()
        {
            // Arrange
            var param = new VerifySecurityCodeParam
            {
                SecurityCode = "SecurityCode",
                Salt = "Salt",
                CustId = 0,
                SubAccId = 1,
                IP = "IP",
                Language = "en"
            };

            securityCodeDataAccess.UpdateSecurityCode(Arg.Any<UpdateSubAccountSecurityCodeCounterCriteria>()).Returns(new Result { Code = 1 });
            encryptor.ComputeHash(Arg.Is("SHA256"), Arg.Any<string>(), Arg.Is(param.Salt)).Returns("SecurityCode");

            // Act
            securityCodeService.VerifySecurityCode(param);

            // Assert
            securityCodeDataAccess.Received().UpdateSecurityCode(
                Arg.Is<UpdateSubAccountSecurityCodeCounterCriteria>(
                    p => p.CustId == param.CustId
                        && p.CheckedSecurityCode == true
                        && p.IP == param.IP
                        && p.Language == param.Language));
        }

        [Test]
        public void VerifySecurityCode_AccountIsClosedAndSubAccountIdIsEqualZero_CallsCloseCasinoAccount()
        {
            // Arrange
            var param = new VerifySecurityCodeParam
            {
                SecurityCode = "SecurityCode",
                Salt = "Salt",
                CustId = 1,
                SubAccId = 0,
                IP = "127.0.0.1",
                Language = "en"
            };

            securityCodeDataAccess.UpdateSecurityCode(Arg.Any<UpdateMainAccountSecurityCodeCounterCriteria>()).Returns(new Result { Code = 200 });
            encryptor.ComputeHash(Arg.Is("SHA256"), Arg.Any<string>(), Arg.Is(param.Salt)).Returns("SecurityCode");

            // Act
            securityCodeService.VerifySecurityCode(param);

            // Assert
            securityCodeService.Received().CloseCasinoAccount(param.CustId, param.IP);
        }

        [Test]
        public void VerifySecurityCode_UpdateSecurityCodeFailed_UpdatesResultCodeAndMessage()
        {
            // Arrange
            var param = new VerifySecurityCodeParam
            {
                SecurityCode = "SecurityCode",
                Salt = "Salt",
                CustId = 0,
                SubAccId = 1,
                IP = "IP",
                Language = "en"
            };

            securityCodeDataAccess.UpdateSecurityCode(Arg.Any<UpdateSubAccountSecurityCodeCounterCriteria>()).Returns(new Result { Code = 1 });
            encryptor.ComputeHash(Arg.Is("SHA256"), Arg.Any<string>(), Arg.Is(param.Salt)).Returns("SecurityCode");

            // Act
            var result = securityCodeService.VerifySecurityCode(param);

            // Assert
            Assert.IsTrue(result.Code == 9009 && result.Message == Security.alertInputWrongSecCode);
        }

        [Test]
        public void VerifySecurityCode_AccountIsClosed_UpdatesResultCode()
        {
            // Arrange
            var param = new VerifySecurityCodeParam
            {
                SecurityCode = "SecurityCode",
                Salt = "Salt",
                CustId = 0,
                SubAccId = 1,
                IP = "IP",
                Language = "en"
            };

            securityCodeDataAccess.UpdateSecurityCode(Arg.Any<UpdateSubAccountSecurityCodeCounterCriteria>()).Returns(new Result { Code = 200 });
            encryptor.ComputeHash(Arg.Is("SHA256"), Arg.Any<string>(), Arg.Is(param.Salt)).Returns("SecurityCode");

            // Act
            var result = securityCodeService.VerifySecurityCode(param);

            // Assert
            Assert.IsTrue(result.Code == 2);
        }

        #endregion

        #region Generate Security Token

        [Test]
        public void GenerateSecurityToken_Always_CallsEncryptorRandomString()
        {
            // Act
            securityCodeService.GenerateSecurityToken();

            // Assert
            encryptor.Received().RandomString(Arg.Is(6));
        }

        [Test]
        public void GenerateSecurityToken_Always_ReturnCorrectValue()
        {
            // Arrange
            var expectedSecurityToken = "123456";
            encryptor.RandomString(Arg.Is(6)).Returns(expectedSecurityToken);

            // Act
            var actualSecurityToken = securityCodeService.GenerateSecurityToken();

            // Assert
            Assert.AreEqual(expectedSecurityToken, actualSecurityToken);
        }

        #endregion

        #region Change Main Account Security Code

        [Test]
        public void ChangeMainAccountSecurityCode_ParamIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            ChangeSecurityCodeParam param = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => securityCodeService.ChangeMainAccountSecurityCode(param));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void ChangeMainAccountSecurityCode_SecurityCodeIsNullOrWhiteSpace_ReturnsSecurityCodeEmptyResult(string securityCode)
        {
            // Arrange
            var param = new ChangeSecurityCodeParam();
            param.NewSecurityCode = securityCode;

            // Act
            var result = securityCodeService.ChangeMainAccountSecurityCode(param);

            // Assert
            Assert.IsTrue(result.Code == 1 && result.Message == Security.alertSecCodeEmpty);
        }

        [Test]
        public void ChangeMainAccountSecurityCode_SecurityCodeIsWrongRule_ReturnsWrongRuleSecurityCodeResult()
        {
            // Arrange
            var param = new ChangeSecurityCodeParam();
            param.NewSecurityCode = "1234";

            // Act
            var result = securityCodeService.ChangeMainAccountSecurityCode(param);

            // Assert
            Assert.IsTrue(result.Code == 1 && result.Message == Security.lblSecCodeWrongRule);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void ChangeMainAccountSecurityCode_CurrentSecurityCodeIsNullOrWhiteSpace_ReturnsOldSecurityCodeEmptyResult(string oldSecurityCode)
        {
            // Arrange
            var param = new ChangeSecurityCodeParam();
            param.NewSecurityCode = "123457";
            param.MustVerifyCurrentSecurityCode = true;
            param.CurrentSecurityCode = oldSecurityCode;

            // Act
            var result = securityCodeService.ChangeMainAccountSecurityCode(param);

            // Assert
            Assert.IsTrue(result.Code == 1 && result.Message == Security.alertOldSecCodeEmpty);
        }

        [Test]
        public void ChangeMainAccountSecurityCode_VerifySecurityCodeFailed_ReturnIsNotOkResult()
        {
            // Arrange
            var param = new ChangeSecurityCodeParam();
            param.NewSecurityCode = "123458";
            param.MustVerifyCurrentSecurityCode = true;
            param.CurrentSecurityCode = "123457";
            param.Salt = string.Empty;

            // Act
            var result = securityCodeService.ChangeMainAccountSecurityCode(param);

            // Assert
            Assert.IsTrue(result.IsNotOk);
        }

        [Test]
        public void ChangeMainAccountSecurityCode_VerifySecurityCodeSuccessful_CallsUpdateSecurityCode()
        {
            // Arrange
            var param = new ChangeSecurityCodeParam();
            param.NewSecurityCode = "123458";
            param.MustVerifyCurrentSecurityCode = true;
            param.CurrentSecurityCode = "123457";
            param.Salt = "Salt";
            param.IsResetMember = true;

            encryptor.CfsEnCode(Arg.Is(param.NewSecurityCode)).Returns("HashSecurityCode");
            securityCodeDataAccess.UpdateSecurityCode(Arg.Any<CriteriaBase>()).Returns(new Result());

            // Act
            var result = securityCodeService.ChangeMainAccountSecurityCode(param);

            // Assert
            securityCodeDataAccess.Received().UpdateSecurityCode(
                Arg.Is<UpdateMainAccountSecurityCodeCriteria>(
                    p => p.CustId == param.CustId
                        && p.SubaccId == param.SubAccId
                        && p.UserId == param.UserId
                        && p.HashedSecurityCode == "HashSecurityCode"
                        && p.IP == param.IP
                        && p.Language == param.Language));
        }

        #endregion

        #region Change SubAccount Security Code

        [Test]
        public void ChangeSubAccountSecurityCode_ParamIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            ChangeSecurityCodeParam param = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => securityCodeService.ChangeSubAccountSecurityCode(param));
        }

        [Test]
        public void ChangeSubAccountSecurityCode_VerifySecurityCodeFailed_ReturnIsNotOkResult()
        {
            // Arrange
            var param = new ChangeSecurityCodeParam();
            param.NewSecurityCode = "123458";
            param.MustVerifyCurrentSecurityCode = true;
            param.CurrentSecurityCode = "123457";
            param.Salt = string.Empty;

            // Act
            var result = securityCodeService.ChangeSubAccountSecurityCode(param);

            // Assert
            Assert.IsTrue(result.IsNotOk);
        }

        [Test]
        public void ChangeSubAccountSecurityCode_VerifySecurityCodeSuccessful_CallsUpdateSecurityCode()
        {
            // Arrange
            var param = new ChangeSecurityCodeParam();
            param.NewSecurityCode = "123458";
            param.MustVerifyCurrentSecurityCode = true;
            param.CurrentSecurityCode = "123457";
            param.Salt = "Salt";
            param.IsResetMember = true;

            encryptor.ComputeHash(Arg.Is("SHA1"), Arg.Any<string>(), Arg.Any<string>()).Returns("HashSecurityCode");
            securityCodeDataAccess.UpdateSecurityCode(Arg.Any<CriteriaBase>()).Returns(new Result());

            // Act
            var result = securityCodeService.ChangeSubAccountSecurityCode(param);

            // Assert
            securityCodeDataAccess.Received().UpdateSecurityCode(
                Arg.Is<UpdateSubAccountSecurityCodeCriteria>(
                    p => p.CustId == param.CustId
                        && p.SubaccId == param.SubAccId
                        && p.HashedSecurityCode == "HashSecurityCode"
                        && p.IP == param.IP
                        && p.Language == param.Language));
        }

        #endregion

        #region Reset SubAccount Security Code
        [Test]
        public void ResetSubAccountSecurityCode_ParamIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            ResetSecurityCodeParam param = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => securityCodeService.ResetSubAccountSecurityCode(param));
        }

        [Test]
        public void ResetSubAccountSecurityCode_VerifySecurityCodeFailed_ReturnIsNotOkResult()
        {
            // Arrange
            var param = new ResetSecurityCodeParam();
            param.NewSecurityCode = string.Empty;

            // Act
            var result = securityCodeService.ResetSubAccountSecurityCode(param);

            // Assert
            Assert.IsTrue(result.IsNotOk);
        }

        [Test]
        public void ResetSubAccountSecurityCode_VerifySecurityCodeSuccessful_ReturnIsNotOkResult()
        {
            // Arrange
            var param = new ResetSecurityCodeParam();
            param.NewSecurityCode = "123457";

            encryptor.ComputeHash(Arg.Is("SHA1"), Arg.Any<string>(), Arg.Any<string>()).Returns("HashSecurityCode");

            // Act
            var result = securityCodeService.ResetSubAccountSecurityCode(param);

            // Assert
            securityCodeDataAccess.Received().UpdateSecurityCode(
                Arg.Is<ResetSubAccountSecurityCodeCriteria>(
                    p => p.CustId == param.CustId
                    && p.SubaccId == param.SubAccId
                    && p.UserId == param.UserId
                    && p.HashedSecurityCode == "HashSecurityCode"
                    && p.IP == param.IP
                    && p.Language == param.Language));
        }

        #endregion
    }
}
