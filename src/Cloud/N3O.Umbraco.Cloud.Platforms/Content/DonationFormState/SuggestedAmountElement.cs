using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.DonationFormStates.SuggestedAmount)]
public class DonationFormStateSuggestedAmountElement : UmbracoElement<DonationFormStateSuggestedAmountElement> {
    public decimal Amount => GetValue(x => x.Amount);
    public string Description => GetValue(x => x.Description);
}
