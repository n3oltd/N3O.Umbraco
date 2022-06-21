using N3O.Umbraco.Attributes;
using N3O.Umbraco.Giving.Checkout.Lookups;
using N3O.Umbraco.Lookups;
using NodaTime;

namespace N3O.Umbraco.Giving.Checkout.Models;

public class RegularGivingOptionsReq : IRegularGivingOptions {
    [Name("Preferred Collection Day")]
    public DayOfMonth PreferredCollectionDay { get; set; }

    [Name("Frequency")]
    public RegularGivingFrequency Frequency { get; set; }

    [Name("First Collection Date")]
    public LocalDate? FirstCollectionDate { get; set; }
}
