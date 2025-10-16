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
    
    public override void SetContent(IPublishedContent content) {
        base.SetContent(content);
        
        if (Type == ElementTypes.DonateButton) {
            DonateButton = new DonateButtonElementContent();
            DonateButton.SetContent(content);
        } else if (Type == ElementTypes.DonationForm) {
            DonationForm = new DonationFormElementContent();
            DonationForm.SetContent(content);
        } else {
            throw UnrecognisedValueException.For(Type);
        }
    }
    
    public override void SetVariationContext(VariationContext variationContext) {
        base.SetVariationContext(variationContext);
        
        DonateButton?.SetVariationContext(variationContext);
        DonationForm?.SetVariationContext(variationContext);
    }
    
    public Guid Key => Content().Key;
    public string EmbedCode => GetValue(x => x.EmbedCode);
    public bool IsSystemGenerated => GetValue(x => x.IsSystemGenerated);
    public IReadOnlyDictionary<string, string> Tags => GetConvertedValue<IEnumerable<DataListItem>, IReadOnlyDictionary<string, string>>(x => x.Tags, x => x.ToTagsDictionary());

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