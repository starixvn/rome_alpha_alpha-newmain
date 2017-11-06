namespace Alpha.Main.Tests.DataAccesses
{
    using System;
    using System.Collections;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using KellermanSoftware.CompareNetObjects;
    using Main.Criteria.Password;
    using Main.DataAccesses.Impl;
    using Main.Models;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class PasswordDataAccessImplTests
    {
        private CompareLogic compareLogic;
        private IObjectDb objectDb;
        private PasswordDataAccessImpl passwordDataAccess;

        [SetUp]
        public void SetUp()
        {
            DbSettingProviderManager.StartTestingSession().Run();

            objectDb = Substitute.For<IObjectDb>();
            ObjectDbFactory.Mock((settingKey, commandTimeout) => objectDb);

            compareLogic = new CompareLogic();
            passwordDataAccess = new PasswordDataAccessImpl();
        }

        [Test]
        public void UpdatePassword_NullCriteria_ThrowsArgumentNullException()
        {
            // Arrange
            ICriteria criteria = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => passwordDataAccess.UpdatePassword(criteria));
        }

        [Test]
        public void UpdatePassword_UpdateSuccessfully_ReturnsSuccessedResult()
        {
            // Arrange
            var expectedResult = new Result
            {
                Code = 0,
                Message = "Successfully updated."
            };

            var outputParams = new Hashtable()
            {
                ["errcode"] = 0,
                ["errmsg"] = string.Empty
            };
            objectDb.GetOuputParams().Returns(outputParams);

            // Act
            var actualResult = passwordDataAccess.UpdatePassword(new UpdateMainAccountPasswordCriteria());

            // Assert
            Assert.IsTrue(compareLogic.Compare(expectedResult, actualResult).AreEqual);
        }

        [Test]
        public void UpdatePassword_UpdateSuccessfully_ReturnsFailedResult()
        {
            // Arrange
            var expectedResult = new Result
            {
                Code = 1,
                Message = "ErrorMessage"
            };

            var outputParams = new Hashtable()
            {
                ["errcode"] = 1,
                ["errmsg"] = "ErrorMessage"
            };
            objectDb.GetOuputParams().Returns(outputParams);

            // Act
            var actualResult = passwordDataAccess.UpdatePassword(new UpdateSubAccountPasswordCriteria());

            // Assert
            Assert.IsTrue(compareLogic.Compare(expectedResult, actualResult).AreEqual);
        }
    }
}
