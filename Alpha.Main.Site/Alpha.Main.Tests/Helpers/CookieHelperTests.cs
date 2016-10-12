namespace Alpha.Main.Tests.Helpers
{
    using System;
    using Main.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class CookieHelperTests
    {
        [TestCase(-1, -1)]
        [TestCase(-1, 0)]
        [TestCase(-1, 1)]
        [TestCase(0, -1)]
        [TestCase(0, 0)]
        [TestCase(0, 1)]
        [TestCase(1, -1)]
        [TestCase(1, 0)]
        [TestCase(1, 1)]
        [TestCase(int.MinValue, int.MinValue)]
        [TestCase(int.MaxValue, int.MaxValue)]
        public void GetCurrentUrlCookieName_AnyCustIdAndAnySubAccIdWithoutEncodeData_ReturnsRawCombinedString(
            int custId,
            int subAccId)
        {
            // Arrange
            var expectedCurrentUrlCookieName = $"{custId}_{subAccId}_NewMain";

            // Act
            var actualCurrentUrlCookieName = CookieHelper.GetCurrentUrlCookieName(custId, subAccId, null);

            // Assert
            Assert.AreEqual(expectedCurrentUrlCookieName, actualCurrentUrlCookieName);
        }

        [TestCase(-1, -1)]
        [TestCase(-1, 0)]
        [TestCase(-1, 1)]
        [TestCase(0, -1)]
        [TestCase(0, 0)]
        [TestCase(0, 1)]
        [TestCase(1, -1)]
        [TestCase(1, 0)]
        [TestCase(1, 1)]
        [TestCase(int.MinValue, int.MinValue)]
        [TestCase(int.MaxValue, int.MaxValue)]
        public void GetCurrentUrlCookieName_AnyCustIdAndAnySubAccIdAndEncodeData_ReturnsRawCombinedString(
            int custId,
            int subAccId)
        {
            // Arrange
            var expectedCurrentUrlCookieName = "encodedData";
            Func<string, string> encodeDataFunction = (currentUrlCookieName) => expectedCurrentUrlCookieName;

            // Act
            var actualCurrentUrlCookieName = CookieHelper.GetCurrentUrlCookieName(custId, subAccId, encodeDataFunction);

            // Assert
            Assert.AreEqual(expectedCurrentUrlCookieName, actualCurrentUrlCookieName);
        }
    }
}
