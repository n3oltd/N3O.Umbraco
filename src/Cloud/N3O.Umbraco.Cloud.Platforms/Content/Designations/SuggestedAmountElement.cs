using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Designations.SuggestedAmount)]
public class SuggestedAmountElement : UmbracoElement<SuggestedAmountElement> {
    public decimal Amount => GetValue(x => x.Amount);
    public string Description => GetValue(x => x.Description);
}