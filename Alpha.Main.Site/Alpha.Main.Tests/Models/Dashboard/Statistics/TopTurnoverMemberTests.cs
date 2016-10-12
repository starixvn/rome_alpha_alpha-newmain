namespace Alpha.Main.Tests.Models.Dashboard.Statistics
{
    using Main.Models.Statistics;
    using NUnit.Framework;
    using TestData.Helper;

    [TestFixture]
    public class TopTurnoverMemberTests
    {
        [Test]
        public void CustId_Always_SetValueFromCtor()
        {
            // Arrange
            int custId = 100;

            // Act
            var member = FakeDataHelper.Create<TopTurnoverMember>("custId", custId);

            // Assert
            Assert.AreEqual(custId, member.CustId);
        }

        [Test]
        public void UserName_Always_SetValueFromCtor()
        {
            // Arrange
            string userName = "member";

            // Act
            var member = FakeDataHelper.Create<TopTurnoverMember>("userName", userName);

            // Assert
            Assert.AreEqual(userName, member.UserName);
        }

        [Test]
        public void Turnover_Always_SetValueFromCtor()
        {
            // Arrange
            decimal turnover = 10;

            // Act
            var member = FakeDataHelper.Create<TopTurnoverMember>("turnOver", turnover);

            // Assert
            Assert.AreEqual(turnover, member.Turnover);
        }
    }
}