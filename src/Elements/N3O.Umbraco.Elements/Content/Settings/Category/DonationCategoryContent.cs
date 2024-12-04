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

public class DonationCategoryContent : UmbracoContent<DonationCategoryContent> {
    private static readonly string DimensionDonationCategoryAlias = AliasHelper<DimensionDonationCategoryContent>.ContentTypeAlias();
    private static readonly string EphemeralDonationCategoryAlias = AliasHelper<EphemeralDonationCategoryContent>.ContentTypeAlias();
    private static readonly string GeneralDonationCategoryAlias = AliasHelper<GeneralDonationCategoryContent>.ContentTypeAlias();
    
    public string Name => Content().Name;
    public MediaWithCrops Image => GetValue(x => x.Image);
    
    public override void Content(IPublishedContent content) {
        base.Content(content);
        
        if (Type == DonationCategoryTypes.General) {
            General = new GeneralDonationCategoryContent();
            General.Content(content);
        } else if (Type == DonationCategoryTypes.Dimension) {
            Dimension = new DimensionDonationCategoryContent();
            Dimension.Content(content);
        } else if (Type == DonationCategoryTypes.Ephemeral) {
            Ephemeral = new EphemeralDonationCategoryContent();
            Ephemeral.Content(content);
        } else {
            throw UnrecognisedValueException.For(Type);
        }
    }
    
    public DimensionDonationCategoryContent Dimension { get; private set; }
    public GeneralDonationCategoryContent General { get; private set; }
    public EphemeralDonationCategoryContent Ephemeral { get; private set; }
    
    public DonationCategoryType Type {
        get {
            if (Content().ContentType.Alias.EqualsInvariant(GeneralDonationCategoryAlias)) {
                return DonationCategoryTypes.General;
            } else if (Content().ContentType.Alias.EqualsInvariant(DimensionDonationCategoryAlias)) {
                return DonationCategoryTypes.Dimension;
            } else if (Content().ContentType.Alias.EqualsInvariant(EphemeralDonationCategoryAlias)) {
                return DonationCategoryTypes.Ephemeral;
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

        foreach (var childCategory in Content().Children.As<DonationCategoryContent>()) {
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
            Type = Type.Id,
            Image = Image?.Url(),
            //EphemeralDonationCategory = Ephemeral.ToFormJson(),
            //DimensionDonationCategory = Dimension.ToFormJson(),
            Entries = entries
        };

        return jsonProvider.SerializeObject(category);
    }
}