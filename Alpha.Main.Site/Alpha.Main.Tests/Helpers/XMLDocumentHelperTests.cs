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
            Assert.Throws<NullReferenceException>(new TestDelegate(() => XMLDocumentHelper.LoadDocumentFromPath(documentPath)));
        }
    }
}
