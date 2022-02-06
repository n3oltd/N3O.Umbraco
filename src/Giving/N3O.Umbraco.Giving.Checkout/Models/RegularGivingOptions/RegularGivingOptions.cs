using N3O.Umbraco.Giving.Checkout.Lookups;
using N3O.Umbraco.Lookups;
using Newtonsoft.Json;
using NodaTime;

namespace N3O.Umbraco.Giving.Checkout.Models {
    public class RegularGivingOptions : Value, IRegularGivingOptions {
        [JsonConstructor]
        public RegularGivingOptions(DayOfMonth collectionDay,
                                    RegularGivingFrequency frequency,
                                    LocalDate? firstCollectionDate) {
            CollectionDay = collectionDay;
            Frequency = frequency;
            FirstCollectionDate = firstCollectionDate;
        }

        public RegularGivingOptions(IRegularGivingOptions givingOptions)
            : this(givingOptions.CollectionDay, givingOptions.Frequency, givingOptions.FirstCollectionDate) { }

        public DayOfMonth CollectionDay { get; }
        public RegularGivingFrequency Frequency { get; }
        public LocalDate? FirstCollectionDate { get; }
    }
}