namespace Alpha.Main.Tests.Criteria.BalanceInfo
{
    using System;
    using Main.Constants;
    using Main.Criteria.BalanceInfo;
    using Main.Models.Enumerations;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class GetBalanceInfoCriteriaTest
    {
        [TestCase(RoleId.Agent, "Age_CustomerSelBalanceInfo_Ag")]
        [TestCase(RoleId.Master, "Age_CustomerSelBalanceInfo_MAg")]
        [TestCase(RoleId.Super, "Age_CustomerSelBalanceInfo_SAg")]
        public void GetSettingKey_Always_ReturnValidSettingKeyByLevel(byte userLevelValue, string settingKey)
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(userLevelValue);
            var criteria = new GetBalanceInfoCriteria(userLevel, 100, DateTime.Now);

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual(settingKey, actualSettingKey);
        }

        [Test]
        public void IsValid_CustIdIsGreaterThanZero_ReturnTrue()
        {
            // Arrange          
            int custId = 69;  
            var criteria = new GetBalanceInfoCriteria(UserLevel.Agent, custId, DateTime.Now);

            // Act
            bool actual = criteria.IsValid();

            /// Assert           
            Assert.AreEqual(true, actual);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void IsValid_CustIdIsLessThanOrEqualToZero_ReturnFalse(int custId)
        {
            // Arrange            
            var criteria = new GetBalanceInfoCriteria(UserLevel.Agent, custId, DateTime.Now);

            // Act
            bool actual = criteria.IsValid();

            /// Assert           
            Assert.AreEqual(false, actual);
        }

        [Test]
        public void MoveBetsDay_Always_ReturnValueSetFromCtor()
        {
            // Arrange
            var moveBetsDay = new DateTime(1981, 2, 15);
            var criteria = new GetBalanceInfoCriteria(UserLevel.Agent, 69, moveBetsDay);

            // Act & Assert
            Assert.AreEqual(moveBetsDay, criteria.MoveBetsDay);
        }

        [Test]
        public void CustId_Always_ReturnValueSetFromCtor()
        {
            // Arrange
            var custId = 69;
            var criteria = new GetBalanceInfoCriteria(UserLevel.Agent, custId, DateTime.Now);
                
            // Act & Assert
            Assert.AreEqual(custId, criteria.CustId);
        }
    }
}
