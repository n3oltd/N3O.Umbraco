using N3O.Umbraco.Content;

namespace N3O.Umbraco.Giving.Allocations.Content {
    public abstract class FundDimension : UmbracoContent<FundDimension> {
        public string Name => Content.Name;
        public bool IsActive => GetValue(x => x.IsActive);
    }
}