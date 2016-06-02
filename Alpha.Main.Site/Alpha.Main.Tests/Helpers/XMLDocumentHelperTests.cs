namespace Alpha.Main.Tests.Helpers
{
    using Main.Helpers;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.IO;
    using System.Xml;

    [TestFixture]
    public class XMLDocumentHelperTests
    {
        private string applicationBaseDirectory;

        [TestFixtureSetUp]
        public void Init()
        {
            applicationBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        }

        [Test]
        public void LoadDocumentFromPath_NullPath_ThrowArgumentNullException()
        {
            // Arrange
            string nullDocumentPath = null;

            // Action & Assert
            Assert.Throws<ArgumentNullException>(new TestDelegate(() => XMLDocumentHelper.LoadDocumentFromPath(nullDocumentPath)));
        }

        [Test]
        public void LoadDocumentFromPath_EmptyPath_ThrowArgumentException()
        {
            // Arrange
            string emptyDocumentPath = string.Empty;

            // Action & Assert
            Assert.Throws<ArgumentException>(new TestDelegate(() => XMLDocumentHelper.LoadDocumentFromPath(emptyDocumentPath)));
        }

        [TestCase(@"TestData\XMLDocumentHelper")]
        [TestCase(@"TestData\XMLDocumentHelper\NotExistXMLDocument.xml")]
        public void LoadDocumentFromPath_NotExistDirectoryPath_ThrowDirectoryNotFoundException(string documentPath)
        {
            // Arrange

            // Action & Assert
            Assert.Throws<DirectoryNotFoundException>(new TestDelegate(() => XMLDocumentHelper.LoadDocumentFromPath(documentPath)));
        }

        [Test]
        public void LoadDocumentFromPath_InvalidXMLFile_ThrowXmlException()
        {
            // Arrange
            var absoluteDocumentPath = Path.Combine(applicationBaseDirectory, @"TestData\XMLDocumentHelper\InvalidDocument.xml");

            // Action & Assert
            Assert.Throws<XmlException>(new TestDelegate(() => XMLDocumentHelper.LoadDocumentFromPath(absoluteDocumentPath)));
        }

        [TestCase(@"TestData\")]
        public void LoadDocumentFromPath_FileNotFoundPath_ThrowFileNotFoundException(string documentPath)
        {
            // Arrange

            // Action & Assert
            Assert.Throws<FileNotFoundException>(new TestDelegate(() => XMLDocumentHelper.LoadDocumentFromPath(documentPath)));
        }

        [TestCase(@"TestData\XMLDocumentHelper\ExampleXMLDocument.xml")]
        public void LoadDocumentFromPath_ExistFilePath_ReturnCorrectOuterXml(string relativeDocumentPath)
        {
            // Arrange
            var expectedDocument = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Root></Root>";
            var absoluteDocumentPath = Path.Combine(applicationBaseDirectory, relativeDocumentPath);

            // Action
            var document = XMLDocumentHelper.LoadDocumentFromPath(absoluteDocumentPath);

            // Assert
            Assert.AreEqual(expectedDocument, document.OuterXml);
        }

        [Test]
        public void AppendDocument_NullSource_ThrowArgumentNullException()
        {
            // Arrange
            XmlDocument documentSource = null;

            // Action & Assert
            Assert.Throws<ArgumentNullException>(new TestDelegate(() => XMLDocumentHelper.AppendDocument(documentSource, new XmlDocument())));
        }

        [Test]
        public void AppendDocument_HasDataSourceAndNullTarget_ThrowArgumentNullException()
        {
            // Arrange
            XmlDocument documentTarget = null;

            var absoluteDocumentSourcePath = Path.Combine(applicationBaseDirectory, @"TestData\XMLDocumentHelper\ExampleXMLDocument.xml");
            XmlDocument documentSource = XMLDocumentHelper.LoadDocumentFromPath(absoluteDocumentSourcePath);

            // Action & Assert
            Assert.Throws<ArgumentNullException>(new TestDelegate(() => XMLDocumentHelper.AppendDocument(documentSource, documentTarget)));
        }

        [Test]
        public void AppendDocument_SourceWithNoElement_ReturnTargetOuterXml()
        {
            // Arrange
            var absoluteDocumentSourcePath = Path.Combine(applicationBaseDirectory, @"TestData\XMLDocumentHelper\ExampleXMLDocument.xml");
            XmlDocument documentSource = XMLDocumentHelper.LoadDocumentFromPath(absoluteDocumentSourcePath);
            documentSource.RemoveAll();

            var absoluteDocumentTargetPath = Path.Combine(applicationBaseDirectory, @"TestData\XMLDocumentHelper\ExampleXMLDocument.xml");
            var documentTarget = XMLDocumentHelper.LoadDocumentFromPath(absoluteDocumentTargetPath);

            var expectedDocument = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Root></Root>";

            // Action
            XMLDocumentHelper.AppendDocument(documentSource, documentTarget);

            // Assert
            Assert.AreEqual(expectedDocument, documentTarget.OuterXml);
        }

        [Test]
        public void AppendDocument_TargetWithNoElement_ReturnEmptyOuterXml()
        {
            // Arrange
            var absoluteDocumentSourcePath = Path.Combine(applicationBaseDirectory, @"TestData\XMLDocumentHelper\ExampleXMLDocument.xml");
            XmlDocument documentSource = XMLDocumentHelper.LoadDocumentFromPath(absoluteDocumentSourcePath);

            var absoluteDocumentTargetPath = Path.Combine(applicationBaseDirectory, @"TestData\XMLDocumentHelper\ExampleXMLDocument.xml");
            var documentTarget = XMLDocumentHelper.LoadDocumentFromPath(absoluteDocumentTargetPath);
            documentTarget.RemoveAll();

            var expectedDocument = string.Empty;

            // Action
            XMLDocumentHelper.AppendDocument(documentSource, documentTarget);

            // Assert
            Assert.AreEqual(expectedDocument, documentTarget.OuterXml);
        }

        [Test]
        public void AppendDocument_SourceAndTargetWithTheSameElement_ReturnCorrectOuterXml()
        {
            // Arrange
            var absoluteDocumentSourcePath = Path.Combine(applicationBaseDirectory, @"TestData\XMLDocumentHelper\ExampleXMLDocument.xml");
            XmlDocument documentSource = XMLDocumentHelper.LoadDocumentFromPath(absoluteDocumentSourcePath);

            var absoluteDocumentTargetPath = Path.Combine(applicationBaseDirectory, @"TestData\XMLDocumentHelper\ExampleXMLDocument.xml");
            var documentTarget = XMLDocumentHelper.LoadDocumentFromPath(absoluteDocumentTargetPath);

            var expectedDocument = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Root></Root>";

            // Action
            XMLDocumentHelper.AppendDocument(documentSource, documentTarget);

            // Assert
            Assert.AreEqual(expectedDocument, documentTarget.OuterXml);
        }

        [Test]
        public void AppendDocument_SourceAndTargetWithDifferentRootElements_ReturnTargetOuterXml()
        {
            // Arrange
            var absoluteDocumentTargetPath = Path.Combine(applicationBaseDirectory, @"TestData\XMLDocumentHelper\ExampleXMLDocument.xml");
            var documentTarget = XMLDocumentHelper.LoadDocumentFromPath(absoluteDocumentTargetPath);

            var absoluteDocumentSourcePath = Path.Combine(applicationBaseDirectory, @"TestData\XMLDocumentHelper\DifferentRootElement.xml");
            XmlDocument documentSource = XMLDocumentHelper.LoadDocumentFromPath(absoluteDocumentSourcePath);

            var expectedDocument = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Root></Root>";

            // Action
            XMLDocumentHelper.AppendDocument(documentSource, documentTarget);

            // Assert
            Assert.AreEqual(expectedDocument, documentTarget.OuterXml);
        }

        [Test]
        public void AppendDocument_SourceAndTargetSameRootAndDifferentElements_CombinedOuterXml()
        {
            // Arrange
            var absoluteDocumentTargetPath = Path.Combine(applicationBaseDirectory, @"TestData\XMLDocumentHelper\SameRootAndDifferentElements_Target.xml");
            var documentTarget = XMLDocumentHelper.LoadDocumentFromPath(absoluteDocumentTargetPath);

            var absoluteDocumentSourcePath = Path.Combine(applicationBaseDirectory, @"TestData\XMLDocumentHelper\SameRootAndDifferentElements_Source.xml");
            XmlDocument documentSource = XMLDocumentHelper.LoadDocumentFromPath(absoluteDocumentSourcePath);

            var expectedDocument = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Root><SubNode2></SubNode2><SubNode1 /></Root>";

            // Action
            XMLDocumentHelper.AppendDocument(documentSource, documentTarget);

            // Assert
            Assert.AreEqual(expectedDocument, documentTarget.OuterXml);
        }

        [Test]
        public void AppendDocument_SourceAndTargetSameRootAndSameElements_CombinedOuterXml()
        {
            // Arrange
            var absoluteDocumentTargetPath = Path.Combine(applicationBaseDirectory, @"TestData\XMLDocumentHelper\SameRootAndSameElements.xml");
            var documentTarget = XMLDocumentHelper.LoadDocumentFromPath(absoluteDocumentTargetPath);

            var absoluteDocumentSourcePath = Path.Combine(applicationBaseDirectory, @"TestData\XMLDocumentHelper\SameRootAndSameElements.xml");
            XmlDocument documentSource = XMLDocumentHelper.LoadDocumentFromPath(absoluteDocumentSourcePath);

            var expectedDocument = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Root><SubNode1></SubNode1><SubNode1 /></Root>";

            // Action
            XMLDocumentHelper.AppendDocument(documentSource, documentTarget);

            // Assert
            Assert.AreEqual(expectedDocument, documentTarget.OuterXml);
        }
    }
}
