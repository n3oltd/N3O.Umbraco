using N3O.Umbraco.Content;

namespace N3O.Umbraco.Giving.Allocations.Content; 

public abstract class FundDimension : UmbracoContent {
    public string Name => Content.Name;
    public bool IsActive => GetValue<FundDimension, bool>(x => x.IsActive);
}