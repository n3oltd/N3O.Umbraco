using N3O.Umbraco.Giving.Checkout.Lookups;
using N3O.Umbraco.Lookups;
using NodaTime;

namespace N3O.Umbraco.Giving.Checkout.Models;

public class RegularGivingOptionsRes : IRegularGivingOptions {
    public DayOfMonth PreferredCollectionDay { get; set; }
    public RegularGivingFrequency Frequency { get; set; }
    public LocalDate? FirstCollectionDate { get; set; }
}
