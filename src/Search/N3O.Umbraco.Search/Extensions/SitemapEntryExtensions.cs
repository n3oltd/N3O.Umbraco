using N3O.Umbraco.Extensions;
using N3O.Umbraco.Search.Models;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace N3O.Umbraco.Search.Extensions;

public static class SitemapEntryExtensions {
    public static string ToXml(this IEnumerable<SitemapEntry> entries) {
        using (var stream = new MemoryStream()) {
            var settings = new XmlWriterSettings();

            settings.Encoding = Encoding.UTF8;
            settings.Indent = true;
            settings.OmitXmlDeclaration = false;
            
            using (var writer = XmlWriter.Create(stream, settings)) {
                writer.WriteStartDocument();
                writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");

                foreach (var entry in entries) {
                    writer.WriteStartElement("url");
                    writer.WriteElementString("loc", entry.Url);
                    writer.WriteElementString("lastmod", entry.LastModified.ToString("yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture));
                    writer.WriteElementString("changefreq", entry.ChangeFrequency);
                    writer.WriteElementString("priority", entry.Priority.ToString(CultureInfo.InvariantCulture));

                    foreach (var (cultureCode, url) in entry.CultureVariantUrls.OrEmpty()) {
                        writer.WriteStartElement("xhtml:link");
                        writer.WriteAttributeString("rel", "alternate");
                        writer.WriteAttributeString("hreflang", cultureCode);
                        writer.WriteAttributeString("href", url);
                    }
                    
                    writer.WriteEndElement();
                }

                writer.WriteEndElement(); 
                writer.WriteEndDocument();
                writer.Flush();

                stream.Rewind();
                
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }
    }
}