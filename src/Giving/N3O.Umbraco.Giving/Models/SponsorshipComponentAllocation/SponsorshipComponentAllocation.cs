using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Lookups;
using Newtonsoft.Json;

namespace N3O.Umbraco.Giving.Models {
    public class SponsorshipComponentAllocation : Value, ISponsorshipComponentAllocation {
        [JsonConstructor]
        public SponsorshipComponentAllocation(SponsorshipComponent component, Money value) {
            Component = component;
            Value = value;
        }

        public SponsorshipComponentAllocation(ISponsorshipComponentAllocation componentAllocation)
            : this(componentAllocation.Component, componentAllocation.Value) { }

        public SponsorshipComponent Component { get; }
        public Money Value { get; }
    }
}