using N3O.Umbraco.Giving.Checkout.Lookups;
using N3O.Umbraco.Lookups;
using Newtonsoft.Json;
using NodaTime;

namespace N3O.Umbraco.Giving.Checkout.Models;

public class RegularGivingOptions : Value, IRegularGivingOptions {
    [JsonConstructor]
    public RegularGivingOptions(DayOfMonth preferredCollectionDay,
                                RegularGivingFrequency frequency,
                                LocalDate? firstCollectionDate) {
        PreferredCollectionDay = preferredCollectionDay;
        Frequency = frequency;
        FirstCollectionDate = firstCollectionDate;
    }

    public RegularGivingOptions(IRegularGivingOptions givingOptions)
        : this(givingOptions.PreferredCollectionDay, givingOptions.Frequency, givingOptions.FirstCollectionDate) { }

    public DayOfMonth PreferredCollectionDay { get; }
    public RegularGivingFrequency Frequency { get; }
    public LocalDate? FirstCollectionDate { get; }
}
