using Alpha.Main.Criteria.Password;
using Alpha.Main.DataAccesses;
using Alpha.Main.Models;
using Alpha.Main.Params.Password;
using Alpha.Main.Services.Impl;
using NSubstitute;
using NUnit.Framework;
using Sunplus.Agent.Model.Casino;
using Sunplus.Agent.Utilities;
using System;

namespace Alpha.Main.Tests.Services
{
    [TestFixture]
    public class PasswordServiceImplTests
    {
        private PasswordServiceImpl passwordService;
        private Encryptor encryptor;
        private PasswordDataAccess passwordDataAccess;
        private CasinoService casinoService;

        [SetUp]
        public void SetUp()
        {
            encryptor = Substitute.For<Encryptor>();
            passwordDataAccess = Substitute.For<PasswordDataAccess>();
            casinoService = Substitute.For<CasinoService>();

            passwordService = new PasswordServiceImpl(
                encryptor,
                passwordDataAccess,
                casinoService);
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

        [Test]
        public void ChangeMainAccountPassword_ValidatePasswordFailed_ReturnsIsNotOkResult()
        {
            // Arrange
            var param = new ChangeMainAccountPasswordParam
            {
                NewPassword = string.Empty,
            };

            encryptor.RC4(Arg.Any<string>(), Arg.Any<string>()).Returns(string.Empty);

            // Act 
            var result = passwordService.ChangeMainAccountPassword(param);

            // Assert
            Assert.IsTrue(result.IsNotOk);
        }

        [Test]
        public void ChangeMainAccountPassword_ValidatePasswordFailed_CallsUpdatePassword()
        {
            // Arrange
            var newPassword = "1234aaAA";
            var currentPassword = "1234aa";
            var param = new ChangeMainAccountPasswordParam
            {
                NewPassword = newPassword,
                CurrentPassword = currentPassword,
            };


            encryptor.HexDecode(Arg.Is(param.CurrentPassword)).Returns("CurrentPassword");
            encryptor.HexDecode(Arg.Is(param.NewPassword)).Returns("NewPassword");

            encryptor.RC4(Arg.Any<string>(), Arg.Is("CurrentPassword")).Returns(currentPassword);
            encryptor.RC4(Arg.Any<string>(), Arg.Is("NewPassword")).Returns(newPassword);

            encryptor.CfsEnCode(Arg.Is(currentPassword)).Returns("OldPassword");
            encryptor.CfsEnCode(Arg.Is(newPassword)).Returns("UserPassword");

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

        #endregion

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

            encryptor.RC4(Arg.Any<string>(), Arg.Any<string>()).Returns(string.Empty);

            // Act 
            var result = passwordService.ChangeSubAccountPassword(param);

            // Assert
            Assert.IsTrue(result.IsNotOk);
        }

        [Test]
        public void ChangeSubAccountPassword_ValidatePasswordFailed_CallsUpdatePassword()
        {
            // Arrange
            var newPassword = "1234aaAA";
            var currentPassword = "1234aa";
            var param = new ChangeSubAccountPasswordParam
            {
                NewPassword = newPassword,
                CurrentPassword = currentPassword,
            };


            encryptor.HexDecode(Arg.Is(param.CurrentPassword)).Returns("CurrentPassword");
            encryptor.HexDecode(Arg.Is(param.NewPassword)).Returns("NewPassword");

            encryptor.RC4(Arg.Any<string>(), Arg.Is("CurrentPassword")).Returns(currentPassword);
            encryptor.RC4(Arg.Any<string>(), Arg.Is("NewPassword")).Returns(newPassword);

            encryptor.CfsEnCode(Arg.Is(currentPassword)).Returns("OldPassword");
            encryptor.CfsEnCode(Arg.Is(newPassword)).Returns("UserPassword");

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

        #endregion

        #region Reset Main Account Password

        [Test]
        public void ResetMainAccountPassword_ParamIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            ResetMainAccountPasswordParam param = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => passwordService.ResetMainAccountPassword(param));
        }

        #endregion

        #region Reset Sub Account Password

        [Test]
        public void ResetSubAccountPassword_ParamIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            ResetSubAccountPasswordParam param = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => passwordService.ResetSubAccountPassword(param));
        }

        #endregion
    }
}
