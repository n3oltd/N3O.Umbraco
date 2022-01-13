using N3O.Umbraco.Content;
using Perplex.ContentBlocks.Rendering;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.Extensions {
    public static partial class ContentHelperExtensions {
        public static ContentBlocks GetContentBlocks(this IContentHelper contentHelper,
                                                     string contentTypeAlias,
                                                     IProperty property) {
            if (!property.PropertyType.IsContentBlocks()) {
                throw new Exception("Property is not content blocks");
            }
            
            return contentHelper.GetContentBlocks(contentTypeAlias, property.PropertyType.Alias, property.GetValue());
        }

        public static TProperty GetConvertedValue<TConverter, TProperty>(this IContentHelper contentHelper,
                                                                         string contentTypeAlias,
                                                                         IProperty property)
            where TConverter : class, IPropertyValueConverter {
            return contentHelper.GetConvertedValue<TConverter, TProperty>(contentTypeAlias,
                                                                          property.PropertyType.Alias,
                                                                          property.GetValue());
        }

        public static IPublishedElement GetNestedContent(this IContentHelper contentHelper,
                                                         string contentTypeAlias,
                                                         IProperty property) {
            if (!property.PropertyType.IsNestedContent()) {
                throw new Exception("Property is not nested content");
            }
            
            return contentHelper.GetNestedContent(contentTypeAlias, property.PropertyType.Alias, property.GetValue());
        }

        public static IReadOnlyList<IPublishedElement> GetNestedContents(this IContentHelper contentHelper,
                                                                         string contentTypeAlias,
                                                                         IProperty property) {
            if (!property.PropertyType.IsNestedContent()) {
                throw new Exception("Property is not nested content");
            }
            
            return contentHelper.GetNestedContents(contentTypeAlias, property.PropertyType.Alias, property.GetValue());
        }

        public static T GetPickerValue<T>(this IContentHelper contentHelper,
                                          string contentTypeAlias,
                                          IProperty property) {
            if (!property.PropertyType.IsPicker()) {
                throw new Exception("Property is not picker");
            }
            
            return contentHelper.GetPickerValue<T>(contentTypeAlias, property.PropertyType.Alias, property.GetValue());
        }

        public static IReadOnlyList<T> GetPickerValues<T>(this IContentHelper contentHelper,
                                                          string contentTypeAlias,
                                                          IProperty property) {
            if (!property.PropertyType.IsPicker()) {
                throw new Exception("Property is not picker");
            }
            
            return contentHelper.GetPickerValues<T>(contentTypeAlias, property.PropertyType.Alias, property.GetValue());
        }
    }
}