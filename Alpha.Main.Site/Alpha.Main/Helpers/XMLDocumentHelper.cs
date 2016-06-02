namespace Alpha.Main.Helpers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Xml;

    public static class XMLDocumentHelper
    {
        [SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", Justification = "Reviewed!")]
        public static XmlDocument LoadDocumentFromPath(string documentPath)
        {
            var xmlAnnouncement = new XmlDocument();
            xmlAnnouncement.Load(documentPath);
            return xmlAnnouncement;
        }

        [SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", Justification = "Reviewed!")]
        public static void AppendDocument(XmlDocument documentSource, XmlDocument documentTarget)
        {
            if (documentSource == null)
            {
                throw new ArgumentNullException(nameof(documentSource), "Document Source cannot be null");
            }

            if (documentTarget == null)
            {
                throw new ArgumentNullException(nameof(documentTarget), "Document Target cannot be null");
            }

            if (documentSource.DocumentElement == null || documentTarget.DocumentElement == null)
            {
                return;
            }

            foreach (XmlNode childNode in documentSource.DocumentElement.ChildNodes)
            {
                var newNode = documentTarget.ImportNode(childNode, true);
                documentTarget.DocumentElement.AppendChild(newNode);
            }
        }
    }
}