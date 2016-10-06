namespace Alpha.Main.Tests.Criteria.Statistics
{
    using Main.Criteria.Statistics;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class GetNewMembersCriteriaTest
    {
        [Test]
        public void GetSettingKey_Always_Return_Age_CustomerSelNewMembers()
        {
            // Arrange
            var criteria = new GetNewMembersCriteria(100);

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual("Age_CustomerSelNewMembers", settingKey);
        }

        [Test]
        public void CustId_Always_ReturnValueSetFromCtor()
        {
            // Arrange
            var custId = 69;
            var criteria = new GetNewMembersCriteria(custId);

            // Act & Assert
            Assert.AreEqual(custId, criteria.CustId);
        }

        public void IsValid_CustIdIsGreaterThanZero_ReturnTrue()
        {
            // Arrange          
            int custId = 69;
            var criteria = new GetNewMembersCriteria(custId);

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
            var criteria = new GetNewMembersCriteria(custId);

            // Act
            bool actual = criteria.IsValid();

            /// Assert           
            Assert.AreEqual(false, actual);
        }
    }
}
