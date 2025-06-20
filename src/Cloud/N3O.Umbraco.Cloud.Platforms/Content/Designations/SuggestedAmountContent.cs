using N3O.Umbraco.Content;

namespace N3O.Umbraco.Cloud.Platforms.Content;

public class SuggestedAmountContent : UmbracoContent<SuggestedAmountContent> {
    public decimal Amount => GetValue(x => x.Amount);
    public string Description => GetValue(x => x.Description);
}