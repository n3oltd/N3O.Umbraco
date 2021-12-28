using N3O.Umbraco.Element;

namespace N3O.Umbraco.Giving.Donations.Models {
    public class PriceHandle : UmbracoElement {
        public decimal Amount => GetValue<PriceHandle, decimal>(x => x.Amount);
        public string Description => GetValue<PriceHandle, string>(x => x.Description);
    }
}
