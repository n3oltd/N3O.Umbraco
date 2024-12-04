using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;

namespace N3O.Umbraco.Giving.Allocations.Content;

public class DonationItemsContent : UmbracoContent<DonationItemsContent> {
    public IEnumerable<DonationItem> GetDonationItems() => Content().Children.As<DonationItem>();
}
