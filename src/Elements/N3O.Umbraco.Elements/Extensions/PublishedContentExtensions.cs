﻿using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Elements.Extensions;

public static class PublishedContentExtensions {
    public static bool IsDonationCategory(this IPublishedContent content) {
        if (content.ContentType.CompositionAliases.Contains(ElementsConstants.DonationCategory.CompositionAlias)) {
            return true;
        }
        
        return false;
    }
    
    public static bool IsDonationOption(this IPublishedContent content) {
        return content.ContentType.CompositionAliases.Contains(ElementsConstants.DonationOption.CompositionAlias);
    }
}