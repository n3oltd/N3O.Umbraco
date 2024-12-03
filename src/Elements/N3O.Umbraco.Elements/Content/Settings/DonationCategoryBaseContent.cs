using N3O.Umbraco.Content;
using N3O.Umbraco.Elements.Clients;
using N3O.Umbraco.Elements.Extensions;
using N3O.Umbraco.Elements.Lookups;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace N3O.Umbraco.Elements.Content;

public class DonationCategoryBaseContent : UmbracoContent<DonationCategoryBaseContent> {
    private static readonly string DonationCategoryAlias = AliasHelper<DonationCategoryContent>.ContentTypeAlias();
    private static readonly string DimensionDonationCategoryAlias = AliasHelper<DimensionDonationCategoryContent>.ContentTypeAlias();
    private static readonly string EphemeralDonationCategoryAlias = AliasHelper<EphemeralDonationCategoryContent>.ContentTypeAlias();
    
    public MediaWithCrops Image => GetValue(x => x.Image);
    
    public override void Content(IPublishedContent content) {
        base.Content(content);
        
        if (Type == DonationCategoryTypes.DimensionCategory) {
            DimensionDonationCategory = new DimensionDonationCategoryContent();
            DimensionDonationCategory.Content(content);
        } else if (Type == DonationCategoryTypes.DefaultCategory) {
            DonationCategory = new DonationCategoryContent();
            DonationCategory.Content(content);
        } else if (Type == DonationCategoryTypes.EphemeralCategory) {
            EphemeralDonationCategory = new EphemeralDonationCategoryContent();
            EphemeralDonationCategory.Content(content);
        } else {
            throw UnrecognisedValueException.For(Type);
        }
    }
    
    public DimensionDonationCategoryContent DimensionDonationCategory { get; private set; }
    public DonationCategoryContent DonationCategory { get; private set; }
    public EphemeralDonationCategoryContent EphemeralDonationCategory { get; private set; }
    
    public DonationCategoryType Type {
        get {
            if (Content().ContentType.Alias.EqualsInvariant(DonationCategoryAlias)) {
                return DonationCategoryTypes.DefaultCategory;
            } else if (Content().ContentType.Alias.EqualsInvariant(DimensionDonationCategoryAlias)) {
                return DonationCategoryTypes.DimensionCategory;
            } else if (Content().ContentType.Alias.EqualsInvariant(EphemeralDonationCategoryAlias)) {
                return DonationCategoryTypes.EphemeralCategory;
            } else {
                throw UnrecognisedValueException.For(Content().ContentType.Alias);
            }
        }
    }
    
    public object ToFormJson(IJsonProvider jsonProvider, IReadOnlyList<DonationOptionContent> options) {
        var entries = new List<object>();
        
        foreach (var option in options) {
            var entry = new {
                option.Name,
                option.DefaultOption,
                Type = PartialType.DonationFormOption,
                Image = option.Image.Url(),
                OptionId = option.Content().Key
            };
            
            entries.Add(entry);
        }

        foreach (var childCategory in Content().Children.As<DonationCategoryBaseContent>()) {
            var entry = new {
                childCategory.Content().Name,
                //Type = PartialType.category,
                Image = childCategory.Image.Url(), 
                CategoryId = childCategory.Content().Key
            };
            
            entries.Add(entry);
        }
        
        var category = new {
            ParentId = Content().Parent.IsDonationCategory() ? Content().Parent?.Key : Guid.Empty,
            Id = Content().Key,
            Type = Type,
            Image = Image?.Url(),
            EphemeralDonationCategory = EphemeralDonationCategory,
            DimensionDonationCategory = DimensionDonationCategory,
            Entries = entries
        };

        return jsonProvider.SerializeObject(category);
    }
}