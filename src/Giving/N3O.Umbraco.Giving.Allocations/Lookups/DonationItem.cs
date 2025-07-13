using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations.Lookups;

public class DonationItem : NamedLookup, IHoldAllowedGivingTypes, IHoldFundDimensionOptions, IHoldPricing {
    public DonationItem(string id,
                        string name,
                        IEnumerable<GivingType> allowedGivingTypes,
                        FundDimensionOptions fundDimensionOptions,
                        Pricing pricing)
        : base(id, name) {
        AllowedGivingTypes = allowedGivingTypes;
        FundDimensionOptions = FundDimensionOptions;
        Pricing = pricing;
    }

    public IEnumerable<GivingType> AllowedGivingTypes { get; }
    public FundDimensionOptions FundDimensionOptions { get; }
    public Pricing Pricing { get; }

    [JsonIgnore]
    IFundDimensionOptions IHoldFundDimensionOptions.FundDimensionOptions => FundDimensionOptions;
    
    [JsonIgnore]
    IPricing IHoldPricing.Pricing => Pricing;
}