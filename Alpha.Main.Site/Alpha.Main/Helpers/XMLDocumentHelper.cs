namespace Alpha.Main.Helpers
{
    using System.Xml;

    public static class XMLDocumentHelper
    {
        public static XmlDocument LoadDocumentFromPath(string documentPath)
        {
            var xmlAnnouncement = new XmlDocument();
            xmlAnnouncement.Load(documentPath);
            return xmlAnnouncement;
        }

        public static void AppendDocument(XmlDocument documentSource, XmlDocument documentTarget)
        {
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