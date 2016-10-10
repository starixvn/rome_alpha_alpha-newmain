namespace Alpha.Main.Tests.Criteria.Message
{
    using System;
    using Main.Constants;
    using Main.Criteria.Message;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class GetMessageCriteriaTests
    {
        [TestCase(MessageType.Private, "Dot_Msg_GetPrivateMessage")]
        [TestCase(MessageType.Public, "Dot_Msg_GetPublicMessage")]
        [TestCase(MessageType.Special, "Dot_Msg_GetSpecialMessage")]
        public void GetSettingKey_Always_ReturnValidSettingKeyByLevel(byte messageType, string settingKey)
        {
            // Arrange
            var criteria = new GetMessageCriteria(
                Arg.Any<int>(),
                Arg.Any<string>(),
                Arg.Any<int>(),
                Arg.Any<int>(),
                Arg.Any<DateTime>(),
                Arg.Any<DateTime>(),
                Arg.Any<bool>(),
                messageType);

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual(settingKey, actualSettingKey);
        }

        [Test]
        public void IsValid_CustIdIsGreaterThanZeroAndParentSitesIsNotEmpty_ReturnTrue()
        {
            // Arrange
            var custId = 69;
            var parentSites = "1";

            var criteria = new GetMessageCriteria(
                custId,
                parentSites,
                Arg.Any<int>(),
                Arg.Any<int>(),
                Arg.Any<DateTime>(),
                Arg.Any<DateTime>(),
                Arg.Any<bool>(),
                Arg.Any<int>());

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.AreEqual(true, actual);
        }

        [TestCase(0, "")]
        [TestCase(0, null)]
        [TestCase(-1, "")]
        [TestCase(-1, null)]
        public void IsValid_CustIdIsLessThanOrEqualToZeroAndParentSitesIsEmpty_ReturnFalse(int custId, string parentSites)
        {
            // Arrange
            var criteria = new GetMessageCriteria(
                custId,
                parentSites,
                Arg.Any<int>(),
                Arg.Any<int>(),
                Arg.Any<DateTime>(),
                Arg.Any<DateTime>(),
                Arg.Any<bool>(),
                Arg.Any<int>());

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.AreEqual(false, actual);
        }

        [Test]
        public void AllProperties_Always_ReturnValuesSetFromCtor()
        {
            // Arrange
            var custId = 69;
            var parentSites = "1";
            var specialType = 1;
            var totalMessage = 50;
            var startDate = DateTime.Today;
            var endDate = DateTime.Today;
            var isTest = false;

            var criteria = new GetMessageCriteria(
                custId,
                parentSites,
                specialType,
                totalMessage,
                startDate,
                endDate,
                isTest,
                Arg.Any<int>());

            // Act & Assert
            Assert.IsTrue(custId == criteria.CustId &&
                          parentSites == criteria.ParentSites &&
                          specialType == criteria.Special &&
                          totalMessage == criteria.TotalMessasge &&
                          startDate == criteria.StartDate &&
                          endDate == criteria.EndDate &&
                          isTest == criteria.IsTest);
        }
    }
}