﻿using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Elements.Extensions;

public static class ContentExtensions {
    public static bool IsDonationCategory(this IContent content, IContentTypeService contentTypeService) {
        return HasComposition(content,contentTypeService, ElementsConstants.DonationCategory.CompositionAlias);
    }
    
    public static bool IsDonationOption(this IContent content, IContentTypeService contentTypeService) {
        return HasComposition(content,contentTypeService, ElementsConstants.DonationOption.CompositionAlias);
    }
    
    private static bool HasComposition(this IContent content, IContentTypeService contentTypeService, string alias) {
        var contentType = contentTypeService.Get(content.ContentType.Id);
        
        return contentType.ContentTypeCompositionExists(alias);
    }
}