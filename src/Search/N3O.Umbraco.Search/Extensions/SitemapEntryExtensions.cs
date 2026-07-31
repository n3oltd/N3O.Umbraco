using N3O.Umbraco.Extensions;
using N3O.Umbraco.Search.Models;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace N3O.Umbraco.Search.Extensions;

public static class SitemapEntryExtensions {
    public const string XDefault = "x-default";

    private const string SitemapNamespace = "http://www.sitemaps.org/schemas/sitemap/0.9";
    private const string XhtmlNamespace = "http://www.w3.org/1999/xhtml";

    public static string ToXml(this IEnumerable<SitemapEntry> entries) {
        var entryLinks = entries.OrEmpty().Select(x => (Entry: x, HrefLangLinks: GetHrefLangLinks(x))).ToList();

        using (var stream = new MemoryStream()) {
            var settings = GetWriterSettings();

            using (var writer = XmlWriter.Create(stream, settings)) {
                writer.WriteStartDocument();
                writer.WriteStartElement("urlset", SitemapNamespace);

                if (entryLinks.Any(x => x.HrefLangLinks.Count > 1)) {
                    writer.WriteAttributeString("xmlns", "xhtml", null, XhtmlNamespace);
                }

                foreach (var (entry, hrefLangLinks) in entryLinks) {
                    writer.WriteStartElement("url");
                    writer.WriteElementString("loc", entry.Url);

                    if (entry.LastModified.HasValue()) {
                        writer.WriteElementString("lastmod", FormatDate(entry.LastModified.GetValueOrThrow()));
                    }

                    if (hrefLangLinks.Count > 1) {
                        foreach (var (hrefLang, url) in hrefLangLinks) {
                            writer.WriteStartElement("xhtml", "link", XhtmlNamespace);
                            writer.WriteAttributeString("rel", "alternate");
                            writer.WriteAttributeString("hreflang", hrefLang);
                            writer.WriteAttributeString("href", url);
                            writer.WriteEndElement();
                        }
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

    public static string ToSitemapIndexXml(this IEnumerable<SitemapIndexEntry> sitemaps) {
        using (var stream = new MemoryStream()) {
            var settings = GetWriterSettings();

            using (var writer = XmlWriter.Create(stream, settings)) {
                writer.WriteStartDocument();
                writer.WriteStartElement("sitemapindex", SitemapNamespace);

                foreach (var sitemap in sitemaps) {
                    writer.WriteStartElement("sitemap");
                    writer.WriteElementString("loc", sitemap.Location);

                    if (sitemap.LastModified.HasValue()) {
                        writer.WriteElementString("lastmod", FormatDate(sitemap.LastModified.GetValueOrThrow()));
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

    public static string ToHrefLang(this string culture) {
        if (!culture.HasValue()) {
            return null;
        }

        if (string.Equals(culture, XDefault, StringComparison.OrdinalIgnoreCase)) {
            return XDefault;
        }

        return culture.ToLowerInvariant();
    }

    private static IReadOnlyDictionary<string, string> GetHrefLangLinks(SitemapEntry entry) {
        var links = new Dictionary<string, string>();

        if (!entry.Culture.HasValue()) {
            return links;
        }

        links[entry.Culture.ToHrefLang()] = entry.Url;

        foreach (var alternate in entry.AlternateUrls.OrEmpty()) {
            links[alternate.Key.ToHrefLang()] = alternate.Value;
        }

        return links;
    }

    private static string FormatDate(LocalDate date) {
        return date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
    }

    private static XmlWriterSettings GetWriterSettings() {
        var settings = new XmlWriterSettings();

        settings.Encoding = new UTF8Encoding(false);
        settings.Indent = true;
        settings.OmitXmlDeclaration = false;

        return settings;
    }
}
