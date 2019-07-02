namespace Alpha.Main.Tests.Criteria
{
    using Main.Criteria;
    using Main.Models.Enumerations;
    using NUnit.Framework;
    using Params;

    [TestFixture]
    public class KickoutCustomerCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_Return_CorrectSPName()
        {
            // Arrange
            var criteria = new KickoutCustomerCriteria(GetKickoutCustomerParam());

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual("Age_OnUser_ForceKickedout", settingKey);
        }

        [Test]
        public void Constructor_Always_SetCorrectProperties()
        {
            // Arrange
            var criteria = new KickoutCustomerCriteria(GetKickoutCustomerParam());

            // Act & Assert
            Assert.True(
                criteria.CustId == 1
                && criteria.RoleId == UserLevel.Master.Value
                && criteria.ServerId == 90
                && criteria.IP == "::1"
                && criteria.SubAccId == 0);
        }

        [TestCase(-1, false)]
        [TestCase(0, false)]
        [TestCase(8, true)]
        public void IsValid_CustId_ReturnCorrectValue(int custId, bool expected)
        {
            // Arrange
            var param = new KickoutCustomerParam(custId, UserLevel.Master, 90, "::1", 0);
            var criteria = new KickoutCustomerCriteria(param);

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestCase(-1, false)]
        [TestCase(0, false)]
        [TestCase(90, true)]
        public void IsValid_ServerId_ReturnCorrectValue(int serverId, bool expected)
        {
            // Arrange
            var param = new KickoutCustomerParam(1, UserLevel.Master, serverId, "::1", 0);
            var criteria = new KickoutCustomerCriteria(param);

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        private static KickoutCustomerParam GetKickoutCustomerParam()
            => new KickoutCustomerParam(1, UserLevel.Master, 90, "::1", 0);
    }
}
