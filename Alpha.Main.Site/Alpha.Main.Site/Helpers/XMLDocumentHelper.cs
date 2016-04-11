namespace Alpha.Main.Site.Helpers
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
            if (documentSource.DocumentElement == null)
            {
                return;
            }

            foreach (XmlNode childNode in documentSource.DocumentElement.ChildNodes)
            {
                var newNode = documentTarget.ImportNode(childNode, true);
                if (documentTarget.DocumentElement == null)
                {
                    continue;
                }

                documentTarget.DocumentElement.AppendChild(newNode);
            }
        }
    }
}