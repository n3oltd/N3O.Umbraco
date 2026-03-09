using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Community.Contentment.DataEditors;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Elements.CompositionAlias)]
public class ElementContent : UmbracoContent<ElementContent>, IHoldCustomFormState {
    private static readonly string DonationButtonElementAlias = AliasHelper<DonationButtonElementContent>.ContentTypeAlias();
    private static readonly string DonationFormElementAlias = AliasHelper<DonationFormElementContent>.ContentTypeAlias();
    private static readonly string DonationPopupElementAlias = AliasHelper<DonationPopupElementContent>.ContentTypeAlias();
    
    public override void SetContent(IPublishedContent content) {
        base.SetContent(content);
        
        if (Type == ElementTypes.DonationButton) {
            DonationButton = new DonationButtonElementContent();
            DonationButton.SetContent(content);
        } else if (Type == ElementTypes.DonationForm) {
            DonationForm = new DonationFormElementContent();
            DonationForm.SetContent(content);
        } else if (Type == ElementTypes.DonationPopup) {
            DonationPopup = new DonationPopupElementContent();
            DonationPopup.SetContent(content);
        } else {
            throw UnrecognisedValueException.For(Type);
        }
    }
    
    public override void SetVariationContext(VariationContext variationContext) {
        base.SetVariationContext(variationContext);
        
        DonationButton?.SetVariationContext(variationContext);
        DonationForm?.SetVariationContext(variationContext);
        DonationPopup?.SetVariationContext(variationContext);
    }
    
    public Guid Key => Content().Key;
    public string Name => Content().Name;
    public CampaignContent Campaign => GetAs(x => x.Campaign);
    public string EmbedCode => GetValue(x => x.EmbedCode);
    public bool IsSystemGenerated => GetValue(x => x.IsSystemGenerated);
    public string CustomFormState => GetValue(x => x.CustomFormState);
    public IReadOnlyDictionary<string, string> Tags => GetConvertedValue<IEnumerable<DataListItem>, IReadOnlyDictionary<string, string>>(x => x.Tags, x => x.ToTagsDictionary());

    public DonationButtonElementContent DonationButton { get; private set; }
    public DonationFormElementContent DonationForm { get; private set; }
    public DonationPopupElementContent DonationPopup { get; private set; }
    
    public ElementType Type {
        get {
            if (Content().ContentType.Alias.EqualsInvariant(DonationButtonElementAlias)) {
                return ElementTypes.DonationButton;
            } else if (Content().ContentType.Alias.EqualsInvariant(DonationFormElementAlias)) {
                return ElementTypes.DonationForm;
            } else if (Content().ContentType.Alias.EqualsInvariant(DonationPopupElementAlias)) {
                return ElementTypes.DonationPopup;
            } else {
                throw UnrecognisedValueException.For(Content().ContentType.Alias);
            }
        }
    }
    
    public IFundDimensionValues GetFixedFundDimensionValues(OfferingContent offering) {
        IFundDimensionValues fundDimensionValues;

        if (Type == ElementTypes.DonationButton) {
            fundDimensionValues = DonationButton.GetFixedFundDimensionValues(offering);
        } else if (Type == ElementTypes.DonationForm) {
            fundDimensionValues = DonationForm.GetFixedFundDimensionValues(offering);
        } else if (Type == ElementTypes.DonationPopup) {
            fundDimensionValues = DonationPopup.GetFixedFundDimensionValues(offering);
        } else {
            throw UnrecognisedValueException.For(Type);
        }

        return fundDimensionValues;
    }
}