using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations.Content;

public class DonationItemsContent : UmbracoContent<DonationItemsContent> {
    public IEnumerable<DonationItemContent> GetDonationItems() => Content().Children.As<DonationItemContent>();
}
