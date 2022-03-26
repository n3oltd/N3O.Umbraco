using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Content {
    public class DonationItemsContent : UmbracoContent<DonationItemsContent> {
        public IEnumerable<DonationItem> GetDonationItems() => Content().Children.As<DonationItem>();
    }
}