using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Community.Contentment.DataEditors;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Elements.CompositionAlias)]
public class ElementContent : UmbracoContent<ElementContent> {
    private static readonly string DonateButtonElementAlias = AliasHelper<DonateButtonElementContent>.ContentTypeAlias();
    private static readonly string DonationFormElementAlias = AliasHelper<DonationFormElementContent>.ContentTypeAlias();
    
    public override void Content(IPublishedContent content) {
        base.Content(content);
        
        if (Type == ElementTypes.DonateButton) {
            DonateButton = new DonateButtonElementContent();
            DonateButton.Content(content);
        } else if (Type == ElementTypes.DonationForm) {
            DonationForm = new DonationFormElementContent();
            DonationForm.Content(content);
        } else {
            throw UnrecognisedValueException.For(Type);
        }
    }
    
    public Guid Key => Content().Key;
    public string Label => GetValue(x => x.Label);
    public CampaignContent Campaign => GetAs(x => x.Campaign);
    public IReadOnlyDictionary<string, string> AnalyticsTags => GetConvertedValue<IEnumerable<DataListItem>, IReadOnlyDictionary<String, string>>(x => x.AnalyticsTags, x => x.ToTags());

    public DonateButtonElementContent DonateButton { get; private set; }
    public DonationFormElementContent DonationForm { get; private set; }
    
    public ElementType Type {
        get {
            if (Content().ContentType.Alias.EqualsInvariant(DonateButtonElementAlias)) {
                return ElementTypes.DonateButton;
            } else if (Content().ContentType.Alias.EqualsInvariant(DonationFormElementAlias)) {
                return ElementTypes.DonationForm;
            } else {
                throw UnrecognisedValueException.For(Content().ContentType.Alias);
            }
        }
    }
}