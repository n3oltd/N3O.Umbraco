using N3O.Umbraco.Content;

namespace N3O.Umbraco.Giving.Donations.Models {
    public class PriceHandle : UmbracoElement<PriceHandle> {
        public decimal Amount => GetValue(x => x.Amount);
        public string Description => GetValue(x => x.Description);
    }
}
