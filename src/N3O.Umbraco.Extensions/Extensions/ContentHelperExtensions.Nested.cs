using N3O.Umbraco.Content;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Extensions;

public static partial class ContentHelperExtensions {
    public static IPublishedElement GetNestedContent(this IContentHelper contentHelper, ElementsProperty property) {
        throw new NotSupportedException("Nested Content has been removed in Umbraco 14. Use Block List instead.");
    }

    public static IPublishedElement GetNestedContent(this IContentHelper contentHelper,
                                                     string contentTypeAlias,
                                                     IProperty property) {
        throw new NotSupportedException("Nested Content has been removed in Umbraco 14. Use Block List instead.");
    }

    public static IPublishedElement GetNestedContent(this IContentHelper contentHelper,
                                                     string contentTypeAlias,
                                                     string propertyTypeAlias,
                                                     object propertyValue) {
        throw new NotSupportedException("Nested Content has been removed in Umbraco 14. Use Block List instead.");
    }

    public static IReadOnlyList<IPublishedElement> GetNestedContents(this IContentHelper contentHelper,
                                                                     ElementsProperty property) {
        throw new NotSupportedException("Nested Content has been removed in Umbraco 14. Use Block List instead.");
    }

    public static IReadOnlyList<IPublishedElement> GetNestedContents(this IContentHelper contentHelper,
                                                                     string contentTypeAlias,
                                                                     IProperty property) {
        throw new NotSupportedException("Nested Content has been removed in Umbraco 14. Use Block List instead.");
    }

    public static IReadOnlyList<IPublishedElement> GetNestedContents(this IContentHelper contentHelper,
                                                                     string contentTypeAlias,
                                                                     string propertyTypeAlias,
                                                                     object propertyValue) {
        throw new NotSupportedException("Nested Content has been removed in Umbraco 14. Use Block List instead.");
    }
}
