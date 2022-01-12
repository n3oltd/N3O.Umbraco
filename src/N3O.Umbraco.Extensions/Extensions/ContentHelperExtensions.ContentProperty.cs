using N3O.Umbraco.Content;
using Perplex.ContentBlocks.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.Extensions {
    public static partial class ContentHelperExtensions {
        public static ContentBlocks GetContentBlocks(this IContentHelper contentHelper, ContentProperty property) {
            if (!property.Type.IsContentBlocks()) {
                throw new Exception("Property is not content blocks");
            }
            
            return contentHelper.GetContentBlocks(property.ContentType.Alias, property.Type.Alias, property.Value);
        }

        public static TProperty GetConvertedValue<TConverter, TProperty>(this IContentHelper contentHelper,
                                                                         ContentProperty property)
            where TConverter : class, IPropertyValueConverter {
            return contentHelper.GetConvertedValue<TConverter, TProperty>(property.ContentType.Alias,
                                                                          property.Type.Alias,
                                                                          property.Value);
        }

        public static IPublishedElement GetNestedContent(this IContentHelper contentHelper, ContentProperty property) {
            return GetNestedContents(contentHelper, property).Single();
        }

        public static IReadOnlyList<IPublishedElement> GetNestedContents(this IContentHelper contentHelper,
                                                                         ContentProperty property) {
            if (!property.Type.IsContentBlocks()) {
                throw new Exception("Property is not nested content");
            }
            
            return contentHelper.GetNestedContents(property.ContentType.Alias, property.Type.Alias, property.Value);
        }

        public static T GetPickerValue<T>(this IContentHelper contentHelper, ContentProperty property) {
            return GetPickerValues<T>(contentHelper, property).Single();
        }

        public static IReadOnlyList<T> GetPickerValues<T>(this IContentHelper contentHelper, ContentProperty property) {
            if (!property.Type.IsContentBlocks()) {
                throw new Exception("Property is not picker");
            }
            
            return contentHelper.GetPickerValues<T>(property.ContentType.Alias, property.Type.Alias, property.Value);
        }
    }
}