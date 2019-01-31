namespace Alpha.Main.Tests.Services
{
    using AppResources;
    using Constants;
    using KellermanSoftware.CompareNetObjects;
    using Main.Criteria.Password;
    using Main.DataAccesses;
    using Main.Models;
    using Main.Services;
    using Main.Services.Impl;
    using NSubstitute;
    using NUnit.Framework;
    using Params.Password;
    using Sunplus.Agent.Model.Casino;
    using Sunplus.Agent.Utilities;
    using System;

    [TestFixture]
    public class PasswordServiceImplTests
    {
        private CompareLogic compareLogic;
        private PasswordServiceImpl passwordService;
        private Encryptor encryptor;
        private PasswordDataAccess passwordDataAccess;
        private CasinoService casinoService;
        private AesService aes;

        [SetUp]
        public void SetUp()
        {
            compareLogic = new CompareLogic();

            encryptor = Substitute.For<Encryptor>();
            passwordDataAccess = Substitute.For<PasswordDataAccess>();
            casinoService = Substitute.For<CasinoService>();
            aes = Substitute.For<AesService>();

            passwordService = Substitute.ForPartsOf<PasswordServiceImpl>(
                encryptor,
                passwordDataAccess,
                casinoService,
                aes);
        }

        #region Change Main Account Passowrd

        [Test]
        public void ChangeMainAccountPassword_ParamIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            ChangeMainAccountPasswordParam param = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => passwordService.ChangeMainAccountPassword(param));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void ChangeMainAccountPassword_PasswordsAreNullOrWhiteSpace_ReturnsNotAuthorizedResult(string password)
        {
            // Arrange
            var param = new ChangeMainAccountPasswordParam
            {
                NewPassword = password,
                CurrentPassword = password,
            };

            aes.Decrypt(Arg.Any<string>(), Arg.Any<string>()).Returns(password);

            // Act
            var result = passwordService.ChangeMainAccountPassword(param);

            // Assert
            Assert.IsTrue(result.IsNotOk && result.Message == Security.NotAuthorized);
        }

        [Test]
        public void ChangeMainAccountPassword_NewPasswordIsEqualCurrentPassword_ReturnsPasswordOldOneResult()
        {
            // Arrange
            var password = "1234aaAA";
            var param = new ChangeMainAccountPasswordParam
            {
                NewPassword = password,
                CurrentPassword = password,
            };

            aes.Decrypt(Arg.Any<string>(), Arg.Any<string>()).Returns(password);

            // Act
            var result = passwordService.ChangeMainAccountPassword(param);

            // Assert
            Assert.IsTrue(result.IsNotOk && result.Message == Security.lblPasswordDOldOne);
        }

        [Test]
        public void ChangeMainAccountPassword_ValidatePasswordSuccessful_CallsUpdatePassword()
        {
            // Arrange
            var newPassword = "1234aaAA";
            var currentPassword = "1234aa";
            var param = new ChangeMainAccountPasswordParam
            {
                CustId = CustId.ValidId,
                NewPassword = newPassword,
                CurrentPassword = currentPassword,
                IsCasinoDisabled = true,
                LanguageKey = "en",
                IP = "127.0.0.1",
                ActorId = 1,
                HashKey = "HashKey"
            };

            encryptor.HexDecode(param.CurrentPassword).Returns("CurrentPassword");
            encryptor.HexDecode(param.NewPassword).Returns("NewPassword");

            aes.Decrypt(currentPassword, "HashKey").Returns(currentPassword);
            aes.Decrypt(newPassword, "HashKey").Returns(newPassword);

            encryptor.CfsEnCode(currentPassword).Returns("OldPassword");
            encryptor.CfsEnCode(newPassword).Returns("UserPassword");

            passwordDataAccess.UpdatePassword(Arg.Any<UpdateMainAccountPasswordCriteria>()).Returns(new Result { Code = 1 });

            // Act
            passwordService.ChangeMainAccountPassword(param);

            // Assert
            passwordDataAccess.Received().UpdatePassword(
                Arg.Is<UpdateMainAccountPasswordCriteria>(
                    p => p.CustId == param.CustId
                        && p.DisCasino == param.IsCasinoDisabled
                        && p.Language == param.LanguageKey
                        && p.LoginIP == param.IP
                        && p.OldPwd == "OldPassword"
                        && p.UserPwd == "UserPassword"
                        && p.UserId == param.ActorId));
        }

        [Test]
        public void ChangeMainAccountPassword_UpdatePasswordSuccessfulAndIsCasinoSync_CallsChangeCasionAccountPassword()
        {
            // Arrange
            var newPassword = "1234aaAA";
            var currentPassword = "1234aa";
            var param = new ChangeMainAccountPasswordParam
            {
                NewPassword = newPassword,
                CurrentPassword = currentPassword,
                IsCasinoSync = true,
                ActorId = 1,
                ActorName = "ActorName",
                IP = "127.0.0.1"
            };

            encryptor.HexDecode(Arg.Is(param.CurrentPassword)).Returns("CurrentPassword");
            encryptor.HexDecode(Arg.Is(param.NewPassword)).Returns("NewPassword");

            aes.Decrypt(Arg.Any<string>(), Arg.Is("CurrentPassword")).Returns(currentPassword);
            aes.Decrypt(Arg.Any<string>(), Arg.Is("NewPassword")).Returns(newPassword);

            encryptor.CfsEnCode(Arg.Is(currentPassword)).Returns("OldPassword");
            encryptor.CfsEnCode(Arg.Is(newPassword)).Returns("UserPassword");

            passwordDataAccess.UpdatePassword(Arg.Any<UpdateMainAccountPasswordCriteria>()).Returns(new Result { Code = 0 });

            // Act
            passwordService.ChangeMainAccountPassword(param);

            // Assert
            passwordService.Received().ChangeCasinoAccountPassword(
                param.CustId,
                "UserPassword",
                param.ActorId,
                param.ActorName,
                param.IP);
        }

        #endregion Change Main Account Passowrd

        #region Change SubAccount Password

        [Test]
        public void ChangeSubAccountPassword_ParamIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            ChangeSubAccountPasswordParam param = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => passwordService.ChangeSubAccountPassword(param));
        }

        [Test]
        public void ChangeSubAccountPassword_ValidatePasswordFailed_ReturnsIsNotOkResult()
        {
            // Arrange
            var param = new ChangeSubAccountPasswordParam
            {
                NewPassword = string.Empty,
            };

            aes.Decrypt(Arg.Any<string>(), Arg.Any<string>()).Returns(string.Empty);

            // Act
            var result = passwordService.ChangeSubAccountPassword(param);

            // Assert
            Assert.IsTrue(result.IsNotOk);
        }

        [Test]
        public void ChangeSubAccountPassword_ValidatePasswordSuccessful_CallsUpdatePassword()
        {
            // Arrange
            var newPassword = "1234aaAA";
            var currentPassword = "1234aa";
            var param = new ChangeSubAccountPasswordParam
            {
                NewPassword = newPassword,
                CurrentPassword = currentPassword,
                HashKey = "HashKey"
            };

            encryptor.HexDecode(Arg.Is(param.CurrentPassword)).Returns("CurrentPassword");
            encryptor.HexDecode(Arg.Is(param.NewPassword)).Returns("NewPassword");

            aes.Decrypt(currentPassword, "HashKey").Returns(currentPassword);
            aes.Decrypt(newPassword, "HashKey").Returns(newPassword);

            encryptor.CfsEnCode(Arg.Is(currentPassword)).Returns("OldPassword");
            encryptor.CfsEnCode(Arg.Is(newPassword)).Returns("UserPassword");

            passwordDataAccess.UpdatePassword(Arg.Any<UpdateSubAccountPasswordCriteria>()).Returns(new Result { Code = 0 });

            // Act
            passwordService.ChangeSubAccountPassword(param);

            // Assert
            passwordDataAccess.Received().UpdatePassword(
                Arg.Is<UpdateSubAccountPasswordCriteria>(
                    p => p.CustId == param.CustId
                        && p.Language == param.LanguageKey
                        && p.LoginIP == param.IP
                        && p.OldPwd == "OldPassword"
                        && p.UserPwd == "UserPassword"
                        && p.SubAccId == param.SubAccountId));
        }

        #endregion Change SubAccount Password

        #region Reset Main Account Password

        [Test]
        public void ResetMainAccountPassword_ParamIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            ResetMainAccountPasswordParam param = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => passwordService.ResetMainAccountPassword(param));
        }

        [Test]
        public void ResetMainAccountPassword_ValidatePasswordFailed_ReturnsIsNotOkResult()
        {
            // Arrange
            var param = new ResetMainAccountPasswordParam();
            param.IsResetMemberPassword = true;
            aes.Decrypt(Arg.Any<string>(), Arg.Any<string>()).Returns("1234 aa");

            // Act
            var result = passwordService.ResetMainAccountPassword(param);

            // Assert
            Assert.IsTrue(result.IsNotOk);
        }

        [Test]
        public void ResetMainAccountPassword_ValidatePasswordSuccessful_CallsUpdatePassword()
        {
            // Arrange
            var param = new ResetMainAccountPasswordParam
            {
                IsResetMemberPassword = true,
                CustId = 1,
                LanguageKey = "en",
                IP = "127.0.0.1",
                SubAccountId = 0,
                IsCasinoDisabled = true,
                ActorId = 1
            };
            aes.Decrypt(Arg.Any<string>(), Arg.Any<string>()).Returns("1234aaAA");
            encryptor.CfsEnCode(Arg.Is("1234aaAA")).Returns("EncodedPassword");

            var criteria = new ResetMainAccountPasswordCriteria
            {
                CustId = param.CustId,
                Language = param.LanguageKey,
                LoginIP = param.IP,
                UserPwd = "EncodedPassword",
                SubAccId = param.SubAccountId,
                DisCasino = param.IsCasinoDisabled,
                UserId = param.ActorId
            };

            passwordDataAccess.UpdatePassword(Arg.Any<ResetMainAccountPasswordCriteria>()).Returns(new Result { Code = 1 });

            // Act
            passwordService.ResetMainAccountPassword(param);

            // Assert
            passwordDataAccess.Received().UpdatePassword(
                Arg.Is<ResetMainAccountPasswordCriteria>(
                    c => compareLogic.Compare(c, criteria).AreEqual));
        }

        [Test]
        public void ResetMainAccountPassword_UpdatePasswordSuccessfulAndIsCasinoSync_CallsChangeCasionAccountPassword()
        {
            // Arrange
            var param = new ResetMainAccountPasswordParam
            {
                IsResetMemberPassword = true,
                CustId = 1,
                LanguageKey = "en",
                IP = "127.0.0.1",
                SubAccountId = 0,
                IsCasinoDisabled = true,
                ActorId = 1,
                ActorName = "ActorName",
                IsCasinoSync = true
            };
            aes.Decrypt(Arg.Any<string>(), Arg.Any<string>()).Returns("1234aaAA");
            encryptor.CfsEnCode(Arg.Is("1234aaAA")).Returns("EncodedPassword");

            var criteria = new ResetMainAccountPasswordCriteria
            {
                CustId = param.CustId,
                Language = param.LanguageKey,
                LoginIP = param.IP,
                UserPwd = "EncodedPassword",
                SubAccId = param.SubAccountId,
                DisCasino = param.IsCasinoDisabled,
                UserId = param.ActorId
            };

            passwordDataAccess.UpdatePassword(Arg.Any<ResetMainAccountPasswordCriteria>()).Returns(new Result { Code = 0 });

            // Act
            passwordService.ResetMainAccountPassword(param);

            // Assert
            passwordService.Received().ChangeCasinoAccountPassword(
                param.CustId,
                "EncodedPassword",
                param.ActorId,
                param.ActorName,
                param.IP);
        }

        #endregion Reset Main Account Password

        #region Reset Sub Account Password

        [Test]
        public void ResetSubAccountPassword_ParamIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            ResetSubAccountPasswordParam param = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => passwordService.ResetSubAccountPassword(param));
        }

        [TestCase("59D7!4$h")]
        [TestCase("493abcDE")]
        public void ResetSubAccountPassword_WrongRulePassword_ReturnsWrongRulePasswordResult(string password)
        {
            // Arrange
            var param = new ResetSubAccountPasswordParam();
            aes.Decrypt(Arg.Any<string>(), Arg.Any<string>()).Returns(password);

            // Act
            var result = passwordService.ResetSubAccountPassword(param);

            // Assert
            Assert.IsTrue(result.IsNotOk && result.Message == Security.lblPasswordWrongRule);
        }

        [Test]
        public void ResetSubAccountPassword_PasswordContainsActorName_ReturnsPasswordDAccountNameResult()
        {
            // Arrange
            var param = new ResetSubAccountPasswordParam();
            param.NewPassword = "1234aaAA";
            param.ActorName = "aaAA";

            aes.Decrypt(Arg.Any<string>(), Arg.Any<string>()).Returns("1234aaAA");

            // Act
            var result = passwordService.ResetSubAccountPassword(param);

            // Assert
            Assert.IsTrue(result.IsNotOk && result.Message == Security.lblPasswordDAccountName);
        }

        [Test]
        public void ResetSubAccountPassword_ValidatePasswordSuccessful_CallsUpdatePassword()
        {
            // Arrange
            var param = new ResetSubAccountPasswordParam
            {
                NewPassword = "1234aaAA",
                CustId = 1,
                LanguageKey = "en",
                IP = "127.0.0.1",
                SubAccountId = 0,
                ActorId = 1
            };
            aes.Decrypt(Arg.Any<string>(), Arg.Any<string>()).Returns("1234aaAA");
            encryptor.CfsEnCode(Arg.Is("1234aaAA")).Returns("EncodedPassword");

            var criteria = new ResetSubAccountPasswordCriteria
            {
                CustId = param.CustId,
                Language = param.LanguageKey,
                LoginIP = param.IP,
                UserPwd = "EncodedPassword",
                SubAccId = param.SubAccountId,
                UserId = param.ActorId
            };

            // Act
            passwordService.ResetSubAccountPassword(param);

            // Assert
            passwordDataAccess.Received().UpdatePassword(
                Arg.Is<ResetSubAccountPasswordCriteria>(
                    c => compareLogic.Compare(c, criteria).AreEqual));
        }

        #endregion Reset Sub Account Password
    }
}
