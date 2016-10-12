namespace Alpha.Main.Tests.Models.Dashboard.Statistics
{
    using System;
    using Helpers;
    using Main.Models.Statistics;
    using NUnit.Framework;

    [TestFixture]
    public class NewMemberTests
    {
        [Test]
        public void CustId_Always_SetValueFromCtor()
        {
            // Arrange
            int custId = 100;

            // Act
            var member = FakeDataHelper.Create<NewMember>("custId", custId);

            // Assert
            Assert.AreEqual(custId, member.CustId);
        }

        [Test]
        public void UserName_Always_SetValueFromCtor()
        {
            // Arrange
            string userName = "member";

            // Act
            var member = FakeDataHelper.Create<NewMember>("userName", userName);

            // Assert
            Assert.AreEqual(userName, member.UserName);
        }

        [Test]
        public void GivenCredit_Always_SetValueFromCtor()
        {
            // Arrange
            decimal givenCredit = 10;

            // Act
            var member = FakeDataHelper.Create<NewMember>("givenCredit", givenCredit);

            // Assert
            Assert.AreEqual(givenCredit, member.GivenCredit);
        }

        [Test]
        public void CreatedDate_Always_SetValueFromCtor()
        {
            // Arrange
            DateTime createdDate = DateTime.Today;

            // Act
            var member = FakeDataHelper.Create<NewMember>("createdDate", createdDate);

            // Assert
            Assert.AreEqual(createdDate, member.CreatedDate);
        }
    }
}