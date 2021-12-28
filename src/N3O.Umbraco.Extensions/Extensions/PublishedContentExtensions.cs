using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace N3O.Umbraco.Extensions {
    public static class PublishedContentExtensions {
        public static string AbsoluteUrl(this IPublishedContent content) {
            return content.Url(mode: UrlMode.Absolute);
        }
    
        public static T Child<T>(this IPublishedContent content)
            where T : class, IPublishedContent {
            return Child<T>(content, _ => true);
        }
    
        public static T Child<T>(this IPublishedContent content, Func<T, bool> filter)
            where T : class, IPublishedContent {
            return Children(content, filter).FirstOrDefault();
        }

        public static IEnumerable<T> Children<T>(this IPublishedContent content)
            where T : class, IPublishedContent {
            return Children<T>(content, _ => true);
        }
    
        public static IEnumerable<T> Children<T>(this IPublishedContent content, Func<T, bool> filter)
            where T : class, IPublishedContent {
            return content.Children<T>().Where(filter);
        }

        public static T Descendant<T>(this IPublishedContent content) where T : class, IPublishedContent {
            return Descendant<T>(content, x => true);
        }
    
        public static T Descendant<T>(this IPublishedContent content, Func<T, bool> filter)
            where T : class, IPublishedContent {
            return Descendants(content, filter).FirstOrDefault();
        }
    
        public static IEnumerable<T> Descendants<T>(this IPublishedContent content, Func<T, bool> filter)
            where T : class, IPublishedContent {
            return content.Descendants().OfType<T>().Where(filter);
        }

        public static bool HasTemplate(this IPublishedContent content) {
            return content.TemplateId != 0;
        }

        public static string RelativeUrl(this IPublishedContent content) {
            return content.Url(mode: UrlMode.Relative);
        }
    }
}
