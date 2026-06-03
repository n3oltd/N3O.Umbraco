using N3O.Umbraco.Content;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Extensions;

// TODO Migration Review: Nested Content was removed in Umbraco 14. These helpers are retained only so external consumers
// fail at COMPILE time (error) with a message directing them to the Block List equivalents
// (GetBlockList) in ContentHelperExtensions.BlockList.cs, rather than failing at runtime.
public static partial class ContentHelperExtensions {
    private const string NestedObsolete =
        "Nested Content was removed in Umbraco 14. Use GetBlockList(...) instead.";

    [Obsolete(NestedObsolete, error: true)]
    public static IPublishedElement GetNestedContent(this IContentHelper contentHelper, ElementsProperty property) {
        throw new NotSupportedException(NestedObsolete);
    }

    [Obsolete(NestedObsolete, error: true)]
    public static IPublishedElement GetNestedContent(this IContentHelper contentHelper,
                                                     string contentTypeAlias,
                                                     IProperty property) {
        throw new NotSupportedException(NestedObsolete);
    }

    [Obsolete(NestedObsolete, error: true)]
    public static IPublishedElement GetNestedContent(this IContentHelper contentHelper,
                                                     string contentTypeAlias,
                                                     string propertyTypeAlias,
                                                     object propertyValue) {
        throw new NotSupportedException(NestedObsolete);
    }

    [Obsolete(NestedObsolete, error: true)]
    public static IReadOnlyList<IPublishedElement> GetNestedContents(this IContentHelper contentHelper,
                                                                     ElementsProperty property) {
        throw new NotSupportedException(NestedObsolete);
    }

    [Obsolete(NestedObsolete, error: true)]
    public static IReadOnlyList<IPublishedElement> GetNestedContents(this IContentHelper contentHelper,
                                                                     string contentTypeAlias,
                                                                     IProperty property) {
        throw new NotSupportedException(NestedObsolete);
    }

    [Obsolete(NestedObsolete, error: true)]
    public static IReadOnlyList<IPublishedElement> GetNestedContents(this IContentHelper contentHelper,
                                                                     string contentTypeAlias,
                                                                     string propertyTypeAlias,
                                                                     object propertyValue) {
        throw new NotSupportedException(NestedObsolete);
    }
}
