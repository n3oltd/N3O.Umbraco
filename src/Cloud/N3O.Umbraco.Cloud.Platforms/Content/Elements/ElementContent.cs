using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Community.Contentment.DataEditors;

namespace N3O.Umbraco.Cloud.Platforms.Content;

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
    
    public IEnumerable<DataListItem> AnalyticsTags => GetValue(x => x.AnalyticsTags);

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