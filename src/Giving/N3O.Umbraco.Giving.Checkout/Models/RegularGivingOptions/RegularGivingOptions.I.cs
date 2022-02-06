using N3O.Umbraco.Giving.Checkout.Lookups;
using N3O.Umbraco.Lookups;
using NodaTime;

namespace N3O.Umbraco.Giving.Checkout.Models {
    public interface IRegularGivingOptions {
        DayOfMonth CollectionDay { get; }
        RegularGivingFrequency Frequency { get; }
        LocalDate? FirstCollectionDate { get; }
    }
}