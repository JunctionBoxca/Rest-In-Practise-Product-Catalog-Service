using System.IO;
using System.Xml;
using Restbucks.Xml;

namespace ProductCatalog.Notifications.Model
{
    public class RewriteEntityBody : IEntityBodyTransformationStrategy
    {
        private readonly Stream source;

        public RewriteEntityBody(Stream source)
        {
            this.source = source;
        }

        public void WriteEntityBody(Stream destination)
        {
            using (source)
            {
                Xml.CopyXml(source, destination, RewriteLinks);
            }
        }

        private static void RewriteLinks(XmlReader reader, XmlWriter writer)
        {
            if (IsAtSelfLink(reader))
            {
                reader.Skip();
            }
            else if (IsAtViaLink(reader))
            {
                string href = reader.GetAttribute("href");
                reader.Skip();
                writer.WriteStartElement("link", "http://www.w3.org/2005/Atom");
                writer.WriteAttributeString("rel", "self");
                writer.WriteAttributeString("href", href);
                writer.WriteEndElement();
                writer.Flush();
            }
        }

        private static bool IsAtSelfLink(XmlReader reader)
        {
            return IsAtLinkWithRelValue(reader, "self");
        }

        private static bool IsAtViaLink(XmlReader reader)
        {
            return IsAtLinkWithRelValue(reader, "via");
        }

        private static bool IsAtLinkWithRelValue(XmlReader reader, string relValue)
        {
            return reader.IsStartElement("link") && (reader.GetAttribute("rel") != null) && reader.GetAttribute("rel").Equals(relValue);
        }
    }
}