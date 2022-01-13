using N3O.Umbraco.Content;
using Perplex.ContentBlocks.Rendering;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.Extensions {
    public static partial class ContentHelperExtensions {
        public static ContentBlocks GetContentBlocks(this IContentHelper contentHelper, NestedContentProperty property) {
            if (!property.Type.IsContentBlocks()) {
                throw new Exception("Property is not content blocks");
            }
            
            return contentHelper.GetContentBlocks(property.ContentType.Alias, property.Type.Alias, property.Json);
        }

        public static TProperty GetConvertedValue<TConverter, TProperty>(this IContentHelper contentHelper,
                                                                         IContentProperty property)
            where TConverter : class, IPropertyValueConverter {
            return contentHelper.GetConvertedValue<TConverter, TProperty>(property.ContentType.Alias,
                                                                          property.Type.Alias,
                                                                          property.Value);
        }

        public static IPublishedElement GetNestedContent(this IContentHelper contentHelper, NestedContentProperty property) {
            if (!property.Type.IsNestedContent()) {
                throw new Exception("Property is not nested content");
            }
            
            return contentHelper.GetNestedContent(property.ContentType.Alias, property.Type.Alias, property.Json);
        }

        public static IReadOnlyList<IPublishedElement> GetNestedContents(this IContentHelper contentHelper,
                                                                         NestedContentProperty property) {
            if (!property.Type.IsNestedContent()) {
                throw new Exception("Property is not nested content");
            }
            
            return contentHelper.GetNestedContents(property.ContentType.Alias, property.Type.Alias, property.Json);
        }

        public static T GetPickerValue<T>(this IContentHelper contentHelper, IContentProperty property) {
            if (!property.Type.IsPicker()) {
                throw new Exception("Property is not picker");
            }
            
            return contentHelper.GetPickerValue<T>(property.ContentType.Alias, property.Type.Alias, property.Value);
        }

        public static IReadOnlyList<T> GetPickerValues<T>(this IContentHelper contentHelper, IContentProperty property) {
            if (!property.Type.IsPicker()) {
                throw new Exception("Property is not picker");
            }
            
            return contentHelper.GetPickerValues<T>(property.ContentType.Alias, property.Type.Alias, property.Value);
        }
    }
}