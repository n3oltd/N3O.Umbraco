using N3O.Umbraco.Content;

namespace N3O.Umbraco.Giving.Allocations.Content {
    public abstract class FundDimensionContent : UmbracoContent<FundDimensionContent> {
        public string Name => Content.Name;
        public bool IsActive => GetValue(x => x.IsActive);
    }
}