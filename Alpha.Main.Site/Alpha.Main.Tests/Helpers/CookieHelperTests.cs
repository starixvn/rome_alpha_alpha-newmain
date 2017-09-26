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
        public void GetCookieNameOfCurrentPage_AnyCustIdAndAnySubAccIdWithoutEncodeData_ReturnsRawCombinedString(
            int custId,
            int subAccId)
        {
            // Arrange
            var expectedCookieNameOfCurrentPage = $"{custId}_{subAccId}_NewMain";

            // Act
            var actualCookieNameOfCurrentPage = CookieHelper.GetCookieNameOfCurrentPage(custId, subAccId, null);

            // Assert
            Assert.AreEqual(expectedCookieNameOfCurrentPage, actualCookieNameOfCurrentPage);
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
        public void GetCookieNameOfCurrentPage_AnyCustIdAndAnySubAccIdAndEncodeData_ReturnsRawCombinedString(
            int custId,
            int subAccId)
        {
            // Arrange
            var expectedCookieNameOfCurrentPage = "encodedData";
            Func<string, string> encodeDataFunction = (currentUrlCookieName) => expectedCookieNameOfCurrentPage;

            // Act
            var actualCookieNameOfCurrentPage = CookieHelper.GetCookieNameOfCurrentPage(custId, subAccId, encodeDataFunction);

            // Assert
            Assert.AreEqual(expectedCookieNameOfCurrentPage, actualCookieNameOfCurrentPage);
        }
    }
}
