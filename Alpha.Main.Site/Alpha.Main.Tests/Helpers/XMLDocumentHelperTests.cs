namespace Alpha.Main.Tests.Helpers
{
    using Main.Helpers;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class XMLDocumentHelperTests
    {
        [Test]
        public void LoadDocumentFromPath_NullPath_ArgumentNullException()
        {
            // Arrange
            string documentPath = null;

            // Action & Assert
            Assert.Throws<ArgumentNullException>(new TestDelegate(() => XMLDocumentHelper.LoadDocumentFromPath(documentPath)));
        }

        [Test]
        public void LoadDocumentFromPath_NullPath_ArgumentException()
        {
            // Arrange
            string documentPath = string.Empty;

            // Action & Assert
            Assert.Throws<ArgumentException>(new TestDelegate(() => XMLDocumentHelper.LoadDocumentFromPath(documentPath)));
        }
    }
}
